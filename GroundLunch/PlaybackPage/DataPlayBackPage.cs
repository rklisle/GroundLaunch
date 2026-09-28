using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using OfficeOpenXml;
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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet; // 如果操作 Excel 文件
using System.Xml;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using DocumentFormat.OpenXml.Wordprocessing;
using MathNet.Numerics.RootFinding;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using GroundLunch.PlaybackPage;

namespace GroundLunch
{
    public partial class DataPlayBackPage : DevExpress.XtraEditors.XtraUserControl
    {
        public bool pageSelected = false;
        public List<string> paramNameList = new List<string>();
        TreeListNode selNode = new TreeListNode();
        DataTable dt = new DataTable();
        DataTable dtForSaveDatFile = new DataTable();
        public List<TMParam> allParamList = new List<TMParam>();
        AnalyzeDatForm analyzeForm = new AnalyzeDatForm();
        public DataPlayBackPage()
        {
            InitializeComponent();
            InitFileTree();
            
        }

        public void InitFileTree()
        {
            treeListFile.Columns.Add();
            treeListFile.Columns[0].Caption = "文件结构";

            DisplayDirectoryStructure(".\\", null, treeListFile);
        }

        private int DisplayDirectoryStructure(string path, TreeListNode parentNode, TreeList treeList)
        {
            if (path.Contains("$"))
            {
                return 0;
            }
            try
            {

                // 添加当前目录
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                TreeListNode directoryNode = treeList.AppendNode(new object[] { directoryInfo.Name }, parentNode);
                //directoryNode.StateImageIndex = 0;
                directoryNode.StateImageIndex = 0;
                directoryNode.Tag = directoryInfo.FullName;

                // 添加子目录
                int excelCount = 0;
                foreach (var subDirectory in directoryInfo.GetDirectories())
                {
                    int folderExcelCount = DisplayDirectoryStructure(subDirectory.FullName, directoryNode, treeList);
                    excelCount += folderExcelCount;
                }


                // 添加文件
                int fileCount = excelCount;
                foreach (var file in directoryInfo.GetFiles())
                {
                    // 仅显示 Excel 文件
                    if (file.Extension.Equals(".xls", StringComparison.OrdinalIgnoreCase) ||
                        file.Extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                        file.Extension.Equals(".dat", StringComparison.OrdinalIgnoreCase))
                    {
                        if (file.Name.Contains("$"))
                            continue;
                        TreeListNode fileNode = treeList.AppendNode(new object[] { file.Name }, directoryNode);
                        fileNode.StateImageIndex = 1;
                        fileNode.Tag = file.FullName;
                        fileCount++;
                    }
                }
                if (fileCount == 0)
                {
                    treeList.DeleteNode(directoryNode);
                }
                return fileCount;
            }
            catch (UnauthorizedAccessException)
            {
                // 忽略无权限访问的目录
                return 0;
            }
        }

        private void DataPlayBackPage_Load(object sender, EventArgs e)
        {
            treeListFile.Refresh();
            treeListFile.ExpandAll();
        }
        private void treeListFile_RowCellClick(object sender, DevExpress.XtraTreeList.RowCellClickEventArgs e)
        {
            TreeListNode tempNode = treeListFile.FocusedNode;
            string path = tempNode.Tag.ToString();
            if (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx")
            {
                if (AnalyzeExcel(path) != -1)
                {
                    selNode.StateImageIndex = 1;
                    selNode = tempNode;
                    selNode.StateImageIndex = 2;
                }
            }
            if (Path.GetExtension(path).ToUpper() == ".DAT" )
            {
                
                analyzeForm.func = AnalyzeDatFast;
                analyzeForm.fileName = path;
                analyzeForm.ShowDialog();
                //AnalyzeDat(path);
               
            }
        }

        public int AnalyzeDatFast(string path)
        {
            try
            {
                //准备待写入csv文件
                string csvName = "DatFileTM" + new FileInfo(path).CreationTime.ToString("yyyyMMddHHmmss") + ".csv";

                analyzeForm.csvFileName = csvName;
                StreamWriter writer = new StreamWriter(csvName, false, Encoding.UTF8);
                string csvHead = "second,";
                foreach (TMParam pam in TMHandler.allParamList[(1,1)])
                {
                    csvHead = csvHead + pam.paramName + ",";
                }
                csvHead = csvHead.Remove(csvHead.Length - 1);
                // csvHead += "\r\n";
                writer.WriteLine(csvHead);
                //csv文件头已经准备好

                //开始解析文件，每解1行写入一次
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    allParamList.Clear();
                    
                    foreach (var item in TMHandler.allParamList[(1,1)])
                    {
                        allParamList.Add(item);
                    }
                    
                    Byte[] unhandledTmBuf = reader.ReadBytes((int)fs.Length);
                    int curPos = 0;
                    //先计算包长度
                    int frameLen = 0;
                    foreach (TMParam param in allParamList)
                    {
                        frameLen += param.paramByteCount;
                    }
                    //计算需要处理的总包数
                    double packetCount = (double)unhandledTmBuf.Length / (frameLen + 5);
                    analyzeForm.analyzeProgress = 0;
                    int curPacket = 0;

                    int frameLenAndTick = frameLen + 5;
                    while (curPos < unhandledTmBuf.Length - frameLenAndTick - 100)//少解最后一包避免出现错误
                    {
                        analyzeForm.analyzeProgress = curPacket/packetCount;
                        curPacket++;
                        curPos += 1;//groupID
                        TMFrame selframe = new TMFrame();
                        byte[] bytetick = new byte[4];
                        bytetick[0] = unhandledTmBuf[curPos++];
                        bytetick[1] = unhandledTmBuf[curPos++];
                        bytetick[2] = unhandledTmBuf[curPos++];
                        bytetick[3] = unhandledTmBuf[curPos++];
                        selframe.tick = BitConverter.ToUInt32(bytetick, 0);
                        selframe.second = selframe.tick * 0.0001;
                        for (int i = 0; i < frameLen; i++)
                        {
                            selframe.payLoad[i] = unhandledTmBuf[curPos++];
                        }
                        int index = 0;
                        foreach (TMParam param in allParamList)
                        {
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
                        string content = selframe.second.ToString() + ",";
                        foreach (var item1 in  allParamList)
                        {
                            content = content + item1.物理量 + ",";
                        }
                        content = content.Remove(content.Length - 1);
                        //content += "\r\n";
                        writer.WriteLine(content);
                    }
                }
                writer.Close();
            }
            catch (Exception ex) 
            {
                ;
            }
            return 0;
        }

        public int AnalyzeDat(string path)
        {
            //progressAnalyzeDat.Visible = true;
            //dat文件不包含列名称，是SD卡读取的遥测源码，需要将隔壁页面的表头搞过来
            dtForSaveDatFile.Columns.Clear();
            dtForSaveDatFile.Columns.Add("second", typeof(double));
            allParamList.Clear();
            /*
            foreach (var item in tmPage.allParamList[0])
            {
                allParamList.Add(item);
                if (item.paramName == "备用")
                        continue;
                dtForSaveDatFile.Columns.Add(item.paramName, typeof(double));
            }
            */
            //不在界面上显示先，直接存成excel
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                int curPos = 0;
                try
                {
                    //一次性读出所有文件
                    Byte[] unhandledTmBuf = reader.ReadBytes((int)fs.Length);
                    analyzeForm.fileSize = fs.Length;
                    analyzeForm.analyzeProgress = 1;
                    //不需要双缓冲了，直接一个一个字节读
                    // curPos = (172+5) * (200 * 60 * 52);//包字节数*200hz*60秒*40分钟，丢弃前54分钟数据
                    //curPos += 107342042;
                    //curPos += (172 + 5) * (200 * 60 * 35);

                    //除了最后一包外，每一包都解，从包中判断发射时刻
                    //包长度从隔壁读取的数据计算得来
                    int jumpSecond = 3000;
                    int endSecond = 5000;
                    //先计算包长度
                    int frameLen = 0;
                    foreach (TMParam param in allParamList)
                    {
                        frameLen += param.paramByteCount;
                    }
                    int frameLenAndTick = frameLen + 5;
                    curPos += frameLenAndTick * 200 * jumpSecond;
                    while (curPos < unhandledTmBuf.Length - frameLenAndTick - 100)//少解最后一包避免出现错误
                    {
                        curPos += 1;//groupID
                        TMFrame selframe = new TMFrame();
                        byte[] bytetick = new byte[4];
                        bytetick[0] = unhandledTmBuf[curPos++];
                        bytetick[1] = unhandledTmBuf[curPos++];
                        bytetick[2] = unhandledTmBuf[curPos++];
                        bytetick[3] = unhandledTmBuf[curPos++];
                        selframe.tick = BitConverter.ToUInt32(bytetick, 0);
                        selframe.second = selframe.tick * 0.0001;

                       
                        for (int i = 0; i < frameLen; i++)
                        {
                            selframe.payLoad[i] = unhandledTmBuf[curPos++];
                        }
                        int index = 0;
                        foreach (TMParam param in allParamList)
                        {
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
                        //至此，一帧解析完成
                        DataRow dr = dtForSaveDatFile.NewRow();
                        List<object> objlist = new List<object>();
                        objlist.Add(selframe.second);
                        foreach (TMParam param in allParamList)
                        {
                            if (param.paramName == "备用")
                                continue;
                            objlist.Add(param.物理量);//param.源码值HEX + " " + 
                        }
                        object[] rowArray = objlist.ToArray();
                        dr.ItemArray = rowArray;
                        dtForSaveDatFile.Rows.Add(dr);
                        if (curPos > frameLenAndTick * 200 * endSecond)
                            break;
                    }
                    //至此，所有数据解析完成
                    analyzeForm.analyzeProgress = 30;
                    //判断发射时刻
                    int luanchRow = 0;
                    int index1 = 0;
                    foreach(DataRow dr in dtForSaveDatFile.Rows) 
                    {
                        double flightTime = (double)dr.ItemArray[0];
                        if (flightTime < 0.005)
                        {
                            luanchRow = index1;
                            break;
                        }
                        index1++;
                    }
                    //删除发射时刻前的数据
                    for(int i=0;i<luanchRow; i++)
                    { 
                        dtForSaveDatFile.Rows.RemoveAt(0);
                    }
                    //写入csv文件
                    string csvName = "DatFileTM" + new FileInfo(path).CreationTime.ToString("yyyyMMddHHmmss") + ".csv";
                    StreamWriter writer = new StreamWriter(csvName, false, Encoding.UTF8);
                    string csvHead = "";
                    DataTable dt1 = dtForSaveDatFile;
                    foreach (DataColumn col in dt1.Columns)
                    {
                        csvHead = csvHead + col.ColumnName + ",";
                    }
                    csvHead = csvHead.Remove(csvHead.Length - 1);
                   // csvHead += "\r\n";
                    writer.WriteLine(csvHead);

                    //开始写入内容
                   
                    foreach (DataRow dr in dt1.Rows)
                    {
                        string content = "";
                        foreach (var item1 in dr.ItemArray)
                        {
                            content = content + item1 + ",";
                        }
                        content = content.Remove(content.Length - 1);
                        //content += "\r\n";
                        writer.WriteLine(content);
                    }
                    writer.Close();
                    /*写入excel文件
                    string excelName = "DatFileTM" + new FileInfo(path).CreationTime.ToString("yyyyMMddHHmmss") + ".xlsx";
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelPackage ep = new ExcelPackage(excelName);
                    ExcelWorksheets sheets = ep.Workbook.Worksheets;
                    int pktIndex = 0;
                    //foreach (var item in dtForSaveDatFile)
                    {
                        DataTable dt = dtForSaveDatFile;
                        ExcelWorksheet curSheet; 
                        if (sheets.Count > 0)
                            curSheet = sheets[0];
                        else
                            curSheet = sheets.Add("200hz");

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
                    */
                    analyzeForm.analyzeProgress = 100;
                }
                catch (EndOfStreamException e)
                {
                    Console.WriteLine("Reached the end of the stream: " + e.Message);
                }
                catch (IOException e)
                {
                    Console.WriteLine("I/O Error: " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected error: " + e.Message);
                }
            }

            return 0;
        }

        public int AnalyzeExcel(string path)
        {
            paramNameList.Clear();
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, false))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First(); // 假设只有一个工作表

                OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);
                int rowIndex = 0;
                while (reader.Read())
                {
                    
                    if (reader.ElementType == typeof(Row))
                    {
                        
                        Row row = (Row)reader.LoadCurrentElement();
                        int cellIndex = 0;
                        foreach (Cell cell in row.Elements<Cell>())
                        {
                            string cellValue = GetCellValue(cell, workbookPart);
                            if (rowIndex == 0 && cellIndex == 0)
                            {
                                if (cellValue != "second")
                                {
                                    return -1;
                                }
                                checkListParams.Items.Clear();
                            }
                            if (rowIndex == 0)
                            {
                                if (cellIndex != 0)
                                {
                                    paramNameList.Add(cellValue);
                                    checkListParams.Items.Add(cellValue);
                                }
                            }
                            else
                            {
                                return 0;
                            }
                            cellIndex++;
                        }
                        rowIndex++;
                    }
                }
            }
            return -1;
        }

        static string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                SharedStringTablePart sharedStringTablePart = workbookPart.SharedStringTablePart;
                if (sharedStringTablePart.SharedStringTable != null)
                {
                    return sharedStringTablePart.SharedStringTable.ElementAt(int.Parse(cell.InnerText)).InnerText;
                }
            }
            return cell.InnerText;
        }

        private void checkListParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = checkListParams.SelectedIndex;
            checkListParams.SetItemChecked(index, !checkListParams.GetItemChecked(index));
            UpdateSelGrid();
        }

        
        private void UpdateSelGrid()
        {
            dt.Columns.Clear();
            dt.Columns.Add("second");
            for (int i=0;i<checkListParams.Items.Count;i++) 
            {
                if (checkListParams.GetItemChecked(i))
                {
                    dt.Columns.Add(checkListParams.Items[i].ToString());
                }
            }

            gridSelParam.DataSource = dt;
            viewSelParam.PopulateColumns(); // 自动创建列
            for(int i=0;i<viewSelParam.Columns.Count;i++)
            {
                viewSelParam.Columns[i].Width = (int)(gridSelParam.Width * 0.1);
            }

        }

        
    }
}
