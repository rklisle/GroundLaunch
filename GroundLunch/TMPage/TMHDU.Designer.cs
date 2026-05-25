namespace GroundLunch
{
    partial class TMHDU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TMHDU));
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.openGLHDU = new SharpGL.OpenGLControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.openGLHDU)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl3
            // 
            this.groupControl3.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("groupControl3.CaptionImageOptions.SvgImage")));
            this.groupControl3.Controls.Add(this.openGLHDU);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(525, 495);
            this.groupControl3.TabIndex = 43;
            this.groupControl3.Text = "HDU";
            // 
            // openGLHDU
            // 
            this.openGLHDU.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLHDU.DrawFPS = false;
            this.openGLHDU.Location = new System.Drawing.Point(2, 33);
            this.openGLHDU.Margin = new System.Windows.Forms.Padding(4);
            this.openGLHDU.Name = "openGLHDU";
            this.openGLHDU.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLHDU.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLHDU.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLHDU.Size = new System.Drawing.Size(521, 460);
            this.openGLHDU.TabIndex = 1;
            this.openGLHDU.OpenGLInitialized += new System.EventHandler(this.openGLHDU_OpenGLInitialized);
            this.openGLHDU.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLHDU_OpenGLDraw);
            this.openGLHDU.Resized += new System.EventHandler(this.openGLHDU_Resized);
            // 
            // TMHDU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl3);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TMHDU";
            this.Size = new System.Drawing.Size(525, 495);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.openGLHDU)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl3;
        private SharpGL.OpenGLControl openGLHDU;
    }
}
