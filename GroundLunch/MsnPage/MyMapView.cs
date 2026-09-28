using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraScheduler;
using Esri.ArcGISRuntime.ArcGISServices;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Mapping.Labeling;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Offline;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using SharpGL.SceneGraph;

//using Microsoft.UI.Xaml;
namespace GroundLunch
{
    public class MyMapView
    {
        public MissionMap mainWindow;
        private List<Graphic> _polylineGraphics = new List<Graphic>();
        private List<PolylineBuilder> _polylineBuilder = new List<PolylineBuilder>();
        private List<Graphic> _planeGraphics = new List<Graphic>();
        private List<SimpleMarkerSymbol> _trianglePlaneSymbol = new List<SimpleMarkerSymbol>();
        private Dictionary<(int, int), int> _planeGraphicIndex = new Dictionary<(int, int), int>();
        private Dictionary<(int, int), int> _polylineIndex = new Dictionary<(int, int), int>();

        private GraphicsOverlay missionOverlay = new GraphicsOverlay();

        private List<WaypointInfo> wayPoints = new List<WaypointInfo>();
        private List<HoverInfo> hoverPoints = new List<HoverInfo>();
        private TargetInfo targetPoint = new TargetInfo();
        private RecycleInfo recyclePoint = new RecycleInfo();
        public SelectedSquare selectedSquare = new SelectedSquare();

        // 新增：统一的航点管理器
        public List<IWaypointInfo> allWaypoints = new List<IWaypointInfo>();
        private Dictionary<Graphic, IWaypointInfo> graphicToWaypoint = new Dictionary<Graphic, IWaypointInfo>();

        private GraphicsOverlay SafeAreaOverlay = null;

        public MSN_MODE msnMode = MSN_MODE.MSN_NULL;
        int ii = 0;

        public MissionFile missionFile = null;
        public MapPoint curSelectedWayPoint = null;
        
        // 新增：存储选中的航点索引和类型
        public int selectedWaypointIndex = -1;
        public bool selectedWaypointIsTemp = false;
        
        // 新增：统一的航点管理方法
        public void AddWaypoint(IWaypointInfo waypoint)
        {
            allWaypoints.Add(waypoint);
            graphicToWaypoint[waypoint.Graphic] = waypoint;
        }

        public void ClearAllWaypoints()
        {
            allWaypoints.Clear();
            graphicToWaypoint.Clear();
        }

        public IWaypointInfo FindWaypointByGraphic(Graphic selectedGraphic)
        {
            if (graphicToWaypoint.TryGetValue(selectedGraphic, out var waypoint))
            {
                return waypoint;
            }
            return null;
        }

        public int GetWaypointIndex(IWaypointInfo waypoint)
        {
            return allWaypoints.IndexOf(waypoint);
        }

        // 新增：通过图形找到对应的航点索引（支持所有航点类型）
        private void FindWaypointIndexByGraphic(Graphic selectedGraphic)
        {
            selectedWaypointIndex = -1;
            selectedWaypointIsTemp = false;
            
            // 获取选中图形的坐标
            var selectedPoint = selectedGraphic.Geometry as MapPoint;
            if (selectedPoint == null)
            {
                System.Diagnostics.Debug.WriteLine("选中图形不是MapPoint类型");
                return;
            }
            
            System.Diagnostics.Debug.WriteLine($"查找航点索引: 选中坐标({selectedPoint.X:F8}, {selectedPoint.Y:F8})");
            
            // 1. 首先在统一航点列表中查找（所有类型）
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                if (allWaypoints[i].Graphic == selectedGraphic)
                {
                    selectedWaypointIndex = i;
                    selectedWaypointIsTemp = false;
                    System.Diagnostics.Debug.WriteLine($"通过图形对象找到航点索引: {i}, 类型: {allWaypoints[i].WpType}");
                    return;
                }
            }
            
            // 2. 如果图形对象匹配失败，通过坐标匹配
            for (int i = 0; i < allWaypoints.Count; i++)
            {
                var wpPoint = allWaypoints[i].Graphic.Geometry as MapPoint;
                if (wpPoint != null &&
                    Math.Abs(wpPoint.X - selectedPoint.X) < 0.0001 &&
                    Math.Abs(wpPoint.Y - selectedPoint.Y) < 0.0001)
                {
                    selectedWaypointIndex = i;
                    selectedWaypointIsTemp = false;
                    System.Diagnostics.Debug.WriteLine($"通过坐标匹配找到航点索引: {i}, 类型: {allWaypoints[i].WpType}, 坐标({wpPoint.X:F8}, {wpPoint.Y:F8})");
                    return;
                }
            }
            
            // 3. 在临时航点中查找
            if (mainWindow != null && mainWindow.tempWayPoints != null)
            {
                for (int i = 0; i < mainWindow.tempWayPoints.Count; i++)
                {
                    var tempWp = mainWindow.tempWayPoints[i];
                    var tempPoint = new MapPoint(tempWp.lon, tempWp.lat, SpatialReferences.Wgs84);
                    
                    if (Math.Abs(tempPoint.X - selectedPoint.X) < 0.0001 &&
                        Math.Abs(tempPoint.Y - selectedPoint.Y) < 0.0001)
                    {
                        selectedWaypointIndex = i;
                        selectedWaypointIsTemp = true;
                        System.Diagnostics.Debug.WriteLine($"找到临时航点索引: {i}, 类型: {tempWp.wpType}, 坐标({tempPoint.X:F8}, {tempPoint.Y:F8})");
                        return;
                    }
                }
            }
            
            System.Diagnostics.Debug.WriteLine("未找到匹配的航点索引");
        }

        public MapView Control { get; set; }

        public MyMapView(MapView view)
        {
            Control = view;
            InitializeMapView();
            

            Control.MouseMove += mapView_MouseMove;
            Control.GeoViewTapped += mapView_Tapped;
            
            Control.ViewpointChanged += Control_ViewpointChanged;

            Control.PreviewMouseLeftButtonDown += mapView_mouseDown;
            Control.PreviewMouseMove += mapView_MouseMove1;
            // Control.
        }

        private async Task UpdateMapData()
        {
            var currentViewpoint = Control.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
            var centerViewpoint = Control.GetCurrentViewpoint(ViewpointType.CenterAndScale);

            if (currentViewpoint == null)
                return;
            var extent = currentViewpoint.TargetGeometry as Envelope;

            // 判断是否需要加载新地图数据
            bool a = await ShouldLoadNewData(extent);

            if (a)
            {
                await DownloadOfflineMapAsync(extent);
            }
            else
            {
                string tileCachePath = @".\flashFile\OfflineMap.tpkx";
                await LoadOfflineMapAsync(tileCachePath);
            }
        }

        private async void Control_ViewpointChanged(object sender, EventArgs e)
        {
           // await UpdateMapData();
        }
        // 判断是否需要加载新数据
        private async Task<bool> ShouldLoadNewData(Envelope extent)
        {
            // 根据当前范围与缓存中的范围对比，决定是否需要下载
            // 示例逻辑：如果当前范围超过缓存范围，返回 true
            return  !await IsExtentCached(extent);
        }

        //检查extent1是否完全在extent2范围内
        private bool ExtentInExtent(Envelope extent1, Envelope extent2)
        {
            if (extent1.XMin < extent2.XMin ||
                extent1.XMax > extent2.XMax ||
                extent1.YMin < extent2.YMin ||
                extent1.YMax > extent2.YMax)
                return false;
            else
                return true;
        }
        // 检查范围是否已经缓存
        private async Task<bool> IsExtentCached(Envelope extent)
        {
            // 指定的瓦片缓存路径
            string tileCachePath = @".\flashFile\OfflineMap.tpkx";

            if (!File.Exists(tileCachePath)) 
            {
                return false;
            }

            // 创建 TileCache 对象
            var tileCache = new TileCache(tileCachePath);

            // 获取瓦片图层
            var tiledLayer = new ArcGISTiledLayer(tileCache);

            await tiledLayer.LoadAsync();

            if (tiledLayer.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
            {
                Envelope tileCacheExtent = tiledLayer.FullExtent;

                // 判断指定的 extent 是否与瓦片缓存的范围重叠
                
                if (ExtentInExtent(extent, tileCacheExtent))
                {
                    // 如果有重叠，说明指定的区域在缓存中
                    return true;
                }
                else
                {
                    // 如果没有重叠，说明指定的区域不在缓存中
                    return false;
                }
            }
            return false;  
        }

        private async Task DownloadOfflineMapAsync(Envelope areaOfInterest)
        {
            // 定义导出任务的服务 URL
            string serviceUrl = "https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer";
            string apiKey = "AAPTxy8BH1VEsoebNVZXo8HurAgEx518B3Ilj8Qxe1-0-OboEQvvWbL-MmN_ywlKFfU7pzStixqAEObiVrjyvqbQLZ4glJe5ysemFN47aWyC2oJ2OEV4svxj1kZCLqQEaqIk_926d0pmbo-hmbt-HO_1n-PJi-QFD61IJH0_c--SqPPvQtRVORou74PXJrsTH2qJOJ9vRbApoSYItAyXwyb5Xvlgv2EKdqDbGDZ1TM-giJc.AT1_vLBRSxgx";
            var exportTask = await ExportTileCacheTask.CreateAsync(new Uri(serviceUrl));

             // 配置导出参数
            var exportParams = await exportTask.CreateDefaultExportTileCacheParametersAsync(areaOfInterest, 400000,300000);

            // 设置导出路径
            string _tileCachePath = @".\\flashFile\\OfflineMap.tpkx";
           // string _tileCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OfflineMap.tpkx");

            // 创建导出操作
            var exportJob = exportTask.ExportTileCache(exportParams, _tileCachePath);

            // 启动导出任务
            exportJob.Start();

            // 等待任务完成
            await exportJob.GetResultAsync();

            // 检查任务状态
            if (exportJob.Status == Esri.ArcGISRuntime.Tasks.JobStatus.Succeeded)
            {
                MessageBox.Show($"离线地图已保存到: {_tileCachePath}");
            }
            else
            {
                throw new Exception("离线地图导出失败！");
            }

            // 将离线地图加载到 MapView
            await LoadOfflineMapAsync(_tileCachePath);
        }

        private async Task LoadOfflineMapAsync(string tileCachePath)
        {
            if (!File.Exists(tileCachePath))
            {
                throw new FileNotFoundException("离线地图文件未找到！");
            }

            // 创建 TileCache 对象
            var tileCache = new TileCache(tileCachePath);

            // 创建离线图层
            var tiledLayer = new ArcGISTiledLayer(tileCache);

            await tiledLayer.LoadAsync();
            // 创建新地图
            var offlineMap = new Map
            {
                Basemap = new Basemap(tiledLayer)
            };

            // 将离线地图设置为当前地图
            Control.Map = offlineMap;

            await Task.CompletedTask;
        }

        public void CreateWayPointSetting()
        {
            WaypointInfo wp = new WaypointInfo();
            wayPoints.Add(wp);
            mainWindow.SetTextValues("0", "0", "0", "0", "0");
            //航点添加到图层
            missionOverlay.Graphics.Add(wp.wayPointGraphic);
            missionOverlay.LabelDefinitions.Add(wp.labelDefinition);

            missionOverlay.LabelsEnabled = true;
        }

        // 新增：更新预览航点位置
        public void UpdatePreviewWaypointPosition(double lon, double lat)
        {
            try
            {
                if ((msnMode == MSN_MODE.MSN_WAYPOINT || msnMode == MSN_MODE.MSN_WAYPOINT_SELECTED || msnMode == MSN_MODE.MSN_WAYPOINT_DIRCONFIRMED) && wayPoints.Count > 0)
                {
                    // 更新航点位置
                    var newPoint = new MapPoint(lon, lat, SpatialReferences.Wgs84);
                    wayPoints.Last().wayPointGraphic.Geometry = newPoint;
                    wayPoints.Last().UpdateLabel();
                    
                    // 强制刷新地图显示
                    if (Control != null)
                    {
                        Control.InvalidateVisual();
                    }
                }
                else if (msnMode == MSN_MODE.MSN_ATTACK || msnMode == MSN_MODE.MSN_ATTACK_SELECTED)
                {
                    // 更新攻击目标位置
                    var newPoint = new MapPoint(lon, lat, SpatialReferences.Wgs84);
                    targetPoint.targetGraphic.Geometry = newPoint;
                    
                    // 强制刷新地图显示
                    if (Control != null)
                    {
                        Control.InvalidateVisual();
                    }
                }
                else if ((msnMode == MSN_MODE.MSN_HOVER || msnMode == MSN_MODE.MSN_HOVER_SELECTED || msnMode == MSN_MODE.MSN_HOVER_RADIUS_CONFIRMED) && hoverPoints.Count > 0)
                {
                    // 更新盘旋点位置
                    var newPoint = new MapPoint(lon, lat, SpatialReferences.Wgs84);
                    hoverPoints.Last().hoverPtGraphic.Geometry = newPoint;
                    
                    // 强制刷新地图显示
                    if (Control != null)
                    {
                        Control.InvalidateVisual();
                    }
                }
                else if (msnMode == MSN_MODE.MSN_RECYCLE || msnMode == MSN_MODE.MSN_RECYCLE_SELECTED)
                {
                    // 更新伞降点位置
                    var newPoint = new MapPoint(lon, lat, SpatialReferences.Wgs84);
                    recyclePoint.recycleGraphic.Geometry = newPoint;
                    
                    // 强制刷新地图显示
                    if (Control != null)
                    {
                        Control.InvalidateVisual();
                    }
                }
            }
            catch (Exception ex)
            {
                // 静默处理错误
            }
        }
        public void DestoryWayPointSetting(int index)
        {
            if (index == -1)
            {
                missionOverlay.Graphics.Remove(wayPoints.Last().wayPointGraphic);
                wayPoints.Remove(wayPoints.Last());
            }
            else
            {
                missionOverlay.Graphics.Remove(wayPoints[index].wayPointGraphic);
                wayPoints.RemoveAt(index);
            }
        }

        public void CreateTargetSetting()
        {
            mainWindow.SetTextValues("0", "0", "0", "0", "0");
            // 目标添加到图层
            // 检查是否已经存在，避免重复添加
            DestoryTargetSetting();
            missionOverlay.Graphics.Add(targetPoint.targetGraphic);       
        }
        public void CreateRecycleSetting()
        {
            mainWindow.SetTextValues("0", "0", "0", "0", "0");
            // 目标添加到图层
            // 检查是否已经存在，避免重复添加
            DestoryTargetSetting();
            missionOverlay.Graphics.Add(recyclePoint.recycleGraphic);
        }

        public void DestoryTargetSetting()
        {
            // 确保完全移除所有重复的 Graphic
            while (missionOverlay.Graphics.Contains(targetPoint.targetGraphic))
            {
                missionOverlay.Graphics.Remove(targetPoint.targetGraphic);
            }
        }
        public void DestoryRecycleSetting()
        {
            missionOverlay.Graphics.Remove(recyclePoint.recycleGraphic);
        }

        public void CreateHoverSetting()
        {
            mainWindow.SetTextValues("0", "0", "0", "0", "0");
            HoverInfo hoverInfo = new HoverInfo();

            // 将点添加到图层
            missionOverlay.Graphics.Add(hoverInfo.hoverPtGraphic);
            missionOverlay.Graphics.Add(hoverInfo.hoverRaduisGraphic);
            hoverPoints.Add(hoverInfo);
        }

        public void DestoryHoverSetting(int index)
        {
            if (index == -1)
            {
                missionOverlay.Graphics.Remove(hoverPoints.Last().hoverPtGraphic);
                missionOverlay.Graphics.Remove(hoverPoints.Last().hoverRaduisGraphic);
                hoverPoints.Remove(hoverPoints.Last());
            }
            else
            {
                missionOverlay.Graphics.Remove(hoverPoints[index].hoverPtGraphic);
                missionOverlay.Graphics.Remove(hoverPoints[index].hoverRaduisGraphic);
                hoverPoints.RemoveAt(index);
            }
        }

        private void mapView_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            
            // 如果选中了已有航点（文件航点或临时航点），禁用鼠标移动事件
            if (selectedWaypointIndex >= 0)
            {
                return; // 直接返回，不处理鼠标移动事件
            }
            
           // var envelope = mapView.GetCurrentViewpoint(Esri.ArcGISRuntime.Mapping.ViewpointType.BoundingGeometry).TargetGeometry as Esri.ArcGISRuntime.Geometry.Envelope; ;
           // var wgs84Envelope = GeometryEngine.Project(envelope, SpatialReferences.Wgs84);

            // 获取鼠标的屏幕坐标
            var screenPoint = e.GetPosition(Control);

            // 将屏幕坐标转换为地图上的坐标
            var mapPoint = Control.ScreenToLocation(screenPoint);
            if (mapPoint != null)
            {
                // 如果地图使用的是 Web Mercator 坐标系，转换为经纬度坐标系
                var wgs84Point = GeometryEngine.Project(mapPoint, SpatialReferences.Wgs84);

                var wgs84 = wgs84Point as MapPoint;
                if (wgs84 != null)
                {
                    // 获取经纬度
                    double longitude = wgs84.X;
                    double latitude = wgs84.Y;

                    //当模式是选取航点时，显示经纬度，当航点已经选取到之后，显示读数
                    if (msnMode == MSN_MODE.MSN_WAYPOINT || msnMode == MSN_MODE.MSN_NULL)
                    {
                        // 显示经纬度，或者执行其他操作
                        mainWindow.SetLonLat(longitude.ToString("F6"), latitude.ToString("F6"));
                    }
                    if (msnMode == MSN_MODE.MSN_WAYPOINT)
                    {//航点选取模式时，额外绘制一个三角形,这个三角形已经绘制好，改变它的位置即可
                        var newPoint = new MapPoint(longitude,
                            latitude, SpatialReferences.Wgs84);
                        wayPoints.Last().wayPointGraphic.Geometry = newPoint;
                        wayPoints.Last().UpdateLabel();
                    }
                    if (msnMode == MSN_MODE.MSN_WAYPOINT_SELECTED)
                    {
                        double dir = 0;
                        //根据航点与鼠标点的位置调整角度
                        MapPoint mp = (MapPoint)wayPoints.Last().wayPointGraphic.Geometry;
                        dir = CommonCalc.Pt1Pt2Dir(mp.X,mp.Y,longitude, latitude);
                        wayPoints.Last().wayPointSymbol.Angle = dir;
                        wayPoints.Last().inTrack = dir;
                        wayPoints.Last().UpdateLabel();
                        mainWindow.SetInTrack(dir.ToString("F1"));
                    }
                    if (msnMode == MSN_MODE.MSN_ATTACK)
                    {
                        mainWindow.SetLonLat(longitude.ToString("F6"), latitude.ToString("F6"));
                        var newPoint = new MapPoint(longitude,
                           latitude, SpatialReferences.Wgs84);
                        targetPoint.targetGraphic.Geometry = newPoint;
                    }
                    if (msnMode == MSN_MODE.MSN_RECYCLE)
                    {
                        mainWindow.SetLonLat(longitude.ToString("F6"), latitude.ToString("F6"));
                        var newPoint = new MapPoint(longitude,
                           latitude, SpatialReferences.Wgs84);
                        recyclePoint.recycleGraphic.Geometry = newPoint;
                    }
                    if (msnMode == MSN_MODE.MSN_HOVER)
                    {
                        mainWindow.SetLonLat(longitude.ToString("F6"), latitude.ToString("F6"));
                        var newPoint = new MapPoint(longitude,
                           latitude, SpatialReferences.Wgs84);
                        hoverPoints.Last().hoverPtGraphic.Geometry = newPoint;
                    }
                    if (msnMode == MSN_MODE.MSN_HOVER_SELECTED)
                    {
                        //绘制半径圆
                        var newPoint = new MapPoint(longitude,
                          latitude, SpatialReferences.Wgs84);

                        MapPoint pointO = (MapPoint)hoverPoints.Last().hoverPtGraphic.Geometry;

                        double radiusInMeters = CommonCalc.haversine_distance(latitude,longitude,
                            pointO.Y,pointO.X); // 半径，单位为米

                        // 地球椭球参数
                        const double EarthRadius = 6378137.0; // 地球平均半径（单位：米）

                        // 点集，用于创建圆
                        var points = new PointCollection(SpatialReferences.Wgs84);

                        // 生成圆的点（360个点近似圆）
                        for (int i = 0; i < 36; i++)
                        {
                            double angle = i * 10 * Math.PI / 180; // 将角度转换为弧度

                            // 计算在当前角度上的偏移量（以米为单位）
                            double offsetX = radiusInMeters * Math.Cos(angle);
                            double offsetY = radiusInMeters * Math.Sin(angle);

                            // 将偏移量从米转换为经纬度
                            double latitudeOffset = offsetY / EarthRadius * (180 / Math.PI);
                            double longitudeOffset = offsetX / (EarthRadius * Math.Cos(newPoint.Y * Math.PI / 180)) * (180 / Math.PI);

                            // 计算新的点的经纬度
                            double x = pointO.X + longitudeOffset;
                            double y = pointO.Y + latitudeOffset;
                            points.Add(new MapPoint(x, y));
                        }

                        // 创建圆形几何
                        var circleGeometry = new Polygon(points);
                        double displayR = radiusInMeters;
                        if (pointO.Y > latitude)
                        {
                            hoverPoints.Last().hoverRaduisSymbol.Color = System.Drawing.Color.Red;
                            displayR = - displayR;
                        }
                        else
                        {
                            hoverPoints.Last().hoverRaduisSymbol.Color = System.Drawing.Color.Orange;
                        }
                        hoverPoints.Last().hoverRaduisGraphic.Geometry = circleGeometry;
                        mainWindow.SetRadius(displayR.ToString("F1"));
                    }
                }
            }
        }

        private void mapView_MouseMove1(object sender, MouseEventArgs e)
        {
           // e.Handled = true;
           
           // 如果选中了已有航点（文件航点或临时航点），禁用鼠标移动事件
           if (selectedWaypointIndex >= 0)
           {
               return; // 直接返回，不处理鼠标移动事件
           }
        }

        private void mapView_mouseDown(object sender, MouseButtonEventArgs e)
        {
            // 1. 获取鼠标点击的屏幕坐标
            Point screenPoint = e.GetPosition(Control);

            // 2. 转换为地图坐标（MapPoint）
            MapPoint mapLocation = Control.ScreenToLocation(screenPoint);

            // Project the user-tapped map point location to a geometry
            Geometry myGeometry = mapLocation.Project(SpatialReferences.Wgs84);

            // Convert to geometry to a traditional Lat/Long map point
            MapPoint projectedLocation = (MapPoint)myGeometry;

            double longitude = projectedLocation.X;
            double latitude = projectedLocation.Y;
            if (msnMode == MSN_MODE.MSN_NULL)
            {
                // 首先尝试识别任务航点图层
                var missionResults = Control.IdentifyGraphicsOverlayAsync(missionOverlay, screenPoint, 10, false);
                
                // 查找临时航点图层
                var tempOverlay = Control.GraphicsOverlays.FirstOrDefault(o => o.Id == "TempWayPointsOverlay");
                var tempResults = tempOverlay != null ? 
                    Control.IdentifyGraphicsOverlayAsync(tempOverlay, screenPoint, 10, false) : 
                    null;

                // 优先选择任务航点，如果没有则选择临时航点
                var selectedGraphic = null as Graphic;
                
                if (missionResults.Result.Graphics.Count > 0)
                {
                    selectedGraphic = missionResults.Result.Graphics[0];
                }
                else if (tempResults != null && tempResults.Result.Graphics.Count > 0)
                {
                    selectedGraphic = tempResults.Result.Graphics[0];
                }

                // 如果找到图形
                if (selectedGraphic != null)
                {
                    //判断选中的哪一类航点的哪一个
                    curSelectedWayPoint = ((MapPoint)(selectedGraphic.Geometry));

                    // 修改图形的符号以显示选中状态
                    if (selectedGraphic.Symbol is PictureMarkerSymbol)
                    {
                        selectedSquare.squareGraphic.Geometry = selectedGraphic.Geometry;
                        mainWindow.EnableMsnBtn(false);
                        mainWindow.EnableInfo(true);
                        
                        // 通过图形属性找到对应的航点索引
                        FindWaypointIndexByGraphic(selectedGraphic);
                        
                        //通知xaml将选中航点信息更新到左侧textbox中
                        mainWindow.OnSelectWayPoint();
                        
                    }
                }
                else
                {
                    //selectedSquare.squareGraphic.Geometry = new MapPoint(0, 0);
                    mainWindow.EnableMsnBtn(true);
                    mainWindow.EnableInfo(false);
                    selectedSquare.squareGraphic.Geometry = new MapPoint(0, 0);
                    curSelectedWayPoint = null;
                }
            }
            if (msnMode == MSN_MODE.MSN_WAYPOINT)
            {
                msnMode = MSN_MODE.MSN_WAYPOINT_SELECTED;
            }
            else if (msnMode == MSN_MODE.MSN_WAYPOINT_SELECTED)
            {
                msnMode = MSN_MODE.MSN_WAYPOINT_DIRCONFIRMED;
            }
            else if (msnMode == MSN_MODE.MSN_ATTACK)
            {
                msnMode = MSN_MODE.MSN_ATTACK_SELECTED;
            }
            else if (msnMode == MSN_MODE.MSN_HOVER)
            {
                msnMode = MSN_MODE.MSN_HOVER_SELECTED;
            }
            else if (msnMode == MSN_MODE.MSN_HOVER_SELECTED)
            {
                msnMode = MSN_MODE.MSN_HOVER_RADIUS_CONFIRMED;
            }
            else if (msnMode == MSN_MODE.MSN_RECYCLE)
            {
                msnMode = MSN_MODE.MSN_RECYCLE_SELECTED;
            }
        }


        private void mapView_Tapped(object sender, GeoViewInputEventArgs e)
        {
            
            //e.Position
            return;
        }

        private async void InitializeMapView()
        {
            bool offline = true;  //通过offline控制true加载离线地图false下载在线地图，通过安全区的选取确定地图块的中心来下载适用安全的地图块
            if (offline)
            {
                string tileCachePath = @".\flashFile\OfflineMap.tpkx";
                await LoadOfflineMapAsync(tileCachePath);
            }
            else
            {
                //BasemapStyle.ArcGISNavigation,带街道的导航地图
                //BasemapStyle.ArcGISTerrain,地形图
                //BasemapStyle.ArcGISImagery,卫星地图
                Map map = new Map(BasemapStyle.ArcGISImagery);

                Control.Map = map;
            }
            //一次性加载的对象放在这里
            Control.GraphicsOverlays?.Add(missionOverlay);
            //鼠标点击时选中的航点上面的提示方框
            CreateSelectSquare();

            //与任务相关的放在下面这里
            DataInterface.InitUves();
            LoadMissionFile();
            
            //创建三角形方式的飞机模型
            LoadModelAndLabel();


        }

        public void LoadMissionFile()
        {
            if (missionFile == null) 
            {
                return;
            }
            if (Control.Map == null) 
            {
                return;
            }
            // 设置地图的初始中心点 (经度, 纬度) 和缩放比例

            //中心点由安全区计算得来
            double latitude = 0;   // 纬度
            double longitude = 0; // 经度
            double maxlon = -180;
            double maxlat = -90;
            double minlon = 180;
            double minlat = 90;

            int safeAreaPtCount = 0;
            foreach (var pt in missionFile.safeArea)
            {
                latitude += pt.lat;
                longitude += pt.lon;
                safeAreaPtCount++;
                if(maxlon < pt.lon)
                    maxlon = pt.lon;
                if(maxlat < pt.lat)
                    maxlat = pt.lat;
                if(minlon > pt.lon) 
                    minlon = pt.lon;
                if(minlat > pt.lat)
                    minlat = pt.lat;
            }
            longitude /= safeAreaPtCount;
            latitude /= safeAreaPtCount;
            //比例尺由经纬度范围计算得到
            double stepLon = maxlon - minlon;
            double stepLat = maxlat - minlat;
            double step = stepLon>stepLat ? stepLon : stepLat;

            double scale = 100000 * step * 4.0;// 缩放比例（50000意思就是1：50000）
            // 创建地图中心点
            var centerPoint = new MapPoint(longitude, latitude, SpatialReferences.Wgs84);

            // 创建初始视图点
            var initialViewpoint = new Viewpoint(centerPoint, scale);

            // 将视图点设置到地图上
            Control.Map.InitialViewpoint = initialViewpoint;
            Control.SetViewpoint(initialViewpoint);

            //加载安全区到地图中
            AddSafeAreaToMap();

            //加载地图上的关键点信息，如航点、伞降点等
            LoadPointToMap();

            //绘制连接各个航点的曲线
            LoadCurveToMap();

            //创建等待绘制的航迹
            ReadyForRealInfo();


        }

        private void CreateSelectSquare()
        {
            GraphicsOverlay overlay = new GraphicsOverlay();
            overlay.Graphics.Add(selectedSquare.squareGraphic);
            Control.GraphicsOverlays?.Add(overlay);
        }


        private void LoadPointToMap()
        {
            missionOverlay.Graphics.Clear();
            wayPoints.Clear(); // 清空wayPoints列表，避免重复添加
            ClearAllWaypoints(); // 清空统一航点列表
            
            // 获取当前选中的编队索引
            int selectedGroupIndex = 0; // 默认选择第一个编队
            if (mainWindow != null)
            {
                selectedGroupIndex = mainWindow.SelectedGroupIndex;
                if (selectedGroupIndex >= missionFile.flightGroups.Count)
                {
                    selectedGroupIndex = 0; // 如果索引超出范围，使用第一个编队
                }
            }
            
            //绘制所有类型的航点 - 使用当前选中的编队
            foreach (var pt in missionFile.flightGroups[selectedGroupIndex].airLine)
            {
                IWaypointInfo waypointInfo = null;
                
                if (pt.wpType == 1 || pt.wpType == 0) // 飞行航点
                {
                    var wp = new WaypointInfo();
                    wp.lon = pt.lon;
                    wp.lat = pt.lat;
                    wp.high = pt.alt;
                    wp.inTrack = pt.dir;
                    waypointInfo = wp;
                    wayPoints.Add(wp); // 保持原有wayPoints列表用于兼容
                }
                else if (pt.wpType == 2) // 盘旋点
                {
                    var hover = new HoverInfo();
                    hover.lon = pt.lon;
                    hover.lat = pt.lat;
                    hover.high = pt.alt;
                    hover.inTrack = pt.radis; // 半径
                    waypointInfo = hover;
                }
                else if (pt.wpType == 3) // 攻击目标
                {
                    var target = new TargetInfo();
                    target.lon = pt.lon;
                    target.lat = pt.lat;
                    target.high = pt.alt;
                    waypointInfo = target;
                }
                else if (pt.wpType == 6) // 回收点
                {
                    var recycle = new RecycleInfo();
                    recycle.lon = pt.lon;
                    recycle.lat = pt.lat;
                    recycle.high = pt.alt;
                    waypointInfo = recycle;
                }
                
                if (waypointInfo != null)
                {
                    waypointInfo.UpdateGraphic();
                    AddWaypoint(waypointInfo); // 添加到统一航点列表
                    missionOverlay.Graphics.Add(waypointInfo.Graphic);
                    if (waypointInfo.LabelDefinition != null)
                    {
                        missionOverlay.LabelDefinitions.Add(waypointInfo.LabelDefinition);
                    }
                }
            }
            missionOverlay.LabelsEnabled = true;
            //绘制伞降点
            MapPoint mpRecycle = new MapPoint(DataInterface.recyclePoint.lon, DataInterface.recyclePoint.lat, SpatialReferences.Wgs84);

            string recyclePngPath = @".\\flashFile\\parachute.png";

            // 如果 FBX 文件和纹理图片在同一目录下，ArcGIS 会自动加载纹理
            Uri recyclePngUri = new Uri(recyclePngPath, UriKind.Relative); //($"file:///{modelPath}");
            
            var markerSymbol1 = new PictureMarkerSymbol(recyclePngUri)
            {
                Width = 24,
                Height = 24,
                OffsetY = 12 // 使图标底部对齐位置点
            };
            // 创建 Graphic
            var graphic1 = new Graphic(mpRecycle, markerSymbol1);// 设置大头针样式
                                                                 // 将点添加到图层
            // 创建 GraphicsOverlay
            var graphicsOverlay = new GraphicsOverlay();
            Control.GraphicsOverlays.Add(graphicsOverlay);
            graphicsOverlay.Graphics.Add(graphic1);
        }

        // 新增：加载临时航点到地图
        public void LoadTempWayPointsToMap(List<WayPoint> tempWayPoints)
        {
            // 先清除之前的临时航点图层
            var existingOverlay = Control.GraphicsOverlays.FirstOrDefault(o => o.Id == "TempWayPointsOverlay");
            if (existingOverlay != null)
            {
                Control.GraphicsOverlays.Remove(existingOverlay);
                System.Diagnostics.Debug.WriteLine("已清除临时航点图层");
            }
            
            // 如果没有临时航点，直接返回（已经清除了图层）
            if (tempWayPoints == null || tempWayPoints.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("临时航点列表为空，已清除地图上的临时航点");
                return;
            }
            
            System.Diagnostics.Debug.WriteLine($"正在加载 {tempWayPoints.Count} 个临时航点到地图");

            // 为临时航点创建单独的图层
            GraphicsOverlay tempWayPointsOverlay = new GraphicsOverlay();
            tempWayPointsOverlay.Id = "TempWayPointsOverlay";
            
            foreach (var pt in tempWayPoints)
            {
                if (pt.wpType == 1 || pt.wpType == 0) // 飞行航点
                {
                    WaypointInfo wp = new WaypointInfo();
                    wp.lon = pt.lon;
                    wp.lat = pt.lat;
                    wp.high = pt.alt;
                    wp.inTrack = pt.dir;
                    wp.UpdateGraphic();
                    
                    // 使用不同的颜色或样式来区分临时航点
                    // 可以修改WaypointInfo的符号样式
                    tempWayPointsOverlay.Graphics.Add(wp.wayPointGraphic);
                    tempWayPointsOverlay.LabelDefinitions.Add(wp.labelDefinition);
                }
                else if (pt.wpType == 3) // 攻击目标
                {
                    TargetInfo target = new TargetInfo();
                    target.lon = pt.lon;
                    target.lat = pt.lat;
                    target.high = pt.alt;
                    target.UpdateGraphic();
                    
                    tempWayPointsOverlay.Graphics.Add(target.targetGraphic);
                    tempWayPointsOverlay.LabelDefinitions.Add(target.labelDefinition);
                }
                else if (pt.wpType == 6) // 回收点
                {
                    RecycleInfo recyclept = new RecycleInfo();
                    recyclept.lon = pt.lon;
                    recyclept.lat = pt.lat;
                    recyclept.high = pt.alt;
                    recyclept.UpdateGraphic();
                    
                    tempWayPointsOverlay.Graphics.Add(recyclept.recycleGraphic);
                    tempWayPointsOverlay.LabelDefinitions.Add(recyclept.labelDefinition);
                }
            }
            
            // 添加新的临时航点图层
            tempWayPointsOverlay.LabelsEnabled = true;
            Control.GraphicsOverlays.Add(tempWayPointsOverlay);
        }

        private void LoadModelAndLabel()
        {
            var graphicsOverlay = new GraphicsOverlay
            {
                Id = "PointsOfInterestGraphics",
               // SceneProperties = new LayerSceneProperties(SurfacePlacement.Absolute)
            };
                       
            foreach(var uve in DataInterface.UVEs)
            {
                int pix = 25;
                if (uve.Value.rank == UVE_RANK.MASTER)
                {
                    pix = 25;
                }
                var triangleSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Triangle, uve.Value.color, pix);
                triangleSymbol.Outline = new SimpleLineSymbol
                {
                    Style = SimpleLineSymbolStyle.Solid,   // 实线样式
                    Color = System.Drawing.Color.White,      // 边缘颜色
                    Width = 2                              // 边缘宽度（像素）
                };
                triangleSymbol.Angle = DataInterface.luanchInfo.dir;
                _trianglePlaneSymbol.Add(triangleSymbol);

                var planeGraphic = new Graphic(new MapPoint(
                    DataInterface.luanchInfo.wGS84Pos.lon,
                    DataInterface.luanchInfo.wGS84Pos.lat, SpatialReferences.Wgs84), triangleSymbol);
                planeGraphic.Attributes["Name"] = String.Format("{0}", uve.Value.uveName);
                planeGraphic.Attributes["Description"] = "不知道1";
                _planeGraphicIndex[(uve.Value.uveGroupID, uve.Value.uveMsnID)] = _planeGraphics.Count;
                _planeGraphics.Add(planeGraphic);
                graphicsOverlay.Graphics.Add(planeGraphic);
            }
            var textSymbol = new TextSymbol
            {
                Color = System.Drawing.Color.Red,
                HaloColor = System.Drawing.Color.White,
                HaloWidth = 2,
                Size = 20
            };

            
            var simpleLabelExpression = new SimpleLabelExpression("[Name]");

            // 定义标签规则，根据属性名称生成标签
            LabelDefinition labelDefinition = new LabelDefinition(simpleLabelExpression, textSymbol)
            {
                Placement = LabelingPlacement.PointCenterRight
            };
            graphicsOverlay.LabelDefinitions.Add(labelDefinition);
            graphicsOverlay.LabelsEnabled = true;

            // Add the graphic overlay to the geo view's graphics overlay collection.
            Control.GraphicsOverlays?.Add(graphicsOverlay);
        }

        private void AddSafeAreaToMap()
        {
            // 创建区域的坐标点列表
            var points = new List<MapPoint>();
            foreach (var pt in missionFile.safeArea) 
            {
                points.Add(new MapPoint(pt.lon, pt.lat, SpatialReferences.Wgs84));
            }
           
            // 使用点列表创建多边形
            var polygon = new Polygon(points);

            // 创建填充样式
            var fillColor = System.Drawing.Color.FromArgb(10, 0, 0, 200); // 半透明绿色，A=100
            var lineColor = System.Drawing.Color.Yellow; // 红色边界

            var fillSymbol = new SimpleFillSymbol(
                SimpleFillSymbolStyle.Solid,   // 填充样式
                fillColor,                    // 填充颜色
                new SimpleLineSymbol(
                    SimpleLineSymbolStyle.Solid,
                    lineColor,                // 边界颜色
                    2)                         // 边界宽度
            );

            // 创建 Graphic 对象，将样式与多边形绑定
            var polygonGraphic = new Graphic(polygon, fillSymbol);
            // 创建 GraphicsOverlay 并添加到 MapView
            if (SafeAreaOverlay != null)
            {
                Control.GraphicsOverlays?.Remove(SafeAreaOverlay);
            }
            var graphicsOverlay = new GraphicsOverlay();
            SafeAreaOverlay = graphicsOverlay;
            Control.GraphicsOverlays?.Add(graphicsOverlay);

            // 将 Graphic 添加到 GraphicsOverlay
            graphicsOverlay.Graphics.Add(polygonGraphic);

        }

        List<List<MapPoint>> curvePoints = new List<List<MapPoint>>();
        List<GraphicsOverlay> curveOverlays = new List<GraphicsOverlay>();
        
        // 新增：加载指定编队的航点到地图
        public void LoadPointToMapForGroup(int groupIndex)
        {
            if (groupIndex < 0 || groupIndex >= missionFile.flightGroups.Count)
            {
                System.Diagnostics.Debug.WriteLine($"编队索引无效: {groupIndex}");
                return;
            }
            
            // 清除当前编队的航点
            missionOverlay.Graphics.Clear();
            wayPoints.Clear();
            ClearAllWaypoints();
            
            // 重新加载指定编队的航点
            var group = missionFile.flightGroups[groupIndex];
            System.Diagnostics.Debug.WriteLine($"LoadPointToMapForGroup - 开始重新加载编队{groupIndex}的航点");
            System.Diagnostics.Debug.WriteLine($"LoadPointToMapForGroup - airLine中的航点坐标:");
            for (int i = 0; i < group.airLine.Count; i++)
            {
                var pt = group.airLine[i];
                System.Diagnostics.Debug.WriteLine($"  airLine[{i}]: wpType={pt.wpType}, 坐标({pt.lon:F6}, {pt.lat:F6})");
            }
            
            // 验证引用是否相同
            System.Diagnostics.Debug.WriteLine($"LoadPointToMapForGroup - 引用验证: group.airLine == missionFile.airLine: {group.airLine == missionFile.flightGroups[groupIndex].airLine}");
            
            foreach (var pt in group.airLine)
            {
                IWaypointInfo waypointInfo = null;
                
                if (pt.wpType == 1 || pt.wpType == 0) // 飞行航点
                {
                    var wp = new WaypointInfo();
                    wp.lon = pt.lon;
                    wp.lat = pt.lat;
                    wp.high = pt.alt;
                    wp.inTrack = pt.dir;
                    wp.speed = pt.speed;
                    waypointInfo = wp;
                    wayPoints.Add(wp);
                }
                else if (pt.wpType == 2) // 盘旋点
                {
                    var hover = new HoverInfo();
                    hover.lon = pt.lon;
                    hover.lat = pt.lat;
                    hover.high = pt.alt;
                    hover.inTrack = pt.radis; // 半径
                    waypointInfo = hover;
                }
                else if (pt.wpType == 3) // 攻击目标
                {
                    var target = new TargetInfo();
                    target.lon = pt.lon;
                    target.lat = pt.lat;
                    target.high = pt.alt;
                    waypointInfo = target;
                }
                else if (pt.wpType == 6) // 回收点
                {
                    var recycle = new RecycleInfo();
                    recycle.lon = pt.lon;
                    recycle.lat = pt.lat;
                    recycle.high = pt.alt;
                    waypointInfo = recycle;
                }
                
                if (waypointInfo != null)
                {
                    waypointInfo.UpdateGraphic();
                    AddWaypoint(waypointInfo);
                    missionOverlay.Graphics.Add(waypointInfo.Graphic);
                    if (waypointInfo.LabelDefinition != null)
                    {
                        missionOverlay.LabelDefinitions.Add(waypointInfo.LabelDefinition);
                    }
                    System.Diagnostics.Debug.WriteLine($"LoadPointToMapForGroup - 添加航点: wpType={pt.wpType}, 坐标({pt.lon:F6}, {pt.lat:F6})");
                }
            }
            
            missionOverlay.LabelsEnabled = true;
            System.Diagnostics.Debug.WriteLine($"重新加载编队{groupIndex}的航点: 共{allWaypoints.Count}个航点");
        }

        // 新增：更新指定编队的航线
        public void UpdateCurveForGroup(int groupIndex)
        {
            if (groupIndex < 0 || groupIndex >= missionFile.flightGroups.Count)
            {
                System.Diagnostics.Debug.WriteLine($"编队索引无效: {groupIndex}");
                return;
            }
            
            // 移除所有航线（因为每次都是重新创建）
            while (curveOverlays.Count > 0)
            {
                Control.GraphicsOverlays?.Remove(curveOverlays[0]);
                curveOverlays.RemoveAt(0);
            }
            System.Diagnostics.Debug.WriteLine($"清除所有航线，curveOverlays.Count={curveOverlays.Count}");
            
            // 重新创建当前编队的航线
            var group = missionFile.flightGroups[groupIndex];
            var allWaypoints = new List<MapPoint>();
            
            System.Diagnostics.Debug.WriteLine($"UpdateCurveForGroup - 开始更新编队{groupIndex}的航线");
            System.Diagnostics.Debug.WriteLine($"UpdateCurveForGroup - airLine中的航点坐标:");
            for (int i = 0; i < group.airLine.Count; i++)
            {
                var pt = group.airLine[i];
                System.Diagnostics.Debug.WriteLine($"  airLine[{i}]: wpType={pt.wpType}, 坐标({pt.lon:F6}, {pt.lat:F6})");
            }
            
            // 验证引用是否相同
            System.Diagnostics.Debug.WriteLine($"UpdateCurveForGroup - 引用验证: group.airLine == missionFile.airLine: {group.airLine == missionFile.flightGroups[groupIndex].airLine}");
            
            // 收集所有航点的坐标（包括所有类型）
            foreach (var pt in group.airLine)
            {
                allWaypoints.Add(new MapPoint(pt.lon, pt.lat, SpatialReferences.Wgs84));
                System.Diagnostics.Debug.WriteLine($"更新航线 - 添加航点: 编队{groupIndex}, wpType={pt.wpType}, 坐标({pt.lon:F6}, {pt.lat:F6})");
            }
            
            // 只有当有足够的点时才创建折线
            if (allWaypoints.Count >= 2)
            {
                // 更新curvePoints
                if (groupIndex < curvePoints.Count)
                {
                    curvePoints[groupIndex] = allWaypoints;
                }
                else
                {
                    curvePoints.Add(allWaypoints);
                }
                
                // 创建 Polyline（折线）
                var polyline = new Polyline(allWaypoints);

                // 创建折线样式
                var lineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Dash, System.Drawing.Color.Cyan, 2);

                // 创建 Graphic 对象，并添加到地图
                var polylineGraphic = new Graphic(polyline, lineSymbol);

                // 创建 GraphicsOverlay 并将其添加到 MapView
                var graphicsOverlay = new GraphicsOverlay();
                Control.GraphicsOverlays?.Add(graphicsOverlay);
                
                // 添加到末尾（因为每次都是重新创建）
                curveOverlays.Add(graphicsOverlay);
                System.Diagnostics.Debug.WriteLine($"添加航线到末尾，curveOverlays.Count={curveOverlays.Count}");
                
                // 将折线添加到 GraphicsOverlay
                graphicsOverlay.Graphics.Add(polylineGraphic);
                
                System.Diagnostics.Debug.WriteLine($"更新航线: 编队{groupIndex}, 航点数{allWaypoints.Count}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"编队{groupIndex}航点不足，无法创建航线: 航点数{allWaypoints.Count}");
            }
        }
        public void LoadCurveToMap()
        {
            curvePoints.Clear();
            while (curveOverlays.Count > 0) 
            {
                Control.GraphicsOverlays?.Remove(curveOverlays[0]);
                curveOverlays.RemoveAt(0);
            }  
            int i = 0;
            foreach (var group in missionFile.flightGroups)
            {
                curvePoints.Add(new List<MapPoint>());
                foreach (var pt in group.airLine)
                {
                    curvePoints[i].Add(new MapPoint(pt.lon, pt.lat, SpatialReferences.Wgs84));
                    // 创建 Polyline（折线）
                    var polyline = new Polyline(curvePoints[i]);

                    // 创建折线样式
                    var lineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Dash, System.Drawing.Color.Cyan, 2);

                     // 创建 Graphic 对象，并添加到地图
                    var polylineGraphic = new Graphic(polyline, lineSymbol);

                    // 创建 GraphicsOverlay 并将其添加到 MapView
                    var graphicsOverlay = new GraphicsOverlay();
                    Control.GraphicsOverlays?.Add(graphicsOverlay);
                    curveOverlays.Add(graphicsOverlay);
                    // 将折线添加到 GraphicsOverlay
                    graphicsOverlay.Graphics.Add(polylineGraphic);
                }
                i++;
            }


           
        }
        private void ReadyForRealInfo() 
        {
            foreach (var uve in DataInterface.UVEs)
            {
                
                var polylineBuilder = new PolylineBuilder(SpatialReferences.Wgs84);

                // var dynamicPolyline = new Polyline();
                // 初始化 Polyline 和图形覆盖层
                var dynamicPolyline = new Polyline(new List<MapPoint>());
                var lineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, uve.Value.color, 7);
                var polylineGraphic = new Graphic(dynamicPolyline, lineSymbol);
                var graphicsOverlay = new GraphicsOverlay();
                graphicsOverlay.Graphics.Add(polylineGraphic);
                _polylineIndex[(uve.Value.uveGroupID, uve.Value.uveMsnID)] = _polylineGraphics.Count;
                _polylineBuilder.Add(polylineBuilder);
                _polylineGraphics.Add(polylineGraphic);
                Control.GraphicsOverlays?.Add(graphicsOverlay);
            }
        }

        public void TimerFresh()
        {
            AddPointToDynamicCurve();
        }

        private void AddPointToDynamicCurve()
        {
            foreach (var uve in DataInterface.UVEs)
            {
                if (uve.Value.uveEnable == 0)
                    continue;
                if (uve.Value.curInfo.wGS84Pos.lon == 0)
                    continue;
                // System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "轨迹调试.log",
                //     string.Format("[{0}] AddPoint uve=({1},{2}) en={3} lon={4} lat={5}\r\n",
                //     DateTime.Now.ToString("HH:mm:ss.fff"), uve.Value.uveGroupID, uve.Value.uveMsnID,
                //     uve.Value.uveEnable, uve.Value.curInfo.wGS84Pos.lon, uve.Value.curInfo.wGS84Pos.lat));
                if (!_planeGraphicIndex.TryGetValue((uve.Value.uveGroupID, uve.Value.uveMsnID), out int idx))
                    continue;
                var newPoint = new MapPoint(uve.Value.curInfo.wGS84Pos.lon,
                    uve.Value.curInfo.wGS84Pos.lat, SpatialReferences.Wgs84);
                if (idx < _polylineBuilder.Count && _polylineBuilder[idx] != null)
                    _polylineBuilder[idx].AddPoint(newPoint);
                if (idx < _polylineGraphics.Count && _polylineGraphics[idx] != null && _polylineBuilder[idx] != null)
                    _polylineGraphics[idx].Geometry = _polylineBuilder[idx].ToGeometry();
                if (idx < _planeGraphics.Count && _planeGraphics[idx] != null)
                    _planeGraphics[idx].Geometry = newPoint;
                if (idx < _trianglePlaneSymbol.Count && _trianglePlaneSymbol[idx] != null)
                    _trianglePlaneSymbol[idx].Angle = uve.Value.curInfo.dir;
            }
        }
    }

    public enum MSN_MODE
    {
        MSN_NULL = 0,
        MSN_WAYPOINT,
        MSN_WAYPOINT_SELECTED,
        MSN_WAYPOINT_DIRCONFIRMED,
        MSN_HOVER,
        MSN_HOVER_SELECTED,
        MSN_HOVER_RADIUS_CONFIRMED,
        MSN_ATTACK,
        MSN_ATTACK_SELECTED,
        MSN_CANCEL_ATTACK,
        MSN_HOME,
        MSN_RECYCLE,
        MSN_RECYCLE_SELECTED,
    };

    // 新增：统一的航点信息接口
    public interface IWaypointInfo
    {
        Graphic Graphic { get; }
        LabelDefinition LabelDefinition { get; }
        double Lon { get; set; }
        double Lat { get; set; }
        double Alt { get; set; }
        int WpType { get; }
        void UpdateGraphic();
        void UpdateLabel();
    }

    public class WaypointInfo : IWaypointInfo
    {
        public Graphic wayPointGraphic;
        public LabelDefinition labelDefinition;
        public PictureMarkerSymbol wayPointSymbol;
        public double lon = 0;
        public double lat = 0;
        public double inTrack = 0;
        public double high = 1500;
        public double speed = 0;

        // 实现IWaypointInfo接口
        public Graphic Graphic => wayPointGraphic;
        public LabelDefinition LabelDefinition => labelDefinition;
        public double Lon { get => lon; set => lon = value; }
        public double Lat { get => lat; set => lat = value; }
        public double Alt { get => high; set => high = value; }
        public int WpType => 1; // 飞行航点


        
        public WaypointInfo()
        {
            MapPoint mp = new MapPoint(0, 0, SpatialReferences.Wgs84);
            string modelPath = @".\\flashFile\\right-arrow.png";

            // 如果 FBX 文件和纹理图片在同一目录下，ArcGIS 会自动加载纹理
            Uri modelUri = new Uri(modelPath, UriKind.Relative); //($"file:///{modelPath}");

            wayPointSymbol = new PictureMarkerSymbol(modelUri)
            {
                Width = 24,
                Height = 24,
            };
            wayPointSymbol.Angle = 0;
            // 创建 Graphic
            wayPointGraphic = new Graphic(mp, wayPointSymbol);
            wayPointGraphic.Attributes["Name"] = $"alt: {high}m\ndir: {inTrack}°";

            var textSymbol = new TextSymbol
            {
                Color = System.Drawing.Color.White,
                HaloColor = System.Drawing.Color.Black,
                HaloWidth = 2,
                Size = 18
            };

            var simpleLabelExpression = new SimpleLabelExpression("[Name]");

            // 定义标签规则，根据属性名称生成标签
            labelDefinition = new LabelDefinition(simpleLabelExpression, textSymbol)
            {
                Placement = LabelingPlacement.PointCenterRight
            };
        }
        public void UpdateLabel()
        {
            wayPointGraphic.Attributes["Name"] = $"alt: {high:F0}m\ndir: {inTrack:F1}°";
        }

        public void UpdateGraphic()
        {
            var newPoint = new MapPoint(lon,
                          lat, SpatialReferences.Wgs84);
            wayPointGraphic.Geometry = newPoint;
            wayPointSymbol.Angle = inTrack;

            UpdateLabel();
        }
    }

    public class HoverInfo : IWaypointInfo
    {
        public Graphic hoverPtGraphic;
        public Graphic hoverRaduisGraphic;
        public SimpleLineSymbol hoverRaduisSymbol;

        public double lon = 0;
        public double lat = 0;
        public double inTrack = 0;
        public double high = 0;

        // 实现IWaypointInfo接口
        public Graphic Graphic => hoverPtGraphic;
        public LabelDefinition LabelDefinition => null; // 盘旋点没有标签定义
        public double Lon { get => lon; set => lon = value; }
        public double Lat { get => lat; set => lat = value; }
        public double Alt { get => high; set => high = value; }
        public int WpType => 2; // 盘旋点
        public HoverInfo()
        {
            MapPoint mp = new MapPoint(0, 0, SpatialReferences.Wgs84);
            string modelPath = @".\\flashFile\\hover.png";

            // 如果 FBX 文件和纹理图片在同一目录下，ArcGIS 会自动加载纹理
            Uri modelUri = new Uri(modelPath, UriKind.Relative); //($"file:///{modelPath}");

            var hoverSettingSymbol = new PictureMarkerSymbol(modelUri)
            {
                Width = 28,
                Height = 28,
            };
            // 创建中心点Graphic
            hoverPtGraphic = new Graphic(mp, hoverSettingSymbol);

            //创建半径圆圈Graphic
            var points = new PointCollection(SpatialReferences.Wgs84);
            var circleGeometry = new Polygon(points);
            hoverRaduisSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Dash, System.Drawing.Color.Red, 2);
            hoverRaduisGraphic = new Graphic(circleGeometry, hoverRaduisSymbol);

        }

        public void UpdateGraphic()
        {
            var newPoint = new MapPoint(lon,
                          lat, SpatialReferences.Wgs84);
            hoverPtGraphic.Geometry = newPoint;

            hoverPtGraphic.Attributes["Name"] = $"alt: {high:F0}m\nR: {inTrack:F1}m";
        }

        public void UpdateLabel()
        {
            if (hoverPtGraphic != null)
            {
                hoverPtGraphic.Attributes["Name"] = $"alt: {high:F0}m\nR: {inTrack:F1}m";
            }
        }
    }
    public class TargetInfo : IWaypointInfo
    {
        public Graphic targetGraphic;
        public double lon = 0;
        public double lat = 0;
        public double high = 0;
        public LabelDefinition labelDefinition;

        // 实现IWaypointInfo接口
        public Graphic Graphic => targetGraphic;
        public LabelDefinition LabelDefinition => labelDefinition;
        public double Lon { get => lon; set => lon = value; }
        public double Lat { get => lat; set => lat = value; }
        public double Alt { get => high; set => high = value; }
        public int WpType => 3; // 攻击目标

        public TargetInfo()
        {
            MapPoint mp = new MapPoint(0, 0, SpatialReferences.Wgs84);
            string modelPath = @".\\flashFile\\attack.png";
            var targetSymbol = new PictureMarkerSymbol(new Uri(modelPath, UriKind.Relative))
            {
                Width = 32,
                Height = 32,
            };
            targetGraphic = new Graphic(mp, targetSymbol);

            targetGraphic.Attributes["Name"] = $"alt: {high}m";

            var textSymbol = new TextSymbol
            {
                Color = System.Drawing.Color.White,
                HaloColor = System.Drawing.Color.Black,
                HaloWidth = 2,
                Size = 18
            };

            var simpleLabelExpression = new SimpleLabelExpression("[Name]");

            // 定义标签规则，根据属性名称生成标签
            labelDefinition = new LabelDefinition(simpleLabelExpression, textSymbol)
            {
                Placement = LabelingPlacement.PointCenterRight
            };
        }
        public void UpdateGraphic()
        {
            var newPoint = new MapPoint(lon,
                          lat, SpatialReferences.Wgs84);
            targetGraphic.Geometry = newPoint;

            targetGraphic.Attributes["Name"] = $"alt: {high:F0}m";
        }

        public void UpdateLabel()
        {
            if (targetGraphic != null)
            {
                targetGraphic.Attributes["Name"] = $"alt: {high:F0}m";
            }
        }
        
    }

    public class RecycleInfo : IWaypointInfo
    {
        public Graphic recycleGraphic;
        public double lon = 0;
        public double lat = 0;
        public double high = 0;
        public LabelDefinition labelDefinition;

        // 实现IWaypointInfo接口
        public Graphic Graphic => recycleGraphic;
        public LabelDefinition LabelDefinition => labelDefinition;
        public double Lon { get => lon; set => lon = value; }
        public double Lat { get => lat; set => lat = value; }
        public double Alt { get => high; set => high = value; }
        public int WpType => 6; // 回收点

        public RecycleInfo()
        {
            MapPoint mp = new MapPoint(0, 0, SpatialReferences.Wgs84);
            string modelPath = @".\\flashFile\\parachute.png";
            var targetSymbol = new PictureMarkerSymbol(new Uri(modelPath, UriKind.Relative))
            {
                Width = 32,
                Height = 32,
            };
            recycleGraphic = new Graphic(mp, targetSymbol);

            recycleGraphic.Attributes["Name"] = $"alt: {high}m";

            var textSymbol = new TextSymbol
            {
                Color = System.Drawing.Color.White,
                HaloColor = System.Drawing.Color.Black,
                HaloWidth = 2,
                Size = 18
            };

            var simpleLabelExpression = new SimpleLabelExpression("[Name]");

            // 定义标签规则，根据属性名称生成标签
            labelDefinition = new LabelDefinition(simpleLabelExpression, textSymbol)
            {
                Placement = LabelingPlacement.PointCenterRight
            };
        }
        public void UpdateGraphic()
        {
            var newPoint = new MapPoint(lon,
                          lat, SpatialReferences.Wgs84);
            recycleGraphic.Geometry = newPoint;

            recycleGraphic.Attributes["Name"] = $"alt: {high:F0}m";
        }

        public void UpdateLabel()
        {
            if (recycleGraphic != null)
            {
                recycleGraphic.Attributes["Name"] = $"alt: {high:F0}m";
            }
        }

    }

    public class SelectedSquare
    {
        public Graphic squareGraphic;
        public double lat = 0;
        public double lon = 0;
        public SelectedSquare()
        {
            var hollowSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Square, System.Drawing.Color.Transparent, 35)
            {
                Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Lime, 2) // 边框
            };
            MapPoint mp = new MapPoint(0, 0, SpatialReferences.Wgs84);
            squareGraphic = new Graphic(mp, hollowSymbol);
        }
    }
}
