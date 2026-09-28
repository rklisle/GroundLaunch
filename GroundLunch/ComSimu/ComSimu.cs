using DevExpress.XtraEditors;
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
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GroundLunch
{
    public partial class ComSimu : DevExpress.XtraEditors.XtraForm
    {
        // 组播地址和端口
        static string lastConnComName = "";
        static public UdpClient udpClient = new UdpClient();
        static public int port = 39701;
        public ComSimu()
        {
            InitializeComponent();
            InitComs();
            InitNet();
        }

        private void ComSimu_Load(object sender, EventArgs e)
        {
        }

        public void InitNet()
        {
            StartReceiveUdpData();
        }

        static public void StartReceiveUdpData()
        {
            try
            {
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 39701));
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                IPAddress multicastAddress = IPAddress.Parse("238.0.0.1");
                // udpNode.udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 8001));
                // 加入组播组
                udpClient.JoinMulticastGroup(multicastAddress, IPAddress.Parse("127.0.0.1"));
                udpClient.BeginReceive(new AsyncCallback(OnRecviceUdpData), (udpClient, remoteEndPoint));
            }
            catch (SocketException)
            {
                // 39701 已被占用（常见于 Debug 实例还在跑）时不要让整个地面站退出
            }
            catch (Exception)
            {
            }
        }

        static public void OnRecviceUdpData(IAsyncResult ar)
        {
            //所有接收到的数据，均带有标识，可区分为任务机或地面端机
            //通过界面选项选择的curSelMsn仅用于刷新页面，不在处理过程中影响数值
            try
            {
                //通过EndReceive获取本次数据
                var (udpClient, remoteEndPoint) = ((UdpClient, IPEndPoint))ar.AsyncState;
                byte[] buf = udpClient.EndReceive(ar, ref remoteEndPoint);
                //重新开始监听
                udpClient.BeginReceive(new AsyncCallback(OnRecviceUdpData), (udpClient, remoteEndPoint));
                if (buf.Length <= 0)
                {
                    return;
                }
                //对数据进行初步解析，得到管号和炮号
                //UDP接收到就是一个整包，不需要再进行断包和粘包处理
                //剔除发往数据链地面端的数据
                if (buf[0]== 0x01 && buf[1] == 0x02 )
                    ComSend.Send_To_TM(buf);

            }
            catch (Exception ex)
            {
                ;
            }
        }

        public void InitComs()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboComSimu.Properties.Items.Add(port);
            }
            if (comboComSimu.Properties.Items.Count > 0)
                comboComSimu.SelectedIndex = 0;
            if (ComSend.comSetting.nodes.Count == 0)
            {
                ComSend.comSetting.nodes.Add(new ComNode());
                ComSend.comSetting.nodes.Add(new ComNode());
            }
            LoadLastConnName();
        }

        private void btConnSimu_Click(object sender, EventArgs e)
        {
            ConnToCom(0);
        }

        private void ConnToCom(int comIndex)
        {
            ComNode com = ComSend.comSetting.nodes[comIndex];
            if (com.connectState)
            {//如果连接，则断开
                com.connectState = false; //数传连接标志
                com.serialPort.Close();
                RefreshState();
                EnalbeComControl(comIndex, true);
                com.serialPort.DataReceived -= CmdPortCmdRecvData;
            }
            else
            {//如果没连接则连接
                ComboBoxEdit comboCom;
                comboCom = comboComSimu;
                com.serialPort.BaudRate = 921600;
                com.serialPort.PortName = comboCom.Text;
                com.serialPort.Parity = Parity.None;
                com.serialPort.DataBits = 8;
                com.serialPort.StopBits = StopBits.One;
                try
                {
                    com.serialPort.Open();
                    com.connectState = true;

                    RefreshState();
                    EnalbeComControl(comIndex, false);
                    com.serialPort.DataReceived += new SerialDataReceivedEventHandler(CmdPortCmdRecvData);

                    //第一次收到数据，将该串口号保存，以便下次自动连接
                    lastConnComName = com.serialPort.PortName;
                    SaveLastConnName();
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        //-------------------串口状态刷新------------------
        public void RefreshState()
        {
            btConnSimu.DataBindings.Clear();
            btStateSimu.DataBindings.Clear();
            btConnSimu.DataBindings.Add("Text", ComSend.comSetting.nodes[0], "connectBtnText");
            btStateSimu.DataBindings.Add("Text", ComSend.comSetting.nodes[0], "connectStateDes");
            btStateSimu.DataBindings.Add("ImageIndex", ComSend.comSetting.nodes[0], "connectStateImgIndex");
        }

        //---------------使能串口区域按钮------------------------
        public void EnalbeComControl(int comIndex, bool enable)
        {
            comboComSimu.Enabled = enable;
        }

        int comsendCount = 0;
        //-------------------串口数据接收---------------------------
        public void CmdPortCmdRecvData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            int num = port.BytesToRead;
            byte[] buf = new byte[num];
            port.Read(buf, 0, num);
            HandleComRecv.rawBufList.Add(buf);
            HandleComRecv.HandleRawData();

            while (HandleComRecv.frames.Count > 0) 
            {
                var remoteEndPoint = new IPEndPoint(IPAddress.Parse("238.0.0.1"), 39702);
                byte[] buf1 = new byte[255];
                int len = HandleComRecv.frames[0].dataLen + 12;
                buf1[0] = 0x01;
                buf1[1] = 0x01;
                buf1[2] = (byte)len;
                buf1[3] = 0x00;
                Buffer.BlockCopy(BitConverter.GetBytes(comsendCount), 0, buf1, 4, 4);
                buf1[8] = (byte)FormDataLink.linkTerminals[0].TerminalID;
                buf1[9] = 0x00;
                buf1[10] = HandleComRecv.frames[0].data[4];
                buf1[11] = HandleComRecv.frames[0].data[5];
                Buffer.BlockCopy(HandleComRecv.frames[0].data, 0, buf1, 12, HandleComRecv.frames[0].dataLen);
                udpClient.Send(buf1, len, remoteEndPoint);
                HandleComRecv.RemoveFrame();
            }
        }

        private void ComSimu_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;        // 阻止关闭
            this.Hide();            // 隐藏而不是释放
           
        }

        private void SaveLastConnName()
        {
            string savePath = System.AppDomain.CurrentDomain.BaseDirectory + "./Config/simuCom.xml";
            XmlSerializer myxml = new XmlSerializer(typeof(string));
            FileStream fs = new FileStream(savePath, FileMode.Create);
            myxml.Serialize(fs, lastConnComName);
            fs.Close();
        }

        private void LoadLastConnName()
        {
            if (File.Exists("./Config/simuCom.xml"))
            {
                FileStream reader = new FileStream("./Config/simuCom.xml", FileMode.Open);
                XmlSerializer zer = new XmlSerializer(typeof(string));
                lastConnComName = (string)zer.Deserialize(reader);
                reader.Close();

                foreach (var comitem in comboComSimu.Properties.Items)
                {
                    if (comitem.ToString() == lastConnComName)
                    {
                        comboComSimu.SelectedItem = comitem;
                        ConnToCom(0);
                        break;
                    }
                }
            }
           
        }
    }
}
