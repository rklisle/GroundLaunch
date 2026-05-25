using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GroundLunch
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        //HomePage homePage;
        LuanchPage luanchPage;
        UpLoadPage upLoadPage;
        SrvTestPage srvTestPage;
        VideoForm videoForm;
        public MissionForm missionForm;
        public ComSimu comSimuForm;
        public FormDataLink datalinkForm;
        DataPlayBackPage playBackPage;

        public ComNode comNode = new ComNode();
        public ComNode comTmNode = new ComNode();
        private Timer handleTimer = new Timer();
        private Timer RefreshUITimer = new Timer();

        const int PAGE_HOME = 0;
        const int PAGE_UPLOAD = 1;
        const int PAGE_SRV = 2;
        const int PAGE_PLAYBACK = 3;
        const int PAGE_COMM_SIMU = 4;
        public MainForm()
        {
            InitializeComponent();
            InitNavPages();
            InitRefreshTimer();

           // ComDataHandle.comNode = comNode;
            //ComDataHandle.comTmNode = comTmNode;
            //ComDataHandle.comTmNode = comNode;
           // ComSend.comNode = comNode;
        }

        private void InitRefreshTimer()
        {
            RefreshUITimer.Interval = 100;
            RefreshUITimer.Tick += new EventHandler(OnTimerFresh);
            RefreshUITimer.Start();
        }

        private void OnTimerFresh(object sender, EventArgs e)
        {
            //测试
           // NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime += 0.05;
            //测试完
            switch (navFrame.SelectedPageIndex)
            {
                case PAGE_HOME:
                    luanchPage.RefreshUI(); 
                    break;
                case PAGE_UPLOAD:
                    upLoadPage.RefreshUI();
                    break;
                case PAGE_SRV:
                    srvTestPage.RefreshUI();
                    break;
                case PAGE_PLAYBACK: 
                    break;
                case PAGE_COMM_SIMU:
                    break;
                default: 
                    break;
            }
        }

        private void InitNavPages()
        {
            luanchPage = new LuanchPage(this);
            
            navFrame.AddPage(luanchPage);

            //homePage = new HomePage();
           // homePage.pageSelected = true;
           // navFrame.AddPage(homePage);

            upLoadPage = new UpLoadPage();
            navFrame.AddPage(upLoadPage);

            srvTestPage = new SrvTestPage();
            navFrame.AddPage(srvTestPage);

            playBackPage = new DataPlayBackPage();
            navFrame.AddPage(playBackPage);

            navFrame.TransitionAnimationProperties.FrameInterval = 1000;
            navFrame.TransitionAnimationProperties.FrameCount = 3000;
            navFrame.TransitionType = DevExpress.Utils.Animation.Transitions.Dissolve;
           

            this.WindowState = FormWindowState.Maximized;

            videoForm = new VideoForm();
            missionForm = new MissionForm();
            datalinkForm = new FormDataLink();
            comSimuForm = new ComSimu();
        }

        private void MenuHome_Click(object sender, EventArgs e)
        {
            navFrame.SelectedPageIndex = 0;
            //homePage.pageSelected = true;

            upLoadPage.pageSelected = false;
            srvTestPage.pageSelected = false;
            playBackPage.pageSelected = false;
        }

        private void MenuFileUpload_Click(object sender, EventArgs e)
        {
            navFrame.SelectedPageIndex = 1;
            upLoadPage.pageSelected = true;

            //homePage.pageSelected = false;
            srvTestPage.pageSelected = false;
            playBackPage.pageSelected = false;
        }

        

        private void MenuSrv_Click(object sender, EventArgs e)
        {
            navFrame.SelectedPageIndex = 2;
            srvTestPage.pageSelected = true;

            //homePage.pageSelected = false;
            upLoadPage.pageSelected = false;
            playBackPage.pageSelected = false;
        }

        private void MenuData_Click(object sender, EventArgs e)
        {
            navFrame.SelectedPageIndex = 4;
            playBackPage.pageSelected = true;

            srvTestPage.pageSelected = false;
            //homePage.pageSelected = false;
            upLoadPage.pageSelected = false;
        }

        private void StripMenuVideo_Click(object sender, EventArgs e)
        {
            Screen[] screens = Screen.AllScreens;
            if (screens.Length > 1)
            {
                // 找到主屏（通常是最左边的屏幕）
                Screen primaryScreen = Screen.PrimaryScreen;

                // 找到右侧屏幕（X坐标大于主屏的屏幕）
                Screen rightScreen = screens.FirstOrDefault(
                    s => s.Bounds.X > primaryScreen.Bounds.X);

                if (rightScreen != null) // 存在右侧屏幕
                {
                    // 将窗口移动到右侧屏幕并最大化
                    videoForm.StartPosition = FormStartPosition.Manual;
                    videoForm.Location = rightScreen.WorkingArea.Location;
                    videoForm.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    videoForm.StartPosition = FormStartPosition.CenterScreen;
                    videoForm.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                videoForm.StartPosition = FormStartPosition.CenterScreen;
                videoForm.WindowState = FormWindowState.Normal;
            }
            videoForm.Show();
        }

        private void StripMenuMission_Click(object sender, EventArgs e)
        {
            Screen[] screens = Screen.AllScreens;
            if (screens.Length > 2)
            {
                // 找到主屏（通常是最左边的屏幕）
                Screen primaryScreen = Screen.PrimaryScreen;

                // 找到右侧屏幕（X坐标大于主屏的屏幕）
                Screen rightScreen = screens.FirstOrDefault(
                    s => s.Bounds.X > primaryScreen.Bounds.X);

                // 找到左侧屏幕（X坐标小于主屏的屏幕）
                Screen leftScreen = screens.FirstOrDefault(
                    s => s.Bounds.X < primaryScreen.Bounds.X);

                if (leftScreen != null) // 存在右侧屏幕
                {
                    // 将窗口移动到右侧屏幕并最大化
                    missionForm.StartPosition = FormStartPosition.Manual;
                    missionForm.Location = leftScreen.WorkingArea.Location;
                    missionForm.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    missionForm.StartPosition = FormStartPosition.CenterScreen;
                    missionForm.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                missionForm.StartPosition = FormStartPosition.CenterScreen;
                missionForm.WindowState = FormWindowState.Normal;
            }
            missionForm.Show();
        }

        private void StripMenuLink_Click(object sender, EventArgs e)
        {
            comSimuForm.Show();
        }

        private void StripMenuDataLink_Click(object sender, EventArgs e)
        {
            datalinkForm.Show();
        }
    }
}
