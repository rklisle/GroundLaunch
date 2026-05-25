namespace GroundLunch
{
    partial class ModDevPower
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY1 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModDevPower));
            this.chartDevPower = new DevExpress.XtraCharts.ChartControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelCurrent = new DevExpress.XtraEditors.LabelControl();
            this.labelVoltage = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelName = new DevExpress.XtraEditors.LabelControl();
            this.btPowerOn = new DevExpress.XtraEditors.SimpleButton();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chartDevPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartDevPower
            // 
            xyDiagram1.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            secondaryAxisY1.AxisID = 0;
            secondaryAxisY1.Label.Visible = false;
            secondaryAxisY1.Name = "Secondary AxisY 1";
            secondaryAxisY1.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            secondaryAxisY1.VisibleInPanesSerializable = "-1";
            xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY1});
            this.chartDevPower.Diagram = xyDiagram1;
            this.chartDevPower.Location = new System.Drawing.Point(204, 33);
            this.chartDevPower.Margin = new System.Windows.Forms.Padding(2);
            this.chartDevPower.Name = "chartDevPower";
            series1.Name = "Series 1";
            series1.View = lineSeriesView1;
            this.chartDevPower.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartDevPower.Size = new System.Drawing.Size(313, 99);
            this.chartDevPower.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("groupControl1.CaptionImageOptions.SvgImage")));
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelCurrent);
            this.groupControl1.Controls.Add(this.labelVoltage);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelName);
            this.groupControl1.Controls.Add(this.btPowerOn);
            this.groupControl1.Controls.Add(this.chartDevPower);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(528, 137);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "设备供电状态";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(123, 104);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(11, 21);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "A";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(123, 74);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(11, 21);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "V";
            // 
            // labelCurrent
            // 
            this.labelCurrent.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelCurrent.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelCurrent.Appearance.Options.UseFont = true;
            this.labelCurrent.Appearance.Options.UseForeColor = true;
            this.labelCurrent.Location = new System.Drawing.Point(74, 104);
            this.labelCurrent.Margin = new System.Windows.Forms.Padding(2);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(22, 21);
            this.labelCurrent.TabIndex = 7;
            this.labelCurrent.Text = "1.8";
            // 
            // labelVoltage
            // 
            this.labelVoltage.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelVoltage.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelVoltage.Appearance.Options.UseFont = true;
            this.labelVoltage.Appearance.Options.UseForeColor = true;
            this.labelVoltage.Location = new System.Drawing.Point(74, 74);
            this.labelVoltage.Margin = new System.Windows.Forms.Padding(2);
            this.labelVoltage.Name = "labelVoltage";
            this.labelVoltage.Size = new System.Drawing.Size(31, 21);
            this.labelVoltage.TabIndex = 6;
            this.labelVoltage.Text = "28.4";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(24, 104);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 21);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "电流:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(24, 74);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 21);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "电压:";
            // 
            // labelName
            // 
            this.labelName.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.labelName.Appearance.Options.UseFont = true;
            this.labelName.Location = new System.Drawing.Point(24, 43);
            this.labelName.Margin = new System.Windows.Forms.Padding(2);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(64, 22);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "合路供电";
            // 
            // btPowerOn
            // 
            this.btPowerOn.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btPowerOn.Appearance.Options.UseFont = true;
            this.btPowerOn.ImageOptions.ImageIndex = 0;
            this.btPowerOn.ImageOptions.ImageList = this.imageCollection1;
            this.btPowerOn.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btPowerOn.Location = new System.Drawing.Point(111, 36);
            this.btPowerOn.Margin = new System.Windows.Forms.Padding(2);
            this.btPowerOn.Name = "btPowerOn";
            this.btPowerOn.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btPowerOn.Size = new System.Drawing.Size(78, 36);
            this.btPowerOn.TabIndex = 1;
            this.btPowerOn.Text = "上电";
            this.btPowerOn.Click += new System.EventHandler(this.btPowerOn_Click);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "b_32x32.png");
            this.imageCollection1.Images.SetKeyName(1, "a_32x32.png");
            // 
            // ModDevPower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "ModDevPower";
            this.Size = new System.Drawing.Size(528, 137);
            this.Load += new System.EventHandler(this.ModDevPower_Load);
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDevPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartDevPower;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelCurrent;
        private DevExpress.XtraEditors.LabelControl labelVoltage;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelName;
        private DevExpress.XtraEditors.SimpleButton btPowerOn;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.Utils.ImageCollection imageCollection1;
    }
}
