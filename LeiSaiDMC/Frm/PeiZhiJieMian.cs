using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUI.UC;
using JieMianLei.FuFrom;
using LeiSaiDMC.Frm.KJ;
using LeiSaiDMC.Model;
using SSheBei.Model;

namespace LeiSaiDMC.Frm
{
    public partial class PeiZhiJieMian : BaseFuFrom
    {
        private List<ZhouKJ> KJZhou = new List<ZhouKJ>();
        private  PeiZhiLei PeiZhiLei;
        public PeiZhiJieMian(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
            PeiZhiLei = peiZhiLei;
        }
        protected override void GuanBi()
        {
            this.timer1.Enabled = false;
            base.GuanBi();
        }
        private void SetCanShu()
        {
            if (PeiZhiLei.IsPeiZhi)
            {
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                List<LSModel> lis = PeiZhiLei.GetSheBei();
                for (int i = 0; i < lis.Count; i++)
                {
                    LSKJ kj = new LSKJ();
                    kj.SetCanShu(lis[i]);
                    this.flowLayoutPanel1.Controls.Add(kj);
                }
            }
            else
            {
                this.QuXiaoBiaoTi();
                this.tabPage1.Parent = null;
                List<JiLuModel> duios = new List<JiLuModel>();
                List<JiLuModel> xieios = new List<JiLuModel>();
                Dictionary<KaCanShuModel, List<JiCunQiModel>> cashu= PeiZhiLei.DuiYingJiCunQi;
                foreach (var item in cashu.Keys)
                {
                    if (item.IsZhou == false)
                    {
                        if (item.IsXieIO == false)
                        {
                            List<JiCunQiModel> jisun = cashu[item];
                            for (int i = 0; i < jisun.Count; i++)
                            {
                                JiLuModel model = new JiLuModel();
                                model.DaKaiColor = Color.Green;
                                model.CloseColor = Color.Red;
                                model.IsChengGong = false;
                                model.MingCheng = jisun[i].WeiYiBiaoShi;
                                model.SetJiCunIDAndSheBeiID(jisun[i].WeiYiBiaoShi);
                                duios.Add(model);
                            }
                        }
                        else
                        {
                            List<JiCunQiModel> jisun = cashu[item];
                            for (int i = 0; i < jisun.Count; i++)
                            {
                                JiLuModel model = new JiLuModel();
                                model.DaKaiColor = Color.Green;
                                model.CloseColor = Color.Red;
                                model.IsChengGong = false;
                                model.MingCheng = jisun[i].WeiYiBiaoShi;
                                model.SetJiCunIDAndSheBeiID(jisun[i].WeiYiBiaoShi);
                                xieios.Add(model);
                            }
                        }
                    }
                    else
                    {
                        ZhouKJ kjsd = new ZhouKJ();
                        kjsd.SetCanShu(item, cashu[item],PeiZhiLei);
                        this.flowLayoutPanel2.Controls.Add(kjsd);
                        KJZhou.Add(kjsd);
                    }

                }
                duoIOKJ1.SetXieIO(duios);
                duoIOKJ2.SetXieIO(xieios);
                duoIOKJ2.DianJiOKOrCloseEvent += DuoIOKJ2_DianJiOKOrCloseEvent;


                this.timer1.Enabled = true;
            }
        }

        private void DuoIOKJ2_DianJiOKOrCloseEvent(object kj, bool isdakai, JiLuModel e)
        {
            PeiZhiLei.SetIO(e.JiCunQiID, isdakai);
        }

        private void PeiZhiJieMian_Load(object sender, EventArgs e)
        {
            SetCanShu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LSKJ kj = new LSKJ();
            kj.SetCanShu(new LSModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<LSModel> lis = new List<LSModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is LSKJ)
                {
                    LSKJ kj = this.flowLayoutPanel1.Controls[i] as LSKJ;
                    lis.Add(kj.GetModel());
                }
            }
            PeiZhiLei.BaoCun(lis);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Dictionary<KaCanShuModel, List<JiCunQiModel>> cashu = PeiZhiLei.DuiYingJiCunQi;
            foreach (var item in cashu.Keys)
            {
                if (item.IsZhou == false)
                {
                    if (item.IsXieIO == false)
                    {
                        List<JiCunQiModel> jisun = new List<JiCunQiModel>();
                        for (int i = 0; i < jisun.Count; i++)
                        {
                            duoIOKJ1.SetYanSe(jisun[i].WeiYiBiaoShi, jisun[i].Value.ToString() == "1");
                        }

                    }
                    else
                    {
                        List<JiCunQiModel> jisun = new List<JiCunQiModel>();
                        for (int i = 0; i < jisun.Count; i++)
                        {
                            duoIOKJ2.SetYanSe(jisun[i].WeiYiBiaoShi, jisun[i].Value.ToString() == "1");
                        }
                    }
                }
            }
            duoIOKJ1.ShuaXin();
            duoIOKJ2.ShuaXin();
            for (int i = 0; i < KJZhou.Count; i++)
            {
                KJZhou[i].ShuaXinData();
            }
        }
    }
}
