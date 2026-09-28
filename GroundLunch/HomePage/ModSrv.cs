using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroundLunch
{
    public partial class ModSrv : DevExpress.XtraEditors.XtraUserControl
    {
        List<DataTable> DataTableSrvs = new List<DataTable>();
        DataTable DataTableAirSpd = new DataTable();
        bool userSetting = false;
        Timer timer = new Timer();
        public ModSrv()
        {
            InitializeComponent();
            CreateDataTableSrv();
        }

        private void ModSrv_Load(object sender, EventArgs e)
        {
            InitChart(chartSrv1, DataTableSrvs[0]);
            InitChart(chartSrv2, DataTableSrvs[1]);
            InitChart(chartSrv3, DataTableSrvs[2]);
            InitChart(chartAirSpd, DataTableAirSpd);
        }

        private void CreateDataTableSrv()
        {
            for (int i = 0; i < 6; i+=2)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("second", typeof(double));
                string name1 = string.Format("舵机{0}线位移指令", i + 1);
                string name2 = string.Format("舵机{0}线位移反馈", i + 1);
                string name3 = string.Format("舵机{0}线位移指令", i + 2);
                string name4 = string.Format("舵机{0}线位移反馈", i + 2);
                dt.Columns.Add(name1, typeof(double));
                dt.Columns.Add(name2, typeof(double));
                dt.Columns.Add(name3, typeof(double));
                dt.Columns.Add(name4, typeof(double));
                DataTableSrvs.Add(dt);
            }
            DataTableAirSpd.Columns.Add("second", typeof(double));
            DataTableAirSpd.Columns.Add("空速", typeof(double));
            DataTableAirSpd.Columns.Add("校准", typeof(double));
        }

        private void InitChart(ChartControl chatCtrl, DataTable dt)
        {
            chatCtrl.Series.Clear();

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                Series se = new Series(dt.Columns[i].ColumnName, ViewType.Line);
                se.DataSource = dt;
                se.ArgumentDataMember = "second";
                se.ValueDataMembers.AddRange(dt.Columns[i].ColumnName);
                chatCtrl.Series.Add(se);
                ((XYDiagram)chatCtrl.Diagram).AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;

                AxisY firstY = ((XYDiagram)chatCtrl.Diagram).AxisY;
                //firstY.WholeRange.SetMinMaxValues(-32, 32);
                
            }
            if(chatCtrl.Series.Count > 1)
                ((LineSeriesView)chatCtrl.Series[1].View).Color = Color.Yellow;
        }
        private void UpdateFrameToChart()
        {
            int j = 0;
            for (int i = 0; i < 3; i++)
            {
                while (DataTableSrvs[i].Rows.Count > 0 &&
                NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime - Convert.ToDouble(DataTableSrvs[i].Rows[0].ItemArray[0].ToString()) >= 10.0)
                {
                    DataTableSrvs[i].Rows.RemoveAt(0);
                }
                if (DataTableSrvs[i].Rows.Count > 0 && NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime - Convert.ToDouble(DataTableSrvs[i].Rows[0].ItemArray[0].ToString()) < 0.0)
                {
                    DataTableSrvs[i].Rows.Clear();
                }

                DataRow dr = DataTableSrvs[i].NewRow();
                List<object> objlist = new List<object>();
                objlist.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime.ToString("0.###"));
                objlist.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCtrl[j]);
                objlist.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCur[j]);
                objlist.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCtrl[j + 1]);
                objlist.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCur[j + 1]);
                j += 2;
                object[] rowArray = objlist.ToArray();
                dr.ItemArray = rowArray;
                DataTableSrvs[i].Rows.Add(dr);
            }
            while (DataTableAirSpd.Rows.Count > 0 &&
               NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime - Convert.ToDouble(DataTableAirSpd.Rows[0].ItemArray[0].ToString()) >= 10.0)
            {
                DataTableAirSpd.Rows.RemoveAt(0);
            }
            if (DataTableAirSpd.Rows.Count > 0 && NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime - Convert.ToDouble(DataTableAirSpd.Rows[0].ItemArray[0].ToString()) < 0.0)
            {
                DataTableAirSpd.Rows.Clear();
            }

            DataRow dr1 = DataTableAirSpd.NewRow();
            List<object> objlist1 = new List<object>();
            objlist1.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].flightTime.ToString("0.###"));
            objlist1.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].airSpd);
            objlist1.Add(NetDataHandle.groundFrame[NetDataHandle.curSelMsn].airClibration);
            object[] rowArray1 = objlist1.ToArray();
            dr1.ItemArray = rowArray1;
            DataTableAirSpd.Rows.Add(dr1);

        }
        public void RefreshUI()
        {
            UpdateFrameToChart();
            labelCur1.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCur[0].ToString("F1");
            labelCur2.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCur[2].ToString("F1");
            labelCur3.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCur[4].ToString("F1");
            //labelCur4.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCur[3].ToString("F1");

            if (userSetting == false)
            {
               // spin1MM.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCtrl[0].ToString("F1");
             //   spin2MM.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCtrl[2].ToString("F1");
              //  spin3MM.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCtrl[4].ToString("F1");
                //spin4MM.Text = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].srvCtrl[3].ToString("F1");
            }
        }

        private void spin1MM_EditValueChanged(object sender, EventArgs e)
        {
            userSetting = true;
        }

        private void spin2MM_EditValueChanged(object sender, EventArgs e)
        {
            userSetting = true;
        }

        private void spin3MM_EditValueChanged(object sender, EventArgs e)
        {
            userSetting = true;
        }

        private void spin4MM_EditValueChanged(object sender, EventArgs e)
        {
            userSetting = true;
        }

        private void btSetAll_Click(object sender, EventArgs e)
        {
            //延迟1秒设置为刷新模式
            timer.Interval = 1000;
            timer.Tick += new EventHandler(OnTimerFresh);
            timer.Start();

            Byte[] data = new Byte[12];
            short srv1 = (short)(((double)spin1MM.Value) * 100);
            short srv2 = (short)(srv1 * -1);
            short srv3 = (short)(((double)spin2MM.Value) * 100);
            short srv4 = srv3;
            short srv5 = (short)(((double)spin3MM.Value) * 100);
            short srv6 = srv5;
            Byte[] temp = new Byte[2];
            temp = BitConverter.GetBytes(srv1);
            Buffer.BlockCopy(temp, 0, data, 0, 2);
            temp = BitConverter.GetBytes(srv2);
            Buffer.BlockCopy(temp, 0, data, 2, 2);
            temp = BitConverter.GetBytes(srv3);
            Buffer.BlockCopy(temp, 0, data, 4, 2);
            temp = BitConverter.GetBytes(srv4);
            Buffer.BlockCopy(temp, 0, data, 6, 2);
            temp = BitConverter.GetBytes(srv5);
            Buffer.BlockCopy(temp, 0, data, 8, 2);
            temp = BitConverter.GetBytes(srv6);
            Buffer.BlockCopy(temp, 0, data, 10, 2);
            NetDataHandle.Send_To_FK(12, 0x42, data);
        }

        private void OnTimerFresh(object sender, EventArgs e)
        {
            userSetting = false;
            timer.Stop();
        }

        private void btCycle5_Click(object sender, EventArgs e)
        {
            ushort srvHz;
            switch ((srvHzCombo.SelectedIndex))
            {
                case 0:
                    srvHz = (ushort)(0.1 * 100);
                    break;
                case 1:
                    srvHz = (ushort)(0.5 * 100);
                    break;
                case 2:
                    srvHz = (ushort)(1 * 100);
                    break;
                case 3:
                    srvHz = (ushort)(2 * 100);
                    break;
                case 4:
                    srvHz = (ushort)(5 * 100);
                    break;
                default: 
                    srvHz = 0; 
                    break;

            }
            ushort width = (ushort)((double)5 * 10);//5°
            ushort count = (ushort)(2);
            int[] enable = new int[6];
            enable[0] = 1;
            enable[1] = 1;
            enable[2] = 1;
            enable[3] = 1;
            enable[4] = 1;
            enable[5] = 1;
            int a = enable[5] * 32 + enable[4] * 16 + enable[3] * 8 + enable[2] * 4 + enable[1] * 2 + enable[0];
            Byte enableByte = (Byte)a;

            Byte[] data = new Byte[20];
            Byte[] temp = new Byte[2];
            temp = BitConverter.GetBytes(srvHz);
            Buffer.BlockCopy(temp, 0, data, 0, 2);
            temp = BitConverter.GetBytes(width);
            Buffer.BlockCopy(temp, 0, data, 2, 2);
            temp = BitConverter.GetBytes(count);
            Buffer.BlockCopy(temp, 0, data, 4, 2);
            data[6] = enableByte;
            data[7] = 0;
            data[8] = 0;

            NetDataHandle.Send_To_FK(9, 0x44, data);
        }

        private void btCycle10_Click(object sender, EventArgs e)
        {
            ushort srvHz;
            switch ((srvHzCombo.SelectedIndex))
            {
                case 0:
                    srvHz = (ushort)(0.1 * 100);
                    break;
                case 1:
                    srvHz = (ushort)(0.5 * 100);
                    break;
                case 2:
                    srvHz = (ushort)(1 * 100);
                    break;
                case 3:
                    srvHz = (ushort)(2 * 100);
                    break;
                case 4:
                    srvHz = (ushort)(5 * 100);
                    break;
                default:
                    srvHz = 0;
                    break;

            }
            ushort width = (ushort)((double)10 * 10);//10°
            ushort count = (ushort)(2);
            int[] enable = new int[6];
            enable[0] = 1;
            enable[1] = 1;
            enable[2] = 1;
            enable[3] = 1;
            enable[4] = 1;
            enable[5] = 1;
            int a = enable[5] * 32 + enable[4] * 16 + enable[3] * 8 + enable[2] * 4 + enable[1] * 2 + enable[0];
            Byte enableByte = (Byte)a;

            Byte[] data = new Byte[20];
            Byte[] temp = new Byte[2];
            temp = BitConverter.GetBytes(srvHz);
            Buffer.BlockCopy(temp, 0, data, 0, 2);
            temp = BitConverter.GetBytes(width);
            Buffer.BlockCopy(temp, 0, data, 2, 2);
            temp = BitConverter.GetBytes(count);
            Buffer.BlockCopy(temp, 0, data, 4, 2);
            data[6] = enableByte;
            data[7] = 0;
            data[8] = 0;

            NetDataHandle.Send_To_FK(9, 0x44, data);
        }
    }

    
}
