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
using ModBusRTU.Frm.KJ;
using SundyChengZhong.Model;

namespace SundyChengZhong.Frm.KJ
{
    public partial class CZKJ : UserControl
    {
        private SheBeiModel LSModel;
        public CZKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(SheBeiModel lsmodel)
        {
            LSModel = ChangYong.FuZhiShiTi(lsmodel);
            this.txbMoShi.Text = LSModel.Com;
            this.textBox1.Text = LSModel.Port.ToString();
            this.textBox2.Text = LSModel.SheBeiID.ToString();
            this.textBox4.Text = LSModel.QiHuanTime.ToString();
            this.textBox3.Text = LSModel.SheBeiName;
            this.textBox5.Text = LSModel.XieYanShi.ToString();
            this.textBox6.Text = LSModel.DuYanShi.ToString();
        }

        public SheBeiModel GetModel()
        {
            SheBeiModel lsmodel = ChangYong.FuZhiShiTi(LSModel);
            lsmodel.Com = this.txbMoShi.Text;
            lsmodel.Port = ChangYong.TryInt(this.textBox1.Text, 9600);
            lsmodel.SheBeiID = ChangYong.TryInt(this.textBox2.Text, 1);
            lsmodel.QiHuanTime = ChangYong.TryInt(this.textBox4.Text, 80);
            lsmodel.SheBeiName = this.textBox3.Text;
            lsmodel.XieYanShi= ChangYong.TryInt(this.textBox5.Text, 80);
            lsmodel.DuYanShi = ChangYong.TryInt(this.textBox6.Text, 300);
            return lsmodel;
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
            FenKuaiFrm kj = new FenKuaiFrm();
            kj.SetCanShu(LSModel.LisJiCunQis);
            if (kj.ShowDialog(this) == DialogResult.OK)
            {
                LSModel.LisJiCunQis = ChangYong.FuZhiShiTi(kj.GetShuJu());
            }
        }
    }
}
