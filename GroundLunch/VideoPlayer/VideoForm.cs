using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using DevExpress.XtraTreeList.ViewInfo;
using System.Collections.Concurrent;

namespace GroundLunch
{
    public partial class VideoForm : DevExpress.XtraEditors.XtraForm
    {
        public List<TMParam> allParamList = new List<TMParam>();
        private System.Windows.Forms.Timer RefreshUITimer = new System.Windows.Forms.Timer();
        public int paoID;
        public int guanID;
        int startFly = 0;
        int bjTimeSyncDiv = 0;

        //public double curTime = 0;

        public VideoForm()
        {
            InitializeComponent();
            videoControl1.OnDataProcessed += VideoControl1_OnDataProcessed;
        }

        private void VideoControl1_OnDataProcessed(float value1, float value2)
        {
            textPointX.Text = value1.ToString();
            textPointY.Text = value2.ToString();
            //throw new NotImplementedException();
        }

        private UInt16 ParseU16(string text, UInt16 fallback)
        {
            UInt16 v;
            return UInt16.TryParse(text, out v) ? v : fallback;
        }

        internal void SendLockTarget(UInt16 x, UInt16 y)
        {
            UInt16 w = ParseU16(textGateWidth.Text, 64);
            UInt16 h = ParseU16(textGateHeight.Text, 64);
            UInt32 frameNo = 0;
            byte[] sendData = new byte[12];
            Buffer.BlockCopy(BitConverter.GetBytes(x), 0, sendData, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(y), 0, sendData, 2, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(w), 0, sendData, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(h), 0, sendData, 6, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(frameNo), 0, sendData, 8, 4);
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan,
                12, 0xD3, sendData);
        }

        private void btLockTarget_Click(object sender, EventArgs e)
        {
            SendLockTarget(ParseU16(textPointX.Text, 0), ParseU16(textPointY.Text, 0));
        }

        private void btCancelTrack_Click(object sender, EventArgs e)
        {
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan,
                0, 0xD5, new byte[0]);
        }

        private void VideoForm_Shown(object sender, EventArgs e)
        {
        }

        private void VideoForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshUITimer.Interval = 50;
                RefreshUITimer.Tick -= OnTimerFresh;
                RefreshUITimer.Tick += OnTimerFresh;
                RefreshUITimer.Start();
            }
            else
            {
                RefreshUITimer.Stop();
            }
        }
        private void VideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;        // 阻止关闭
            this.Hide();            // 隐藏而不是释放
            RefreshUITimer.Stop();
        }

        private void VideoForm_Load(object sender, EventArgs e)
        {
            OnSelectPlaneChanged();
            paoID = NetDataHandle.curPao;
            guanID = NetDataHandle.curGuan;
        }

        public void OnSelectPlaneChanged()
        {
            allParamList = TMHandler.allParamList[(NetDataHandle.curPao, NetDataHandle.curGuan)];
            gridTM.DataSource = null;
            gridTM.DataSource = allParamList;
            viewTM.Columns[0].Width = 5 * gridTM.Width / 100;

            viewTM.Columns[1].Width = 22 * gridTM.Width / 100;
            viewTM.Columns[2].Width = 10 * gridTM.Width / 100;
            viewTM.Columns[3].Width = 5 * gridTM.Width / 100;
            viewTM.Columns[4].Width = 7 * gridTM.Width / 100;
            viewTM.Columns[5].Width = 9 * gridTM.Width / 100;
            viewTM.Columns[6].Width = 10 * gridTM.Width / 100;
            viewTM.Columns[7].Width = 7 * gridTM.Width / 100;
            viewTM.Columns[8].Width = 13 * gridTM.Width / 100;
            viewTM.Columns[9].Width = 10 * gridTM.Width / 100;
            paoID = NetDataHandle.curPao;
            guanID = NetDataHandle.curGuan;
            for (int i = 1; i < 10; i++)
            {
                viewTM.Columns[i].OptionsColumn.AllowFocus = false;
                viewTM.Columns[i].OptionsColumn.AllowEdit = false;
            }

            btDoUmb.Text = "紧急伞降(0)";
            btHome.Text = "紧急返航";
        }

        private void OnTimerFresh(object sender, EventArgs e)
        {
            if (paoID != NetDataHandle.curPao || guanID != NetDataHandle.curGuan) 
            {
                OnSelectPlaneChanged();
            }
            viewTM.RefreshData();
            RefreshUI();
            // PacketToDataTable(NetDataHandle.curSelPlaneTMFrame.flightTime);
            while (TMHandler.uiPendingRows.TryDequeue(out var itemArray))
            {
                DataRow dr = TMHandler.FlightDataTableForView[(paoID, guanID)].NewRow();
                dr.ItemArray = itemArray;
                TMHandler.FlightDataTableForView[(paoID, guanID)].Rows.Add(dr);
            }
            //将数据同步到wpf
            int year=0, month=0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
            // 每帧清零，避免链路中断后 HDU 一直显示旧空速/高度
            DataInterface.viewPlane.speed = 0;
            DataInterface.viewPlane.alt = 0;
            DataInterface.viewPlane.pitch = 0;
            DataInterface.viewPlane.heading = 0;
            DataInterface.viewPlane.roll = 0;
            double navHigh = 0;
            bool gotCurAlt = false, gotNavHigh = false;

            foreach (TMParam param in allParamList)
            {
                if (param.paramID == "msnGrpID")
                {
                    DataInterface.viewPlane.groupID = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "msnDevID")
                {
                    DataInterface.viewPlane.msnID = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "msnLead")
                {
                    DataInterface.viewPlane.leadID = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "ecuGetRp")
                {
                    DataInterface.viewPlane.rpm = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "AirSpd")
                {
                    DataInterface.viewPlane.speed = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "sctPitch")
                {
                    //DataInterface.viewPlane.scoutPitch = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "sctYaw")
                {
                   // DataInterface.viewPlane.scoutHeading = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "navPitch" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    DataInterface.viewPlane.pitch = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "navDir" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    DataInterface.viewPlane.heading = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "navRoll" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    DataInterface.viewPlane.roll = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "navLon" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    DataInterface.viewPlane.lon = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "navLat" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    DataInterface.viewPlane.lat = Convert.ToDouble(param.物理量);
                }
                if (param.paramID == "curAlt")
                {
                    DataInterface.viewPlane.alt = Convert.ToDouble(param.物理量);
                    gotCurAlt = true;
                }
                if (param.paramID == "navHigh" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    navHigh = Convert.ToDouble(param.物理量);
                    gotNavHigh = true;
                }
                // GPS 时间在遥测表中为 nav 池（不是 imu）
                if (param.paramID == "gpsYear" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    year = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "gpsMonth" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    month = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "gpsDay" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    day = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "gpsHour" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    hour = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "gpsMinit" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    minute = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "gpsSec" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    second = Convert.ToInt32(param.物理量);
                }
                if ((param.paramID == "gpsMSec" || param.paramID == "gpsMs")
                    && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                {
                    ms = Convert.ToInt32(param.物理量);
                }
                if (param.paramID == "startFly")
                {
                    int startFly1 = Convert.ToInt32(param.物理量);
                    if (startFly1 != startFly)
                    {
                        TMHandler.ClearFlightDT(NetDataHandle.curPao, NetDataHandle.curGuan);
                    }
                    startFly = startFly1;
                }
                if (param.paramID == "luncTime")
                {
                    // 飞控：相对 2000-01-01 的秒数；仅点火(DoIgnition)时写入，未授时则为 0
                    long secondsSince2000 = (long)param.paramSourceCode;
                    if (secondsSince2000 > 0)
                    {
                        DateTime baseDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
                        DateTime targetDate = baseDate.AddSeconds(secondsSince2000);
                        labelLuanchTime.Text = "发射零时 " + targetDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        labelLuanchTime.Text = "发射零时 未记录";
                    }
                }
            }

            // 起飞前周期性对时（半实物无 GPS）；已起飞则不再发，避免无意义
            if (startFly == 0 && (++bjTimeSyncDiv % 40) == 0) // ~2s @50ms
            {
                try { NetDataHandle.SendBjTimeSet(paoID, guanID); } catch { }
            }
            // 仅当控制包无 curAlt 时才用导航高度；curAlt=0 时不再用旧 navHigh 顶住
            if (!gotCurAlt && gotNavHigh)
                DataInterface.viewPlane.alt = navHigh;

            // gpsYear：飞控 IMU 路径存 year-2000；表头单位写的是 1970+；半实物常不填 GPS 时间
            int fullYear;
            if (year <= 0)
                fullYear = 0;
            else if (year < 100)
                fullYear = 2000 + year;
            else if (year < 1900)
                fullYear = 1970 + year;
            else
                fullYear = year;

            if (fullYear >= 2000 && month >= 1 && month <= 12 && day >= 1 && day <= 31)
            {
                labelBJTime.Text = string.Format("北京时间 {0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}",
                    fullYear, month, day, hour, minute, second);
            }
            else
            {
                // GPS 未授时（仿真/HIL 常见）：用本机时间，避免一直停在 2000/2048-00-00
                labelBJTime.Text = "本机时间 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            tmhdu1.SetLocation(DataInterface.viewPlane.pitch,
                DataInterface.viewPlane.heading,
                DataInterface.viewPlane.roll,
                DataInterface.viewPlane.alt,
                DataInterface.viewPlane.speed);
            DataInterface.isUpdate = 1;

            int dbm = -128;
            if (NetDataHandle.planeConnectDbm.ContainsKey((paoID, guanID)))
                dbm = NetDataHandle.planeConnectDbm[(paoID, guanID)];

            int tmOnline = 0;
            if (NetDataHandle.planeConnectStatus.ContainsKey((paoID, guanID)))
                tmOnline = NetDataHandle.planeConnectStatus[(paoID, guanID)];

            if (dbm == -128)
            {
                // 无数传 RSSI：若 UDP 遥测在刷新，仍提示在线，避免一直 ---dbm
                if (tmOnline > 0)
                {
                    labelDbm.Text = "信号强度 遥测在线";
                    labelDbm.ForeColor = Color.Orange;
                }
                else
                {
                    labelDbm.Text = "信号强度 ---dbm";
                    labelDbm.ForeColor = Color.Red;
                }
            }
            else
            {
                labelDbm.Text = string.Format("信号强度 {0}dbm", dbm);
                if (dbm > -70)
                    labelDbm.ForeColor = Color.Lime;
                else if (dbm > -95)
                    labelDbm.ForeColor = Color.Orange;
                else
                    labelDbm.ForeColor = Color.Red;
            }
        }

        private void viewTM_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int[] rows = viewTM.GetSelectedRows();
            int row = rows[0];
            allParamList[row].showInImage = !allParamList[row].showInImage;
            InitChart();
        }

        private void InitChart()
        {
            chartFlight.Series.Clear();
            for (int i = 1; i < TMHandler.FlightDataTableForView[(paoID,guanID)].Columns.Count; i++)
            {
                if (allParamList[i - 1].showInImage == false)
                    continue;
                Series se = new Series(TMHandler.FlightDataTableForView[(paoID, guanID)].Columns[i].ColumnName, ViewType.Line);
                se.DataSource = TMHandler.FlightDataTableForView[(paoID, guanID)];
                se.ArgumentDataMember = "second";
                se.ValueDataMembers.AddRange(TMHandler.FlightDataTableForView[(paoID, guanID)].Columns[i].ColumnName);
                chartFlight.Series.Add(se);
            }          
            if(chartFlight.Series.Count == 0)
            {
                chartFlight.Series.Add(new Series());
            }
        }

        public void RefreshUI()
        {
            if (OpenModelUpload)
            {
                labelPacketProgress.Text = string.Format("{0} / {1}", scoutneedbagid + 1, manager._templates.totalcount);
                labelPacketProgress.Position = (int)(scoutneedbagid + 1) * 100 / (manager._templates.totalcount);
            }
            if (bookstate == 0)
                bookstatelab.Text = "装订情况";
            else if (bookstate == 1)
                bookstatelab.Text = "装订中";
            else if (bookstate == 2)
                bookstatelab.Text = "装订成功";
            else if (bookstate == 3)
                bookstatelab.Text = "装订失败";

        }


        /// <summary>
        /// 贺静
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        List<fileImageLocalFolder> fileInFolderList = new List<fileImageLocalFolder>();
        string defaultFolder;
        public static bool OpenModelUpload = false; //是否进入装订状态
        public static int scoutneedbagid = 0; //需要的第几包数据
        public static bool typetxt = false; //卫星图
        public static int bookstate = 0;//0：装订情况 1.装订中 2.装订成功 3.装订失败 
        public static int soutrecvcount = 200;
        public static UploadManager manager = new UploadManager();

        private static void ThreadWork()
        {
            // 执行你的函数
            ExecuteFunction();
        }
        private void btLoadImage_Click(object sender, EventArgs e)
        {
            OpenModelUpload = false;
            scoutneedbagid = 0;
            manager = new UploadManager();
            typetxt = false;
            manager.ScanTemplates("./test");
            //发送装订指令
            Byte[] data = new Byte[62];
            string firsttxt = manager._templates.TxtContent;
            if (firsttxt[0] == char.Parse("1"))
            {
                typetxt = true;
                if (firsttxt[3] == char.Parse("1")) //红外图
                    data[4] = 0x01;
                else
                    data[4] = 0x02;
            }
            else
                data[4] = 0x02;
            NetDataHandle.Send_To_TM(1, 1, 62, 0xAA, data);

            Thread thread = new Thread(ThreadWork);
            //开启线程发送信息
            thread.Start();
        }

        public enum ImageFormat { Jpg = 0x01, Bmp = 0x02, Png = 0x03, }

        public class ImageTemplate
        {
            public string TxtPath { get; set; }
            public string ImagePath { get; set; }
            public ImageFormat Format { get; private set; }

            public string TxtContent => File.ReadAllText(TxtPath);
            //public byte[] ImageData => File.ReadAllBytes(ImagePath);
            public byte[] ImageData;
            //public Mat imageInfo;

            public int totalcount;
            public ushort imagewidth;
            public ushort imageheight;
            public byte imageid;

            public void DetectFormat()
            {
                string ext = Path.GetExtension(ImagePath).ToLower();
                if (ext == ".png")
                {
                    Format = ImageFormat.Png;
                }
                else if (ext == ".jpg" || ext == ".jpeg")
                {
                    Format = ImageFormat.Jpg;
                }
                else if (ext == ".bmp")
                {
                    Format = ImageFormat.Bmp;
                }
                using (Image image = Image.FromFile(ImagePath))
                {
                    imagewidth = (ushort)image.Width;
                    imageheight = (ushort)image.Height;
                }
                Mat imageInfo = Cv2.ImRead(ImagePath, ImreadModes.Unchanged);//需指定单通道读取，否则会自动加载成双通道。
                ImageData = new byte[imageInfo.Total() * imageInfo.ElemSize()];
                Marshal.Copy(imageInfo.Data, ImageData, 0, ImageData.Length);
            }
        }
        public class UploadManager
        {
            public ImageTemplate _templates;
            private CancellationTokenSource _cts;
            public int UploadedPackets { get; private set; }
            public void ScanTemplates(string directory)
            {
                var txtFiles = Directory.GetFiles(directory, "img*.txt");

                foreach (var txtFile in txtFiles)
                {
                    string baseName = Path.GetFileNameWithoutExtension(txtFile);
                    int number = int.Parse(Regex.Match(baseName, @"\d+").Value);
                    //_templates.imageid = (byte)number;
                    var imgFile = Directory.GetFiles(directory, $"{baseName}.*")
                        .FirstOrDefault(f =>
                            f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase));

                    if (imgFile != null)
                    {
                        var template = new ImageTemplate
                        {
                            TxtPath = txtFile,
                            ImagePath = imgFile
                        };
                        template.DetectFormat();
                        template.imageid = (byte)0x01;
                        template.totalcount = 1 + (int)Math.Ceiling(template.ImageData.Length / 200.0);
                        _templates = template;
                    }
                }
            }

        }

        // 每 133ms 执行的函数
        private static void ExecuteFunction()
        {
            bool firstwait = true;
            if (firstwait)
            {
                Thread.Sleep(1000);//等待装订中信息回报
                firstwait = false;
            }
            while (OpenModelUpload)
            {
                if (soutrecvcount == 200)
                {
                    if (scoutneedbagid < manager._templates.totalcount)
                    {
                        if (scoutneedbagid == 0)//第一包为txt模板信息
                        {
                            byte[] sendinfo = new byte[32];
                            if (typetxt) //拍摄图
                            {
                                double[] numbers = manager._templates.TxtContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(double.Parse)
                                .ToArray();

                                numbers[5] *= 10; numbers[6] *= 100;
                                ushort[] ints = numbers.Select(d => (ushort)d).ToArray();

                                sendinfo[0] = (byte)0x01;
                                sendinfo[1] = manager._templates.imageid;
                                sendinfo[2] = (byte)(scoutneedbagid);
                                sendinfo[3] = (byte)ints[0];
                                sendinfo[4] = (byte)ints[1];
                                sendinfo[5] = (byte)(ints[2] & 0xFF);
                                sendinfo[6] = (byte)((ints[2] >> 8) & 0xFF);
                                sendinfo[7] = (byte)(ints[3] & 0xFF);
                                sendinfo[8] = (byte)((ints[3] >> 8) & 0xFF);
                                sendinfo[9] = (byte)ints[4];
                                sendinfo[10] = (byte)(ints[5] & 0xFF);
                                sendinfo[11] = (byte)((ints[5] >> 8) & 0xFF);
                                sendinfo[12] = (byte)(ints[6] & 0xFF);
                                sendinfo[13] = (byte)((ints[6] >> 8) & 0xFF);
                                sendinfo[14] = (byte)manager._templates.Format;
                                sendinfo[15] = (byte)(manager._templates.imagewidth);
                                sendinfo[16] = (byte)((manager._templates.imagewidth >> 8) & 0xFF);
                                sendinfo[17] = (byte)(manager._templates.imageheight & 0xFF);
                                sendinfo[18] = (byte)((manager._templates.imageheight >> 8) & 0xFF);
                                sendinfo[19] = 0x01;
                                sendinfo[20] = (byte)(ints[7] & 0xFF);
                                sendinfo[21] = (byte)((ints[7] >> 8) & 0xFF);
                                sendinfo[22] = (byte)(ints[8] & 0xFF);
                                sendinfo[23] = (byte)((ints[8] >> 8) & 0xFF);
                                sendinfo[24] = (byte)(ints[9] & 0xFF);
                                sendinfo[25] = (byte)((ints[9] >> 8) & 0xFF);
                                sendinfo[26] = (byte)(ints[10] & 0xFF);
                                sendinfo[27] = (byte)((ints[10] >> 8) & 0xFF);
                                NetDataHandle.Send_To_TM(1, 1, 32, 0xB1, sendinfo);
                                soutrecvcount--;
                            }
                            else //卫星图
                            {
                                double[] numbers = manager._templates.TxtContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(double.Parse)
                                 .ToArray();
                                numbers[1] *= 10;
                                ushort[] ints = numbers.Select(d => (ushort)d).ToArray();

                                sendinfo[0] = (byte)0x01;
                                sendinfo[1] = (byte)manager._templates.imageid;
                                sendinfo[2] = (byte)(scoutneedbagid);
                                sendinfo[3] = (byte)ints[0];
                                sendinfo[4] = (byte)manager._templates.Format;
                                sendinfo[5] = (byte)(manager._templates.imagewidth & 0xFF);
                                sendinfo[6] = (byte)((manager._templates.imagewidth >> 8) & 0xFF);
                                sendinfo[7] = (byte)(manager._templates.imageheight & 0xFF);
                                sendinfo[8] = (byte)((manager._templates.imageheight >> 8) & 0xFF);
                                sendinfo[9] = 0x01;
                                sendinfo[10] = (byte)(ints[1]);
                                sendinfo[11] = (byte)(ints[2] & 0xFF);
                                sendinfo[12] = (byte)((ints[2] >> 8) & 0xFF);
                                sendinfo[13] = (byte)(ints[3] & 0xFF);
                                sendinfo[14] = (byte)((ints[3] >> 8) & 0xFF);
                                sendinfo[15] = (byte)(ints[4] & 0xFF);
                                sendinfo[16] = (byte)((ints[4] >> 8) & 0xFF);
                                sendinfo[17] = (byte)(ints[5] & 0xFF);
                                sendinfo[18] = (byte)((ints[5] >> 8) & 0xFF);

                                NetDataHandle.Send_To_TM(1, 1, 32, 0xB1, sendinfo);
                                soutrecvcount--;
                            }

                        }
                        else//图片信息
                        {
                            byte[] sendinfo = new byte[207];
                            sendinfo[0] = (byte)manager._templates.imageid;
                            sendinfo[1] = (byte)(manager._templates.totalcount & 0xFF);
                            sendinfo[2] = (byte)((manager._templates.totalcount >> 8) & 0xFF);
                            sendinfo[3] = (byte)(scoutneedbagid & 0xFF);
                            sendinfo[4] = (byte)((scoutneedbagid >> 8) & 0xFF);
                            ushort imagelength = 0;
                            if (scoutneedbagid < manager._templates.totalcount - 1)
                                imagelength = 200;
                            else
                                imagelength = (ushort)((manager._templates.ImageData.Length) % 200);
                            sendinfo[5] = (byte)(imagelength & 0xFF);
                            sendinfo[6] = (byte)((imagelength >> 8) & 0xFF);
                            Array.Copy(manager._templates.ImageData, (scoutneedbagid - 1) * 200, sendinfo, 7, imagelength);
                            NetDataHandle.Send_To_TM(1, 1, 207, 0xB2, sendinfo);
                            soutrecvcount--;
                        }
                    }
                }
                Thread.Sleep(135);
            }
        }

        private void btDoUmb_Click(object sender, EventArgs e)
        {

            byte[] data = new byte[1];
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0, 0x22, data);
            /*
            if (btDoUmb.Text == "已执行紧急伞降")
            {
                //再次发出紧急伞降指令
                byte[] data = new byte[1];
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0, 0x22, data);
                return;
            }
               
            string[] part = btDoUmb.Text.Split(new[] { '(', ')' });
            if (Convert.ToInt32(part[1]) < 5)
            {
                btDoUmb.Text = string.Format("紧急伞降({0})", Convert.ToInt32(part[1]) + 1);
            }
            else
            {
                btDoUmb.Text = "已执行紧急伞降";
                byte[] data = new byte[1];
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0, 0x22, data);
            }
            */
        }

        private void btHome_Click(object sender, EventArgs e)
        {
            //btHome.Text = "已执行紧急返航";
            byte[] data = new byte[1];
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0, 0x23, data);
        }

        private void btSaveExcel_Click(object sender, EventArgs e)
        {
            TMHandler.SaveTablesToFile();
        }

        private void btClearData_Click(object sender, EventArgs e)
        {
            TMHandler.ClearFlightDT(NetDataHandle.curPao,NetDataHandle.curGuan);
        }

        private void btClearSelection_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < TMHandler.FlightDataTable[(0,0)].Columns.Count; i++)
            {
                allParamList[i - 1].showInImage = false;
            }
            InitChart();
            viewTM.RefreshData();
        }

        private void checkAutoSendByClick_CheckedChanged(object sender, EventArgs e)
        {
            videoControl1.AutoSendToFK = checkAutoSendByClick.Checked;
        }

        private void radioLowlight_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioLowlight.Checked)
                return;
            byte[] data = new byte[1];
            data[0] = 2; // 协议 0x09：红外波段 0x02
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan,
                1, 0xD4, data);
        }

        private void radioTV_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioTV.Checked)
                return;
            byte[] data = new byte[1];
            data[0] = 1; // 协议 0x09：可见光波段 0x01
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan,
                1, 0xD4, data);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SatelliteTxt
    {
        byte modcount; //装订模板总数
        byte imageid;  //模板图像编号
        byte bagid;    //包编号
        byte picturemode; // 0:卫星图
        byte typeimage;   // 1：jpg格式 2：bmp格式 3：png格式
        ushort widthimg;
        ushort heightimg;
        byte channel;
        byte scale;    //*10
        ushort targetlocationx;
        ushort targetlocationy;
        ushort targetxlength;
        ushort targetylength;
        //26-38备用
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PhotoTxt
    {
        byte modcount;
        byte imageid;
        byte bagid;
        byte picturemode; // 1：拍摄图
        byte typemodel;   // 0：电视 1红外

        ushort distance;
        byte pitchpic;
        ushort dirpic;
        ushort focusepic;  //*10
        ushort sizepic;  //*100

        byte typeimage;   //  1：jpg格式 2：bmp格式 3：png格式
        ushort widthimg;
        ushort heightimg;
        byte channel;
        ushort targetlocationx;
        ushort targetlocationy;
        ushort targetxlength;
        ushort targetylength;
        //35-38备用
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetImageInfoBack
    {
        byte imageid;
        byte feedbackinfo; // 0x00 默认 0x01 包文件收到 0x02 校验和失败 oxA1 上传成功 0xA2上传失败
        ushort bigid; //重新上传的包编号
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageInfo
    {
        byte imageid;  //模板图像编号
        ushort totalbig;//总包数 包括txt
        ushort bagid;    //包编号
        ushort sendlenth;
        //14-213为数据区
    }
    //7-48
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetBFrameBack
    {
        byte dytctrlback;
        byte workmodback;
        byte lowlighlback;
        byte bindmodback;
    }

    //7-68
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SendAFrame
    {
        byte ctrlCmd;
        byte modelIndex;
        byte workMode;
        byte lowlighlCtrl;
        byte modelbookCmd;

    }
}
