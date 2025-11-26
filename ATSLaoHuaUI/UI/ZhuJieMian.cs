using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK;
using ATSJianMianJK.Log;
using ATSLaoHuaUI.UI;
using BaseUI.DaYIngMoBan.Frm;
using CommLei.JiChuLei;
using SSheBei.PeiZhi;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;
using ZuZhuangUI.PeiZhi.Frm;

namespace ZuZhuangUI.UI
{
    public partial class ZhuJieMian : UserControl
    {
        List<GongWeiKJ> liskj = new List<GongWeiKJ>();
       
        private ZiYuanModel ZiYuanModel;
        public ZhuJieMian(ZiYuanModel ziYuanModel)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            ZiYuanModel = ziYuanModel;
        }

        public void Close()
        {
            JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.Close, new  JieMianCaoZuoModel());

            JieMianCaoZuoLei.CerateDanLi().JieMianEvent -= ZhuKJ_JieMianEvent;
        }

        public void SetLog(List<RiJiModel> lismodel)
        {
            ZiYuanModel.It.FanXingGaiBing(() =>
            {
                for (int i = 0; i < lismodel.Count; i++)
                {
                    switch (lismodel[i].RiJiEnum)
                    {
                        case RiJiEnum.TDTangChuang:
                            {
                                ZiYuanModel.TiShiKuang(lismodel[i].Msg);
                            }
                            break;
                        case RiJiEnum.SheBeiTangChuang:
                            {
                                ZiYuanModel.TiShiKuang(lismodel[i].Msg);
                            }
                            break;
                        case RiJiEnum.TDLiuCheng:
                        case RiJiEnum.TDLiuChengRed:
                        case RiJiEnum.TDCuoWuRedLog:
                        case RiJiEnum.MesData:
                            {
                               
                            }
                            break;

                        default:
                            break;
                    }

                }
            });

        }
        public void QieHuanYongHu()
        {
          
            QuanXian();
        }

        private void QuanXian()
        {
            string msg = "";
            if (ZiYuanModel.QuanXian.IsYouQuanXian("配置通道", out msg))
            {
                if (this.tabPage3.Parent == null)
                {
                    this.tabPage3.Parent = this.tabControl1;
                }
            }
            else
            {
                this.tabPage3.Parent = null;
            }
        }

        private void ZhuJieMian_Load(object sender, EventArgs e)
        {
            QuanXian();
            List<SheBeiZhanModel> lis = JieMianCaoZuoLei.CerateDanLi().DataJiHe.LisSheBeiBianHao;
            int count = lis.Count;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();         
            if (count > 0)
            {

                float bili = (1f / count) * 100f;
                for (int i = 0; i < count; i++)
                {
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, bili));

                }
                if (count > 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    }
                }
                else
                {
                    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                }


                int c = 0;
                liskj.Clear();
                
                foreach (var item in lis)
                {
                    if (item.SheBeiType == SheBeiType.JuanChengLoaHua)
                    {
                        GongWeiKJ kj = new GongWeiKJ();
                        kj.Dock = DockStyle.Fill;
                        //根据配置来数据填充
                        kj.SetCanShu(item);
                        kj.SetCanShu(ZiYuanModel);
                   
                        liskj.Add(kj);
                        this.tableLayoutPanel1.Controls.Add(kj);
                    }
                   
                    c++;

                }
            }

            JieMianCaoZuoLei.CerateDanLi().JieMianEvent += ZhuKJ_JieMianEvent;
         
            JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.Open, new JieMianCaoZuoModel());
            this.timer1.Enabled = true;
        }

     

        private void ZhuKJ_JieMianEvent(EventType arg1, JieMianShiJianModel arg2)
        {

            ZiYuanModel.It.FanXingGaiBing(() => {
                //foreach (var item in liskj)
                //{
                //    item.SetCanShu(arg1, arg2);
                //}
                //foreach (var item in lisShou)
                //{
                //    item.SetCanShu(arg1, arg2);
                //}
            });
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            string msg = "";
            bool zhen = ZiYuanModel.QuanXian.IsYouQuanXian("配置通道", out msg);
            if (zhen)
            {
                ZhanDianFrm zhanDianFrm = new ZhanDianFrm();
                zhanDianFrm.ShowDialog();
            }
            else
            {
                ZiYuanModel.TiShiKuang(msg);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TDShuJuFrm tDShuJuFrm = new TDShuJuFrm();
            tDShuJuFrm.Show(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (liskj.Count>0)
            {
                for (int i = 0; i < liskj.Count; i++)
                {
                    liskj[i].ShuaXin();
                }
            }
        }
    }
}
