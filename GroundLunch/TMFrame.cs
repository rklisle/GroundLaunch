using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.XtraScheduler.Drawing;
using OfficeOpenXml;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroundLunch
{
    public class TMFrame : CommonFrame
    {
        public double flightTime;
        public char[] version = new char[11];
     
        //public int frameLen = 0;
        //public int frameGroup = -1;
        // public Byte[] data = new byte[2048];//包含帧头及校验和
        //public Byte[] payLoad = new byte[1024];
        //  public ushort dataLen = 0;
        //  public UInt32 tick;
        // public double second;
        //  public int seq;
        //   public int dev;
        //  public int msgID;
        //    public Byte[] crc16check = new byte[2];
        public override bool AnalyzeFrame()
        {
            //将data转义到帧格式
            byte[] byteLen = new byte[2];
            byteLen[0] = data[2];
            byteLen[1] = data[3];

            dataLen = BitConverter.ToUInt16(byteLen, 0);
            if (dataLen > 300)
            {
                return false;
            }
            seq = data[4];
            // dev = data[5];
            msgID = data[5];
            if (msgID == 0x99)
            {
                int a = 0;
            }
            frameGroup = data[6];
            byte[] bytetick = new byte[4];
            bytetick[0] = data[7];
            bytetick[1] = data[8];
            bytetick[2] = data[9];
            bytetick[3] = data[10];
            tick = BitConverter.ToUInt32(bytetick, 0);
            second = tick * 0.0001;
            flightTime = second;
            crc16check[0] = data[6 + dataLen];
            crc16check[1] = data[7 + dataLen];

            ushort check = NetDataHandle.crc16_ccitt(data, (dataLen + 6));
            byte checkA, checkB;
            checkA = (byte)(check & 0xFF);
            checkB = (byte)(0xFF & (check >> 8));
            //校验和不通过，将数据长度置为0即可
            if (checkA != crc16check[0] || checkB != crc16check[1])
            {
                dataLen = 0;
                return false;
            }
            for (int i = 0; i < dataLen; i++)
            {
                payLoad[i] = data[i + 11];
            }
            return true;
        }
    }

    static public class TMHandler
    {
        static public List<TMPacket> packets = new List<TMPacket>();
        static public Dictionary<(int,int), List<TMParam>> allParamList = new Dictionary<(int,int), List<TMParam>>();
        static public Dictionary<(int,int), DataTable> FlightDataTable = new Dictionary<(int,int), DataTable>();
        static public Dictionary<(int, int), DataTable> FlightDataTableForView = new Dictionary<(int, int), DataTable>();
        static public int controlParamByte = 0;
     
        static TMHandler() 
        {
           
        }

        static public void InitTMHandler()
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    allParamList[(i, j)] = new List<TMParam>();
                }
            }
            AutoLoadExcel();
        }

        static public void AutoLoadExcel()
        {
            AnalyzeExcel(".\\遥测\\遥测.xlsx");
        }

        static public void AnalyzeExcel(string path)
        {
            packets.Clear();
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    allParamList[(i, j)].Clear();
                }
            }
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
                    packet.packetFreq = 10;
                    packet.packetGroupID = Convert.ToInt32(packet.packetName.Split('_')[1]) - 1;
                }
                int i = 2;

                while (sheet.Cells[i, 1].Value != null)
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
                    for (int m = 0; m <= 8; m++)
                    {
                        for (int n = 0; n <= 12; n++)
                        {
                            TMParam tMParam = param.DeepCopy();
                            allParamList[(m, n)].Add(tMParam); 
                        }
                    }
                    TMParam param1 = param.DeepCopy();
                    tempListParamInSheet.Add(param1);
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
                packets.Add(packet);
                sheetIndex++;
            }
            CreateFlightDT();
        }

        public static void ClearFlightDT(int paoID, int guanID)
        {
            TMHandler.FlightDataTable[(paoID,guanID)].Clear();
            FlightDataTableForView[(paoID, guanID)].Clear();
        }

        public static void SaveTablesToFile()
        {
            DateTime t = DateTime.Now;
            string excelName = "TMDetail" + t.Year + t.Month + t.Day + t.Hour + t.Minute + t.Second + ".xlsx";
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage ep = new ExcelPackage(excelName);
            ExcelWorksheets sheets = ep.Workbook.Worksheets;
            for (int m = 1; m <= 8; m++)
            {
                for (int n = 1; n <= 12; n++) 
                {
                    if (TMHandler.FlightDataTable[(m, n)].Rows.Count > 0) 
                    {
                        DataTable dt = TMHandler.FlightDataTable[(m, n)];
                        //当有数据时，创建标签页
                        ExcelWorksheet curSheet;
                        curSheet = sheets.Add(string.Format("pao{0:D2}-guan{1:D2}", m, n));

                        int colIndex = 1;
                        foreach (DataColumn col in dt.Columns)
                        {
                            curSheet.Cells[1, colIndex].Value = col.ToString();
                            colIndex++;
                        }
                        colIndex = 1;

                        int rowIndex = 2;
                        int rowsCount = dt.Rows.Count;
                        for (int i = 0; i < rowsCount; i++) 
                        {
                            DataRow dr = dt.Rows[i];
                            foreach (var item1 in dr.ItemArray)
                            {
                                curSheet.Cells[rowIndex, colIndex].Value = item1;
                                colIndex++;
                            }
                            colIndex = 1;
                            rowIndex++;
                        }
                    }
                }
            }
            ep.Save();
        }

        static public void FrameToAllParamList(TMFrame frame, int paoID, int guanID)
        {
            TMFrame selframe = frame;//控制的groupID是20
            int index = 0;//载荷开始,或控制开始
            if (frame.msgID == 0x9A)
            {
                int a = 0;
            }
            foreach (TMParam param in allParamList[(paoID,guanID)])
            {
                if (frame.msgID == 0x99 && param.packetIndex == 1)
                {
                    continue;
                }
                if (frame.msgID == 0x9A && param.packetIndex == 2)
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
            AllParamListToDataTable(selframe.second, paoID, guanID);
        }

        static public void CreateFlightDT()
        {

            for (int m = 0; m <= 8; m++)
            {
                for (int n = 0; n <= 12; n++)
                {
                    FlightDataTable[(m, n)] = new DataTable();
                    FlightDataTable[(m, n)].Columns.Clear();
                    FlightDataTable[(m, n)].Columns.Add("second", typeof(double));


                    FlightDataTableForView[(m, n)] = new DataTable();
                    FlightDataTableForView[(m, n)].Columns.Clear();
                    FlightDataTableForView[(m, n)].Columns.Add("second", typeof(double));
                    
                    for (int i = 0; i < allParamList[(m, n)].Count; i++)
                    {
                        if (allParamList[(m, n)][i].paramName == "备用")
                            continue;
                        FlightDataTable[(m, n)].Columns.Add(allParamList[(m, n)][i].paramName, typeof(double));
                        FlightDataTableForView[(m, n)].Columns.Add(allParamList[(m, n)][i].paramName, typeof(double));
                    }
                }
            }
        }

        public static ConcurrentQueue<object[]> uiPendingRows = new ConcurrentQueue<object[]>(); // 
        static public void AllParamListToDataTable(double second, int paoID, int guanID)
        {
            DataTable dt = FlightDataTable[(paoID,guanID)];
            if (dt.Rows.Count > 1)
            {
                if (FlightDataTable[(paoID, guanID)].Rows[0][0].ToString() == "0")
                {
                    FlightDataTable[(paoID, guanID)].Rows.RemoveAt(0);
                }
            }
            DataRow dr = dt.NewRow();
            List<object> objlist = new List<object>();
            objlist.Add(second);
            foreach (TMParam param in allParamList[(paoID, guanID)])
            {
                if (param.paramName == "备用")
                    continue;
                objlist.Add(param.物理量);
            }
            object[] rowArray = objlist.ToArray();
            dr.ItemArray = rowArray;
            //dt.ImportRow(dr);
            dt.Rows.Add(dr);
            uiPendingRows.Enqueue(rowArray);     
        }

        static public double GetValueByParamID(int paoID, int guanID, string paramID)
        {
            foreach (TMParam param in allParamList[(paoID, guanID)])
            {
                if (param.paramID == paramID)
                {
                    double val = Convert.ToDouble(param.物理量);
                    return val;
                }
            }
            return 0;
        }
    }
}
