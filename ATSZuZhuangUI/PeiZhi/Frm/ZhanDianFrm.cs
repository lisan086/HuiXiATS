using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSZuZhuangUI.PeiZhi.Frm;
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
            this.commBoxE2.Items.Clear();           
            PeiFangChuLi = new PeiFangChuLi();
            Dictionary<string, string> peifangnames= PeiFangChuLi.GetPeiFangNames();
            List<string> keys = peifangnames.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                this.commBoxE2.Items.Add(keys[i]);
            }
            this.commBoxE2.Text = PeiFangChuLi.WenJianLuJing1;
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
            this.label1.Text =$"目前使用配方:{PeiFangChuLi.WenJianLuJing1}" ;
            this.label4.Text = $"正在修改的配方:{PeiFangChuLi.XiuGaiDePeiFangName}";
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
                this.textBox6.Text = item.BangDingMa;
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
                    model[model.Count - 1].BangDingMa = this.textBox6.Text;
                }
            }
            return model;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XuanZeSheBeiFrm frm = new XuanZeSheBeiFrm();
            frm.SetCanShu("");
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                SheBeiType leixing = ChangYong.GetMeiJuZhi<SheBeiType>(frm.JieGuo);
                SheBeiZhanKJ dian = new SheBeiZhanKJ();
                dian.SetCanShu(new SheBeiZhanModel() { IsZhengZhanDian= leixing });
                this.flowLayoutPanel1.Controls.Add(dian);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string peifangnming = this.commBoxE2.Text;
             bool zhen= PeiFangChuLi.SetDanQianBaoPeiFang(peifangnming);
            if (zhen)
            {
                this.QiDongTiShiKuang("设置成功");
            }
            else
            {
                this.QiDongTiShiKuang("配方不存在");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PeiFangChuLi.JiaZaiPeiFang(this.commBoxE2.Text);
            ShuJu(PeiFangChuLi.BTKLineModel);
            PeiFangNameGengXin();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<SheBeiZhanModel> model = GetShuJu();
            PeiFangChuLi.BTKLineModel = model;
            bool zhen = PeiFangChuLi.BaoCun(this.commBoxE2.Text);
            PeiFangNameGengXin();
            this.QiDongTiShiKuang("保存成功");
        }

    

        private void button6_Click(object sender, EventArgs e)
        {
            int dianid = ChangYong.TryInt(this.textBox2.Text, -1);
            if (dianid >= 0)
            {
                string qian = this.textBox1.Text.Trim();
                string hou = this.textBox3.Text.Trim();
                List<SheBeiZhanModel> sheBeiZhanModels = GetShuJu();
                if (sheBeiZhanModels.Count > 0)
                {
                    for (int C = 0; C < sheBeiZhanModels.Count; C++)
                    {
                        if (sheBeiZhanModels[C].GWID== dianid)
                        {
                            SheBeiZhanModel xinde = ChangYong.FuZhiShiTi(sheBeiZhanModels[C]);
                            if (xinde != null)
                            {
                                for (int i = 0; i < xinde.LisQingQiu.Count; i++)
                                {
                                    if (string.IsNullOrEmpty(xinde.LisQingQiu[i].Value.JCQStr) == false)
                                    {
                                        xinde.LisQingQiu[i].Value.JCQStr = xinde.LisQingQiu[i].Value.JCQStr.Replace(this.textBox4.Text.Trim(), this.textBox5.Text.Trim());
                                        xinde.LisQingQiu[i].Value.JCQStr = $"{qian}{xinde.LisQingQiu[i].Value.JCQStr}{hou}";
                                    }
                                }
                                for (int i = 0; i < xinde.LisData.Count; i++)
                                {
                                    if (string.IsNullOrEmpty(xinde.LisData[i].Value.JCQStr) == false)
                                    {
                                        xinde.LisQingQiu[i].Value.JCQStr = xinde.LisQingQiu[i].Value.JCQStr.Replace(this.textBox4.Text.Trim(), this.textBox5.Text.Trim());
                                        xinde.LisData[i].Value.JCQStr = $"{qian}{xinde.LisData[i].Value.JCQStr}{hou}";
                                    }
                                    if (string.IsNullOrEmpty(xinde.LisData[i].Low.JCQStr) == false)
                                    {
                                        xinde.LisQingQiu[i].Low.JCQStr = xinde.LisQingQiu[i].Low.JCQStr.Replace(this.textBox4.Text.Trim(), this.textBox5.Text.Trim());
                                        xinde.LisData[i].Low.JCQStr = $"{qian}{xinde.LisData[i].Low.JCQStr}{hou}";
                                    }
                                    if (string.IsNullOrEmpty(xinde.LisData[i].Up.JCQStr) == false)
                                    {
                                        xinde.LisQingQiu[i].Up.JCQStr = xinde.LisQingQiu[i].Up.JCQStr.Replace(this.textBox4.Text.Trim(), this.textBox5.Text.Trim());
                                        xinde.LisData[i].Up.JCQStr = $"{qian}{xinde.LisData[i].Up.JCQStr}{hou}";
                                    }
                                    if (string.IsNullOrEmpty(xinde.LisData[i].State.JCQStr) == false)
                                    {
                                        xinde.LisQingQiu[i].State.JCQStr = xinde.LisQingQiu[i].State.JCQStr.Replace(this.textBox4.Text.Trim(), this.textBox5.Text.Trim());
                                        xinde.LisData[i].State.JCQStr = $"{qian}{xinde.LisData[i].State.JCQStr}{hou}";
                                    }
                                }
                                SheBeiZhanKJ dian = new SheBeiZhanKJ();
                                dian.SetCanShu(xinde);
                                this.flowLayoutPanel1.Controls.Add(dian);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    this.QiDongTiShiKuang("还没有建立站");
                }
            }
            else
            {
                this.QiDongTiShiKuang("没有选择相应的站id");
            }
        }

       
    }
}
