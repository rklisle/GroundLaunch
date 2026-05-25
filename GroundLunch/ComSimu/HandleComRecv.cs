//using OfficeOpenXml;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GroundLunch
{
    class HandleComRecv
    {
        const int MAX_LEN = 50000;
        static public List<Byte[]> rawBufList = new List<Byte[]>();
        static public byte[] unhandledTmBuf = new byte[MAX_LEN];
        static public int head = 0, tail = 0;
        static public List<CommonFrame> frames = new List<CommonFrame>();
        static public bool AutoSaveFlag = false;
        public static void HandleRawData()
        {
            while (rawBufList.Count > 0)
            {
                byte[] data = rawBufList[0];
                if (data == null)
                {
                    rawBufList.RemoveAt(0);
                    continue;
                }
                for (int idx = 0; idx < data.Length; idx++)
                {
                    unhandledTmBuf[tail++] = data[idx];
                    if (tail == MAX_LEN)
                    {
                        tail = 0;
                    }
                }

                while ((tail + MAX_LEN - head) % MAX_LEN > 6)//剩余未处理长度小于帧头长度，说明是不完整帧，返回等待下一帧
                {
                    int head_1 = (head + 1) % MAX_LEN;
                    int buf_len_low = (head + 2) % MAX_LEN;
                    int buf_len_high = (head + 3) % MAX_LEN;
                    Byte[] lenBuf = new Byte[2];
                    lenBuf[0] = unhandledTmBuf[buf_len_low];
                    lenBuf[1] = unhandledTmBuf[buf_len_high];
                    UInt16 msgLen = BitConverter.ToUInt16(lenBuf, 0);

                    UInt16 frameLen = (UInt16)(msgLen + 4);

                    if ((unhandledTmBuf[head] == 0xEB) && (unhandledTmBuf[head_1] == 0x90))//0x55AA
                    {
                        if (frameLen > 500)
                        {
                            head = tail = 0;
                            break;
                        }
                        if ((tail + MAX_LEN - head) % MAX_LEN < frameLen)
                        {//剩余长度不足帧长
                            break;
                        }
                        CommonFrame frame = new CommonFrame();
                        for (int i = 0; i < frameLen && head != tail; i++)
                        {
                            frame.data[i] = unhandledTmBuf[head++];
                            if (head == MAX_LEN)
                            {
                                head = 0;
                            }
                        }
                        frame.dataLen = frameLen;
                        frames.Add(frame);
                    }
                    else
                    {
                        while (unhandledTmBuf[head] != 0xEB || unhandledTmBuf[head_1] != 0x90)
                        {
                            head = (head + 1) % MAX_LEN;
                            head_1 = (head + 1) % MAX_LEN;
                            if (head == tail)
                            {
                                break;
                            }
                        }
                        //tmhead = tmtail = 0;
                    }
                }
                rawBufList.RemoveAt(0);
            }
        }

        public static void RemoveFrame()
        {
            frames.RemoveAt(0);
        }
    }
}
