using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraScheduler.Outlook.Native;
//using DocumentFormat.OpenXml.Wordprocessing;
//using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;
using SharpGL.SceneGraph.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using static DevExpress.Utils.Diagnostics.GUIResources;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace GroundLunch
{

    public partial class TMPage : DevExpress.XtraEditors.XtraUserControl
    {
        public bool pageSelected = false;
        private bool dataSource422 = true;
        public List<string> ips = new List<string>();
        public List<string> ports = new List<string>();
        public List<TMPacket> packets = new List<TMPacket>();

        public Dictionary<int, List<TMFrame>> tMFramesByGroupID = new Dictionary<int, List<TMFrame>>();
        public Dictionary<int, List<TMParam>> allParamList = new Dictionary<int, List<TMParam>>();
        public Dictionary<string, KeyValuePair<TMPacket, DataTable>> SaveDataTables = new Dictionary<string, KeyValuePair<TMPacket, DataTable>>();

        public DataTable FlightDataTable = new DataTable();

        string defaultTmExcel = ".\\遥测\\遥测.xlsx";

        public int _10hzPageCount = 0;
        public int controlParamByte = 0;
        public int payloadParamByte = 0;
        public int _200hzParamByte = 0;
        public int _40hzParamByte = 0;
        public int _10hzParamByte = 0;

        MyMMTimer tmHandleTimer = new MyMMTimer();
        public TMPage()
        {
            InitializeComponent();
            
           
            tmHandleTimer.CreateTimer(HandleTmData100Ms, 95);
            tmTrack3D.eNormalize = Line3D.eNormalize.MaintainXY;
            //tmTrack3D1.eNormalize = Line3D.eNormalize.MaintainXYZ;
            tmTrack3D.LoadSimuData();
            //tmTrack3D1.LoadSimuData();
            for (int i = 0; i < 6; i++)
            {
                tMFramesByGroupID[i] = new List<TMFrame>();
                allParamList[i] = new List<TMParam>();
            }
            InitFramesByGroupID();
            AutoLoadExcel();
        }

        private void TMPage_Load(object sender, EventArgs e)
        {
            InitGrid(0);
            InitUDPIPList();
            if (NetDataHandle.udps.Count > 0)
            {
                btIPAdress.Text = NetDataHandle.udps[0];
            }
            else
            {
                btIPAdress.Text = "127.0.0.1";
            }
        }

        public void InitFramesByGroupID()
        {
            for (int idIndex = 0; idIndex < 6; idIndex++)
            {
                for (int i = 0; i < 21; i++)
                {
                    TMFrame frame = new TMFrame();
                    tMFramesByGroupID[idIndex].Add(frame);
                }
            }
        }
        private void HandleTmData100Ms(uint id, uint msg, UIntPtr user, UIntPtr dw1, UIntPtr dw2)
        {
            /*
            for (int i = 0; i < 6; i++)
            {
                if (NetDataHandle.tmframes[i].Count > 0)
                {
                    if (NetDataHandle.tmframes[i][0] != null && NetDataHandle.tmframes[i][0].dataLen != 0)
                    {
                        tMFramesByGroupID[i][NetDataHandle.tmframes[i][0].frameGroup] = NetDataHandle.tmframes[i][0];
                        SaveFrame(NetDataHandle.tmframes[i][0]);
                    }
                    NetDataHandle.tmframes[i].RemoveAt(0);
                }
                if (NetDataHandle.tmframes[i].Count > 10)
                {
                    NetDataHandle.tmframes[i].Clear();
                }
            }*/
        }

        

        public void InitDataTables(TMPacket pkt)
        {
            DataTable dt = new DataTable();
            dt.TableName = pkt.packetName;
            dt.Columns.Add("second", typeof(double));
            for (int i = 0; i < pkt.paramList.Count; i++)
            {
                if (pkt.paramList[i].paramName == "备用")
                    continue;
                dt.Columns.Add(pkt.paramList[i].paramName, typeof(double));
            }
            KeyValuePair<TMPacket, DataTable> kvp = new KeyValuePair<TMPacket, DataTable>(pkt, dt);
            SaveDataTables[pkt.packetName] = kvp;
        }

        public void SaveFrame(TMFrame frame)
        {
            if (frame.tick == 0 || frame.tick == 1)
            {
                ClearSaveDataTables();
            }
            if (frame.frameGroup == 20)
                SaveControlFrame(frame);
            else
                SavePlatFrame(frame);
        }

        private void ClearSaveDataTables()
        {
            foreach (var item in SaveDataTables)
            {
                ((DataTable)item.Value.Value).Clear();
            }
        }

        public void SavePlatFrame(TMFrame frame)
        {
            DataTable dt200hz = SaveDataTables["200hz"].Value;
            TMPacket pkt200hz = SaveDataTables["200hz"].Key;
            int index = 0;
            SaveFrameToDataTable(index, frame, pkt200hz, dt200hz);
        }

        public void SaveControlFrame(TMFrame frame)
        {
            DataTable dt = SaveDataTables["控制"].Value;
            TMPacket pkt = SaveDataTables["控制"].Key;
            int index = 0;
            SaveFrameToDataTable(index, frame, pkt, dt);
        }

        public int SaveFrameToDataTable(int byteIndex, TMFrame frame, TMPacket pkt, DataTable dt)
        {
            foreach (TMParam param in pkt.paramList)
            {
                if (param.paramByteCount == 1)
                {
                    param.paramSourceCode = frame.payLoad[byteIndex];
                }
                else if (param.paramByteCount == 2)
                {
                    param.paramSourceCode = BitConverter.ToUInt16(frame.payLoad, byteIndex);
                }
                else if (param.paramByteCount == 4)
                {
                    param.paramSourceCode = BitConverter.ToUInt32(frame.payLoad, byteIndex);
                }
                else if (param.paramByteCount == 8)
                {
                    param.paramSourceCode = BitConverter.ToUInt64(frame.payLoad, byteIndex);
                }
                byteIndex += param.paramByteCount;
            }
            DataRow dr = dt.NewRow();
            List<object> objlist = new List<object>();
            objlist.Add(frame.second);
            foreach (TMParam param in pkt.paramList)
            {
                if (param.paramName == "备用")
                    continue;
                objlist.Add(param.物理量);//param.源码值HEX + " " + 
            }
            object[] rowArray = objlist.ToArray();
            dr.ItemArray = rowArray;
            dt.Rows.Add(dr);
            return byteIndex;
        }

        public void SaveTablesToFile()
        {
            DateTime t = DateTime.Now;
            string excelName = "TMDetail" + t.Year + t.Month + t.Day + t.Hour + t.Minute + t.Second + ".xlsx";
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage ep = new ExcelPackage(excelName);
            ExcelWorksheets sheets = ep.Workbook.Worksheets;
            int pktIndex = 0;
            foreach (var item in SaveDataTables)
            {
                DataTable dt = item.Value.Value;
                ExcelWorksheet curSheet;
                if (sheets.Count > pktIndex)
                    curSheet = sheets[pktIndex];
                else
                    curSheet = sheets.Add(item.Value.Key.packetName);

                int colIndex = 1;
                foreach (DataColumn col in dt.Columns)
                {
                    curSheet.Cells[1, colIndex].Value = col.ToString();
                    colIndex++;
                }
                colIndex = 1;
                int rowIndex = 2;
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (var item1 in dr.ItemArray)
                    {
                        curSheet.Cells[rowIndex, colIndex].Value = item1;
                        colIndex++;
                    }
                    colIndex = 1;
                    rowIndex++;
                }
                pktIndex++;
            }
            ep.Save();
        }
        /***************************
        * 网络连接
        ****************************/

        private void InitUDPIPList()
        {
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            int i = 0;
            foreach (IPAddress address in addresses) 
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    continue;
                ips.Add(address.ToString());
            }
            if (ips.Count > 0)
            {
               // btUDPIP.Text = ips[0];
            }
        }


        /************************************************
         * 图像绘制区域
         
         ************************************************/



        public void InitGrid(int index)
        {
            gridTM.DataSource = null;
            gridTM.DataSource = allParamList[index];

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
            // viewTM.Columns[10].Width = 10 * gridTM.Width / 100;
            //viewTM.Columns[11].Width = 10 * gridTM.Width / 100;

           // viewTM.IsShowRowFooterCell = false;
            for (int i = 1; i < 10; i++)
            {
                viewTM.Columns[i].OptionsColumn.AllowFocus = false;
                viewTM.Columns[i].OptionsColumn.AllowEdit = false;
            }
            
        }

        private void btReloadExcel_Click(object sender, EventArgs e)
        {
            if (File.Exists(defaultTmExcel))
            {
                AnalyzeExcel(defaultTmExcel);
            }
            else
            {
                openFileDialog.Title = "选择要打开的遥测excel文件";
                openFileDialog.Filter = "遥测描述文件 (*.xlsx;*.xls)|*.xlsx;*.xls";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    defaultTmExcel = openFileDialog.FileName;
                    AnalyzeExcel(openFileDialog.FileName);
                }
            }
        }

        private void AutoLoadExcel()
        {
            AnalyzeExcel(".\\遥测\\遥测.xlsx");
        }

        public void AnalyzeExcel(string path)
        {
            packets.Clear();
            for(int i=0;i<6;i++)
                allParamList[i].Clear();
            _10hzPageCount = 0;
            //listPacket.Items.Clear();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage ep = new ExcelPackage(path);
            ExcelWorksheets sheets = ep.Workbook.Worksheets;
            int sheetIndex = 0;
            foreach (ExcelWorksheet sheet in sheets)
            {
                List<TMParam> tempListParamInSheet = new List<TMParam>();
                TMPacket packet = new TMPacket();
                packet.packetName = sheet.Name;
                packet.packetIndex = sheetIndex;

                if (packet.packetName == "载荷" || packet.packetName == "200hz")
                {
                    packet.packetFreq = 200;
                    packet.packetGroupID = 0;
                }
                if (packet.packetName == "控制")
                {
                    packet.packetFreq = 200;
                    packet.packetGroupID = 0x14;
                }
                else if (packet.packetName.StartsWith("40hz"))
                {
                    packet.packetFreq = 40;
                    packet.packetGroupID = Convert.ToInt32(packet.packetName.Split('_')[1]) - 1;
                }
                else if (packet.packetName.StartsWith("10hz"))
                {
                    _10hzPageCount++;
                    packet.packetFreq = 10;
                    packet.packetGroupID = Convert.ToInt32(packet.packetName.Split('_')[1]) - 1;
                }
                int i = 2;

                while (sheet.Cells[i, 1].Value != null)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        TMParam param = new TMParam();
                        param.paramName = sheet.Cells[i, 1].Value.ToString();
                        // if (param.paramName == "备用")
                        //{
                        //     i++;
                        //     continue;
                        // }
                        if (sheet.Cells[i, 2].Value != null)
                            param.paramID = sheet.Cells[i, 2].Value.ToString();
                        else
                            param.paramID = "";
                        param.paramByteCount = Convert.ToInt32(sheet.Cells[i, 3].Value);
                        string type = sheet.Cells[i, 4].Value.ToString().ToUpper();
                        switch (type)
                        {
                            case "ENUM":
                            case "UINT16":
                                param.paramType = typeof(UInt16);
                                break;
                            case "INT16":
                                param.paramType = typeof(Int16);
                                break;
                            case "UINT32":
                                param.paramType = typeof(UInt32);
                                break;
                            case "INT32":
                                param.paramType = typeof(Int32);
                                break;
                            case "UINT64":
                                param.paramType = typeof(UInt64);
                                break;
                            case "INT64":
                                param.paramType = typeof(Int64);
                                break;
                            case "UINT8":
                                param.paramType = typeof(byte);
                                break;
                            case "INT8":
                                param.paramType = typeof(sbyte);
                                break;
                            case "DOUBLE":
                                param.paramType = typeof(double);
                                break;
                            case "FLOAT":
                                param.paramType = typeof(float);
                                break;
                            default:
                                param.paramType = typeof(UInt32);
                                break;
                        }
                        if (sheet.Cells[i, 5].Value != null)
                            param.paramUnit = sheet.Cells[i, 5].Value.ToString();
                        else
                            param.paramUnit = "";
                        if (sheet.Cells[i, 6].Value != null)
                            param.paramCoefficient = Convert.ToDouble(sheet.Cells[i, 6].Value);
                        else
                            param.paramCoefficient = 1;
                        if (sheet.Cells[i, 7].Value != null)
                            param.paramDataPool = sheet.Cells[i, 7].Value.ToString();
                        else
                            param.paramDataPool = "";
                        param.fomula = sheet.Cells[i, 8].Value.ToString();
                        if (sheet.Cells[i, 9].Value != null)
                            param.detail = sheet.Cells[i, 9].Value.ToString();
                        else
                            param.detail = "";
                        param.packetIndex = packet.packetIndex;
                        //packet.paramList.Add(param);
                        allParamList[k].Add(param);
                        if (k == 0)
                        {
                            TMParam param1 = param.DeepCopy();
                            tempListParamInSheet.Add(param1);
                        }
                    }
                    i++;
                }
                foreach (var item in tempListParamInSheet)
                {
                    packet.paramList.Add(item);
                }

                packet.CalcPacketByte();
                if (packet.packetName == "控制")
                {
                    controlParamByte = packet.packetByteCount;
                }
                else if (packet.packetName == "载荷")
                {
                    payloadParamByte = packet.packetByteCount;
                }
                else if (packet.packetName == "200hz")
                {
                    _200hzParamByte = packet.packetByteCount;
                }
                else if (packet.packetName.StartsWith("40hz"))
                {
                    if (packet.packetByteCount > _40hzParamByte)
                        _40hzParamByte = packet.packetByteCount;
                }
                else
                {
                    if (packet.packetByteCount > _10hzParamByte)
                        _10hzParamByte = packet.packetByteCount;
                }
                packets.Add(packet);
                sheetIndex++;
            }
            viewTM.RefreshData();
            CreateFlightDT();
            
            foreach (TMPacket packet in packets)
            {
                InitDataTables(packet);
            }
            
            // ((MainForm)MdiParent).tmHandler.AutoStartSave();
        }

        private void TMPage_Resize(object sender, EventArgs e)
        {
            panelControl.Location = new Point((panelControl.Parent.Size.Width - panelControl.Size.Width) / 2, 10);
        }

        /********************************************
         * 串口连接
         *********************************************/
        private void EnableUDPButton(bool enable)
        {
           // btConn.Enabled = enable;
            btIPAdress.Enabled = enable;
            //btIDSelect.Enabled = enable;
        }



        //曲线绘制
        

        public void CreateFlightDT()
        {
            FlightDataTable.Columns.Clear();
            FlightDataTable.Columns.Add("second", typeof(double));
            for (int i = 0; i < allParamList[0].Count; i++)
            {
                if (allParamList[0][i].paramName == "备用")
                    continue;
                FlightDataTable.Columns.Add(allParamList[0][i].paramName, typeof(double));
            }
        }

        private void viewTM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //InitChart();

        }

        private void viewTM_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            /*
            int[] rows = viewTM.GetSelectedRows();
            int row = rows[0];
            allParamList[NetDataHandle.curSelMsn][row].showInImage = !allParamList[NetDataHandle.curSelMsn][row].showInImage;
            InitChart();*/
        }

        private void btClearSelection_Click(object sender, EventArgs e)
        {
            /*
            for (int i = 1; i < FlightDataTable.Columns.Count; i++)
            {
                allParamList[NetDataHandle.curSelMsn][i-1].showInImage = false;
            }
            InitChart();
            viewTM.RefreshData();*/
        }

        private void btClearData_Click(object sender, EventArgs e)
        {
            FlightDataTable.Rows.Clear();
        }
        /****************************************************************
         * 定时界面刷新
         ****************************************************************/
        public void RefreshUITMChart()
        {
            
            if (tMFramesByGroupID[0][0].second < 0.3)
            {
                FlightDataTable.Rows.Clear();
            }
            UpdateFrameToGrid();
            UpdateFrameToMap(tMFramesByGroupID[0][0].second);
            
        }

        public void UpdateFrameToEngine()
        {
            /*
            foreach (TMParam param in allParamList[NetDataHandle.curSelMsn])
            {
                if (param.paramID == "mcuV" && param.paramDataPool == "mcu" )
                {
                    tmEngine.powerV = Convert.ToDouble(param.物理量);
                    continue;
                }
                if (param.paramID == "mcuA" && param.paramDataPool == "mcu")
                {
                    tmEngine.powerA = Convert.ToDouble(param.物理量);
                    continue;
                }
                if (param.paramID == "mcuBTemp")
                {
                    tmEngine.tempture = Convert.ToDouble(param.物理量);
                    continue;
                }
                if (param.paramID == "mcuRpm" )
                {
                    tmEngine.rpm = Convert.ToDouble(param.物理量);
                    continue;
                }
                if (param.paramID == "mcuCmd")
                {
                    tmEngine.throttle = Convert.ToDouble(param.物理量);
                    continue;
                }
            }
            tmEngine.RefreshUI();*/
        }

        public void RefreshUI()
        {
            UpdateFrameToEngine();
            
            UpdateFrameToHDU();

            UpdateBJTime();
            /*
            //顶部时间
            if (tMFramesByGroupID[NetDataHandle.curSelMsn][0].second < 1000)
            {
                flightTimeCtrl.Text = tMFramesByGroupID[NetDataHandle.curSelMsn][0].second.ToString("F2");
            }
            else if (tMFramesByGroupID[NetDataHandle.curSelMsn][0].second < 10000)
            {
                flightTimeCtrl.Text = tMFramesByGroupID[NetDataHandle.curSelMsn][0].second.ToString("F1");
            }
            else
            {
                flightTimeCtrl.Text = tMFramesByGroupID[NetDataHandle.curSelMsn][0].second.ToString("F0");
            }
            
            //连接状态刷新
            if (NetDataHandle.udpNode.connectState == false)
            {
                btConn.Text = "UDP已断开";
                btConn.ForeColor = Color.Red;
                EnableUDPButton(true);
            }
            else
            {
                btConn.Text = "UDP已连接";
                btConn.ForeColor = Color.Lime;
                EnableUDPButton(false);

            }
            btIPAdress.Text = NetDataHandle.udpNode.udpIP.ToString();
            btIDSelect.Text = string.Format("ID:{0:D2}", NetDataHandle.curSelMsn + 1);*/
        }

        int insert3DCount = 0;

        //更新地图数据，地图数据需要绘制所有在线的飞机数据
        private void UpdateFrameToMap(double time)
        {
            double[] lon = new double[6];
            double[] lat = new double[6];
            double[] high = new double[6];
            double ptlon = 0;
            double ptlat = 0;
            for (int i = 0; i < 6; i++)
            {
                time = tMFramesByGroupID[i][0].second;
                foreach (TMParam param in allParamList[i])
                {
                    if (param.paramID == "navLon" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                    {
                        lon[i] = Convert.ToDouble(param.物理量);
                        continue;
                    }
                    if (param.paramID == "navLat" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                    {
                        lat[i] = Convert.ToDouble(param.物理量);
                        continue;
                    }
                    if (param.paramID == "navHigh" && (param.paramDataPool == "imu" || param.paramDataPool == "nav"))
                    {
                        high[i] = Convert.ToDouble(param.物理量);
                        continue;
                    }
                    if (param.paramID == "ptLon")
                    {
                        ptlon = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "ptLat")
                    {
                        ptlat = Convert.ToDouble(param.物理量);
                    }
                }
                if (lon[i] != 0)
                {
                    if (tmMap2.RealMapDataTable[i].Rows.Count > 2 && time < (double)(tmMap2.RealMapDataTable[i].Rows[2].ItemArray[0]))
                    {
                        tmMap2.RealMapDataTable[i].Clear();
                        tmTrack3D.ClearLine();
                        //tmTrack3D1.ClearLine();
                    }

                    tmMap2.InsertRealPoint(i, time, lon[i], lat[i], high[i]);
                    tmMap2.InsertTargetPoint(ptlon, ptlat, 0);
                    if (insert3DCount % 5 == 0)
                    {
                        tmTrack3D.InsertRealPoint(time, lon[i], lat[i], high[i]);
                        //tmTrack3D1.InsertRealPoint(time, lon[i], lat[i], high[i]);
                        insert3DCount = 0;
                    }

                    insert3DCount++;
                }
            }
        }

        private void UpdateBJTime()
        {
            /*
            foreach (TMParam param in allParamList[NetDataHandle.curSelMsn])
            {
                if (param.paramID == "BJTime")
                {
                    DateTime baseDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                    // 假设这是从2000年1月1日以来经过的秒数
                    long secondsSince2000 = (long)param.paramSourceCode; // 示例秒数，可替换为实际值

                    // 将秒数转换为TimeSpan
                    TimeSpan timeSpan = TimeSpan.FromSeconds(secondsSince2000);

                    // 计算目标UTC时间
                    DateTime targetDate = baseDate.Add(timeSpan);

                    string dateString = targetDate.ToString("u");
                    dateString = dateString.Substring(0,dateString.Length - 1);
                    labelBJCur.Text = dateString;
                }
                if (param.paramID == "luncTime")
                {
                    DateTime baseDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                    // 假设这是从2000年1月1日以来经过的秒数
                    long secondsSince2000 = (long)param.paramSourceCode; // 示例秒数，可替换为实际值

                    // 将秒数转换为TimeSpan
                    TimeSpan timeSpan = TimeSpan.FromSeconds(secondsSince2000);

                    // 计算目标UTC时间
                    DateTime targetDate = baseDate.Add(timeSpan);

                    string dateString = targetDate.ToString("u");
                    dateString = dateString.Substring(0, dateString.Length - 1);
                    labelBJLuanch.Text = dateString;
                }
            }*/
        }
        private void UpdateFrameToHDU()
        {
            double pitch = 0, dir = 0, roll = 0, high = 0, speed = 0;
            double curAlt = 0, navHigh = 0;
            bool gotCurAlt = false, gotNavHigh = false;
            // TMPage 参数表按 0..5 存副本；界面当前网格固定用 index 0（curSelMsn 已废弃）
            const int msn = 0;
            if (!allParamList.ContainsKey(msn) || allParamList[msn] == null)
                return;

            foreach (TMParam param in allParamList[msn])
            {
                if (param.paramID == "navPitch" && IsNavPool(param.paramDataPool))
                {
                    pitch = Convert.ToDouble(param.物理量);
                    continue;
                }
                if (param.paramID == "navDir" && IsNavPool(param.paramDataPool))
                {
                    dir = Convert.ToDouble(param.物理量);
                    continue;
                }
                if (param.paramID == "navRoll" && IsNavPool(param.paramDataPool))
                {
                    roll = Convert.ToDouble(param.物理量);
                    continue;
                }
                if (param.paramID == "curAlt")
                {
                    curAlt = Convert.ToDouble(param.物理量);
                    gotCurAlt = Math.Abs(curAlt) > 1e-6;
                    continue;
                }
                if (param.paramID == "navHigh" && IsNavPool(param.paramDataPool))
                {
                    navHigh = Convert.ToDouble(param.物理量);
                    gotNavHigh = Math.Abs(navHigh) > 1e-6;
                    continue;
                }
                if (param.paramID == "AirSpd")
                {
                    speed = Convert.ToDouble(param.物理量);
                    continue;
                }
            }

            // 优先当前高度 curAlt，无效时回退导航高度
            if (gotCurAlt)
                high = curAlt;
            else if (gotNavHigh)
                high = navHigh;

            tmhdu2.SetLocation(pitch, dir, roll, high, speed);
        }

        private static bool IsNavPool(string pool)
        {
            return string.Equals(pool, "nav", StringComparison.OrdinalIgnoreCase)
                || string.Equals(pool, "imu", StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateFrameToGrid()
        {//查看目前的页面
            /*
            foreach (TMPacket selpkt in packets)
            {
                TMFrame selframe = tMFramesByGroupID[NetDataHandle.curSelMsn][selpkt.packetGroupID];//控制的groupID是20
                int index = 0;//载荷开始,或控制开始
                if (selpkt.packetName == "200hz")
                {
                    index = payloadParamByte;
                  //  index = controlParamByte;
                }
                if (selpkt.packetFreq == 40)
                {
                    index = payloadParamByte + _200hzParamByte;
                }
                if (selpkt.packetFreq == 10)
                {
                    index = payloadParamByte + _200hzParamByte + _40hzParamByte;
                }
                foreach (TMParam param in allParamList[NetDataHandle.curSelMsn])
                {
                    if (param.packetIndex != selpkt.packetIndex)
                    {
                        continue;
                    }
                    if (param.paramByteCount == 1)
                    {
                        param.paramSourceCode = selframe.payLoad[index];
                    }
                    else if (param.paramByteCount == 2)
                    {
                        param.paramSourceCode = BitConverter.ToUInt16(selframe.payLoad, index);
                    }
                    else if (param.paramByteCount == 4)
                    {
                        param.paramSourceCode = BitConverter.ToUInt32(selframe.payLoad, index);
                    }
                    else if (param.paramByteCount == 8)
                    {
                        param.paramSourceCode = BitConverter.ToUInt64(selframe.payLoad, index);
                    }
                    index += param.paramByteCount;
                }
                
            }
            PacketToDataTable(tMFramesByGroupID[NetDataHandle.curSelMsn][0].second);
            viewTM.RefreshData();*/
        }

        public void PacketToDataTable(double second)
        {
            /*
            DataTable dt = FlightDataTable;
            if(dt.Rows.Count > 1) 
            {
                if (FlightDataTable.Rows[0][0].ToString() == "0")
                {
                    FlightDataTable.Rows.RemoveAt(0);
                }
            }
            


            DataRow dr = dt.NewRow();

            List<object> objlist = new List<object>();
            objlist.Add(second);
            foreach (TMParam param in allParamList[NetDataHandle.curSelMsn])
            {
                if (param.paramName == "备用")
                    continue;
                objlist.Add(param.物理量);
            }

            object[] rowArray = objlist.ToArray();
            dr.ItemArray = rowArray;
            dt.Rows.Add(dr);*/
        }

        private void btSaveExcel_Click(object sender, EventArgs e)
        {
            SaveTablesToFile();
        }

        //紧急伞降
        private void btEmgrecyUnbrl_Click(object sender, EventArgs e)
        {
            Byte[] data = new Byte[200];
            for (int i = 0; i < 200; i++)
            {
                data[i] = 0xCC;
            }
           // NetDataHandle.Send_To_TM(200, 0x22, data);
        }

        //紧急回收
        private void btEmgrecyRecycle_Click(object sender, EventArgs e)
        {
            Byte[] data = new Byte[1];
            //NetDataHandle.Send_To_TM(0, 0x23, data);
        }

        private void btConn_Click_1(object sender, EventArgs e)
        {
            if (NetDataHandle.udpNode.connectState)
            {//如果连接，则断开
                NetDataHandle.udpNode.connectState = false;
                NetDataHandle.StopReceiveUdpData();
                NetDataHandle.udpNode.udpClient.Close();
                EnableUDPButton(true);
                // NetDataHandle.udpNode.udpClient.  DataReceived -= ComDataHandle.CmdPortCmdRecvData;
                btConn.Text = "UDP已断开";
                btConn.ForeColor = Color.Red;
            }
            else
            {//如果没连接则连接
                NetDataHandle.udpNode.udpIP = btIPAdress.Text;
                NetDataHandle.udpNode.udpPort = 60000;
                try
                {
                    if (NetDataHandle.udpNode.udpClient.Client == null)
                        NetDataHandle.udpNode.udpClient = new UdpClient();
                    NetDataHandle.udpNode.udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse(NetDataHandle.udpNode.udpIP), NetDataHandle.udpNode.udpPort));
                    NetDataHandle.udpNode.connectState = true;
                    EnableUDPButton(false);
                    btConn.Text = "UDP已连接";
                    btConn.ForeColor = Color.Lime;
                    NetDataHandle.StartReceiveUdpData();
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void btIPAdress_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NetDataHandle.udps.Count; i++)
            {
                if (btIPAdress.Text == NetDataHandle.udps[i])
                {
                    if (i + 1 < NetDataHandle.udps.Count)
                    {
                        btIPAdress.Text = NetDataHandle.udps[i + 1];
                        NetDataHandle.udpNode.udpIP = btIPAdress.Text;
                    }
                    else
                    {
                        btIPAdress.Text = NetDataHandle.udps[0];
                        NetDataHandle.udpNode.udpIP = btIPAdress.Text;
                    }
                    break;
                }
            }
        }

        private void btIDSelect_Click(object sender, EventArgs e)
        {
            /*
            NetDataHandle.curSelMsn++;
            if (NetDataHandle.curSelMsn == 6)
                NetDataHandle.curSelMsn = 0;
            string text = string.Format("ID:{0:D2}", NetDataHandle.curSelMsn + 1);
            btIDSelect.Text = text;

            InitGrid(NetDataHandle.curSelMsn);*/
        }
    }
}
