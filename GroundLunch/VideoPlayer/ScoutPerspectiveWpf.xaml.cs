using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroundLunch
{
    /// <summary>
    /// ScoutPerspectiveWpf.xaml 的交互逻辑
    /// </summary>
    public partial class ScoutPerspectiveWpf : UserControl
    {
        private System.Windows.Threading.DispatcherTimer timer;
        MyCameraView _myCameraView;
        public ScoutPerspectiveWpf()
        {
            InitializeComponent();
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPTxy8BH1VEsoebNVZXo8HurAgEx518B3Ilj8Qxe1-0-OboEQvvWbL-MmN_ywlKFfU7pzStixqAEObiVrjyvqbQLZ4glJe5ysemFN47aWyC2oJ2OEV4svxj1kZCLqQEaqIk_926d0pmbo-hmbt-HO_1n-PJi-QFD61IJH0_c--SqPPvQtRVORou74PXJrsTH2qJOJ9vRbApoSYItAyXwyb5Xvlgv2EKdqDbGDZ1TM-giJc.AT1_vLBRSxgx";

            _myCameraView = new MyCameraView(CameraView);

            StartDynamicCurve();

            InitTest();
        }

        public void InitTest()
        {

        }

        private void StartDynamicCurve()
        {
            // 设置定时器，100ms 执行一次
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += (sender, e) => _100msTimerFresh();
            timer.Start();
        }

        private void _100msTimerFresh()
        {
            if (DataInterface.isUpdate == 1)
            {
                DataInterface.isUpdate = 0;
                _myCameraView.TimerFresh();
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                /*
                // Update the progress slider
                MissionProgressBar.Value = missionProgress;

                // Update stats display
                AltitudeLabel.Text = currentFrame.Elevation.ToString("F") + "m";
                HeadingLabel.Text = currentFrame.Heading.ToString("F") + "\u00b0";
                PitchLabel.Text = currentFrame.Pitch.ToString("F") + "\u00b0";
                RollLabel.Text = currentFrame.Roll.ToString("F") + "\u00b0";
                */

                AltitudeLabel.Text = DataInterface.viewPlane.alt.ToString("F") + " m";
                HeadingLabel.Text = DataInterface.viewPlane.heading.ToString("F") + "\u00b0";
                PitchLabel.Text = DataInterface.viewPlane.pitch.ToString("F") + "\u00b0";
                RollLabel.Text = DataInterface.viewPlane.roll.ToString("F") + "\u00b0";

                LongitudeLabel.Text = DataInterface.viewPlane.lon.ToString("F7") + "\u00b0";
                LatitudeLabel.Text = DataInterface.viewPlane.lat.ToString("F7") + "\u00b0";

                ScoutPitchLabel.Text = DataInterface.viewPlane.scoutPitch.ToString("F") + "\u00b0";
                ScoutHeadingLabel.Text = DataInterface.viewPlane.scoutHeading.ToString("F") + "\u00b0";

                SpeedLabel.Text = DataInterface.viewPlane.speed.ToString("F") + " m/s";
                RpmLabel.Text = DataInterface.viewPlane.rpm.ToString("F") + " rpm";

                TimeLabel.Text = DataInterface.viewPlane.ignationTime.ToString("F3");
                MsnIDLabel.Text = DataInterface.viewPlane.groupID.ToString() + "-" + DataInterface.viewPlane.msnID.ToString();
                LeadIDLabel.Text = DataInterface.viewPlane.leadID.ToString();
            }));
        }
    }


}
