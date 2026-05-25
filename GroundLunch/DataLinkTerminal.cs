using DevExpress.XtraCharts.Designer.Native;
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
    public partial class DataLinkTerminal : DevExpress.XtraEditors.XtraUserControl
    {
        private Timer RefreshUITimer = new Timer();
        public int TerminalOnline = 0;
        public bool TerminalOpen = false;
        public int TerminalID = 0;
        public int paoID = 1;
        public int powerMode = 1;//1小功率 2大功率
        public bool needReInit = false;
        public List<PlaneDataLink> planeDataLinks = new List<PlaneDataLink>();
        public DataLinkTerminal()
        {
            InitializeComponent();
            InitTimer();
            InitPlaneGrid();
        }

        public void InitTimer()
        {
            RefreshUITimer.Interval = 400;
            RefreshUITimer.Tick += new EventHandler(OnTimerFresh);
            RefreshUITimer.Start();
        }

        public void InitPlaneGrid()
        {
            gridPlanes.DataSource = null;
            gridPlanes.DataSource = planeDataLinks;
            viewPlanes.Columns[0].Width = 20 * gridPlanes.Width / 100;
            viewPlanes.Columns[0].OptionsColumn.AllowEdit = false;
            viewPlanes.Columns[1].Width = 30 * gridPlanes.Width / 100;
            viewPlanes.Columns[1].OptionsColumn.AllowEdit = false;
            viewPlanes.Columns[2].Width = 30 * gridPlanes.Width / 100;
            viewPlanes.Columns[2].OptionsColumn.AllowEdit = false;
            viewPlanes.Columns[3].Width = 20 * gridPlanes.Width / 100;
            viewPlanes.Columns[3].ColumnEdit = btPowerModeTransfer;
        }

        public void UpdatePlaneGrid()
        {
            viewPlanes.RefreshData();
        }

        private void OnTimerFresh(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                UpdateUI();
                UpdatePlaneGrid();
                if (needReInit)
                {
                    InitPlaneGrid();
                    needReInit = false;
                }
            }
            // AutoQuery();
            AutoOpen();
        }

        public void UpdateUI()
        {
            toggleOpenDataLink.IsOn = TerminalOnline > 0 ? true : false;
            editSetID.Text = TerminalID.ToString();
            if (powerMode == 1)
            {
                // radioLowPower.Checked = true;
                // radioHighPower.Checked = false;
            }
            else
            {
                //radioLowPower.Checked = false;
                // radioHighPower.Checked = true;
            }
            if (TerminalOnline > 0)
            {
                labelOnline.Text = "在线";
                labelOnline.ForeColor = Color.Lime;
            }
            else
            {
                labelOnline.Text = "离线";
            }
            //comboPaoID.SelectedIndex = paoID - 1;
        }

        public void AutoQuery()
        {
            NetDataHandle.Send_To_DataLink(0xFF, TerminalID);
        }

        public void AutoOpen()
        {
            if (TerminalOnline > 0 && TerminalOpen == false)
            {
                DoOpenTerminal();
            }
        }

        public void DoOpenTerminal()
        {
            NetDataHandle.Send_To_DataLink(0, TerminalID, 1);
        }

        private void toggleOpenDataLink_Toggled(object sender, EventArgs e)
        {
            bool status = toggleOpenDataLink.IsOn;
            if (status)
            {
                NetDataHandle.Send_To_DataLink(0, TerminalID, 1);
            }
            else
            {
                NetDataHandle.Send_To_DataLink(0, TerminalID, 0);
            }
        }

        private void radioHighPower_CheckedChanged(object sender, EventArgs e)
        {
            NetDataHandle.Send_To_DataLink(4, TerminalID, 0);
        }

        private void radioLowPower_CheckedChanged(object sender, EventArgs e)
        {
            NetDataHandle.Send_To_DataLink(4, TerminalID, 1);
        }

        private void btPowerModeTransfer_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            
            
        }

        private void btPowerModeTransfer_Click(object sender, EventArgs e)
        {
            int[] rows = viewPlanes.GetSelectedRows();
            int row = rows[0];

            if (planeDataLinks.Count > row)
            {
                PlaneDataLink plane = planeDataLinks[row];
                //通过终端的paoID,和飞机的guanID，查找对应的映射表，获取飞机ID
                (int, int) planeID0 = NetDataHandle.paoguanTodataLink[(paoID, plane.guanID)];
                UInt16 planeID = (UInt16)(planeID0.Item1 * 0x100 + planeID0.Item2);
                NetDataHandle.Send_To_DataLink(5, planeID, plane.powerMode == 0 ? 1 : 0);
            }
        }
    }

    public class PlaneDataLink
    {
        public int guanID;
        public int powerMode;
        public int dbm;
        public string ID { get { return guanID.ToString(); } set {; } }

        public string 信号强度 { get { return dbm.ToString(); } set {; } }
        public string 功率模式 { get { return powerMode == 0 ? "大功率":"小功率"; } set {; } }
        public string 转换 { get; set; }
    }
        
}
