using DevExpress.LookAndFeel;
using SharpGL;
using SharpGL.SceneGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace GroundLunch
{
    public partial class TMHDU : DevExpress.XtraEditors.XtraUserControl
    {
        public TMHDU()
        {
            InitializeComponent();
            //开启定时器
            StartTimer();
        }

        private static System.Timers.Timer timerhdu;

        OpenGL gl;

        public class _PitchAndRoll
        {
            static public double Rect_WidthL = -0.5f;
            static public double Rect_WidthR = 0.5f;
            static public double Rect_HeightT = 0.6f;
            static public double Rect_HeightB = -0.5f;
            static public double Rect_Width = -0.5f;

            static public double Yaw_Rect_WidthL;
            static public double Yaw_Rect_WidthR;
            static public double Yaw_Rect_HeightT;
            static public double Yaw_Rect_HeightB;
            static public double Yaw_Rect_Width;

            static public double radius = 100.5f;//圆的半径

            static public double RmidTickLength = 10.03f;  //0°/10°/20°/45°
            static public double RmaxTickLength = 10.06f; //30°

            //三角形的位置
            static public double widthl = -0.03f;
            static public double widthr = 0.03f;
            static public double bottom = 0.55f;

            public const int divisions = 24;//刻度数量
            static public double pitchgrid_height = 0.045f;   //每度的俯仰角所占的高度像素
            static public double PminTickLength = 0.05f;
            static public double PmidTickLength = 0.1f;
            static public double PmaxTickLength = 0.2f;

            //飞机两翼的位置
            static public double rudderwidthl = Rect_WidthL + 0.03f;
            static public double rudderwidthr = Rect_WidthL + 0.25f;
            static public double rudderheight = 0.018f;
            static public double rudderwidthmr = Rect_WidthL + 0.22f;
            static public double rudderheightm = -0.08f;

            //基准单位i
            static public double standUnit = 0;
        }

        //输入变量
        public struct _input
        {
            public float pitch;
            public float yaw;
            public float roll;
            public float height;
            public float speed;
            public _input(float Pitch, float Yaw, float Roll, int Height, int Speed)
            {
                pitch = Pitch;
                yaw = Yaw;
                roll = Roll;
                height = Height;
                speed = Speed;
            }
        }
        _input MyInput;
        //速度参数
       
        
        private void StartTimer()
        {
            //初始化定时器
            timerhdu = new System.Timers.Timer(100);
            //注册定时器
            timerhdu.Elapsed += new ElapsedEventHandler(Timer1_Tick);
            //设置定时器为可重复触发
            timerhdu.AutoReset = true;
            //启动定时器
            timerhdu.Enabled = true;
        }

        private void Timer1_Tick(object sender, ElapsedEventArgs e)
        {

            Action action = () =>
            {
                openGLHDU.Invalidate();
            };

            if (this.IsHandleCreated)
            {
                Invoke(action);
            }
        }

        private void ResetParam()
        {
            //高度划分为110°的间隔
            _PitchAndRoll.pitchgrid_height = openGLHDU.Height / 90.0; 

            //俯仰刻度线的长度计算，要考虑竖长形和宽扁形的控件
            if(openGLHDU.Width > openGLHDU.Height)
            {
                //宽度足够，以高度做限制计算
                _PitchAndRoll.PminTickLength = openGLHDU.Height * 0.08;
                _PitchAndRoll.PmidTickLength = openGLHDU.Height * 0.12;
                _PitchAndRoll.PmaxTickLength = openGLHDU.Height * 0.16;

                _PitchAndRoll.radius = openGLHDU.Height * 0.4;
                _PitchAndRoll.RmidTickLength = openGLHDU.Height * 0.04;
                _PitchAndRoll.RmaxTickLength = openGLHDU.Height * 0.06;

                _PitchAndRoll.widthl = -openGLHDU.Height * 0.025;
                _PitchAndRoll.widthr = openGLHDU.Height * 0.025;
                _PitchAndRoll.bottom = openGLHDU.Height * 0.46;
               // _PitchAndRoll.Rect_HeightT = openGLHDU.Height * 0.5;

                _PitchAndRoll.standUnit = openGLHDU.Height * 0.008;

                _PitchAndRoll.rudderwidthl = openGLHDU.Height * 0.10;
                _PitchAndRoll.rudderwidthr = openGLHDU.Height * 0.3;
                _PitchAndRoll.rudderheight = _PitchAndRoll.standUnit * 1.7;

                _PitchAndRoll.rudderwidthmr = _PitchAndRoll.rudderwidthl + _PitchAndRoll.standUnit * 3.4;
                _PitchAndRoll.rudderheightm = -openGLHDU.Height * 0.05;

                _PitchAndRoll.Rect_WidthL = -openGLHDU.Width * 0.5 + openGLHDU.Height * 0.03;
                _PitchAndRoll.Rect_WidthR = openGLHDU.Width * 0.5 - openGLHDU.Height * 0.03;
                _PitchAndRoll.Rect_Width = openGLHDU.Height * 0.15;
                _PitchAndRoll.Rect_HeightT = openGLHDU.Height * 0.45;
                _PitchAndRoll.Rect_HeightB = -openGLHDU.Height * 0.45;

                _PitchAndRoll.Yaw_Rect_WidthL = -openGLHDU.Width * 0.5 + openGLHDU.Height * 0.03;
                _PitchAndRoll.Yaw_Rect_WidthR = openGLHDU.Width * 0.5 - openGLHDU.Height * 0.03;
                _PitchAndRoll.Yaw_Rect_HeightT = -openGLHDU.Height * 0.35;
                _PitchAndRoll.Yaw_Rect_HeightB = -openGLHDU.Height * 0.45;
            }
            else
            {
                _PitchAndRoll.PminTickLength = openGLHDU.Width * 0.02;
                _PitchAndRoll.PmidTickLength = openGLHDU.Width * 0.03;
                _PitchAndRoll.PmaxTickLength = openGLHDU.Width * 0.04;
            }
            
        }

        private void ResetProjectMatrix()
        {
            ResetParam();
            //  设置当前矩阵模式,对投影矩阵应用随后的矩阵操作
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            // 重置当前指定的矩阵为单位矩阵,将当前的用户坐标系的原点移到了屏幕中心
            gl.LoadIdentity();

            
            //抗锯齿
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
            
            // 创建透视投影变换
            //  gl.Perspective(10.0f, (double)Width / (double)Height, 5, 10000.0);
            if (openGLHDU.Width <= 1)
                return;

            _3DPoint eyePos = new _3DPoint(0, 0, 10);

            //正视投影，其中的near，far参数针对观察点的距离，而不是原点距离
            gl.Ortho(-openGLHDU.Width * 0.5, openGLHDU.Width * 0.5, -openGLHDU.Height * 0.5, openGLHDU.Height * 0.5, eyePos.z - 1, eyePos.z + 1);

            // 视点变换
            gl.LookAt(eyePos.x, eyePos.y, eyePos.z, 0, 0, 0, 0, 1, 0);
            // 设置当前矩阵为模型视图矩阵
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void openGLHDU_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = openGLHDU.OpenGL;
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);//设置背景颜色
            
            //输入变量初始化
            MyInput = new _input(0f, 0f, 0.0f, 130, 0);
        }

        private void openGLHDU_Resized(object sender, EventArgs e)
        {
            ResetProjectMatrix();
        }

        private void openGLHDU_OpenGLDraw(object sender, RenderEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //设置视图
            
            gl.LoadIdentity();//画图象之前模型矩阵得先写
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            Draw();
        }

        private void Draw()
        {
            DrawCoordinate();
            
            //绘制表滚转角和偏航角的部分
            drawRollAndPitch(MyInput.roll, MyInput.pitch);

            //先绘制左右两侧速度高度
            
             drawTickLineSpd(MyInput.speed);
            
             drawTickLineHeight(MyInput.height);
            //再绘制底部偏航角
            
           // _stdnumb stdYaw = new _stdnumb(_yaw.Rect_WidthL, _yaw.Rect_WidthR, _yaw.Rect_HeightT, _yaw.Rect_HeightB, _yaw.eachgrid_num, _yaw.minTickLength, _yaw.maxTickLength, _yaw.attribute, _yaw.tickoffset);
            drawTickLineYaw(MyInput.yaw);
            

            gl.Flush();
        }

        //绘制表滚转角和偏航角的部分
        private void drawRollAndPitch(float roll, double pitch)
        {

            //图像跟随滚转角旋转
            gl.Rotate(0, 0, roll);
            roll = -roll;
            //地平线高度计算，俯仰角越高，地平线越向下
            double Horizon_h = -1.0 * pitch * _PitchAndRoll.pitchgrid_height;
            //地平线往下 地面
            gl.Color(0.7608f, 0.5569f, 0.0f);
            drawRect(-Width , Width , Horizon_h, -Height);
            //地平线往上 天空
            gl.Color(0.5294f, 0.808f, 0.9412f);
            drawRect(-Width, Width, Horizon_h, Height);

            //画俯仰刻度线
            gl.LineWidth(3.0f);//需在Color前，后续文字才能显示,效果才能展现  
            gl.Color(1.0f, 1.0f, 1.0f);
            drawLines(-Width * 0.5, Width * 0.5, Horizon_h, 0.0f);


            //只绘制当前俯仰角的前后22°的线
            for (int i = 1; i <= _PitchAndRoll.divisions; i++)
            {
                if (i % 4 == 1 || i % 4 == 3)
                {
                    if((i * 2.5 > pitch - 22) && (i * 2.5 < pitch + 22))
                        drawLines(-_PitchAndRoll.PminTickLength / 2, _PitchAndRoll.PminTickLength / 2, Horizon_h + i * _PitchAndRoll.pitchgrid_height * 2.5, 0.0f);
                    if (-i * 2.5 > pitch - 22 && -i * 2.5 < pitch + 22)
                        drawLines(-_PitchAndRoll.PminTickLength / 2, _PitchAndRoll.PminTickLength / 2, Horizon_h - i * _PitchAndRoll.pitchgrid_height * 2.5, 0.0f);
                }
                if (i % 4 == 2)
                {
                    if ((i * 2.5 > pitch - 22) && (i * 2.5 < pitch + 22))
                        drawLines(-_PitchAndRoll.PmidTickLength / 2, _PitchAndRoll.PmidTickLength / 2, Horizon_h + i * _PitchAndRoll.pitchgrid_height * 2.5, 0.0f);
                    if (-i * 2.5 > pitch - 22 && -i * 2.5 < pitch + 22)
                        drawLines(-_PitchAndRoll.PmidTickLength / 2, _PitchAndRoll.PmidTickLength / 2, Horizon_h - i * _PitchAndRoll.pitchgrid_height * 2.5, 0.0f);
                }
                if (i % 4 == 0)
                {
                    if ((i * 2.5 > pitch - 22) && (i * 2.5 < pitch + 22))
                    { 
                        drawLines(-_PitchAndRoll.PmaxTickLength / 2, _PitchAndRoll.PmaxTickLength / 2, Horizon_h + i * _PitchAndRoll.pitchgrid_height * (2.5), 0.0f);
                        double a = _PitchAndRoll.PmaxTickLength / 2 + 6;
                        double b = Horizon_h + i * _PitchAndRoll.pitchgrid_height * 2.5 - 6;
                        double c = Math.Sqrt(a * a + b * b);

                        double alpha = Math.Atan2(a, b);
                        double x = Math.Cos(Math.PI / 2 - (alpha + roll / 57.3)) * c;
                        double y = Math.Sin(Math.PI / 2 - (alpha + roll / 57.3)) * c;
                        gl.DrawText((int)(openGLHDU.Width * 0.5 + x), (int)(openGLHDU.Height * 0.5 + y), (float)1.0, (float)1.0, (float)1.0, "黑体", (float)(_PitchAndRoll.pitchgrid_height * 4.0), string.Format("{0:F0}", i * 2.5));
                    }
                    //gl.DrawText((int)(openGLHDU.Width * 0.5 + _PitchAndRoll.PmaxTickLength / 2 + 4), (int)(openGLHDU.Height * 0.5 + (Horizon_h + i * _PitchAndRoll.pitchgrid_height * 2.5 - 6)), (float)1.0, (float)1.0, (float)1.0, "黑体", (float)(_PitchAndRoll.pitchgrid_height * 4.0), string.Format("{0:F0}", i * 2.5));
                    gl.Color(1f, 1f, 1f);
                    if (-i * 2.5 > pitch - 22 && -i * 2.5 < pitch + 22)
                    {
                        drawLines(-_PitchAndRoll.PmaxTickLength / 2, _PitchAndRoll.PmaxTickLength / 2, Horizon_h - i * _PitchAndRoll.pitchgrid_height * 2.5, 0.0f);

                    
                        double a = _PitchAndRoll.PmaxTickLength / 2 + 6;
                        double b = Horizon_h - i * _PitchAndRoll.pitchgrid_height * 2.5 - 6;
                        double c = Math.Sqrt(a * a + b * b);

                        double alpha = Math.Atan2(a, b);
                        double x = Math.Cos(Math.PI / 2 - (alpha + roll / 57.3)) * c;
                        double y = Math.Sin(Math.PI / 2 - (alpha + roll / 57.3)) * c;
                        gl.DrawText((int)(openGLHDU.Width * 0.5 + x), (int)(openGLHDU.Height * 0.5 + y), (float)1.0, (float)1.0, (float)1.0, "黑体", (float)(_PitchAndRoll.pitchgrid_height * 4.0), string.Format("{0:F0}", i * 2.5));
                        // gl.DrawText((int)(openGLHDU.Width * 0.5 + _PitchAndRoll.PmaxTickLength / 2 + 4), (int)(openGLHDU.Height * 0.5 + (Horizon_h - i * _PitchAndRoll.pitchgrid_height * 2.5 - 6)), (float)1.0, (float)1.0, (float)1.0, "黑体", (float)(_PitchAndRoll.pitchgrid_height * 4.0), string.Format("{0:F0}", i * 2.5));
                        //gl.DrawText((int)((_PitchAndRoll.Rect_WidthL + openGLHDU.Width * 0.5)), (int)((openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightT - 20)), 1.0f, 1.0f, 0.0f, "微软雅黑", 20f, "speed");

                    }
                    gl.Color(1f, 1f, 1f);
                }
            }
            //画滚转刻度线
            for (int i = 90; i >= 50; i -= 5)
            {
                double Outradius = 0.0f;
                if (i % 15 == 0)
                {
                    Outradius = _PitchAndRoll.radius + _PitchAndRoll.RmaxTickLength;
                }
                else 
                {
                    Outradius = _PitchAndRoll.radius + _PitchAndRoll.RmidTickLength;
                }
                double angle_r = Math.PI * i / 180;
                double angle_l = Math.PI * (180 - i) / 180;

                float satrtXr = (float)(_PitchAndRoll.radius * Math.Cos(angle_r));
                float satrtYr = (float)(_PitchAndRoll.radius * Math.Sin(angle_r));
                float endXr = (float)(Outradius * Math.Cos(angle_r));
                float endYr = (float)(Outradius * Math.Sin(angle_r));

                gl.Begin(OpenGL.GL_LINES);
                gl.Vertex(satrtXr, satrtYr);
                gl.Vertex(endXr, endYr);
                gl.End();
                float satrtXl = (float)(_PitchAndRoll.radius * Math.Cos(angle_l));
                float satrtYl = (float)(_PitchAndRoll.radius * Math.Sin(angle_l));
                float endXl = (float)(Outradius * Math.Cos(angle_l));
                float endYl = (float)(Outradius * Math.Sin(angle_l));
                gl.Begin(OpenGL.GL_LINES);
                gl.Vertex(satrtXl, satrtYl);
                gl.Vertex(endXl, endYl);
                gl.End();

                if (i == 90)
                {
                    //绘制0度指示三角形
                    gl.Color(1.0, 0.0, 1.0);
                    gl.Begin(OpenGL.GL_TRIANGLES);
                    gl.Vertex(satrtXr, satrtYr - _PitchAndRoll.standUnit * 2);
                    gl.Vertex(satrtXr - _PitchAndRoll.standUnit * 3.5, satrtYr - _PitchAndRoll.standUnit * 8);
                    gl.Vertex(satrtXr + _PitchAndRoll.standUnit * 3.5, satrtYr - _PitchAndRoll.standUnit * 8);
                    gl.End();
                    gl.Color(1.0, 1.0, 1.0);
                }
                if (i == 75)
                {
                    //绘制15度指示三角形
                    gl.Color(0.0, 1.0, 0.0);
                    gl.Begin(OpenGL.GL_TRIANGLES);
                    gl.Vertex(satrtXr - _PitchAndRoll.standUnit * 0.5, satrtYr - _PitchAndRoll.standUnit * 2);
                    gl.Vertex(satrtXr - _PitchAndRoll.standUnit * 3.5, satrtYr - _PitchAndRoll.standUnit * 5);
                    gl.Vertex(satrtXr + _PitchAndRoll.standUnit * 0.5, satrtYr - _PitchAndRoll.standUnit * 6);

                    gl.Vertex(satrtXl + _PitchAndRoll.standUnit * 0.5, satrtYl - _PitchAndRoll.standUnit * 2);
                    gl.Vertex(satrtXl - _PitchAndRoll.standUnit * 0.5, satrtYl - _PitchAndRoll.standUnit * 6);
                    gl.Vertex(satrtXl + _PitchAndRoll.standUnit * 3.5, satrtYl - _PitchAndRoll.standUnit * 5);

                    gl.End();
                    gl.Color(1.0, 1.0, 1.0);
                }
            }

             //画不动的图像,          
            gl.LoadIdentity();

            //滚转角指示-三角形
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Vertex(_PitchAndRoll.widthl, openGLHDU.Height * 0.5);
            gl.Vertex(_PitchAndRoll.widthr, openGLHDU.Height * 0.5);
            gl.Vertex(0.0f, _PitchAndRoll.bottom);
            gl.End();

            
            //飞机
            //鼻翼,黄色外框，黑色内框
            gl.Color(1.0f, 1.0f, 0.0f);
            drawRect(-_PitchAndRoll.standUnit * 1.7, _PitchAndRoll.standUnit * 1.7, _PitchAndRoll.standUnit * 1.7, -_PitchAndRoll.standUnit * 1.7);
            gl.Color(0.0f, 0.0f, 0.0f);
            drawRect(-_PitchAndRoll.standUnit, _PitchAndRoll.standUnit, _PitchAndRoll.standUnit, -_PitchAndRoll.standUnit);
            
            //机翼
            for (int i = -1; i <= 1; i += 2)
            {
                gl.Color(1.0f, 1.0f, 0.0f);
                drawRect(_PitchAndRoll.rudderwidthl * i, _PitchAndRoll.rudderwidthr * i, _PitchAndRoll.rudderheight, -_PitchAndRoll.rudderheight);
                drawRect(_PitchAndRoll.rudderwidthmr * i, _PitchAndRoll.rudderwidthl * i, _PitchAndRoll.rudderheight, _PitchAndRoll.rudderheightm);
                gl.Color(0.0f, 0.0f, 0.0f);
                drawRect((_PitchAndRoll.rudderwidthl + _PitchAndRoll.standUnit * 0.8) * i, (_PitchAndRoll.rudderwidthr - _PitchAndRoll.standUnit * 0.5) * i, _PitchAndRoll.rudderheight * 0.59, -_PitchAndRoll.rudderheight * 0.59);
                drawRect((_PitchAndRoll.rudderwidthmr - _PitchAndRoll.standUnit * 0.5) * i, (_PitchAndRoll.rudderwidthl + _PitchAndRoll.standUnit * 0.8) * i, _PitchAndRoll.rudderheight * 0.59, _PitchAndRoll.rudderheightm * 0.97);
            }
            
            //两侧底色绘制
            gl.Color(0.0f, 0.0f, 0.0f);
            //左
            drawRect(_PitchAndRoll.Rect_WidthL, _PitchAndRoll.Rect_WidthL + _PitchAndRoll.Rect_Width, _PitchAndRoll.Rect_HeightT, _PitchAndRoll.Rect_HeightB);
            //右
            drawRect(_PitchAndRoll.Rect_WidthR, _PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width, _PitchAndRoll.Rect_HeightT, _PitchAndRoll.Rect_HeightB);
        }

        //绘制高度速度
        private void drawTickLineSpd(double inputnum)
        {
            //确定数值显示(先画框再显示字)
            //根据输入值生成11个数据
            //inputnum = -1.5;
            int[] num = new int[15];
            int inputnumI = (int)(inputnum);
            for (int i = 0; i < 15; i++)
            {
                num[i] = i  + inputnumI - 7;
                string strSpd = string.Format("{0:D2}", num[i]); 
                double strLen = (3 - strSpd.Length) * _PitchAndRoll.standUnit * 4.3;
                int x = (int)((_PitchAndRoll.Rect_WidthL + openGLHDU.Width * 0.5 + _PitchAndRoll.Rect_Width * 0.17) + strLen);
                int y = (int)((openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightB + (_PitchAndRoll.Rect_HeightT - _PitchAndRoll.Rect_HeightB) * 0.08 * (i - (inputnum - inputnumI) - 1.04)));
                if (y < openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightT - _PitchAndRoll.standUnit * 6.4 &&
                    y > openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightB) 
                    gl.DrawText(x, y, 0.8f, 0.8f, 1.0f, "黑体", (float)(_PitchAndRoll.standUnit * 6.4), strSpd);
                
            }

            //在中间画框框
            gl.Color(0.8, 1.0, 0.0);
            gl.LineWidth(4);
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Vertex(_PitchAndRoll.Rect_WidthL - 3, _PitchAndRoll.standUnit * 5.0);
            gl.Vertex(_PitchAndRoll.Rect_WidthL + _PitchAndRoll.Rect_Width + 3, _PitchAndRoll.standUnit * 5.0);
            gl.Vertex(_PitchAndRoll.Rect_WidthL + _PitchAndRoll.Rect_Width + 3, _PitchAndRoll.standUnit * -5.0);
            gl.Vertex(_PitchAndRoll.Rect_WidthL - 3, _PitchAndRoll.standUnit * -5.0);
            gl.Vertex(_PitchAndRoll.Rect_WidthL - 3, _PitchAndRoll.standUnit * 5.0);
            gl.End();

            gl.Color(0, 0, 0.0);
            drawRect(_PitchAndRoll.Rect_WidthL, _PitchAndRoll.Rect_WidthL + _PitchAndRoll.Rect_Width, _PitchAndRoll.Rect_HeightT, _PitchAndRoll.Rect_HeightT - _PitchAndRoll.standUnit * 10);
            gl.DrawText((int)((_PitchAndRoll.Rect_WidthL + openGLHDU.Width * 0.5 + 3)), (int)((openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightT - 20)), 0.0f, 1.0f, 0.0f, "黑体", (float)(_PitchAndRoll.standUnit * 4.4), "AirSpd");
        }

        //绘制高度速度
        private void drawTickLineHeight(double inputnum)
        {
            //确定数值显示(先画框再显示字)
            //根据输入值生成11个数据
            //inputnum = 751.5;
            int[] num = new int[15];
            double a = Math.Abs(inputnum % 10);
            double b = inputnum % 10;
            int inputnumI = (int)(inputnum - a);
            for (int i = 0; i < 15; i++)
            {
                num[i] = i * 10 + inputnumI - 70;
                string strHeight = string.Format("{0:D3}", num[i]);
                double strLen = (4 - strHeight.Length) * _PitchAndRoll.standUnit * 4.3;
                int x = (int)((_PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width + openGLHDU.Width * 0.5 + _PitchAndRoll.Rect_Width * 0.1) + strLen);
                int y = (int)((openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightB + (_PitchAndRoll.Rect_HeightT - _PitchAndRoll.Rect_HeightB) * 0.08 * (i - (b/10) - 1.04)));
                if (y < openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightT - _PitchAndRoll.standUnit * 6.4 &&
                    y > openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightB)
                    gl.DrawText(x, y, 0.8f, 0.8f, 1.0f, "黑体", (float)(_PitchAndRoll.standUnit * 6.4), strHeight);

            }

            //在中间画框框
            gl.Color(0.8, 1.0, 0.0);
            gl.LineWidth(4);
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Vertex(_PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width - 3, _PitchAndRoll.standUnit * 5.0);
            gl.Vertex(_PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width + _PitchAndRoll.Rect_Width + 3, _PitchAndRoll.standUnit * 5.0);
            gl.Vertex(_PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width + _PitchAndRoll.Rect_Width + 3, _PitchAndRoll.standUnit * -5.0);
            gl.Vertex(_PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width - 3, _PitchAndRoll.standUnit * -5.0);
            gl.Vertex(_PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width - 3, _PitchAndRoll.standUnit * 5.0);
            gl.End();

            gl.Color(0, 0, 0.0);
            drawRect(_PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width, _PitchAndRoll.Rect_WidthR , _PitchAndRoll.Rect_HeightT, _PitchAndRoll.Rect_HeightT - _PitchAndRoll.standUnit * 10);
            gl.DrawText((int)((_PitchAndRoll.Rect_WidthR - _PitchAndRoll.Rect_Width + openGLHDU.Width * 0.5 + 5)), (int)((openGLHDU.Height * 0.5 + _PitchAndRoll.Rect_HeightT - 20)), 0.0f, 1.0f, 0.0f, "黑体", (float)(_PitchAndRoll.standUnit * 4.4), "Height");
        }
        //绘制偏航

        private void drawTickLineYaw(double inputnum)
        {
            //先将底色变为灰色
            gl.Color(0.502f, 0.502f, 0.502f);

            drawRect(_PitchAndRoll.Yaw_Rect_WidthL, _PitchAndRoll.Yaw_Rect_WidthR, _PitchAndRoll.Yaw_Rect_HeightT, _PitchAndRoll.Yaw_Rect_HeightB);

            gl.LineWidth(2);
            gl.Color(1.0f, 1.0f, 1.0f);//刻度线为白色
            gl.Begin(OpenGL.GL_LINES);

            //矩形外侧线条
            gl.Vertex(_PitchAndRoll.Yaw_Rect_WidthL, _PitchAndRoll.Yaw_Rect_HeightB);
            gl.Vertex(_PitchAndRoll.Yaw_Rect_WidthL, _PitchAndRoll.Yaw_Rect_HeightT);
            gl.Vertex(_PitchAndRoll.Yaw_Rect_WidthL, _PitchAndRoll.Yaw_Rect_HeightT);
            gl.Vertex(_PitchAndRoll.Yaw_Rect_WidthR, _PitchAndRoll.Yaw_Rect_HeightT);
            gl.Vertex(_PitchAndRoll.Yaw_Rect_WidthR, _PitchAndRoll.Yaw_Rect_HeightT);
            gl.Vertex(_PitchAndRoll.Yaw_Rect_WidthR, _PitchAndRoll.Yaw_Rect_HeightB);
            gl.End();

            //inputnum = 17.0;
            int[] num = new int[15];
            double a = Math.Abs(inputnum % 10);
            double b = inputnum % 10;
            int inputnumI = (int)(inputnum - a);

            for (int i = 0; i < 15; i++)
            {
                num[i] = i * 10 + inputnumI - 70;
                if (num[i] < 0)
                    num[i] = 360 + num[i];
                if (num[i] > 359)
                    num[i] -= 360;
                string strYaw = string.Format("{0:D3}", num[i]);
                double gap = _PitchAndRoll.standUnit * 4.3;
                double strLen = 3 * _PitchAndRoll.standUnit * 4.3 + gap;
                int x = (int)(openGLHDU.Width * 0.5 + (strLen * (i - 7 -(b/10))) - strLen * 0.375);
                int y = (int)(openGLHDU.Height * 0.5 + _PitchAndRoll.Yaw_Rect_HeightB + _PitchAndRoll.standUnit * 3);
                if (x < openGLHDU.Width * 0.5 + _PitchAndRoll.Yaw_Rect_WidthR - strLen &&
                    x > openGLHDU.Width * 0.5 + _PitchAndRoll.Yaw_Rect_WidthL)
                {
                    gl.DrawText(x, y, 1.0f, 1.0f, 1.0f, "黑体", (float)(_PitchAndRoll.standUnit * 6.4), strYaw);
                    x = (int)(strLen * (i - 7 - (b / 10)));
                    gl.Begin(OpenGL.GL_LINES);
                    gl.Vertex(x, _PitchAndRoll.Yaw_Rect_HeightT);
                    gl.Vertex(x, _PitchAndRoll.Yaw_Rect_HeightT - _PitchAndRoll.standUnit * 3);
                    gl.Vertex(x + strLen * 0.5, _PitchAndRoll.Yaw_Rect_HeightT);
                    gl.Vertex(x + strLen * 0.5, _PitchAndRoll.Yaw_Rect_HeightT - _PitchAndRoll.standUnit * 2);
                    gl.Vertex(x - strLen * 0.5, _PitchAndRoll.Yaw_Rect_HeightT);
                    gl.Vertex(x - strLen * 0.5, _PitchAndRoll.Yaw_Rect_HeightT - _PitchAndRoll.standUnit * 2);
                    gl.End();
                }
            }
            gl.Color(1.0f, 1.0f, 0.0f);
            gl.LineWidth(3);
            drawLines(0.0f, 0.0f, _PitchAndRoll.Yaw_Rect_HeightT + _PitchAndRoll.RmaxTickLength, _PitchAndRoll.Yaw_Rect_HeightB );
        }
        //画矩形
        private void drawRect(double widthLeft, double widthRight, double heightTop, double heightBottom)
        {
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(widthLeft, heightTop);
                gl.Vertex(widthRight, heightTop);
                gl.Vertex(widthRight, heightBottom);
                gl.Vertex(widthLeft, heightBottom);
            }
            gl.End();
        }

        //画直线
        private void drawLines(double wl, double wr, double ht, double hb)
        {
            gl.Begin(OpenGL.GL_LINES);
            if (Math.Abs(wl) > float.Epsilon && Math.Abs(wr) > float.Epsilon)
            {
                gl.Vertex(wl, ht);
                gl.Vertex(wr, ht);
            }
            else
            {
                gl.Vertex(wl, ht);
                gl.Vertex(wr, hb);
            }
            gl.End();
        }

        //输入飞机姿态和高度速度数据
        public void SetLocation(double pitch, double yaw, double roll, double height, double speed)
        {
            MyInput.pitch = (float)pitch;
            MyInput.yaw = (float)yaw;
            MyInput.roll = (float)roll;
            MyInput.height = (float)height;
            MyInput.speed = (float)speed;
        }

        public void DrawCoordinate()
        {
            gl.LineWidth(3);

            //x
            gl.Color(1.0, 0, 0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0);
            gl.Vertex(Width * 0.49, 0, 0);
            gl.End();

            //y
            gl.Color(0, 1.0, 0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, openGLHDU.Height * 0.49, 0);
            gl.Vertex(0, -openGLHDU.Height * 0.49, 0);
            gl.End();

            //z
            gl.Color(0, 0, 1.0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, 1000);
            gl.End();

            gl.Color(1.0, 1.0, 1.0);
            gl.LineWidth(1);

        }
    }

    public class _3DPoint
    {
        public double x;
        public double y;
        public double z;

        public _3DPoint(double x, double y, double z) 
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
