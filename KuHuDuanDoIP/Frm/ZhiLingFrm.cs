using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;

namespace KuHuDuanDoIP.Frm
{
    public partial class ZhiLingFrm : BaseFuFrom
    {
        public string ZhiLingType { get; set; } = "";
        public ZhiLingFrm()
        {
            InitializeComponent();
            this.commBoxE1.Items.Clear();
            List<string> lis = ChangYong.MeiJuLisName(typeof(Model.ZhiLingType));
            for (int i = 0; i < lis.Count; i++)
            {
                this.commBoxE1.Items.Add(lis[i]);
            }
            if (this.commBoxE1.Items.Count>0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
        }
        public void SetCanShu(string zhiling)
        {
            this.commBoxE1.Text = zhiling;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ZhiLingType = this.commBoxE1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
