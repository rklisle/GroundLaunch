namespace GroundLunch
{
    partial class HomePage
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
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.btLanuchMode = new DevExpress.XtraEditors.SimpleButton();
            this.btConn = new DevExpress.XtraEditors.SimpleButton();
            this.btIDSelect = new DevExpress.XtraEditors.SimpleButton();
            this.btIPAdress = new DevExpress.XtraEditors.SimpleButton();
            this.labelVersion = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.gaugeControl1 = new DevExpress.XtraGauges.Win.GaugeControl();
            this.digitalBackgroundLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.flightTimeCtrl = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge();
            this.modDevPowerFuse = new GroundLunch.ModDevPower();
            this.modPayload1 = new GroundLunch.ModPayload();
            this.modLanuch = new GroundLunch.ModLanuch();
            this.mod3DCtrl = new GroundLunch.Mod3D();
            this.modFileCtrl = new GroundLunch.ModFile();
            this.modNavCtrl = new GroundLunch.ModNav();
            this.modDevPowerSrv = new GroundLunch.ModDevPower();
            this.modDevPowerBatt = new GroundLunch.ModDevPower();
            this.modDevPowerCombin = new GroundLunch.ModDevPower();
            this.modIMUCtrl = new GroundLunch.ModIMU();
            this.modSrvCtrl = new GroundLunch.ModSrv();
            this.modGpsCtrl = new GroundLunch.ModGps();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flightTimeCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.panelControl.Appearance.Options.UseBackColor = true;
            this.panelControl.AutoSize = true;
            this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl.Controls.Add(this.btLanuchMode);
            this.panelControl.Controls.Add(this.btConn);
            this.panelControl.Controls.Add(this.btIDSelect);
            this.panelControl.Controls.Add(this.btIPAdress);
            this.panelControl.Controls.Add(this.labelVersion);
            this.panelControl.Controls.Add(this.labelControl2);
            this.panelControl.Controls.Add(this.gaugeControl1);
            this.panelControl.Location = new System.Drawing.Point(653, 8);
            this.panelControl.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(733, 55);
            this.panelControl.TabIndex = 0;
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
            this.btLanuchMode.Click += new System.EventHandler(this.btLanuchMode_Click);
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
            this.btIDSelect.TabIndex = 31;
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
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.Black;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.panelControl1.Controls.Add(this.modDevPowerFuse);
            this.panelControl1.Controls.Add(this.modPayload1);
            this.panelControl1.Controls.Add(this.modLanuch);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.mod3DCtrl);
            this.panelControl1.Controls.Add(this.modFileCtrl);
            this.panelControl1.Controls.Add(this.modNavCtrl);
            this.panelControl1.Controls.Add(this.modDevPowerSrv);
            this.panelControl1.Controls.Add(this.modDevPowerBatt);
            this.panelControl1.Controls.Add(this.modDevPowerCombin);
            this.panelControl1.Controls.Add(this.modIMUCtrl);
            this.panelControl1.Controls.Add(this.modSrvCtrl);
            this.panelControl1.Controls.Add(this.modGpsCtrl);
            this.panelControl1.Location = new System.Drawing.Point(10, 28);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1900, 940);
            this.panelControl1.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("等线", 30F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(87, 15);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(392, 41);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "PJ60-Y1 巡飞发控系统";
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Appearance.Font = new System.Drawing.Font("方正姚体", 30F, System.Drawing.FontStyle.Italic);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(1416, 7);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(442, 46);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "研制单位：北京白月飞空";
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
            // modDevPowerFuse
            // 
            this.modDevPowerFuse.Location = new System.Drawing.Point(11, 496);
            this.modDevPowerFuse.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modDevPowerFuse.Name = "modDevPowerFuse";
            this.modDevPowerFuse.Size = new System.Drawing.Size(531, 137);
            this.modDevPowerFuse.TabIndex = 13;
            // 
            // modPayload1
            // 
            this.modPayload1.Location = new System.Drawing.Point(820, 613);
            this.modPayload1.Margin = new System.Windows.Forms.Padding(4);
            this.modPayload1.Name = "modPayload1";
            this.modPayload1.Size = new System.Drawing.Size(190, 316);
            this.modPayload1.TabIndex = 12;
            // 
            // modLanuch
            // 
            this.modLanuch.Location = new System.Drawing.Point(11, 836);
            this.modLanuch.Margin = new System.Windows.Forms.Padding(4);
            this.modLanuch.Name = "modLanuch";
            this.modLanuch.Size = new System.Drawing.Size(531, 93);
            this.modLanuch.TabIndex = 11;
            // 
            // mod3DCtrl
            // 
            this.mod3DCtrl.Location = new System.Drawing.Point(553, 66);
            this.mod3DCtrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.mod3DCtrl.Name = "mod3DCtrl";
            this.mod3DCtrl.Size = new System.Drawing.Size(851, 530);
            this.mod3DCtrl.TabIndex = 8;
            // 
            // modFileCtrl
            // 
            this.modFileCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.modFileCtrl.Location = new System.Drawing.Point(553, 613);
            this.modFileCtrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modFileCtrl.Name = "modFileCtrl";
            this.modFileCtrl.Size = new System.Drawing.Size(256, 316);
            this.modFileCtrl.TabIndex = 7;
            // 
            // modNavCtrl
            // 
            this.modNavCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.modNavCtrl.Location = new System.Drawing.Point(11, 639);
            this.modNavCtrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modNavCtrl.Name = "modNavCtrl";
            this.modNavCtrl.Size = new System.Drawing.Size(531, 190);
            this.modNavCtrl.TabIndex = 6;
            // 
            // modDevPowerSrv
            // 
            this.modDevPowerSrv.Location = new System.Drawing.Point(11, 353);
            this.modDevPowerSrv.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modDevPowerSrv.Name = "modDevPowerSrv";
            this.modDevPowerSrv.Size = new System.Drawing.Size(531, 137);
            this.modDevPowerSrv.TabIndex = 5;
            // 
            // modDevPowerBatt
            // 
            this.modDevPowerBatt.Location = new System.Drawing.Point(11, 210);
            this.modDevPowerBatt.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modDevPowerBatt.Name = "modDevPowerBatt";
            this.modDevPowerBatt.Size = new System.Drawing.Size(531, 137);
            this.modDevPowerBatt.TabIndex = 4;
            // 
            // modDevPowerCombin
            // 
            this.modDevPowerCombin.Location = new System.Drawing.Point(11, 66);
            this.modDevPowerCombin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modDevPowerCombin.Name = "modDevPowerCombin";
            this.modDevPowerCombin.Size = new System.Drawing.Size(531, 138);
            this.modDevPowerCombin.TabIndex = 3;
            // 
            // modIMUCtrl
            // 
            this.modIMUCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modIMUCtrl.Location = new System.Drawing.Point(1021, 613);
            this.modIMUCtrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modIMUCtrl.Name = "modIMUCtrl";
            this.modIMUCtrl.Size = new System.Drawing.Size(383, 316);
            this.modIMUCtrl.TabIndex = 2;
            // 
            // modSrvCtrl
            // 
            this.modSrvCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modSrvCtrl.Location = new System.Drawing.Point(1415, 66);
            this.modSrvCtrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modSrvCtrl.Name = "modSrvCtrl";
            this.modSrvCtrl.Size = new System.Drawing.Size(471, 530);
            this.modSrvCtrl.TabIndex = 1;
            // 
            // modGpsCtrl
            // 
            this.modGpsCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.modGpsCtrl.Location = new System.Drawing.Point(1416, 613);
            this.modGpsCtrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modGpsCtrl.Name = "modGpsCtrl";
            this.modGpsCtrl.Size = new System.Drawing.Size(471, 316);
            this.modGpsCtrl.TabIndex = 0;
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "HomePage";
            this.Size = new System.Drawing.Size(1920, 980);
            this.Load += new System.EventHandler(this.HomePage_Load);
            this.Resize += new System.EventHandler(this.HomePage_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flightTimeCtrl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl;
        private DevExpress.XtraGauges.Win.GaugeControl gaugeControl1;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge flightTimeCtrl;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent digitalBackgroundLayerComponent1;
        private DevExpress.XtraEditors.LabelControl labelVersion;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btIPAdress;
        private DevExpress.XtraEditors.SimpleButton btConn;
        private DevExpress.XtraEditors.SimpleButton btIDSelect;
        private DevExpress.XtraEditors.SimpleButton btLanuchMode;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private ModGps modGpsCtrl;
        private ModSrv modSrvCtrl;
        private ModIMU modIMUCtrl;
        private ModDevPower modDevPowerSrv;
        private ModDevPower modDevPowerBatt;
        private ModDevPower modDevPowerCombin;
        private ModNav modNavCtrl;
        private ModFile modFileCtrl;
        private Mod3D mod3DCtrl;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private ModLanuch modLanuch;
        private ModPayload modPayload1;
        private ModDevPower modDevPowerFuse;
    }
}
