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
using YiBanSaoMaQi.Model;

namespace YiBanDuXiePeiZhi.Frm
{
    public partial class JiCunQiKJ : UserControl
    {
        public JiCunQiKJ()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }
        public void SetCanShu(CunModel model)
        {
            this.commBoxE1.SetCanShu<CunType>();
            this.txbMoShi.Text = model.Name;
            this.textBox1.Text = model.Time.ToString();
            this.textBox4.Text = model.JieShouCount.ToString();
            this.textBox6.Text = model.MiaoSu.ToString();
            this.textBox2.Text = model.ZhiLing.ToString();
            this.commBoxE1.Text = model.IsDu.ToString();
        }
        public CunModel GetSaoMaModel()
        {
            CunModel model =new CunModel();
            model.Name = this.txbMoShi.Text;
            model.Time = ChangYong.TryInt(this.textBox1.Text, 0);
            model.JieShouCount = ChangYong.TryInt(this.textBox4.Text, 0);
            model.MiaoSu = this.textBox6.Text;
            model.ZhiLing = this.textBox2.Text;
            model.IsDu=this.commBoxE1.GetCanShu<CunType>();
            return model;
        }
    }
}
