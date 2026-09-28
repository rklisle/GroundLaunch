using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroundLunch
{
    public partial class ModGps : DevExpress.XtraEditors.XtraUserControl
    {
        double leftLon = 73.498;//喀什
        double rightLon = 135.098;//黑龙江
        double topLat = 48.14;//东内蒙古，西侧尖尖
        double bottomLat = 18.177;//三亚
        public ModGps()
        {
            InitializeComponent();
            InitMap();
        }

        private void InitMap()
        {
            
            picLocationLanuch.Parent = svgImage;
            picLocationCur.Parent = picLocationLanuch;
            //  svgImage.SizeMode = DevExpress.XtraEditors.SvgImageSizeMode.Zoom;
            // svgImage.st
        }

        public void RefreshUI()
        {
            MoveLanuchToLocation(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].lanuchLon,
                NetDataHandle.groundFrame[NetDataHandle.curSelMsn].lanuchLat);
            MoveCurToLocation(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsLon,
                NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsLat);
            labelGpsLon.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsLon.ToString("0.######");
            labelGpsLat.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsLat.ToString("0.######");
            labelGpsHigh.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsHigh.ToString("0.##");
           
            //定位模式
            labelGpsState.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsPositioned == 0 ? "未定位" : "已定位";
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsPositioned == 0)
            {
                labelGpsState.ForeColor = Color.Red;
               // labelSatelliteCount.Text = "--";
               // labelVn.Text = "--";
              //  labelVe.Text = "--";
              //  labelVs.Text = "--";
              //  labelPDOP.Text = "--";
               // labelGDOP.Text = "--";
              //  labelGPSDate.Text = "--";
               // return;
            }
            else
            {
                labelGpsState.ForeColor = Color.Lime;
            }


            //星数
            labelSatelliteCount.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsSatelliteCount.ToString();
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsSatelliteCount < 7)
            {
                labelSatelliteCount.ForeColor = Color.Red;
            }
            else if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsSatelliteCount < 14)
            {
                labelSatelliteCount.ForeColor = Color.Yellow;
            }
            else
            {
                labelSatelliteCount.ForeColor = Color.Lime;
            }
            
            //北天东速度
            labelVn.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsVn.ToString("0.##");
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsVn > 0.3)
            {
                labelVn.ForeColor = Color.Yellow;
            }
            else
            {
                labelVn.ForeColor = Color.Lime;
            }


            labelVs.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsVs.ToString("0.##");
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsVs > 0.3)
            {
                labelVs.ForeColor = Color.Yellow;
            }
            else
            {
                labelVs.ForeColor = Color.Lime;
            }

            labelVe.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsVe.ToString("0.##");
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsVe > 0.3)
            {
                labelVe.ForeColor = Color.Yellow;
            }
            else
            {
                labelVe.ForeColor = Color.Lime;
            }



            //PDOP和GDOP
            labelPDOP.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsPDOP.ToString("0.#");
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsPDOP > 5)
            {
                labelPDOP.ForeColor = Color.Red;
            }
            else if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsPDOP > 2.5)
            {
                labelPDOP.ForeColor = Color.Yellow;
            }
            else
            {
                labelPDOP.ForeColor = Color.Lime;
            }
            labelGDOP.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsGDOP.ToString("0.#");
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsGDOP > 5)
            {
                labelGDOP.ForeColor = Color.Red;
            }
            else if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsGDOP > 2.5)
            {
                labelGDOP.ForeColor = Color.Yellow;
            }
            else
            {
                labelGDOP.ForeColor = Color.Lime;
            }

            labelGPSDate.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsDate;
        }

        private void MoveLanuchToLocation(double lon, double lat)
        {
            if(lon > leftLon) 
            {
                picLocationLanuch.Visible = true;
            }

            int x, y;
            int effectWidth = (int)(svgImage.Width * 0.78);
            int effectHigh = (int)(svgImage.Height * 0.69);
            double diffLon = rightLon - leftLon;
            double diffLat = topLat - bottomLat;

            double a = (lon - leftLon) / diffLon; //a 0~1之间的数 
            double b = (lat - bottomLat) / diffLat;
            
            double zeroX = -150;
            double zeroY = 13;

            x = (int)(zeroX + a * effectWidth);
            y = (int)(zeroY - b * effectHigh);

            if (y < -90)
            {
                picLocationLanuch.Properties.Caption.Offset = new Point(10, 150);
            }
            else 
            {
                picLocationLanuch.Properties.Caption.Offset = new Point(10, 98);
            }
            picLocationLanuch.Location = new Point(x, y);
        }

        private void MoveCurToLocation(double lon, double lat)
        {
            if (lon > leftLon)
            {
                picLocationCur.Visible = true;
            }
            //算法一样，但有两个要注意
            //1.坐标是根据发射点设计的，所以要先找发射点的位置
            //2.重新计算0坐标
            int x, y;
            int effectWidth = (int)(svgImage.Width * 0.78);
            int effectHigh = (int)(svgImage.Height * 0.69);
            double diffLon = rightLon - leftLon;
            double diffLat = topLat - bottomLat;

            double a = (lon - leftLon) / diffLon; //a 0~1之间的数 
            double b = (lat - bottomLat) / diffLat;

            double lanuchX = picLocationLanuch.Location.X;
            double lanuchY = picLocationLanuch.Location.Y;

            double zeroX = -10 - lanuchX ;
            double zeroY = 102 - lanuchY;

            x = (int)(zeroX + a * effectWidth);
            y = (int)(zeroY - b * effectHigh);

           // x = 0;
            //y = 0;

            if (y < 40)
            {
                picLocationCur.Properties.Caption.Offset = new Point(10, 62);
            }
            else
            {
                picLocationCur.Properties.Caption.Offset = new Point(10, 10);
            }
             picLocationCur.Location = new Point(x, y);
        }

        private void btGetEphGps_Click(object sender, EventArgs e)
        {
            Byte[] data = new Byte[2];
            data[0] = 0;
            NetDataHandle.Send_To_FK(1,0xE7, data);
        }

        private void btGetEphBD_Click(object sender, EventArgs e)
        {
            Byte[] data = new Byte[2];
            data[0] = 1;
            NetDataHandle.Send_To_FK(1,0xE7, data);
        }
    }
}
