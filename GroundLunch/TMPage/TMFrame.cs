using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public override void AnalyzeFrame()
        {
            //将data转义到帧格式
            byte[] byteLen = new byte[2];
            byteLen[0] = data[2];
            byteLen[1] = data[3];

            dataLen = BitConverter.ToUInt16(byteLen, 0);
            if (dataLen > 300)
            {
                return;
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
            crc16check[0] = data[6 + dataLen];
            crc16check[1] = data[7 + dataLen];

            ushort check = ComSend.crc16_ccitt(data, (dataLen + 6));
            byte checkA, checkB;
            checkA = (byte)(check & 0xFF);
            checkB = (byte)(0xFF & (check >> 8));
            //校验和不通过，将数据长度置为0即可
            if (checkA != crc16check[0] || checkB != crc16check[1])
            {
                dataLen = 0;
                return;
            }
            for (int i = 0; i < dataLen; i++)
            {
                payLoad[i] = data[i + 11];
            }
        }
    }
}
