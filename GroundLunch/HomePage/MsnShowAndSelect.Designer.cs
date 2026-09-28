namespace GroundLunch
{
    partial class MsnShowAndSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsnShowAndSelect));
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.ScatterLineSeriesView scatterLineSeriesView1 = new DevExpress.XtraCharts.ScatterLineSeriesView();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelTips = new DevExpress.XtraEditors.LabelControl();
            this.gridMsnGroup = new DevExpress.XtraGrid.GridControl();
            this.viewGroupList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btUploadMsn = new DevExpress.XtraEditors.SimpleButton();
            this.chartMap = new DevExpress.XtraCharts.ChartControl();
            this.gridMsnList = new DevExpress.XtraGrid.GridControl();
            this.viewMsnList = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMsnGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewGroupList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(scatterLineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMsnList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewMsnList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupControl1.CaptionImageOptions.Image")));
            this.groupControl1.Controls.Add(this.labelTips);
            this.groupControl1.Controls.Add(this.gridMsnGroup);
            this.groupControl1.Controls.Add(this.btUploadMsn);
            this.groupControl1.Controls.Add(this.chartMap);
            this.groupControl1.Controls.Add(this.gridMsnList);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(450, 800);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "任务选择 ( Mission Select )";
            // 
            // labelTips
            // 
            this.labelTips.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelTips.Appearance.ForeColor = System.Drawing.Color.Orange;
            this.labelTips.Appearance.Options.UseFont = true;
            this.labelTips.Appearance.Options.UseForeColor = true;
            this.labelTips.Location = new System.Drawing.Point(10, 766);
            this.labelTips.Name = "labelTips";
            this.labelTips.Size = new System.Drawing.Size(224, 21);
            this.labelTips.TabIndex = 4;
            this.labelTips.Text = "已就绪飞机数量不满足当前任务";
            // 
            // gridMsnGroup
            // 
            this.gridMsnGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMsnGroup.Location = new System.Drawing.Point(5, 586);
            this.gridMsnGroup.MainView = this.viewGroupList;
            this.gridMsnGroup.Name = "gridMsnGroup";
            this.gridMsnGroup.Size = new System.Drawing.Size(440, 167);
            this.gridMsnGroup.TabIndex = 3;
            this.gridMsnGroup.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewGroupList});
            // 
            // viewGroupList
            // 
            this.viewGroupList.Appearance.GroupPanel.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.viewGroupList.Appearance.GroupPanel.Options.UseFont = true;
            this.viewGroupList.Appearance.HeaderPanel.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.viewGroupList.Appearance.HeaderPanel.Options.UseFont = true;
            this.viewGroupList.Appearance.Row.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.viewGroupList.Appearance.Row.Options.UseFont = true;
            this.viewGroupList.GridControl = this.gridMsnGroup;
            this.viewGroupList.Name = "viewGroupList";
            this.viewGroupList.OptionsBehavior.Editable = false;
            this.viewGroupList.OptionsCustomization.AllowFilter = false;
            this.viewGroupList.OptionsCustomization.AllowSort = false;
            this.viewGroupList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.viewGroupList.OptionsView.EnableAppearanceEvenRow = true;
            this.viewGroupList.OptionsView.EnableAppearanceOddRow = true;
            this.viewGroupList.OptionsView.ShowGroupPanel = false;
            this.viewGroupList.OptionsView.ShowIndicator = false;
            // 
            // btUploadMsn
            // 
            this.btUploadMsn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btUploadMsn.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btUploadMsn.Appearance.Options.UseFont = true;
            this.btUploadMsn.Enabled = false;
            this.btUploadMsn.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btUploadMsn.ImageOptions.Image")));
            this.btUploadMsn.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btUploadMsn.Location = new System.Drawing.Point(248, 760);
            this.btUploadMsn.Name = "btUploadMsn";
            this.btUploadMsn.Size = new System.Drawing.Size(196, 32);
            this.btUploadMsn.TabIndex = 2;
            this.btUploadMsn.Text = "加载任务 ( UpLoad )";
            this.btUploadMsn.Click += new System.EventHandler(this.btUploadMsn_Click);
            // 
            // chartMap
            // 
            this.chartMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartMap.Diagram = xyDiagram1;
            this.chartMap.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartMap.Location = new System.Drawing.Point(5, 169);
            this.chartMap.Name = "chartMap";
            series1.Name = "Series 1";
            series1.View = scatterLineSeriesView1;
            this.chartMap.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartMap.Size = new System.Drawing.Size(440, 413);
            this.chartMap.TabIndex = 1;
            // 
            // gridMsnList
            // 
            this.gridMsnList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMsnList.Location = new System.Drawing.Point(5, 36);
            this.gridMsnList.MainView = this.viewMsnList;
            this.gridMsnList.Name = "gridMsnList";
            this.gridMsnList.Size = new System.Drawing.Size(440, 127);
            this.gridMsnList.TabIndex = 0;
            this.gridMsnList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewMsnList});
            // 
            // viewMsnList
            // 
            this.viewMsnList.Appearance.HeaderPanel.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.viewMsnList.Appearance.HeaderPanel.Options.UseFont = true;
            this.viewMsnList.Appearance.Row.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.viewMsnList.Appearance.Row.Options.UseFont = true;
            this.viewMsnList.GridControl = this.gridMsnList;
            this.viewMsnList.Name = "viewMsnList";
            this.viewMsnList.OptionsBehavior.Editable = false;
            this.viewMsnList.OptionsCustomization.AllowFilter = false;
            this.viewMsnList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.viewMsnList.OptionsView.EnableAppearanceEvenRow = true;
            this.viewMsnList.OptionsView.EnableAppearanceOddRow = true;
            this.viewMsnList.OptionsView.ShowGroupPanel = false;
            this.viewMsnList.OptionsView.ShowIndicator = false;
            this.viewMsnList.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.viewMsnList_RowCellClick);
            // 
            // MsnShowAndSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MsnShowAndSelect";
            this.Size = new System.Drawing.Size(450, 800);
            this.Load += new System.EventHandler(this.MsnShowAndSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMsnGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewGroupList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(scatterLineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMsnList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewMsnList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl gridMsnList;
        private DevExpress.XtraGrid.Views.Grid.GridView viewMsnList;
        private DevExpress.XtraEditors.SimpleButton btUploadMsn;
        private DevExpress.XtraCharts.ChartControl chartMap;
        private DevExpress.XtraGrid.GridControl gridMsnGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView viewGroupList;
        private DevExpress.XtraEditors.LabelControl labelTips;
    }
}
