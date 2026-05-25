using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
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
    public partial class OnePlane : DevExpress.XtraEditors.XtraUserControl
    {
        public int guanID;
        public int paoID;
        public int step = -1;
        public byte linkDbm;
        public OnePlane()
        {
            InitializeComponent();
            
        }

        public void DrawRect()
        {
            if (NetDataHandle.curPao == paoID && NetDataHandle.curGuan == guanID)
            {
                // 最简单的绘制线条
                Graphics g = groupControl1.CreateGraphics();
                Pen pen = new Pen(Color.Lime, 2);
                g.DrawLine(pen, 0, 0, 340, 0);
                g.DrawLine(pen, 340, 0, 340, 276);
                g.DrawLine(pen, 340, 276, 0, 276);
                g.DrawLine(pen, 0, 276, 0, 0);

                // 记得释放资源（或者使用using语句）
                pen.Dispose();
                g.Dispose();
            }
            else
            {
                groupControl1.Invalidate();
                //Graphics g = groupControl1.CreateGraphics();
                //g.Clear(this.BackColor); // 清除为背景色
            }
        }

        public void RefreshUI(int paoID, int guanID)
        {
            this.guanID = guanID;
            this.paoID = paoID;
            groupControl1.Text = string.Format("Gun Barrel ID: {0}",guanID.ToString("D2"));
            DrawRect();
            if (NetDataHandle.planeConnectStatus[(paoID, guanID)] != 0)
            {
                NetDataHandle.planeConnectStatus[(paoID, guanID)]--;
                if (NetDataHandle.planeConnectStatus[(paoID, guanID)] == 0)
                {
                    picPlane.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("./pic/导弹_灰.svg");
                    step = -1;
                    return;
                }
                double v = 0, a = 0, rpm = 0, groupid = 0, msnid = 0, pitch = 0;
                double lon = 0, lat = 0, alt = 0, roll = 0, yaw = 0;
                int curStep = 0, luanchRecv = 0, startFly = 0, payloadtp = 0, guaID = 0, scCnt = 0, navState = 0, engState = 0;
                foreach (TMParam param in TMHandler.allParamList[(paoID, guanID)])
                {
                    if (param.paramID == "ecu24V")
                    {
                        v = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "ecu24A")
                    {
                        a = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "msnGrpID")
                    {
                        groupid = Convert.ToInt16(param.物理量);
                    }
                    if (param.paramID == "msnDevID")
                    {
                        msnid = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "ecuGetRp")
                    {
                        rpm = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "navPitch" && param.paramDataPool == "imu")
                    {
                        pitch = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "autoStep")
                    {
                        curStep = Convert.ToInt32(param.物理量);
                    }
                    if (param.paramID == "RecvLunc")
                    {
                        luanchRecv = Convert.ToInt32(param.物理量);
                    }
                    if (param.paramID == "startFly")
                    {
                        startFly = Convert.ToInt32(param.物理量);
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
                    if (param.paramID == "navRoll" && param.paramDataPool == "imu")
                    {
                        roll = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "navDir" && param.paramDataPool == "imu")
                    {
                        yaw = Convert.ToDouble(param.物理量);
                    }
                    if (param.paramID == "paylodtp")
                    {
                        payloadtp = Convert.ToInt32(param.物理量);
                    }
                    if (param.paramID == "msnGuaID")
                    {
                        guaID = Convert.ToInt32(param.物理量);
                    }
                    if (param.paramID == "gpsScCnt")
                    {
                        scCnt = Convert.ToInt32(param.物理量);
                    }
                    if (param.paramID == "navState" && param.paramDataPool == "imu")
                    {
                        navState = Convert.ToInt32(param.物理量);
                    }
                    if (param.paramID == "ecuState")
                    {
                        engState = Convert.ToInt32(param.物理量);
                    }
                }

                Color labelColor = Color.White;
                if (step != curStep)
                {
                    if (curStep == 11 || curStep == 12)
                    {
                        picPlane.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("./pic/导弹_绿.svg");
                    }
                    else if (curStep == 13)
                    {
                        picPlane.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("./pic/导弹_橙.svg");
                    }
                    else if (curStep > 13)
                    {
                        picPlane.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("./pic/导弹_红.svg");
                    }
                    else
                    {
                        picPlane.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("./pic/导弹_蓝.svg");
                    }

                    
                }
                /*
                if (startFly != isFly)
                {
                    if (startFly == 1)
                    {
                        picPlane.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("./pic/导弹_橙.svg");
                    }
                }
                isFly = startFly;*/
                step = curStep;
                labelV.Text = v.ToString("F1") + "V";
                labelA.Text = a.ToString("F1") + "A";
                labelRpm.Text = "rpm: " + rpm.ToString("F0");
                labelPitch.Text = "pitch: " + pitch.ToString("F1") + "°";
                labelLon.Text = "Lon: " + lon.ToString("F5") + "°";
                labelLat.Text = "Lat: " + lat.ToString("F5") + "°";
                labelAlt.Text = "Alt: " + alt.ToString("F1") + "m";
                labelRoll.Text = "Roll : " + roll.ToString("F1") + "°";
                labelYaw.Text = "Yaw: " + yaw.ToString("F1") + "°";
                labelpaylodtp.Text = "Payload: " + payloadtp.ToString("F0");
                labelScCnt.Text = "Sats: " + scCnt.ToString("F0");
                labelnavState.Text = "Nav: " + navState.ToString("F0");
                labelEngSate.Text = "Eng: " + engState.ToString("F0");

                if (curStep < 4)
                {
                    labelMsnID.Text = "Check step:" + curStep.ToString();
                    //labelMsnID.ForeColor = Color.White;

                }
                else if (curStep == 4)
                {
                    labelMsnID.Text = "Upload Msn";
                    labelColor = Color.Lime;
                    // labelMsnID.ForeColor = Color.Lime;                  
                }
                else if (curStep == 6)
                {
                    labelMsnID.Text = "Focusing..";
                    labelColor = Color.Yellow;
                    labelmsnControl.Text = groupid.ToString("F0") + "-" + msnid.ToString("F0");
                }
                else if (curStep == 8)
                {
                    labelMsnID.Text = "Eng-W-Start";
                    labelColor = Color.Lime;
                }
                else if (curStep == 9)
                {
                    labelMsnID.Text = "Eng-Starting";
                    labelColor = Color.Lime;
                }
                else if (curStep == 11)
                {
                    labelMsnID.Text = "W-Unlock";
                    labelColor = Color.Lime;
                }
                else if (curStep == 12)
                {
                    labelMsnID.Text = "W-Launch";
                    labelColor = Color.Lime;
                }
                else if (curStep == 13)
                {
                    labelMsnID.Text = "Launched";
                    labelColor = Color.Lime;
                }
                else
                {
                    //labelMsnID.Text = groupid.ToString("F0") + "-" + msnid.ToString("F0");
                }

                if (startFly == 1)
                {
                    labelColor = Color.Orange;
                }
                else
                {
                    labelColor = Color.Lime;
                }

                if (luanchRecv != 0xEE)
                {
                    picLock.Visible = true;
                }
                else
                {
                    picLock.Visible = false;
                }
                labelV.ForeColor = labelColor;
                labelA.ForeColor = labelColor;
                labelRpm.ForeColor = labelColor;
                labelPitch.ForeColor = labelColor;
                labelLon.ForeColor = labelColor;
                labelLat.ForeColor = labelColor;
                labelAlt.ForeColor = labelColor;
                labelRoll.ForeColor = labelColor;
                labelYaw.ForeColor = labelColor;
                labelMsnID.ForeColor = labelColor;
                labelpaylodtp.ForeColor = labelColor;
                labelError.ForeColor = labelColor;
                labelScCnt.ForeColor = labelColor;
                labelnavState.ForeColor = labelColor;
                labelEngSate.ForeColor = labelColor;
                labelmsnControl.ForeColor = labelColor;
            }
        }

        public void SelectSelf()
        {
            NetDataHandle.curPao = paoID;
            NetDataHandle.curGuan = guanID;
        }

        private void picPlane_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void labelLat_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void labelA_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void labelRpm_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void labelAlt_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void labelControl8_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void labelScCntControl(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void groupControl1_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void panelControl1_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            SelectSelf();
        }
    }
}
