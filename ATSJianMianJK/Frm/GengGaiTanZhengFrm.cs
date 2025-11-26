using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JieMianLei.FuFrom;

namespace ATSJianMianJK.Frm
{
    public partial class GengGaiTanZhengFrm : BaseFuFrom
    {
        public bool GaoPing { get; set; } = false;
        public bool DiPing { get; set; } = false;
        public GengGaiTanZhengFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GaoPing = this.checkBox1.Checked;
            DiPing = this.checkBox2.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
