using DevExpress.XtraCharts;
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
using DevExpress.XtraEditors;
using static GroundLunch.GroundProtocol;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using System.IO.Ports;
using DevExpress.XtraPrinting.Native;

namespace GroundLunch
{
    public partial class UpLoadPage : DevExpress.XtraEditors.XtraUserControl
    {
        public bool pageSelected = false;
        //List<CurFileInRocket> curFileList = new List<CurFileInRocket>();
        List<fileInLocalFolder> fileInFolderList = new List<fileInLocalFolder>();
        string defaultFolder;
        const UInt16 packetLength = 200;
        uint curFileSize = 0;
        public UpLoadPage()
        {
            InitializeComponent();
            InitLocalGrid();
        }

        private void UpLoadPage_Load(object sender, EventArgs e)
        {
            AutoGetFileList();
            InitCurGrid();
        }



        private void UpLoadPage_Resize(object sender, EventArgs e)
        {
            panelControl.Location = new Point((panelControl.Parent.Size.Width - panelControl.Size.Width) / 2, 10);
        }

        /***********************************************
         * 本地文件夹待烧写列表
         * 
         ***********************************************/

        private void InitLocalGrid() 
        {
            

            gridLocalFolder.DataSource = fileInFolderList;

            viewLocalFolder.Columns["文件类型"].Group();

            viewLocalFolder.Columns[0].Width = 5 * gridLocalFolder.Width / 100;
            viewLocalFolder.Columns[0].OptionsColumn.AllowEdit = false;
            viewLocalFolder.Columns[1].Width = 65 * gridLocalFolder.Width / 100;
            viewLocalFolder.Columns[1].ColumnEdit = ItemButtonChangeFile;
            viewLocalFolder.Columns[2].Width = 10 * gridLocalFolder.Width / 100;
            viewLocalFolder.Columns[2].OptionsColumn.AllowEdit = false;
            viewLocalFolder.Columns[3].Width = 15 * gridLocalFolder.Width / 100;
            viewLocalFolder.Columns[3].OptionsColumn.AllowEdit = false;
            viewLocalFolder.Columns[4].Width = 5 * gridLocalFolder.Width / 100;
           // viewLocalFolder.Columns[5].Width = 10 * gridLocalFolder.Width / 100;

            //viewLocalFolder.Columns[4].ColumnEdit = ItemButtonChangeFile;
            //viewLocalFolder.Columns[4].OptionsColumn.AllowEdit = true;
            viewLocalFolder.Columns[4].ColumnEdit = ItemButtonSend;
            //viewLocalFolder.Columns[4].OptionsColumn.AllowEdit = true;

           
        }

        //当文件列表刷新后，手动更新gridview
        private void UpdateLocalFileGrid()
        {
            viewLocalFolder.RefreshData();
        }

        //从指定文件夹路径获取文件列表
        private void GetFileList(string folder)
        {
            if(Directory.Exists(folder) == false) return;
            Byte fileIndex = 0;
            string[] files = Directory.GetFiles(folder);
            foreach (string file in files) 
            {
                fileInLocalFolder bfile = new fileInLocalFolder();
                bfile.filePath = file;
                bfile.fileSize = (uint)(new FileInfo(file).Length);
                bfile.fileCreateDate = new FileInfo(file).LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                string extName = Path.GetExtension(bfile.filePath);
                if (extName == ".dat")
                {
                    bfile.fileIndex = fileIndex;
                    fileIndex++;
                    bfile.fileType = 0;
                }
                else if (extName == ".a0")
                {
                    bfile.fileIndex = 0xA0;
                    bfile.fileType = 1;
                }
                else if (extName == ".b0")
                {
                    bfile.fileIndex = 0xB0;
                    bfile.fileType = 2;
                }
                else if (extName == ".b1")
                {
                    bfile.fileIndex = 0xB1;
                    bfile.fileType = 2;
                }
                else
                {
                    continue;
                }
                fileInFolderList.Add(bfile);
            }
            UpdateLocalFileGrid();
            
        }

        //加载后自动从默认文件夹读取文件
        private void AutoGetFileList()
        {
            string folderPath = System.AppDomain.CurrentDomain.BaseDirectory + "flashFile\\";
            defaultFolder = folderPath;
            GetFileList(folderPath);
        }

        //当文件夹内容更改后，无需重启，直接刷新文件夹
        private void btUpdateFlashFileFolder_Click(object sender, EventArgs e)
        {
            fileInFolderList.Clear();
            GetFileList(defaultFolder);
            UpdateLocalFileGrid();

        }

        //不使用默认路径，更换为其它文件夹
        private void btChangeFlashFileFolder_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog.Description = "请选择文件夹";
            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                fileInFolderList.Clear();
                string selectedFolder = FolderBrowserDialog.SelectedPath;
                GetFileList(selectedFolder);
                UpdateLocalFileGrid();
                defaultFolder = selectedFolder;
            }
        }

        private void ItemButtonChangeFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int []rows = viewLocalFolder.GetSelectedRows();
            int row = rows[0];
            fileInLocalFolder gridRow = (fileInLocalFolder)viewLocalFolder.GetRow(row);

            openFileDialog.Title = "选择要替换的文件";
            switch (gridRow.fileType)
            {
                case 0:
                    openFileDialog.Filter = "发射诸元 (*.dat)|*.dat";
                    break;
                case 1:
                    openFileDialog.Filter = "模飞文件 (*.a0)|*.a0";
                    break;
                case 2:
                    openFileDialog.Filter = "应用程序 (*.b0;*.b1)|*.b0;*.b1";
                    break;
            }
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                for (int i = 0; i < fileInFolderList.Count; i++)
                {
                    if (gridRow.filePath == fileInFolderList[i].filePath)
                    {
                        fileInFolderList[i].filePath = selectedFilePath;
                        fileInFolderList[i].fileSize = (uint)(new FileInfo(selectedFilePath).Length);
                        fileInFolderList[i].fileCreateDate = new FileInfo(selectedFilePath).CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    }
                }
            }
            else
            {

            }
            UpdateLocalFileGrid();
            viewLocalFolder.RefreshRow(row);
        }

        //烧写
        private void ItemButtonSend_Click(object sender, EventArgs e)
        {
            int[] rows = viewLocalFolder.GetSelectedRows();
            int row = rows[0];
            fileInLocalFolder gridRow = (fileInLocalFolder)viewLocalFolder.GetRow(row);
           

            //1.计算所需包数，每包250字节
            int packetCount = (int)gridRow.fileSize / packetLength + (gridRow.fileSize % packetLength == 0 ? 0 : 1);
            UpLoadFileState.totalCount = (ushort)packetCount;
            UpLoadFileState.state = 0;
            UpLoadFileState.progress = 0;
            UpLoadFileState.curIndex = 1;

            //2.更新界面
            labelPacketProgress.Text = string.Format("1 / {0}", packetCount);
            progressUpLoad.Position = 0;
            btUpLoadTips.Text = string.Format("{0} 烧写中",Path.GetFileName(gridRow.filePath));
            btUpLoadTips.ForeColor = Color.Yellow;
            btUpLoadTips.Visible = true;

            //3.交烧写进程
            Thread newThread = new Thread(new ParameterizedThreadStart(ThreadUploadFile));
            newThread.Start(gridRow);

            //4.禁止点击其它烧写按钮
            gridLocalFolder.Enabled = false;
            btChangeFlashFileFolder.Enabled = false;
            btUpdateFlashFileFolder.Enabled = false;
        }

        //仅烧写中可看到可点击
        private void btUpLoadTips_Click(object sender, EventArgs e)
        {
            btUpLoadTips.Text = btUpLoadTips.Text.Replace("烧写中", "烧写停止");
            btUpLoadTips.ForeColor = Color.Red;
            gridLocalFolder.Enabled = true;
            btChangeFlashFileFolder.Enabled = true;
            btUpdateFlashFileFolder.Enabled = true;
            UpLoadFileState.state = 6;
            //发送停止烧写到飞控
            Byte[] ctrlPacket = new Byte[0x8A];//纯数据区长度
            ctrlPacket[0] = 0x22;//退出烧写
            if (btProgramBoard.Text == "主控板")
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x8A, 0x60, ctrlPacket);
            else
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x8A, 0x70, ctrlPacket);

        }

        //烧写线程
        private void ThreadUploadFile(object inData)
        {
            fileInLocalFolder fileInfo = (fileInLocalFolder)inData;
            Byte[] fileData = File.ReadAllBytes(fileInfo.filePath);
            curFileSize = fileInfo.fileSize;
            //1.先发送首包(烧写控制请求)

            Byte[] ctrlPacket = new Byte[0x8A];//纯数据区长度
            ctrlPacket[0] = 0x11;//进入烧写
            ctrlPacket[1] = fileInfo.fileIndex;
            Byte[] fileSize = BitConverter.GetBytes(fileInfo.fileSize);
            Buffer.BlockCopy(fileSize, 0, ctrlPacket, 2, 4);
            Byte[] crcResult = BitConverter.GetBytes(Crc32.CalCRC32(fileData, fileInfo.fileSize));
            Buffer.BlockCopy(crcResult, 0, ctrlPacket, 6, 4);

            byte[] fileName = Encoding.UTF8.GetBytes(Path.GetFileName(fileInfo.filePath));
            Buffer.BlockCopy(fileName, 0, ctrlPacket, 10, fileName.Length);

            byte[] fileDate = Encoding.UTF8.GetBytes(fileInfo.fileCreateDate);
            Buffer.BlockCopy(fileDate, 0, ctrlPacket, 74, fileDate.Length);

            UpLoadFileState.uploadMark = false;
            UpLoadFileState.state = 1;
            UpLoadFileState.curIndex = 0;
            if(btProgramBoard.Text == "主控板")
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x8A, 0x60, ctrlPacket);
            else
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x8A, 0x70, ctrlPacket);

            //2.循环判断标志，发送下一包
            long overTimeCount = 0;
            Byte[] UnsuccessPacket = new Byte[packetLength + 6];
            int UnsuccessPacketLen = 0;
            while (UpLoadFileState.state == 1)
            {
                Byte[] DataPacket = new Byte[packetLength + 6];
                UInt16 effectLen = 0;
                //超时判断
                if (UpLoadFileState.uploadMark == false)
                {
                    Thread.Sleep(1);
                    overTimeCount++;
                    uint totalCount = curFileSize / 10 + 1000;
                    if (overTimeCount == totalCount)
                    {
                        UpLoadFileState.state = 5;
                    }
                    if ( overTimeCount %100 == 0)
                    {
                        //发出后未收到反馈
                        //重新发送
                        if (btProgramBoard.Text == "主控板")
                            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, (UInt16)(UnsuccessPacketLen), 0x62, UnsuccessPacket);
                        else
                            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, (UInt16)(UnsuccessPacketLen), 0x72, UnsuccessPacket);
                    }
                }
                //发送数据包
                else
                {
                    overTimeCount = 0;
                    
                    Buffer.BlockCopy(BitConverter.GetBytes(UpLoadFileState.totalCount), 0, DataPacket, 0, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes(UpLoadFileState.curIndex), 0, DataPacket, 2, 2);

                    effectLen = (UInt16)((fileInfo.fileSize - UpLoadFileState.curIndex * packetLength > packetLength) ? packetLength : (fileInfo.fileSize - UpLoadFileState.curIndex * packetLength));
                    Buffer.BlockCopy(BitConverter.GetBytes(effectLen), 0, DataPacket, 4, 2);
                    Buffer.BlockCopy(fileData, packetLength * UpLoadFileState.curIndex, DataPacket, 6, effectLen);

                    UpLoadFileState.uploadMark = false;

                    if (btProgramBoard.Text == "主控板")
                        NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, (UInt16)(effectLen + 6), 0x62, DataPacket);
                    else
                        NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, (UInt16)(effectLen + 6), 0x72, DataPacket);

                    UnsuccessPacketLen = effectLen + 6;
                    Buffer.BlockCopy(DataPacket, 0, UnsuccessPacket, 0, UnsuccessPacketLen);
                    
                }
            }
            //发送完成后发送校验包
            if (UpLoadFileState.state == 2)
            {
                if (btProgramBoard.Text == "主控板")
                    NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 4, 0x66, crcResult);
                else
                    NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 4, 0x76, crcResult);

            }
        }

        /***********************************************
         * 刷新当前已烧写列表
         * 
         ***********************************************/

        private void InitCurGrid()
        {
            //InitTestList();
            gridCtrlCur.DataSource = null;
            gridCtrlCur.DataSource = NetDataHandle.planeFileList[(NetDataHandle.curPao, NetDataHandle.curGuan)];

            gridViewCur.Columns["文件类型"].Group();
            
            gridViewCur.Columns[0].Width = 10 * gridCtrlCur.Width / 100;
            gridViewCur.Columns[1].Width = 40 * gridCtrlCur.Width / 100;
            gridViewCur.Columns[2].Width = 17 * gridCtrlCur.Width / 100;
            gridViewCur.Columns[3].Width = 33 * gridCtrlCur.Width / 100;
        }

        private void InitTestList()
        {
            
            
        }

        /**************************************************
         * 刷新界面
         * 主要刷新串口连接数据
         *
         *************************************************/
        private void EnableUDPButton(bool enable)
        {
           // btBuad.Enabled = enable;
            btIPAdress.Enabled = enable;
            //btCom.Enabled = enable;
        }
        public void RefreshUI()
        {
            gridViewCur.RefreshData();
            //刷新自己的内容
            if (NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].flightTime < 1000)
            {
                flightTimeCtrl.Text = NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].flightTime.ToString("F2");
            }
            else if (NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].flightTime < 10000)
            {
                flightTimeCtrl.Text = NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].flightTime.ToString("F1");
            }
            else
            {
                flightTimeCtrl.Text = NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].flightTime.ToString("F0");
            }

            //连接状态刷新
            if (NetDataHandle.udpNode.connectState == false)
            {
                btConn.Text = "UDP已断开";
                btConn.ForeColor = Color.Red;
                btIPAdress.Enabled = true;
            }
            else
            {
                btConn.Text = "UDP已连接";
                btConn.ForeColor = Color.Lime;
                btIPAdress.Enabled = false;

            }

            btIPAdress.Text = NetDataHandle.udpNode.udpIP.ToString();
            btPaoSelect.Text = string.Format("{0:D2}", NetDataHandle.curPao);
            btGuanSelect.Text = string.Format("{0:D2}", NetDataHandle.curGuan);
            //刷新烧写进度条
            if (UpLoadFileState.state != 6 && UpLoadFileState.state != 7)
            {
                UpLoadFileState.progress = ((double)(UpLoadFileState.curIndex ) / (UpLoadFileState.totalCount - 1.0)) * 100.0;
                progressUpLoad.Position = (int)UpLoadFileState.progress;
                labelPacketProgress.Text = string.Format("{0} / {1}", UpLoadFileState.curIndex + 1, UpLoadFileState.totalCount);
                switch(UpLoadFileState.state) 
                {
                    case 0:
                    case 1:
                        btUpLoadTips.Text = btUpLoadTips.Text.Split(' ')[0] + " 烧写中";
                        btUpLoadTips.ForeColor = Color.Yellow;
                        break;
                    case 2:
                        btUpLoadTips.Text = btUpLoadTips.Text.Split(' ')[0] + " 校验中";
                        btUpLoadTips.ForeColor = Color.Yellow;
                        break;
                    case 3:
                        btUpLoadTips.Text = btUpLoadTips.Text.Split(' ')[0] + " 校验成功";
                        btUpLoadTips.ForeColor = Color.Lime;
                        btChangeFlashFileFolder.Enabled = true;
                        btUpdateFlashFileFolder.Enabled = true;
                        btGetCurFileList.Enabled = true;
                        gridLocalFolder.Enabled = true;
                        UpLoadFileState.state = 7;
                        break;
                    case 4:
                        btUpLoadTips.Text = btUpLoadTips.Text.Split(' ')[0] + " 校验失败";
                        btUpLoadTips.ForeColor = Color.Red;
                        UpLoadFileState.state = 7;
                        break;
                    case 5:
                        btUpLoadTips.Text = btUpLoadTips.Text.Split(' ')[0] + " 超时";
                        btUpLoadTips.ForeColor = Color.Red;
                        UpLoadFileState.state = 7;
                        break;

                }
            }

            if (UpLoadFileState.clearState == 2)
            {
                NetDataHandle.planeFileList[(NetDataHandle.curPao, NetDataHandle.curGuan)].Clear();
                Byte[] data = new byte[8];
                if (btProgramBoard.Text == "主控板")
                    NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x0, 0x64, data);
                else
                    NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x0, 0x74, data);
                UpLoadFileState.clearState = 0;
            }
            else if (UpLoadFileState.clearState == 1)
            {
                btClearFlash.Enabled = false;
                btGetCurFileList.Enabled = false;
            }
            else
            {
                btClearFlash.Enabled = true;
                btGetCurFileList.Enabled = true;
            }
        }

        //获取已烧写至智能控制器的诸元文件列表
        private void btGetCurFileList_Click(object sender, EventArgs e)
        {
            InitCurGrid();
            NetDataHandle.planeFileList[(NetDataHandle.curPao, NetDataHandle.curGuan)].Clear();
            Byte[] data = new byte[8];
            if (btProgramBoard.Text == "主控板")
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x0, 0x64, data);
            else
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x0, 0x74, data);

        }

        private void btClearFlash_Click(object sender, EventArgs e)
        {
            Byte[] data = new byte[8];
            if (btProgramBoard.Text == "主控板")
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x0, 0x6A, data);
            else
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 0x0, 0x7A, data);
            UpLoadFileState.clearState = 1;
        }

        private void btProgramBoard_Click(object sender, EventArgs e)
        {
            if (btProgramBoard.Text == "主控板")
            {
                btProgramBoard.Text = "导航板";
            }
            else if (btProgramBoard.Text == "导航板")
            {
                btProgramBoard.Text = "主控板";
            }
        }
    }
}
