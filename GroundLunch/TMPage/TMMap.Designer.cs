namespace GroundLunch
{
    partial class TMMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TMMap));
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.ScatterLineSeriesView scatterLineSeriesView1 = new DevExpress.XtraCharts.ScatterLineSeriesView();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.chartMap = new DevExpress.XtraCharts.ChartControl();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(scatterLineSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("groupControl2.CaptionImageOptions.SvgImage")));
            this.groupControl2.Controls.Add(this.chartMap);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(801, 545);
            this.groupControl2.TabIndex = 42;
            this.groupControl2.Text = "飞行地图";
            // 
            // chartMap
            // 
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartMap.Diagram = xyDiagram1;
            this.chartMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMap.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartMap.Location = new System.Drawing.Point(2, 33);
            this.chartMap.Name = "chartMap";
            series1.Name = "Series 1";
            series1.View = scatterLineSeriesView1;
            this.chartMap.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartMap.Size = new System.Drawing.Size(797, 510);
            this.chartMap.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "飞机1.png");
            this.imageList.Images.SetKeyName(1, "飞机2.png");
            this.imageList.Images.SetKeyName(2, "飞机3.png");
            this.imageList.Images.SetKeyName(3, "飞机4.png");
            this.imageList.Images.SetKeyName(4, "飞机5.png");
            this.imageList.Images.SetKeyName(5, "飞机6.png");
            // 
            // TMMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TMMap";
            this.Size = new System.Drawing.Size(801, 545);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(scatterLineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraCharts.ChartControl chartMap;
        private System.Windows.Forms.ImageList imageList;
    }
}
