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

        public void SetCanShu(YeWuDataModel wuDataModel,int biaozhi)
        {
            this.commBoxE2.Items.Clear();
         
            List<string> lisd = ChangYong.MeiJuLisName(typeof(CaoZuoType));
            for (int i = 0; i < lisd.Count; i++)
            {
                if (biaozhi == 1)
                {
                    if (lisd[i].Contains("Zong"))
                    {
                        this.commBoxE2.Items.Add(lisd[i]);
                    }
                }
                else if (biaozhi==2)
                {
                    if (lisd[i].Contains("MaXie"))
                    {
                        this.commBoxE2.Items.Add(lisd[i]);
                    }
                }
                else if (biaozhi == 3)
                {
                    if (lisd[i].Contains("MaDataShangChuan"))
                    {
                        this.commBoxE2.Items.Add(lisd[i]);
                    }
                }
            }
            if (this.commBoxE2.Items.Count > 0)
            {
                this.commBoxE2.SelectedIndex = 0;
            }
            YeWuDataModel = ChangYong.FuZhiShiTi(wuDataModel);
            this.textBox16.Text = wuDataModel.ItemName;
            this.textBox6.Text = wuDataModel.CodeOrNo;
            this.xuanZeDuXieKJ2.SetCanShu(wuDataModel.Value);
            this.textBox15.Text = wuDataModel.QingQiuPiPei;
            this.textBox14.Text = wuDataModel.NGZhi;
            this.textBox11.Text = wuDataModel.QingLingZhi;
            this.textBox13.Text = wuDataModel.PassZhi;
            this.commBoxE2.Text= wuDataModel.CaoZuoType.ToString();
            this.textBox17.Text = wuDataModel.DanWei;
            this.textBox10.Text = wuDataModel.ShangXian.ToString();
            this.textBox9.Text = wuDataModel.XiaXian.ToString();
            this.checkBox2.Checked = wuDataModel.IsHuiDu;
            this.checkBox1.Checked = wuDataModel.IsXunHuanHuanXian == 1;
            this.checkBox3.Checked = wuDataModel.IsXianShi;
            this.textBox1.Text = wuDataModel.BaoHuZhi.ToString();
        }

        public YeWuDataModel GetCanShu()
        {
            YeWuDataModel model = ChangYong.FuZhiShiTi(YeWuDataModel);
            model.CaoZuoType = this.commBoxE2.GetCanShu<CaoZuoType>();
            model.CodeOrNo = this.textBox6.Text;
            model.ItemName = this.textBox16.Text;        
            model.Value = this.xuanZeDuXieKJ2.GetCanShu();
            model.Value.JiCunValue = "";
            model.QingLingZhi = this.textBox11.Text;
            model.QingQiuPiPei = this.textBox15.Text;
            model.NGZhi = this.textBox14.Text;
            model.PassZhi = this.textBox13.Text;
            model.DanWei = this.textBox17.Text;
            model.ShangXian = ChangYong.TryDouble(this.textBox10.Text,0);
            model.XiaXian = ChangYong.TryDouble(this.textBox9.Text, 0);
            model.BaoHuZhi = ChangYong.TryDouble(this.textBox1.Text, 0);
            model.IsHuiDu = this.checkBox2.Checked;
            model.IsXianShi = this.checkBox3.Checked;
            model.IsXunHuanHuanXian = this.checkBox1.Checked ? 1 : 0;
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
