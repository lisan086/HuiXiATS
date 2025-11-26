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
using ZuZhuangUI.Model;

namespace ATSZuZhuangUI.PeiZhi.KJ
{
    public partial class MaKJ : UserControl
    {
        private MaGuiZeModel MaGuiZeModel;
        public MaKJ()
        {
            InitializeComponent();
        }

        public void SetShuJu(MaGuiZeModel maGuiZe)
        {
            this.commBoxE2.Items.Clear();
            this.commBoxE2.SetCanShu<MaaType>();
            MaGuiZeModel = ChangYong.FuZhiShiTi(maGuiZe);
            this.textBox2.Text = maGuiZe.Zhi.ToString();
            this.textBox4.Text= maGuiZe.ChangDu.ToString();
            this.commBoxE2.Text=maGuiZe.MaaType.ToString();
        }

        public MaGuiZeModel GetModel()
        {
            MaGuiZeModel model = ChangYong.FuZhiShiTi(MaGuiZeModel);
            model.Zhi = this.textBox2.Text;
            model.ChangDu = ChangYong.TryInt(this.textBox4.Text,5);
            model.MaaType = this.commBoxE2.GetCanShu<MaaType>();
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
