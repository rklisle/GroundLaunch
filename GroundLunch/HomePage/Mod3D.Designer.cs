namespace GroundLunch
{
    partial class Mod3D
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mod3D));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btRoll = new DevExpress.XtraEditors.SimpleButton();
            this.btPitch = new DevExpress.XtraEditors.SimpleButton();
            this.btYaw = new DevExpress.XtraEditors.SimpleButton();
            this.bt3D = new DevExpress.XtraEditors.SimpleButton();
            this.openGLCtrl = new SharpGL.OpenGLControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.openGLCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("groupControl1.CaptionImageOptions.SvgImage")));
            this.groupControl1.Controls.Add(this.btRoll);
            this.groupControl1.Controls.Add(this.btPitch);
            this.groupControl1.Controls.Add(this.btYaw);
            this.groupControl1.Controls.Add(this.bt3D);
            this.groupControl1.Controls.Add(this.openGLCtrl);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1104, 762);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "模拟三维模型";
            // 
            // btRoll
            // 
            this.btRoll.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btRoll.Appearance.Options.UseFont = true;
            this.btRoll.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btRoll.Location = new System.Drawing.Point(217, 57);
            this.btRoll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btRoll.Name = "btRoll";
            this.btRoll.Size = new System.Drawing.Size(61, 63);
            this.btRoll.TabIndex = 4;
            this.btRoll.Text = "后视";
            this.btRoll.Click += new System.EventHandler(this.btRoll_Click);
            // 
            // btPitch
            // 
            this.btPitch.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btPitch.Appearance.Options.UseFont = true;
            this.btPitch.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btPitch.Location = new System.Drawing.Point(147, 57);
            this.btPitch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btPitch.Name = "btPitch";
            this.btPitch.Size = new System.Drawing.Size(61, 63);
            this.btPitch.TabIndex = 3;
            this.btPitch.Text = "侧视";
            this.btPitch.Click += new System.EventHandler(this.btPitch_Click);
            // 
            // btYaw
            // 
            this.btYaw.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btYaw.Appearance.Options.UseFont = true;
            this.btYaw.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btYaw.Location = new System.Drawing.Point(77, 57);
            this.btYaw.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btYaw.Name = "btYaw";
            this.btYaw.Size = new System.Drawing.Size(61, 63);
            this.btYaw.TabIndex = 2;
            this.btYaw.Text = "俯视";
            this.btYaw.Click += new System.EventHandler(this.btYaw_Click);
            // 
            // bt3D
            // 
            this.bt3D.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.bt3D.Appearance.Options.UseFont = true;
            this.bt3D.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.bt3D.Location = new System.Drawing.Point(7, 57);
            this.bt3D.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bt3D.Name = "bt3D";
            this.bt3D.Size = new System.Drawing.Size(61, 63);
            this.bt3D.TabIndex = 1;
            this.bt3D.Text = "3D";
            this.bt3D.Click += new System.EventHandler(this.bt3D_Click);
            // 
            // openGLCtrl
            // 
            this.openGLCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLCtrl.DrawFPS = false;
            this.openGLCtrl.Location = new System.Drawing.Point(2, 49);
            this.openGLCtrl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.openGLCtrl.Name = "openGLCtrl";
            this.openGLCtrl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLCtrl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLCtrl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLCtrl.Size = new System.Drawing.Size(1100, 711);
            this.openGLCtrl.TabIndex = 0;
            this.openGLCtrl.OpenGLInitialized += new System.EventHandler(this.openGLCtrl_OpenGLInitialized);
            this.openGLCtrl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLCtrl_OpenGLDraw);
            this.openGLCtrl.Resized += new System.EventHandler(this.openGLCtrl_Resized);
            // 
            // Mod3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "Mod3D";
            this.Size = new System.Drawing.Size(1104, 762);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.openGLCtrl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private SharpGL.OpenGLControl openGLCtrl;
        private DevExpress.XtraEditors.SimpleButton bt3D;
        private DevExpress.XtraEditors.SimpleButton btPitch;
        private DevExpress.XtraEditors.SimpleButton btYaw;
        private DevExpress.XtraEditors.SimpleButton btRoll;
    }
}
