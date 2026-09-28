using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Native;
using DocumentFormat.OpenXml.InkML;
using DevExpress.Utils;
using System.Security.Cryptography;

namespace GroundLunch
{
    public partial class LuanchPage : DevExpress.XtraEditors.XtraUserControl
    {
        public MainForm mainForm;
        private Timer LuanchTimer = new Timer();
        public LuanchPage()
        {
            InitializeComponent();
            NetDataHandle.InitNet();
            NetDataHandle.InitData();
            
        }
        public LuanchPage(MainForm mform)
        {
            InitializeComponent();
            NetDataHandle.InitNet();
            NetDataHandle.InitData();
            mainForm = mform;
            msnShowAndSelect1.mainForm = mainForm;
        }


        public void RefreshUI()
        {
            Paoche1.RefreshUI(1);

            msnShowAndSelect1.ReFreshUI();
            DataInterface.viewPlane.ignationTime = NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao,NetDataHandle.curGuan)].flightTime;
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
            //版本号刷新
            labelVersion.Text = new string(NetDataHandle.curSelPlaneTMFrame[(NetDataHandle.curPao, NetDataHandle.curGuan)].version);

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

        private void LuanchPage_Load(object sender, EventArgs e)
        {
            if (NetDataHandle.udps.Count > 0)
            {
                btIPAdress.Text = NetDataHandle.udps[0];
            }
            else
            {
                btIPAdress.Text = "127.0.0.1";
            }
            
        }

        public void ConnToUDPPort()
        {
            
        }

        private void btConn_Click(object sender, EventArgs e)
        {
            if (NetDataHandle.udpNode.connectState)
            {//如果连接，则断开
                NetDataHandle.udpNode.connectState = false;
                NetDataHandle.StopReceiveUdpData();
                NetDataHandle.udpNode.udpClient.Close();
                NetDataHandle.udpDataLinkNode.udpClient.Close();
                btIPAdress.Enabled = true;
                // NetDataHandle.udpNode.udpClient.  DataReceived -= ComDataHandle.CmdPortCmdRecvData;
                btConn.Text = "UDP已断开";
                btConn.ForeColor = Color.Red;
            }
            else
            {//如果没连接则连接
                NetDataHandle.udpNode.udpIP = btIPAdress.Text;
                NetDataHandle.udpNode.udpPort = 39702;
                NetDataHandle.udpDataLinkNode.udpIP = btIPAdress.Text;
                NetDataHandle.udpDataLinkNode.udpPort = 39703;
                try
                {
                    if (NetDataHandle.udpNode.udpClient.Client == null)
                        NetDataHandle.udpNode.udpClient = new UdpClient();
                    if (NetDataHandle.udpDataLinkNode.udpClient.Client == null)
                        NetDataHandle.udpDataLinkNode.udpClient = new UdpClient();
                    NetDataHandle.udpNode.udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse(NetDataHandle.udpNode.udpIP), NetDataHandle.udpNode.udpPort));
                    NetDataHandle.udpDataLinkNode.udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse(NetDataHandle.udpDataLinkNode.udpIP), NetDataHandle.udpDataLinkNode.udpPort));
                    NetDataHandle.udpNode.connectState = true;
                    NetDataHandle.udpDataLinkNode.connectState = true;
                    btIPAdress.Enabled = false;
                    btConn.Text = "UDP已连接";
                    btConn.ForeColor = Color.Lime;
                    NetDataHandle.StartReceiveUdpData();
                    foreach (var item in FormDataLink.linkTerminals)
                    {
                        NetDataHandle.Send_To_DataLink(0, item.TerminalID, 1);
                    }
                    // 起飞前对时，否则 DoIgnition 锁存的 luncTime 为 0
                    if (NetDataHandle.curPao > 0 && NetDataHandle.curGuan > 0)
                        NetDataHandle.SendBjTimeSet(NetDataHandle.curPao, NetDataHandle.curGuan);
                    else
                        NetDataHandle.SendBjTimeSet(1, 1);
                    
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }
        private void btEngineStart_Click(object sender, EventArgs e)
        {
            foreach (MsnFlightGroup group in msnShowAndSelect1.missionFiles[msnShowAndSelect1.selectedMissionIndex].flightGroups)
            {
                foreach (MsnPlane plane in group.planes)
                {
                    byte[] data = new byte[1];
                    NetDataHandle.Send_To_TM(plane.paoID, plane.guanID, (ushort)(0), 0xF6, data);
                }
            }
        }

        private void btEngineStop_Click(object sender, EventArgs e)
        {
            foreach (MsnFlightGroup group in msnShowAndSelect1.missionFiles[msnShowAndSelect1.selectedMissionIndex].flightGroups)
            {
                foreach (MsnPlane plane in group.planes)
                {
                    byte[] data = new byte[1];
                    NetDataHandle.Send_To_TM(plane.paoID, plane.guanID, (ushort)(0), 0xF7, data);
                }
            }
        }

        private void btUnlockAll_Click(object sender, EventArgs e)
        {
            foreach (MsnFlightGroup group in msnShowAndSelect1.missionFiles[msnShowAndSelect1.selectedMissionIndex].flightGroups)
            {
                foreach (MsnPlane plane in group.planes)
                {
                    //先发预发射指令
                   // byte[] data0 = new byte[1];
                   // data0[0] = 0x11;
                   // NetDataHandle.Send_To_TM(plane.paoID, plane.guanID, (ushort)(1), 0xF8, data0);
                    //再发发射指令
                    byte[] data = new byte[5];
                    data[0] = (byte)0xAA;
                    data[1] = (byte)0xBB;
                    data[2] = (byte)0xCC;
                    data[3] = (byte)0xDD;
                    data[4] = (byte)0xEE;
                    NetDataHandle.Send_To_TM(plane.paoID, plane.guanID, (ushort)(5), 0xFA, data);
                }
            }
        }

        private void btDoIgnation_Click(object sender, EventArgs e)
        {
            int luanchMode = comboLuanchMode.SelectedIndex;
            if (luanchMode == 0)//单发 
            {
                //单发需要记录飞机是否已经被发射，下次点击时需要发射未发射飞机
                foreach (MsnFlightGroup group in msnShowAndSelect1.missionFiles[msnShowAndSelect1.selectedMissionIndex].flightGroups)
                {
                    foreach (MsnPlane plane in group.planes)
                    {
                        //if (plane.luanched == 0)
                        {
                            FireRocket(plane.paoID, plane.guanID);
                            plane.luanched = 1;
                        }
                    }
                }
            }
            else if (luanchMode == 1) //连发
            {
                LuanchTimer.Interval = 2000;
                LuanchTimer.Tick += new EventHandler(OnTimerLuanch);
                LuanchTimer.Start();
            }
            else if (luanchMode == 2)//齐射
            {
                foreach (MsnFlightGroup group in msnShowAndSelect1.missionFiles[msnShowAndSelect1.selectedMissionIndex].flightGroups)
                {
                    foreach (MsnPlane plane in group.planes)
                    {
                        FireRocket(plane.paoID, plane.guanID);
                        plane.luanched = 1; 
                    }
                }
            }
        }

        private void OnTimerLuanch(object sender, EventArgs e)
        {
            int find = 0;
            foreach (MsnFlightGroup group in msnShowAndSelect1.missionFiles[msnShowAndSelect1.selectedMissionIndex].flightGroups)
            {
                foreach (MsnPlane plane in group.planes)
                {
                    if (plane.luanched == 0)
                    {
                        FireRocket(plane.paoID, plane.guanID);
                        plane.luanched = 1;
                        find = 1;
                    }
                }
            }
            if (find == 0) 
            {
                LuanchTimer.Stop();
            }
        }

        private void FireRocket(int paoID, int guanID)
        {
            //真实情况未知
            byte[] data = new byte[5];
            data[0] = (byte)0xAA;
            data[1] = (byte)0xBB;
            data[2] = (byte)0xCC;
            data[3] = (byte)0xDD;
            data[4] = (byte)0xEE;
            NetDataHandle.Send_To_TM(paoID, guanID, (ushort)(5), 0xFB, data);


            //往仿真发送
            byte[] data1 = new byte[1];
            NetDataHandle.Send_To_TM(paoID, guanID, (ushort)(1), 0xFC, data1);
        }


    }
}
