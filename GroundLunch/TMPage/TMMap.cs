using DevExpress.Charts.Native;
using DevExpress.Utils.Extensions;
using DevExpress.XtraCharts;
using Newtonsoft.Json;
using OfficeOpenXml;
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

namespace GroundLunch
{
    public partial class TMMap : DevExpress.XtraEditors.XtraUserControl
    {
        DataTable dt = new DataTable();
        string defaultSimuExcel = ".\\模飞\\SimuPath.xlsx";
        string defaultSimuA0 = ".\\flashFile\\SimuFlight.a0";
        string defaultPtFile = ".\\flashFile\\NavPtJson.dat";
        public List<SimuMapInfo> mapInfos = new List<SimuMapInfo>();
        public List<SimuMapInfo> ptInfos = new List<SimuMapInfo>();
        SimuMapData mapData = new SimuMapData();
        public DataTable SimuMapDataTable = new DataTable();
        List<Series> sePlane = new List<Series>();
        Series seTargetPoint;
       // public DataTable SimuPointDataTable = new DataTable();
        public List<DataTable> RealMapDataTable = new List<DataTable>();

        public TMMap()
        {
            InitializeComponent();
            LoadSimuData();
            InitMapDataTable();
            InitChart();
        }

        public void LoadSimuData()
        {
            if (File.Exists(defaultSimuA0))
            {
                //AnalyzeExcel(defaultSimuExcel);
                AnalyzeA0File(defaultSimuA0);
            }
            if (File.Exists(defaultPtFile))
            {
                AnalyzePtFile(defaultPtFile);
            }
        }

        public void AnalyzePtFile(string path)
        {
            string json = File.ReadAllText(path);
            FlightPointFile person = JsonConvert.DeserializeObject<FlightPointFile>(json);

            foreach (var item in person.mission.items)
            {
                SimuMapInfo ptInfo = new SimuMapInfo();
                ptInfo.time = 0;
                ptInfo.lon = Convert.ToDouble(item.Params[5]);
                ptInfo.lat = Convert.ToDouble(item.Params[4]);
                ptInfo.high = Convert.ToDouble(item.Params[6]);
                ptInfos.Add(ptInfo);
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
                if (itemIndex % 10 != 0)
                {
                    itemIndex++;
                    continue;
                }
                SimuMapInfo mapInfo = new SimuMapInfo();
                mapInfo.time = BitConverter.ToInt32(byteSimu, i) * 0.005;
                mapInfo.lon = BitConverter.ToDouble(byteSimu, i + 28);
                mapInfo.lat = BitConverter.ToDouble(byteSimu, i + 36);
                mapInfo.high = BitConverter.ToDouble(byteSimu, i + 44);
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

        public void AnalyzeExcel(string path)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage ep = new ExcelPackage(path);
            ExcelWorksheets sheets = ep.Workbook.Worksheets;
            foreach (ExcelWorksheet sheet in sheets)
            {
                int i = 2;//行号
                while (sheet.Cells[i, 1].Value != null)
                {
                    if (i % 400 != 0)
                    {
                        i++;
                        continue;
                    }
                    SimuMapInfo mapInfo = new SimuMapInfo();
                    mapInfo.time = Convert.ToDouble(sheet.Cells[i, 1].Value);
                    mapInfo.lon = Convert.ToDouble(sheet.Cells[i, 2].Value);
                    mapInfo.lat = Convert.ToDouble(sheet.Cells[i, 3].Value);
                    mapInfos.Add(mapInfo);
                    if (mapInfo.lon > mapData.maxlon)
                        mapData.maxlon = mapInfo.lon;
                    if (mapInfo.lat > mapData.maxlat)
                        mapData.maxlat = mapInfo.lat;
                    if (mapInfo.lon < mapData.minlon)
                        mapData.minlon = mapInfo.lon;
                    if (mapInfo.lat < mapData.minlat)
                        mapData.minlat = mapInfo.lat;

                    i++;
                }
            }
        }

        public void InitMapDataTable()
        {
            SimuMapDataTable.Columns.Clear();
            SimuMapDataTable.Columns.Add("second", typeof(double));
            SimuMapDataTable.Columns.Add("simulon", typeof(double));
            SimuMapDataTable.Columns.Add("simulat", typeof(double));
            SimuMapDataTable.Columns.Add("simuhigh", typeof(double));

            for(int i=0;i<6;i++) 
            {
                DataTable dt = new DataTable();
                dt.Columns.Clear();
                dt.Columns.Add("second", typeof(double));
                dt.Columns.Add("lon", typeof(double));
                dt.Columns.Add("lat", typeof(double));
                dt.Columns.Add("high", typeof(double));
                RealMapDataTable.Add(dt);
            }
            SimuExcelToDataTable();
        }

        private void InitChart()
        {
            chartMap.Series.Clear();
           
            Series seSimu = new Series("预设飞行轨迹", ViewType.ScatterLine);
            seSimu.DataSource = SimuMapDataTable;
            seSimu.ArgumentDataMember = "simulon";
            seSimu.ValueDataMembers.AddRange("simulat");
            chartMap.Series.Add(seSimu);

            mapData.gaplat = mapData.maxlat - mapData.minlat;
            mapData.gaplon = mapData.maxlon - mapData.minlon;

            double gridLat = mapData.gaplat / this.Height;
            double gridLon = mapData.gaplon / this.Width;

            double rangeXmax, rangeXmin, rangeYmax, rangeYmin;
            if (gridLat > gridLon)
            {
                rangeYmax = mapData.maxlat;
                rangeYmin = mapData.minlat;
                rangeXmax = (mapData.minlon + mapData.maxlon) * 0.5 + gridLat * this.Width * 0.6;
                rangeXmin = (mapData.minlon + mapData.maxlon) * 0.5 - gridLat * this.Width * 0.6;
            }
            else
            {
                rangeYmax = (mapData.minlat + mapData.maxlat) * 0.5 + gridLon * this.Height * 0.6;
                rangeYmin = (mapData.minlat + mapData.maxlat) * 0.5 - gridLon * this.Height * 0.6;
                rangeXmax = mapData.maxlon;
                rangeXmin = mapData.minlon;
            }


            AxisX firstX = ((XYDiagram)chartMap.Diagram).AxisX;
            //firstX.WholeRange.SetMinMaxValues(rangeXmin, rangeXmax);

            AxisY firstY = ((XYDiagram)chartMap.Diagram).AxisY;
           // firstY.WholeRange.SetMinMaxValues(rangeYmin, rangeYmax);

            for (int i = 0; i < 6; i++)
            {
                string seName = string.Format("真实飞行轨迹{0}", i + 1);
                Series se = new Series(seName, ViewType.ScatterLine);
                se.DataSource = RealMapDataTable[i];
                se.ArgumentDataMember = "lon";
                se.ValueDataMembers.AddRange("lat");
                chartMap.Series.Add(se);

                string sePlaneName = string.Format("飞机{0}位置", i + 1);
                Series sePlane1 = new Series(sePlaneName, ViewType.Point);
                (sePlane1.View as PointSeriesView).PointMarkerOptions.Kind = MarkerKind.Circle;
                (sePlane1.View as PointSeriesView).PointMarkerOptions.Size = 10;
                (sePlane1.View as PointSeriesView).Color = (se.View as LineSeriesView).Color;
                sePlane.Add(sePlane1);
                chartMap.Series.Add(sePlane1);
            }


            Series sePoint = new Series("预设航点", ViewType.Point);
            (sePoint.View as PointSeriesView).PointMarkerOptions.Size = 4;
            int j = 0;
            foreach(var item in ptInfos) 
            {
                //if(j>4)
                sePoint.Points.Add(new SeriesPoint(item.lon, item.lat));
                j++;
            }
            chartMap.Series.Add(sePoint);

            seTargetPoint = new Series("下一个航点", ViewType.Point);
            (seTargetPoint.View as PointSeriesView).PointMarkerOptions.Size = 20;
            (seTargetPoint.View as PointSeriesView).PointMarkerOptions.Kind = MarkerKind.Star;
            chartMap.Series.Add(seTargetPoint);

            /*
            string seNameplane = string.Format("飞行{0}", 1);
            Series sePlane = new Series(seNameplane, ViewType.Point);
            sePlane.Points.Add(new SeriesPoint( 109.21, 38.545 ));
            sePlane.Points.Add(new SeriesPoint( 109.215, 38.55  ));
            sePlane.Points.Add(new SeriesPoint( 109.22, 38.545  ));
            sePlane.Points.Add(new SeriesPoint( 109.215, 38.54 ));
            sePlane.Points.Add(new SeriesPoint(109.21, 38.545  ));*/
            //chartMap.Series.Add(sePlane);

            /*
           // Create a range area series.
           Series series1 = new Series("Series 1", ViewType.RangeArea);

           // Add points to them.
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 1), 2.08, 4.28));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 2), 2.42, 4.03));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 3), 2.78, 3.98));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 4), 2.57, 3.94));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 5), 2.69, 4.18));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 6), 2.69, 5.02));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 7), 2.36, 5.60));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 8), 1.97, 5.37));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 9), 2.76, 4.94));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 10), 3.54, 3.66));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 11), 4.31, 1.07));
           series1.Points.Add(new SeriesPoint(new DateTime(2008, 1, 12), 4.08, 0.09));

           // Add a series to the chart.
           chartMap.Series.Add(series1);

           // Access the view-type-specific options of the series.
           ((RangeAreaSeriesView)series1.View).Transparency = 80;

           // Access the type-specific options of the diagram.
           ((XYDiagram)chartMap.Diagram).AxisX.GridLines.Visible = true;
           ((XYDiagram)chartMap.Diagram).AxisY.WholeRange.MinValue = -0.5;
           ((XYDiagram)chartMap.Diagram).AxisY.WholeRange.MaxValue = 6;

           // Hide the legend (if necessary).
           chartMap.Legend.Visible = false;

           // Add a title to the chart (if necessary).
           chartMap.Titles.Add(new ChartTitle());
           chartMap.Titles[0].Text = "A Range Area Chart"; */
        }


        public void InsertRealPoint(int index, double time, double lon, double lat, double high)
        {
            DataTable dt = RealMapDataTable[index];
            DataRow dr = dt.NewRow();
            List<object> objlist = new List<object>();

            objlist.Add(time);
            objlist.Add(lon);
            objlist.Add(lat);
            objlist.Add(high);
            object[] rowArray = objlist.ToArray();
            dr.ItemArray = rowArray;
            dt.Rows.Add(dr);

            sePlane[index].Points.Clear();
            sePlane[index].Points.Add(new SeriesPoint(lon, lat));
        }

        public void InsertTargetPoint(double lon, double lat, double high) 
        {
            seTargetPoint.Points.Clear();
            seTargetPoint.Points.Add(new SeriesPoint(lon, lat));
        }

        private void SimuExcelToDataTable()
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
}
