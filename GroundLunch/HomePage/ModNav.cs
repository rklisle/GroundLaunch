using DevExpress.XtraEditors.TextEditController.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace GroundLunch
{
    public partial class ModNav : DevExpress.XtraEditors.XtraUserControl
    {
        MyMMTimer focusTimer = new MyMMTimer();
        const double focusTotalSecond = 210;
        double focusCurSecond = 0;
        string stringFocusLastSecond = "0秒";
        //double
        public ModNav()
        {
            InitializeComponent();
            InitTimer();
        }

        private void InitTimer()
        {
            
          
        }

        private void AutoSendEvery20Ms(uint id, uint msg, UIntPtr user, UIntPtr dw1, UIntPtr dw2)
        {
            focusCurSecond += 0.02;
            int lastTime = (int)(focusTotalSecond - focusCurSecond);
            stringFocusLastSecond = string.Format("{0}秒",lastTime + 1);
            if (focusCurSecond >= focusTotalSecond)
            {
                focusTimer.DestroyTimer();
                focusTimer.Cleanup();
            }
        }

        public void RefreshUI()
        {
            progressFocus.Position = (int)(focusCurSecond / focusTotalSecond * 100);
            labelLastSecond.Text = stringFocusLastSecond;
            if (focusCurSecond >= focusTotalSecond)
            {
                btFocus.Enabled = true;
                labelLastSecond.Text = "0秒";
            }
            //姿态
            labelPitch.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].navPitch.ToString("F2");
            labelYaw.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].navYaw.ToString("F2");
            labelRoll.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].navRoll.ToString("F2");

            //对准结果
            labelAzi.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].navAzimuth.ToString("F1");

            labelWEarth.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].velocityRotation.ToString("F2");
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].velocityRotation > 16 || NetDataHandle.groundFrame[NetDataHandle.curSelMsn].velocityRotation < 14)
            {
                labelWEarth.ForeColor = Color.Yellow;
            }
            else
            {
                labelWEarth.ForeColor = Color.Lime;
            }
            labelLocalG.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].localG.ToString("F2");
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].localG > 10 || NetDataHandle.groundFrame[NetDataHandle.curSelMsn].localG < 9.6)
            {
                labelLocalG.ForeColor = Color.Yellow;
            }
            else
            {
                labelLocalG.ForeColor = Color.Lime;
            }

            //导航模式
            switch (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuNavMode)
            {
                case 0x20:
                    labelNavMode.Text = "对准中";
                    if (btFocus.Enabled == true)
                    {
                        //通过任务机启动的对准，
                        StartFocus();
                    }
                    break;
                case 0x2F:
                    labelNavMode.Text = "对准超时";
                    btFocus.Enabled = true;
                    focusCurSecond = focusTotalSecond;
                    break;
                case 0x3F:
                    labelNavMode.Text = "对准完成";
                    btFocus.Enabled = true;
                    focusCurSecond = focusTotalSecond;
                    break;
                case 0x60:
                    labelNavMode.Text = "组合导航";
                    btFocus.Enabled = true;
                    focusCurSecond = focusTotalSecond;
                    break;
                case 0x64:
                    labelNavMode.Text = "惯性导航";
                    btFocus.Enabled = true;
                    focusCurSecond = focusTotalSecond;
                    break;
                case 0x00:
                    labelNavMode.Text = "未开始";
                    btFocus.Enabled = true;
                    focusCurSecond = 0;
                    break;
            }
            
        }

        private void StartFocus()
        {
            focusTimer.CreateTimer(AutoSendEvery20Ms, 20);
            btFocus.Enabled = false;
            focusCurSecond = 0;
        }

        private void btFocus_Click(object sender, EventArgs e)
        {
           // StartFocus();
            Byte[] data = new byte[8];
            data[0] = 0;//水平对准
            data[0] = 1;//垂直对准
            // NetDataHandle.Send_To_FK(0x1, 0xE0, data);
            NetDataHandle.Send_To_FK(0x1, 0xE0, data);
        }

        private void btNav_Click(object sender, EventArgs e)
        {
            Byte[] data = new byte[8];
            NetDataHandle.Send_To_FK(0x0, 0xE2, data);
        }
    }
}
