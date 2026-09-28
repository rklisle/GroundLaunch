using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GroundLunch.PlaybackPage
{
    public partial class AnalyzeDatForm : DevExpress.XtraEditors.XtraForm
    {

        public delegate int AnalyzeDat(string path);
        public AnalyzeDat func;
        public string fileName;
        public string csvFileName;
        public long fileSize;
        public double analyzeProgress = 0;
        Timer freshTimer = new Timer();

        public AnalyzeDatForm()
        {
            InitializeComponent();
            
        }

        private void btStartAnalyze_Click(object sender, EventArgs e)
        {
            btStartAnalyze.Enabled = false;
            labelCurState.Visible = true;
            analyzeProgress = 0;
            freshTimer.Start();
            Task.Run(()=> func(fileName));
            // func(fileName);
        }

        private void btReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AnalyzeDatForm_Load(object sender, EventArgs e)
        {
            //progressAnalyze.Properties.MarqueeAnimationSpeed = 100;
            freshTimer.Interval = 50;
            freshTimer.Tick += FreshTimer_Tick;
        }

        private void FreshTimer_Tick(object sender, EventArgs e)
        {
            progressAnalyze.Position = (int)(analyzeProgress * 100);
            if (analyzeProgress >= 0.999)
            {
                progressAnalyze.Position = 100;
                labelCurState.Text = "解析完成";
                labelCurState.ForeColor = Color.Lime;
                btStartAnalyze.Enabled = true;
                labelFileName.Text = csvFileName;
                labelFileName.Visible = true;
                btOpenCsv.Visible = true;
            }
        }

        private void btOpenCsv_Click(object sender, EventArgs e)
        {
            // 获取当前程序的目录
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 组合完整文件路径
            string filePath = Path.Combine(currentDirectory, csvFileName);

            Process.Start(filePath);
        }
    }
}
