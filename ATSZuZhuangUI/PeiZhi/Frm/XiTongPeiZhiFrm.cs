using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSFoZhaoZuZhuangUI.Model;
using CommLei.DataChuLi;
using JieMianLei.FuFrom;

namespace ATSFoZhaoZuZhuangUI.PeiZhi.Frm
{
    public partial class XiTongPeiZhiFrm : BaseFuFrom
    {
        public XiTongPeiZhiFrm()
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
        }
        private void SetCanShu()
        {
            this.commBoxE1.Items.Clear();
            XiTongModel ximodel = HCDanGeDataLei<XiTongModel>.Ceratei().LisWuLiao;
            this.xuanZeDuXieKJ3.SetCanShu(ximodel.QiDongXinHaoName);
            this.xuanZeDuXieKJ1.SetCanShu(ximodel.BangDingHuanXing);
            this.xuanZeDuXieKJ2.SetCanShu(ximodel.XieShuJu);
            this.checkBox1.Checked = ximodel.IsQieHuan;
            this.textBox2.Text = ximodel.PiPeiZhi;
            this.commBoxE1.SetCanShu<PanDuanMoShi>();
            this.commBoxE1.Text = ximodel.PanDuanMoShi.ToString();
            this.textBox1.Text = ximodel.NGZhi;
            this.textBox3.Text = ximodel.PassZhi;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            XiTongModel ximodel = new XiTongModel();
            ximodel.QiDongXinHaoName = this.xuanZeDuXieKJ3.GetCanShu();
            ximodel.BangDingHuanXing = this.xuanZeDuXieKJ1.GetCanShu();
            ximodel.XieShuJu= this.xuanZeDuXieKJ2.GetCanShu();
            ximodel.IsQieHuan = this.checkBox1.Checked;
            ximodel.PiPeiZhi = this.textBox2.Text;
            ximodel.PanDuanMoShi = this.commBoxE1.GetCanShu<PanDuanMoShi>();
            ximodel.NGZhi = this.textBox1.Text;
            ximodel.PassZhi= this.textBox3.Text;
            HCDanGeDataLei<XiTongModel>.Ceratei().LisWuLiao = ximodel;
            HCDanGeDataLei<XiTongModel>.Ceratei().BaoCun();
            this.QiDongTiShiKuang("保存成功");
        }

        private void XiTongPeiZhiFrm_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }
    }
}
