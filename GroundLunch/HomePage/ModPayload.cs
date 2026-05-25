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
    public partial class ModPayload : DevExpress.XtraEditors.XtraUserControl
    {
        public ModPayload()
        {
            InitializeComponent();
        }

        public void RefreshUI()
        {
            labelMsn.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[0] > 0 ? 1 : 0;
            labelImu.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[1] > 0 ? 1 : 0;
            labelBattery.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[2] > 0 ? 1 : 0;
            labelEngine.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[3] > 0 ? 1 : 0;
            labelNav.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[4] > 0 ? 1 : 0;

            labelSrv1.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[8] > 0 ? 1 : 0;
            labelSrv2.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[9] > 0 ? 1 : 0;
            labelSrv3.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[10] > 0 ? 1 : 0;
            labelSrv4.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[11] > 0 ? 1 : 0;
            labelSrv5.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[12] > 0 ? 1 : 0;
            labelSrv6.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[13] > 0 ? 1 : 0;
            labelUm.ImageOptions.ImageIndex = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[14] > 0 ? 1 : 0;

            switch(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].lastCmd) 
            {
                case 0x05:
                    labelLastCmd.Text = "上一条指令: 单机配电";
                    break;
                case 0xE0:
                    labelLastCmd.Text = "上一条指令: 水平计算";
                    break;
                case 0xE2:
                    labelLastCmd.Text = "上一条指令: 转导航";
                    break;
                case 0xF8:
                    labelLastCmd.Text = "上一条指令: 预发射";
                    break;
                case 0x64:
                    labelLastCmd.Text = "上一条指令:FLASH查询";
                    break;
                case 0xFA:
                    labelLastCmd.Text = "上一条指令: 发射";
                    break;
                default:
                    labelLastCmd.Text = "上一条指令: 无";
                    break;
            }

            labelBatteryStatus.ForeColor = Color.White;
            string showText = "电池状态未知";

            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[2] > 0)
            {
                if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].onceBatteryVOK == 1)
                {
                    labelBatteryStatus.ForeColor = Color.Lime;
                    showText = "电池温度正常";
                }
                else if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].onceBatteryVOK == 2)
                {
                    labelBatteryStatus.ForeColor = Color.Yellow;
                    showText = "电池温度不足";
                }
                if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].onceBatteryTempStatus == 1)
                {
                    labelBatteryStatus.ForeColor = Color.Yellow;
                    showText = "电池加热中";
                }
                if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].onceBatteryError == 1)
                {
                    labelBatteryStatus.ForeColor = Color.Red;
                    showText = "电池加热故障";
                }
                if (showText != "电池状态未知")
                    showText = showText + " " + NetDataHandle.groundFrame[NetDataHandle.curSelMsn].onceBatteryV.ToString("F1") + "V";
            }
            labelBatteryStatus.Text = showText;
        }
    }
}
