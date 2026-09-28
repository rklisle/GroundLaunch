namespace GroundLunch
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.navFrame = new DevExpress.XtraBars.Navigation.NavigationFrame();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.MenuHome = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSrv = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuData = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuMission = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuLink = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuDataLink = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.navFrame)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // navFrame
            // 
            this.navFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navFrame.Location = new System.Drawing.Point(0, 24);
            this.navFrame.Margin = new System.Windows.Forms.Padding(2);
            this.navFrame.Name = "navFrame";
            this.navFrame.SelectedPage = null;
            this.navFrame.Size = new System.Drawing.Size(1920, 966);
            this.navFrame.TabIndex = 4;
            this.navFrame.Text = "navigationFrame1";
            // 
            // MainMenu
            // 
            this.MainMenu.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHome,
            this.MenuFileUpload,
            this.MenuSrv,
            this.MenuData,
            this.StripMenuVideo,
            this.StripMenuMission,
            this.StripMenuLink,
            this.StripMenuDataLink});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.MainMenu.Size = new System.Drawing.Size(1920, 24);
            this.MainMenu.TabIndex = 5;
            this.MainMenu.Text = "menuStrip1";
            // 
            // MenuHome
            // 
            this.MenuHome.Name = "MenuHome";
            this.MenuHome.Size = new System.Drawing.Size(68, 22);
            this.MenuHome.Text = "发控首页";
            this.MenuHome.Click += new System.EventHandler(this.MenuHome_Click);
            // 
            // MenuFileUpload
            // 
            this.MenuFileUpload.Name = "MenuFileUpload";
            this.MenuFileUpload.Size = new System.Drawing.Size(68, 22);
            this.MenuFileUpload.Text = "诸元文件";
            this.MenuFileUpload.Click += new System.EventHandler(this.MenuFileUpload_Click);
            // 
            // MenuSrv
            // 
            this.MenuSrv.Name = "MenuSrv";
            this.MenuSrv.Size = new System.Drawing.Size(68, 22);
            this.MenuSrv.Text = "伺服时序";
            this.MenuSrv.Click += new System.EventHandler(this.MenuSrv_Click);
            // 
            // MenuData
            // 
            this.MenuData.Name = "MenuData";
            this.MenuData.Size = new System.Drawing.Size(68, 22);
            this.MenuData.Text = "数据回看";
            this.MenuData.Click += new System.EventHandler(this.MenuData_Click);
            // 
            // StripMenuVideo
            // 
            this.StripMenuVideo.Name = "StripMenuVideo";
            this.StripMenuVideo.Size = new System.Drawing.Size(80, 22);
            this.StripMenuVideo.Text = "摄像头视频";
            this.StripMenuVideo.Click += new System.EventHandler(this.StripMenuVideo_Click);
            // 
            // StripMenuMission
            // 
            this.StripMenuMission.Name = "StripMenuMission";
            this.StripMenuMission.Size = new System.Drawing.Size(68, 22);
            this.StripMenuMission.Text = "任务编辑";
            this.StripMenuMission.Click += new System.EventHandler(this.StripMenuMission_Click);
            // 
            // StripMenuLink
            // 
            this.StripMenuLink.Name = "StripMenuLink";
            this.StripMenuLink.Size = new System.Drawing.Size(80, 22);
            this.StripMenuLink.Text = "链路模拟器";
            this.StripMenuLink.Click += new System.EventHandler(this.StripMenuLink_Click);
            // 
            // StripMenuDataLink
            // 
            this.StripMenuDataLink.Name = "StripMenuDataLink";
            this.StripMenuDataLink.Size = new System.Drawing.Size(80, 22);
            this.StripMenuDataLink.Text = "数据链配置";
            this.StripMenuDataLink.Click += new System.EventHandler(this.StripMenuDataLink_Click);
            // 
            // MainForm
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1920, 990);
            this.Controls.Add(this.navFrame);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("MainForm.IconOptions.Image")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "MainForm";
            this.Text = "地面发控系统";
            ((System.ComponentModel.ISupportInitialize)(this.navFrame)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Navigation.NavigationFrame navFrame;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuHome;
        private System.Windows.Forms.ToolStripMenuItem MenuFileUpload;
        private System.Windows.Forms.ToolStripMenuItem MenuSrv;
        private System.Windows.Forms.ToolStripMenuItem MenuData;
        private System.Windows.Forms.ToolStripMenuItem StripMenuVideo;
        private System.Windows.Forms.ToolStripMenuItem StripMenuMission;
        private System.Windows.Forms.ToolStripMenuItem StripMenuLink;
        private System.Windows.Forms.ToolStripMenuItem StripMenuDataLink;
    }
}

