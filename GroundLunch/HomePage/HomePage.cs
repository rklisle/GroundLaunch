using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GroundLunch
{
    public partial class HomePage : DevExpress.XtraEditors.XtraUserControl
    {
        public bool pageSelected;
                
        public HomePage()
        {
            InitializeComponent();
            InitPowerDev();
            NetDataHandle.InitNet();
            NetDataHandle.InitData();
        }

        private void InitPowerDev()
        {
            modDevPowerCombin.powerIndex = 4;
            modDevPowerCombin.devName = "合路供电";

            modDevPowerBatt.powerIndex = 1;
            modDevPowerBatt.devName = "主电池";

            modDevPowerSrv.powerIndex = 2;
            modDevPowerSrv.devName = "12V舵系统";

            modDevPowerFuse.powerIndex = 3;
            modDevPowerFuse.devName = "地面供电";
        }

        private void HomePage_Load(object sender, EventArgs e)
        {

            if (NetDataHandle.udps.Count > 0)
            {
                btIPAdress.Text = NetDataHandle.udps[0];
            }
            else
            {
                btIPAdress.Text = "127.0.0.1";
            }
            //RefreshUI();
        }

        private void HomePage_Resize(object sender, EventArgs e)
        {
            panelControl.Location = new Point((panelControl.Parent.Size.Width - panelControl.Size.Width) /2, 10);
        }

        /***************************************************
         * UDP连接
         **************************************************/
        private void EnableUDPButton(bool enable)
        {
            btIPAdress.Enabled = enable;
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
        //点击连接
        private void btConn_Click(object sender, EventArgs e)
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
                    /*
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    NetDataHandle.udpNode.udpClient.BeginReceive(new AsyncCallback((ar) =>
                    {
                        UdpClient u = (UdpClient)ar.AsyncState;
                        byte[] data = u.EndReceive(ar, ref remoteEndPoint);
                        NetDataHandle.OnDataReceived(data, remoteEndPoint);
                        // 继续异步接收数据
                        u.BeginReceive(new AsyncCallback((ar2) => {
                            UdpClient u2 = (UdpClient)ar2.AsyncState;
                            byte[] data2 = u2.EndReceive(ar2, ref remoteEndPoint);
                            NetDataHandle.OnDataReceived(data2, remoteEndPoint);
                        }), u);
                    }), NetDataHandle.udpNode.udpClient);
                    */
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void btIDSelect_Click(object sender, EventArgs e)
        {
            NetDataHandle.curSelMsn++;
            if (NetDataHandle.curSelMsn == 6)
                NetDataHandle.curSelMsn = 0;
            string text = string.Format("ID:{0:D2}", NetDataHandle.curSelMsn + 1);
            btIDSelect.Text = text; 
        }
        /*******************************************
         * 发射模式及版本号
         ******************************************/
        private void btLanuchMode_Click(object sender, EventArgs e)
        {
            //labelVersion.Focus();
            //  Enable422Button(false);
            Byte[] data = new Byte[8];
            if (btLanuchMode.Text == "正式发射")
            {
                btLanuchMode.Text = "无时序模飞";
                btLanuchMode.ForeColor = Color.Yellow;
                NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightMode = 0x55;
                
                data[0] = 0x55;
                
            }
            else if(btLanuchMode.Text == "无时序模飞")
            {
                btLanuchMode.Text = "有时序模飞";
                btLanuchMode.ForeColor = Color.Yellow;
                NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightMode = 0x44;
                data[0] = 0x44;
            }
            else if (btLanuchMode.Text == "有时序模飞")
            {
                btLanuchMode.Text = "正式发射";
                btLanuchMode.ForeColor = Color.Lime;
                NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightMode = 0x11;
                data[0] = 0x11;
            }
            NetDataHandle.Send_To_FK(1, 0x01, data);
        }

        /*******************************************
         * 界面刷新
         ******************************************/
        int freshCount = 0;
        public void RefreshUI()
        {
            modSrvCtrl.RefreshUI();
            mod3DCtrl.RefreshUI();
            modDevPowerCombin.RefreshUI();
            modDevPowerBatt.RefreshUI();
            modDevPowerSrv.RefreshUI();
            modDevPowerFuse.RefreshUI();
            modFileCtrl.RefreshUI();
            modGpsCtrl.RefreshUI();
            modIMUCtrl.RefreshUI();
            modNavCtrl.RefreshUI();
            modLanuch.RefreshUI();
            modPayload1.RefreshUI();

            //刷新自己的内容
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime < 1000)
            {
                flightTimeCtrl.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime.ToString("F2");
            }
            else if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime < 10000)
            {
                flightTimeCtrl.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime.ToString("F1");
            }
            else
            {
                flightTimeCtrl.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime.ToString("F0");
            }
            //版本号刷新
            labelVersion.Text = new string(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].version);

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
            btIDSelect.Text = string.Format("ID:{0:D2}", NetDataHandle.curSelMsn + 1);
            
            //发射模式刷新，为了保证地面使用效果，指令发出后等待1秒再刷新
            Byte Mode;
            if (btLanuchMode.Text == "正式发射")
            {
                Mode = 0x11;
            }
            else if (btLanuchMode.Text == "有时序模飞")
            {
                Mode = 0x44;
            }
            else
            {
                Mode = 0x55;
            }
            if (Mode != NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightMode)
            {
                if (freshCount < 10)
                {
                    freshCount++;
                }
                else
                {
                    freshCount = 0;
                    switch (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightMode)
                    {
                        case 0x11:
                            btLanuchMode.Text = "正式发射";
                            btLanuchMode.ForeColor = Color.Lime;
                            break;
                        case 0x44:
                            btLanuchMode.Text = "有时序模飞";
                            btLanuchMode.ForeColor = Color.Yellow;
                            break;
                        case 0x55:
                            btLanuchMode.Text = "无时序模飞";
                            btLanuchMode.ForeColor = Color.Yellow;
                            break;
                    }
                }
            }
        }


    }
}
