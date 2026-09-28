namespace GroundLunch
{
    partial class ModGps
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModGps));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.picLocationCur = new DevExpress.XtraEditors.PictureEdit();
            this.picLocationLanuch = new DevExpress.XtraEditors.PictureEdit();
            this.svgImage = new DevExpress.XtraEditors.SvgImageBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btGetEphBD = new DevExpress.XtraEditors.SimpleButton();
            this.btGetEphGps = new DevExpress.XtraEditors.SimpleButton();
            this.labelGPSDate = new DevExpress.XtraEditors.LabelControl();
            this.labelVe = new DevExpress.XtraEditors.LabelControl();
            this.labelVs = new DevExpress.XtraEditors.LabelControl();
            this.labelVn = new DevExpress.XtraEditors.LabelControl();
            this.labelGDOP = new DevExpress.XtraEditors.LabelControl();
            this.labelPDOP = new DevExpress.XtraEditors.LabelControl();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelSatelliteCount = new DevExpress.XtraEditors.LabelControl();
            this.labelGpsState = new DevExpress.XtraEditors.LabelControl();
            this.labelGpsHigh = new DevExpress.XtraEditors.LabelControl();
            this.labelGpsLat = new DevExpress.XtraEditors.LabelControl();
            this.labelGpsLon = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLocationCur.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLocationLanuch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.picLocationCur);
            this.panelControl1.Controls.Add(this.picLocationLanuch);
            this.panelControl1.Controls.Add(this.svgImage);
            this.panelControl1.Location = new System.Drawing.Point(186, 47);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(274, 199);
            this.panelControl1.TabIndex = 1;
            // 
            // picLocationCur
            // 
            this.picLocationCur.EditValue = ((object)(resources.GetObject("picLocationCur.EditValue")));
            this.picLocationCur.Location = new System.Drawing.Point(5, 5);
            this.picLocationCur.Name = "picLocationCur";
            this.picLocationCur.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picLocationCur.Properties.Appearance.Options.UseBackColor = true;
            this.picLocationCur.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picLocationCur.Properties.Caption.Alignment = System.Drawing.ContentAlignment.TopCenter;
            this.picLocationCur.Properties.Caption.Text = "当前位置";
            this.picLocationCur.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picLocationCur.Size = new System.Drawing.Size(83, 107);
            this.picLocationCur.TabIndex = 1;
            // 
            // picLocationLanuch
            // 
            this.picLocationLanuch.EditValue = ((object)(resources.GetObject("picLocationLanuch.EditValue")));
            this.picLocationLanuch.Location = new System.Drawing.Point(7, 5);
            this.picLocationLanuch.Name = "picLocationLanuch";
            this.picLocationLanuch.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picLocationLanuch.Properties.Appearance.Options.UseBackColor = true;
            this.picLocationLanuch.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picLocationLanuch.Properties.Caption.Alignment = System.Drawing.ContentAlignment.TopCenter;
            this.picLocationLanuch.Properties.Caption.Offset = new System.Drawing.Point(10, 98);
            this.picLocationLanuch.Properties.Caption.Text = "发射点";
            this.picLocationLanuch.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picLocationLanuch.Size = new System.Drawing.Size(362, 296);
            this.picLocationLanuch.TabIndex = 2;
            this.picLocationLanuch.Visible = false;
            // 
            // svgImage
            // 
            this.svgImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.svgImage.Location = new System.Drawing.Point(2, 2);
            this.svgImage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.svgImage.Name = "svgImage";
            this.svgImage.Size = new System.Drawing.Size(270, 195);
            this.svgImage.SizeMode = DevExpress.XtraEditors.SvgImageSizeMode.Zoom;
            this.svgImage.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImage.SvgImage")));
            this.svgImage.TabIndex = 0;
            this.svgImage.Text = "svgImageBox1";
            // 
            // groupControl1
            // 
            this.groupControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupControl1.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("groupControl1.CaptionImageOptions.SvgImage")));
            this.groupControl1.Controls.Add(this.btGetEphBD);
            this.groupControl1.Controls.Add(this.btGetEphGps);
            this.groupControl1.Controls.Add(this.labelGPSDate);
            this.groupControl1.Controls.Add(this.labelVe);
            this.groupControl1.Controls.Add(this.labelVs);
            this.groupControl1.Controls.Add(this.labelVn);
            this.groupControl1.Controls.Add(this.labelGDOP);
            this.groupControl1.Controls.Add(this.labelPDOP);
            this.groupControl1.Controls.Add(this.labelControl16);
            this.groupControl1.Controls.Add(this.labelControl15);
            this.groupControl1.Controls.Add(this.labelControl14);
            this.groupControl1.Controls.Add(this.labelControl13);
            this.groupControl1.Controls.Add(this.labelControl12);
            this.groupControl1.Controls.Add(this.labelControl11);
            this.groupControl1.Controls.Add(this.labelSatelliteCount);
            this.groupControl1.Controls.Add(this.labelGpsState);
            this.groupControl1.Controls.Add(this.labelGpsHigh);
            this.groupControl1.Controls.Add(this.labelGpsLat);
            this.groupControl1.Controls.Add(this.labelGpsLon);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(476, 316);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "GPS数据";
            // 
            // btGetEphBD
            // 
            this.btGetEphBD.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btGetEphBD.Appearance.Options.UseFont = true;
            this.btGetEphBD.Location = new System.Drawing.Point(358, 3);
            this.btGetEphBD.Name = "btGetEphBD";
            this.btGetEphBD.Size = new System.Drawing.Size(100, 27);
            this.btGetEphBD.TabIndex = 25;
            this.btGetEphBD.Text = "星历提取BD";
            this.btGetEphBD.Click += new System.EventHandler(this.btGetEphBD_Click);
            // 
            // btGetEphGps
            // 
            this.btGetEphGps.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btGetEphGps.Appearance.Options.UseFont = true;
            this.btGetEphGps.Location = new System.Drawing.Point(253, 3);
            this.btGetEphGps.Name = "btGetEphGps";
            this.btGetEphGps.Size = new System.Drawing.Size(100, 27);
            this.btGetEphGps.TabIndex = 24;
            this.btGetEphGps.Text = "星历提取GPS";
            this.btGetEphGps.Click += new System.EventHandler(this.btGetEphGps_Click);
            // 
            // labelGPSDate
            // 
            this.labelGPSDate.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelGPSDate.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelGPSDate.Appearance.Options.UseFont = true;
            this.labelGPSDate.Appearance.Options.UseForeColor = true;
            this.labelGPSDate.Location = new System.Drawing.Point(267, 280);
            this.labelGPSDate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelGPSDate.Name = "labelGPSDate";
            this.labelGPSDate.Size = new System.Drawing.Size(144, 21);
            this.labelGPSDate.TabIndex = 23;
            this.labelGPSDate.Text = "2024-3-12 15:17:02";
            // 
            // labelVe
            // 
            this.labelVe.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelVe.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelVe.Appearance.Options.UseFont = true;
            this.labelVe.Appearance.Options.UseForeColor = true;
            this.labelVe.Location = new System.Drawing.Point(93, 280);
            this.labelVe.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelVe.Name = "labelVe";
            this.labelVe.Size = new System.Drawing.Size(22, 21);
            this.labelVe.TabIndex = 22;
            this.labelVe.Text = "0.2";
            // 
            // labelVs
            // 
            this.labelVs.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelVs.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelVs.Appearance.Options.UseFont = true;
            this.labelVs.Appearance.Options.UseForeColor = true;
            this.labelVs.Location = new System.Drawing.Point(93, 246);
            this.labelVs.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelVs.Name = "labelVs";
            this.labelVs.Size = new System.Drawing.Size(22, 21);
            this.labelVs.TabIndex = 21;
            this.labelVs.Text = "0.5";
            // 
            // labelVn
            // 
            this.labelVn.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelVn.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelVn.Appearance.Options.UseFont = true;
            this.labelVn.Appearance.Options.UseForeColor = true;
            this.labelVn.Location = new System.Drawing.Point(93, 213);
            this.labelVn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelVn.Name = "labelVn";
            this.labelVn.Size = new System.Drawing.Size(22, 21);
            this.labelVn.TabIndex = 20;
            this.labelVn.Text = "0.1";
            // 
            // labelGDOP
            // 
            this.labelGDOP.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelGDOP.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelGDOP.Appearance.Options.UseFont = true;
            this.labelGDOP.Appearance.Options.UseForeColor = true;
            this.labelGDOP.Location = new System.Drawing.Point(388, 250);
            this.labelGDOP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelGDOP.Name = "labelGDOP";
            this.labelGDOP.Size = new System.Drawing.Size(31, 21);
            this.labelGDOP.TabIndex = 19;
            this.labelGDOP.Text = "1.75";
            // 
            // labelPDOP
            // 
            this.labelPDOP.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelPDOP.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelPDOP.Appearance.Options.UseFont = true;
            this.labelPDOP.Appearance.Options.UseForeColor = true;
            this.labelPDOP.Location = new System.Drawing.Point(267, 250);
            this.labelPDOP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelPDOP.Name = "labelPDOP";
            this.labelPDOP.Size = new System.Drawing.Size(22, 21);
            this.labelPDOP.TabIndex = 18;
            this.labelPDOP.Text = "2.3";
            // 
            // labelControl16
            // 
            this.labelControl16.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl16.Appearance.Options.UseFont = true;
            this.labelControl16.Location = new System.Drawing.Point(186, 280);
            this.labelControl16.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(78, 21);
            this.labelControl16.TabIndex = 17;
            this.labelControl16.Text = "GPS时间：";
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl15.Appearance.Options.UseFont = true;
            this.labelControl15.Location = new System.Drawing.Point(319, 250);
            this.labelControl15.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(62, 21);
            this.labelControl15.TabIndex = 16;
            this.labelControl15.Text = "GDOP：";
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl14.Appearance.Options.UseFont = true;
            this.labelControl14.Location = new System.Drawing.Point(186, 250);
            this.labelControl14.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(59, 21);
            this.labelControl14.TabIndex = 15;
            this.labelControl14.Text = "PDOP：";
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl13.Appearance.Options.UseFont = true;
            this.labelControl13.Location = new System.Drawing.Point(13, 279);
            this.labelControl13.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(80, 21);
            this.labelControl13.TabIndex = 14;
            this.labelControl13.Text = "东向速度：";
            // 
            // labelControl12
            // 
            this.labelControl12.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl12.Appearance.Options.UseFont = true;
            this.labelControl12.Location = new System.Drawing.Point(13, 246);
            this.labelControl12.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(80, 21);
            this.labelControl12.TabIndex = 13;
            this.labelControl12.Text = "天向速度：";
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl11.Appearance.Options.UseFont = true;
            this.labelControl11.Location = new System.Drawing.Point(13, 213);
            this.labelControl11.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(80, 21);
            this.labelControl11.TabIndex = 12;
            this.labelControl11.Text = "北向速度：";
            // 
            // labelSatelliteCount
            // 
            this.labelSatelliteCount.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSatelliteCount.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelSatelliteCount.Appearance.Options.UseFont = true;
            this.labelSatelliteCount.Appearance.Options.UseForeColor = true;
            this.labelSatelliteCount.Location = new System.Drawing.Point(93, 180);
            this.labelSatelliteCount.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelSatelliteCount.Name = "labelSatelliteCount";
            this.labelSatelliteCount.Size = new System.Drawing.Size(9, 21);
            this.labelSatelliteCount.TabIndex = 11;
            this.labelSatelliteCount.Text = "8";
            // 
            // labelGpsState
            // 
            this.labelGpsState.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelGpsState.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelGpsState.Appearance.Options.UseFont = true;
            this.labelGpsState.Appearance.Options.UseForeColor = true;
            this.labelGpsState.Location = new System.Drawing.Point(93, 147);
            this.labelGpsState.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelGpsState.Name = "labelGpsState";
            this.labelGpsState.Size = new System.Drawing.Size(48, 21);
            this.labelGpsState.TabIndex = 10;
            this.labelGpsState.Text = "已定位";
            // 
            // labelGpsHigh
            // 
            this.labelGpsHigh.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelGpsHigh.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelGpsHigh.Appearance.Options.UseFont = true;
            this.labelGpsHigh.Appearance.Options.UseForeColor = true;
            this.labelGpsHigh.Location = new System.Drawing.Point(93, 114);
            this.labelGpsHigh.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelGpsHigh.Name = "labelGpsHigh";
            this.labelGpsHigh.Size = new System.Drawing.Size(18, 21);
            this.labelGpsHigh.TabIndex = 9;
            this.labelGpsHigh.Text = "27";
            // 
            // labelGpsLat
            // 
            this.labelGpsLat.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelGpsLat.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelGpsLat.Appearance.Options.UseFont = true;
            this.labelGpsLat.Appearance.Options.UseForeColor = true;
            this.labelGpsLat.Location = new System.Drawing.Point(93, 81);
            this.labelGpsLat.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelGpsLat.Name = "labelGpsLat";
            this.labelGpsLat.Size = new System.Drawing.Size(58, 21);
            this.labelGpsLat.TabIndex = 8;
            this.labelGpsLat.Text = "39.3634";
            // 
            // labelGpsLon
            // 
            this.labelGpsLon.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelGpsLon.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelGpsLon.Appearance.Options.UseFont = true;
            this.labelGpsLon.Appearance.Options.UseForeColor = true;
            this.labelGpsLon.Location = new System.Drawing.Point(93, 48);
            this.labelGpsLon.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelGpsLon.Name = "labelGpsLon";
            this.labelGpsLon.Size = new System.Drawing.Size(67, 21);
            this.labelGpsLon.TabIndex = 7;
            this.labelGpsLon.Text = "116.2145";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(13, 180);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(80, 21);
            this.labelControl5.TabIndex = 6;
            this.labelControl5.Text = "定位星数：";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(13, 147);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(80, 21);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "定位状态：";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(13, 114);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(78, 21);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "GPS高度：";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(13, 81);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(78, 21);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "GPS纬度：";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(13, 48);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(78, 21);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "GPS经度：";
            // 
            // ModGps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "ModGps";
            this.Size = new System.Drawing.Size(476, 316);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLocationCur.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLocationLanuch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SvgImageBox svgImage;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelSatelliteCount;
        private DevExpress.XtraEditors.LabelControl labelGpsState;
        private DevExpress.XtraEditors.LabelControl labelGpsHigh;
        private DevExpress.XtraEditors.LabelControl labelGpsLat;
        private DevExpress.XtraEditors.LabelControl labelGpsLon;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelGPSDate;
        private DevExpress.XtraEditors.LabelControl labelVe;
        private DevExpress.XtraEditors.LabelControl labelVs;
        private DevExpress.XtraEditors.LabelControl labelVn;
        private DevExpress.XtraEditors.LabelControl labelGDOP;
        private DevExpress.XtraEditors.LabelControl labelPDOP;
        private DevExpress.XtraEditors.PictureEdit picLocationCur;
        private DevExpress.XtraEditors.PictureEdit picLocationLanuch;
        private DevExpress.XtraEditors.SimpleButton btGetEphGps;
        private DevExpress.XtraEditors.SimpleButton btGetEphBD;
    }
}
