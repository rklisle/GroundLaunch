namespace GroundLunch
{
    partial class ModPayload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModPayload));
            this.textBatteryStatus = new DevExpress.XtraEditors.GroupControl();
            this.labelMsn = new DevExpress.XtraEditors.LabelControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.labelBattery = new DevExpress.XtraEditors.LabelControl();
            this.labelLastCmd = new DevExpress.XtraEditors.LabelControl();
            this.labelEngine = new DevExpress.XtraEditors.LabelControl();
            this.labelSrv6 = new DevExpress.XtraEditors.LabelControl();
            this.labelSrv5 = new DevExpress.XtraEditors.LabelControl();
            this.labelSrv4 = new DevExpress.XtraEditors.LabelControl();
            this.labelSrv3 = new DevExpress.XtraEditors.LabelControl();
            this.labelSrv2 = new DevExpress.XtraEditors.LabelControl();
            this.labelSrv1 = new DevExpress.XtraEditors.LabelControl();
            this.labelUm = new DevExpress.XtraEditors.LabelControl();
            this.labelNav = new DevExpress.XtraEditors.LabelControl();
            this.labelImu = new DevExpress.XtraEditors.LabelControl();
            this.labelBatteryStatus = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.textBatteryStatus)).BeginInit();
            this.textBatteryStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBatteryStatus
            // 
            this.textBatteryStatus.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("groupControl1.CaptionImageOptions.SvgImage")));
            this.textBatteryStatus.Controls.Add(this.labelBatteryStatus);
            this.textBatteryStatus.Controls.Add(this.labelMsn);
            this.textBatteryStatus.Controls.Add(this.labelBattery);
            this.textBatteryStatus.Controls.Add(this.labelLastCmd);
            this.textBatteryStatus.Controls.Add(this.labelEngine);
            this.textBatteryStatus.Controls.Add(this.labelSrv6);
            this.textBatteryStatus.Controls.Add(this.labelSrv5);
            this.textBatteryStatus.Controls.Add(this.labelSrv4);
            this.textBatteryStatus.Controls.Add(this.labelSrv3);
            this.textBatteryStatus.Controls.Add(this.labelSrv2);
            this.textBatteryStatus.Controls.Add(this.labelSrv1);
            this.textBatteryStatus.Controls.Add(this.labelUm);
            this.textBatteryStatus.Controls.Add(this.labelNav);
            this.textBatteryStatus.Controls.Add(this.labelImu);
            this.textBatteryStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBatteryStatus.Location = new System.Drawing.Point(0, 0);
            this.textBatteryStatus.Name = "textBatteryStatus";
            this.textBatteryStatus.Size = new System.Drawing.Size(190, 316);
            this.textBatteryStatus.TabIndex = 0;
            this.textBatteryStatus.Text = "通信状态";
            // 
            // labelMsn
            // 
            this.labelMsn.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelMsn.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelMsn.Appearance.Options.UseFont = true;
            this.labelMsn.Appearance.Options.UseForeColor = true;
            this.labelMsn.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelMsn.ImageOptions.ImageIndex = 0;
            this.labelMsn.ImageOptions.Images = this.imageCollection1;
            this.labelMsn.Location = new System.Drawing.Point(95, 45);
            this.labelMsn.Margin = new System.Windows.Forms.Padding(2);
            this.labelMsn.Name = "labelMsn";
            this.labelMsn.Size = new System.Drawing.Size(69, 21);
            this.labelMsn.TabIndex = 25;
            this.labelMsn.Text = "导引头";
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "cancel_32x32.png");
            this.imageCollection1.Images.SetKeyName(1, "apply_32x32.png");
            // 
            // labelBattery
            // 
            this.labelBattery.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelBattery.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelBattery.Appearance.Options.UseFont = true;
            this.labelBattery.Appearance.Options.UseForeColor = true;
            this.labelBattery.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelBattery.ImageOptions.ImageIndex = 0;
            this.labelBattery.ImageOptions.Images = this.imageCollection1;
            this.labelBattery.Location = new System.Drawing.Point(95, 210);
            this.labelBattery.Margin = new System.Windows.Forms.Padding(2);
            this.labelBattery.Name = "labelBattery";
            this.labelBattery.Size = new System.Drawing.Size(53, 21);
            this.labelBattery.TabIndex = 24;
            this.labelBattery.Text = "电池";
            // 
            // labelLastCmd
            // 
            this.labelLastCmd.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelLastCmd.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelLastCmd.Appearance.Options.UseFont = true;
            this.labelLastCmd.Appearance.Options.UseForeColor = true;
            this.labelLastCmd.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelLastCmd.ImageOptions.ImageIndex = 0;
            this.labelLastCmd.Location = new System.Drawing.Point(23, 283);
            this.labelLastCmd.Margin = new System.Windows.Forms.Padding(2);
            this.labelLastCmd.Name = "labelLastCmd";
            this.labelLastCmd.Size = new System.Drawing.Size(105, 21);
            this.labelLastCmd.TabIndex = 23;
            this.labelLastCmd.Text = "上一条指令: 无";
            // 
            // labelEngine
            // 
            this.labelEngine.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelEngine.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelEngine.Appearance.Options.UseFont = true;
            this.labelEngine.Appearance.Options.UseForeColor = true;
            this.labelEngine.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelEngine.ImageOptions.ImageIndex = 0;
            this.labelEngine.ImageOptions.Images = this.imageCollection1;
            this.labelEngine.Location = new System.Drawing.Point(19, 210);
            this.labelEngine.Margin = new System.Windows.Forms.Padding(2);
            this.labelEngine.Name = "labelEngine";
            this.labelEngine.Size = new System.Drawing.Size(53, 21);
            this.labelEngine.TabIndex = 22;
            this.labelEngine.Text = "电调";
            // 
            // labelSrv6
            // 
            this.labelSrv6.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSrv6.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelSrv6.Appearance.Options.UseFont = true;
            this.labelSrv6.Appearance.Options.UseForeColor = true;
            this.labelSrv6.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelSrv6.ImageOptions.ImageIndex = 0;
            this.labelSrv6.ImageOptions.Images = this.imageCollection1;
            this.labelSrv6.Location = new System.Drawing.Point(95, 177);
            this.labelSrv6.Margin = new System.Windows.Forms.Padding(2);
            this.labelSrv6.Name = "labelSrv6";
            this.labelSrv6.Size = new System.Drawing.Size(69, 21);
            this.labelSrv6.TabIndex = 21;
            this.labelSrv6.Text = "方向右";
            // 
            // labelSrv5
            // 
            this.labelSrv5.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSrv5.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelSrv5.Appearance.Options.UseFont = true;
            this.labelSrv5.Appearance.Options.UseForeColor = true;
            this.labelSrv5.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelSrv5.ImageOptions.ImageIndex = 0;
            this.labelSrv5.ImageOptions.Images = this.imageCollection1;
            this.labelSrv5.Location = new System.Drawing.Point(19, 177);
            this.labelSrv5.Margin = new System.Windows.Forms.Padding(2);
            this.labelSrv5.Name = "labelSrv5";
            this.labelSrv5.Size = new System.Drawing.Size(69, 21);
            this.labelSrv5.TabIndex = 20;
            this.labelSrv5.Text = "方向左";
            // 
            // labelSrv4
            // 
            this.labelSrv4.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSrv4.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelSrv4.Appearance.Options.UseFont = true;
            this.labelSrv4.Appearance.Options.UseForeColor = true;
            this.labelSrv4.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelSrv4.ImageOptions.ImageIndex = 0;
            this.labelSrv4.ImageOptions.Images = this.imageCollection1;
            this.labelSrv4.Location = new System.Drawing.Point(95, 144);
            this.labelSrv4.Margin = new System.Windows.Forms.Padding(2);
            this.labelSrv4.Name = "labelSrv4";
            this.labelSrv4.Size = new System.Drawing.Size(69, 21);
            this.labelSrv4.TabIndex = 19;
            this.labelSrv4.Text = "升降右";
            // 
            // labelSrv3
            // 
            this.labelSrv3.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSrv3.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelSrv3.Appearance.Options.UseFont = true;
            this.labelSrv3.Appearance.Options.UseForeColor = true;
            this.labelSrv3.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelSrv3.ImageOptions.ImageIndex = 0;
            this.labelSrv3.ImageOptions.Images = this.imageCollection1;
            this.labelSrv3.Location = new System.Drawing.Point(19, 144);
            this.labelSrv3.Margin = new System.Windows.Forms.Padding(2);
            this.labelSrv3.Name = "labelSrv3";
            this.labelSrv3.Size = new System.Drawing.Size(69, 21);
            this.labelSrv3.TabIndex = 18;
            this.labelSrv3.Text = "升降左";
            // 
            // labelSrv2
            // 
            this.labelSrv2.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSrv2.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelSrv2.Appearance.Options.UseFont = true;
            this.labelSrv2.Appearance.Options.UseForeColor = true;
            this.labelSrv2.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelSrv2.ImageOptions.ImageIndex = 0;
            this.labelSrv2.ImageOptions.Images = this.imageCollection1;
            this.labelSrv2.Location = new System.Drawing.Point(95, 111);
            this.labelSrv2.Margin = new System.Windows.Forms.Padding(2);
            this.labelSrv2.Name = "labelSrv2";
            this.labelSrv2.Size = new System.Drawing.Size(69, 21);
            this.labelSrv2.TabIndex = 17;
            this.labelSrv2.Text = "副翼右";
            // 
            // labelSrv1
            // 
            this.labelSrv1.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSrv1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelSrv1.Appearance.Options.UseFont = true;
            this.labelSrv1.Appearance.Options.UseForeColor = true;
            this.labelSrv1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelSrv1.ImageOptions.ImageIndex = 0;
            this.labelSrv1.ImageOptions.Images = this.imageCollection1;
            this.labelSrv1.Location = new System.Drawing.Point(19, 111);
            this.labelSrv1.Margin = new System.Windows.Forms.Padding(2);
            this.labelSrv1.Name = "labelSrv1";
            this.labelSrv1.Size = new System.Drawing.Size(69, 21);
            this.labelSrv1.TabIndex = 15;
            this.labelSrv1.Text = "副翼左";
            // 
            // labelUm
            // 
            this.labelUm.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelUm.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelUm.Appearance.Options.UseFont = true;
            this.labelUm.Appearance.Options.UseForeColor = true;
            this.labelUm.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelUm.ImageOptions.ImageIndex = 0;
            this.labelUm.ImageOptions.Images = this.imageCollection1;
            this.labelUm.Location = new System.Drawing.Point(95, 78);
            this.labelUm.Margin = new System.Windows.Forms.Padding(2);
            this.labelUm.Name = "labelUm";
            this.labelUm.Size = new System.Drawing.Size(69, 21);
            this.labelUm.TabIndex = 14;
            this.labelUm.Text = "伞舵机";
            // 
            // labelNav
            // 
            this.labelNav.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelNav.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelNav.Appearance.Options.UseFont = true;
            this.labelNav.Appearance.Options.UseForeColor = true;
            this.labelNav.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelNav.ImageOptions.ImageIndex = 0;
            this.labelNav.ImageOptions.Images = this.imageCollection1;
            this.labelNav.Location = new System.Drawing.Point(19, 78);
            this.labelNav.Margin = new System.Windows.Forms.Padding(2);
            this.labelNav.Name = "labelNav";
            this.labelNav.Size = new System.Drawing.Size(69, 21);
            this.labelNav.TabIndex = 13;
            this.labelNav.Text = "导航板";
            // 
            // labelImu
            // 
            this.labelImu.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelImu.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelImu.Appearance.Options.UseFont = true;
            this.labelImu.Appearance.Options.UseForeColor = true;
            this.labelImu.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelImu.ImageOptions.ImageIndex = 0;
            this.labelImu.ImageOptions.Images = this.imageCollection1;
            this.labelImu.Location = new System.Drawing.Point(19, 45);
            this.labelImu.Margin = new System.Windows.Forms.Padding(2);
            this.labelImu.Name = "labelImu";
            this.labelImu.Size = new System.Drawing.Size(71, 21);
            this.labelImu.TabIndex = 12;
            this.labelImu.Text = "MEMS";
            // 
            // labelBatteryStatus
            // 
            this.labelBatteryStatus.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 11F, System.Drawing.FontStyle.Bold);
            this.labelBatteryStatus.Appearance.Options.UseFont = true;
            this.labelBatteryStatus.Location = new System.Drawing.Point(24, 247);
            this.labelBatteryStatus.Name = "labelBatteryStatus";
            this.labelBatteryStatus.Size = new System.Drawing.Size(96, 20);
            this.labelBatteryStatus.TabIndex = 26;
            this.labelBatteryStatus.Text = "电池状态未知";
            // 
            // ModPayload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBatteryStatus);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ModPayload";
            this.Size = new System.Drawing.Size(190, 316);
            ((System.ComponentModel.ISupportInitialize)(this.textBatteryStatus)).EndInit();
            this.textBatteryStatus.ResumeLayout(false);
            this.textBatteryStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl textBatteryStatus;
        private DevExpress.XtraEditors.LabelControl labelImu;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.LabelControl labelLastCmd;
        private DevExpress.XtraEditors.LabelControl labelEngine;
        private DevExpress.XtraEditors.LabelControl labelSrv6;
        private DevExpress.XtraEditors.LabelControl labelSrv5;
        private DevExpress.XtraEditors.LabelControl labelSrv4;
        private DevExpress.XtraEditors.LabelControl labelSrv3;
        private DevExpress.XtraEditors.LabelControl labelSrv2;
        private DevExpress.XtraEditors.LabelControl labelSrv1;
        private DevExpress.XtraEditors.LabelControl labelUm;
        private DevExpress.XtraEditors.LabelControl labelNav;
        private DevExpress.XtraEditors.LabelControl labelBattery;
        private DevExpress.XtraEditors.LabelControl labelMsn;
        private DevExpress.XtraEditors.LabelControl labelBatteryStatus;
    }
}
