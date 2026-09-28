using DevExpress.Charts.Native;
using DevExpress.Utils.MVVM.Services;
using DevExpress.XtraCharts;
//using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GroundLunch
{
    public partial class MsnShowAndSelect : DevExpress.XtraEditors.XtraUserControl
    {
        public List<MissionFile> missionFiles = new List<MissionFile>();
        public int selectedMissionIndex = -1;
        public DataTable dtMap = new DataTable();
        public DataTable dtSafeArea = new DataTable();
        public List<DataTable> dtGroupAirLines = new List<DataTable>();
        public MainForm mainForm;
       // List<Series> seTargetLine;
       // List<Series> seTargetPoint;
        public MsnShowAndSelect()
        {
            if (DesignMode)
                return;
            InitializeComponent();
            AutoLoadMsnList();
            InitMsnListGird();
            InitMsnGorupListGrid();
            InitMapCtrl();
            ShowSelectMsnMap();
            //InitTestMsnFile();
            MessageEvents.OnMessageMsnUpdateReceived += HandleMsnUpdateMessage;
        }

        private void HandleMsnUpdateMessage(string message)
        {
            ShowSelectMsnMap();
            AutoSaveMsnList();
        }

        public void InitMsnListGird()
        {
            gridMsnList.DataSource = null;
            gridMsnList.DataSource = missionFiles;
            viewMsnList.Columns[0].Width = 50 * gridMsnList.Width / 100;
            viewMsnList.Columns[1].Width = 25 * gridMsnList.Width / 100;
            viewMsnList.Columns[2].Width = 25 * gridMsnList.Width / 100;
        }

        public void InitMsnGorupListGrid()
        {
            if (selectedMissionIndex == -1)
                return;
            gridMsnGroup.DataSource = null;
            gridMsnGroup.DataSource = missionFiles[selectedMissionIndex].flightGroups;
        }

        public void InitMapCtrl() 
        {
            dtSafeArea.Columns.Clear();
            dtSafeArea.Columns.Add("lonSafe", typeof(double));
            dtSafeArea.Columns.Add("latSafe", typeof(double));

            for (int i = 0; i < 8; i++)
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("lon", typeof(double));
                dataTable.Columns.Add("lat", typeof(double));
                dtGroupAirLines.Add(dataTable);
            }

            Series seSafe = new Series("飞行安全区", ViewType.ScatterLine);
            seSafe.DataSource = dtSafeArea;
            seSafe.ArgumentDataMember = "lonSafe";
            seSafe.ValueDataMembers.AddRange("latSafe");
            chartMap.Series.Add(seSafe);

            for (int i = 0; i < 8; i++)
            {
                string sePointName = string.Format("编队{0}飞行航点", i + 1);
                Series sePoint = new Series(sePointName, ViewType.Point);
                (sePoint.View as PointSeriesView).PointMarkerOptions.Size = 10;
                (sePoint.View as PointSeriesView).PointMarkerOptions.Kind = MarkerKind.Diamond;
                sePoint.DataSource = dtGroupAirLines[i];
                sePoint.ArgumentDataMember = "lon";
                sePoint.ValueDataMembers.AddRange("lat");
                chartMap.Series.Add(sePoint);

                string seLineName = string.Format("编队{0}飞行航线", i + 1);
                Series seLine = new Series(seLineName, ViewType.ScatterLine);
                (seLine.View as ScatterLineSeriesView).PointMarkerOptions.BorderColor = (sePoint.View as PointSeriesView).PointMarkerOptions.BorderColor;
                seLine.DataSource = dtGroupAirLines[i];
                seLine.ArgumentDataMember = "lon";
                seLine.ValueDataMembers.AddRange("lat");
                chartMap.Series.Add(seLine);
            }
        }

        public void InitTestMsnFile()
        {
            MissionFile missionFile = new MissionFile();
            for (int i = 0; i < 3; i++)
            {
                MsnFlightGroup msnFlightGroup = new MsnFlightGroup();
                for (int j = 0; j < 12; j++)
                {
                    MsnPlane msnPlane = new MsnPlane();
                    msnPlane.MsnPlaneType = (j % 3) + 1;
                    msnFlightGroup.planes.Add(msnPlane);
                }
                WayPoint wayPoint1 = new WayPoint();
                wayPoint1.lon = 116.0;
                wayPoint1.lat = 39.0;
                wayPoint1.alt = 50;
                wayPoint1.dir = 15;
                wayPoint1.wpType = 1;
                wayPoint1.radis = 0;
                WayPoint wayPoint2 = new WayPoint();
                wayPoint2.lon = 117.0;
                wayPoint2.lat = 39.1;
                wayPoint2.alt = 50.1;
                wayPoint2.dir = 150;
                wayPoint2.wpType = 2;
                wayPoint2.radis = 20;
                msnFlightGroup.airLine.Add(wayPoint1);
                msnFlightGroup.airLine.Add(wayPoint2);

                missionFile.flightGroups.Add(msnFlightGroup);
            }
            MsnPlane plane = new MsnPlane();

            missionFiles.Add(missionFile);
            AutoSaveMsnList();
        }


        public void ResetMapArea()
        {
            double minLon = 180, minLat = 90, maxLon = -180, maxLat = -90;
            foreach (DataRow safePointRow in dtSafeArea.Rows)
            {
                double valuelon = (double)safePointRow["lonSafe"];
                double valuelat = (double)safePointRow["latSafe"];
                if (minLon > valuelon)
                    minLon = valuelon;
                if (minLat > valuelat)
                    minLat = valuelat;
                if (maxLon < valuelon)
                    maxLon = valuelon;
                if (maxLat < valuelat)
                    maxLat = valuelat;
            }
            double snapLon = maxLon - minLon;
            double snapLat = maxLat - minLat;
            double centerLon = (maxLon + minLon) / 2;
            double centerLat = (maxLat + minLat) / 2;
            if (snapLon > snapLat)
            {
                AxisX firstX = ((XYDiagram)chartMap.Diagram).AxisX;
                firstX.WholeRange.SetMinMaxValues(minLon - 0.002, maxLon + 0.002);

                AxisY firstY = ((XYDiagram)chartMap.Diagram).AxisY;
                firstY.WholeRange.SetMinMaxValues(centerLat - snapLon / 2, centerLat + snapLon / 2);
            }
            else
            {
                AxisX firstX = ((XYDiagram)chartMap.Diagram).AxisX;
                firstX.WholeRange.SetMinMaxValues(centerLon - snapLat / 2, centerLon + snapLat / 2);
                AxisY firstY = ((XYDiagram)chartMap.Diagram).AxisY;
                firstY.WholeRange.SetMinMaxValues(minLat - 0.002, maxLat + 0.002);
            }
        }

        public void ShowSelectMsnMap()
        {
            if (selectedMissionIndex == -1)
                return;
            //根据安全区确定地图范围
            DataTable dt = dtSafeArea;
            dt.Clear();
            int ptCount = (int)missionFiles[selectedMissionIndex].safeArea.Count;
            for (int i = 0; i < ptCount; i ++)
            {
                DataRow dr = dt.NewRow();
                List<object> objlist = new List<object>();
                objlist.Add(missionFiles[selectedMissionIndex].safeArea[i].lon);
                objlist.Add(missionFiles[selectedMissionIndex].safeArea[i].lat);
                object[] rowArray = objlist.ToArray();
                dr.ItemArray = rowArray;
                dt.Rows.Add(dr);
            }
            DataRow dr1 = dt.NewRow();
            List<object> objlist1 = new List<object>();
            objlist1.Add(missionFiles[selectedMissionIndex].safeArea[0].lon);
            objlist1.Add(missionFiles[selectedMissionIndex].safeArea[0].lat);
            object[] rowArray1 = objlist1.ToArray();
            dr1.ItemArray = rowArray1;
            dt.Rows.Add(dr1);
            ResetMapArea();
            //根据group绘制不同的点及曲线段
            int index = 0;
            foreach (var item in dtGroupAirLines) { item.Clear(); }
            foreach (MsnFlightGroup msnFlightGroup in missionFiles[selectedMissionIndex].flightGroups)
            {
                dt = dtGroupAirLines[index];
                
                ptCount = msnFlightGroup.airLine.Count;
                for (int i = 0; i < ptCount; i++)
                {
                    DataRow dr = dt.NewRow();
                    List<object> objlist = new List<object>();
                    objlist.Add(msnFlightGroup.airLine[i].lon);
                    objlist.Add(msnFlightGroup.airLine[i].lat);
                    object[] rowArray = objlist.ToArray();
                    dr.ItemArray = rowArray;
                    dt.Rows.Add(dr);
                }
                index++;
            }
            
        }

        public void ReFreshUI()
        {
            int res = JudgePlaneCountOK();
            if (res == 0)
            {
                labelTips.Text = "已就绪飞机数量不满足当前任务";
                labelTips.ForeColor = Color.Orange;
                btUploadMsn.Enabled = false;
            }
            else
            {
                labelTips.Text = "已就绪飞机数量已满足当前任务";
                labelTips.ForeColor = Color.Lime;
                btUploadMsn.Enabled = true;
            }
            viewGroupList.RefreshData();
        }

        public int JudgePlaneCountOK()
        {
            //按编队获取是否满足，如果不满足就不再往下找了
            foreach (MsnFlightGroup group in missionFiles[selectedMissionIndex].flightGroups)
            {
                //查找在线的炮车
                int find = 0;
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        if(NetDataHandle.planeConnectStatus[(i, j)] != 0)
                        {
                            //所有的当前编队必须在本炮车获取
                            int count1 = group.getPlaneCountByType(1);
                            int count2 = group.getPlaneCountByType(2);
                            int count3 = group.getPlaneCountByType(3);
                            if (IsPaoContainsPlane(i, count1, count2, count3))
                            {
                                //本炮车满足了本编队足够的飞机数量,开始查找下一编队
                                find = 1;
                            }
                            //count1,2,3是尚未满足的数量
                            if (find == 1)
                            {
                                //给设备做任务和炮车的匹配
                                MsnPaoConfig(group, i, count1, count2, count3);
                            }
                            break;
                        }
                    }
                    if (find == 1)
                        break;
                }
                if (find == 0)
                    return 0;
            }
            return 1;
        }

        private bool IsPaoContainsPlane(int paoID, int attackCount, int scoutCount, int interferCount)
        {
            for (int i = 0; i < 12; i++)
            {
                int step = (int)TMHandler.GetValueByParamID(paoID, i, "autoStep");
                if (step >= 4)
                {
                    int payloadType = (int)TMHandler.GetValueByParamID(paoID, i, "paylodtp");
                    if ((payloadType & 1) == 1 && scoutCount > 0)
                    {
                        scoutCount--;
                    }
                    if ((payloadType & 0B10) == 0B10 && attackCount > 0)
                    {
                        attackCount--;
                    }
                }
            }
            if (attackCount == 0 && scoutCount == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MsnPaoConfig(MsnFlightGroup group, int paoID, int attackCount, int scoutCount, int interferCount)
        {
            int[] payloadType = new int[12];
            for (int i = 0; i < 12; i++)
            {
                int step = (int)TMHandler.GetValueByParamID(paoID, i + 1, "autoStep");
                if (step >= 4)
                {
                    payloadType[i] = (int)TMHandler.GetValueByParamID(paoID, i + 1, "paylodtp");

                }
            }
            foreach (var plane in group.planes)
            {
                //先匹配摄像头
                if (plane.MsnPlaneType == 2)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if ((payloadType[i] & 0B1) == 0B1)
                        {
                            plane.paoID = paoID;
                            plane.guanID = i + 1;
                            payloadType[i] = 0;
                            break;
                        }
                    }
                }
            }
            foreach (var plane in group.planes)
            {
                //先匹配摄像头
                if (plane.MsnPlaneType == 1)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if ((payloadType[i] & 0B10) == 0B10)
                        {
                            plane.paoID = paoID;
                            plane.guanID = i + 1;
                            payloadType[i] = 0;
                            break;
                        }
                    }
                }
            }
        }

        private void viewMsnList_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            selectedMissionIndex = e.RowHandle;
            InitMsnGorupListGrid();
            ShowSelectMsnMap();
            mainForm.missionForm.OnSelMsnFileChange(missionFiles[selectedMissionIndex]);
        }

        private void btUploadMsn_Click(object sender, EventArgs e)
        {
            //要向所有飞机更新任务信息
            foreach (MsnFlightGroup group in missionFiles[selectedMissionIndex].flightGroups)
            {
                if (group.airLine != null && group.airLine.Count > 0)
                {
                    WayPoint initWp = group.airLine.Find(w => w.wpType == 0);
                    if (initWp == null)
                        initWp = group.airLine[0];
                    if (initWp.dir < 0 || initWp.dir >= 360)
                    {
                        MessageBox.Show(string.Format(
                            "初始航向角 {1:F2}° 不在 0~359.99，无法装订任务",
                            group.groupName, initWp.dir));
                        return;
                    }
                }
                foreach (MsnPlane plane in group.planes) 
                {
                    //默认需要分包，依据为安全区点个数和航点个数
                    int pointUseByteLen = missionFiles[selectedMissionIndex].safeArea.Count * 8 +
                        group.airLine.Count * 17;
                    //第一包上传包号0、编组号和任务号、安全区点数、安全区内容、航点数量
                    //第二包上传包号1、0-11航点
                    //第三包上传包号2、12-23航点
                    //第四包上传包号3、24-35航点

                    //检查数据合理性
                    if (missionFiles[selectedMissionIndex].safeArea.Count > 20)
                    {
                        MessageBox.Show("安全区点数过多");
                        return;
                    }
                    if (group.airLine.Count > 36)
                    {
                        MessageBox.Show("航点数超过36个，请减少航点");
                        return;
                    }
                    //第一包
                    byte[] data = new byte[220];
                    data[0] = (byte)(0);//包号
                    data[1] = (byte)plane.groupID;
                    data[2] = (byte)plane.msnID;
                    data[3] = (byte)missionFiles[selectedMissionIndex].safeArea.Count;
                    data[4] = (byte)group.airLine.Count;
                    data[5] = missionFiles[selectedMissionIndex].navAlignMode;
                    Buffer.BlockCopy(BitConverter.GetBytes(missionFiles[selectedMissionIndex].navAlignTimeSec), 0, data, 6, 2);
                    for (int i = 0; i < missionFiles[selectedMissionIndex].safeArea.Count; i++)
                    {
                        int iLon = (int)(missionFiles[selectedMissionIndex].safeArea[i].lon * 1e7);
                        int iLat = (int)(missionFiles[selectedMissionIndex].safeArea[i].lat * 1e7);
                        Buffer.BlockCopy(BitConverter.GetBytes(iLon), 0, data, 8 + i * 8, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(iLat), 0, data, 12 + i * 8, 4);
                    }
                    NetDataHandle.Send_To_TM(plane.paoID, plane.guanID, (ushort)(8 + 8 * missionFiles[selectedMissionIndex].safeArea.Count), 0x13, data);
                    System.Threading.Thread.Sleep(10); // 等飞控处理完包0，避免被后续航点包覆盖

                    //第二三四包
                    byte[] pointsByte = new byte[17 * 36];
                    int k = 0;
                    foreach (WayPoint wayPoint in group.airLine)
                    {
                        byte[] wp = new byte[17];
                        int iLon = (int)(wayPoint.lon * 1e7);
                        int iLat = (int)(wayPoint.lat * 1e7);
                        Int16 iAlt = (Int16)(wayPoint.alt);
                        // 0~360°×100 最大 36000，超过 Int16(32767)；328° 会溢出成约 32.6°
                        double dir360 = wayPoint.dir % 360.0;
                        if (dir360 < 0)
                            dir360 += 360.0;
                        UInt16 iDir = (UInt16)Math.Round(dir360 * 100.0);
                        Int16 iRadis = (Int16)(wayPoint.radis);
                        UInt16 iSpeed = (UInt16)(wayPoint.speed);
                        Buffer.BlockCopy(BitConverter.GetBytes(iLon), 0, wp, 0, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(iLat), 0, wp, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(iAlt), 0, wp, 8, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(iDir), 0, wp, 10, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(iRadis), 0, wp, 12, 2);
                        wp[14] = (byte)wayPoint.wpType;
                        Buffer.BlockCopy(BitConverter.GetBytes(iSpeed), 0, wp, 15, 2);
                        Buffer.BlockCopy(wp, 0, pointsByte, 17 * k, 17);
                        k++;
                    }
                    //判断航点数量，是否需要分包
                    byte[] sendPointsPacketByte = new byte[205]; //12 * 17 + 1 = 205
                    int s = 0;
                    while (s < group.airLine.Count)
                    {
                        sendPointsPacketByte[0] = (byte)(s / 12 + 1);
                        Buffer.BlockCopy(pointsByte, s * 17, sendPointsPacketByte, 1, 12 * 17);
                        NetDataHandle.Send_To_TM(plane.paoID, plane.guanID, (ushort)(205), 0x13, sendPointsPacketByte);
                        System.Threading.Thread.Sleep(10);
                        s += 12;
                    }
                }
            }
           
        }
        private void AutoLoadMsnList()
        {
            string msnDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Msn");
            if (!Directory.Exists(msnDir))
                return;
            foreach (string file in Directory.EnumerateFiles(msnDir, "*.xml", SearchOption.AllDirectories))
            {
                FileStream reader = new FileStream(file, FileMode.Open);
                XmlSerializer zer = new XmlSerializer(typeof(MissionFile));
                MissionFile missionFile = (MissionFile)zer.Deserialize(reader);
                reader.Close();
                missionFile.FileName = Path.GetFileName(file);
                missionFiles.Add(missionFile);
            }
            if (missionFiles.Count > 0)
            {
                selectedMissionIndex = 0;
            }
            foreach (MissionFile missionFile in missionFiles)
            {
                int groupIndex = 1;
                foreach (MsnFlightGroup group in missionFile.flightGroups)
                {
                    int msnIndex = 1;
                    foreach (MsnPlane plane in group.planes)
                    {
                        plane.groupID = groupIndex;
                        plane.msnID = msnIndex;
                        plane.paoID = 0;
                        plane.guanID = 0;
                        plane.luanched = 0;
                        msnIndex++;
                    }
                    groupIndex++;
                }
            }
        }

        private void AutoSaveMsnList()
        {
            string savePath = string.Format("./Msn/{0}", missionFiles[selectedMissionIndex].FileName);
            XmlSerializer myxml = new XmlSerializer(typeof(MissionFile));
            FileStream fs = new FileStream(savePath, FileMode.Create);
            myxml.Serialize(fs, missionFiles[selectedMissionIndex]);
            fs.Close();
        }

        private void MsnShowAndSelect_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            mainForm.missionForm.OnSelMsnFileChange(missionFiles[selectedMissionIndex]);
        }
    }



    public class MissionFile
    {
        public List<MsnFlightGroup> flightGroups = new List<MsnFlightGroup>();
        public List<WayPoint> safeArea = new List<WayPoint>();

        /// <summary>导航对准模式：0=水平，1=垂直。
        [XmlAttribute]
        public byte navAlignMode = 1;

        [XmlAttribute]
        public ushort navAlignTimeSec = 210;

        [XmlIgnore]
        public string FileName;

        [XmlIgnore]
        public string 任务名称
        {
            get { return FileName; } 
        }

        [XmlIgnore]
        public string 总编队数
        {
            get { return flightGroups.Count.ToString(); }
        }

        [XmlIgnore]
        public string 总飞机数
        {
            get { return getPlaneCount().ToString(); }
        }

        public int getPlaneCount()
        {
            int count = 0;
            foreach (MsnFlightGroup f in flightGroups)
            {
                count += f.getPlaneCountByType(1);
                count += f.getPlaneCountByType(2);
                count += f.getPlaneCountByType(3);
            }
            return count;
        }
    }

    public class WayPoint
    {
        [XmlAttribute]
        public double lon;
        [XmlAttribute]
        public double lat;
        [XmlAttribute]
        public double alt;
        [XmlAttribute]
        public double dir;
        [XmlAttribute]
        public int wpType;
        [XmlAttribute]
        public double radis;
        [XmlAttribute]
        public double speed;

        public string type
        {
            get 
            {
                switch (wpType)
                {
                    case 0:
                        return "起飞点";
                    case 1:
                        return "普通航点";
                    case 2:
                        return "指点飞行";
                    case 3:
                        return "盘旋飞行";
                    case 4:
                        return "打击点";
                    case 5:
                        return "佯攻点";
                    case 6:
                        return "回收点";
                    default:
                        return "未知";
                }
                
            }
        }

        public string longitude
        {
            get { return lon.ToString("F4"); }
        }

        public string latitude
        {
            get { return lat.ToString("F4"); }
        }

        public string altitude
        {
            get { return alt.ToString("F0"); }
        }

        public string outTrack
        {
            get { return dir.ToString("F1"); }
        }

        public string flightspeed
        {
            get { return speed.ToString("F1"); }
        }

        public string radius
        {
            get { return radis.ToString("F1"); }
        }
    }

    public class MsnFlightGroup
    {
        [XmlAttribute]
        public string groupName;
        public List<MsnPlane> planes = new List<MsnPlane>();
        public List<WayPoint> airLine = new List<WayPoint>();
        [XmlIgnore]
        public string 编队名称
        {
            get { return groupName; }
            set {; }
        }

        public string 攻击飞机
        {
            get { return getCanusePlaneCountByType(1).ToString() + "/" + getPlaneCountByType(1).ToString(); }
            set {; }
        }

        public string 侦察飞机
        {
            get { return getCanusePlaneCountByType(2).ToString() + "/" + getPlaneCountByType(2).ToString(); }
            set {; }
        }

        public string 干扰飞机
        {
            get { return getCanusePlaneCountByType(3).ToString() + "/" + getPlaneCountByType(3).ToString(); }
            set {; }
        }


        public int getPlaneCountByType(int type)
        {
            int count = 0;
            foreach (MsnPlane plane in planes) 
            {
                if (plane.MsnPlaneType == type)
                {
                    count++;
                }
            }
            return count;
        }

        public int getCanusePlaneCountByType(int type)
        {
            int count = 0;
            foreach (MsnPlane plane in planes)
            {
                if (plane.MsnPlaneType == type  && plane.paoID != 0)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public class MsnPlane
    {
        [XmlAttribute]
        public int MsnPlaneType;
        [XmlIgnore]
        public int groupID;
        [XmlIgnore]
        public int msnID;
        [XmlIgnore]
        public int paoID;
        [XmlIgnore]
        public int guanID;
        [XmlIgnore]
        public int luanched;
    }

    // 新增：航点显示包装类，包含序号和线段距离信息
    public class WayPointDisplayItem
    {
        public WayPoint WayPoint { get; set; }
        public int Index { get; set; }
        public double SegmentDistance { get; set; } // 到前一个点的距离

        public WayPointDisplayItem(WayPoint wayPoint, int index, double segmentDistance)
        {
            WayPoint = wayPoint;
            Index = index;
            SegmentDistance = segmentDistance;
        }

        // 代理属性，直接访问 WayPoint 的属性
        public string type => WayPoint.type;
        public string longitude => WayPoint.longitude;
        public string latitude => WayPoint.latitude;
        public string altitude => WayPoint.altitude;
        public string outTrack => WayPoint.outTrack;
        public string flightspeed => WayPoint.flightspeed;
        public string radius => WayPoint.radius;

        // 新增：序号显示属性（从0开始）
        public string indexNumber => (Index).ToString();

        // 新增：线段距离显示属性（到前一个点的距离，米）
        public string segmentDistance => SegmentDistance.ToString("F0");
    }
}
