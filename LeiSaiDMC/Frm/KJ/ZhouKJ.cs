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
using LeiSaiDMC.Model;
using SSheBei.Model;

namespace LeiSaiDMC.Frm.KJ
{
    public partial class ZhouKJ : UserControl
    {
        private KaCanShuModel KaCanShuModel;
        private List<JiCunQiModel> ZhouModel;
        private PeiZhiLei PeiZhiLei;
        public ZhouKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(KaCanShuModel ka,List<JiCunQiModel> lismodel, PeiZhiLei peiZhi)
        {
            KaCanShuModel = ka;
            ZhouModel = lismodel;
            PeiZhiLei = peiZhi;
            this.label5.Text = $"{ka.KaName} 报警";
        }

        public void ShuaXinData()
        {
            for (int i = 0; i < ZhouModel.Count; i++)
            {
                DuCanShuType xieCaoZuoType = DuCanShuType.Wu;
                object zhi= PeiZhiLei.GetZhi(ZhouModel[i],out xieCaoZuoType);
                switch (xieCaoZuoType)
                {
                    case DuCanShuType.位置:
                        {
                            string weizhi = $"位置:{zhi}";
                            if (label2.Text.Equals(weizhi)==false)
                            {
                                label2.Text = weizhi;
                            }                    
                        }
                        break;
                    case DuCanShuType.速度:
                        {
                            string weizhi = $"速度:{zhi}";
                            if (label3.Text.Equals(weizhi) == false)
                            {
                                label3.Text = weizhi;
                            }
                        }
                        break;
                    case DuCanShuType.使能:
                        {
                            int shineng = ChangYong.TryInt(zhi, 0);
                            if (shineng == 1)
                            {
                                if (label1.BackColor != Color.Green)
                                {
                                    label1.BackColor = Color.Green;
                                }
                            }
                            else
                            {
                                if (label1.BackColor != Color.Red)
                                {
                                    label1.BackColor = Color.Red;
                                }
                            }
                        }
                        break;                
                    case DuCanShuType.运动状态:
                        {
                            int shineng = ChangYong.TryInt(zhi, 1);
                            if (shineng == 1)
                            {
                                string weizhi = $"状态:停止";
                                if (label4.Text.Equals(weizhi) == false)
                                {
                                    label4.Text = weizhi;
                                }
                            }
                            else
                            {
                                string weizhi = $"状态:运行";
                                if (label4.Text.Equals(weizhi) == false)
                                {
                                    label4.Text = weizhi;
                                }
                            }
                        }
                        break;
                    case DuCanShuType.超限报警:
                        {
                            int shineng = ChangYong.TryInt(zhi, 0);
                            if (shineng == 1)
                            {
                                if (label5.BackColor != Color.Red)
                                {
                                    label5.BackColor = Color.Red;
                                }
                            }
                            else
                            {
                                if (label5.BackColor != Color.Green)
                                {
                                    label5.BackColor = Color.Green;
                                }
                            }
                        }
                        break;                
                    default:
                        break;
                }
            }
         
        }
        private void button4_Click(object sender, EventArgs e)
        {
            double weizhi = ChangYong.TryDouble(this.textBox1.Text, 0);
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.ZhouX位置, weizhi);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Contains("使"))
            {
                PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.Zhou使能, 1);
                button1.Text = "失能";
            }
            else
            {
                PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.Zhou使能, 0);
                button1.Text = "使能";
            }
        }

      

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.Zhou恒速, 0);
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.Zhou停止, 0);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.Zhou恒速, 1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            double weizhi = ChangYong.TryDouble(this.textBox1.Text,0);
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.ZhouX位置, -weizhi);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.Zhou停止, 0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.热复位, 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.Zhou回零, 0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            PeiZhiLei.ZhouGongJu(KaCanShuModel, XieCaoZuoType.Zhou位置清零, 0);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ZhouPeiZhiModel model = PeiZhiLei.GetZhouPeiZhi(KaCanShuModel);
            ZhouXianZhiFrm frm = new ZhouXianZhiFrm();
            if (model != null)
            {
                frm.SetCanShu(model,true);
            }
            else
            {
                frm.SetCanShu(new ZhouPeiZhiModel(),true);
            }
            if (frm.ShowDialog(this)==DialogResult.OK)
            {
                ZhouPeiZhiModel xinmodel = ChangYong.FuZhiShiTi(frm.GetModel());
                PeiZhiLei.SetZhouPeiZhi(KaCanShuModel,xinmodel);

            }
        }
    }
}
