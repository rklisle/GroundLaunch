namespace GroundLunch
{
    partial class LuanchPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LuanchPage));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btEngineStop = new DevExpress.XtraEditors.SimpleButton();
            this.btEngineStart = new DevExpress.XtraEditors.SimpleButton();
            this.comboLuanchMode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btDoIgnation = new DevExpress.XtraEditors.SimpleButton();
            this.btUnlockAll = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btGuanSelect = new DevExpress.XtraEditors.SimpleButton();
            this.btLanuchMode = new DevExpress.XtraEditors.SimpleButton();
            this.btConn = new DevExpress.XtraEditors.SimpleButton();
            this.btPaoSelect = new DevExpress.XtraEditors.SimpleButton();
            this.btIPAdress = new DevExpress.XtraEditors.SimpleButton();
            this.labelVersion = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.gaugeControl1 = new DevExpress.XtraGauges.Win.GaugeControl();
            this.digitalBackgroundLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent();
            this.flightTimeCtrl = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge();
            this.Paoche1 = new GroundLunch.PlaneInPaoche();
            this.msnShowAndSelect1 = new GroundLunch.MsnShowAndSelect();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboLuanchMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightTimeCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.Black;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.panelControl1.Controls.Add(this.btEngineStop);
            this.panelControl1.Controls.Add(this.btEngineStart);
            this.panelControl1.Controls.Add(this.comboLuanchMode);
            this.panelControl1.Controls.Add(this.btDoIgnation);
            this.panelControl1.Controls.Add(this.btUnlockAll);
            this.panelControl1.Controls.Add(this.Paoche1);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.msnShowAndSelect1);
            this.panelControl1.Location = new System.Drawing.Point(10, 28);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1900, 940);
            this.panelControl1.TabIndex = 0;
            // 
            // btEngineStop
            // 
            this.btEngineStop.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 15F);
            this.btEngineStop.Appearance.Options.UseFont = true;
            this.btEngineStop.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btEngineStop.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btEngineStop.ImageOptions.SvgImage")));
            this.btEngineStop.ImageOptions.SvgImageSize = new System.Drawing.Size(28, 28);
            this.btEngineStop.Location = new System.Drawing.Point(12, 905);
            this.btEngineStop.Name = "btEngineStop";
            this.btEngineStop.Size = new System.Drawing.Size(141, 26);
            this.btEngineStop.TabIndex = 26;
            this.btEngineStop.Text = "发动机停机";
            this.btEngineStop.Click += new System.EventHandler(this.btEngineStop_Click);
            // 
            // btEngineStart
            // 
            this.btEngineStart.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 15F);
            this.btEngineStart.Appearance.Options.UseFont = true;
            this.btEngineStart.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btEngineStart.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btEngineStart.ImageOptions.SvgImage")));
            this.btEngineStart.ImageOptions.SvgImageSize = new System.Drawing.Size(28, 28);
            this.btEngineStart.Location = new System.Drawing.Point(12, 875);
            this.btEngineStart.Name = "btEngineStart";
            this.btEngineStart.Size = new System.Drawing.Size(141, 26);
            this.btEngineStart.TabIndex = 24;
            this.btEngineStart.Text = "发动机起动";
            this.btEngineStart.Click += new System.EventHandler(this.btEngineStart_Click);
            // 
            // comboLuanchMode
            // 
            this.comboLuanchMode.EditValue = "单发";
            this.comboLuanchMode.Location = new System.Drawing.Point(296, 875);
            this.comboLuanchMode.Name = "comboLuanchMode";
            this.comboLuanchMode.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 15F);
            this.comboLuanchMode.Properties.Appearance.Options.UseFont = true;
            this.comboLuanchMode.Properties.AppearanceDropDown.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.comboLuanchMode.Properties.AppearanceDropDown.Options.UseFont = true;
            this.comboLuanchMode.Properties.AutoHeight = false;
            this.comboLuanchMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboLuanchMode.Properties.Items.AddRange(new object[] {
            "单发",
            "连发",
            "齐射"});
            this.comboLuanchMode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboLuanchMode.Size = new System.Drawing.Size(70, 56);
            this.comboLuanchMode.TabIndex = 23;
            // 
            // btDoIgnation
            // 
            this.btDoIgnation.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 15F);
            this.btDoIgnation.Appearance.Options.UseFont = true;
            this.btDoIgnation.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btDoIgnation.ImageOptions.Image")));
            this.btDoIgnation.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btDoIgnation.Location = new System.Drawing.Point(374, 875);
            this.btDoIgnation.Name = "btDoIgnation";
            this.btDoIgnation.Size = new System.Drawing.Size(88, 56);
            this.btDoIgnation.TabIndex = 22;
            this.btDoIgnation.Text = "起飞";
            this.btDoIgnation.Click += new System.EventHandler(this.btDoIgnation_Click);
            // 
            // btUnlockAll
            // 
            this.btUnlockAll.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 15F);
            this.btUnlockAll.Appearance.Options.UseFont = true;
            this.btUnlockAll.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btUnlockAll.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btUnlockAll.ImageOptions.SvgImage")));
            this.btUnlockAll.ImageOptions.SvgImageSize = new System.Drawing.Size(28, 28);
            this.btUnlockAll.Location = new System.Drawing.Point(159, 875);
            this.btUnlockAll.Name = "btUnlockAll";
            this.btUnlockAll.Size = new System.Drawing.Size(131, 56);
            this.btUnlockAll.TabIndex = 21;
            this.btUnlockAll.Text = "全部解锁";
            this.btUnlockAll.Click += new System.EventHandler(this.btUnlockAll_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("等线", 30F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(87, 15);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(421, 41);
            this.labelControl3.TabIndex = 11;
            this.labelControl3.Text = "ZT280-Y1 巡飞发控系统";
            // 
            // panelControl
            // 
            this.panelControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.panelControl.Appearance.Options.UseBackColor = true;
            this.panelControl.AutoSize = true;
            this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl.Controls.Add(this.labelControl1);
            this.panelControl.Controls.Add(this.btGuanSelect);
            this.panelControl.Controls.Add(this.btLanuchMode);
            this.panelControl.Controls.Add(this.btConn);
            this.panelControl.Controls.Add(this.btPaoSelect);
            this.panelControl.Controls.Add(this.btIPAdress);
            this.panelControl.Controls.Add(this.labelVersion);
            this.panelControl.Controls.Add(this.labelControl2);
            this.panelControl.Controls.Add(this.gaugeControl1);
            this.panelControl.Location = new System.Drawing.Point(593, 10);
            this.panelControl.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(733, 55);
            this.panelControl.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(261, 16);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(7, 21);
            this.labelControl1.TabIndex = 35;
            this.labelControl1.Text = "-";
            // 
            // btGuanSelect
            // 
            this.btGuanSelect.Appearance.BackColor = System.Drawing.Color.Black;
            this.btGuanSelect.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btGuanSelect.Appearance.ForeColor = System.Drawing.Color.White;
            this.btGuanSelect.Appearance.Options.UseBackColor = true;
            this.btGuanSelect.Appearance.Options.UseFont = true;
            this.btGuanSelect.Appearance.Options.UseForeColor = true;
            this.btGuanSelect.Location = new System.Drawing.Point(268, 15);
            this.btGuanSelect.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btGuanSelect.Name = "btGuanSelect";
            this.btGuanSelect.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btGuanSelect.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btGuanSelect.Size = new System.Drawing.Size(24, 24);
            this.btGuanSelect.TabIndex = 34;
            this.btGuanSelect.Text = "01";
            // 
            // btLanuchMode
            // 
            this.btLanuchMode.AllowFocus = false;
            this.btLanuchMode.Appearance.BackColor = System.Drawing.Color.Black;
            this.btLanuchMode.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btLanuchMode.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.btLanuchMode.Appearance.Options.UseBackColor = true;
            this.btLanuchMode.Appearance.Options.UseFont = true;
            this.btLanuchMode.Appearance.Options.UseForeColor = true;
            this.btLanuchMode.Location = new System.Drawing.Point(454, 15);
            this.btLanuchMode.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btLanuchMode.Name = "btLanuchMode";
            this.btLanuchMode.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btLanuchMode.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btLanuchMode.Size = new System.Drawing.Size(92, 24);
            this.btLanuchMode.TabIndex = 33;
            this.btLanuchMode.TabStop = false;
            this.btLanuchMode.Text = "正式发射";
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
            this.btConn.TabIndex = 32;
            this.btConn.Text = "UDP已断开";
            this.btConn.Click += new System.EventHandler(this.btConn_Click);
            // 
            // btPaoSelect
            // 
            this.btPaoSelect.Appearance.BackColor = System.Drawing.Color.Black;
            this.btPaoSelect.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btPaoSelect.Appearance.ForeColor = System.Drawing.Color.White;
            this.btPaoSelect.Appearance.Options.UseBackColor = true;
            this.btPaoSelect.Appearance.Options.UseFont = true;
            this.btPaoSelect.Appearance.Options.UseForeColor = true;
            this.btPaoSelect.Location = new System.Drawing.Point(238, 15);
            this.btPaoSelect.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btPaoSelect.Name = "btPaoSelect";
            this.btPaoSelect.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btPaoSelect.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btPaoSelect.Size = new System.Drawing.Size(24, 24);
            this.btPaoSelect.TabIndex = 31;
            this.btPaoSelect.Text = "01";
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
            this.btIPAdress.TabIndex = 30;
            this.btIPAdress.Text = "192.168.1.100";
            this.btIPAdress.Click += new System.EventHandler(this.btIPAdress_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersion.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelVersion.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Appearance.Options.UseFont = true;
            this.labelVersion.Appearance.Options.UseForeColor = true;
            this.labelVersion.Location = new System.Drawing.Point(618, 17);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(72, 21);
            this.labelVersion.TabIndex = 27;
            this.labelVersion.Text = "20240315";
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(555, 17);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(64, 21);
            this.labelControl2.TabIndex = 26;
            this.labelControl2.Text = "版本号：";
            // 
            // gaugeControl1
            // 
            this.gaugeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gaugeControl1.BackColor = System.Drawing.Color.Black;
            this.gaugeControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gaugeControl1.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.flightTimeCtrl});
            this.gaugeControl1.LayoutInterval = 4;
            this.gaugeControl1.LayoutPadding = new DevExpress.XtraGauges.Core.Base.Thickness(2);
            this.gaugeControl1.Location = new System.Drawing.Point(300, 0);
            this.gaugeControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gaugeControl1.Name = "gaugeControl1";
            this.gaugeControl1.Size = new System.Drawing.Size(133, 53);
            this.gaugeControl1.TabIndex = 25;
            // 
            // digitalBackgroundLayerComponent1
            // 
            this.digitalBackgroundLayerComponent1.BottomRight = new DevExpress.XtraGauges.Core.Base.PointF2D(265.8125F, 99.9625F);
            this.digitalBackgroundLayerComponent1.Name = "digitalBackgroundLayerComponent1";
            this.digitalBackgroundLayerComponent1.ShapeType = DevExpress.XtraGauges.Core.Model.DigitalBackgroundShapeSetType.Style24;
            this.digitalBackgroundLayerComponent1.TopLeft = new DevExpress.XtraGauges.Core.Base.PointF2D(26F, 0F);
            this.digitalBackgroundLayerComponent1.ZOrder = 1000;
            // 
            // flightTimeCtrl
            // 
            this.flightTimeCtrl.AppearanceOff.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#F3B030");
            this.flightTimeCtrl.AppearanceOn.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#343A49");
            this.flightTimeCtrl.BackgroundLayers.AddRange(new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent[] {
            this.digitalBackgroundLayerComponent1});
            this.flightTimeCtrl.Bounds = new System.Drawing.Rectangle(2, 2, 129, 49);
            this.flightTimeCtrl.DigitCount = 5;
            this.flightTimeCtrl.Name = "flightTimeCtrl";
            this.flightTimeCtrl.Padding = new DevExpress.XtraGauges.Core.Base.TextSpacing(26, 20, 26, 20);
            this.flightTimeCtrl.Text = "00.000";
            // 
            // Paoche1
            // 
            this.Paoche1.Location = new System.Drawing.Point(470, 68);
            this.Paoche1.Margin = new System.Windows.Forms.Padding(4);
            this.Paoche1.Name = "Paoche1";
            this.Paoche1.Size = new System.Drawing.Size(1424, 863);
            this.Paoche1.TabIndex = 12;
            // 
            // msnShowAndSelect1
            // 
            this.msnShowAndSelect1.Location = new System.Drawing.Point(12, 68);
            this.msnShowAndSelect1.Margin = new System.Windows.Forms.Padding(4);
            this.msnShowAndSelect1.Name = "msnShowAndSelect1";
            this.msnShowAndSelect1.Size = new System.Drawing.Size(450, 800);
            this.msnShowAndSelect1.TabIndex = 0;
            // 
            // LuanchPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LuanchPage";
            this.Size = new System.Drawing.Size(1920, 980);
            this.Load += new System.EventHandler(this.LuanchPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboLuanchMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightTimeCtrl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl;
        private DevExpress.XtraEditors.SimpleButton btLanuchMode;
        private DevExpress.XtraEditors.SimpleButton btConn;
        private DevExpress.XtraEditors.SimpleButton btPaoSelect;
        private DevExpress.XtraEditors.SimpleButton btIPAdress;
        private DevExpress.XtraEditors.LabelControl labelVersion;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGauges.Win.GaugeControl gaugeControl1;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge flightTimeCtrl;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent digitalBackgroundLayerComponent1;
        private MsnShowAndSelect msnShowAndSelect1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private PlaneInPaoche Paoche1;
        private DevExpress.XtraEditors.ComboBoxEdit comboLuanchMode;
        private DevExpress.XtraEditors.SimpleButton btDoIgnation;
        private DevExpress.XtraEditors.SimpleButton btUnlockAll;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btGuanSelect;
        private DevExpress.XtraEditors.SimpleButton btEngineStart;
        private DevExpress.XtraEditors.SimpleButton btEngineStop;
    }
}
