using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Model;

namespace ATSJianMianJK.XiTong.Frm.KJ
{
    public partial class HuanCunKJ : UserControl
    {
        public HuanCunKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(HuanCunModel model)
        {
            this.textBox1.Text = model.HuanCunName;
            this.textBox7.Text = model.MoRenZhi.ToString();
            this.checkBox3.Checked = model.IsChangJiuHuanCun;
            this.checkBox1.Checked = model.IsErWeiMa;
        }
        public HuanCunModel GetModel()
        {
            HuanCunModel model = new HuanCunModel();
            model.HuanCunName = this.textBox1.Text;
            model.MoRenZhi = this.textBox7.Text;
            model.IsChangJiuHuanCun = this.checkBox3.Checked;
            model.IsErWeiMa = this.checkBox1.Checked;
            return model;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }
    }
}
