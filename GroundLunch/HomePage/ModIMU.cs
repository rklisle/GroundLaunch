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
    public partial class ModIMU : DevExpress.XtraEditors.XtraUserControl
    {
        public ModIMU()
        {
            InitializeComponent();
        }

        public void RefreshUI()
        {
            //刷新加速度控件
            labelAx.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAx.ToString("0.##");
            labelAy.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAy.ToString("0.##");
            labelAz.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAz.ToString("0.##");
            progressX.Position = (int)(Math.Abs(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAx) * 10);
            progressY.Position = (int)(Math.Abs(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAy) * 10);
            progressZ.Position = (int)(Math.Abs(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAz) * 10);
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAx < 0)
            {
                progressX.LookAndFeel.SkinMaskColor = Color.YellowGreen;
            }
            else
            {
                progressX.LookAndFeel.SkinMaskColor = Color.Transparent;
            }
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAy < 0)
            {
                progressY.LookAndFeel.SkinMaskColor = Color.YellowGreen;
            }
            else 
            {
                progressY.LookAndFeel.SkinMaskColor = Color.Transparent;
            }
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuAz < 0)
            {
                progressZ.LookAndFeel.SkinMaskColor = Color.YellowGreen;
            }
            else
            {
                progressZ.LookAndFeel.SkinMaskColor = Color.Transparent;
            }
            //刷新角速度控件
            try
            {
                labelWx.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuWx.ToString("0.##");
                arcScaleWx.Value = Math.Abs((int)(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuWx / 15.0 * 100));

                labelWy.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuWy.ToString("0.##");
                arcScaleWy.Value = Math.Abs((int)(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuWy / 15.0 * 100));

                labelWz.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuWz.ToString("0.##");
                arcScaleWz.Value = Math.Abs((int)(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].imuWz / 15.0 * 100));
            }
            catch (Exception )
            {
                ;
            }
        }
    }
}
