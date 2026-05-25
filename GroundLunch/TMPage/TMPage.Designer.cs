namespace GroundLunch
{
    partial class TMPage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TMPage));
            DevExpress.XtraCharts.XYDiagram xyDiagram2 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.btConn = new DevExpress.XtraEditors.SimpleButton();
            this.btIDSelect = new DevExpress.XtraEditors.SimpleButton();
            this.btIPAdress = new DevExpress.XtraEditors.SimpleButton();
            this.gaugeControl1 = new DevExpress.XtraGauges.Win.GaugeControl();
            this.flightTimeCtrl = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge();
            this.digitalBackgroundLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btTMDataSource = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tmEngine = new GroundLunch.TMEngine();
            this.tmTrack3D = new GroundLunch.TMTrack3D();
            this.tmMap2 = new GroundLunch.TMMap();
            this.tmhdu2 = new GroundLunch.TMHDU();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelBJLuanch = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelBJCur = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btEmgrecyRecycle = new DevExpress.XtraEditors.SimpleButton();
            this.btEmgrecyUnbrl = new DevExpress.XtraEditors.SimpleButton();
            this.btClearSelection = new DevExpress.XtraEditors.SimpleButton();
            this.btReloadExcel = new DevExpress.XtraEditors.SimpleButton();
            this.btSaveExcel = new DevExpress.XtraEditors.SimpleButton();
            this.btClearData = new DevExpress.XtraEditors.SimpleButton();
            this.gridTM = new DevExpress.XtraGrid.GridControl();
            this.viewTM = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flightTimeCtrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTM)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.panelControl.Appearance.Options.UseBackColor = true;
            this.panelControl.AutoSize = true;
            this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl.Controls.Add(this.btConn);
            this.panelControl.Controls.Add(this.btIDSelect);
            this.panelControl.Controls.Add(this.btIPAdress);
            this.panelControl.Controls.Add(this.gaugeControl1);
            this.panelControl.Controls.Add(this.simpleButton1);
            this.panelControl.Controls.Add(this.btTMDataSource);
            this.panelControl.Location = new System.Drawing.Point(653, 9);
            this.panelControl.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(733, 55);
            this.panelControl.TabIndex = 30;
            // 
            // btConn
            // 
            this.btConn.Appearance.BackColor = System.Drawing.Color.Black;
            this.btConn.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btConn.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btConn.Appearance.Options.UseBackColor = true;
            this.btConn.Appearance.Options.UseFont = true;
            this.btConn.Appearance.Options.UseForeColor = true;
            this.btConn.Location = new System.Drawing.Point(13, 15);
            this.btConn.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btConn.Name = "btConn";
            this.btConn.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btConn.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btConn.Size = new System.Drawing.Size(90, 24);
            this.btConn.TabIndex = 41;
            this.btConn.Text = "UDP已断开";
            this.btConn.Click += new System.EventHandler(this.btConn_Click_1);
            // 
            // btIDSelect
            // 
            this.btIDSelect.Appearance.BackColor = System.Drawing.Color.Black;
            this.btIDSelect.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btIDSelect.Appearance.ForeColor = System.Drawing.Color.White;
            this.btIDSelect.Appearance.Options.UseBackColor = true;
            this.btIDSelect.Appearance.Options.UseFont = true;
            this.btIDSelect.Appearance.Options.UseForeColor = true;
            this.btIDSelect.Location = new System.Drawing.Point(242, 15);
            this.btIDSelect.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btIDSelect.Name = "btIDSelect";
            this.btIDSelect.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btIDSelect.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btIDSelect.Size = new System.Drawing.Size(54, 24);
            this.btIDSelect.TabIndex = 40;
            this.btIDSelect.Text = "ID:01";
            this.btIDSelect.Click += new System.EventHandler(this.btIDSelect_Click);
            // 
            // btIPAdress
            // 
            this.btIPAdress.Appearance.BackColor = System.Drawing.Color.Black;
            this.btIPAdress.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btIPAdress.Appearance.ForeColor = System.Drawing.Color.White;
            this.btIPAdress.Appearance.Options.UseBackColor = true;
            this.btIPAdress.Appearance.Options.UseFont = true;
            this.btIPAdress.Appearance.Options.UseForeColor = true;
            this.btIPAdress.Location = new System.Drawing.Point(107, 15);
            this.btIPAdress.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btIPAdress.Name = "btIPAdress";
            this.btIPAdress.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btIPAdress.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btIPAdress.Size = new System.Drawing.Size(135, 24);
            this.btIPAdress.TabIndex = 39;
            this.btIPAdress.Text = "192.168.1.100";
            this.btIPAdress.Click += new System.EventHandler(this.btIPAdress_Click);
            // 
            // gaugeControl1
            // 
            this.gaugeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gaugeControl1.BackColor = System.Drawing.Color.Black;
            this.gaugeControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gaugeControl1.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.flightTimeCtrl});
            this.gaugeControl1.LayoutPadding = new DevExpress.XtraGauges.Core.Base.Thickness(1);
            this.gaugeControl1.Location = new System.Drawing.Point(300, 0);
            this.gaugeControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gaugeControl1.Name = "gaugeControl1";
            this.gaugeControl1.Size = new System.Drawing.Size(132, 53);
            this.gaugeControl1.TabIndex = 38;
            // 
            // flightTimeCtrl
            // 
            this.flightTimeCtrl.AppearanceOff.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#233342");
            this.flightTimeCtrl.AppearanceOn.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#39A7CE");
            this.flightTimeCtrl.BackgroundLayers.AddRange(new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent[] {
            this.digitalBackgroundLayerComponent1});
            this.flightTimeCtrl.Bounds = new System.Drawing.Rectangle(1, 1, 130, 51);
            this.flightTimeCtrl.DigitCount = 5;
            this.flightTimeCtrl.Name = "flightTimeCtrl";
            this.flightTimeCtrl.Padding = new DevExpress.XtraGauges.Core.Base.TextSpacing(26, 20, 26, 20);
            this.flightTimeCtrl.Text = "00,000";
            // 
            // digitalBackgroundLayerComponent1
            // 
            this.digitalBackgroundLayerComponent1.BottomRight = new DevExpress.XtraGauges.Core.Base.PointF2D(265.8125F, 99.9625F);
            this.digitalBackgroundLayerComponent1.Name = "digitalBackgroundLayerComponent1";
            this.digitalBackgroundLayerComponent1.ShapeType = DevExpress.XtraGauges.Core.Model.DigitalBackgroundShapeSetType.Style19;
            this.digitalBackgroundLayerComponent1.TopLeft = new DevExpress.XtraGauges.Core.Base.PointF2D(26F, 0F);
            this.digitalBackgroundLayerComponent1.ZOrder = 1000;
            // 
            // simpleButton1
            // 
            this.simpleButton1.AllowFocus = false;
            this.simpleButton1.Appearance.BackColor = System.Drawing.Color.Black;
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.ForeColor = System.Drawing.Color.White;
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Appearance.Options.UseForeColor = true;
            this.simpleButton1.Location = new System.Drawing.Point(607, 17);
            this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.simpleButton1.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.simpleButton1.Size = new System.Drawing.Size(105, 26);
            this.simpleButton1.TabIndex = 34;
            this.simpleButton1.TabStop = false;
            this.simpleButton1.Text = "波道表：停用";
            // 
            // btTMDataSource
            // 
            this.btTMDataSource.AllowFocus = false;
            this.btTMDataSource.Appearance.BackColor = System.Drawing.Color.Black;
            this.btTMDataSource.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btTMDataSource.Appearance.ForeColor = System.Drawing.Color.White;
            this.btTMDataSource.Appearance.Options.UseBackColor = true;
            this.btTMDataSource.Appearance.Options.UseFont = true;
            this.btTMDataSource.Appearance.Options.UseForeColor = true;
            this.btTMDataSource.Location = new System.Drawing.Point(449, 17);
            this.btTMDataSource.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btTMDataSource.Name = "btTMDataSource";
            this.btTMDataSource.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btTMDataSource.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btTMDataSource.Size = new System.Drawing.Size(152, 26);
            this.btTMDataSource.TabIndex = 33;
            this.btTMDataSource.TabStop = false;
            this.btTMDataSource.Text = "数据来源: UDP";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tmEngine);
            this.panelControl1.Controls.Add(this.tmTrack3D);
            this.panelControl1.Controls.Add(this.tmMap2);
            this.panelControl1.Controls.Add(this.tmhdu2);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Location = new System.Drawing.Point(10, 28);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1900, 940);
            this.panelControl1.TabIndex = 31;
            // 
            // tmEngine
            // 
            this.tmEngine.Location = new System.Drawing.Point(1431, 506);
            this.tmEngine.Margin = new System.Windows.Forms.Padding(4);
            this.tmEngine.Name = "tmEngine";
            this.tmEngine.Size = new System.Drawing.Size(447, 421);
            this.tmEngine.TabIndex = 51;
            // 
            // tmTrack3D
            // 
            this.tmTrack3D.Location = new System.Drawing.Point(943, 507);
            this.tmTrack3D.Margin = new System.Windows.Forms.Padding(4);
            this.tmTrack3D.Name = "tmTrack3D";
            this.tmTrack3D.Size = new System.Drawing.Size(476, 420);
            this.tmTrack3D.TabIndex = 50;
            // 
            // tmMap2
            // 
            this.tmMap2.Location = new System.Drawing.Point(943, 71);
            this.tmMap2.Margin = new System.Windows.Forms.Padding(4);
            this.tmMap2.Name = "tmMap2";
            this.tmMap2.Size = new System.Drawing.Size(513, 419);
            this.tmMap2.TabIndex = 49;
            // 
            // tmhdu2
            // 
            this.tmhdu2.Location = new System.Drawing.Point(1464, 71);
            this.tmhdu2.Margin = new System.Windows.Forms.Padding(4);
            this.tmhdu2.Name = "tmhdu2";
            this.tmhdu2.Size = new System.Drawing.Size(414, 419);
            this.tmhdu2.TabIndex = 48;
            // 
            // groupControl1
            // 
            this.groupControl1.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("groupControl1.CaptionImageOptions.SvgImage")));
            this.groupControl1.Controls.Add(this.labelBJLuanch);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelBJCur);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.btEmgrecyRecycle);
            this.groupControl1.Controls.Add(this.btEmgrecyUnbrl);
            this.groupControl1.Controls.Add(this.btClearSelection);
            this.groupControl1.Controls.Add(this.btReloadExcel);
            this.groupControl1.Controls.Add(this.btSaveExcel);
            this.groupControl1.Controls.Add(this.btClearData);
            this.groupControl1.Controls.Add(this.gridTM);
            this.groupControl1.Location = new System.Drawing.Point(11, 71);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(914, 860);
            this.groupControl1.TabIndex = 40;
            this.groupControl1.Text = "遥测数据解析";
            // 
            // labelBJLuanch
            // 
            this.labelBJLuanch.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 11F);
            this.labelBJLuanch.Appearance.Options.UseFont = true;
            this.labelBJLuanch.Location = new System.Drawing.Point(529, 7);
            this.labelBJLuanch.Name = "labelBJLuanch";
            this.labelBJLuanch.Size = new System.Drawing.Size(160, 20);
            this.labelBJLuanch.TabIndex = 39;
            this.labelBJLuanch.Text = "2000-01-01 00:00:00.000";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 11F);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(440, 6);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(87, 20);
            this.labelControl5.TabIndex = 38;
            this.labelControl5.Text = "飞行0时标记:";
            // 
            // labelBJCur
            // 
            this.labelBJCur.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 11F);
            this.labelBJCur.Appearance.Options.UseFont = true;
            this.labelBJCur.Location = new System.Drawing.Point(256, 7);
            this.labelBJCur.Name = "labelBJCur";
            this.labelBJCur.Size = new System.Drawing.Size(160, 20);
            this.labelBJCur.TabIndex = 37;
            this.labelBJCur.Text = "2000-01-01 00:00:00.000";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 11F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(159, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(94, 20);
            this.labelControl2.TabIndex = 36;
            this.labelControl2.Text = "当前北京时间:";
            // 
            // btEmgrecyRecycle
            // 
            this.btEmgrecyRecycle.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 10F);
            this.btEmgrecyRecycle.Appearance.Options.UseFont = true;
            this.btEmgrecyRecycle.Location = new System.Drawing.Point(823, 5);
            this.btEmgrecyRecycle.Name = "btEmgrecyRecycle";
            this.btEmgrecyRecycle.Size = new System.Drawing.Size(75, 25);
            this.btEmgrecyRecycle.TabIndex = 35;
            this.btEmgrecyRecycle.Text = "紧急返航";
            this.btEmgrecyRecycle.Click += new System.EventHandler(this.btEmgrecyRecycle_Click);
            // 
            // btEmgrecyUnbrl
            // 
            this.btEmgrecyUnbrl.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 10F);
            this.btEmgrecyUnbrl.Appearance.Options.UseFont = true;
            this.btEmgrecyUnbrl.Location = new System.Drawing.Point(741, 5);
            this.btEmgrecyUnbrl.Name = "btEmgrecyUnbrl";
            this.btEmgrecyUnbrl.Size = new System.Drawing.Size(75, 25);
            this.btEmgrecyUnbrl.TabIndex = 34;
            this.btEmgrecyUnbrl.Text = "紧急伞降";
            this.btEmgrecyUnbrl.Click += new System.EventHandler(this.btEmgrecyUnbrl_Click);
            // 
            // btClearSelection
            // 
            this.btClearSelection.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btClearSelection.Appearance.Options.UseFont = true;
            this.btClearSelection.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btClearSelection.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btClearSelection.ImageOptions.SvgImage")));
            this.btClearSelection.Location = new System.Drawing.Point(824, 693);
            this.btClearSelection.Name = "btClearSelection";
            this.btClearSelection.Size = new System.Drawing.Size(75, 72);
            this.btClearSelection.TabIndex = 33;
            this.btClearSelection.Text = "清空已选";
            this.btClearSelection.Click += new System.EventHandler(this.btClearSelection_Click);
            // 
            // btReloadExcel
            // 
            this.btReloadExcel.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btReloadExcel.Appearance.Options.UseFont = true;
            this.btReloadExcel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btReloadExcel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btReloadExcel.ImageOptions.SvgImage")));
            this.btReloadExcel.Location = new System.Drawing.Point(823, 529);
            this.btReloadExcel.Name = "btReloadExcel";
            this.btReloadExcel.Size = new System.Drawing.Size(75, 72);
            this.btReloadExcel.TabIndex = 32;
            this.btReloadExcel.Text = "重新加载";
            this.btReloadExcel.Click += new System.EventHandler(this.btReloadExcel_Click);
            // 
            // btSaveExcel
            // 
            this.btSaveExcel.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btSaveExcel.Appearance.Options.UseFont = true;
            this.btSaveExcel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btSaveExcel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btSaveExcel.ImageOptions.SvgImage")));
            this.btSaveExcel.Location = new System.Drawing.Point(824, 775);
            this.btSaveExcel.Name = "btSaveExcel";
            this.btSaveExcel.Size = new System.Drawing.Size(75, 72);
            this.btSaveExcel.TabIndex = 31;
            this.btSaveExcel.Text = "保存数据";
            this.btSaveExcel.Click += new System.EventHandler(this.btSaveExcel_Click);
            // 
            // btClearData
            // 
            this.btClearData.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btClearData.Appearance.Options.UseFont = true;
            this.btClearData.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btClearData.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btClearData.ImageOptions.SvgImage")));
            this.btClearData.Location = new System.Drawing.Point(824, 611);
            this.btClearData.Name = "btClearData";
            this.btClearData.Size = new System.Drawing.Size(75, 72);
            this.btClearData.TabIndex = 30;
            this.btClearData.Text = "重新记录";
            this.btClearData.Click += new System.EventHandler(this.btClearData_Click);
            // 
            // gridTM
            // 
            this.gridTM.Location = new System.Drawing.Point(15, 45);
            this.gridTM.MainView = this.viewTM;
            this.gridTM.Name = "gridTM";
            this.gridTM.Size = new System.Drawing.Size(884, 469);
            this.gridTM.TabIndex = 0;
            this.gridTM.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewTM});
            // 
            // viewTM
            // 
            this.viewTM.Appearance.EvenRow.BackColor = System.Drawing.Color.Transparent;
            this.viewTM.Appearance.EvenRow.Options.UseBackColor = true;
            this.viewTM.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 11F);
            this.viewTM.Appearance.HeaderPanel.Options.UseFont = true;
            this.viewTM.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.viewTM.Appearance.OddRow.Options.UseBackColor = true;
            this.viewTM.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 11F);
            this.viewTM.Appearance.Row.Options.UseFont = true;
            this.viewTM.DetailHeight = 375;
            this.viewTM.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.viewTM.GridControl = this.gridTM;
            this.viewTM.Name = "viewTM";
            this.viewTM.OptionsCustomization.AllowFilter = false;
            this.viewTM.OptionsCustomization.AllowSort = false;
            this.viewTM.OptionsFilter.AllowFilterEditor = false;
            this.viewTM.OptionsScrollAnnotations.ShowSelectedRows = DevExpress.Utils.DefaultBoolean.False;
            this.viewTM.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.viewTM.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.viewTM.OptionsView.ColumnAutoWidth = false;
            this.viewTM.OptionsView.EnableAppearanceEvenRow = true;
            this.viewTM.OptionsView.EnableAppearanceOddRow = true;
            this.viewTM.OptionsView.ShowGroupPanel = false;
            this.viewTM.OptionsView.ShowIndicator = false;
            this.viewTM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.viewTM_CellValueChanged);
            this.viewTM.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.viewTM_CellValueChanging);
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Appearance.Font = new System.Drawing.Font("方正姚体", 30F, System.Drawing.FontStyle.Italic);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(1416, 8);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(442, 46);
            this.labelControl1.TabIndex = 39;
            this.labelControl1.Text = "研制单位：北京白月飞空";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("等线", 30F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(11, 16);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(392, 41);
            this.labelControl3.TabIndex = 38;
            this.labelControl3.Text = "PJ60-Y1 巡飞发控系统";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // TMPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TMPage";
            this.Size = new System.Drawing.Size(1920, 980);
            this.Load += new System.EventHandler(this.TMPage_Load);
            this.Resize += new System.EventHandler(this.TMPage_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.flightTimeCtrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl;
        private DevExpress.XtraEditors.SimpleButton btTMDataSource;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gridTM;
        private DevExpress.XtraGrid.Views.Grid.GridView viewTM;
        private DevExpress.XtraEditors.SimpleButton btSaveExcel;
        private DevExpress.XtraEditors.SimpleButton btClearData;
        private DevExpress.XtraEditors.SimpleButton btReloadExcel;
        private DevExpress.XtraEditors.SimpleButton btClearSelection;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.XtraGauges.Win.GaugeControl gaugeControl1;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge flightTimeCtrl;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent digitalBackgroundLayerComponent1;
        private TMHDU tmhdu2;
        private TMMap tmMap2;
        private TMTrack3D tmTrack3D;
        private DevExpress.XtraEditors.SimpleButton btEmgrecyRecycle;
        private DevExpress.XtraEditors.SimpleButton btEmgrecyUnbrl;
        private DevExpress.XtraEditors.SimpleButton btConn;
        private DevExpress.XtraEditors.SimpleButton btIDSelect;
        private DevExpress.XtraEditors.SimpleButton btIPAdress;
        private TMEngine tmEngine;
        private DevExpress.XtraEditors.LabelControl labelBJLuanch;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelBJCur;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
