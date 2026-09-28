using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System.IO;
using System.Windows.Threading;
using System.Collections;
using Esri.ArcGISRuntime.Mapping.Labeling;
using Esri.ArcGISRuntime.ArcGISServices;
using System.Drawing;
using SimuControl280;
//using System.Windows.Media.Media3D;
namespace GroundLunch
{
    public class MyCameraView
    {
        private List<Graphic> _polylineGraphic = new List<Graphic>();
        private List<PolylineBuilder> _polylineBuilder = new List<PolylineBuilder>();

        private SimpleMarkerSceneSymbol _planeSymbol ;
        private Graphic _planeGraphic;
        //private Graphic _textGraphic;


        // Camera controller for centering the camera on the airplane
        private OrbitGeoElementCameraController _orbitCameraController;

        // Timer enables frame-by-frame animation
        private DispatcherTimer _animationTimer;

        // Number of frames in the mission animation
        private int _frameCount;

        // Index of current frame in the animation
        private int _keyframe;


        public SceneView Control { get; set; }

        public MyCameraView(SceneView view)
        {
            Control = view;
            Control.GeoViewTapped += cameraView_Tapped;
            InitializeSceneView();
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
            var offlineMap = new Scene
            {
                Basemap = new Basemap(tiledLayer)
            };

            // 将离线地图设置为当前地图
            Control.Scene = offlineMap;

            await Task.CompletedTask;
        }
        private async void InitializeSceneView()
        {
            bool offline = false;
            if (offline == false)
            {
                // 创建一个新的 Scene
                var scene = new Scene(BasemapStyle.ArcGISImageryStandard);

                // 添加全球地形
                var elevationSource = new ArcGISTiledElevationSource(
                    new Uri("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer"));
                scene.BaseSurface.ElevationSources.Add(elevationSource);

                // 设置 Scene 到 SceneView
                Control.Scene = scene;
            }
            else
            {
                string tileCachePath = @".\flashFile\OfflineMap.tpkx";
                await LoadOfflineMapAsync(tileCachePath);
            }

            // 设置初始视图点

            LoadPlaneModel();
            AddFocusSquare();


            _orbitCameraController = new OrbitGeoElementCameraController(_planeGraphic, 10.0)
            {
                CameraPitchOffset = 0,
            };

            Control.CameraController = _orbitCameraController;

            _orbitCameraController.CameraPitchOffset = 70;

            DataInterface.viewPlane.scoutPitch = -15;
            DataInterface.viewPlane.scoutHeading = 0;
            DataInterface.viewPlane.scoutPitchAim = -15;
            DataInterface.viewPlane.scoutHeadingAim = 0;
        }

        private void AddFocusSquare()
        {
            var screenOverlay = new GraphicsOverlay
            {
                RenderingMode = GraphicsRenderingMode.Static // 使用屏幕坐标
            };


            // 创建一个矩形（使用 WPF 的 Point）
            var points = new List<Point>
            {
                new Point(1, 1),
                new Point(1, 2),
                new Point(2, 2),
                new Point(2, 1),
                new Point(1, 1) // 闭合
            };
        }

       //将飞机模型添加到场景中
        public async void LoadPlaneModel()
        {
            // Create a new graphics overlay.
            var graphicsOverlay = new GraphicsOverlay
            {
                Id = "PointsOfInterestGraphics",
                SceneProperties = new LayerSceneProperties(SurfacePlacement.Absolute)
            };

            // Create a renderer to handle updating plane's orientation
            SimpleRenderer renderer3D = new SimpleRenderer();
            RendererSceneProperties renderProperties = renderer3D.SceneProperties;
            // Use expressions to keep the renderer properties updated as parameters of the rendered object
            renderProperties.HeadingExpression = "[HEADING]";
            renderProperties.PitchExpression = "[PITCH]";
            renderProperties.RollExpression = "[ROLL]";
            // Apply the renderer to the scene view's overlay
            graphicsOverlay.Renderer = renderer3D;

            // Create a new map point to define the graphic location.
            var pierPoint = new MapPoint(DataInterface.luanchInfo.wGS84Pos.lon,
                DataInterface.luanchInfo.wGS84Pos.lat,
                DataInterface.luanchInfo.wGS84Pos.alt + 20, SpatialReferences.Wgs84);

           // var pierPoint1 = new MapPoint(113.602055,
           //     34.9701673,
           //     300, SpatialReferences.Wgs84);

            // 加载 FBX 文件路径（确保模型与纹理图片在相同目录下）
            //string modelPath = @"D:\LP\713_2\GroundUI\GroundUI\bin\x64\Debug\net8.0-windows10.0.19041.0\win-x64\low-poly-airplane\source\Airplane\Bristol.dae"; // FBX 文件路径
            string modelPath = @".\\flashFile\\Bristol.dae";
            //string modelPath = @".\\flashFile\\SWITCHBLADE\\untitled.dae";

            // 如果 FBX 文件和纹理图片在同一目录下，ArcGIS 会自动加载纹理
            Uri modelUri = new Uri(modelPath, UriKind.Relative); //($"file:///{modelPath}");
                                                                 // Uri _modelUri = new Uri(DataManager.GetDataFolder("681d6f7694644709a7c830ec57a2d72b", "Bristol.dae"));

          

            SimpleMarkerSceneSymbol planeSymbol = new SimpleMarkerSceneSymbol
            {
                Style = SimpleMarkerSceneSymbolStyle.Cube,
                Color = System.Drawing.Color.Transparent,   // 设置颜色
                Height = 20,                        // 设置高度
                Width = 20,
                Depth = 20,                            // 设置大小
            };

            planeSymbol.Heading = 0;
            planeSymbol.Pitch = 15;
            planeSymbol.Roll = 0;
            planeSymbol.AnchorPosition = SceneSymbolAnchorPosition.Center;
            _planeSymbol = planeSymbol;
            // Create a new graphic.
            var planeGraphic = new Graphic(new MapPoint(pierPoint.X, pierPoint.Y + 0.00018, pierPoint.Z + 20, SpatialReferences.Wgs84), planeSymbol);
            //planeGraphic.Attributes["Name"] = DataInterface.UVEs[i].uveName;
           // planeGraphic.Attributes["Description"] = "不知道1";
            graphicsOverlay.Graphics.Add(planeGraphic);
            _planeGraphic = planeGraphic;

            Control.GraphicsOverlays?.Add(graphicsOverlay);
          
        }
        
        public void TimerFresh()
        {
            AddRealPointToScene();
        }

        private void AddRealPointToScene()
        {
            // Get the next position; % prevents going out of bounds even if the keyframe value is
            //     changed unexpectedly (e.g. due to user interaction with the progress slider).
            var newPoint = new MapPoint(DataInterface.viewPlane.lon,
                DataInterface.viewPlane.lat,
                DataInterface.viewPlane.alt + 20.1,
                SpatialReferences.Wgs84);

           // var newPoint1 = new MapPoint(113.602055,
             //   34.9701673,
           //     3001,
            //    SpatialReferences.Wgs84);//test
            _planeGraphic.Geometry = newPoint;//test
            //DataInterface.viewPlane.heading = 90;//test
            _planeGraphic.Attributes["HEADING"] = DataInterface.viewPlane.heading;
            // _planeGraphic.Attributes["PITCH"] = DataInterface.curInfo.pitch;
            _planeGraphic.Attributes["ROLL"] = DataInterface.viewPlane.roll;
            //_planeSymbol.Pitch = DataInterface.viewPlane.pitch;

            if (DataInterface.viewPlane.scoutPitch != DataInterface.viewPlane.scoutPitchAim)
            {
                if (DataInterface.viewPlane.scoutPitch < DataInterface.viewPlane.scoutPitchAim)
                {
                    if (DataInterface.viewPlane.scoutPitchAim - DataInterface.viewPlane.scoutPitch > 0.1)
                        DataInterface.viewPlane.scoutPitch += 0.2;
                    else
                        DataInterface.viewPlane.scoutPitch = DataInterface.viewPlane.scoutPitchAim;
                }
                else
                {
                    if (DataInterface.viewPlane.scoutPitchAim - DataInterface.viewPlane.scoutPitch < -0.1)
                        DataInterface.viewPlane.scoutPitch -= 0.2;
                    else
                        DataInterface.viewPlane.scoutPitch = DataInterface.viewPlane.scoutPitchAim;

                }
            }
            if (DataInterface.viewPlane.scoutHeading != DataInterface.viewPlane.scoutHeadingAim)
            {
                if (DataInterface.viewPlane.scoutHeading < DataInterface.viewPlane.scoutHeadingAim)
                {
                    if (DataInterface.viewPlane.scoutHeadingAim - DataInterface.viewPlane.scoutHeading > 0.1)
                        DataInterface.viewPlane.scoutHeading += 0.2;
                    else
                        DataInterface.viewPlane.scoutHeading = DataInterface.viewPlane.scoutHeadingAim;
                }
                else
                {
                    if (DataInterface.viewPlane.scoutHeadingAim - DataInterface.viewPlane.scoutHeading < -0.1)
                        DataInterface.viewPlane.scoutHeading -= 0.2;
                    else
                        DataInterface.viewPlane.scoutHeading = DataInterface.viewPlane.scoutHeadingAim;

                }
            }
            _orbitCameraController.CameraPitchOffset = 90 + DataInterface.viewPlane.scoutPitch;
            _orbitCameraController.CameraHeadingOffset = DataInterface.viewPlane.scoutHeading;
        }


        private void cameraView_Tapped(object sender, GeoViewInputEventArgs e)
        {
            if (e.Location == null)
            {
                return;
            }
            MapPoint mapLocation = e.Location;
            double pitch = 0, yaw = 0, dir = 0;

            double lon = ((MapPoint)_planeGraphic.Geometry).X;
            double lat = ((MapPoint)_planeGraphic.Geometry).Y;
            double alt = ((MapPoint)_planeGraphic.Geometry).Z;
            double heading = DataInterface.viewPlane.heading + _orbitCameraController.CameraHeadingOffset;
            PosCalc.DoCalc(lon,lat,alt,heading,
                e.Location.X, e.Location.Y, e.Location.Z, 0, 0, 0, 0, ref pitch, ref yaw, ref dir);
            
            DataInterface.viewPlane.scoutPitchAim = pitch;
            DataInterface.viewPlane.scoutHeadingAim += yaw;
        }
    }
}
