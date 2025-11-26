using JieMianLei.FuFrom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class XuanZseFrm : BaseFuFrom
    {
        public string JieGuo { get; set; } = "";
        public XuanZseFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(List<string> canshu,string xuanze)
        {
            for (int i = 0; i < canshu.Count; i++)
            {
                this.comboBox1.Items.Add(canshu[i]);
            }
            this.comboBox1.Text = xuanze;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JieGuo = this.comboBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
