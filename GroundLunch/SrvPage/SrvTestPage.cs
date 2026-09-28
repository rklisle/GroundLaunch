using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Media;
using System.Xml.Linq;

namespace GroundLunch
{
    public partial class SrvTestPage : DevExpress.XtraEditors.XtraUserControl
    {
        public bool pageSelected = false;
        public DataTable srvDataTable = new DataTable();
        System.Windows.Forms.Timer freshTimer = new System.Windows.Forms.Timer();
        public SrvTestPage()
        {
            InitializeComponent();
        }

        public void InitFreshTimer()
        {
            freshTimer.Interval = 50;
            freshTimer.Tick += new EventHandler(OnTimerFresh);
            freshTimer.Start();
        }

        private void OnTimerFresh(object sender, EventArgs e)
        {
            UpdatePacketToUI();
        }

        private void btSetAll_Click(object sender, EventArgs e)
        {
            Byte[] data = new Byte[14];
            short[] srv = new short[7];
            srv[0] = (short)(((double)spin1MM.Value) * 100);
            srv[1] = (short)(((double)spin2MM.Value) * 100);
            srv[2] = (short)(((double)spin3MM.Value) * 100);
            srv[3] = (short)(((double)spin4MM.Value) * 100);
            srv[4] = (short)(((double)spin5MM.Value) * 100);
            srv[5] = (short)(((double)spin6MM.Value) * 100);
            srv[6] = (short)(((double)spin7MM.Value) * 100);
            for (int i = 0; i < srv.Length; i++)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(srv[i]), 0, data, i * 2, 2);
            }
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 14, 0x42, data);
        }

        private void btSetSrvZero_Click(object sender, EventArgs e)
        {
            Byte[] data = new Byte[14];
            short[] srv = new short[7];
            srv[0] = (short)(((double)spin1MM.Value) * 100);
            srv[1] = (short)(((double)spin2MM.Value) * 100);
            srv[2] = (short)(((double)spin3MM.Value) * 100);
            srv[3] = (short)(((double)spin4MM.Value) * 100);
            srv[4] = (short)(((double)spin5MM.Value) * 100);
            srv[5] = (short)(((double)spin6MM.Value) * 100);
            srv[6] = (short)(((double)spin7MM.Value) * 100);
            for (int i = 0; i < srv.Length; i++)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(srv[i]), 0, data, i * 2, 2);
            }
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 14, 0x48, data);
        }

        private void SrvTestPage_Load(object sender, EventArgs e)
        {
            InitSrvDataTable();
            InitChart();
           
        }

        private void SrvTestPage_Resize(object sender, EventArgs e)
        {
            panelControl.Location = new Point((panelControl.Parent.Size.Width - panelControl.Size.Width) / 2, 10);
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
           // btCom.Enabled = enable;
        }
        public void RefreshUI()
        {
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
                EnableUDPButton(true);
            }
            else
            {
                btConn.Text = "UDP已连接";
                btConn.ForeColor = Color.Lime;
                EnableUDPButton(false);

            }

            btIPAdress.Text = NetDataHandle.udpNode.udpIP.ToString();
            btPaoSelect.Text = string.Format("{0:D2}", NetDataHandle.curPao);
            btGuanSelect.Text = string.Format("{0:D2}", NetDataHandle.curGuan);

            UpdateFrameToChart();
        }

        /**********************************************
         *串口连接
         **********************************************/

        private void btConn_Click(object sender, EventArgs e)
        {
            /*
            if (ComDataHandle.comNode.connectState)
            {//如果连接，则断开
                ComDataHandle.comNode.connectState = false;
                ComDataHandle.comNode.serialPort.Close();
                Enable422Button(true);
                ComDataHandle.comNode.serialPort.DataReceived -= ComDataHandle.CmdPortCmdRecvData;
                btConn.Text = "422已断开";
                btConn.ForeColor = Color.Red;
            }
            else
            {//如果没连接则连接
                ComDataHandle.comNode.serialPort.BaudRate = ComDataHandle.comNode.comBaud;
                ComDataHandle.comNode.serialPort.PortName = ComDataHandle.comNode.comName;
                ComDataHandle.comNode.serialPort.Parity = ComDataHandle.comNode.comCheck ? Parity.Odd : Parity.None;
                ComDataHandle.comNode.serialPort.DataBits = 8;
                ComDataHandle.comNode.serialPort.StopBits = StopBits.One;
                try
                {
                    ComDataHandle.comNode.serialPort.Open();
                    ComDataHandle.comNode.connectState = true;
                    Enable422Button(false);
                    btConn.Text = "422已连接";
                    btConn.ForeColor = Color.Lime;
                    ComDataHandle.comNode.serialPort.DataReceived += new SerialDataReceivedEventHandler(ComDataHandle.CmdPortCmdRecvData);
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message);
                }
            }*/
        }

        private void btCheckMode_Click(object sender, EventArgs e)
        {
            /*
            if (btCheckMode.Text == "奇校验")
            {
                btCheckMode.Text = "无校验";
                ComDataHandle.comNode.comCheck = false;
            }
            else if (btCheckMode.Text == "无校验")
            {
                btCheckMode.Text = "奇校验";
                ComDataHandle.comNode.comCheck = true;
            }
            */
        }

        private void btBuad_Click(object sender, EventArgs e)
        {
            /*
            if (btBuad.Text == "921600")
            {
                btBuad.Text = "115200";
                ComDataHandle.comNode.comBaud = 115200;
            }
            else if (btBuad.Text == "115200")
            {
                btBuad.Text = "921600";
                ComDataHandle.comNode.comBaud = 921600;
            }
            */
        }

        private void btCom_Click(object sender, EventArgs e)
        {
            /*
            if (btCom.Text == "COM-")
            {
                ComDataHandle.InitComs();
                if (ComDataHandle.coms.Length > 0)
                {
                    btCom.Text = ComDataHandle.coms[0];
                }
                else
                {
                    btCom.Text = "COM-";
                }
                return;
            }
            for (int i = 0; i < ComDataHandle.coms.Length; i++)
            {
                if (btCom.Text == ComDataHandle.coms[i])
                {
                    if (i + 1 < ComDataHandle.coms.Length)
                    {
                        btCom.Text = ComDataHandle.coms[i + 1];
                        ComDataHandle.comNode.comName = ComDataHandle.coms[i + 1];
                    }
                    else
                    {
                        btCom.Text = ComDataHandle.coms[0];
                        ComDataHandle.comNode.comName = ComDataHandle.coms[0];
                    }
                    break;
                }
            }*/
        }

        private void btSeqEnable_Click(object sender, EventArgs e)
        {
            if (btSeqEnable.Text == "时序使能中")
            {

            }
            else
            {
                btSeqEnable.Text = "时序使能中";
                btSeqEnable.ForeColor = Color.Lime;
               // NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightMode = 0x11;
                //NetDataHandle.Send_To_FK(8, 0x01, null);
            }
        }

        /**********************************************
         * 曲线图
         **********************************************/
        private void InitSrvDataTable()
        {
            srvDataTable.Columns.Clear();
            srvDataTable.Columns.Add("second", typeof(double));
            for(int i = 0; i < 8; i++) 
            {
                string name = string.Format("舵机{0}控制(mm)", i + 1);
                srvDataTable.Columns.Add(name, typeof(double));
            }
            for (int i = 0; i < 8; i++)
            {
                string name = string.Format("舵机{0}反馈(mm)", i + 1);
                srvDataTable.Columns.Add(name, typeof(double));
            }
            srvDataTable.Columns.Add("时序板电压(V)", typeof(double));
            srvDataTable.Columns.Add("时序板电流(A)", typeof(double));
        }

        private void InitChart()
        {
            chartSrv.Series.Clear();
            for (int i = 1; i < srvDataTable.Columns.Count; i++)
            {
                Series se = new Series(srvDataTable.Columns[i].ColumnName, ViewType.Line);
                se.DataSource = srvDataTable;
                se.ArgumentDataMember = "second";
                se.ValueDataMembers.AddRange(srvDataTable.Columns[i].ColumnName);
                chartSrv.Series.Add(se);
            }
        }

        private void UpdateFrameToChart()
        {
            while (srvDataTable.Rows.Count > 0 &&
            NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].flightTime - Convert.ToDouble(srvDataTable.Rows[0].ItemArray[0].ToString()) >= 10.0)
            {
                srvDataTable.Rows.RemoveAt(0);
            }
            if (srvDataTable.Rows.Count > 0 && NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].flightTime - Convert.ToDouble(srvDataTable.Rows[0].ItemArray[0].ToString()) < 0.0)
            {
                srvDataTable.Rows.Clear();
            }

            DataRow dr = srvDataTable.NewRow();
            List<object> objlist = new List<object>();
            objlist.Add(NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].flightTime.ToString("0.###"));
            for(int i=0;i<8;i++) 
            {
                //objlist.Add(NetDataHandle.curSelPlaneTMFrame.srvCtrl[i]);
            }
            for (int i = 0; i < 8; i++)
            {
                //objlist.Add(NetDataHandle.curSelPlaneTMFrame.srvCur[i]);
            }
            //objlist.Add(NetDataHandle.curSelPlaneTMFrame.powerV[2]);
            //objlist.Add(NetDataHandle.curSelPlaneTMFrame.powerA[2]);

            object[] rowArray = objlist.ToArray();
            dr.ItemArray = rowArray;
            srvDataTable.Rows.Add(dr);

        }

        private void btReadSD_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            Point location = control.PointToScreen(new Point(control.Width / 2, control.Height / 2));
            popupMenu1.ShowPopup(location);
           // Byte[] data = new byte[8];
            //data[0] = 3;
            //NetDataHandle.Send_To_FK(1, 0x50, data);
        }

        private void btReadSD_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void btGPSOld_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 7;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btPowerOld_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 6;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btNavOld_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 9;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btMEMSOld_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 8;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btTMOld_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 5;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btGPSNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 2;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btPowerNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 1;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btNavNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 4;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btMEMSNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 3;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        private void btTMNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Byte[] data = new byte[8];
            data[0] = 0;
            NetDataHandle.Send_To_TM(1, 1, 1, 0x50, data);
        }

        //伺服小回路
        private void btStartCycle_Click(object sender, EventArgs e)
        {
            ushort srvHz = (ushort)((double)spinCycleHz.Value * 100);
            ushort width = (ushort)((double)spinCycleMM.Value * 10); 
            ushort count = (ushort)(spinCycleCount.Value);
            int[] enable = new int[7];
            enable[0] = checkSrv1.Checked ? 1 : 0;
            enable[1] = checkSrv2.Checked ? 1 : 0;
            enable[2] = checkSrv3.Checked ? 1 : 0;
            enable[3] = checkSrv4.Checked ? 1 : 0;
            enable[4] = checkSrv5.Checked ? 1 : 0;
            enable[5] = checkSrv6.Checked ? 1 : 0;
            enable[6] = checkSrv7.Checked ? 1 : 0;
            int a = enable[6] * 64 + enable[5] * 32 + enable[4] * 16 + enable[3] * 8 + enable[2] * 4 + enable[1] * 2 + enable[0];
            Byte enableByte = (Byte)a;

            Byte[] data = new Byte[20];
            Byte[] temp = new Byte[2];
            temp = BitConverter.GetBytes(srvHz);
            Buffer.BlockCopy(temp, 0, data, 0, 2);
            temp = BitConverter.GetBytes(width);
            Buffer.BlockCopy(temp, 0, data, 2, 2);
            temp = BitConverter.GetBytes(count);
            Buffer.BlockCopy(temp, 0, data, 4, 2);
            data[6] = enableByte;
            data[7] = 0;
            data[8] = 0;

            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 9, 0x44, data);
        }

        
        //时序板输出测试
        private void btSeqTest_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1];
            if (seqWidth.SelectedIndex == 0)//上电
            {
                data[0] = (byte)(Convert.ToByte(seqChannel.Text));
            }
            else//下电
            {
                data[0] = (byte)((Convert.ToByte(seqChannel.Text)) * 16);
            }
            
           // data[0] = (byte)(Convert.ToByte(seqChannel.Text));
           //Byte[] temp = new Byte[2];
           // temp = BitConverter.GetBytes(Convert.ToUInt16(seqWidth.Text));
            //data[1] = temp[0];
           // data[2] = temp[1];
            NetDataHandle.Send_To_TM(1, 1, 1, 0x05, data);
        }

        //时序板使能
        private void btSeqOn_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1];
            NetDataHandle.Send_To_TM(1, 1, 0, 0xEE, data);
        }

        private void labelControl16_Click(object sender, EventArgs e)
        {

        }



        private void btSeqTest_Click_1(object sender, EventArgs e)
        {
            byte[] data = new byte[3];
            data[0] = (byte)(Convert.ToByte(seqChannel.Text) - 1);
            Byte[] temp = new Byte[2];
            temp = BitConverter.GetBytes(Convert.ToUInt16(seqWidth.Text.Replace("ms", "").Replace("s", "000")));
            data[1] = temp[0];
            data[2] = temp[1];
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 3,0xEF,data);
        }

        private void btPwrOpen_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            data[0] = (byte)(Convert.ToByte(comboPwr.SelectedIndex) + 1);

            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0x05, data);
        }

        private void btPwrClose_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            data[0] = (byte)(Convert.ToByte(comboPwr.SelectedIndex) + 1);
            data[0] = (byte)(data[0] << 4);// + 0xF);
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0x05, data);
        }

        private void btFocus_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0xE0, data);
        }

        private void btToNav_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0xE2, data);

        }

        private void btToLuanch_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0xE3, data);

        }

        private void UpdatePacketToUI()
        {
            RUNNING_INFO running_info = new RUNNING_INFO();
            unsafe
            {
                fixed (byte* pSrc = &NetDataHandle._30Packet[0])
                {
                    running_info = *(RUNNING_INFO*)pSrc;
                }
            }
            switch (running_info.runningStatus)
            {
                case 0:
                    textInfoStatus.Text = "停机";
                    break;
                case 1:
                    textInfoStatus.Text = "启动中";
                    break;
                case 2:
                    textInfoStatus.Text = "散热";
                    break;
                case 3:
                    textInfoStatus.Text = "故障";
                    break;
                case 5:
                    textInfoStatus.Text = "运行";
                    break;
                default:
                    textInfoStatus.Text = running_info.runningStatus.ToString();
                    break;
            }

            switch (running_info.error)
            {
                case 0:
                    textInfoError.Text = "无异常";
                    break;
                case 1:
                    textInfoError.Text = "启动失败";
                    break;                               
            }
            textInfoID.Text = running_info.id.ToString();
            textInfoSetRpm.Text = running_info.settingRpm.ToString();
            textInfoGetRpm.Text = running_info.curRpm.ToString();
            textInfoTemp.Text = running_info.temp.ToString("F1");
            textInfoSecond.Text = running_info.runningSecond.ToString();
            textInfoBattV.Text = running_info.battV.ToString("F1");
            textInfoBattA.Text = running_info.battA.ToString("F1");
            textInfoPumpV.Text = running_info.pumpV.ToString("F1");
            textInfoPa.Text = running_info.Pa.ToString("F0");
            textInfoPumpRpm.Text = running_info.pumpRpm.ToString();
            textInfoOilCconsum.Text = running_info.fuelConsum.ToString();
            textInfoOutputW.Text = running_info.ouputV.ToString();
            textInfoOutputV.Text = running_info.outputW.ToString("F1"); 

            START_PARAM start_param = new START_PARAM();
            unsafe
            {
                fixed (byte* pSrc = &NetDataHandle._31Packet[0])
                {
                    start_param = *(START_PARAM*)pSrc;
                }
            }
            textStartSlavePwm.Text = start_param.pwmSlaver.ToString();
            textStartMasterPwm.Text = start_param.pwmMaster.ToString();
            textStartDelay.Text = start_param.startDelay.ToString();
            textStartPumpV.Text = start_param.pumpV.ToString("F1");
            textStartFireRpm.Text = start_param.startFireRpm.ToString();
            textStartHotRpm.Text = start_param.hotEngineRpm.ToString();
            textStartDetachRpm.Text = start_param.engineDetachRpm.ToString();
            textStartDropRpm.Text = start_param.dropPower.ToString();
            textStartSpd.Text = start_param.startSpeed.ToString("F2");
            textStartFireV.Text = start_param.fireV.ToString("F1");
            textStartAccHot.Text = start_param.isAccHot.ToString();
            textStartStable.Text = start_param.eleStableParam.ToString();
            textStartHotSecond.Text = start_param.fireSecond.ToString();

            RUNNGING_PARAM running_param = new RUNNGING_PARAM();
            unsafe
            {
                fixed (byte* pSrc = &NetDataHandle._32Packet[0])
                {
                    running_param = *(RUNNGING_PARAM*)pSrc;
                }
            }
            textParamLowRpm.Text = running_param.slowRpm.ToString();
            textParamStopRpm.Text = running_param.stopRpm.ToString();
            textParamColdRpm.Text = running_param.coldRpm.ToString();
            textParamAccRecord.Text = running_param.accRecord.ToString();
            textParamDeAcc.Text = running_param.deAccRecord.ToString();
            textParamPumpMaxV.Text = running_param.pumpMaxV.ToString("F1");
            textParamLowV.Text = running_param.battLowV.ToString("F1");
            textParamMaxTemp.Text = running_param.maxTemp.ToString();
            textParamPumpSpd.Text = running_param.pumpSpd.ToString();
            textParamFlutClib.Text = running_param.flutCliab.ToString();
            textParamTotalMinite.Text = running_param.totalMinite.ToString();
            textParamVersion.Text = running_param.version.ToString();
            textParamMaxRpm.Text = running_param.maxRpm.ToString();
            textParamStartCount.Text = running_param.startCount.ToString();
        }

        private void btEnginePowerOn_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            data[0] = (byte)(5 + 1);
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0x05, data);
        }

        private void btEnginePowerOff_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[2];
            data[0] = (byte)(5 + 1);
            data[0] = (byte)(data[0] << 4);// + 0xF);
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0x05, data);
        }

        private void btEngineStart_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1];
            data[0] = 0x11;
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0xC1, data);
        }

        private void btEngineStop_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1];
            data[0] = 0x22;
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0xC1, data);
        }

        private void btGetRunningParam_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1];
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0xC3, data);
        }

        private void btGetStartParam_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1];
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0xC4, data);
        }

        private void toggleRunInfo_Toggled(object sender, EventArgs e)
        {
            int status = toggleRunInfo.IsOn ? 0x11 : 0x22;
            byte[] data = new byte[1];
            data[0] = (byte)status;
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 1, 0xC2, data);

            if (toggleRunInfo.IsOn)
            {
                InitFreshTimer();
            }
            else
            {
                freshTimer.Stop();
            }
        }

        private void btSetRpm_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[4];
            UInt32 rpm = Convert.ToUInt32(textSetRpm.Text);
            data = BitConverter.GetBytes(rpm);
            NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, 4, 0xC5, data);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RUNNING_INFO
    {
        public Byte head; //0xFF
        public Byte head1;// 0x0
        public Byte runningStatus;
        public Byte error;
        public UInt32 id;
        public UInt32 settingRpm;
        public UInt32 curRpm;
        public float temp;
        public UInt32 runningSecond;
        public float battV;
        public float battA;
        public float pumpV;
        public float Pa;
        public UInt16 pumpRpm;
        public UInt16 fuelConsum;
        public float ouputV;
        public UInt32 outputW;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RUNNGING_PARAM
    {
        public Byte head;
        public Byte head1;
        public UInt16 slowRpm;
        public UInt16 stopRpm;
        public UInt16 coldRpm;
        public UInt16 accRecord;
        public UInt16 deAccRecord;
        public float pumpMaxV;
        public float battLowV;
        public UInt16 maxTemp;
        public UInt16 pumpSpd;
        public UInt16 flutCliab;
        public UInt16 totalMinite;
        public float version;
        public UInt32 maxRpm;
        public UInt32 startCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct START_PARAM
    {
        public Byte head;
        public Byte pwmSlaver;
        public Byte pwmMaster;
        public Byte startDelay;
        public float pumpV;
        public UInt16 startFireRpm;
        public UInt16 hotEngineRpm;
        public UInt16 engineDetachRpm;
        public UInt16 dropPower;
        public float startSpeed;
        public float fireV;
        public Byte isAccHot;
        public Byte eleStableParam;
        public Byte fireSecond;
    }
}
