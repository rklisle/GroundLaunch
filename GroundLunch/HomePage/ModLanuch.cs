using DevExpress.XtraEditors;
using DevExpress.XtraScheduler.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace GroundLunch
{
    public partial class ModLanuch : DevExpress.XtraEditors.XtraUserControl
    {
        public ModLanuch()
        {
            InitializeComponent();
        }

        public void RefreshUI()
        {
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].readyForLanuch == 1)
            {
                btReady.Text = "退出预发射";
                btIgnation.Enabled = true;
            }
            else
            {
                btReady.Text = "预发射";
                btIgnation.Enabled = false;
            }
            
            int state = NetDataHandle.groundFrame[NetDataHandle.curSelMsn].connState[7];
            if (state == 0x22)
            {
                labelFuseState.Text = "引信正常";
                labelFuseState.Visible = true;
                labelFuseState.ForeColor = System.Drawing.Color.Lime;
            }
            else if (state == 0xDD)
            {
                labelFuseState.Text = "引信异常";
                labelFuseState.Visible = true;
                labelFuseState.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                labelFuseState.Visible = false;
            }
        }

        private void btIgnation_Click(object sender, EventArgs e)
        {
            labelIgnation.Visible = true;
            Byte[] data = new byte[5];
            data[0] = 0xAA;
            data[1] = 0xBB;
            data[2] = 0xCC;
            data[3] = 0xDD;
            data[4] = 0xEE;

            NetDataHandle.Send_To_FK(5, 0xFA, data);
        }

        private void btReady_Click(object sender, EventArgs e)
        {
            Byte[] data = new byte[1];
            if (NetDataHandle.groundFrame[NetDataHandle.curSelMsn].readyForLanuch == 1)
            {
                data[0] = 0x22;
            }
            else
            {
                data[0] = 0x11;
            }
            
            NetDataHandle.Send_To_FK(1, 0xF8, data);

            //初始化SD卡,在预发射时候补充初始化一次
            Byte[] data1 = new Byte[1];
            NetDataHandle.Send_To_FK(0, 0x51, data1);
        }

        private void labelIgnation_Click(object sender, EventArgs e)
        {
            if(labelIgnation.Text == "已发射") 
            {
                ((MainForm)(this.Parent.Parent.Parent.Parent.Parent)).JumpToTM();
            }
        }

        static int IndexOfBytes(byte[] source, byte[] search)
        {
            for (int i = 0; i <= source.Length - search.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < search.Length; j++)
                {
                    if (source[i + j] != search[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return i;
            }
            return -1;
        }

        private void UploadGpsEphFile(string fileName)
        {
            Byte[] fileBytes = File.ReadAllBytes(fileName);
            string searchString = "OK!";
            int index = IndexOfBytes(fileBytes, Encoding.ASCII.GetBytes(searchString));
            if (index != -1)
            {
                Byte[] data = new byte[index + 3];
                data[0] = 0;
                ushort len = (ushort)index;
                Byte[] lenByte = BitConverter.GetBytes(len);
                data[1] = lenByte[0];
                data[2] = lenByte[1];   
                Buffer.BlockCopy(fileBytes,0,data,3,index);
                NetDataHandle.Send_To_FK((ushort)(index + 3), 0xe9, data);
            }
        }

        private void UploadBdEphFile(string fileName)
        {
            Byte[] fileBytes = File.ReadAllBytes(fileName);
            string searchString = "OK!";
            int index = IndexOfBytes(fileBytes, Encoding.ASCII.GetBytes(searchString));
            if (index != -1)
            {
                Byte[] data = new byte[index + 3];
                data[0] = 1;
                ushort len = (ushort)index;
                Byte[] lenByte = BitConverter.GetBytes(len);
                data[1] = lenByte[0];
                data[2] = lenByte[1];
                Buffer.BlockCopy(fileBytes, 0, data, 3, index);
                NetDataHandle.Send_To_FK((ushort)(index + 3), 0xe9, data);
            }
        }

        private void btUpEph_Click(object sender, EventArgs e)
        {
            //弹出对话框选择星历文件
            XtraOpenFileDialog openFileDialog = new XtraOpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                if (selectedFilePath.Contains("gps.txt"))
                {
                    UploadGpsEphFile(selectedFilePath);
                }
                else if (selectedFilePath.Contains("bd.txt"))
                {
                    UploadBdEphFile(selectedFilePath);
                }
                else
                {
                    return ;
                }
            }
        }
    }
}
