using Esri.ArcGISRuntime.Geometry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;

namespace GroundLunch
{
    /// <summary>
    /// MissionMap.xaml 的交互逻辑
    /// </summary>
    public partial class MissionMap : UserControl
    {
        MyMapView _myMapView;
        public MissionFile missionFile = null;
        public List<WayPoint> groupAirLine = new List<WayPoint>();
        public int selectAirLinePtIndex = -1;

        // 新增：临时航点列表，用于存储新添加但未发送的航点
        public List<WayPoint> tempWayPoints = new List<WayPoint>();
        
        // 新增：选中航点类型标志
        public bool isSelectedTempWayPoint = false; // true表示选中的是临时航点，false表示选中的是任务航点
        
        // 新增：防止循环触发的标志
        private bool isUpdatingTextFromSelection = false;
        
        // 新增：获取当前选中的编队索引
        public int SelectedGroupIndex
        {
            get
            {
                if (comboSelectGroup != null && comboSelectGroup.SelectedIndex >= 0)
                {
                    return comboSelectGroup.SelectedIndex;
                }
                return 0; // 默认返回第一个编队
            }
        }
        public MissionMap()
        {
            InitializeComponent();
            _myMapView = new MyMapView(mapView);
            _myMapView.mainWindow = this;
        }

        public void UpdateSelMsnFile()
        {
            if (missionFile == null) 
            {
                return;
            }
            //更新最上方选择的任务文件名称
            labelMsnFileName.Text = missionFile.FileName;
            //更新当前选择的编队内航点列表
            comboSelectGroup.SelectedIndex = 0;
            LoadGroupInfoToListView(0);
            //更新当前选择编队的飞机数量
            LoadGroupInfoToTextedit(0);
            //更新地图内容
            _myMapView.missionFile = missionFile;
            _myMapView.LoadMissionFile();
        }
        
        public void InitGroupListView()
        {
            LoadGroupInfoToListView(0);
            LoadGroupInfoToTextedit(0);
        }

        public void TimerFresh()
        {
            _myMapView.TimerFresh();
        }

        public void LoadGroupInfoToListView(int groupIndex)
        {
            if (missionFile == null)
                return;
            
            // 合并任务航点和临时航点
            var allWayPoints = new List<WayPoint>();
            
            if (missionFile.flightGroups.Count > groupIndex)
            {
                allWayPoints.AddRange(missionFile.flightGroups[groupIndex].airLine);
            }
            
            // 添加临时航点
            allWayPoints.AddRange(tempWayPoints);
            
            // 创建显示项并更新DataGrid显示
            var displayItems = CreateWayPointDisplayItems(allWayPoints);
            SelectGroupAirline.DataContext = displayItems;
        }

        public void LoadGroupInfoToTextedit(int groupIndex)
        {
            if (missionFile == null)
                return;
            if (missionFile.flightGroups.Count <= groupIndex)
            {
                //未有对应的编队
                textAttackCount.Text = "0";
                textScoutCount.Text = "0";
                textGanraoCount.Text = "0";
            }
            else
            {
                textAttackCount.Text = missionFile.flightGroups[groupIndex].getPlaneCountByType(1).ToString();
                textScoutCount.Text = missionFile.flightGroups[groupIndex].getPlaneCountByType(2).ToString();
                textGanraoCount.Text = missionFile.flightGroups[groupIndex].getPlaneCountByType(3).ToString();
            }
        }

        // 新增：创建航点显示项，计算序号和线段距离
        private List<WayPointDisplayItem> CreateWayPointDisplayItems(List<WayPoint> wayPoints)
        {
            var displayItems = new List<WayPointDisplayItem>();
            
            if (wayPoints == null || wayPoints.Count == 0)
                return displayItems;

            for (int i = 0; i < wayPoints.Count; i++)
            {
                double segmentDistance = 0;
                
                if (i == 0)
                {
                    // 第一个点距离为0
                    segmentDistance = 0;
                }
                else
                {
                    // 计算从前一个点到当前点的直线距离
                    var prevPoint = wayPoints[i - 1];
                    var currentPoint = wayPoints[i];
                    
                    // 使用工程中现有的 haversine_distance 函数计算球面距离（直线距离）
                    segmentDistance = CommonCalc.haversine_distance(
                        prevPoint.lat, prevPoint.lon,
                        currentPoint.lat, currentPoint.lon
                    );
                }
                
                var displayItem = new WayPointDisplayItem(wayPoints[i], i, segmentDistance);
                displayItems.Add(displayItem);
            }
            
            return displayItems;
        }

        private void GroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (missionFile == null)
                return;
            // 获取当前选中的项
            ComboBox comboBox = sender as ComboBox;
            LoadGroupInfoToListView(comboBox.SelectedIndex);
            LoadGroupInfoToTextedit(comboBox.SelectedIndex);
        }

        // SelectGroupAirline DataGrid 选择变化事件处理器
        private void SelectGroupAirline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectGroupAirline.SelectedItem is WayPointDisplayItem selectedItem)
            {
                // 防止循环更新标志
                if (isUpdatingTextFromSelection)
                    return;

                try
                {
                    // 设置防循环标志
                    isUpdatingTextFromSelection = true;

                    // 计算实际的航点索引（考虑临时航点）
                    int actualIndex = CalculateActualWaypointIndex(selectedItem.Index);
                    
                    if (actualIndex >= 0)
                    {
                        // 更新选中状态
                        selectAirLinePtIndex = actualIndex;
                        isSelectedTempWayPoint = IsTempWaypointIndex(selectedItem.Index);
                        
                        // 同步到地图视图
                        _myMapView.selectedWaypointIndex = actualIndex;
                        _myMapView.selectedWaypointIsTemp = isSelectedTempWayPoint;
                        
                        // 调用选中处理函数，更新界面显示
                        OnSelectWayPoint();
                        
                        // 在地图上高亮选中的航点
                        HighlightSelectedWaypointOnMap(actualIndex, isSelectedTempWayPoint);
                    }
                }
                finally
                {
                    // 清除防循环标志
                    isUpdatingTextFromSelection = false;
                }
            }
        }

        // 计算实际的航点索引（考虑临时航点）
        private int CalculateActualWaypointIndex(int displayIndex)
        {
            if (missionFile == null || comboSelectGroup.SelectedIndex < 0)
                return -1;

            // 获取当前编队的航点数量
            int missionWaypointCount = 0;
            if (comboSelectGroup.SelectedIndex < missionFile.flightGroups.Count)
            {
                missionWaypointCount = missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine.Count;
            }

            // 如果显示索引小于任务航点数量，说明是任务航点
            if (displayIndex < missionWaypointCount)
            {
                return displayIndex;
            }
            // 否则是临时航点
            else
            {
                return displayIndex - missionWaypointCount;
            }
        }

        // 判断指定索引是否为临时航点
        private bool IsTempWaypointIndex(int displayIndex)
        {
            if (missionFile == null || comboSelectGroup.SelectedIndex < 0)
                return false;

            // 获取当前编队的航点数量
            int missionWaypointCount = 0;
            if (comboSelectGroup.SelectedIndex < missionFile.flightGroups.Count)
            {
                missionWaypointCount = missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine.Count;
            }

            return displayIndex >= missionWaypointCount;
        }

        // 在地图上高亮选中的航点
        private void HighlightSelectedWaypointOnMap(int waypointIndex, bool isTempWaypoint)
        {
            if (_myMapView == null)
                return;

            try
            {
                // 清除之前的选中状态
                _myMapView.selectedSquare.squareGraphic.Geometry = new MapPoint(0, 0);

                if (isTempWaypoint)
                {
                    // 选中临时航点
                    if (waypointIndex >= 0 && waypointIndex < tempWayPoints.Count)
                    {
                        var tempWp = tempWayPoints[waypointIndex];
                        var mapPoint = new MapPoint(tempWp.lon, tempWp.lat, SpatialReferences.Wgs84);
                        _myMapView.selectedSquare.squareGraphic.Geometry = mapPoint;
                    }
                }
                else
                {
                    // 选中任务航点
                    if (waypointIndex >= 0 && waypointIndex < _myMapView.allWaypoints.Count)
                    {
                        var waypointInfo = _myMapView.allWaypoints[waypointIndex];
                        _myMapView.selectedSquare.squareGraphic.Geometry = waypointInfo.Graphic.Geometry;
                    }
                }

                // 启用任务按钮和信息面板
                EnableMsnBtn(false);
                EnableInfo(true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"高亮航点失败: {ex.Message}");
            }
        }

        // 同步地图选中状态到界面
        public void SyncMapSelectionToUI()
        {
            if (_myMapView == null || _myMapView.selectedWaypointIndex < 0)
            {
                return;
            }

            try
            {
                // 计算在SelectGroupAirline中的显示索引
                int displayIndex = CalculateDisplayIndex(_myMapView.selectedWaypointIndex, _myMapView.selectedWaypointIsTemp);
               
                if (displayIndex >= 0 && SelectGroupAirline.Items.Count > displayIndex)
                {
                    // 防止循环更新
                    if (isUpdatingTextFromSelection)
                    {
                        return;
                    }

                    // 选中对应的行
                    SelectGroupAirline.SelectedIndex = displayIndex;
                    
                    // 确保DataGrid获得焦点以显示正确的选中颜色
                    SelectGroupAirline.Focus();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"SyncMapSelectionToUI: 显示索引无效或超出范围 - displayIndex={displayIndex}, Items.Count={SelectGroupAirline.Items.Count}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("同步地图选中状态到界面失败");
            }
        }

        // 计算在SelectGroupAirline中的显示索引
        private int CalculateDisplayIndex(int waypointIndex, bool isTempWaypoint)
        {
            if (missionFile == null || comboSelectGroup.SelectedIndex < 0)
                return -1;

            if (isTempWaypoint)
            {
                // 临时航点的显示索引 = 任务航点数量 + 临时航点索引
                int missionWaypointCount = 0;
                if (comboSelectGroup.SelectedIndex < missionFile.flightGroups.Count)
                {
                    missionWaypointCount = missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine.Count;
                }
                return missionWaypointCount + waypointIndex;
            }
            else
            {
                // 任务航点的显示索引就是其索引
                return waypointIndex;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitGroupListView();
        }

        // 经度文本框失去焦点事件
        private void TextLon_LostFocus(object sender, RoutedEventArgs e)
        {
            // 移除自动触发，只保留预览功能
            UpdateWaypointPreview();
        }

        // 纬度文本框失去焦点事件
        private void TextLat_LostFocus(object sender, RoutedEventArgs e)
        {
            // 移除自动触发，只保留预览功能
            UpdateWaypointPreview();
        }

        // 经度文本框文本改变事件
        private void TextLon_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 防止循环触发：如果正在从选中航点更新文本框，则不处理
            if (isUpdatingTextFromSelection)
                return;
                
            // 只有在创建新航点模式时才更新预览，避免循环触发
            if (_myMapView != null && 
                (_myMapView.msnMode == MSN_MODE.MSN_WAYPOINT || 
                 _myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_SELECTED ||
                 _myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_DIRCONFIRMED ||
                 _myMapView.msnMode == MSN_MODE.MSN_ATTACK ||
                 _myMapView.msnMode == MSN_MODE.MSN_ATTACK_SELECTED ||
                 _myMapView.msnMode == MSN_MODE.MSN_HOVER ||
                 _myMapView.msnMode == MSN_MODE.MSN_HOVER_SELECTED ||
                 _myMapView.msnMode == MSN_MODE.MSN_HOVER_RADIUS_CONFIRMED ||
                 _myMapView.msnMode == MSN_MODE.MSN_RECYCLE ||
                 _myMapView.msnMode == MSN_MODE.MSN_RECYCLE_SELECTED))
            {
                UpdateWaypointPreview();
            }
        }

        // 纬度文本框文本改变事件
        private void TextLat_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 防止循环触发：如果正在从选中航点更新文本框，则不处理
            if (isUpdatingTextFromSelection)
                return;
                
            // 只有在创建新航点模式时才更新预览，避免循环触发
            if (_myMapView != null && 
                (_myMapView.msnMode == MSN_MODE.MSN_WAYPOINT || 
                 _myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_SELECTED ||
                 _myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_DIRCONFIRMED ||
                 _myMapView.msnMode == MSN_MODE.MSN_ATTACK ||
                 _myMapView.msnMode == MSN_MODE.MSN_ATTACK_SELECTED ||
                 _myMapView.msnMode == MSN_MODE.MSN_HOVER ||
                 _myMapView.msnMode == MSN_MODE.MSN_HOVER_SELECTED ||
                 _myMapView.msnMode == MSN_MODE.MSN_HOVER_RADIUS_CONFIRMED ||
                 _myMapView.msnMode == MSN_MODE.MSN_RECYCLE ||
                 _myMapView.msnMode == MSN_MODE.MSN_RECYCLE_SELECTED))
            {
                UpdateWaypointPreview();
            }
        }

        // 更新航点预览位置
        private void UpdateWaypointPreview()
        {
            try
            {
                // 检查_myMapView是否已初始化
                if (_myMapView == null)
                    return;

                // 检查TextBox是否已初始化
                if (textLon == null || textLat == null)
                    return;

                // 检查是否在航点创建模式
                if (_myMapView.msnMode == MSN_MODE.MSN_WAYPOINT || 
                    _myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_SELECTED ||
                    _myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_DIRCONFIRMED ||
                    _myMapView.msnMode == MSN_MODE.MSN_ATTACK ||
                    _myMapView.msnMode == MSN_MODE.MSN_ATTACK_SELECTED ||
                    _myMapView.msnMode == MSN_MODE.MSN_HOVER ||
                    _myMapView.msnMode == MSN_MODE.MSN_HOVER_SELECTED ||
                    _myMapView.msnMode == MSN_MODE.MSN_HOVER_RADIUS_CONFIRMED ||
                    _myMapView.msnMode == MSN_MODE.MSN_RECYCLE ||
                    _myMapView.msnMode == MSN_MODE.MSN_RECYCLE_SELECTED)
                {
                    double lon, lat;
                    if (double.TryParse(textLon.Text, out lon) && double.TryParse(textLat.Text, out lat))
                    {
                        // 更新地图上的预览航点位置
                        _myMapView.UpdatePreviewWaypointPosition(lon, lat);
                    }
                }
            }
            catch (Exception ex)
            {
                // 静默处理错误，避免干扰用户输入
            }
        }

        // 新增：确认修改航点（通过按钮触发）
        private void ConfirmWaypointChange_Click(object sender, RoutedEventArgs e)
        {
            // 检查是否有选中的航点需要更新
            if (selectAirLinePtIndex != -1)
            {
                // 更新选中的航点
                bool updateSuccess = UpdateSelectedWaypointPosition();
                
                // 无论修改成功与否，都清除选中状态
                ClearSelection();
            }
            else
            {
                // 没有选中航点时，只更新预览
                UpdateWaypointPreview();
            }
        }

        private void NewWayPoint_Click(object sender, RoutedEventArgs e)
        {
            if (_myMapView == null) return;
            
            _myMapView.msnMode = MSN_MODE.MSN_WAYPOINT;
            _myMapView.CreateWayPointSetting();
            EnableInfo(true);
            EnableMsnBtn(false);
            cmdConfirm.Text = "信息确认(航点飞行)";
            
            // 设置完模式后，立即更新预览
            UpdateWaypointPreview();
        }
        private void NewHover_Click(object sender, RoutedEventArgs e)
        {
            if (_myMapView == null) return;
            
            _myMapView.msnMode = MSN_MODE.MSN_HOVER;
            _myMapView.CreateHoverSetting();
            EnableInfo(true);
            EnableMsnBtn(false);
            cmdConfirm.Text = "信息确认(盘旋飞行)";
            
            // 设置完模式后，立即更新预览
            UpdateWaypointPreview();
        }

        private void NewAttack_Click(object sender, RoutedEventArgs e)
        {
            if (_myMapView == null) return;
            
            _myMapView.msnMode = MSN_MODE.MSN_ATTACK;
            _myMapView.CreateTargetSetting();
            EnableInfo(true);
            EnableMsnBtn(false);
            cmdConfirm.Text = "信息确认(攻击目标)";
            textAlt.Text = "5";
            
            // 设置完模式后，立即更新预览
            UpdateWaypointPreview();
        }

        private void CancelAttack_Click(object sender, RoutedEventArgs e)
        {
            if (_myMapView == null) return;
            
            _myMapView.msnMode = MSN_MODE.MSN_CANCEL_ATTACK;
            EnableInfo(true);
            EnableMsnBtn(false);
            cmdConfirm.Text = "信息确认(退出攻击)";
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            if (_myMapView == null) return;
            
            _myMapView.msnMode = MSN_MODE.MSN_HOME;
            EnableInfo(true);
            EnableMsnBtn(false);
            cmdConfirm.Text = "信息确认(返航)";
        }

        private void Recycle_Click(object sender, RoutedEventArgs e)
        {
            if (_myMapView == null) return;
            
            _myMapView.msnMode = MSN_MODE.MSN_RECYCLE;
            _myMapView.CreateRecycleSetting();
            EnableInfo(true);
            EnableMsnBtn(false);
            cmdConfirm.Text = "信息确认(开伞回收)";
            textAlt.Text = "5";
            
            // 设置完模式后，立即更新预览
            UpdateWaypointPreview();
        }

        private void DelMsn_Click(object sender, RoutedEventArgs e)
        {
            if (_myMapView == null) return;
            
            if (_myMapView.curSelectedWayPoint != null)
            {
                if (selectAirLinePtIndex != -1)
                {
                    if (isSelectedTempWayPoint)
                    {
                        // 删除临时航点
                        if (selectAirLinePtIndex >= 0 && selectAirLinePtIndex < tempWayPoints.Count)
                        {
                            tempWayPoints.RemoveAt(selectAirLinePtIndex);
                            UpdateTempWayPointsDisplay();
                            MessageBox.Show("临时航点已删除");
                        }
                    }
                    else
                    {
                        // 删除任务文件中的航点
                        if (missionFile != null && 
                            comboSelectGroup.SelectedIndex >= 0 && 
                            comboSelectGroup.SelectedIndex < missionFile.flightGroups.Count &&
                            selectAirLinePtIndex >= 0 && 
                            selectAirLinePtIndex < missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine.Count)
                        {
                            // 删除航点
                            missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine.RemoveAt(selectAirLinePtIndex);
                            
                            // 清除选中状态（在重新加载之前）
                            ClearSelection();
                            
                            // 重新加载当前编队的航点到地图
                            _myMapView.LoadPointToMapForGroup(comboSelectGroup.SelectedIndex);
                            
                            // 更新当前编队的航线
                            _myMapView.UpdateCurveForGroup(comboSelectGroup.SelectedIndex);
                            
                            // 更新ListView显示
                            LoadGroupInfoToListView(comboSelectGroup.SelectedIndex);
                            
                            MessageEvents.SendMsnUpdateMessage("msn Update");                                                     
                        }
                    }
                    
                    _myMapView.selectedSquare.squareGraphic.Geometry = new MapPoint(0, 0);
                    EnableMsnBtn(true);
                    selectAirLinePtIndex = -1;
                    isSelectedTempWayPoint = false;
                    // 使用ClearSelection()方法清除选中状态
                    ClearSelection();
                }
            }
        }
        private void UpdateMsn_Click(object sender, RoutedEventArgs e)
        {
            //DataInterface.startFly

            if (DataInterface.startFly)
            {
                // 起飞状态：先根据遥测数据更新航点列表，然后发送临时航点数据
                UpdateWaypointsFromTelemetry();
                SendWayPointsData();
            }
            else
            {
                // 未起飞状态：提示用户
                MessageBox.Show("当前未处于飞行状态，无需发送航点数据");
            }
        }

        // 新增：手动触发航点更新（用于测试或手动调用）
        public void TriggerWaypointUpdate()
        {
            UpdateWaypointsFromTelemetry();
        }

        // 新增：检查航点是否在安全区内
        public bool IsPointInSafeArea(double lon, double lat)
        {
            if (missionFile == null || missionFile.safeArea == null || missionFile.safeArea.Count < 3)
            {
                return true; // 如果没有安全区定义，允许任意位置
            }
            if (missionFile.safeArea.Count == 4)
            {
                return IsPointInRectangle(lon, lat);
            }

            // 使用射线法判断点是否在多边形内部
            int intersections = 0;
            int n = missionFile.safeArea.Count;
            
            for (int i = 0; i < n; i++)
            {
                double x1 = missionFile.safeArea[i].lon;
                double y1 = missionFile.safeArea[i].lat;
                double x2 = missionFile.safeArea[(i + 1) % n].lon;
                double y2 = missionFile.safeArea[(i + 1) % n].lat;
                
                // 射线法实现：从测试点向右发射水平射线
                // 检查射线是否与边相交
                if (y1 != y2) // 避免水平边
                {
                    // 检查点是否在边的y范围内（使用严格不等式）
                    if ((lat > y1 && lat < y2) || (lat > y2 && lat < y1))
                    {
                        // 计算射线与边的交点的x坐标
                        double intersectX = x1 + (x2 - x1) * (lat - y1) / (y2 - y1);
                        
                        // 如果交点在测试点的右侧，则计数
                        if (lon < intersectX)
                        {
                            intersections++;
                            //System.Diagnostics.Debug.WriteLine($"  与边 {i} 相交于 x={intersectX:F6}");
                        }
                    }
                }
            }
            
            bool isInside = (intersections % 2) == 1;
            //System.Diagnostics.Debug.WriteLine($"交点数量: {intersections}, 是否在内部: {isInside}");
            
            return isInside;
        }

        // 新增：矩形安全区检测
        private bool IsPointInRectangle(double lon, double lat)
        {
            if (missionFile.safeArea.Count != 4)
                return false;

            // 找到矩形的边界
            double minLon = double.MaxValue, maxLon = double.MinValue;
            double minLat = double.MaxValue, maxLat = double.MinValue;

            foreach (var point in missionFile.safeArea)
            {
                minLon = Math.Min(minLon, point.lon);
                maxLon = Math.Max(maxLon, point.lon);
                minLat = Math.Min(minLat, point.lat);
                maxLat = Math.Max(maxLat, point.lat);
            }

            bool isInside = (lon >= minLon && lon <= maxLon && lat >= minLat && lat <= maxLat);
            
            //System.Diagnostics.Debug.WriteLine($"矩形边界: 经度[{minLon:F6}, {maxLon:F6}], 纬度[{minLat:F6}, {maxLat:F6}]");
            //System.Diagnostics.Debug.WriteLine($"测试点: ({lon:F6}, {lat:F6}), 是否在矩形内: {isInside}");
            
            return isInside;
        }

        // 新增：更新选中航点的位置
        public bool UpdateSelectedWaypointPosition()
        {
            // 使用地图视图中的选中索引，确保一致性
            if (_myMapView.selectedWaypointIndex == -1)
            {
                System.Diagnostics.Debug.WriteLine("UpdateSelectedWaypointPosition: 没有选中的航点");
                return false;
            }

            try
            {
                double newLon = Convert.ToDouble(textLon.Text);
                double newLat = Convert.ToDouble(textLat.Text);
                double newAlt = Convert.ToDouble(textAlt.Text);
                double newDir = Convert.ToDouble(textInTrack.Text);
                double newRadius = Convert.ToDouble(textRadius.Text);
                double newSpeed = Convert.ToDouble(textSpeed.Text);

                // 检查新位置是否在安全区内
                bool isInSafeArea = IsPointInSafeArea(newLon, newLat);
                if (!isInSafeArea)
                {
                    MessageBox.Show( "安全区警告");
                    return false; // 修改失败
                }

                if (_myMapView.selectedWaypointIsTemp)
                {
                    // 更新临时航点
                    if (_myMapView.selectedWaypointIndex >= 0 && _myMapView.selectedWaypointIndex < tempWayPoints.Count)
                    {
                        var wp = tempWayPoints[_myMapView.selectedWaypointIndex];
                        wp.lon = newLon;
                        wp.lat = newLat;
                        wp.alt = newAlt;
                        wp.dir = newDir;
                        wp.radis = newRadius;
                        wp.speed = newSpeed;
                        
                        // 更新显示 - 临时航点不需要更新任务规划
                        UpdateTempWayPointsDisplay();                                            
                        MessageBox.Show("临时航点位置已更新");
                        return true; // 修改成功
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine($"临时航点索引越界: selectedWaypointIndex={_myMapView.selectedWaypointIndex}, tempWayPoints.Count={tempWayPoints.Count}");
                        return false;
                    }
                }
                else
                {
                        // 使用新的统一航点系统
                        if (_myMapView.selectedWaypointIndex >= 0 && _myMapView.selectedWaypointIndex < _myMapView.allWaypoints.Count)
                        {
                            var selectedWaypoint = _myMapView.allWaypoints[_myMapView.selectedWaypointIndex];
                            //System.Diagnostics.Debug.WriteLine($"准备更新航点: 索引{_myMapView.selectedWaypointIndex}, 类型{selectedWaypoint.WpType}");

                            // 保存旧坐标用于匹配
                            double oldLon = selectedWaypoint.Lon;
                            double oldLat = selectedWaypoint.Lat;
                            
                            // 更新航点属性
                            selectedWaypoint.Lon = newLon;
                            selectedWaypoint.Lat = newLat;
                            selectedWaypoint.Alt = newAlt;
                        
                        // 根据航点类型更新特定属性
                        if (selectedWaypoint is WaypointInfo wp)
                        {
                            wp.inTrack = newDir;
                            wp.speed = newSpeed;
                        }
                        else if (selectedWaypoint is HoverInfo hover)
                        {
                            hover.inTrack = newRadius; // 盘旋点用 inTrack 存储半径
                        }
                        
                        // 更新图形显示
                        selectedWaypoint.UpdateGraphic();
                        selectedWaypoint.UpdateLabel();
                        
                        // 强制刷新地图显示
                        if (_myMapView.Control != null)
                        {
                            _myMapView.Control.InvalidateVisual();
                        }
                        
                        // 更新任务文件中的对应航点
                        if (missionFile != null && 
                            comboSelectGroup.SelectedIndex >= 0 && 
                            comboSelectGroup.SelectedIndex < missionFile.flightGroups.Count)
                        {
                            var airLine = missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine;
                            
                            // 使用航点索引直接更新 airLine 中的航点
                            if (_myMapView.selectedWaypointIndex >= 0 && _myMapView.selectedWaypointIndex < _myMapView.allWaypoints.Count)
                            {
                                // 获取选中的航点信息
                                var selectedWaypointInfo = _myMapView.allWaypoints[_myMapView.selectedWaypointIndex];
                                
                                // 通过旧坐标和类型匹配找到对应的 airLine 航点
                                bool found = false;
                                for (int i = 0; i < airLine.Count; i++)
                                {
                                    var airLineWp = airLine[i];
                                    if (Math.Abs(airLineWp.lon - oldLon) < 0.0001 &&
                                        Math.Abs(airLineWp.lat - oldLat) < 0.0001 &&
                                        airLineWp.wpType == selectedWaypointInfo.WpType)
                                    {
                                        // 更新 airLine 中的航点
                                        airLineWp.lon = newLon;
                                        airLineWp.lat = newLat;
                                        airLineWp.alt = newAlt;
                                        
                                        if (selectedWaypoint is WaypointInfo)
                                        {
                                            airLineWp.dir = newDir;
                                            airLineWp.speed = newSpeed;
                                        }
                                        else if (selectedWaypoint is HoverInfo)
                                        {
                                            airLineWp.radis = newRadius;
                                        }
                                        
                                        //System.Diagnostics.Debug.WriteLine($"更新airLine航点: airLine[{i}] 类型{airLineWp.wpType}, 新坐标({newLon:F8}, {newLat:F8})");
                                        found = true;
                                        break;
                                    }
                                }
                                
                                if (!found)
                                {
                                    //System.Diagnostics.Debug.WriteLine($"未找到对应的airLine航点: selectedWaypointIndex={_myMapView.selectedWaypointIndex}, 旧坐标({oldLon:F6}, {oldLat:F6}), 类型{selectedWaypointInfo.WpType}");
                                }
                            }
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine($"航点索引越界: selectedWaypointIndex={_myMapView.selectedWaypointIndex}, allWaypoints.Count={_myMapView.allWaypoints.Count}");
                            }
                            
                            // 验证airLine是否已更新
                            //System.Diagnostics.Debug.WriteLine($"验证airLine更新 - 编队{comboSelectGroup.SelectedIndex}的航点坐标:");
                            for (int i = 0; i < airLine.Count; i++)
                            {
                                var airLineWp = airLine[i];
                            }
                            
                            // 验证missionFile是否已更新
                            //System.Diagnostics.Debug.WriteLine($"验证missionFile更新 - 编队{comboSelectGroup.SelectedIndex}的航点坐标:");
                            for (int i = 0; i < missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine.Count; i++)
                            {
                                var missionFileWp = missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine[i];
                                //System.Diagnostics.Debug.WriteLine($"  missionFile.airLine[{i}]: wpType={missionFileWp.wpType}, 坐标({missionFileWp.lon:F6}, {missionFileWp.lat:F6})");
                            }
                            
                            // 验证引用是否相同
                            //System.Diagnostics.Debug.WriteLine($"引用验证 - airLine == missionFile.airLine: {airLine == missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine}");
                            
                            // 更新显示 - 只更新必要的部分，避免重新加载整个任务文件
                            LoadGroupInfoToListView(comboSelectGroup.SelectedIndex);
                            
                            // 重新加载当前编队的航点到地图，确保航点图标与airLine同步
                            _myMapView.LoadPointToMapForGroup(comboSelectGroup.SelectedIndex);
                            
                            // 更新当前编队的航线
                            _myMapView.UpdateCurveForGroup(comboSelectGroup.SelectedIndex);
                            
                            MessageEvents.SendMsnUpdateMessage("msn Update");
                            
                            //System.Diagnostics.Debug.WriteLine($"更新航点成功: 索引{_myMapView.selectedWaypointIndex}, 类型{selectedWaypoint.WpType}, 新坐标({newLon:F8}, {newLat:F8})");
                            MessageBox.Show("航点位置已更新");
                            return true; // 修改成功
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine($"航点索引越界: selectedWaypointIndex={_myMapView.selectedWaypointIndex}, allWaypoints.Count={_myMapView.allWaypoints.Count}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine($"UpdateSelectedWaypointPosition异常: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"更新航点位置失败: {ex.Message}");
                return false; // 修改失败
            }
        }
        // 新增：清除选中状态
        private void ClearSelection()
        {
            selectAirLinePtIndex = -1;
            isSelectedTempWayPoint = false;
            _myMapView.selectedSquare.squareGraphic.Geometry = new MapPoint(0, 0);
            
            // 清除地图视图中的选中索引
            _myMapView.selectedWaypointIndex = -1;
            _myMapView.selectedWaypointIsTemp = false;
            _myMapView.curSelectedWayPoint = null;
            
            EnableMsnBtn(true);
            EnableInfo(false);
        }

        
        private void CancelMsn_Click(object sender, RoutedEventArgs e)
        {
            // 使用ClearSelection()方法清除选中状态
            ClearSelection();
            if (_myMapView.msnMode == MSN_MODE.MSN_WAYPOINT ||
                _myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_SELECTED ||
                _myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_DIRCONFIRMED)
            {
                _myMapView.DestoryWayPointSetting(-1);
            }
            else if (_myMapView.msnMode == MSN_MODE.MSN_ATTACK ||  // 添加这一行
                _myMapView.msnMode == MSN_MODE.MSN_CANCEL_ATTACK ||
                _myMapView.msnMode == MSN_MODE.MSN_ATTACK_SELECTED)
            {
                _myMapView.DestoryTargetSetting();
            }
            else if (_myMapView.msnMode == MSN_MODE.MSN_HOVER ||
                _myMapView.msnMode == MSN_MODE.MSN_HOVER_SELECTED ||
                _myMapView.msnMode == MSN_MODE.MSN_HOVER_RADIUS_CONFIRMED)
            {
                _myMapView.DestoryHoverSetting(-1);
            }
            _myMapView.msnMode = MSN_MODE.MSN_NULL;
            cmdConfirm.Text = "信息确认";
        }

        private void SubmitMsn_Click(object sender, RoutedEventArgs e)
        {
            // 检查是否有选中的航点需要更新
            if (selectAirLinePtIndex != -1)
            {
                // 更新选中的航点
                bool updateSuccess = UpdateSelectedWaypointPosition();
                
                // 无论修改成功与否，都清除选中状态
                ClearSelection();
                
                if (!updateSuccess)
                {
                    MessageBox.Show("航点修改失败，已清除选中状态");
                    return;
                }
            }
            else if (_myMapView.msnMode == MSN_MODE.MSN_WAYPOINT_DIRCONFIRMED ||
                    _myMapView.msnMode == MSN_MODE.MSN_ATTACK_SELECTED ||
                    _myMapView.msnMode == MSN_MODE.MSN_RECYCLE_SELECTED ||
                    _myMapView.msnMode == MSN_MODE.MSN_HOVER_RADIUS_CONFIRMED)
            {
                WayPoint newWp = new WayPoint();
                switch (_myMapView.msnMode)
                {
                    case MSN_MODE.MSN_WAYPOINT_DIRCONFIRMED:
                        newWp.wpType = 1;
                        break;
                    case MSN_MODE.MSN_ATTACK_SELECTED:
                        newWp.wpType = 3;
                        break;
                    case MSN_MODE.MSN_RECYCLE_SELECTED:
                        newWp.wpType = 5;
                        break;
                    case MSN_MODE.MSN_HOVER_RADIUS_CONFIRMED:
                        newWp.wpType = 2;
                        break;
                }
                
                double newLon = Convert.ToDouble(textLon.Text);
                double newLat = Convert.ToDouble(textLat.Text);
                double newAlt = Convert.ToDouble(textAlt.Text);
                double newDir = Convert.ToDouble(textInTrack.Text);
                double newSpeed = Convert.ToDouble(textSpeed.Text);
                double newRadius = Convert.ToDouble(textRadius.Text);
                
                // 检查新航点位置是否在安全区内
                bool isInSafeArea = IsPointInSafeArea(newLon, newLat);
                if (!isInSafeArea)
                {
                    // 添加更详细的调试信息
                    string debugInfo = $"新航点位置超出安全区范围！\n\n";
                    debugInfo += $"输入坐标: 经度={newLon:F6}, 纬度={newLat:F6}\n\n";
                    debugInfo += $"安全区边界:\n";
                    for (int i = 0; i < missionFile.safeArea.Count; i++)
                    {
                        debugInfo += $"  点{i+1}: 经度={missionFile.safeArea[i].lon:F6}, 纬度={missionFile.safeArea[i].lat:F6}\n";
                    }
                    debugInfo += $"\n请检查坐标是否正确，或联系管理员检查安全区设置。";
                    
                    MessageBox.Show(debugInfo, "安全区警告", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                newWp.lon = newLon;
                newWp.lat = newLat;
                newWp.alt = newAlt;
                newWp.dir = newDir;
                newWp.speed = newSpeed;
                newWp.radis = newRadius;
                //DataInterface.startFly
                if (DataInterface.startFly)
                {
                    // 起飞状态：不发送数据
                    tempWayPoints.Add(newWp);
                    UpdateTempWayPointsDisplay();
                    MessageBox.Show($"航点已添加到临时列表，共 {tempWayPoints.Count} 个航点待发送");
                }
                else
                {
                    // 未起飞状态：直接添加到任务文件
                    missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine.Add(newWp);
                    
                    // 重新加载当前编队的航点到地图
                    _myMapView.LoadPointToMapForGroup(comboSelectGroup.SelectedIndex);
                    
                    // 更新当前编队的航线
                    _myMapView.UpdateCurveForGroup(comboSelectGroup.SelectedIndex);
                    
                    // 更新ListView显示
                    LoadGroupInfoToListView(comboSelectGroup.SelectedIndex);
                    
                    MessageEvents.SendMsnUpdateMessage("msn Update");
                    
                    //System.Diagnostics.Debug.WriteLine($"添加航点成功: 类型{newWp.wpType}, 坐标({newWp.lon:F6}, {newWp.lat:F6}), 总航点数{missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine.Count}");
                }
            }
            
            _myMapView.msnMode = MSN_MODE.MSN_NULL;
            EnableInfo(false);
            EnableMsnBtn(true);
            cmdConfirm.Text = "信息确认";
            _myMapView.LoadCurveToMap();
            
            // 清除选中状态
            selectAirLinePtIndex = -1;
            isSelectedTempWayPoint = false;
        }

        public void EnableInfo(bool enable)
        {
            textLon.IsEnabled = enable;
            textLat.IsEnabled = enable;
            textAlt.IsEnabled = enable;
            textInTrack.IsEnabled = enable;
            textRadius.IsEnabled = enable;
            textSpeed.IsEnabled = enable;   
            btnSubmit.IsEnabled = enable;
            btnCancel.IsEnabled = enable;
            btnConfirmChange.IsEnabled = enable; // 新增：确认修改按钮
        }

        public void EnableMsnBtn(bool enable)
        {
            btnWaypoint.IsEnabled = enable;
            btnHover.IsEnabled = enable;
            btnAttack.IsEnabled = enable;
            btnCancelAttack.IsEnabled = enable;
            btnGoHome.IsEnabled = enable;
            btnRecycle.IsEnabled = enable;
        }

        // 新增：设置文本值的方法，供MyMapView调用
        public void SetTextValues(string lon, string lat, string inTrack, string radius, string speed)
        {
            textLon.Text = lon;
            textLat.Text = lat;
            textInTrack.Text = inTrack;
            textRadius.Text = radius;
            textSpeed.Text = speed;
        }

        // 新增：设置单个文本值的方法
        public void SetLonLat(string lon, string lat)
        {
            textLon.Text = lon;
            textLat.Text = lat;
        }

        public void SetInTrack(string inTrack)
        {
            textInTrack.Text = inTrack;
        }

        public void SetRadius(string radius)
        {
            textRadius.Text = radius;
        }

        // 新增：根据航点类型启用不同的界面元素
        private void EnableFlightWaypointInfo(bool enable)
        {
            textLon.IsEnabled = enable;
            textLat.IsEnabled = enable;
            textAlt.IsEnabled = enable;
            textInTrack.IsEnabled = enable;
            textRadius.IsEnabled = false; // 飞行航点不需要半径
            textSpeed.IsEnabled = enable;
        }

        private void EnableHoverWaypointInfo(bool enable)
        {
            textLon.IsEnabled = enable;
            textLat.IsEnabled = enable;
            textAlt.IsEnabled = enable;
            textInTrack.IsEnabled = false; // 盘旋点不需要方向
            textRadius.IsEnabled = enable; // 盘旋点需要半径
            textSpeed.IsEnabled = false;
        }

        private void EnableTargetWaypointInfo(bool enable)
        {
            textLon.IsEnabled = enable;
            textLat.IsEnabled = enable;
            textAlt.IsEnabled = enable;
            textInTrack.IsEnabled = false; // 攻击目标不需要方向
            textRadius.IsEnabled = false; // 攻击目标不需要半径
            textSpeed.IsEnabled = false;
        }

        private void EnableRecycleWaypointInfo(bool enable)
        {
            textLon.IsEnabled = enable;
            textLat.IsEnabled = enable;
            textAlt.IsEnabled = enable;
            textInTrack.IsEnabled = false; // 伞降点不需要方向
            textRadius.IsEnabled = false; // 伞降点不需要半径
            textSpeed.IsEnabled = false;
        }

        // 新增：设置临时航点信息
        private void SetTempWaypointInfo(WayPoint wp)
        {
            textLon.Text = wp.lon.ToString("F8");
            textLat.Text = wp.lat.ToString("F8");
            textAlt.Text = wp.alt.ToString("F1");
            textInTrack.Text = wp.dir.ToString("F2");
            textRadius.Text = wp.radis.ToString("F1");
            textSpeed.Text = wp.speed.ToString("F1");
            
            // 根据临时航点类型启用相应界面
            switch (wp.wpType)
            {
                case 1: // 飞行航点
                    EnableFlightWaypointInfo(true);
                    break;
                case 2: // 盘旋点
                    EnableHoverWaypointInfo(true);
                    break;
                case 3: // 攻击目标
                    EnableTargetWaypointInfo(true);
                    break;
                case 5: // 伞降点
                    EnableRecycleWaypointInfo(true);
                    break;
            }
        }

        public void OnSelectWayPoint()
        {
            if (_myMapView.curSelectedWayPoint != null)
            {
                // 设置防循环标志
                isUpdatingTextFromSelection = true;
                
                try
                {
                    // 使用索引方式查找航点，避免坐标精度问题
                    if (_myMapView.selectedWaypointIndex >= 0)
                    {
                        if (!_myMapView.selectedWaypointIsTemp)
                        {
                            // 选中的是任务航点（支持所有类型）
                            System.Diagnostics.Debug.WriteLine($"选中任务航点: selectedWaypointIndex={_myMapView.selectedWaypointIndex}");
                            
                            // 获取航点信息
                            var waypointInfo = _myMapView.allWaypoints[_myMapView.selectedWaypointIndex];
                            
                            selectAirLinePtIndex = _myMapView.selectedWaypointIndex;
                            isSelectedTempWayPoint = false;
                            
                            // 根据航点类型设置不同的界面状态
                            switch (waypointInfo.WpType)
                            {
                                case 1: // 飞行航点
                                    EnableFlightWaypointInfo(true);
                                    textLon.Text = waypointInfo.Lon.ToString("F8");
                                    textLat.Text = waypointInfo.Lat.ToString("F8");
                                    textAlt.Text = waypointInfo.Alt.ToString("F1");
                                    textInTrack.Text = ((WaypointInfo)waypointInfo).inTrack.ToString("F2");
                                    textRadius.Text = "0";
                                    textSpeed.Text = "190";
                                    break;
                                    
                                case 2: // 盘旋点
                                    EnableHoverWaypointInfo(true);
                                    textLon.Text = waypointInfo.Lon.ToString("F8");
                                    textLat.Text = waypointInfo.Lat.ToString("F8");
                                    textAlt.Text = waypointInfo.Alt.ToString("F1");
                                    textRadius.Text = ((HoverInfo)waypointInfo).inTrack.ToString("F1");
                                    textInTrack.Text = "0";
                                    textSpeed.Text = "0";
                                    break;
                                    
                                case 3: // 攻击目标
                                    EnableTargetWaypointInfo(true);
                                    textLon.Text = waypointInfo.Lon.ToString("F8");
                                    textLat.Text = waypointInfo.Lat.ToString("F8");
                                    textAlt.Text = waypointInfo.Alt.ToString("F1");
                                    textInTrack.Text = "0";
                                    textRadius.Text = "0";
                                    textSpeed.Text = "0";
                                    break;
                                    
                                case 5: // 伞降点
                                    EnableRecycleWaypointInfo(true);
                                    textLon.Text = waypointInfo.Lon.ToString("F8");
                                    textLat.Text = waypointInfo.Lat.ToString("F8");
                                    textAlt.Text = waypointInfo.Alt.ToString("F1");
                                    textInTrack.Text = "0";
                                    textRadius.Text = "0";
                                    textSpeed.Text = "0";
                                    break;
                            }
                            
                            //System.Diagnostics.Debug.WriteLine($"选中航点: 索引{_myMapView.selectedWaypointIndex}, 类型{waypointInfo.WpType}, 坐标({waypointInfo.Lon:F8}, {waypointInfo.Lat:F8})");
                        }
                        else
                        {
                            // 选中的是临时航点（保持原有逻辑）
                            if (_myMapView.selectedWaypointIndex < tempWayPoints.Count)
                            {
                                var wp = tempWayPoints[_myMapView.selectedWaypointIndex];
                                
                                selectAirLinePtIndex = _myMapView.selectedWaypointIndex;
                                isSelectedTempWayPoint = true;
                                
                                // 根据临时航点类型设置界面
                                SetTempWaypointInfo(wp);
                                
                                //System.Diagnostics.Debug.WriteLine($"选中临时航点: 索引{_myMapView.selectedWaypointIndex}, 类型{wp.wpType}, 坐标({wp.lon:F8}, {wp.lat:F8})");
                            }
                        }
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("未找到有效的航点索引");
                    }
                }
                finally
                {
                    // 清除防循环标志
                    isUpdatingTextFromSelection = false;
                    
                    // 同步地图选中状态到SelectGroupAirline
                    SyncMapSelectionToUI();
                }
            }
        }

        // 新增：更新临时航点显示
        public void UpdateTempWayPointsDisplay()
        {
            // 合并原有航点和临时航点
            var allWayPoints = new List<WayPoint>();
            if (missionFile != null && missionFile.flightGroups.Count > comboSelectGroup.SelectedIndex)
            {
                allWayPoints.AddRange(missionFile.flightGroups[comboSelectGroup.SelectedIndex].airLine);
            }
            allWayPoints.AddRange(tempWayPoints);
            
            // 创建显示项并更新ListView显示
            var displayItems = CreateWayPointDisplayItems(allWayPoints);
            SelectGroupAirline.DataContext = displayItems;
            
            // 更新地图显示 - 直接在地图上绘制临时航点，不进行航迹规划
            _myMapView.LoadMissionFile();
            _myMapView.LoadTempWayPointsToMap(tempWayPoints);
        }

        // 新增：发送可变长度的航点数据
        public void SendWayPointsData()
        {
            if (tempWayPoints.Count == 0)
            {
                return;
            }

            try
            {
                // 计算数据长度：每个航点49字节 + 航点数量(4字节)
                int dataLength = 1 + (tempWayPoints.Count * 49);
                byte[] wayPointData = new byte[dataLength];
                
                // 写入航点数量
                Buffer.BlockCopy(BitConverter.GetBytes(tempWayPoints.Count), 0, wayPointData, 0, 1);
                
                // 写入每个航点数据
                int offset = 1;
                foreach (var wp in tempWayPoints)
                {
                    wayPointData[offset] = (byte)wp.wpType;
                    Buffer.BlockCopy(BitConverter.GetBytes(wp.lon), 0, wayPointData, offset + 1, 8);
                    Buffer.BlockCopy(BitConverter.GetBytes(wp.lat), 0, wayPointData, offset + 9, 8);
                    Buffer.BlockCopy(BitConverter.GetBytes(wp.alt), 0, wayPointData, offset + 17, 8);
                    Buffer.BlockCopy(BitConverter.GetBytes(wp.dir), 0, wayPointData, offset + 25, 8);
                    Buffer.BlockCopy(BitConverter.GetBytes(wp.speed), 0, wayPointData, offset + 33, 8);
                    Buffer.BlockCopy(BitConverter.GetBytes(wp.radis), 0, wayPointData, offset + 41, 8);
                    offset += 49;
                }

                // 发送数据
                NetDataHandle.Send_To_TM(NetDataHandle.curPao, NetDataHandle.curGuan, (ushort)dataLength, 0x21, wayPointData);
                //NetDataHandle.Send_To_TM(0x1, 0x1, (ushort)dataLength, 0x21, wayPointData);
                int sentCount = tempWayPoints.Count;
                
                // 清空临时航点列表
                tempWayPoints.Clear();
           
                UpdateTempWayPointsDisplay();
                
                MessageBox.Show($"成功发送 {sentCount} 个航点");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送航点数据失败: {ex.Message}");
            }
        }


        // 新增：根据遥测数据更新航点列表
        public void UpdateWaypointsFromTelemetry()
        {
            try
            {
                
                // 获取当前炮号和管号
                int paoID = NetDataHandle.curPao;
                int guanID = NetDataHandle.curGuan;
                
                if (paoID == 0 || guanID == 0)
                {
                    return; // 没有有效的炮号或管号
                }

                // 通过遥测数据获取航点号
                double waypointNumber = TMHandler.GetValueByParamID(paoID, guanID, "msnComm1");
                int n = (int)waypointNumber;

                // 检查航点号是否有效 (不等于0且不大于12)
                if (n < 0 || n > 12)
                {
                    return;
                }
                
                // 检查任务文件是否存在且当前编队有效
                if (missionFile == null || 
                    comboSelectGroup.SelectedIndex < 0 || 
                    comboSelectGroup.SelectedIndex >= missionFile.flightGroups.Count)
                {
                    return;
                }
                
                var currentGroup = missionFile.flightGroups[comboSelectGroup.SelectedIndex];
                //int n = 5;
                // 检查航点号是否超出当前航点列表范围
                if (n > currentGroup.airLine.Count)
                {
                    return;
                }

                // 删除第n个航点及其之后的所有航点
                int removedCount = currentGroup.airLine.Count - n + 1;
                System.Diagnostics.Debug.WriteLine($"文件航点-飞行航点+1 等于{removedCount}");
                if (removedCount > 0)
                {
                    currentGroup.airLine.RemoveRange(n - 1, removedCount);
                }

                // 将临时航点添加到任务文件中
                int tempWayPointsCount = tempWayPoints.Count;
                System.Diagnostics.Debug.WriteLine($"临时行点数{tempWayPoints.Count}");
                if (tempWayPointsCount > 0)
                {
                    currentGroup.airLine.AddRange(tempWayPoints);
                    
                }

                // 更新ListView显示
                LoadGroupInfoToListView(comboSelectGroup.SelectedIndex);
                
                // 重新加载当前编队的航点到地图
                _myMapView.LoadPointToMapForGroup(comboSelectGroup.SelectedIndex);
                
                // 更新当前编队的航线
                _myMapView.UpdateCurveForGroup(comboSelectGroup.SelectedIndex);
                
                // 发送任务更新消息
                MessageEvents.SendMsnUpdateMessage("msn Update");
                
                // 现在可以清空临时航点列表，因为已经添加到任务文件中了
                if (tempWayPointsCount > 0)
                {
                    tempWayPoints.Clear();
                }
                
                MessageBox.Show($"已删除第{n}个及后续航点，共删除{removedCount}个航点，添加了{tempWayPointsCount}个临时航点");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新航点列表失败: {ex.Message}");
            }
        }

    }
}
