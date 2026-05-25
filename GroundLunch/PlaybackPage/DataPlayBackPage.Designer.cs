namespace GroundLunch
{
    partial class DataPlayBackPage
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
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataPlayBackPage));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.chartSrv = new DevExpress.XtraCharts.ChartControl();
            this.gridSelParam = new DevExpress.XtraGrid.GridControl();
            this.viewSelParam = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.checkListParams = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.treeListFile = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imagesFileFolder = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSrv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSelParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSelParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkListParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagesFileFolder)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.chartSrv);
            this.panelControl1.Controls.Add(this.gridSelParam);
            this.panelControl1.Controls.Add(this.checkListParams);
            this.panelControl1.Controls.Add(this.treeListFile);
            this.panelControl1.Location = new System.Drawing.Point(10, 28);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1900, 940);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("等线", 30F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(11, 69);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(160, 41);
            this.labelControl1.TabIndex = 40;
            this.labelControl1.Text = "数据回看";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("等线", 30F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(11, 16);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(261, 41);
            this.labelControl3.TabIndex = 39;
            this.labelControl3.Text = "PJ60-Y1巡飞器";
            // 
            // chartSrv
            // 
            xyDiagram1.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartSrv.Diagram = xyDiagram1;
            this.chartSrv.Location = new System.Drawing.Point(381, 550);
            this.chartSrv.Margin = new System.Windows.Forms.Padding(2);
            this.chartSrv.Name = "chartSrv";
            series1.Name = "Series 1";
            series1.View = lineSeriesView1;
            this.chartSrv.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartSrv.Size = new System.Drawing.Size(1072, 380);
            this.chartSrv.TabIndex = 30;
            // 
            // gridSelParam
            // 
            this.gridSelParam.Location = new System.Drawing.Point(381, 19);
            this.gridSelParam.MainView = this.viewSelParam;
            this.gridSelParam.Name = "gridSelParam";
            this.gridSelParam.Size = new System.Drawing.Size(1499, 514);
            this.gridSelParam.TabIndex = 2;
            this.gridSelParam.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewSelParam});
            // 
            // viewSelParam
            // 
            this.viewSelParam.DetailHeight = 375;
            this.viewSelParam.GridControl = this.gridSelParam;
            this.viewSelParam.Name = "viewSelParam";
            this.viewSelParam.OptionsCustomization.AllowFilter = false;
            this.viewSelParam.OptionsCustomization.AllowSort = false;
            this.viewSelParam.OptionsFilter.AllowFilterEditor = false;
            this.viewSelParam.OptionsPrint.AutoWidth = false;
            this.viewSelParam.OptionsView.ColumnAutoWidth = false;
            this.viewSelParam.OptionsView.ShowGroupPanel = false;
            // 
            // checkListParams
            // 
            this.checkListParams.Location = new System.Drawing.Point(11, 550);
            this.checkListParams.Name = "checkListParams";
            this.checkListParams.Size = new System.Drawing.Size(352, 380);
            this.checkListParams.TabIndex = 1;
            this.checkListParams.SelectedIndexChanged += new System.EventHandler(this.checkListParams_SelectedIndexChanged);
            // 
            // treeListFile
            // 
            this.treeListFile.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 12F);
            this.treeListFile.Appearance.Row.Options.UseFont = true;
            this.treeListFile.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeListFile.CustomizationFormBounds = new System.Drawing.Rectangle(1656, 735, 264, 297);
            this.treeListFile.Location = new System.Drawing.Point(11, 130);
            this.treeListFile.Name = "treeListFile";
            this.treeListFile.OptionsBehavior.Editable = false;
            this.treeListFile.OptionsView.ShowIndicator = false;
            this.treeListFile.Size = new System.Drawing.Size(352, 404);
            this.treeListFile.StateImageList = this.imagesFileFolder;
            this.treeListFile.TabIndex = 0;
            this.treeListFile.RowCellClick += new DevExpress.XtraTreeList.RowCellClickEventHandler(this.treeListFile_RowCellClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "treeListColumn1";
            this.treeListColumn1.FieldName = "treeListColumn1";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // imagesFileFolder
            // 
            this.imagesFileFolder.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imagesFileFolder.ImageStream")));
            this.imagesFileFolder.Images.SetKeyName(0, "open_32x32.png");
            this.imagesFileFolder.Images.SetKeyName(1, "exporttoxls_32x32.png");
            this.imagesFileFolder.Images.SetKeyName(2, "refresh_32x32.png");
            // 
            // DataPlayBackPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DataPlayBackPage";
            this.Size = new System.Drawing.Size(1920, 980);
            this.Load += new System.EventHandler(this.DataPlayBackPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSrv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSelParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSelParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkListParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagesFileFolder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridSelParam;
        private DevExpress.XtraGrid.Views.Grid.GridView viewSelParam;
        private DevExpress.XtraEditors.CheckedListBoxControl checkListParams;
        private DevExpress.XtraTreeList.TreeList treeListFile;
        private DevExpress.XtraCharts.ChartControl chartSrv;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.Utils.ImageCollection imagesFileFolder;
    }
}
