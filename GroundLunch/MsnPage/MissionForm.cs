using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroundLunch
{
    public partial class MissionForm : DevExpress.XtraEditors.XtraForm
    {
        private Timer RefreshUITimer = new Timer();
        public int groupID;
        public int msnID;
        MissionFile missionFile;
        public MissionForm()
        {
            InitializeComponent();
        }

        public void OnSelMsnFileChange(MissionFile missionFile1)
        {
            missionFile = missionFile1;
            missionMap.missionFile = missionFile;
            missionMap.UpdateSelMsnFile();
        }

        private void MissionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;        // 阻止关闭
            this.Hide();            // 隐藏而不是释放
            RefreshUITimer.Stop();
        }

        private void MissionForm_Shown(object sender, EventArgs e)
        {
        }

        private void MissionForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshUITimer.Interval = 50;
                RefreshUITimer.Tick += new EventHandler(OnTimerFresh);
                RefreshUITimer.Start();
            }
            else
            {
                RefreshUITimer.Stop();
            }
        }

        private void OnTimerFresh(object sender, EventArgs e)
        {
            foreach (KeyValuePair<(int, int), List<TMParam>> kvp in TMHandler.allParamList)
            {
                int groupid = 0, msnid = 0;
                double lon = 0, lat = 0, alt = 0, spd = 0,dir = 0;
                int startFly = 0;
                foreach (TMParam param in kvp.Value)
                {
                    if (param.paramID == "msnGrpID")
                    {
                        groupid = Convert.ToInt32(param.物理量);
                    }
                    if (param.paramID == "msnDevID")
                    {
                        msnid = Convert.ToInt32(param.物理量);
                    }
                    if (param.paramID == "AirSpd")
                    {
                        spd = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "navLon" && param.paramDataPool == "imu")
                    {
                        lon = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "navLat" && param.paramDataPool == "imu")
                    {
                        lat = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "navHigh" && param.paramDataPool == "imu")
                    {
                        alt = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "navDir" && param.paramDataPool == "imu")
                    {
                        dir = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "startFly")
                    {
                        startFly = Convert.ToInt32(param.物理量);
                    }
                }
                
                if (groupid != 0 && groupid != 0xff)
                {
                    DataInterface.startFly = startFly == 1 ? true : false;
                    DataInterface.UVEs[(groupid, msnid)].uveEnable = 1;
                    DataInterface.UVEs[(groupid, msnid)].curInfo.wGS84Pos.lon = lon;
                    DataInterface.UVEs[(groupid, msnid)].curInfo.wGS84Pos.lat = lat;
                    DataInterface.UVEs[(groupid, msnid)].curInfo.wGS84Pos.alt = alt;
                    DataInterface.UVEs[(groupid, msnid)].curInfo.TAS = spd;
                    DataInterface.UVEs[(groupid, msnid)].curInfo.dir = dir;
                }
            }
            
            missionMap.TimerFresh();
        }


    }
}
