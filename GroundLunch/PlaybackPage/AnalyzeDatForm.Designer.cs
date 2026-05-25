namespace GroundLunch.PlaybackPage
{
    partial class AnalyzeDatForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalyzeDatForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btStartAnalyze = new DevExpress.XtraEditors.SimpleButton();
            this.labelCurState = new DevExpress.XtraEditors.LabelControl();
            this.btReturn = new DevExpress.XtraEditors.SimpleButton();
            this.progressAnalyze = new DevExpress.XtraEditors.ProgressBarControl();
            this.btOpenCsv = new DevExpress.XtraEditors.SimpleButton();
            this.labelFileName = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.progressAnalyze.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(23, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(162, 24);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "源码文件，需要解析";
            // 
            // btStartAnalyze
            // 
            this.btStartAnalyze.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.btStartAnalyze.Appearance.Options.UseFont = true;
            this.btStartAnalyze.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btStartAnalyze.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btStartAnalyze.ImageOptions.SvgImage")));
            this.btStartAnalyze.Location = new System.Drawing.Point(23, 69);
            this.btStartAnalyze.Name = "btStartAnalyze";
            this.btStartAnalyze.Size = new System.Drawing.Size(133, 38);
            this.btStartAnalyze.TabIndex = 1;
            this.btStartAnalyze.Text = "开始解析";
            this.btStartAnalyze.Click += new System.EventHandler(this.btStartAnalyze_Click);
            // 
            // labelCurState
            // 
            this.labelCurState.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelCurState.Appearance.ForeColor = System.Drawing.Color.Yellow;
            this.labelCurState.Appearance.Options.UseFont = true;
            this.labelCurState.Appearance.Options.UseForeColor = true;
            this.labelCurState.Location = new System.Drawing.Point(207, 23);
            this.labelCurState.Name = "labelCurState";
            this.labelCurState.Size = new System.Drawing.Size(108, 24);
            this.labelCurState.TabIndex = 3;
            this.labelCurState.Text = "正在解析文件";
            this.labelCurState.Visible = false;
            // 
            // btReturn
            // 
            this.btReturn.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.btReturn.Appearance.Options.UseFont = true;
            this.btReturn.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btReturn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btReturn.ImageOptions.SvgImage")));
            this.btReturn.Location = new System.Drawing.Point(571, 69);
            this.btReturn.Name = "btReturn";
            this.btReturn.Size = new System.Drawing.Size(93, 38);
            this.btReturn.TabIndex = 4;
            this.btReturn.Text = "返回";
            this.btReturn.Click += new System.EventHandler(this.btReturn_Click);
            // 
            // progressAnalyze
            // 
            this.progressAnalyze.Location = new System.Drawing.Point(166, 69);
            this.progressAnalyze.Name = "progressAnalyze";
            this.progressAnalyze.Properties.ShowTitle = true;
            this.progressAnalyze.Size = new System.Drawing.Size(392, 38);
            this.progressAnalyze.TabIndex = 5;
            // 
            // btOpenCsv
            // 
            this.btOpenCsv.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.btOpenCsv.Appearance.Options.UseFont = true;
            this.btOpenCsv.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btOpenCsv.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton1.ImageOptions.SvgImage")));
            this.btOpenCsv.Location = new System.Drawing.Point(571, 18);
            this.btOpenCsv.Name = "btOpenCsv";
            this.btOpenCsv.Size = new System.Drawing.Size(93, 38);
            this.btOpenCsv.TabIndex = 6;
            this.btOpenCsv.Text = "打开";
            this.btOpenCsv.Visible = false;
            this.btOpenCsv.Click += new System.EventHandler(this.btOpenCsv_Click);
            // 
            // labelFileName
            // 
            this.labelFileName.Appearance.Font = new System.Drawing.Font("微软雅黑 Light", 13F);
            this.labelFileName.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelFileName.Appearance.Options.UseFont = true;
            this.labelFileName.Appearance.Options.UseForeColor = true;
            this.labelFileName.Location = new System.Drawing.Point(307, 25);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(52, 23);
            this.labelFileName.TabIndex = 7;
            this.labelFileName.Text = "DatFile";
            this.labelFileName.Visible = false;
            // 
            // AnalyzeDatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 123);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.btOpenCsv);
            this.Controls.Add(this.progressAnalyze);
            this.Controls.Add(this.btReturn);
            this.Controls.Add(this.labelCurState);
            this.Controls.Add(this.btStartAnalyze);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AnalyzeDatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AnalyzeDatForm";
            this.Load += new System.EventHandler(this.AnalyzeDatForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.progressAnalyze.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btStartAnalyze;
        private DevExpress.XtraEditors.LabelControl labelCurState;
        private DevExpress.XtraEditors.SimpleButton btReturn;
        private DevExpress.XtraEditors.ProgressBarControl progressAnalyze;
        private DevExpress.XtraEditors.SimpleButton btOpenCsv;
        private DevExpress.XtraEditors.LabelControl labelFileName;
    }
}