namespace GroundLunch
{
    partial class DataLinkTerminal
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataLinkTerminal));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.labelOnline = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.radioHighPower = new System.Windows.Forms.RadioButton();
            this.radioLowPower = new System.Windows.Forms.RadioButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.editSetID = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.toggleOpenDataLink = new DevExpress.XtraEditors.ToggleSwitch();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridPlanes = new DevExpress.XtraGrid.GridControl();
            this.viewPlanes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btPowerModeTransfer = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.editSetID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleOpenDataLink.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPlanes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPlanes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btPowerModeTransfer)).BeginInit();
            this.SuspendLayout();
            // 
            // labelOnline
            // 
            this.labelOnline.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F, System.Drawing.FontStyle.Bold);
            this.labelOnline.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelOnline.Appearance.Options.UseFont = true;
            this.labelOnline.Appearance.Options.UseForeColor = true;
            this.labelOnline.Location = new System.Drawing.Point(184, 170);
            this.labelOnline.Name = "labelOnline";
            this.labelOnline.Size = new System.Drawing.Size(38, 24);
            this.labelOnline.TabIndex = 66;
            this.labelOnline.Text = "离线";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(16, 170);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(162, 24);
            this.labelControl4.TabIndex = 65;
            this.labelControl4.Text = "地面终端在线状态：";
            // 
            // radioHighPower
            // 
            this.radioHighPower.AutoSize = true;
            this.radioHighPower.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.radioHighPower.Location = new System.Drawing.Point(192, 124);
            this.radioHighPower.Name = "radioHighPower";
            this.radioHighPower.Size = new System.Drawing.Size(82, 28);
            this.radioHighPower.TabIndex = 64;
            this.radioHighPower.Text = "大功率";
            this.radioHighPower.UseVisualStyleBackColor = true;
            this.radioHighPower.CheckedChanged += new System.EventHandler(this.radioHighPower_CheckedChanged);
            // 
            // radioLowPower
            // 
            this.radioLowPower.AutoSize = true;
            this.radioLowPower.Checked = true;
            this.radioLowPower.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.radioLowPower.Location = new System.Drawing.Point(108, 124);
            this.radioLowPower.Name = "radioLowPower";
            this.radioLowPower.Size = new System.Drawing.Size(82, 28);
            this.radioLowPower.TabIndex = 63;
            this.radioLowPower.TabStop = true;
            this.radioLowPower.Text = "小功率";
            this.radioLowPower.UseVisualStyleBackColor = true;
            this.radioLowPower.CheckedChanged += new System.EventHandler(this.radioLowPower_CheckedChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(16, 124);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(90, 24);
            this.labelControl3.TabIndex = 62;
            this.labelControl3.Text = "功率设置：";
            // 
            // editSetID
            // 
            this.editSetID.EditValue = "203";
            this.editSetID.Location = new System.Drawing.Point(161, 66);
            this.editSetID.Name = "editSetID";
            this.editSetID.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 15F);
            this.editSetID.Properties.Appearance.Options.UseFont = true;
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.editSetID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.editSetID.Size = new System.Drawing.Size(100, 34);
            this.editSetID.TabIndex = 61;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(16, 72);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(143, 24);
            this.labelControl2.TabIndex = 60;
            this.labelControl2.Text = "地面终端ID设置：";
            // 
            // toggleOpenDataLink
            // 
            this.toggleOpenDataLink.Location = new System.Drawing.Point(148, 21);
            this.toggleOpenDataLink.Name = "toggleOpenDataLink";
            this.toggleOpenDataLink.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.toggleOpenDataLink.Properties.Appearance.Options.UseFont = true;
            this.toggleOpenDataLink.Properties.OffText = "Off";
            this.toggleOpenDataLink.Properties.OnText = "On";
            this.toggleOpenDataLink.Size = new System.Drawing.Size(126, 29);
            this.toggleOpenDataLink.TabIndex = 59;
            this.toggleOpenDataLink.Toggled += new System.EventHandler(this.toggleOpenDataLink_Toggled);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(16, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(126, 24);
            this.labelControl1.TabIndex = 58;
            this.labelControl1.Text = "开启地面终端：";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridPlanes);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelOnline);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.toggleOpenDataLink);
            this.panelControl1.Controls.Add(this.radioHighPower);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.radioLowPower);
            this.panelControl1.Controls.Add(this.editSetID);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(279, 442);
            this.panelControl1.TabIndex = 67;
            // 
            // gridPlanes
            // 
            this.gridPlanes.Location = new System.Drawing.Point(16, 201);
            this.gridPlanes.MainView = this.viewPlanes;
            this.gridPlanes.Name = "gridPlanes";
            this.gridPlanes.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.btPowerModeTransfer});
            this.gridPlanes.Size = new System.Drawing.Size(245, 232);
            this.gridPlanes.TabIndex = 67;
            this.gridPlanes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewPlanes});
            // 
            // viewPlanes
            // 
            this.viewPlanes.Appearance.HeaderPanel.Font = new System.Drawing.Font("微软雅黑 Light", 11F);
            this.viewPlanes.Appearance.HeaderPanel.Options.UseFont = true;
            this.viewPlanes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.viewPlanes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.viewPlanes.Appearance.Row.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.viewPlanes.Appearance.Row.Options.UseFont = true;
            this.viewPlanes.Appearance.Row.Options.UseTextOptions = true;
            this.viewPlanes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.viewPlanes.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.viewPlanes.GridControl = this.gridPlanes;
            this.viewPlanes.Name = "viewPlanes";
            this.viewPlanes.OptionsCustomization.AllowFilter = false;
            this.viewPlanes.OptionsCustomization.AllowSort = false;
            this.viewPlanes.OptionsView.ShowGroupPanel = false;
            this.viewPlanes.OptionsView.ShowIndicator = false;
            // 
            // btPowerModeTransfer
            // 
            this.btPowerModeTransfer.AutoHeight = false;
            editorButtonImageOptions2.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions2.Image")));
            this.btPowerModeTransfer.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.btPowerModeTransfer.Name = "btPowerModeTransfer";
            this.btPowerModeTransfer.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.btPowerModeTransfer.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btPowerModeTransfer_ButtonClick);
            this.btPowerModeTransfer.Click += new System.EventHandler(this.btPowerModeTransfer_Click);
            // 
            // DataLinkTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DataLinkTerminal";
            this.Size = new System.Drawing.Size(279, 442);
            ((System.ComponentModel.ISupportInitialize)(this.editSetID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleOpenDataLink.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPlanes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPlanes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btPowerModeTransfer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelOnline;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.RadioButton radioHighPower;
        private System.Windows.Forms.RadioButton radioLowPower;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ButtonEdit editSetID;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ToggleSwitch toggleOpenDataLink;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridPlanes;
        private DevExpress.XtraGrid.Views.Grid.GridView viewPlanes;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btPowerModeTransfer;
    }
}
