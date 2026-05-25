namespace GroundLunch
{
    partial class OnePlane
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnePlane));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelmsnControl = new DevExpress.XtraEditors.LabelControl();
            this.labelEngSate = new DevExpress.XtraEditors.LabelControl();
            this.labelnavState = new DevExpress.XtraEditors.LabelControl();
            this.labelScCnt = new DevExpress.XtraEditors.LabelControl();
            this.labelError = new DevExpress.XtraEditors.LabelControl();
            this.labelpaylodtp = new DevExpress.XtraEditors.LabelControl();
            this.picLock = new DevExpress.XtraEditors.PictureEdit();
            this.labelYaw = new DevExpress.XtraEditors.LabelControl();
            this.labelRoll = new DevExpress.XtraEditors.LabelControl();
            this.labelAlt = new DevExpress.XtraEditors.LabelControl();
            this.labelLat = new DevExpress.XtraEditors.LabelControl();
            this.labelLon = new DevExpress.XtraEditors.LabelControl();
            this.labelPitch = new DevExpress.XtraEditors.LabelControl();
            this.labelRpm = new DevExpress.XtraEditors.LabelControl();
            this.labelA = new DevExpress.XtraEditors.LabelControl();
            this.labelV = new DevExpress.XtraEditors.LabelControl();
            this.labelMsnID = new DevExpress.XtraEditors.LabelControl();
            this.picPlane = new DevExpress.XtraEditors.PictureEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlane.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BorderColor = System.Drawing.Color.DimGray;
            this.panelControl1.Appearance.Options.UseBorderColor = true;
            this.panelControl1.Controls.Add(this.labelmsnControl);
            this.panelControl1.Controls.Add(this.labelEngSate);
            this.panelControl1.Controls.Add(this.labelnavState);
            this.panelControl1.Controls.Add(this.labelScCnt);
            this.panelControl1.Controls.Add(this.labelError);
            this.panelControl1.Controls.Add(this.labelpaylodtp);
            this.panelControl1.Controls.Add(this.picLock);
            this.panelControl1.Controls.Add(this.labelYaw);
            this.panelControl1.Controls.Add(this.labelRoll);
            this.panelControl1.Controls.Add(this.labelAlt);
            this.panelControl1.Controls.Add(this.labelLat);
            this.panelControl1.Controls.Add(this.labelLon);
            this.panelControl1.Controls.Add(this.labelPitch);
            this.panelControl1.Controls.Add(this.labelRpm);
            this.panelControl1.Controls.Add(this.labelA);
            this.panelControl1.Controls.Add(this.labelV);
            this.panelControl1.Controls.Add(this.labelMsnID);
            this.panelControl1.Controls.Add(this.picPlane);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 27);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(336, 247);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Click += new System.EventHandler(this.panelControl1_Click);
            // 
            // labelmsnControl
            // 
            this.labelmsnControl.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 14F);
            this.labelmsnControl.Appearance.Options.UseFont = true;
            this.labelmsnControl.Appearance.Options.UseTextOptions = true;
            this.labelmsnControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelmsnControl.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelmsnControl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelmsnControl.Location = new System.Drawing.Point(8, 0);
            this.labelmsnControl.Name = "labelmsnControl";
            this.labelmsnControl.Size = new System.Drawing.Size(80, 24);
            this.labelmsnControl.TabIndex = 24;
            this.labelmsnControl.Click += new System.EventHandler(this.labelControl1_Click);
            // 
            // labelEngSate
            // 
            this.labelEngSate.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelEngSate.Appearance.Options.UseFont = true;
            this.labelEngSate.Location = new System.Drawing.Point(247, 166);
            this.labelEngSate.Name = "labelEngSate";
            this.labelEngSate.Size = new System.Drawing.Size(74, 21);
            this.labelEngSate.TabIndex = 23;
            this.labelEngSate.Text = "EngSate 0";
            // 
            // labelnavState
            // 
            this.labelnavState.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelnavState.Appearance.Options.UseFont = true;
            this.labelnavState.Location = new System.Drawing.Point(247, 58);
            this.labelnavState.Name = "labelnavState";
            this.labelnavState.Size = new System.Drawing.Size(43, 21);
            this.labelnavState.TabIndex = 22;
            this.labelnavState.Text = "Nav 0";
            this.labelnavState.Click += new System.EventHandler(this.labelControl3_Click);
            // 
            // labelScCnt
            // 
            this.labelScCnt.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelScCnt.Appearance.Options.UseFont = true;
            this.labelScCnt.Location = new System.Drawing.Point(246, 22);
            this.labelScCnt.Name = "labelScCnt";
            this.labelScCnt.Size = new System.Drawing.Size(44, 21);
            this.labelScCnt.TabIndex = 21;
            this.labelScCnt.Text = "Sats 0";
            this.labelScCnt.Click += new System.EventHandler(this.labelScCntControl);
            // 
            // labelError
            // 
            this.labelError.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelError.Appearance.Options.UseFont = true;
            this.labelError.Location = new System.Drawing.Point(247, 94);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(79, 21);
            this.labelError.TabIndex = 20;
            this.labelError.Text = "ErrCode: 0";
            // 
            // labelpaylodtp
            // 
            this.labelpaylodtp.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelpaylodtp.Appearance.Options.UseFont = true;
            this.labelpaylodtp.Location = new System.Drawing.Point(247, 130);
            this.labelpaylodtp.Name = "labelpaylodtp";
            this.labelpaylodtp.Size = new System.Drawing.Size(80, 21);
            this.labelpaylodtp.TabIndex = 19;
            this.labelpaylodtp.Text = "paylodtp 0";
            this.labelpaylodtp.Click += new System.EventHandler(this.labelControl8_Click);
            // 
            // picLock
            // 
            this.picLock.EditValue = ((object)(resources.GetObject("picLock.EditValue")));
            this.picLock.Location = new System.Drawing.Point(83, 86);
            this.picLock.Name = "picLock";
            this.picLock.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picLock.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picLock.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picLock.Size = new System.Drawing.Size(28, 28);
            this.picLock.TabIndex = 6;
            this.picLock.Visible = false;
            // 
            // labelYaw
            // 
            this.labelYaw.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelYaw.Appearance.Options.UseFont = true;
            this.labelYaw.Location = new System.Drawing.Point(123, 202);
            this.labelYaw.Name = "labelYaw";
            this.labelYaw.Size = new System.Drawing.Size(51, 21);
            this.labelYaw.TabIndex = 11;
            this.labelYaw.Text = "Yaw 0°";
            // 
            // labelRoll
            // 
            this.labelRoll.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelRoll.Appearance.Options.UseFont = true;
            this.labelRoll.Location = new System.Drawing.Point(123, 166);
            this.labelRoll.Name = "labelRoll";
            this.labelRoll.Size = new System.Drawing.Size(49, 21);
            this.labelRoll.TabIndex = 10;
            this.labelRoll.Text = "Roll 0°";
            // 
            // labelAlt
            // 
            this.labelAlt.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelAlt.Appearance.Options.UseFont = true;
            this.labelAlt.Location = new System.Drawing.Point(123, 94);
            this.labelAlt.Name = "labelAlt";
            this.labelAlt.Size = new System.Drawing.Size(48, 21);
            this.labelAlt.TabIndex = 9;
            this.labelAlt.Text = "Alt 0m";
            this.labelAlt.Click += new System.EventHandler(this.labelAlt_Click);
            // 
            // labelLat
            // 
            this.labelLat.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelLat.Appearance.Options.UseFont = true;
            this.labelLat.Location = new System.Drawing.Point(123, 58);
            this.labelLat.Name = "labelLat";
            this.labelLat.Size = new System.Drawing.Size(43, 21);
            this.labelLat.TabIndex = 8;
            this.labelLat.Text = "Lat 0°";
            this.labelLat.Click += new System.EventHandler(this.labelLat_Click);
            // 
            // labelLon
            // 
            this.labelLon.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelLon.Appearance.Options.UseFont = true;
            this.labelLon.Location = new System.Drawing.Point(123, 22);
            this.labelLon.Name = "labelLon";
            this.labelLon.Size = new System.Drawing.Size(47, 21);
            this.labelLon.TabIndex = 7;
            this.labelLon.Text = "Lon:0°";
            // 
            // labelPitch
            // 
            this.labelPitch.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelPitch.Appearance.Options.UseFont = true;
            this.labelPitch.Location = new System.Drawing.Point(123, 130);
            this.labelPitch.Name = "labelPitch";
            this.labelPitch.Size = new System.Drawing.Size(56, 21);
            this.labelPitch.TabIndex = 5;
            this.labelPitch.Text = "Pitch 0°";
            // 
            // labelRpm
            // 
            this.labelRpm.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelRpm.Appearance.Options.UseFont = true;
            this.labelRpm.Location = new System.Drawing.Point(247, 202);
            this.labelRpm.Name = "labelRpm";
            this.labelRpm.Size = new System.Drawing.Size(48, 21);
            this.labelRpm.TabIndex = 4;
            this.labelRpm.Text = "Rpm 0";
            this.labelRpm.Click += new System.EventHandler(this.labelRpm_Click);
            // 
            // labelA
            // 
            this.labelA.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelA.Appearance.Options.UseFont = true;
            this.labelA.Location = new System.Drawing.Point(30, 166);
            this.labelA.Name = "labelA";
            this.labelA.Size = new System.Drawing.Size(34, 23);
            this.labelA.TabIndex = 3;
            this.labelA.Text = "0.0A";
            this.labelA.Click += new System.EventHandler(this.labelA_Click);
            // 
            // labelV
            // 
            this.labelV.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelV.Appearance.Options.UseFont = true;
            this.labelV.Location = new System.Drawing.Point(30, 202);
            this.labelV.Name = "labelV";
            this.labelV.Size = new System.Drawing.Size(33, 23);
            this.labelV.TabIndex = 2;
            this.labelV.Text = "0.0V";
            // 
            // labelMsnID
            // 
            this.labelMsnID.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.labelMsnID.Appearance.Options.UseFont = true;
            this.labelMsnID.Appearance.Options.UseTextOptions = true;
            this.labelMsnID.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelMsnID.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelMsnID.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelMsnID.Location = new System.Drawing.Point(5, 136);
            this.labelMsnID.Name = "labelMsnID";
            this.labelMsnID.Size = new System.Drawing.Size(107, 24);
            this.labelMsnID.TabIndex = 1;
            this.labelMsnID.Text = "Disconnected";
            // 
            // picPlane
            // 
            this.picPlane.EditValue = ((object)(resources.GetObject("picPlane.EditValue")));
            this.picPlane.Location = new System.Drawing.Point(8, 30);
            this.picPlane.Name = "picPlane";
            this.picPlane.Properties.AllowFocused = false;
            this.picPlane.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picPlane.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picPlane.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.picPlane.Size = new System.Drawing.Size(100, 100);
            this.picPlane.TabIndex = 0;
            this.picPlane.Click += new System.EventHandler(this.picPlane_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(340, 276);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Gun Barrel ID: 01";
            this.groupControl1.Click += new System.EventHandler(this.groupControl1_Click);
            // 
            // OnePlane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OnePlane";
            this.Size = new System.Drawing.Size(340, 276);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlane.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PictureEdit picPlane;
        private DevExpress.XtraEditors.LabelControl labelMsnID;
        private DevExpress.XtraEditors.LabelControl labelYaw;
        private DevExpress.XtraEditors.LabelControl labelRoll;
        private DevExpress.XtraEditors.LabelControl labelAlt;
        private DevExpress.XtraEditors.LabelControl labelLat;
        private DevExpress.XtraEditors.LabelControl labelLon;
        private DevExpress.XtraEditors.LabelControl labelPitch;
        private DevExpress.XtraEditors.LabelControl labelRpm;
        private DevExpress.XtraEditors.LabelControl labelA;
        private DevExpress.XtraEditors.LabelControl labelV;
        private DevExpress.XtraEditors.PictureEdit picLock;
        private DevExpress.XtraEditors.LabelControl labelpaylodtp;
        private DevExpress.XtraEditors.LabelControl labelError;
        private DevExpress.XtraEditors.LabelControl labelScCnt;
        private DevExpress.XtraEditors.LabelControl labelnavState;
        private DevExpress.XtraEditors.LabelControl labelEngSate;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelmsnControl;
    }
}
