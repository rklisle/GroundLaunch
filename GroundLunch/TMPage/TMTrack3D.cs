using DevExpress.Skins;
using DevExpress.XtraGauges.Core.Base;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GroundLunch.Line3D;
using static GroundLunch.TMMap;

namespace GroundLunch
{
    public partial class TMTrack3D : DevExpress.XtraEditors.XtraUserControl
    {
        public List<SimuMapInfo> mapInfos = new List<SimuMapInfo>();
        SimuMapData mapData = new SimuMapData();
        public DataTable SimuMapDataTable = new DataTable();
        public DataTable RealMapDataTable = new DataTable();
        string defaultSimuA0 = ".\\flashFile\\SimuFlight.a0";

        public eNormalize eNormalize = eNormalize.Separate;

        public TMTrack3D()
        {
            InitializeComponent();
           

        }

        public void InitMapDataTable()
        {
            SimuMapDataTable.Columns.Clear();
            SimuMapDataTable.Columns.Add("second", typeof(double));
            SimuMapDataTable.Columns.Add("simulon", typeof(double));
            SimuMapDataTable.Columns.Add("simulat", typeof(double));
            SimuMapDataTable.Columns.Add("simuhigh", typeof(double));

            RealMapDataTable.Columns.Clear();
            RealMapDataTable.Columns.Add("second", typeof(double));
            RealMapDataTable.Columns.Add("lon", typeof(double));
            RealMapDataTable.Columns.Add("lat", typeof(double));
            RealMapDataTable.Columns.Add("high", typeof(double));
            SimuFileToDataTable();
        }

        private void SimuFileToDataTable()
        {
            DataTable dt = SimuMapDataTable;
            foreach (var item in mapInfos)
            {
                DataRow dr = dt.NewRow();
                List<object> objlist = new List<object>();

                objlist.Add(item.time);
                objlist.Add(item.lon);
                objlist.Add(item.lat);
                objlist.Add(item.high);
                object[] rowArray = objlist.ToArray();
                dr.ItemArray = rowArray;
                dt.Rows.Add(dr);
            }
        }

        public void LoadSimuData()
        {
            if (File.Exists(defaultSimuA0))
            {
                //AnalyzeExcel(defaultSimuExcel);
                AnalyzeA0File(defaultSimuA0);
                InitMapDataTable();
                SetSimuScatterPlot();
            }
        }

        public void AnalyzeA0File(string path)
        {
            string filePath = path;
            if (!File.Exists(filePath))
            {
                return;
            }
            byte[] byteSimu = File.ReadAllBytes(filePath);

            int itemIndex = 0;
            for (int i = 4; i < byteSimu.Length; i += 84)
            {
                if (itemIndex % 50 != 0)
                {
                    itemIndex++;
                    continue;
                }
                SimuMapInfo mapInfo = new SimuMapInfo();
                mapInfo.time = BitConverter.ToInt32(byteSimu, i) * 0.005;
                mapInfo.lon = BitConverter.ToDouble(byteSimu, i + 28);
                mapInfo.lat = BitConverter.ToDouble(byteSimu, i + 36);
                mapInfo.high = (double)BitConverter.ToSingle(byteSimu, i + 44);
                mapInfos.Add(mapInfo);
                if (mapInfo.lon > mapData.maxlon)
                    mapData.maxlon = mapInfo.lon;
                if (mapInfo.lat > mapData.maxlat)
                    mapData.maxlat = mapInfo.lat;
                if (mapInfo.lon < mapData.minlon)
                    mapData.minlon = mapInfo.lon;
                if (mapInfo.lat < mapData.minlat)
                    mapData.minlat = mapInfo.lat;
                itemIndex++;
            }
        }

        public void InsertRealPoint(double time, double lon, double lat, double high)
        {
            graph3D.InsertNewRealPoint(new cScatter(lon, lat, high, null));

            /*
            DataTable dt = RealMapDataTable;
            DataRow dr = dt.NewRow();
            List<object> objlist = new List<object>();

            objlist.Add(time);
            objlist.Add(lon);
            objlist.Add(lat);
            objlist.Add(high);
            object[] rowArray = objlist.ToArray();
            dr.ItemArray = rowArray;
            dt.Rows.Add(dr);
            graph3D.Invalidate();




            */
        }

        public void ClearLine()
        {
            graph3D.ClearRealLine();
        }

        private void TMTrack3D_Load(object sender, EventArgs e)
        {
            
            Color[] c_Colors = ColorSchema.GetSchema((ColorSchema.eSchema)11);
            Color[] c_Colors2 = ColorSchema.GetSchema((ColorSchema.eSchema)14);
            graph3D.SetColorScheme(c_Colors, 2, 1);
            graph3D.SetColorScheme(c_Colors2, 6, 2);

            graph3D.Raster = eRaster.Labels;
            
            //SetScatterPlot(true);
        }

        private void SetSimuScatterPlot()
        {
            List<cScatter> i_List = new List<cScatter>();
            foreach (DataRow row in SimuMapDataTable.Rows)
            {
                double lon = (double)row["simulon"];
                double lat = (double)row["simulat"];
                double high = (double)row["simuhigh"];
                i_List.Add(new cScatter(lon, lat, high, null));
            }
            graph3D.SetScatterLines(i_List.ToArray(), eNormalize, 3);
        }
    }
    public class SimuMapInfo
    {
        public double time;
        public double lon;
        public double lat;
        public double high;
    }

    public class RealMapInfo
    {
        public double time;
        public double lon;
        public double lat;
        public double high;
    }

    public class SimuMapData
    {
        public double maxlon = 0;
        public double maxlat = 0;
        public double minlon = 360;
        public double minlat = 90;
        public double gaplon = 0;
        public double gaplat = 0;
    }
}
