using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Charts.Native;
using DevExpress.XtraDashboardLayout;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using SharpGL;
using SharpGL.SceneGraph.Quadrics;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace GroundLunch
{
    public partial class Mod3D : DevExpress.XtraEditors.XtraUserControl
    {
        List<Point> points = new List<Point>();
        public List<Triangle> triangles = new List<Triangle>();
        OpenGL gl;
        string modelFile = ".\\模型\\123.stl";
        DRAW_MODEL curModel = DRAW_MODEL.MODEL_3D;
        double centerx;
        double centery;
        double centerz;
        double scale;
        double scaleSource;

        public float pitch = 0;
        public float yaw = 0;
        public float roll = 0;

        public Mod3D()
        {
            InitializeComponent();
            AnalyzeModel(modelFile);
        }

        public void RefreshUI()
        {
            // Invalidate();
            pitch = (float)NetDataHandle.groundFrame[NetDataHandle.curSelMsn].navPitch;
            yaw = (float)NetDataHandle.groundFrame[NetDataHandle.curSelMsn].navYaw;
            roll = (float)NetDataHandle.groundFrame[NetDataHandle.curSelMsn].navRoll;

            //yaw += 10;
        }

        public void LoadStlFile(string fileName)
        {
            triangles.Clear();
           // points.Clear();
            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            double maxx = 0, maxy = 0, maxz = 0;
            double minx = 10000, miny = 10000, minz = 10000;
            byte[] buffer = new byte[1024];
            buffer = br.ReadBytes(80);
            buffer = br.ReadBytes(4);
            uint triangleCount = BitConverter.ToUInt32(buffer, 0);
            for (uint i = 0; i < triangleCount; i++)
            {
                Triangle triangle = new Triangle();
                buffer = br.ReadBytes(12 * 4);
                triangle.normalSource.x = BitConverter.ToSingle(buffer, 0 * 4);
                triangle.normalSource.y = BitConverter.ToSingle(buffer, 1 * 4);
                triangle.normalSource.z = BitConverter.ToSingle(buffer, 2 * 4);
                triangle.pt1Source.x = BitConverter.ToSingle(buffer, 3 * 4);
                triangle.pt1Source.y = BitConverter.ToSingle(buffer, 4 * 4);
                triangle.pt1Source.z = BitConverter.ToSingle(buffer, 5 * 4);
                triangle.pt2Source.x = BitConverter.ToSingle(buffer, 6 * 4);
                triangle.pt2Source.y = BitConverter.ToSingle(buffer, 7 * 4);
                triangle.pt2Source.z = BitConverter.ToSingle(buffer, 8 * 4);
                triangle.pt3Source.x = BitConverter.ToSingle(buffer, 9 * 4);
                triangle.pt3Source.y = BitConverter.ToSingle(buffer, 10 * 4);
                triangle.pt3Source.z = BitConverter.ToSingle(buffer, 11 * 4);
                triangles.Add(triangle);
               // points.Add(triangle.pt1Source);
               // points.Add(triangle.pt2Source);
               // points.Add(triangle.pt3Source);

                Point pt = triangle.pt1Source;
                if (pt.x > maxx) { maxx = pt.x; }
                if (pt.y > maxy) { maxy = pt.y; }
                if (pt.z > maxz) { maxz = pt.z; }
                if (pt.x < minx) { minx = pt.x; }
                if (pt.y < miny) { miny = pt.y; }
                if (pt.z < minz) { minz = pt.z; }
                pt = triangle.pt2Source;
                if (pt.x > maxx) { maxx = pt.x; }
                if (pt.y > maxy) { maxy = pt.y; }
                if (pt.z > maxz) { maxz = pt.z; }
                if (pt.x < minx) { minx = pt.x; }
                if (pt.y < miny) { miny = pt.y; }
                if (pt.z < minz) { minz = pt.z; }
                pt = triangle.pt3Source;
                if (pt.x > maxx) { maxx = pt.x; }
                if (pt.y > maxy) { maxy = pt.y; }
                if (pt.z > maxz) { maxz = pt.z; }
                if (pt.x < minx) { minx = pt.x; }
                if (pt.y < miny) { miny = pt.y; }
                if (pt.z < minz) { minz = pt.z; }

                buffer = br.ReadBytes(2);
            }
            fs.Close();
            br.Close();

            centerx = (maxx + minx) / 2;
            centery = (maxy + miny) / 2;
            centerz = (maxz + minz) / 2;
            if (maxx - minx > maxy - miny)
                scaleSource = maxx - minx;
            else
                scaleSource = maxy - miny;
            if (scaleSource < maxz - minz)
                scaleSource = maxz - minz;
        }

        static double CalculateTriangleArea(double[] A, double[] B, double[] C)
        {
            // 计算向量AB和AC
            double[] AB = { B[0] - A[0], B[1] - A[1], B[2] - A[2] };
            double[] AC = { C[0] - A[0], C[1] - A[1], C[2] - A[2] };

            // 计算叉积AB x AC
            double[] crossProduct = {
            AB[1] * AC[2] - AB[2] * AC[1],
            AB[2] * AC[0] - AB[0] * AC[2],
            AB[0] * AC[1] - AB[1] * AC[0]
            };

            // 计算叉积的模长
            double crossProductLength = Math.Sqrt(
                crossProduct[0] * crossProduct[0] +
                crossProduct[1] * crossProduct[1] +
                crossProduct[2] * crossProduct[2]
            );

            // 计算三角形面积
            double area = 0.5 * crossProductLength;

            return area;
        }

        private void AdjustTrianglesByWindow()
        {
            if (openGLCtrl.Width > openGLCtrl.Height)
                scale = scaleSource / openGLCtrl.Height;
            else
                scale = scaleSource / openGLCtrl.Width;
            foreach (var triangle in triangles)
            {
                triangle.normal.x = -(triangle.normalSource.x - centerx) / scale;
                triangle.normal.y = (triangle.normalSource.y - centery) / scale;
                triangle.normal.z = (triangle.normalSource.z - centerz) / scale;

                triangle.pt1.x = -(triangle.pt1Source.x - centerx) / scale;
                triangle.pt1.y = (triangle.pt1Source.y - centery) / scale;
                triangle.pt1.z = (triangle.pt1Source.z - centerz) / scale;
                triangle.pt2.x = -(triangle.pt2Source.x - centerx) / scale;
                triangle.pt2.y = (triangle.pt2Source.y - centery) / scale;
                triangle.pt2.z = (triangle.pt2Source.z - centerz) / scale;
                triangle.pt3.x = -(triangle.pt3Source.x - centerx) / scale;
                triangle.pt3.y = (triangle.pt3Source.y - centery) / scale;
                triangle.pt3.z = (triangle.pt3Source.z - centerz) / scale;
            }
        }

        public void AnalyzeModel(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            if (fileName.EndsWith("stl"))
            {
                LoadStlFile(fileName);
                return;
            }
        }
        /***************************************************
         *
         *          绘制函数 Draw
         ***************************************************/

        private void Draw()
        {
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.Disable(OpenGL.GL_LIGHTING);
            //DrawCoordinate();
            gl.Enable(OpenGL.GL_LIGHTING);
            //CreateLight(); // sun_light_position 是固定的世界坐标系下的光源位置
            gl.PopMatrix();
          
            //gl.PopMatrix();
            
            //太阳
            {
                // 绘制太阳

                //画二次曲面球体绘制过程
               // gl.Rotate(yaw, 0.0f, -1.0f, 0.0f);
                //gl.Translate(170.0f, 0.0f, 0.0f);
                // gl.Translate(0, 200, 0);
                //drawSphere(gl);
                //return;
                //gl.PopMatrix();
            }
            
            uint i = 0;
            //gl.Rotate(-90,0,0);
            //gl.Translate(-100.0f, -60.0f, 0.0f);
             gl.Rotate(0, 90, 0);
            gl.Rotate(0, 0, roll);
            gl.Rotate(0, yaw, 0);
            gl.Rotate(-pitch, 0, 0);
            gl.Color(1.0, 1.0, 1.0);
            
            foreach (var triangle in triangles)
            {
                
                gl.Begin(OpenGL.GL_TRIANGLES);
                gl.Normal(triangle.normal.x, triangle.normal.y, triangle.normal.z);
                gl.Vertex(triangle.pt1.x, triangle.pt1.y, triangle.pt1.z);
                gl.Vertex(triangle.pt2.x, triangle.pt2.y, triangle.pt2.z);
                gl.Vertex(triangle.pt3.x, triangle.pt3.y, triangle.pt3.z);
                gl.End();
                gl.Begin(OpenGL.GL_LINE_STRIP);
                gl.Vertex(triangle.pt1.x, triangle.pt1.y, triangle.pt1.z);
                gl.Vertex(triangle.pt2.x, triangle.pt2.y, triangle.pt2.z);
                gl.Vertex(triangle.pt3.x, triangle.pt3.y, triangle.pt3.z);
                gl.End();
                //i++;
            }
            
            gl.PopMatrix();
        }

        

        private void CreateLight()
        {
            // 打开光照处理功能
            float[] light_position = { 100.0f, 60.0f, -60.0f, 1.0f };
            float[] light_ambient = { 0.0f, 0.0f, 0.0f, 1.0f }; //无环境光
            float[] light_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f }; //镜面反射白光
            float[] light_specular = { 1.0f, 1.0f, 1.0f, 1.0f };//漫反射白光
            float[] lightAttenuation = { 1.0f, 0.05f, 0.01f }; // 衰减系数
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light_position);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light_ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light_diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light_specular);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_CONSTANT_ATTENUATION, lightAttenuation[0]);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_LINEAR_ATTENUATION, lightAttenuation[1]);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_QUADRATIC_ATTENUATION, lightAttenuation[2]);

            gl.Enable(OpenGL.GL_LIGHT0);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_LIGHTING);
            return;
            //光源
            {
                // GL_POSITION 属性的值 (x,y,z,w)  w为零表示无限远 x/w,y/w,z/w 表示光源位置
                float[] sun_light_position = { 200.0f, 50.0f, 0.0f, 1.0f };
                // GL_AMBIENT 属性的值 R,G,B,A 
                float[] sun_light_ambient = { 0.0f, 0.0f, 0.0f, 0.0f };
                // GL_DIFFUSE 属性的值 R,G,B,A 
                float[] sun_light_diffuse = { 0.2f, 0.2f, 0.2f, 0.0f };
                // GL_SPECULAR 属性的值 R,G,B,A
                float[] sun_light_specular = { 0.2f, 0.2f, 0.2f, 0.0f };
                // 设置光源方向（聚光灯）
               // float[] direction = { 0.0f, -1.0f, 0.0f }; // 聚光灯的方向向量
              //  gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_DIRECTION, direction);

                // 设置聚光灯的光锥参数
                float spotCutoff = 25.0f; // 光锥的半角
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, spotCutoff);
                //光源1
                // 设置 GL_POSITION (光源位置)属性 

                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, sun_light_position);
                // 设置 GL_AMBIENT( 光源发出的光，经过非常多次反射，遗留在整个光照环境中的强度)
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, sun_light_ambient);
                // 设置 GL_DIFFUSE(光源发出的光，照射到粗糙表面时，经过漫反射，所得到的光强度)
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, sun_light_diffuse);
                // 设置 GL_SPECULAR(光源发出的光，照射到光滑表面时，经过镜面反射，所得到的光强度) 
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, sun_light_specular);
                
                //光源2
                //sun_light_position[0] = 0;
               // sun_light_position[1] = -300;
                //sun_light_position[2] = 0;
               // direction[0] = -1;
               // direction[1] = 0;
               // direction[2] = 0;

               // gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, direction);
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, spotCutoff);
                // 设置 GL_POSITION (光源位置)属性 
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, sun_light_position);
                // 设置 GL_AMBIENT( 光源发出的光，经过非常多次反射，遗留在整个光照环境中的强度)
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, sun_light_ambient);
                // 设置 GL_DIFFUSE(光源发出的光，照射到粗糙表面时，经过漫反射，所得到的光强度)
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, sun_light_diffuse);
                // 设置 GL_SPECULAR(光源发出的光，照射到光滑表面时，经过镜面反射，所得到的光强度) 
                gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, sun_light_specular);

                //光源3
                sun_light_position[0] = -280;
                // 设置 GL_POSITION (光源位置)属性 
                gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_POSITION, sun_light_position);
                // 设置 GL_AMBIENT( 光源发出的光，经过非常多次反射，遗留在整个光照环境中的强度)
                gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_AMBIENT, sun_light_ambient);
                // 设置 GL_DIFFUSE(光源发出的光，照射到粗糙表面时，经过漫反射，所得到的光强度)
                gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_DIFFUSE, sun_light_diffuse);
                // 设置 GL_SPECULAR(光源发出的光，照射到光滑表面时，经过镜面反射，所得到的光强度) 
                gl.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPECULAR, sun_light_specular);

                //光源4
                sun_light_position[0] = 0;
                sun_light_position[2] = -50;
                //sun_light_position[0] = 0;
                // 设置 GL_POSITION (光源位置)属性 
                gl.Light(OpenGL.GL_LIGHT3, OpenGL.GL_POSITION, sun_light_position);
                // 设置 GL_AMBIENT( 光源发出的光，经过非常多次反射，遗留在整个光照环境中的强度)
                gl.Light(OpenGL.GL_LIGHT3, OpenGL.GL_AMBIENT, sun_light_ambient);
                // 设置 GL_DIFFUSE(光源发出的光，照射到粗糙表面时，经过漫反射，所得到的光强度)
                gl.Light(OpenGL.GL_LIGHT3, OpenGL.GL_DIFFUSE, sun_light_diffuse);
                // 设置 GL_SPECULAR(光源发出的光，照射到光滑表面时，经过镜面反射，所得到的光强度) 
                gl.Light(OpenGL.GL_LIGHT3, OpenGL.GL_SPECULAR, sun_light_specular);


                // 开启光源
               gl.Enable(OpenGL.GL_LIGHT0);
               gl.Enable(OpenGL.GL_LIGHT1);
               gl.Enable(OpenGL.GL_LIGHT2);
               gl.Enable(OpenGL.GL_LIGHT3);
               
                // 打开景深测试
                //gl.Enable(OpenGL.GL_DEPTH_TEST);
            }
        }

        private void SetMaterial()
        {
            float[] sun_mat_ambient = { 0.4f, 0.4f, 0.4f, 1.0f };
            float[] sun_mat_diffuse = { 0.4f, 0.4f, 0.4f, 1.0f };
            float[] sun_mat_specular = { 0.5f, 0.5f, 0.5f, 1.0f };
            // 材质发光颜色 值
            float[] sun_mat_emission = { 0.0f, 0.0f, 0.0f, 1.0f };
            //  镜面指数 0 - 128 
            float sun_mat_shininess = 30.0f;

            // 设置 GL_AMBIENT (光线照射到该材质上，经过很多次反射后最终遗留在环境中的光线强度)
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT, sun_mat_ambient);
            // 设置 GL_DIFFUSE (光线照射到该材质上，经过漫反射后形成的光线强度)
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_DIFFUSE, sun_mat_diffuse);
            // 设置 GL_SPECULAR (光线照射到该材质上，经过镜面反射后形成的光线强度)
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SPECULAR, sun_mat_specular);
            // 设置 GL_EMISSION (该属性由四个值组成，表示一种颜色。OpenGL 认为该材质本身就微微的向外发射光线，以至于眼睛感觉到它有这样的颜色，但这光线又比较微弱，以至于不会影响到其它物体的颜色)
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_EMISSION, sun_mat_emission);
            // 设置 GL_SHININESS  (该属性只有一个值，称为“镜面指数”，取值范围是 0 到 128。该值越小，表示材质越粗糙，点光源发射的光线照射到上面，也可以产生较大的亮点。该值越大，表示材质越类似于镜面，光源照射到上面后，产生较小的亮点。)
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_SHININESS, sun_mat_shininess);
        }

        private void openGLCtrl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();//画图象之前模型矩阵得先写
            Draw();
        }

        private void openGLCtrl_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = openGLCtrl.OpenGL;
            gl.ClearColor(0.5294f, 0.808f, 0.9412f, 1.0f);//设置背景颜色

            CreateLight();
            SetMaterial();
            /*
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(OpenGL.GL_LESS);

            // 启用混合
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            
            // 启用抗锯齿
            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Enable(OpenGL.GL_POLYGON_SMOOTH);
            gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
            gl.Hint(OpenGL.GL_POLYGON_SMOOTH_HINT, OpenGL.GL_NICEST);
            gl.Enable(OpenGL.GL_MULTISAMPLE);
            */

        }

        private void openGLCtrl_Resized(object sender, EventArgs e)
        {
            AdjustTrianglesByWindow();
            ResetProjectMatrix();
        }

        private void ResetProjectMatrix()
        {
            //ResetParam();
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
            if (openGLCtrl.Width <= 1)
                return;

            _3DPoint eyePos = new _3DPoint(8000, 5000, 8000); ;
            switch (curModel)
            {
                case DRAW_MODEL.MODEL_3D:
                    eyePos = new _3DPoint(-8000, 6000, 9000);
                    //正视投影，其中的near，far参数针对观察点的距离，而不是原点距离
                    gl.Ortho(-openGLCtrl.Width * 0.35, openGLCtrl.Width * 0.35, -openGLCtrl.Height * 0.35, openGLCtrl.Height * 0.35, 0, 100000);
                    // 视点变换
                    gl.LookAt(eyePos.x, eyePos.y, eyePos.z, 0, -40, 0, 0, 1, 0);
                    break;
                case DRAW_MODEL.MODEL_YAW:
                    eyePos = new _3DPoint(0, 8000, 0);
                    //正视投影，其中的near，far参数针对观察点的距离，而不是原点距离
                    gl.Ortho(-openGLCtrl.Width * 0.6, openGLCtrl.Width * 0.6, -openGLCtrl.Height * 0.6, openGLCtrl.Height * 0.6, 0, 100000);

                    // 视点变换
                    gl.LookAt(eyePos.x, eyePos.y, eyePos.z, 0, 0, 0, 1, 0, 0);
                    break;
                case DRAW_MODEL.MODEL_PITCH:
                    eyePos = new _3DPoint(0, 0, 8000);
                    //正视投影，其中的near，far参数针对观察点的距离，而不是原点距离
                    gl.Ortho(-openGLCtrl.Width * 0.6, openGLCtrl.Width * 0.6, -openGLCtrl.Height * 0.6, openGLCtrl.Height * 0.6, 0, 100000);

                    // 视点变换
                    gl.LookAt(eyePos.x, eyePos.y, eyePos.z, 0, 0, 0, 0, 1, 0);
                    break;
                case DRAW_MODEL.MODEL_ROLL:
                    eyePos = new _3DPoint(-8000, 0, 0);
                    //正视投影，其中的near，far参数针对观察点的距离，而不是原点距离
                    gl.Ortho(-openGLCtrl.Width * 0.2, openGLCtrl.Width * 0.2, -openGLCtrl.Height * 0.2, openGLCtrl.Height * 0.2, 0, 100000);

                    // 视点变换
                    gl.LookAt(eyePos.x, eyePos.y, eyePos.z, 0, 0, 0, 0, 1, 0);
                    break;
            }
            

 
            
            // 设置当前矩阵为模型视图矩阵
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        public void DrawCoordinate()
        {
            gl.LineWidth(3);

            //x
            gl.Color(1.0, 0, 0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0);
            gl.Vertex(100, 0, 0);
            gl.End();

            //y
            gl.Color(0, 1.0, 0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 1000, 0);
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
        void drawSphere(OpenGL gl)
        {

            //绘制二次曲面
            var sphere = gl.NewQuadric();
            //设置二次却面绘制风格。gluQuadricDrawStyle。一般都是选用GLU_FILL风格，采用多边形来模拟
            gl.QuadricDrawStyle(sphere, OpenGL.GLU_FILL);
            //设置法线风格。gluQuadricNormals。一般都是使用GLU_SMOOTH风格，对每个顶点都计算法线向量，是默认方式
            gl.QuadricNormals(sphere, OpenGL.GLU_SMOOTH);
            //设置二次曲面的绘制方向。gluQuadricOrientation。一般使用GLU_OUTSIDE, 按照所有的法线都指向外面的方式绘制。是默认方式
            gl.QuadricOrientation(sphere, (int)OpenGL.GLU_OUTSIDE);
            //设置纹理。gluQuadricTexture。设置是否自动计算纹理。默认是GLU_FALSE。当需要使用纹理时应修改为GLU_TRUE.
            gl.QuadricTexture(sphere, (int)OpenGL.GLU_TRUE);

            gl.Sphere(sphere, 80f, 100, 32);
            gl.DeleteQuadric(sphere);
        }
        public class Point
        {
            public int ptID;
            public double x;
            public double y;
            public double z;
            public Point()
            {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public class Triangle
        {
            public Point normal = new Point(); 
            public Point pt1 = new Point();
            public Point pt2 = new Point();
            public Point pt3 = new Point();
            public Point normalSource = new Point();
            public Point pt1Source = new Point();
            public Point pt2Source = new Point();
            public Point pt3Source = new Point();
        }

        public enum DRAW_MODEL
        {
            MODEL_3D = 0,
            MODEL_YAW,
            MODEL_PITCH,
            MODEL_ROLL
        };

        private void bt3D_Click(object sender, EventArgs e)
        {
            curModel = DRAW_MODEL.MODEL_3D;
            ResetModel();
        }

        private void btYaw_Click(object sender, EventArgs e)
        {
            curModel = DRAW_MODEL.MODEL_YAW;
            ResetModel();
        }

        private void btPitch_Click(object sender, EventArgs e)
        {
            curModel = DRAW_MODEL.MODEL_PITCH;
            ResetModel();
        }

        private void btRoll_Click(object sender, EventArgs e)
        {
            curModel = DRAW_MODEL.MODEL_ROLL;
            ResetModel();
        }

        private void ResetModel()
        {
            ResetProjectMatrix();
        }
    }

    

    
}
