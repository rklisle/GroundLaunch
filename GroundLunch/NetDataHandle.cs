using DevExpress.Utils;
using DevExpress.XtraPrinting.Native;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GroundLunch
{
  
    internal class NetDataHandle
    {
        static public UDPNode udpNode = new UDPNode();
        static public UDPNode udpDataLinkNode = new UDPNode();
        static public List<string> udps = new List<string>();
        static public Dictionary<(int, int), (int, int)> paoguanTodataLink = new Dictionary<(int, int), (int, int)>();
        static public int curPao = 0;
        static public int curGuan = 0;
        static public Byte byteFrameCount = 1;
        static public UInt32 u32FrameCount = 0;
        static public Dictionary<(int,int), TMFrame> curSelPlaneTMFrame = new Dictionary<(int, int), TMFrame>();
        static public Dictionary<(int, int), List<TMFrame>> tmframes = new Dictionary<(int, int), List<TMFrame>>();
        static public Dictionary<(int, int), List<CommonFrame>> commonframes = new Dictionary<(int, int), List<CommonFrame>>();
        static public Dictionary<(int, int), List<CurFileInRocket> > planeFileList = new Dictionary<(int, int), List<CurFileInRocket>>();
        static public Dictionary<(int, int), int> planeConnectStatus = new Dictionary<(int, int), int>();
        static public Dictionary<(int, int), int> planeConnectDbm = new Dictionary<(int, int), int>();
        // static public Dictionary<(int, int), int> tmAckCount = new Dictionary<(int, int), int>();

        static IAsyncResult asyncResult;
        static IAsyncResult asyncResultDataLink;
        static public byte[] _30Packet = new byte[60];
        static public byte[] _31Packet = new byte[60];
        static public byte[] _32Packet = new byte[60];
        public NetDataHandle() 
        {

        }

        static public void InitData()
        {
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    tmframes[(i,j)] = new List<TMFrame>();
                    commonframes[(i , j )] = new List<CommonFrame>();
                    curSelPlaneTMFrame[(i,j)] = new TMFrame();
                    planeFileList[(i , j )] = new List<CurFileInRocket>();
                    planeConnectStatus[(i , j )] = 0;
                    planeConnectDbm[(i, j)] = -128;
                    // tmAckCount[(i, j)] = 0;
                }
            }
            TMHandler.InitTMHandler();
        }

        static public void AutoConnOnlyOneNet()
        {
            if (udps.Count == 1)
            {
                try
                {
                    if (udpNode.udpClient.Client == null)
                        udpNode.udpClient = new UdpClient();
                    udpNode.udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse(udps[0]), 39702));
                    udpDataLinkNode.udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse(udps[0]), 39703));
                    udpNode.connectState = true;
                    StartReceiveUdpData();
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        static public void InitNet()
        {
            udps.Clear();
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                .ToList();
            foreach (NetworkInterface netInterface in networkInterfaces)
            {
                IPInterfaceProperties ipProps = netInterface.GetIPProperties();

                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        if(addr.Address.ToString().StartsWith("192.168.2"))
                            udps.Add(addr.Address.ToString());
                        if (addr.Address.ToString().StartsWith("127.0.0.1"))
                            udps.Add(addr.Address.ToString());
                    }
                }
            }
            if(udps.Count > 0 ) 
            {
                NetDataHandle.udpNode.udpIP = udps[0];
                AutoConnOnlyOneNet();
            }
        }

        static bool stopRecvDataLink = false;
        static bool stopRecvPlane = false;
        static public void StopReceiveUdpData()
        {
            //IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            //udpNode.udpClient.EndReceive(asyncResult, ref remoteEndPoint);

            stopRecvDataLink = true;
            stopRecvPlane = true;
        }

        static public void StartReceiveUdpData()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            IPEndPoint remoteEndPointDataLink = new IPEndPoint(IPAddress.Any, 0);
            IPAddress multicastAddress = IPAddress.Parse("238.0.0.1");

            // 加入组播组
            udpNode.udpClient.JoinMulticastGroup(multicastAddress, IPAddress.Parse(udpNode.udpIP));
            //udpNode.udpClient.MulticastLoopback = false;
            asyncResult = udpNode.udpClient.BeginReceive(new AsyncCallback(OnRecviceUdpData), (udpNode.udpClient, remoteEndPoint));
            stopRecvPlane = false;

            udpDataLinkNode.udpClient.JoinMulticastGroup(multicastAddress, IPAddress.Parse(udpDataLinkNode.udpIP));
            asyncResultDataLink = udpDataLinkNode.udpClient.BeginReceive(new AsyncCallback(OnRecviceDataLinkData), (udpDataLinkNode.udpClient, remoteEndPointDataLink));
            stopRecvDataLink = false;
        }

        static public void OnRecviceDataLinkData(IAsyncResult ar)
        {
            //所有接收到的数据，均带有标识，可区分为任务机或地面端机
            //通过界面选项选择的curSelMsn仅用于刷新页面，不在处理过程中影响数值
            try
            {
                //通过EndReceive获取本次数据
                var (udpClient, remoteEndPoint) = ((UdpClient, IPEndPoint))ar.AsyncState;
                byte[] buf = udpClient.EndReceive(ar, ref remoteEndPoint);
                if (stopRecvDataLink)
                {
                    return;
                }
                //重新开始监听
                asyncResultDataLink = udpDataLinkNode.udpClient.BeginReceive(new AsyncCallback(OnRecviceDataLinkData), (udpDataLinkNode.udpClient, remoteEndPoint));
                if (buf.Length <= 0)
                {
                    return;
                }
                //所有地面端均通过相同的端口号连接至地面站
                //判断协议中携带的地面终端ID判断是哪个站点发回的消息
                //分析上报数据，看开关是否已经打开，是否已经连接到飞机
                if (buf[0] == 0x02 && buf[1] == 0xA1)
                {
                    //400ms地面终端发往地面站消息
                    int terminalID = buf[8];
                    int paoID = 0;
                    foreach (var item in FormDataLink.linkTerminals)
                    {
                        if (item.TerminalID == terminalID) 
                        {
                            item.TerminalOnline = 20;
                            // 必须用终端绑定的炮号，不能 paoID++（否则写入 (0,*) / 错炮，VideoForm 读不到）
                            paoID = item.paoID;
                            break;
                        }
                    }
                    //每8个字节一个飞机
                    int guanID = 0;
                    for (int i = 0; i < 12 * 8; i += 8)
                    {
                        guanID++;
                        if (buf[23 + i] != 0)
                        {//飞机端已经连接，发送查询请求
                            int planeID = buf[23 + i] * 0x100 + buf[24+i];
                            Send_To_DataLink(6, planeID);
                        }
                        sbyte dbm = (sbyte)buf[26 + i];
                        if (paoID > 0)
                            planeConnectDbm[(paoID, guanID)] = dbm;
                    }
                }
                if (buf[0] == 0x02 && buf[1] == 0xA0)
                {
                    //查询地面端机状态回复
                    int terminalID = buf[8];
                    int terminalOpen = buf[17];
                    int terminalPower = buf[21];
                    foreach (var item in FormDataLink.linkTerminals)
                    {
                        if (item.TerminalID == terminalID)
                        {
                            item.TerminalOnline = 20;
                            item.TerminalOpen = terminalOpen == 0 ? true : false;
                            item.powerMode = terminalPower == 1 ? 1 : 2;
                        }
                    }
                }
                if (buf[0] == 0x04 && buf[1] == 0xB1)
                {
                    int terminalID = buf[8];
                    byte planeDataLinkGroupID = buf[10];
                    byte planeDataLinkPlaneID = buf[11];
                    sbyte dbm = (sbyte)buf[30];
                    foreach (var item in FormDataLink.linkTerminals)
                    {
                        if (item.TerminalID == terminalID)
                        {
                            foreach (var id in paoguanTodataLink)
                            {
                                if (id.Value.Item1 == planeDataLinkGroupID &&
                                   id.Value.Item2 == planeDataLinkPlaneID)
                                {
                                    // id.Key = (炮号, 管号)，同步到 VideoForm 读取的 planeConnectDbm
                                    planeConnectDbm[id.Key] = dbm;
                                    foreach (var plane in item.planeDataLinks)
                                    {
                                        if (plane.guanID == id.Key.Item2)
                                        {//已经添加进列表，更新列表内容
                                            plane.dbm = dbm;
                                            item.needReInit = true;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                if (buf[0] == 0x04 && buf[1] == 0xB0)
                {
                    //查询飞机端状态回复
                    int terminalID = buf[8];
                    byte planeDataLinkGroupID = buf[10];
                    byte planeDataLinkPlaneID = buf[11];
                    byte powerMode = buf[18];
                    
                    foreach (var item in FormDataLink.linkTerminals)
                    {
                        if (item.TerminalID == terminalID)
                        {
                            foreach (var id in paoguanTodataLink)
                            {
                                if (id.Value.Item1 == planeDataLinkGroupID &&
                                   id.Value.Item2 == planeDataLinkPlaneID)
                                {
                                    bool alreadyInList = false;
                                    foreach (var plane in item.planeDataLinks)
                                    {
                                        if (plane.guanID == id.Key.Item1)
                                        {//已经添加进列表，更新列表内容
                                            plane.powerMode = powerMode;
                                            alreadyInList = true;
                                            item.needReInit = true;
                                            break;
                                        }
                                    }
                                    if (!alreadyInList)
                                    {
                                        PlaneDataLink plane = new PlaneDataLink();
                                        plane.guanID = id.Key.Item1;
                                        plane.powerMode = powerMode;
                                        item.planeDataLinks.Add(plane);
                                        //item.InitPlaneGrid();
                                    }
                                    
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                ;
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
                if (stopRecvPlane)
                {
                    return;
                }
                //重新开始监听
                asyncResult = udpNode.udpClient.BeginReceive(new AsyncCallback(OnRecviceUdpData), (udpNode.udpClient, remoteEndPoint));
                if (buf.Length <= 0)
                {
                    return;
                }
                //对数据进行初步解析，得到管号和炮号
                //UDP接收到就是一个整包，不需要再进行断包和粘包处理
                int paoID, guanID;
                paoID = buf[16];
                guanID = buf[17];
                if (curPao == 0)
                    curPao = paoID;
                if(curGuan == 0)    
                    curGuan = guanID;

                if(planeConnectStatus[(paoID, guanID)] == 0)
                    paoguanTodataLink[(paoID, guanID)] = (buf[10], buf[11]);

                planeConnectStatus[(paoID, guanID)] = 20;

                //直接送给炮号对应的炮处理
                CommonFrame frame;
                if (buf[27] == 0x99 || buf[27] == 0x9A)
                {
                    frame = new TMFrame();
                }
                else
                {
                    frame = new CommonFrame();
                }
                Buffer.BlockCopy(buf, 22, frame.data, 0, buf.Length - 23);
                frame.AnalyzeFrame();
                if (frame is TMFrame)
                {
                    // tmframes[(paoID, guanID)].Add((TMFrame)frame);
                    curSelPlaneTMFrame[(paoID, guanID)] = (TMFrame)frame;
                    TMHandler.FrameToAllParamList((TMFrame)frame, paoID, guanID);
                    // 屏蔽：每 10 帧遥测向飞控发 0xFF 空应答
                    // tmAckCount[(paoID, guanID)]++;
                    // if (tmAckCount[(paoID, guanID)] >= 10)
                    // {
                    //     tmAckCount[(paoID, guanID)] = 0;
                    //     Send_To_TM(paoID, guanID, 0, 0xFF, new byte[0]);
                    // }
                }
                else
                {
                    if (frame.msgID == 0x65)
                    {
                        planeFileList[(paoID, guanID)].Add(frame.aFile);
                    }
                    else if (frame.msgID == 0x30)
                    {
                        Buffer.BlockCopy(frame.payLoad, 0, _30Packet, 0, frame.dataLen);
                    }
                    else if (frame.msgID == 0x31)
                    {
                        Buffer.BlockCopy(frame.payLoad, 0, _31Packet, 0, frame.dataLen);
                    }
                    else if (frame.msgID == 0x32)
                    {
                        Buffer.BlockCopy(frame.payLoad, 0, _32Packet, 0, frame.dataLen);
                    }
                }
                /*202的东西，280用不到
                //根据接收的IP地址判断，存入哪个rawBufList
                string strIpEnd = remoteEndPoint.Address.ToString().Split(".".ToCharArray())[3];
                int msnIndex = Convert.ToInt32(strIpEnd) - 8 - UdpStart.startGuanhao;
                if (msnIndex >= 0)
                {
                    //来自于任务机的地面数据
                    //判断是否为当前选中任务机，如未选中，则不接收也不解析
                    if (msnIndex == curSelMsn)
                    {
                        rawBufList[msnIndex].Add(buf);
                    }
                }
                else
                {
                    rawTmBufList.Add(buf);
                }
                */
                
            } 
            catch (Exception ex) 
            {
                ;
            }
        }
        
        /// <summary>
        /// Send发送
        /// </summary>
        /// <param name="len"></param>
        /// <param name="msg_id"></param>
        /// <param name="payload"></param>
        static public void Send_To_TM(int paoID, int guanID, ushort len, byte msg_id, byte[] payload)
        {
            //通过链路端口发送数据至飞控
            //获取炮号对应的地面终端ID
            byte TerminalID = 0;
            foreach (var item in FormDataLink.linkTerminals)
            {
                if (item.paoID == paoID)
                {
                    TerminalID = (byte)item.TerminalID;
                    break;
                }
            }
            //加三层包，第一层是标准通信协议包   

            Byte[] Send_Buf = new Byte[len + 8];
            Byte[] len_buf = BitConverter.GetBytes(len);
            byte checkA, checkB;
            ushort check;
            Send_Buf[0] = 0xeb;         //HAD
            Send_Buf[1] = 0x90;         //HDB
            Send_Buf[2] = len_buf[0];   //LEN
            Send_Buf[3] = len_buf[1];   //LEN
            Send_Buf[4] = 0x0;          //SEQ
            Send_Buf[5] = msg_id;       //Message_ID

            //Message_ID_Tx = msg_id;
            Buffer.BlockCopy(payload, 0, Send_Buf, 6, len);//payload
            check = crc16_ccitt(Send_Buf, (len + 6));
            checkA = (byte)(check & 0xFF);
            checkB = (byte)(0xFF & (check >> 8));
            Send_Buf[len + 6] = checkA;
            Send_Buf[len + 7] = checkB;
            len += 8;
            //以上是遥控数据
            //增加链路包
            //加第二层，飞控可以收到的数据
            Byte[] fcRecvData = new Byte[len + 11];
            fcRecvData[0] = 0xEB;
            fcRecvData[1] = 0x90;
            fcRecvData[2] = (Byte)(len + 7);
            fcRecvData[3] = 0;
            fcRecvData[4] = TerminalID;
            fcRecvData[5] = 0x00;
            fcRecvData[6] = (Byte)paoID;
            fcRecvData[7] = (Byte)guanID;
            fcRecvData[8] = (0x00);
            fcRecvData[9] = byteFrameCount++;
            Buffer.BlockCopy(Send_Buf, 0, fcRecvData, 10, len);//payload
            fcRecvData[len + 10] = xorCheck(fcRecvData, 4, len + 6);
            len += 11;

            //加第三层，仅数据链使用
            Byte[] udpSendBuf = new Byte[len + 12];
            udpSendBuf[0] = 0x01;
            udpSendBuf[1] = 0x02;
            udpSendBuf[2] = (Byte)(len + 12);
            udpSendBuf[3] = 0x00;
            BitConverter.GetBytes(u32FrameCount).CopyTo(udpSendBuf, 4);
            u32FrameCount++;
            udpSendBuf[8] = TerminalID;
            udpSendBuf[9] = 0x00;

            byte[] datalinkid = new byte[2];
            foreach (var item in paoguanTodataLink)
            {
                if (item.Key == (paoID, guanID))
                {
                    datalinkid[0] = (byte)item.Value.Item1;
                    datalinkid[1] = (byte)item.Value.Item2;
                }
            }

            udpSendBuf[10] = (Byte)datalinkid[0];
            udpSendBuf[11] = (Byte)datalinkid[1];
            Buffer.BlockCopy(fcRecvData, 0, udpSendBuf, 12, len);//payload
            UdpPortSend(udpNode.udpClient, udpSendBuf, len + 12);
        }

        /// <summary>
        /// 对时 CMD_BJTIME_SET(0x0A)：写入飞控 BJTimeSecond，供起飞时锁存到 luncTime。
        /// 半实物无 GPS 时必须在起飞前调用，否则 luncTime 一直为 0。
        /// </summary>
        static public void SendBjTimeSet(int paoID, int guanID)
        {
            DateTime now = DateTime.Now;
            byte[] data = new byte[9];
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)now.Year), 0, data, 0, 2);
            data[2] = (byte)now.Month;
            data[3] = (byte)now.Day;
            data[4] = (byte)now.Hour;
            data[5] = (byte)now.Minute;
            data[6] = (byte)now.Second;
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)now.Millisecond), 0, data, 7, 2);
            Send_To_TM(paoID, guanID, 9, 0x0A, data);
        }

        static public UInt32 datalinkPacketIndex = 0; 
        static public void Send_To_DataLink(int cmdID, int param1 = 0, int param2 = 0)
        {
            byte[] udpSendBuf = new byte[200];
            UInt16 len = 0;
            switch (cmdID)
            {
                case 0://开启链路
                    {
                        //param1地面端机号
                        //param2是否开启链路1开启，0关闭
                        len = 12;
                        UInt16 tmID = (UInt16)param1;
                        udpSendBuf[0] = 0x02;
                        udpSendBuf[1] = 0x12;
                        Buffer.BlockCopy(BitConverter.GetBytes(len), 0, udpSendBuf, 2, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(datalinkPacketIndex), 0, udpSendBuf, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(tmID), 0, udpSendBuf, 8, 2);
                        udpSendBuf[10] = (byte)(param2 == 1?0:1);
                        udpSendBuf[11] = 0x08;
                    }
                    break;
                case 1://设置ID
                    break;
                case 2://设置波道号
                    break;
                case 3://设置一机带多少路
                    break;
                case 4://设置地面端基大小功率
                    {
                        //param1地面端机号
                        //param2功率大小 0大功率，1小功率，2强功率
                        len = 12;
                        UInt16 tmID = (UInt16)param1;
                        udpSendBuf[0] = 0x02;
                        udpSendBuf[1] = 0x16;
                        Buffer.BlockCopy(BitConverter.GetBytes(len), 0, udpSendBuf, 2, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(datalinkPacketIndex), 0, udpSendBuf, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(tmID), 0, udpSendBuf, 8, 2);
                        udpSendBuf[10] = (byte)(param2);
                        udpSendBuf[11] = 0x08;
                        UdpPortSend(udpDataLinkNode.udpClient, udpSendBuf, len);
                        datalinkPacketIndex++;
                    }
                    break;
                case 5://设置飞机端大小功率
                    {
                        //同时要设定连接的飞机也是大功率/小功率
                        len = 14;
                        UInt16 tmID = 0;
                        UInt16 planeID = (UInt16)param1;
                        foreach (var item in paoguanTodataLink)
                        {
                            if (item.Value.Item1 == (((param1 & 0xFF00) >> 8) & 0xFF) &&
                                item.Value.Item2 == (param1 & 0xFF))
                            {
                                int paoID = item.Key.Item1;
                                foreach (var terminal in FormDataLink.linkTerminals)
                                {
                                    if (terminal.paoID == paoID)
                                    {
                                        tmID = (UInt16)terminal.TerminalID;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        udpSendBuf[0] = 0x04;
                        udpSendBuf[1] = 0x42;
                        Buffer.BlockCopy(BitConverter.GetBytes(len), 0, udpSendBuf, 2, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(datalinkPacketIndex), 0, udpSendBuf, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(tmID), 0, udpSendBuf, 8, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(planeID), 0, udpSendBuf, 10, 2);
                        udpSendBuf[12] = (byte)(param2);
                        udpSendBuf[13] = 0x08;
                    }
                    break;
                case 6://查询飞机端状态
                    {
                        return;
                        //param1:飞机端id,通过该id反查地面端id
                        len = 13;
                        UInt16 tmID = 0;
                        UInt16 planeID = (UInt16)param1;
                        foreach (var item in paoguanTodataLink)
                        {
                            if (item.Value.Item1 == (((param1 & 0xFF00) >> 8) & 0xFF) && 
                                item.Value.Item2 == (param1 & 0xFF))
                            {
                                int paoID = item.Key.Item1;
                                foreach (var terminal in FormDataLink.linkTerminals)
                                {
                                    if (terminal.paoID == paoID)
                                    {
                                        tmID = (UInt16)terminal.TerminalID;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        
                        udpSendBuf[0] = 0x04;
                        udpSendBuf[1] = 0x40;
                        Buffer.BlockCopy(BitConverter.GetBytes(len), 0, udpSendBuf, 2, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(datalinkPacketIndex), 0, udpSendBuf, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(tmID), 0, udpSendBuf, 8, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(planeID), 0, udpSendBuf, 10, 2);
                        udpSendBuf[12] = 0;
                    }
                    break;
                case 0xFF://查询状态
                    {
                        len = 11;
                        UInt16 tmID = (UInt16)param1;
                        udpSendBuf[0] = 0x02;
                        udpSendBuf[1] = 0x10;
                        Buffer.BlockCopy(BitConverter.GetBytes(len), 0, udpSendBuf, 2, 2);
                        Buffer.BlockCopy(BitConverter.GetBytes(datalinkPacketIndex), 0, udpSendBuf, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(tmID), 0, udpSendBuf, 8, 2);
                        udpSendBuf[10] = 0;
                    }
                    break;
                default:
                    break;
            }
            UdpPortSend(udpDataLinkNode.udpClient, udpSendBuf, len);
            datalinkPacketIndex++;
        }


        static public void UdpPortSend(UdpClient client, Byte[] buf, int len)
        {
            if (client.Client != null )
            {
                try
                {
                    var remoteEndPoint = new IPEndPoint(IPAddress.Parse("238.0.0.1"), 39701);//发往数据链
                    var remoteEndPointSimu = new IPEndPoint(IPAddress.Parse("238.0.0.1"), 9527);//发往仿真
                    client.Send(buf, len, remoteEndPoint);
                    if (buf[0] == 0x01 && buf[1] == 0x02)
                        client.Send(buf, len, remoteEndPointSimu);

                }
                catch
                {
                    ComSend.ShowSerialOutputError();
                }

            }
            else
            {
                //MessageBox.Show("请确保串口已打开");
            }
        }

        public static ushort[] crc16tab = new ushort[256]
        {
            0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7,
            0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef,
            0x1231,0x0210,0x3273,0x2252,0x52b5,0x4294,0x72f7,0x62d6,
            0x9339,0x8318,0xb37b,0xa35a,0xd3bd,0xc39c,0xf3ff,0xe3de,
            0x2462,0x3443,0x0420,0x1401,0x64e6,0x74c7,0x44a4,0x5485,
            0xa56a,0xb54b,0x8528,0x9509,0xe5ee,0xf5cf,0xc5ac,0xd58d,
            0x3653,0x2672,0x1611,0x0630,0x76d7,0x66f6,0x5695,0x46b4,
            0xb75b,0xa77a,0x9719,0x8738,0xf7df,0xe7fe,0xd79d,0xc7bc,
            0x48c4,0x58e5,0x6886,0x78a7,0x0840,0x1861,0x2802,0x3823,
            0xc9cc,0xd9ed,0xe98e,0xf9af,0x8948,0x9969,0xa90a,0xb92b,
            0x5af5,0x4ad4,0x7ab7,0x6a96,0x1a71,0x0a50,0x3a33,0x2a12,
            0xdbfd,0xcbdc,0xfbbf,0xeb9e,0x9b79,0x8b58,0xbb3b,0xab1a,
            0x6ca6,0x7c87,0x4ce4,0x5cc5,0x2c22,0x3c03,0x0c60,0x1c41,
            0xedae,0xfd8f,0xcdec,0xddcd,0xad2a,0xbd0b,0x8d68,0x9d49,
            0x7e97,0x6eb6,0x5ed5,0x4ef4,0x3e13,0x2e32,0x1e51,0x0e70,
            0xff9f,0xefbe,0xdfdd,0xcffc,0xbf1b,0xaf3a,0x9f59,0x8f78,
            0x9188,0x81a9,0xb1ca,0xa1eb,0xd10c,0xc12d,0xf14e,0xe16f,
            0x1080,0x00a1,0x30c2,0x20e3,0x5004,0x4025,0x7046,0x6067,
            0x83b9,0x9398,0xa3fb,0xb3da,0xc33d,0xd31c,0xe37f,0xf35e,
            0x02b1,0x1290,0x22f3,0x32d2,0x4235,0x5214,0x6277,0x7256,
            0xb5ea,0xa5cb,0x95a8,0x8589,0xf56e,0xe54f,0xd52c,0xc50d,
            0x34e2,0x24c3,0x14a0,0x0481,0x7466,0x6447,0x5424,0x4405,
            0xa7db,0xb7fa,0x8799,0x97b8,0xe75f,0xf77e,0xc71d,0xd73c,
            0x26d3,0x36f2,0x0691,0x16b0,0x6657,0x7676,0x4615,0x5634,
            0xd94c,0xc96d,0xf90e,0xe92f,0x99c8,0x89e9,0xb98a,0xa9ab,
            0x5844,0x4865,0x7806,0x6827,0x18c0,0x08e1,0x3882,0x28a3,
            0xcb7d,0xdb5c,0xeb3f,0xfb1e,0x8bf9,0x9bd8,0xabbb,0xbb9a,
            0x4a75,0x5a54,0x6a37,0x7a16,0x0af1,0x1ad0,0x2ab3,0x3a92,
            0xfd2e,0xed0f,0xdd6c,0xcd4d,0xbdaa,0xad8b,0x9de8,0x8dc9,
            0x7c26,0x6c07,0x5c64,0x4c45,0x3ca2,0x2c83,0x1ce0,0x0cc1,
            0xef1f,0xff3e,0xcf5d,0xdf7c,0xaf9b,0xbfba,0x8fd9,0x9ff8,
            0x6e17,0x7e36,0x4e55,0x5e74,0x2e93,0x3eb2,0x0ed1,0x1ef0
        };

        public static ushort crc16_ccitt(byte[] buf, int len)
        {
            int counter;
            int crc = 0;
            for (counter = 2; counter < len; counter++)
            {
                crc = ((crc << 8) ^ crc16tab[((crc >> 8) ^ buf[counter]) & 0x00FF]);
            }
            return (ushort)(crc & 0xFFFF);
        }

        public static byte xorCheck(byte[] buf,int startPos, int len) 
        {
            byte checksum = 0;
            int i = startPos;
            while (i < len + startPos)
            {
                checksum = (byte)(checksum ^ buf[i]);
                i++;
            }
            return checksum;
        }
    }
}
