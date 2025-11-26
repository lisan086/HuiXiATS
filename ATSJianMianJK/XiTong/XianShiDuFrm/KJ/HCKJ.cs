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

namespace ATSJianMianJK.XiTong.XianShiDuFrm.KJ
{
    public partial class Huan_ : UserControl
    {
        private HuanCunModel HuanCunModel;
        public Huan_()
        {
            InitializeComponent();
        }

        public void SetCanShu(HuanCunModel model)
        {
            HuanCunModel = model;
            this.label3.Text = model.HuanCunName;
            this.label5.Text = model.MoRenZhi.ToString();
            this.textBox1.Text = model.Value.ToString();
            this.checkBox1.Checked = model.IsErWeiMa;
            this.checkBox2.Checked = model.IsChangJiuHuanCun;
        }

        public void ShuaXin()
        {
            this.textBox1.Text = HuanCunModel.Value.ToString();
        }
    }
}
