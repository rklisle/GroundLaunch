namespace GroundLunch
{
    partial class ModLanuch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModLanuch));
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.btUpEph = new DevExpress.XtraEditors.SimpleButton();
            this.labelIgnation = new DevExpress.XtraEditors.LabelControl();
            this.btIgnation = new DevExpress.XtraEditors.SimpleButton();
            this.btReady = new DevExpress.XtraEditors.SimpleButton();
            this.labelFuseState = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("groupControl2.CaptionImageOptions.SvgImage")));
            this.groupControl2.Controls.Add(this.labelFuseState);
            this.groupControl2.Controls.Add(this.btUpEph);
            this.groupControl2.Controls.Add(this.labelIgnation);
            this.groupControl2.Controls.Add(this.btIgnation);
            this.groupControl2.Controls.Add(this.btReady);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(534, 95);
            this.groupControl2.TabIndex = 25;
            this.groupControl2.Text = "发射";
            // 
            // btUpEph
            // 
            this.btUpEph.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btUpEph.Appearance.Options.UseFont = true;
            this.btUpEph.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btUpEph.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btUpEph.ImageOptions.SvgImage")));
            this.btUpEph.Location = new System.Drawing.Point(29, 41);
            this.btUpEph.Margin = new System.Windows.Forms.Padding(2);
            this.btUpEph.Name = "btUpEph";
            this.btUpEph.Size = new System.Drawing.Size(106, 43);
            this.btUpEph.TabIndex = 14;
            this.btUpEph.Text = "星历装订";
            this.btUpEph.Click += new System.EventHandler(this.btUpEph_Click);
            // 
            // labelIgnation
            // 
            this.labelIgnation.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.labelIgnation.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelIgnation.Appearance.Options.UseFont = true;
            this.labelIgnation.Appearance.Options.UseForeColor = true;
            this.labelIgnation.Location = new System.Drawing.Point(446, 39);
            this.labelIgnation.Name = "labelIgnation";
            this.labelIgnation.Size = new System.Drawing.Size(54, 24);
            this.labelIgnation.TabIndex = 13;
            this.labelIgnation.Text = "已发射";
            this.labelIgnation.Visible = false;
            this.labelIgnation.Click += new System.EventHandler(this.labelIgnation_Click);
            // 
            // btIgnation
            // 
            this.btIgnation.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btIgnation.Appearance.Options.UseFont = true;
            this.btIgnation.Enabled = false;
            this.btIgnation.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btIgnation.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btIgnation.ImageOptions.SvgImage")));
            this.btIgnation.Location = new System.Drawing.Point(290, 41);
            this.btIgnation.Margin = new System.Windows.Forms.Padding(2);
            this.btIgnation.Name = "btIgnation";
            this.btIgnation.Size = new System.Drawing.Size(128, 43);
            this.btIgnation.TabIndex = 12;
            this.btIgnation.Text = "发射";
            this.btIgnation.Click += new System.EventHandler(this.btIgnation_Click);
            // 
            // btReady
            // 
            this.btReady.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btReady.Appearance.Options.UseFont = true;
            this.btReady.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btReady.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btReady.ImageOptions.SvgImage")));
            this.btReady.Location = new System.Drawing.Point(152, 41);
            this.btReady.Margin = new System.Windows.Forms.Padding(2);
            this.btReady.Name = "btReady";
            this.btReady.Size = new System.Drawing.Size(128, 43);
            this.btReady.TabIndex = 11;
            this.btReady.Text = "预发射";
            this.btReady.Click += new System.EventHandler(this.btReady_Click);
            // 
            // labelFuseState
            // 
            this.labelFuseState.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.labelFuseState.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.labelFuseState.Appearance.Options.UseFont = true;
            this.labelFuseState.Appearance.Options.UseForeColor = true;
            this.labelFuseState.Location = new System.Drawing.Point(440, 62);
            this.labelFuseState.Name = "labelFuseState";
            this.labelFuseState.Size = new System.Drawing.Size(72, 24);
            this.labelFuseState.TabIndex = 15;
            this.labelFuseState.Text = "引信正常";
            this.labelFuseState.Visible = false;
            // 
            // ModLanuch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ModLanuch";
            this.Size = new System.Drawing.Size(534, 95);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btIgnation;
        private DevExpress.XtraEditors.SimpleButton btReady;
        private DevExpress.XtraEditors.LabelControl labelIgnation;
        private DevExpress.XtraEditors.SimpleButton btUpEph;
        private DevExpress.XtraEditors.LabelControl labelFuseState;
    }
}
