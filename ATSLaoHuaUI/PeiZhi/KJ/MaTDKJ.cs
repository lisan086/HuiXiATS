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
using SSheBei.Model;
using ZuZhuangUI.Model;
using ZuZhuangUI.PeiZhi.Frm;

namespace ATSLaoHuaUI.PeiZhi.KJ
{
    public partial class MaTDKJ : UserControl
    {
        private MaTDModel MaTDModel;
        public MaTDKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(MaTDModel wuDataModel)
        {

            MaTDModel = ChangYong.FuZhiShiTi(wuDataModel);
            this.textBox1.Text = wuDataModel.TDName;
        
            this.textBox4.Text = wuDataModel.MaTDID.ToString();
            this.textBox2.Text = wuDataModel.CiShu.ToString();
        }

        public MaTDModel GetCanShu()
        {
            MaTDModel model = ChangYong.FuZhiShiTi(MaTDModel);
            model.TDName = this.textBox1.Text;
            model.MaTDID = ChangYong.TryInt(this.textBox4.Text,1);
            model.CiShu = ChangYong.TryInt(this.textBox2.Text, 1);
            if (model.CiShu<=0)
            {
                model.CiShu = 1;
            }
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
            YeWuShuJuFrm zhanFrm = new YeWuShuJuFrm();
            zhanFrm.SetCanShu(MaTDModel.LisKongZhi, 2);
            zhanFrm.ShowDialog(this);
            MaTDModel.LisKongZhi = ChangYong.FuZhiShiTi(zhanFrm.GetModel());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            YeWuShuJuFrm zhanFrm = new YeWuShuJuFrm();
            zhanFrm.SetCanShu(MaTDModel.LisData, 3);
            zhanFrm.ShowDialog(this);
            MaTDModel.LisData = ChangYong.FuZhiShiTi(zhanFrm.GetModel());
        }
    }
}
