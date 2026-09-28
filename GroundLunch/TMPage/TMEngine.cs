using DevExpress.XtraGauges.Core.Base;
using DevExpress.XtraGauges.Core.Drawing;
using DevExpress.XtraGauges.Core.Model;
using DevExpress.XtraGauges.Win.Base;
using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroundLunch
{
    public partial class TMEngine : DevExpress.XtraEditors.XtraUserControl
    {
        public double throttle = 1200;
        public double rpm = 4700;
        public double tempture = 50;
        public double powerV = 75;
        public double powerA = 160;
        private LabelComponent label;

        public TMEngine()
        {
            InitializeComponent();

        }

        

        public void RefreshUI()
        {
            //刷新加速度控件
            labelV.Text = powerV.ToString("F2");
            labelA.Text = powerA.ToString("F2");
            progressV.Position = (int)powerV;
            progressA.Position = (int)powerA;
            NeedleRpm.Value = (float)rpm;
            NeedleThrottle.Value = (float)throttle;
            NeedleTempture.Value = (float)tempture;
            if (powerV < 80)
            {
                progressV.LookAndFeel.SkinMaskColor = Color.Yellow;
            }
            else
            {
                progressV.LookAndFeel.SkinMaskColor = Color.Transparent;
            }
            if (powerA < 100)
            {
                progressA.LookAndFeel.SkinMaskColor = Color.Transparent;
            }
            else if(powerA < 120)
            {
                progressA.LookAndFeel.SkinMaskColor = Color.Yellow;
            }
            else if (powerA < 150)
            {
                progressA.LookAndFeel.SkinMaskColor = Color.OrangeRed;
            }
            else
            {
                progressA.LookAndFeel.SkinMaskColor = Color.Red;
            }
        }
    }
}
