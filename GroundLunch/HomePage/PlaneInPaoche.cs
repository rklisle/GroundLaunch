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
    public partial class PlaneInPaoche : DevExpress.XtraEditors.XtraUserControl
    {
        public int paoID;
        public int paocheConnected = 0;
        public PlaneInPaoche()
        {
            InitializeComponent();
        }

        public void RefreshUI(int paoID)
        {
            this.paoID = paoID;
            for (int i = 1; i <= 12; i++)
            {
                if (NetDataHandle.planeConnectStatus[(paoID, i)] != 0)
                {
                    paocheConnected = 1;
                    break;
                }
            }

            if (paoID >= 1 && paoID <= 8)
            {
                if (FormDataLink.linkTerminals[paoID - 1].TerminalOnline > 0)
                {
                    paocheConnected = 1;
                    FormDataLink.linkTerminals[paoID - 1].TerminalOnline--;
                }
                else
                {
                    paocheConnected = 0;
                }
            }
            if (paocheConnected == 1)
            {
                btPaoID.Appearance.BackColor = Color.Green;
            }
            else
            {
                btPaoID.Appearance.BackColor = Color.DimGray;
            }
            btPaoID.Text = paoID.ToString();
            onePlane1.RefreshUI(paoID, 1);
            onePlane2.RefreshUI(paoID, 2);
            onePlane3.RefreshUI(paoID, 3);
            onePlane4.RefreshUI(paoID, 4);
            onePlane5.RefreshUI(paoID, 5);
            onePlane6.RefreshUI(paoID, 6);
            onePlane7.RefreshUI(paoID, 7);
            onePlane8.RefreshUI(paoID, 8);
            onePlane9.RefreshUI(paoID, 9);
            onePlane10.RefreshUI(paoID, 10);
            onePlane11.RefreshUI(paoID, 11);
            onePlane12.RefreshUI(paoID, 12);
        }
    }
}
