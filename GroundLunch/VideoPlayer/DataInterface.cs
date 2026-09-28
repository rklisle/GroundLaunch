using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GroundLunch
{
    static public class DataInterface
    {
        static public ViewPlaneParams viewPlane = new ViewPlaneParams();
        static public bool startFly = false;
        static public List<System.Drawing.Color> colorTable = new List<System.Drawing.Color>
        {
            System.Drawing.Color.Red,
            System.Drawing.Color.DarkViolet,
            System.Drawing.Color.LightGreen,
            System.Drawing.Color.Yellow,
            System.Drawing.Color.Orange,
            System.Drawing.Color.DarkTurquoise,
            System.Drawing.Color.DeepSkyBlue,
            System.Drawing.Color.HotPink,
        };
        static public int isUpdate;
        static public Dictionary<(int, int), UVE> UVEs = new Dictionary<(int, int), UVE>();
        static public UVE viewedUve = null;
        static public LuanchInfo luanchInfo = new LuanchInfo();
        static public WGS84Pos recyclePoint = new WGS84Pos();
        static public void InitUves()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    UVE uve = new UVE(i + 1, j + 1);
                    UVEs.Add((i + 1, j + 1), uve);
                }
            }
        }
    }

    public class ViewPlaneParams
    {
        public double lon;
        public double lat;
        public double alt;
        public double speed;
        public double pitch;
        public double roll;
        public double heading;
        public double rpm;
        public double scoutPitch;
        public double scoutHeading;
        public double ignationTime;
        public int groupID;
        public int msnID;
        public int leadID;


        public double scoutPitchAim;
        public double scoutHeadingAim;
    }

    public enum UVE_RANK
    {
        MASTER = 0,
        SLAVER = 1,
        UNKNOW = 2,
    }

    public class UVE
    {
        public int uveEnable;
        public int uveGroupID;
        public int uveMsnID;
        public string uveName;
        public CurInfo curInfo = new CurInfo();
        public System.Drawing.Color color;
        public UVE_RANK rank;
        public IPAddress uveIP;
        public UVE(int uvegroupid, int uvemsnid)
        {
            uveMsnID = uvemsnid;
            uveGroupID = uvegroupid;
            uveName = string.Format("{0}-{1}", uveGroupID, uveMsnID);
            color = DataInterface.colorTable[uveGroupID-1];
            rank = UVE_RANK.UNKNOW;
            uveIP = IPAddress.Any;
            uveEnable = 0;
        }
        
    }

    public class LuanchInfo
    {
        public WGS84Pos wGS84Pos = new WGS84Pos();
        public double dir;
        public double pitch;
    }

    public class CurInfo
    {
        public WGS84Pos wGS84Pos = new WGS84Pos();
        public double dir;
        public double pitch;
        public double roll;
        public double TAS;
    
    }

    public class WGS84Pos
    {
        public WGS84Pos(double inLon, double inLat, double inAlt)
        {
            lon = inLon;
            lat = inLat;
            alt = inAlt;
        }

        public WGS84Pos()
        {
            lon = 0; lat=0; alt = 0;
        }

        public double lon;
        public double lat;
        public double alt;
    }


    static public class CommonCalc
    {
        static public double Pt1Pt2Dir(double lon1, double lat1, double lon2, double lat2)
        {
            const double d2r = (57.29577951308402);
            double dir = 0;
            lat1 = lat1 / d2r;
            lon1 = lon1 / d2r;
            lat2 = lat2 / d2r;
            lon2 = lon2 / d2r;

            // 计算经度差
            double deltaLon = lon2 - lon1;

            // 计算方位角
            double x = Math.Sin(deltaLon) * Math.Cos(lat2);
            double y = Math.Cos(lat1) * Math.Sin(lat2) - (Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon));
            double initialBearing = Math.Atan2(x, y);

            // 将方位角从弧度转换为度
            initialBearing = initialBearing * d2r;

            // 使方位角在0到360度之间
            dir = (initialBearing + 360.0) % 360.0;

            return dir;
        }
        // 地球半径（单位：米）


        // 将角度转换为弧度
        static double degrees_to_radians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        // 将弧度转换为角度
        static double radians_to_degrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }
        // 计算两点之间的球面距离（米）
        static public double haversine_distance(double lat1, double lon1, double lat2, double lon2)
        {
            double EARTH_RADIUS = 6371000.0;
            double dlat = degrees_to_radians(lat2 - lat1);
            double dlon = degrees_to_radians(lon2 - lon1);
            double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                       Math.Cos(degrees_to_radians(lat1)) * Math.Cos(degrees_to_radians(lat2)) *
                       Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EARTH_RADIUS * c;
        }

    }
}
