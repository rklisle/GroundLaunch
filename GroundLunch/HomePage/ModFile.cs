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
    public partial class ModFile : DevExpress.XtraEditors.XtraUserControl
    {
        public ModFile()
        {
            InitializeComponent();
        }

        public void RefreshUI()
        {
            labelLanuchLon.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].lanuchLon.ToString("0.#######");
            labelLanuchLat.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].lanuchLat.ToString("0.#######");
            labelLanuchHigh.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].lanuchHigh.ToString("0.##");
            labelLanuchAzi.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].lanuchAzimuth.ToString("0.#");
            labelLanuchPitch.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].lanuchPitch.ToString("0.#");

            //星历装订状态
            switch(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].gpsEphemeris)
            {
                case 1://注入中
                    labelEmph.ImageOptions.ImageIndex = 2;
                    break;
                case 2://成功
                    labelEmph.ImageOptions.ImageIndex = 0;
                    break;
                case 3://失败
                    labelEmph.ImageOptions.ImageIndex = 4;
                    break;
                case 4://超时
                    labelEmph.ImageOptions.ImageIndex = 3;
                    break;
                case 0://未开始
                    labelEmph.ImageOptions.ImageIndex = 1;
                    break;
            }
            
            //预发射状态
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].readyForLanuch == 1)
            {
                labelReady.ImageOptions.ImageIndex = 0;
            }
            else
            {
                labelReady.ImageOptions.ImageIndex = 1;
            }
            //sd卡状态
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].sdState == 0xAA)
            {
                labelSDState.ImageOptions.ImageIndex = 0;
            }
            else
            {
                labelSDState.ImageOptions.ImageIndex = 1;
            }
            //拖插检测状态
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].IOState[0] == 1)
            {
                labelIO.ImageOptions.ImageIndex = 0;
            }
            else
            {
                labelIO.ImageOptions.ImageIndex = 1;
            }
        }

        private void labelSDState_Click(object sender, EventArgs e)
        {
            //初始化SD卡
            Byte[] data = new Byte[1];
            NetDataHandle.Send_To_FK(0, 0x51, data);
        }
    }
}
