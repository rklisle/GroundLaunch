using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraScheduler.Drawing;
using MathNet.Numerics.LinearAlgebra;
using Org.BouncyCastle.Utilities;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GroundLunch
{

    public class SDFileInfo
    {
        public UInt32 fsize;          /* File size */
        public UInt16 fdate;         /* Modified date */
        public UInt16 ftime;         /* Modified time */
        public Byte fattrib;       /* File attribute */
        public string fname;    /* File name */

    }
    public class ComSetting
    {
        public List<ComNode> nodes = new List<ComNode>();
        [XmlAttribute]
        public bool autoStart = true;
        public bool autoStartp { get { return autoStart; } set { autoStart = value; } }
    }
    public class ComNode
    {
        [XmlAttribute]
        public string settingName;
        [XmlIgnore]
        public bool connectState = false;
        [XmlIgnore]
        public SerialPort serialPort = new SerialPort();
        public ComInfo info = new ComInfo();

        [XmlIgnore]
        public string connectBtnText
        {
            get { return connectState ? "点击断开" : "点击连接"; }
            set {; }
        }
        [XmlIgnore]
        public string connectStateDes
        {
            get { return connectState ? "已连接" : "已断开"; }
            set {; }
        }
        [XmlIgnore]
        public int connectStateImgIndex
        {
            get { return connectState ? 0 : 1; }
            set {; }
        }
    }
    public class ComInfo
    {
        [XmlAttribute]
        public string comName;
        [XmlAttribute]
        public uint comBaud;
        [XmlAttribute]
        public bool comCheck;

        [XmlIgnore]
        public bool comCheckp { get { return comCheck; } set { comCheck = value; } }
        [XmlIgnore]
        public bool comCheckp1 { get { return !comCheck; } set { comCheck = !value; } }
        [XmlIgnore]
        public string comBuadp
        {
            get { return comBaud.ToString(); }
            set { try { comBaud = Convert.ToUInt32(value); } catch (Exception e) { MessageBox.Show(e.Message); } }
        }
        [XmlIgnore]
        public string comNamep { get { return comName; } set { comName = value; } }
    }

    public class UDPNode
    {
        public bool connectState = false;
        public string udpIP = "238.0.0.1";
        public int udpPort = 39702;
        public UdpClient udpClient = new UdpClient();

        public void SendData(byte[] data, string ip, int port)
        {
            udpClient.Send(data, data.Length, ip, port);
        }
    }

    public class CommonFrame
    {
        public int frameGroup = -1;  //帧组号
        public Byte[] data = new byte[0xFFF];//包含帧头及校验和
        public Byte[] payLoad = new byte[0xFFF];//有效载荷数据
        public UInt16 dataLen = 0;
        public UInt32 tick;
        public double second;
        public int seq;
        public int msgID;
        public Byte[] crc16check = new byte[2];
        public CurFileInRocket aFile = new CurFileInRocket();
        static public FileStream fs;
        static public FileStream fsEphGps;
        static public FileStream fsEphBd;

        public virtual bool AnalyzeFrame() 
        {
            byte[] byteLen = new byte[2];
            byteLen[0] = data[2];
            byteLen[1] = data[3];

            dataLen = BitConverter.ToUInt16(byteLen, 0);
            if(dataLen > 300 ) 
            {
                return false;
            }
            seq = data[4];
            msgID = data[5];
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

            Buffer.BlockCopy(data, 6, payLoad, 0, dataLen);
            switch (msgID) 
            {
                case 0xE8:  //星历提取
                    {
                        DateTime dateTime = DateTime.Now;
                        FileStream fsTemp = null;
                        string filePath = "";
                        if (payLoad[0] == 1) //GPS
                        {
                            string strGpsFileName = dateTime.ToString("yy_MM_dd_HH_mm_ss") + "gps.txt";
                            fsEphGps = File.Open(strGpsFileName, FileMode.Create);
                            fsTemp = fsEphGps;
                            filePath = strGpsFileName + " 已提取";
                        }
                        else if (payLoad[0] == 2) //BD
                        {
                            string strBdFileName = dateTime.ToString("yy_MM_dd_HH_mm_ss") + "bd.txt";
                            fsEphBd = File.Open(strBdFileName, FileMode.Create);
                            fsTemp = fsEphBd;
                            filePath = strBdFileName + " 已提取";
                        }
                        if (fsTemp == null)
                            return false;
                        BinaryWriter writer = new BinaryWriter(fsTemp);
                        writer.Write(payLoad, 1, dataLen);
                        fsTemp.Close();
                        
                        XtraMessageBox.Show(filePath, "星历提取", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    }
                    break;
                case 0x50:  //SD卡提取
                    if (payLoad[0] == 4)
                    {
                        switch (payLoad[1])
                        {
                            case 2://文件信息
                                {
                                    SDFileInfo fileInfo = new SDFileInfo();
                                    fileInfo.fsize = BitConverter.ToUInt32(payLoad, 0x0F);
                                    fileInfo.fdate = BitConverter.ToUInt16(payLoad, 0x0F + 4);
                                    fileInfo.ftime = BitConverter.ToUInt16(payLoad, 0x0F + 6);
                                    fileInfo.fattrib = payLoad[0x0F + 8];
                                    int zeroPos = Array.IndexOf(payLoad, (byte)0, 0x0F + 9) - 0x0F - 9;
                                    fileInfo.fname = Encoding.ASCII.GetString(payLoad,0x0F + 9, zeroPos);
                                    fileInfo.fname = fileInfo.fname.TrimEnd();
                                    DateTime dateTime = DateTime.Now;
                                    string strCsvFileName = dateTime.ToString("yy_MM_dd_HH_mm_ss") + fileInfo.fname;
                                    fs = File.Open(strCsvFileName, FileMode.Create);
                                    Byte[] data = new byte[8];
                                    data[0] = payLoad[2];
                                    //NetDataHandle.Send_To_FK(1, 0x50, data);
                                }
                            break;
                            case 3://文件内容
                                {
                                    BinaryWriter writer = new BinaryWriter(fs);
                                    writer.Write(payLoad, 0x0F, 0x7F0);
                                    string strTemp = Encoding.ASCII.GetString(  payLoad, 0x0F, 0x7f0);
                                    Byte[] data = new byte[8];
                                    data[0] = payLoad[2];
                                    //NetDataHandle.Send_To_FK(1, 0x50, data);
                                }
                                break;
                            case 4://文件最后一包
                                {
                                    UInt16 len = BitConverter.ToUInt16(payLoad, 3);
                                    BinaryWriter writer = new BinaryWriter(fs);

                                    writer.Write(payLoad, 0x0F, len);
                                    string strTemp = Encoding.ASCII.GetString(payLoad, 0x0F, 0x7f0);
                                    fs.Close();
                                }
                                break;
                        }
                    }
                    break;
                case 0x61://烧写控制反馈
                    if (payLoad[0] == 0xAA)
                    {
                        if (payLoad[1] == 0x11)
                        {
                            //进入烧写状态
                            UpLoadFileState.uploadMark = true;
                        }
                        else
                        {
                            UpLoadFileState.uploadMark = false;
                            UpLoadFileState.state = 6;
                        }    
                    }
                    break;
                case 0x63://烧写中反馈
                    
                    if (payLoad[0] == 0x11)//烧写过程中
                    {
                        //判断当前包数和发送包数是否对应
                        ushort curIndex = BitConverter.ToUInt16(payLoad, 4);

                        if(curIndex == UpLoadFileState.curIndex)
                        {
                            if (curIndex + 1 == UpLoadFileState.totalCount)
                            {
                                UpLoadFileState.uploadMark = false;
                            }
                            else
                            {
                                UpLoadFileState.uploadMark = true;
                                UpLoadFileState.curIndex++;
                                
                            }
                        }
                        
                    }
                    else if (payLoad[0] == 0x22)//烧写完成
                    {
                        UpLoadFileState.state = 2;
                    }
                    else//烧写失败 
                    {
                        
                    }
                    break;
                case 0x65://查询反馈
                    {
                        Byte fileIndex = payLoad[0];
                        uint fileLen = BitConverter.ToUInt32(payLoad, 1);
                        Byte[] fileName = new Byte[64];
                        Buffer.BlockCopy(payLoad,5,fileName,0,64);
                        string strFileName = Encoding.UTF8.GetString(fileName,0, Array.IndexOf<byte>(fileName, 0));
                        Byte[] fileDate = new Byte[64];
                        Buffer.BlockCopy(payLoad, 69, fileDate, 0, 64);
                        string strFileDate = Encoding.UTF8.GetString(fileDate, 0, Array.IndexOf<byte>(fileDate, 0));


                        aFile.fileIndex = fileIndex;
                        aFile.fileName = strFileName;
                        aFile.fileSize = fileLen;
                        aFile.fileCreateDate = strFileDate;
                        aFile.fileType = fileIndex == 0xA0?1: fileIndex==0xB0 || fileIndex==0xB1?2:0;
                        
                    }
                    break;
                case 0x67://校验反馈
                    switch(payLoad[0])
                    {
                        case 0://校验成功
                            UpLoadFileState.state = 3;
                            break;
                        case 1:
                            break;
                        case 2:
                            UpLoadFileState.state = 4;
                            break;
                        case 3:
                            break; 
                    }
                    break;
                case 0x6B://清除反馈
                    UpLoadFileState.clearState = 2;
                    break;
            }
            return true;
        }
    }


    public class GroundFrame : CommonFrame 
    {
        public GroundProtocol groundData = new GroundProtocol();
        public override bool AnalyzeFrame()
        {
            byte[] byteLen = new byte[2];
            byteLen[0] = data[2];
            byteLen[1] = data[3];

            dataLen = BitConverter.ToUInt16(byteLen, 0);
            if (dataLen > 300)
            {
                return false;
            }
            seq = data[4];
            msgID = data[5];
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


            Buffer.BlockCopy(data, 6, payLoad, 0, dataLen);

            short shortValue;
            ushort ushortValue;
            int intValue;
            uint uintValue;
            float floatValue;
            
            int curPos = 0;
            //飞控时间
            intValue = BitConverter.ToInt32(payLoad, curPos); 
            curPos += 4;
            groundData.flightTime = intValue * 0.005;
            //上一条指令
            groundData.lastCmd = payLoad[curPos];
            curPos++;
            //飞控模式
            groundData.flightMode = payLoad[curPos];
            curPos++;
            //版本号
            char[] tempVersion = new char[8];
            groundData.version = new char[11];
            for (int i = 0; i < 8; i++)
            {
                tempVersion[i] = (char)payLoad[curPos + i];
                
            }
            groundData.version[0] = tempVersion[0];
            groundData.version[1] = tempVersion[1];
            groundData.version[2] = '-';
            groundData.version[3] = tempVersion[2];
            groundData.version[4] = tempVersion[3];
            groundData.version[5] = ' ';
            groundData.version[6] = tempVersion[4];
            groundData.version[7] = tempVersion[5];
            groundData.version[8] = ':';
            groundData.version[9] = tempVersion[6];
            groundData.version[10] = tempVersion[7];
            //groundData.version[8] = '\0';
            //Buffer.BlockCopy(payLoad, curPos, groundData.version, 0, 12);
            curPos += 14;
            //发射点诸元
            intValue = BitConverter.ToInt32(payLoad, curPos); curPos += 4;
            groundData.lanuchLon = intValue * 1e-6;
            intValue = BitConverter.ToInt32(payLoad, curPos); curPos += 4;
            groundData.lanuchLat = intValue * 1e-6;
            uintValue = BitConverter.ToUInt32(payLoad, curPos); curPos += 4;
            groundData.lanuchHigh = uintValue * 1e-3;
            ushortValue = BitConverter.ToUInt16(payLoad, curPos); curPos += 2;
            groundData.lanuchAzimuth = ushortValue * 1e-2;
            //shortValue = BitConverter.ToInt16(payLoad, curPos); curPos += 2;
            //groundData.lanuchAzimuth = shortValue * 1e-2;
            shortValue = BitConverter.ToInt16(payLoad, curPos); curPos += 2;
            groundData.lanuchPitch = shortValue * 1e-2;
            //IO检测
            Byte state = payLoad[curPos];
            for (int i = 0; i < 6; i++)
            {
                groundData.IOState[i] = (Byte)(state & (1 << i));
            }
            curPos += 2;
            //设备通信状态
            state = payLoad[curPos];
            for (int i = 0; i < 8; i++)
            {
                int temp = (1 << i);
                groundData.connState[i] = (Byte)(state & temp);
            }
            state = payLoad[curPos + 1];
            for (int i = 0; i < 8; i++)
            {
                groundData.connState[i + 8] = (Byte)(state & (1 << i));
            }
            curPos += 2;
            //配电状态
            state = payLoad[curPos];
            byte[] powerState = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                powerState[i] = (Byte)(state & (1 << i));
            }
            groundData.powerState[1] = powerState[0];
            groundData.powerState[2] = powerState[4];
            groundData.powerState[3] = powerState[7];
            groundData.powerState[4] = powerState[1];

            curPos += 1;
            //电压电流状态
            //6路电压
           // for (int i = 0; i < 6; i++)
            {
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerV[1] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerV[4] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerV[0] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerV[2] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerV[3] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerV[5] = shortValue * 1e-2;
                curPos += 2;
            }
            //针对双路地面电压进行调整
            if (groundData.powerV[5] > groundData.powerV[3])
            {
                groundData.powerV[3] = groundData.powerV[5];
            }

            //6路电流
            //for (int i = 0; i < 6; i++)
            {
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerA[3] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerA[5] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerA[1] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerA[4] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerA[0] = shortValue * 1e-2;
                curPos += 2;
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.powerA[2] = shortValue * 1e-2;
                curPos += 2;
            }
            //针对双路地面电流进行调整
            if (groundData.powerA[5] > groundData.powerA[3])
            {
                groundData.powerA[3] = groundData.powerA[5];
            }
            curPos += 12;//12字节预留
            //惯组安装模式
            groundData.imuInstallMode = payLoad[curPos];
            curPos++;
            //惯组对准模式
            groundData.imuAlignMode = payLoad[curPos];
            curPos++;
            //惯组导航模式
            groundData.imuNavMode = payLoad[curPos];
            curPos++;
            //导航姿态角
            shortValue = BitConverter.ToInt16(payLoad, curPos);
            groundData.navYaw = shortValue * 1e-2;
            curPos += 2;

            shortValue = BitConverter.ToInt16(payLoad, curPos);
            groundData.navPitch = shortValue * 1e-2;
            curPos += 2;

            shortValue = BitConverter.ToInt16(payLoad, curPos);
            groundData.navRoll = shortValue * 1e-2;
            curPos += 2;
            
            //方位角
            ushortValue = BitConverter.ToUInt16(payLoad,curPos);
            groundData.navAzimuth = ushortValue * 1e-2;
            curPos += 2;

            //当地加速度
            ushortValue = BitConverter.ToUInt16(payLoad, curPos);
            groundData.localG = ushortValue * 1e-2;
            curPos += 2;

            //合成角速度
            ushortValue = BitConverter.ToUInt16(payLoad, curPos);
            groundData.velocityRotation = ushortValue * 1e-2;
            curPos += 2;

            //角速度
            floatValue = BitConverter.ToSingle(payLoad, curPos);
            groundData.imuWx = floatValue;
            curPos += 4;

            floatValue = BitConverter.ToSingle(payLoad, curPos);
            groundData.imuWy = floatValue;
            curPos += 4;

            floatValue = BitConverter.ToSingle(payLoad, curPos);
            groundData.imuWz = floatValue;
            curPos += 4;

            //加速度
            floatValue = BitConverter.ToSingle(payLoad, curPos);
            groundData.imuAx = floatValue;
            curPos += 4;

            floatValue = BitConverter.ToSingle(payLoad, curPos);
            groundData.imuAy = floatValue;
            curPos += 4;

            floatValue = BitConverter.ToSingle(payLoad, curPos);
            groundData.imuAz = floatValue;
            curPos += 4;

            //定位模式
            curPos++;//预留

            //定位状态
            groundData.gpsPositioned = payLoad[curPos];
            curPos++;

            //定位星数
            groundData.gpsSatelliteCount = payLoad[curPos];
            curPos++;

            //卫导经纬高
            intValue = BitConverter.ToInt32(payLoad, curPos);
            groundData.gpsLon = intValue * 1e-7;
            curPos += 4;

            intValue = BitConverter.ToInt32(payLoad, curPos);
            groundData.gpsLat = intValue * 1e-7;
            curPos += 4;

            intValue = BitConverter.ToInt32(payLoad, curPos);
            groundData.gpsHigh = intValue * 1e-1;   
            curPos += 4;

            //卫导北天东
            shortValue = BitConverter.ToInt16(payLoad, curPos);
            groundData.gpsVn = shortValue * 1e-2;
            curPos += 2;

            shortValue = BitConverter.ToInt16(payLoad, curPos);
            groundData.gpsVs = shortValue * 1e-2;
            curPos += 2;

            shortValue = BitConverter.ToInt16(payLoad, curPos);
            groundData.gpsVe = shortValue * 1e-2;
            curPos += 2;

            //PDOP
            ushortValue = BitConverter.ToUInt16(payLoad, curPos);
            groundData.gpsPDOP = ushortValue * 1e-2;
            curPos += 2;

            //GDOP
            ushortValue = BitConverter.ToUInt16(payLoad, curPos);
            groundData.gpsGDOP = ushortValue * 1e-2;
            curPos += 2;

            //PPS
            groundData.gpsPPS = payLoad[curPos];
            curPos++;

            //星历装订结果
            groundData.gpsEphemeris = payLoad[curPos];
            curPos++;

            //gps日期
            ushortValue = BitConverter.ToUInt16(payLoad, curPos + 6);
            string stringValue = string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}.{6:000}",
                payLoad[curPos + 0] + 2000, payLoad[curPos + 1], payLoad[curPos + 2],
                payLoad[curPos + 3], payLoad[curPos + 4], payLoad[curPos + 5],
                ushortValue);
            groundData.gpsDate = stringValue;
            curPos += 8;

            //舵控1-4
            for (int i = 0; i < 4; i++)
            {
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.srvCtrl[i] = shortValue * 1e-2;
                curPos += 2;
            }

            //舵反1-4
            for (int i = 0; i < 4; i++)
            {
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.srvCur[i] = shortValue * 1e-2;
                curPos += 2;
            }

            //舵控5-8
            for (int i = 4; i < 8; i++)
            {
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.srvCtrl[i] = shortValue * 1e-2;
                curPos += 2;
            }

            //舵反5-8
            for (int i = 4; i < 8; i++)
            {
                shortValue = BitConverter.ToInt16(payLoad, curPos);
                groundData.srvCur[i] = shortValue * 1e-2;
                curPos += 2;
            }
            //预留舵
            //curPos += 16;

            //SD卡工作状态
            groundData.sdState = payLoad[curPos];
            curPos++;

            //预发射状态
            groundData.readyForLanuch = payLoad[curPos];
            curPos++;

            groundData.airSpd = BitConverter.ToUInt16(payLoad ,curPos) * 0.1;
            curPos+=2;

            groundData.airClibration = BitConverter.ToInt16(payLoad, curPos);
            curPos += 2;

            groundData.onceBatteryVOK = payLoad[curPos++] == 0?1:2;
            groundData.onceBatteryTempStatus = payLoad[curPos++];
            groundData.onceBatteryError = payLoad[curPos++];
            groundData.onceBatteryV = BitConverter.ToUInt16(payLoad, curPos) * 0.1;

            return true;
        }

    }

    public class GroundProtocol
    {
        public double flightTime;   //源码为tick，5ms加一
        public Byte lastCmd;    //上一条指令
        public Byte flightMode = 0x11; //飞控模式 0x11:正式 0x44:有时序模飞 0x55:无时序模飞
        public char[] version = new char[12]; //版本号
        public double lanuchLon;    //int转当量-6e
        public double lanuchLat;    //int转当量-6e
        public double lanuchHigh;   //uint转当量-3e     
        public double lanuchAzimuth;    //ushort转当量-2e (0-360)
        public double lanuchPitch;  //ushort转当量-2e (+-90)
        public Byte[] IOState = new Byte[6];   //6路通断检测，1字节按比特转换
        public Byte[] connState = new Byte[16]; //16路通信状态，1字节按比特转换
        public Byte[] powerState = new byte[8];//6路供电状态，1字节按比特转换
        public double[] powerV = new double[6];//6路供电电压 short转当量-2e
        public double[] powerA = new double[6];//6路供电电流 short转当量-2e\
        public Byte imuInstallMode = 0; //惯组安装模式 1：垂直安装 2：水平安装 0：未设置
        public Byte imuAlignMode = 0;   //惯组对准模式 1：垂直对准 2：水平对准 0：未开始对准
        public Byte imuNavMode = 0;     //惯组导航模式 0x20对准中 0x2F对准超时 0x3F对准完成 0x60组合导航 0x64纯惯性导航 0x00默认
        public double navYaw;       //导航偏航角 short转当量-2e (-180~180)
        public double navPitch;     //导航俯仰角 short转当量-2e (-90~90)
        public double navRoll;      //导航滚转角 short转当量-2e (-180~180)
        public double navAzimuth;   //导航方位角 ushort转当量-2e (0~360)
        public double localG;       //当地重力加速度 ushort转当量-2e 正常值9.8
        public double velocityRotation; //自转角速度 ushort转当量-2e 正常值15
        public float imuAx;     //惯组x轴加速度 float直传
        public float imuAy;     //惯组y轴加速度 float直传
        public float imuAz;     //惯组z轴加速度 float直传
        public float imuWx;     //惯组x轴角速度 float直传
        public float imuWy;     //惯组y轴角速度 float直传
        public float imuWz;     //惯组z轴角速度 float直传
        public Byte gpsPositioned = 0;  //定位状态
        public Byte gpsSatelliteCount = 0;//定位星数
        public double gpsLon;
        public double gpsLat;
        public double gpsHigh;
        public double gpsVn;
        public double gpsVs;
        public double gpsVe;
        public double gpsPDOP;
        public double gpsGDOP;
        public double gpsPPS;
        public Byte gpsEphemeris;  // 0：未开始装订，1：装订中，2：装订完成，3：装订错误，4：装订超时
        public double[] srvCtrl = new double[8]; //舵控 short转当量 -2e
        public double[] srvCur = new double[8]; //舵反 short转当量 -2e
        public Byte sdState = 0;    //1:正常 0：异常
        public Byte readyForLanuch = 0; //1:进入预发射，0：未进入预发射
        public double airSpd;
        public double airClibration;
        public int fuseState;
        public string gpsDate;
        public Byte luanchRecv; //起飞接收标志
        public int onceBatteryTempStatus;
        public int onceBatteryVOK;
        public int onceBatteryError;
        public double onceBatteryV;
        public GroundProtocol()
        {
            GenTestData();
        }

        private void GenTestData()
        {
            flightTime = 0.0;
            lastCmd = 0xFA;
            flightMode = 0x11;
            version = "--".ToArray();
            lanuchLon = 94.87053612;
            lanuchLat = 36.35289834;
            lanuchHigh = 33.45;
            lanuchAzimuth = 329.12;
            lanuchPitch = 45.557;
            IOState[0] = 0;
            IOState[1] = 0;
            IOState[2] = 1;
            connState[0] = 0;
            connState[1] = 1;
            powerState[0] = 0;
            powerState[1] = 1;
            powerState[2] = 1;
            powerV[0] = 0.46;
            powerV[1] = 29.46;
            powerV[2] = 32.46;
            powerA[0] = 0.01;
            powerA[1] = 2.46;
            powerA[2] = 1.46;
            imuInstallMode = 2;
            imuAlignMode = 2;
            imuNavMode = 0x00;
            navYaw = 0;
            navPitch = 0;
            navRoll = 0;
            navAzimuth = 338;
            localG = 6.99;
            velocityRotation = 16.45;
            imuAx = (float)0.45;
            imuAy = (float)9.12;
            imuAz = (float)-3.12;

            imuWx = (float)3.789;
            imuWy = (float)-3.447;
            imuWz = (float)10.20;

            gpsPositioned = 1;
            gpsSatelliteCount = 16;

            gpsLon = 116.494783;
            gpsLat = 39.144898;
            gpsHigh = 1.1234;
            gpsVn = 0.35;
            gpsVe = 0.46;
            gpsVs = 0.27;
            gpsPDOP = 5.3;
            gpsGDOP = 4.5;

            gpsEphemeris = 0;
            srvCtrl[0] = 0.0;
            srvCtrl[1] = 0.0;
            srvCtrl[2] = 0.0;
            srvCtrl[3] = 0.0;

            srvCur[0] = 1.1;
            srvCur[1] = 2.1;
            srvCur[2] = 3.1;
            srvCur[3] = 4.1;

            srvCtrl[4] = 0.0;
            srvCtrl[5] = 0.0;
            srvCtrl[6] = 0.0;
            srvCtrl[7] = 0.0;

            srvCur[4] = 5.1;
            srvCur[5] = 6.1;
            srvCur[6] = 7.1;
            srvCur[7] = 8.1;

            sdState = 1;
            readyForLanuch = 1;
            gpsDate = "2024-03-01 22:12:34";

            onceBatteryTempStatus = 0;
            onceBatteryError = 0;
            onceBatteryVOK = 0;
            onceBatteryV = 99.4;
        }

        
    }
    public class fileInLocalFolder
    {
        public Byte fileIndex;
        public string filePath;
        public uint fileSize;
        public string fileCreateDate;
        public int fileType;   //0:普通诸元文件 1:模飞文件 2:CPU文件

        public string ID { get { return fileIndex.ToString("X2"); } set {; } }
        public string 文件路径 { get { return filePath; } set {; } }
        public string 文件容量 { get { return fileSize.ToString("N0"); } set {; } }

        public string 创建时间 { get { return fileCreateDate; } set {; } }
        //public string 更改文件 { get; set; }

        public string 烧写 { get; set; }
        public string 文件类型
        {
            get
            {
                switch (fileType)
                {
                    case 0:
                        return "发射诸元";
                    case 1:
                        return "模飞文件";
                    case 2:
                        return "应用程序";
                    default:
                        return "";
                }

            }
            set {; }
        }

        
    }
    public class CurFileInRocket
    {
        public int fileIndex;
        public string fileName;
        public uint fileSize;
        public string fileCreateDate;
        public int fileType;   //0:普通诸元文件 1:模飞文件 2:CPU文件
            

        public string ID { get { return fileIndex.ToString("X2"); } set {; } }
        public string 文件名称 { get { return fileName; } set {; } }
        public string 容量 { get { return fileSize.ToString("N0"); } set {; } }

        public string 创建时间 { get { return fileCreateDate; } set {; } }

        public string 文件类型 
        {
            get
            {
                switch (fileType)
                {
                    case 0:
                        return "发射诸元";
                    case 1:
                        return "模飞文件";
                    case 2:
                        return "应用程序";
                    default:
                        return "";
                }
                    
            }
            set {; }
        }
    }

    public class UpLoadFileState
    {
        static public double progress;
        static public ushort curIndex;
        static public ushort totalCount;
        static public int state = 7;   //0烧写交互中，1烧写中，2校验中，3校验成功，4校验失败，5超时, 6手动中断,7未开始
        static public bool uploadMark = false;
        static public int clearState = 0;
    }

    public class TMPacket
    {
        public string packetName;
        public int packetFreq;
        public int packetGroupID;
        public int packetByteCount;
        public int packetIndex;
        public List<TMParam> paramList = new List<TMParam>();
        public void CalcPacketByte()
        {
            packetByteCount = 0;
            foreach (var param in paramList)
            {
                packetByteCount += param.paramByteCount;
            }
        }
    }
    [Serializable]
    public class TMParam
    {
        public string paramName;
        public string paramID;
        public int paramByteCount;
        public Type paramType;
        public string paramUnit;
        public double paramCoefficient;
        public string paramDataPool;
        public UInt64 paramSourceCode;
        public double paramValue;
        public string fomula;
        public string detail;
        public bool showInImage;
        public int packetIndex;
        public TMParam DeepCopy()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, this);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (TMParam)formatter.Deserialize(memoryStream);
            }
        }

        public double DoFomulaCalc(object rawValue)
        {
            return (double)rawValue;
        }
        public bool 绘制 { get { return showInImage; } set { showInImage = value; } }
        public string 参数名称 { get { return paramName; } set {; } }
        public string 参数代号 { get { return paramID; } set {; } }
        public string 字节 { get { return paramByteCount.ToString(); } set {; } }
        public string 类型 { get { return paramType.ToString().Split('.')[1]; } set {; } }
        public string 单位 { get { return paramUnit; } set {; } }
        public string 当量 { get { return paramCoefficient.ToString("0.##########"); } set {; } }
        public string 来源 { get { return paramDataPool; } set {; } }

        public string 物理量
        {
            get
            {

                if (paramType == typeof(float))
                {
                    float a = BitConverter.ToSingle(BitConverter.GetBytes(paramSourceCode), 0);
                    //if (paramUnit.Contains("°"))
                    {
                        //a = (float)(a * 180 / 3.1415926);
                    }
                    double phyValue = a * paramCoefficient;
                    if (fomula != "n")
                        phyValue = DoFomulaCalc(phyValue);
                    string str = (phyValue).ToString("0.###");
                    return str;
                }
                else if (paramType == typeof(double))
                {
                    double a = BitConverter.ToDouble(BitConverter.GetBytes(paramSourceCode), 0);
                    //if (paramUnit.Contains("°"))
                    {
                        // a = (double)(a * 180 / 3.1415926);
                    }
                    double phyValue = a * paramCoefficient;
                    if (fomula != "n")
                        phyValue = DoFomulaCalc(phyValue);
                    string str = (phyValue).ToString("0.##########");
                    return str;
                }
                else
                {
                    if (paramType == typeof(Int32))
                    {
                        int dec;
                        byte[] source = BitConverter.GetBytes(paramSourceCode);
                        dec = BitConverter.ToInt32(source, 0);
                        //if (paramUnit.Contains("°"))
                        {
                            //  double a = dec;
                            //  a = (a * 180 / 3.1415926);
                            //  return (a * paramCoefficient).ToString("0.##########");
                        }
                        double phyValue = dec * paramCoefficient;
                        if (fomula != "n")
                            phyValue = DoFomulaCalc(phyValue);
                        return (phyValue).ToString("0.##########");
                    }
                    if (paramType == typeof(Int16))
                    {
                        int dec;
                        byte[] source = BitConverter.GetBytes(paramSourceCode);
                        dec = BitConverter.ToInt16(source, 0);
                        //if (paramUnit.Contains("°"))
                        // {
                        //    double a = dec;
                        //     a = (a * 180 / 3.1415926);
                        //    return (a * paramCoefficient).ToString("0.##########");
                        //}
                        double phyValue = dec * paramCoefficient;
                        if (fomula != "n")
                            phyValue = DoFomulaCalc(phyValue);
                        return (phyValue).ToString("0.###");
                    }
                    if (paramType == typeof(SByte))
                    {
                        sbyte dec;
                        byte[] source = BitConverter.GetBytes(paramSourceCode);
                        dec = (sbyte)source[0];
                        double phyValue = dec * paramCoefficient;
                        if (fomula != "n")
                            phyValue = DoFomulaCalc(phyValue);
                        return (phyValue).ToString("0.##########");
                    }
                    

                }
                    return (paramSourceCode * paramCoefficient).ToString("0.##########");
                }
            
            set {; }
        }

        public string 源码值HEX
        {
            get
            {
                string format = "x" + (paramByteCount * 2).ToString();
                return paramSourceCode.ToString(format);
            }
            set {; }
        }

        /*
        public string 源码值DEC
        {
            get
            {
                if (paramType == typeof(Int32))
                {
                    int dec;
                    byte[] source = BitConverter.GetBytes(paramSourceCode);
                    dec = BitConverter.ToInt32(source, 0);
                    return dec.ToString();
                }
                if (paramType == typeof(Int16))
                {
                    int dec;
                    byte[] source = BitConverter.GetBytes(paramSourceCode);
                    dec = BitConverter.ToInt16(source, 0);
                    return dec.ToString();
                }
                return paramSourceCode.ToString();
            }
            set {; }
        }
        
        public string 源码值BIN
        {
            get
            {
                if (paramByteCount <= 4)
                    return Convert.ToString((int)paramSourceCode, 2);
                else
                    return "";// return paramSourceCode.ToString("b");
            }
            set {; }
        }
        */

    }

    public class FlightPointFile
    {
        public string fileType { get;set; }
        //public string geoFence { get;set; }
        public string groundStation { get; set; }

        public Mission mission { get;set; }
    }

    public class Mission
    {
        public string cruiseSpeed { get; set; }

        public string firmwareType { get; set; }

        public string hoverSpeed { get; set; }

        public ITEM[] items { get; set; }
    }

    public class ITEM 
    {
        public string AMSLAltAboveTerrain { get; set; }
        public string Altitude { get; set; }

        public string AltitudeMode { get; set; }
        public string autoContinue { get; set; }
        public string command { get; set; }
        public string doJumpId { get; set; }

        public string frame { get; set; }

        public string[] Params { get; set; }
    }

    static public class UdpStart
    {
        static public int curPaohao = 0;
        static public int startGuanhao = 0;
        static public void OnPaohaoUpdate()
        {
            startGuanhao = curPaohao * 6;
        }
    }

    public class fileImageLocalFolder
    {
        public Byte fileIndex;
        public string filePath;
        public uint fileSize;
        public string fileCreateDate;

        public string ID { get { return fileIndex.ToString("X2"); } set {; } }
        public string 文件路径 { get { return filePath; } set {; } }
        public string 文件容量 { get { return fileSize.ToString("N0"); } set {; } }

        public string 创建时间 { get { return fileCreateDate; } set {; } }
        //public string 更改文件 { get; set; }
    }
}
