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
using ZhongWangSheBei.Model;

namespace XiangTongChuanKouSheBei.Frm
{
    public partial class SheBeiKJ : UserControl
    {
        private ZSModel ZSModel;
        public SheBeiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(ZSModel zSModel)
        {
            ZSModel = ChangYong.FuZhiShiTi(zSModel);
            this.txbMoShi.Text = zSModel.IpOrCom;
            this.textBox1.Text = zSModel.Port.ToString();
            this.textBox3.Text = zSModel.SheBeiID.ToString();
            this.textBox4.Text = zSModel.QieHuanTime.ToString();
        }

        public ZSModel GetCanShu()
        {
            ZSModel model = ChangYong.FuZhiShiTi(ZSModel);
            model.IpOrCom = this.txbMoShi.Text;
            model.Port = ChangYong.TryInt(this.textBox1.Text,9600);
            model.QieHuanTime= ChangYong.TryInt(this.textBox4.Text, 80);
            model.SheBeiID = ChangYong.TryInt(this.textBox3.Text, 1);
            return model;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DuJiCunQiFrm frm = new DuJiCunQiFrm();
            frm.SetCanShu(ZSModel.LisSheBei);
            if (frm.ShowDialog(this)==DialogResult.OK)
            {
                ZSModel.LisSheBei = ChangYong.FuZhiShiTi(frm.GetSheBei());
            }
        }
    }
}
