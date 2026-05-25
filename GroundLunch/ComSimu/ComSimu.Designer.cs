namespace GroundLunch
{
    partial class ComSimu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComSimu));
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.btStateSimu = new DevExpress.XtraEditors.SimpleButton();
            this.btConnSimu = new DevExpress.XtraEditors.SimpleButton();
            this.comboComSimu = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboComSimu.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("微软雅黑 Light", 11F);
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.btStateSimu);
            this.groupControl2.Controls.Add(this.btConnSimu);
            this.groupControl2.Controls.Add(this.comboComSimu);
            this.groupControl2.Controls.Add(this.labelControl13);
            this.groupControl2.Location = new System.Drawing.Point(11, 11);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(189, 118);
            this.groupControl2.TabIndex = 23;
            this.groupControl2.Text = "转发串口";
            // 
            // btStateSimu
            // 
            this.btStateSimu.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btStateSimu.Appearance.Options.UseFont = true;
            this.btStateSimu.ImageOptions.ImageIndex = 1;
            this.btStateSimu.Location = new System.Drawing.Point(7, 74);
            this.btStateSimu.Name = "btStateSimu";
            this.btStateSimu.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btStateSimu.Size = new System.Drawing.Size(54, 31);
            this.btStateSimu.TabIndex = 24;
            this.btStateSimu.Text = "未连接";
            // 
            // btConnSimu
            // 
            this.btConnSimu.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.btConnSimu.Appearance.Options.UseFont = true;
            this.btConnSimu.Location = new System.Drawing.Point(76, 73);
            this.btConnSimu.Name = "btConnSimu";
            this.btConnSimu.Size = new System.Drawing.Size(88, 31);
            this.btConnSimu.TabIndex = 23;
            this.btConnSimu.Text = "点击连接";
            this.btConnSimu.Click += new System.EventHandler(this.btConnSimu_Click);
            // 
            // comboComSimu
            // 
            this.comboComSimu.Location = new System.Drawing.Point(76, 37);
            this.comboComSimu.Name = "comboComSimu";
            this.comboComSimu.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.comboComSimu.Properties.Appearance.Options.UseFont = true;
            this.comboComSimu.Properties.AppearanceDropDown.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.comboComSimu.Properties.AppearanceDropDown.Options.UseFont = true;
            this.comboComSimu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboComSimu.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboComSimu.Size = new System.Drawing.Size(88, 30);
            this.comboComSimu.TabIndex = 21;
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelControl13.Appearance.Options.UseFont = true;
            this.labelControl13.Location = new System.Drawing.Point(10, 40);
            this.labelControl13.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(61, 24);
            this.labelControl13.TabIndex = 19;
            this.labelControl13.Text = "COM：";
            // 
            // ComSimu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 143);
            this.Controls.Add(this.groupControl2);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ComSimu.IconOptions.SvgImage")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ComSimu";
            this.Text = "模拟COM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ComSimu_FormClosing);
            this.Load += new System.EventHandler(this.ComSimu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboComSimu.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btStateSimu;
        private DevExpress.XtraEditors.SimpleButton btConnSimu;
        private DevExpress.XtraEditors.ComboBoxEdit comboComSimu;
        private DevExpress.XtraEditors.LabelControl labelControl13;
    }
}