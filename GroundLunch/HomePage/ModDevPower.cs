using DevExpress.Utils.Extensions;
using DevExpress.XtraCharts;
using SharpGL.SceneGraph.Primitives;
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
    public partial class ModDevPower : DevExpress.XtraEditors.XtraUserControl
    {
        public int powerIndex = 0;
        public string devName = "";
        DataTable DataTablePower = new DataTable();
       // DataTable DataTablePowerA = new DataTable();

        public ModDevPower()
        {
            InitializeComponent();
            CreateDataTablePower();
        }

        private void ModDevPower_Load(object sender, EventArgs e)
        {
            InitChart();
            if (this.devName == "地面供电")
            {
                btPowerOn.Text = "引信";
            }
        }

        private void CreateDataTablePower()
        {
            DataTablePower.Columns.Add("second", typeof(double));
            DataTablePower.Columns.Add("电压", typeof(double));
            //DataTablePower.Columns.Add("second", typeof(double));
            DataTablePower.Columns.Add("电流", typeof(double));
         }

        private void InitChart()
        {
            chartDevPower.Series.Clear();

            Series seV = new Series(DataTablePower.Columns[1].ColumnName, ViewType.Line);
            seV.DataSource = DataTablePower;
            seV.ArgumentDataMember = "second";
            seV.ValueDataMembers.AddRange(DataTablePower.Columns[1].ColumnName);
            chartDevPower.Series.Add(seV);

            AxisY firstY = ((XYDiagram)chartDevPower.Diagram).AxisY;
            firstY.WholeRange.SetMinMaxValues(-1, 32);
            ((XYDiagram)chartDevPower.Diagram).AxisY.Title.Text = "电压V";
            ((XYDiagram)chartDevPower.Diagram).AxisY.Title.Font = new Font("微软雅黑 Light", 11);
            ((XYDiagram)chartDevPower.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;


            Series seA = new Series(DataTablePower.Columns[2].ColumnName, ViewType.Line);
            seA.DataSource = DataTablePower;
            seA.ArgumentDataMember = "second";
            seA.ValueDataMembers.AddRange(DataTablePower.Columns[2].ColumnName);
            chartDevPower.Series.Add(seA);
            ((LineSeriesView)seA.View).Color = Color.Yellow;

            SecondaryAxisY secondY = ((XYDiagram)chartDevPower.Diagram).SecondaryAxesY[0];
            secondY.WholeRange.SetMinMaxValues(-0.1, 6.0);

            //((XYDiagram)chartDevPower.Diagram).SecondaryAxesY.Add(secondY);
            ((LineSeriesView)seA.View).AxisY = secondY;
            secondY.Label.Visible = true;
            secondY.Title.Text = "电流A";
            secondY.Title.Font = new Font("微软雅黑 Light", 11);
            secondY.Visibility = DevExpress.Utils.DefaultBoolean.True;

            chartDevPower.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        }

        private void UpdateFrameToChart()
        {
            while (DataTablePower.Rows.Count > 0 &&
                NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime - Convert.ToDouble(DataTablePower.Rows[0].ItemArray[0].ToString()) >= 10.0)
            {
                DataTablePower.Rows.RemoveAt(0);
            }
            if (DataTablePower.Rows.Count > 0 && NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime - Convert.ToDouble(DataTablePower.Rows[0].ItemArray[0].ToString()) < 0.0)
            {
                DataTablePower.Rows.Clear();
            }

            DataRow dr = DataTablePower.NewRow();
            List<object> objlist = new List<object>();
            objlist.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime.ToString("0.###"));
            objlist.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerV[powerIndex]);
            objlist.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerA[powerIndex]);
            object[] rowArray = objlist.ToArray();
            dr.ItemArray = rowArray;
            DataTablePower.Rows.Add(dr);
        }

        public void RefreshUI()
        {
            UpdateFrameToChart();
            labelName.Text = devName;
            labelVoltage.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerV[powerIndex].ToString("F2");
            labelCurrent.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerA[powerIndex].ToString("F2");
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerV[powerIndex] < 18)
            {
                if (powerIndex == 2 && NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerV[powerIndex] > 11.5
                    && NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerV[powerIndex] < 13.0)
                {
                    labelVoltage.ForeColor = Color.Lime;
                }
                else
                {
                    labelVoltage.ForeColor = Color.Red;
                }
            }
            else if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerV[powerIndex] < 28)
            {
                labelVoltage.ForeColor = Color.Yellow;
            }
            else 
            {
                labelVoltage.ForeColor = Color.Lime;
            }

            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerState[powerIndex] == 0)
            {
                btPowerOn.ImageOptions.ImageIndex = 0;
                if(this.devName != "地面供电")
                    btPowerOn.Text = "上电";
            }
            else
            {
                btPowerOn.ImageOptions.ImageIndex = 1;
                if (this.devName != "地面供电")
                    btPowerOn.Text = "下电";
                //btPowerOff.Enabled = true;
            }
        }

        private void PowerOn(int index, int onoff)
        {
            byte len, msgid;
            byte[] payload = new byte[256];
            if (onoff == 1)
            {
                payload[0] = (Byte)(0x00 + index);
            }
            else
            {
                payload[0] = (Byte)(0x00 + index * 0x10);
            }
            len = 1;
            msgid = 0x05;
            NetDataHandle.Send_To_FK(len, msgid, payload);
        }

        private void btPowerOn_Click(object sender, EventArgs e)
        {
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].powerState[powerIndex] == 0)
            {
                PowerOn(powerIndex, 1);
            }
            else
            {
                PowerOn(powerIndex, 0);
            }
        }
    }
}
