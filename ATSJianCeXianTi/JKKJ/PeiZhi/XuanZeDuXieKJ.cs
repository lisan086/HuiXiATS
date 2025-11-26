using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Frm.FM;

namespace ATSJianCeXianTi.JKKJ.PeiZhi
{
    public partial class XuanZeDuXieKJ : UserControl
    {
        public XuanZeDuXieKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public int IsDu { get; set; } = 1;

        public string SetText
        {
            get { return this.label3.Text; }
            set 
            {
                this.label3.Text = value;
            }
        }

        public void SetCanShu(string canshu)
        {
            this.textBox2.Text = canshu;
        }
        public string GetCanShu()
        {
            return this.textBox2.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XuanZeDuFrm xuanZeDuFrm = new XuanZeDuFrm();
            xuanZeDuFrm.SetCanShu(GetCanShu(), IsDu);
            if (xuanZeDuFrm.ShowDialog(this) == DialogResult.OK)
            {
                SetCanShu(xuanZeDuFrm.MingZi);
            }
        }
    }
}
