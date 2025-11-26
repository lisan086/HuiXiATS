using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;
using ZuZhuangUI.PeiZhi.KJ;

namespace ZuZhuangUI.PeiZhi.Frm
{
    public partial class ZhanDianFrm : BaseFuFrom
    {
        private PeiFangChuLi PeiFangChuLi;
        public ZhanDianFrm()
        {
            InitializeComponent();
            PeiFangChuLi = new PeiFangChuLi();
            IniData();
        }
        private void IniData()
        {
            string WenJianLuJing = HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().LisWuLiao.LuJin;
            PeiFangChuLi.JiaZaiPeiFang(WenJianLuJing);
            ShuJu(PeiFangChuLi.BTKLineModel);
            PeiFangNameGengXin();
        }

        private void PeiFangNameGengXin()
        {
            this.label3.Text = ChangYong.GetWenJianName(PeiFangChuLi.WenJianLuJing1);
            this.label2.Text = ChangYong.GetWenJianName(HCDanGeDataLei<PeiFangXuanZeModel>.Ceratei().LisWuLiao.LuJin);
        }
        private void ShuJu(List<SheBeiZhanModel> model)
        {
            this.flowLayoutPanel1.Controls.Clear();

            List<SheBeiZhanKJ> kjs = new List<SheBeiZhanKJ>();
            foreach (var item in model)
            {
                SheBeiZhanKJ dian = new SheBeiZhanKJ();
                dian.SetCanShu(item);
                kjs.Add(dian);
            }

            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        private List<SheBeiZhanModel> GetShuJu()
        {
            List<SheBeiZhanModel> model = new List<SheBeiZhanModel>();

            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is SheBeiZhanKJ)
                {
                    SheBeiZhanKJ zhan = this.flowLayoutPanel1.Controls[i] as SheBeiZhanKJ;
                    model.Add(ChangYong.FuZhiShiTi(zhan.GetCanShu()));
                }
            }
            return model;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SheBeiZhanKJ dian = new SheBeiZhanKJ();
            dian.SetCanShu(new  SheBeiZhanModel());
            this.flowLayoutPanel1.Controls.Add(dian);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PeiFangChuLi.GengHuanDanQianPeiFang();
            this.QiDongTiShiKuang("设置成功");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                PeiFangChuLi.JiaZaiPeiFang(openFileDialog.FileName);
                ShuJu(PeiFangChuLi.BTKLineModel);
                PeiFangNameGengXin();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<SheBeiZhanModel> model = GetShuJu();
            PeiFangChuLi.BTKLineModel = model;
            bool zhen = PeiFangChuLi.BaoCun("");
            if (zhen == false)
            {
                SaveFileDialog opens = new SaveFileDialog();
                if (opens.ShowDialog(this) == DialogResult.OK)
                {
                    PeiFangChuLi.BaoCun(opens.FileName);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<SheBeiZhanModel> model = GetShuJu();
            PeiFangChuLi.BTKLineModel = model;
            SaveFileDialog opens = new SaveFileDialog();
            opens.Filter = "*.txt|*.txt";
            if (opens.ShowDialog(this) == DialogResult.OK)
            {
                PeiFangChuLi.BaoCun(opens.FileName);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int zhanid = ChangYong.TryInt(this.textBox1.Text,-1);
            List<SheBeiZhanModel> lis= GetShuJu();
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].GWID==zhanid)
                {
                    SheBeiZhanModel xinmodel = ChangYong.FuZhiShiTi(lis[i]);
                    xinmodel.GWID = -1;
                    SheBeiZhanKJ dian = new SheBeiZhanKJ();
                    dian.SetCanShu(xinmodel);
                    this.flowLayoutPanel1.Controls.Add(dian);
                    break;
                }
            }
        }
    }
}
