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
using ZuZhuangUI.Model;

namespace ATSZuZhuangUI.PeiZhi.Frm
{
    public partial class XuanZeSheBeiFrm : BaseFuFrom
    {
        public string JieGuo { get; set; } = "";
        public XuanZeSheBeiFrm()
        {
            InitializeComponent();
        }
        public void SetCanShu(string jieguo)
        {
            this.commBoxE1.Items.Clear();
            List<string> meijus = ChangYong.MeiJuLisName(typeof(SheBeiType));
            for (int i = 0; i < meijus.Count; i++)
            {
                this.commBoxE1.Items.Add(meijus[i]);
            }
            this.commBoxE1.Text = jieguo;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            JieGuo = this.commBoxE1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
