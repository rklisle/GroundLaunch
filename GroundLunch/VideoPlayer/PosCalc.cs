using DevExpress.XtraPrinting.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuControl280
{
    internal class PosCalc
    {
        const double ae = (6378137.0);
        const double e2 = (6.69437999014e-3);
        const double PI = (3.141592653589793);
        const double DTR = (PI / 180.0);

        public static void DoCalc(double startLon, double startLat, double startHigh, double startPos, double targetLon, double targetLat, double targetHigh, double vn, double vs, double ve, double delaySet, ref double pitch, ref double yaw, ref double course)
        {
            double x = 0, y = 0, z = 0;
            double vx = 0, vy = 0, vz = 0;
            DoCalcXYZ(startLon, startLat, startHigh, startPos, targetLon, targetLat, targetHigh, vn, vs, ve, ref x, ref y, ref z, ref vx, ref vy, ref vz);
            double delaySecond = delaySet / 1000;
            x = x + vx * delaySecond;
            y = y + vy * delaySecond;
            z = z + vz * delaySecond;
            //开始计算角度
            double AngleRoll = Math.Atan2(z, x) / DTR;
            double AnglePitch = Math.Atan2(y, Math.Sqrt(x * x + z * z)) / DTR;

            yaw = AngleRoll;
            pitch = AnglePitch;
            
            course = (AngleRoll + startPos) % 360;
            if (course < 0)
            {
                course = 360 + course;
            }
        }

        static public void DoCalcXYZ(double startLon, double startLat, double startHigh, double startPos, double targetLon, double targetLat, double targetHigh, double vn, double vs, double ve, ref double x, ref double y, ref double z, ref double vx, ref double vy, ref double vz)
        {
            startLon *= DTR; startLat *= DTR; startPos *= DTR; 
            targetLon *= DTR; targetLat *= DTR; 

            double[] stateWGS84 = new double[6];
            stateWGS84[0] = targetLon;
            stateWGS84[1] = targetLat;
            stateWGS84[2] = targetHigh;
            stateWGS84[3] = vn;
            stateWGS84[4] = vs;
            stateWGS84[5] = ve;

            double[] state = new double[6];
            ConSys_EarthWGS84_To_Launch(stateWGS84, state, startLon, startLat, startHigh, startPos);

            x = state[0];
            y = state[1];
            z = state[2];
            vx = state[3];
            vy = state[4];
            vz = state[5];

        }

     

        static void ConSys_EarthWGS84_To_Launch(double[] stateWGS84, double[] state, double startLon, double startLat, double startHigh, double startPos)
        {

            //---------------------WGS-84系转发射系计算函数--------------------------
            // 输入：
            // stateLBH   地固系(WGS-84)状态 0-2经纬高 3-5北天东速度
            //
            // 输出：
            // state      发射系状态 0-2位置 3-5速度
            //
            // 调用格式
            // ConSys_EarthWGS84_To_Launch(stateWGS84, state);
            //-----------------------------------------------------------------------

            double Lon, Be, H;
            double[] Vn = new double[3];
            Lon = stateWGS84[0];
            Be = stateWGS84[1];
            H = stateWGS84[2];
            Vn[0] = stateWGS84[3];
            Vn[1] = stateWGS84[4];
            Vn[2] = stateWGS84[5];


            double RN = ae / Math.Sqrt(1 - e2 * Math.Sin(Be) * Math.Sin(Be));

            // 地固系位置坐标
            double[] R_Eg = new double[3];
            R_Eg[0] = (RN + H) * Math.Cos(Be) * Math.Cos(Lon);
            R_Eg[1] = (RN + H) * Math.Cos(Be) * Math.Sin(Lon);
            R_Eg[2] = (RN * (1 - e2) + H) * Math.Sin(Be);

            double[,] Ge = new double[3, 3];
            double[,] Gex = new double[3, 3];
            double[,] Gey = new double[3, 3];
            double[,] Gez = new double[3, 3];
            double[,] Mtemp = new double[3, 3];

            ConSys_cy(-(PI / 2 + startPos), ref Gey);
            ConSys_cx(startLat, ref Gex);
            ConSys_cz(-(PI / 2 - startLon), ref Gez);

            ConSys_Matrix3x3(Gey, Gex, ref Mtemp);
            ConSys_Matrix3x3(Mtemp, Gez, ref Ge);

            // 发射系状态 但原点在地心
            double[] statefr = new double[3];
            ConSys_Matrix3x1(Ge, R_Eg, ref statefr);

            // 北天东坐标系下速度计算
            double[,] MB0 = new double[3, 3];
            double[,] MA0 = new double[3, 3];
            double[,] M_EL = new double[3, 3];

            ConSys_cz(startLat, ref MB0);
            ConSys_cy(startPos, ref MA0);
            ConSys_Matrix3x3(MB0, MA0, ref M_EL);

            double delta_L = Lon - startLon;

            double[,] MBE = new double[3, 3];
            double[,] MdL = new double[3, 3];
            ConSys_cz(-Be, ref MBE);
            ConSys_cx(delta_L, ref MdL);

            double[,] M_NE = new double[3, 3];
            double[,] M_NL = new double[3, 3];
            ConSys_Matrix3x3(MBE, MdL, ref M_NE);
            ConSys_Matrix3x3(M_NE, M_EL, ref M_NL);

            double[,] M_LN = new double[3, 3];
            ConSys_M3x3_transpose(M_NL, M_LN);   // 正交矩阵逆和转置相同

            double[] V_vector = new double[3];
            ConSys_Matrix3x1(M_LN, Vn, ref V_vector);


            double ConSys_Rox = 0, ConSys_Roy = 0, ConSys_Roz = 0;
            CalcRoxyz(startLon, startLat, startHigh, Ge, ref ConSys_Rox, ref ConSys_Roy, ref ConSys_Roz);

            state[0] = statefr[0] - ConSys_Rox;
            state[1] = statefr[1] - ConSys_Roy;
            state[2] = statefr[2] - ConSys_Roz;
            state[3] = V_vector[0];
            state[4] = V_vector[1];
            state[5] = V_vector[2];

        }

        static void CalcRoxyz(double lon, double lat, double high, double[,] Ge, ref double rox, ref double roy, ref double roz)
        {
            double[] stateLBH0 = new double[3];
            double[] stateR0 = new double[3];
            stateLBH0[0] = lon;
            stateLBH0[1] = lat;
            stateLBH0[2] = high;
            ConSys_EarthLBH_To_EarthFixed(stateLBH0, ref stateR0);
            double[] ConSys_R0Fa = new double[3];
            ConSys_Matrix3x1(Ge, stateR0, ref ConSys_R0Fa);
            rox = ConSys_R0Fa[0];
            roy = ConSys_R0Fa[1];
            roz = ConSys_R0Fa[2];
        }

        static void ConSys_M3x3_transpose(double[,] M_in, double[,] M_out)
        {

            //------------三维矩阵转置函数------------
            // 3x3矩阵转置函数
            //
            // 输入：
            // M_in
            //
            // 输出：
            // M_out
            //
            // 调用格式:
            // ConSys_M3x3_transpose(M_in, M_out);
            //----------------------------------------

            int i, j;
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    M_out[i, j] = M_in[j, i];
                }
            }

        }

        static void ConSys_EarthLBH_To_EarthFixed(double[] stateLBH, ref double[] stateEg)
        {

            //---------------------经纬高计算地固系状态函数--------------------------
            // 输入：
            // stateLBH  经纬高 0-2经纬高
            //
            // 输出：
            // stateEg   地固系(WGS-84)状态 0-2位置
            //
            // 调用格式
            // ConSys_EarthLBH_To_EarthFixed(stateLBH, stateEg);
            //-----------------------------------------------------------------------

            double Lon, Be, H;

            Lon = stateLBH[0];
            Be = stateLBH[1];
            H = stateLBH[2];

            double RN = ae / Math.Sqrt(1 - e2 * Math.Sin(Be) * Math.Sin(Be));

            double x, y, z;
            x = (RN + H) * Math.Cos(Be) * Math.Cos(Lon);
            y = (RN + H) * Math.Cos(Be) * Math.Sin(Lon);
            z = (RN * (1 - e2) + H) * Math.Sin(Be);

            stateEg[0] = x;
            stateEg[1] = y;
            stateEg[2] = z;

        }

        static void ConSys_cx(double th, ref double[,] M)
        {

            //--------------X轴旋转函数---------------
            // 三维坐标绕X轴旋转 生成旋转矩阵
            //
            // 输入：
            // th   rad
            //
            // 输出：
            // M    旋转矩阵
            //
            // 调用格式:
            // ConSys_cx(th, M);
            //----------------------------------------

            M[0, 0] = 1;
            M[0, 1] = 0;
            M[0, 2] = 0;
            M[1, 0] = 0;
            M[1, 1] = Math.Cos(th);
            M[1, 2] = Math.Sin(th);
            M[2, 0] = 0;
            M[2, 1] = -Math.Sin(th);
            M[2, 2] = Math.Cos(th);

        }

        static void ConSys_cy(double th, ref double[,] M)
        {

            //--------------Y轴旋转函数---------------
            // 三维坐标绕Y轴旋转 生成旋转矩阵
            //
            // 输入：
            // th   rad
            //
            // 输出：
            // M    旋转矩阵
            //
            // 调用格式:
            // ConSys_cy(th, M);
            //----------------------------------------

            M[0, 0] = Math.Cos(th);
            M[0, 1] = 0;
            M[0, 2] = -Math.Sin(th);
            M[1, 0] = 0;
            M[1, 1] = 1;
            M[1, 2] = 0;
            M[2, 0] = Math.Sin(th);
            M[2, 1] = 0;
            M[2, 2] = Math.Cos(th);

        }

        static void ConSys_cz(double th, ref double[,] M)
        {

            //--------------Z轴旋转函数---------------
            // 三维坐标绕Z轴旋转 生成旋转矩阵
            //
            // 输入：
            // th   rad
            //
            // 输出：
            // M    旋转矩阵
            //
            // 调用格式:
            // ConSys_cz(th, M);
            //----------------------------------------

            M[0, 0] = Math.Cos(th);
            M[0, 1] = Math.Sin(th);
            M[0, 2] = 0;
            M[1, 0] = -Math.Sin(th);
            M[1, 1] = Math.Cos(th);
            M[1, 2] = 0;
            M[2, 0] = 0;
            M[2, 1] = 0;
            M[2, 2] = 1;

        }

        static void ConSys_Matrix3x3(double[,] Matrix1, double[,] Matrix2, ref double[,] Matrixout)
        {

            //------------矩阵乘法运算函数------------
            // 矩阵相乘运算 Matrix1*Matrix2 输出矩阵 Matrixout 为3行3列
            //
            // 输入：
            // Matrix1、Matrix2
            //
            // 输出：
            // Matrixout
            //
            // 调用格式:
            // ConSys_Matrix3x1(Matrix1, Matrix2, Matrixout);
            //----------------------------------------

            double sum = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        sum = sum + Matrix1[i, j] * Matrix2[j, k];
                    }
                    Matrixout[i, k] = sum;
                    sum = 0;
                }
            }

        }

        static void ConSys_Matrix3x1(double[,] Matrix1, double[] Matrix2, ref double[] Matrixout)
        {
            //------------矩阵乘法运算函数------------
            // 矩阵相乘运算 Matrix1*Matrix2 输出矩阵 Matrixout 为3行1列
            //
            // 输入：
            // Matrix1、Matrix2
            //
            // 输出：
            // Matrixout
            //----------------------------------------
            double sum = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sum = sum + Matrix1[i, j] * Matrix2[j];
                }
                Matrixout[i] = sum;
                sum = 0;
            }

        }
    }


}
