using DevExpress.XtraScheduler.Reporting;
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
    public partial class FormDataLink : DevExpress.XtraEditors.XtraForm
    {
        static public List<DataLinkTerminal> linkTerminals = new List<DataLinkTerminal>();
        public FormDataLink()
        {
            InitializeComponent();
            InitTerminal();
        }

        public void InitTerminal()
        {
            linkTerminals.Add(dataLinkTerminal1);
            linkTerminals.Add(dataLinkTerminal2);
            linkTerminals.Add(dataLinkTerminal3);
            linkTerminals.Add(dataLinkTerminal4);
            linkTerminals.Add(dataLinkTerminal5);
            linkTerminals.Add(dataLinkTerminal6);
            linkTerminals.Add(dataLinkTerminal7);
            linkTerminals.Add(dataLinkTerminal8);

            for (int i = 0; i < linkTerminals.Count; i++)
            {
                linkTerminals[i].TerminalID = 0x92 + i;
                linkTerminals[i].paoID = i + 1;
            }
        }

        private void FormDataLink_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;        // 阻止关闭
            this.Hide();            // 隐藏而不是释放
        }
    }
}
