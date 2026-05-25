using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace GroundLunch
{
    public partial class VideoControl : UserControl
    {
        tstRtmp rtmp = new tstRtmp();
        Thread thPlayer;

        public bool AutoSendToFK = false;

        float azimuth_range = 26;
        float pitch_range = 15;

        private float pitch_angle = 0;
        private float azimuth_angle = 0;

        // 定义一个委托类型，接受两个 float 参数   
        public delegate void FloatDelegate(float value1, float value2);
        // 定义事件
        public event FloatDelegate OnDataProcessed;
        public VideoControl()
        {
            InitializeComponent();

            // SaveVideoFile();

           
        }

        /// <summary>
        /// 视频解码及显示开始
        /// </summary>
        public void ThreadStart()
        {
            if (thPlayer != null)
            {
                rtmp.Stop();
                thPlayer = null;
            }
            else
            {
                thPlayer = new Thread(DeCoding);
                thPlayer.IsBackground = true;
                thPlayer.Start();
            }
        }

        /// <summary>
        /// 播放线程执行方法
        /// </summary>
        private unsafe void DeCoding()
        {
            try
            {
                Console.WriteLine("DeCoding run...");
                Bitmap oldBmp = null;

                // 更新图片显示
                tstRtmp.ShowBitmap show = (bmp) =>
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.pic.Image = bmp;
                        if (oldBmp != null)
                        {
                            oldBmp.Dispose();
                        }
                        oldBmp = bmp;
                    }));
                };
                rtmp.Start(show, "udp://@226.0.0.80:8001");
            }
            catch (Exception ex)
            {
                ;
            }
        }
        /// <summary>
        /// 创建文件夹用来保存视频
        /// </summary>
        private static string SaveVideoFile()
        {
            string videoPath = System.IO.Directory.GetCurrentDirectory() + "\\rtsp_Video";
            if (!Directory.Exists(videoPath))
            {
                Directory.CreateDirectory(videoPath);
            }
            return videoPath;
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            // 获取PictureBox的宽度和高度
            int width = pic.Width;
            int height = pic.Height;
            // 计算相对于PictureBox坐标系下的位置
            float mappedX = (2f * e.X / width) - 1;
            float mappedY = 1 - (2f * e.Y / height);
            // Console.WriteLine($"屏幕坐标 X: {mappedX}, Y: {mappedY}");
            // MessageBox.Show($"屏幕坐标 X: {mappedX}, Y: {mappedY}");
            pitch_angle += (float)(pitch_range / 2.0 * mappedY);
            azimuth_angle += (float)(azimuth_range / 2.0 * mappedX);
            // 触发事件，传递两个 float 数据
            OnDataProcessed?.Invoke(pitch_angle, azimuth_angle);
        }

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            pic.Cursor = Cursors.Cross;
        }

        private void pic_MouseLeave(object sender, EventArgs e)
        {
            // 当鼠标离开PictureBox时，将鼠标指针恢复为默认样式
            pic.Cursor = Cursors.Default;
        }

        private void pic_Click(object sender, EventArgs e)
        {

        }

        private void VideoControl_Load(object sender, EventArgs e)
        {
            ThreadStart();
        }

        private void pic_MouseClick_1(object sender, MouseEventArgs e)
        {
            // 获取PictureBox的宽度和高度
            int width = 640;// pic.Width;
            int height = 512;// pic.Height;
            // 计算相对于PictureBox坐标系下的位置
            float mappedX = (2f * e.X / width) - 1;
            float mappedY = 1 - (2f * e.Y / height);
            // Console.WriteLine($"屏幕坐标 X: {mappedX}, Y: {mappedY}");
            // MessageBox.Show($"屏幕坐标 X: {mappedX}, Y: {mappedY}");
            pitch_angle += (float)(pitch_range / 2.0 * mappedY);
            azimuth_angle += (float)(azimuth_range / 2.0 * mappedX);
            // 触发事件，传递两个 float 数据
            //OnDataProcessed?.Invoke(pitch_angle, azimuth_angle);
            OnDataProcessed(e.X, e.Y);

            if (AutoSendToFK)
            {
                UInt16 x = Convert.ToUInt16(e.X);
                UInt16 y = Convert.ToUInt16(e.Y);
                UInt32 frameNo = 0;
                byte[] sendData = new byte[8];
                Buffer.BlockCopy(BitConverter.GetBytes(x), 0, sendData, 0, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(y), 0, sendData, 2, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(frameNo), 0, sendData, 4, 4);
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan,
                    8, 0xD3, sendData);
            }
        }
    }
}
