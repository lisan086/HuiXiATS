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

namespace ZuZhuangUI.PeiZhi.KJ
{
    public partial class KongZhiKJ : UserControl
    {
        private YeWuDataModel YeWuDataModel;
        public KongZhiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(YeWuDataModel wuDataModel,string kaitou)
        {
            this.commBoxE1.Items.Clear();
         
            List<string> lisd = ChangYong.MeiJuLisName(typeof(CaoZuoType));
            for (int i = 0; i < lisd.Count; i++)
            {
                if (lisd[i].Contains("DataShangChuan")==false)
                {
                    if (lisd[i].StartsWith(kaitou))
                    {
                        this.commBoxE1.Items.Add(lisd[i]);
                    }
                }
            }
            if (this.commBoxE1.Items.Count > 0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
            this.commBoxE2.SelectedIndex = 0;
            YeWuDataModel = ChangYong.FuZhiShiTi(wuDataModel);
            this.textBox2.Text = wuDataModel.ItemName;
            this.xuanZeDuXieKJ3.SetCanShu(wuDataModel.Value);
            this.textBox7.Text = wuDataModel.QingQiuPiPei;
            this.textBox8.Text = wuDataModel.NGZhi;
            this.textBox1.Text = wuDataModel.QingLingZhi;
            this.textBox3.Text = wuDataModel.PassZhi;
            this.commBoxE1.Text= wuDataModel.CaoZuoType.ToString();
            this.commBoxE2.Text = wuDataModel.YongDeMaMingCheng;
        }

        public YeWuDataModel GetCanShu()
        {
            YeWuDataModel model = ChangYong.FuZhiShiTi(YeWuDataModel);
            model.CaoZuoType = this.commBoxE1.GetCanShu<CaoZuoType>();
            model.CodeOrNo = this.textBox12.Text;
            model.ItemName = this.textBox2.Text;        
            model.Value = this.xuanZeDuXieKJ3.GetCanShu();
           // model.Value.JiCunValue = "";
            model.QingLingZhi = this.textBox1.Text;
            model.QingQiuPiPei = this.textBox7.Text;
            model.NGZhi = this.textBox8.Text;
            model.PassZhi = this.textBox3.Text;
            model.YongDeMaMingCheng = this.commBoxE2.Text;
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
