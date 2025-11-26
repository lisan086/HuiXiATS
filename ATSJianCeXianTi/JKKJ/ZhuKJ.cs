using ATSJianCeXianTi.Model;
using ATSJianCeXianTi.PeiFangFrm;
using ATSJianMianJK;
using ATSJianMianJK.Log;
using CommLei.JiChuLei;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianCeXianTi.JKKJ
{
    public partial class ZhuKJ : UserControl
    {
        Dictionary<int,TanChuanKJ> liskj=new Dictionary<int, TanChuanKJ>();
        private ZiYuanModel ZiYuanModel;
        public ZhuKJ(ZiYuanModel ziYuanModel)
        {          
            InitializeComponent();
            ZiYuanModel = ziYuanModel;
        }
       
        public void Close()
        {
            ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.Close,new JieMianModel());

            ATSJianCeXianTi.Lei.JieMianLei.Cerate().JieMianEvent -= ZhuKJ_JieMianEvent;
        }

        public void SetLog(List<RiJiModel> lismodel)
        {
            for (int i = 0; i < lismodel.Count; i++)
            {
                this.ucJiLvContor1.LogAppend(lismodel[i].IsRed ? Color.Red : Color.Black, lismodel[i].Msg);
            }
        }
        public  void QieHuanYongHu()
        {
            foreach (var item in liskj.Keys)
            {
                liskj[item].IniData(item,ZiYuanModel);
            }
           

            QuanXian();
        }

        private void QuanXian()
        {
            string msg = "";
            if (ZiYuanModel.QuanXian.IsYouQuanXian("配置通道", out msg) )
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

        private void ZhuKJ_Load(object sender, EventArgs e)
        {
            QuanXian();
            List<TDModel> lis = ATSJianCeXianTi.Lei.JieMianLei.Cerate().GetTongDao();
            int count = lis.Count;
            this.tableLayoutPanel1.Controls.Clear();
            if (count==1)
            {
                this.tableLayoutPanel1.ColumnStyles.Clear();
                this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle( SizeType.Percent,100));
            }
            for (int i = 0; i < count; i++)
            {
                TanChuanKJ kj = new TanChuanKJ();
                kj.Dock = DockStyle.Fill;
                kj.IniData(lis[i]);
                kj.IniData(lis[i].TDID,ZiYuanModel);
                this.tableLayoutPanel1.Controls.Add(kj,i,0);
                liskj.Add(lis[i].TDID, kj);
            }
          
            ATSJianCeXianTi.Lei.JieMianLei.Cerate().JieMianEvent += ZhuKJ_JieMianEvent;
            ATSJianCeXianTi.Lei.JieMianLei.Cerate().ChuLiUIEvent += ZhuKJ_ChuLiUIEvent;
            ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.Open,new JieMianModel());
            foreach (var item in liskj.Keys)
            {
                liskj[item].JiaZaiPeiFang();
            }
        }

        private void ZhuKJ_ChuLiUIEvent(TangChuanUIModel canshu, int type, int tdid)
        {
            ZiYuanModel.It.FanXingGaiBing(() => {
                if (liskj.ContainsKey(tdid))
                {
                    liskj[tdid].SetTanChaung(canshu, type,tdid);
                }
            });
        }

        private void ZhuKJ_JieMianEvent(EventType arg1, ShiJianModel arg2)
        {
            switch (arg1)
            {             
             
                case EventType.TestXiangMu:
                    {
                        XiangMuModel xiangmu = arg2.XiangMuModel;
                        ZiYuanModel.It.FanXingGaiBing(() => {
                            if (liskj.ContainsKey(xiangmu.TDID))
                            {
                                liskj[xiangmu.TDID].SetTestXiangMu(xiangmu.TDID, xiangmu);
                            }
                        });
                    }
                    break;            
           
                case EventType.TDTiShiKuang:
                    {
                        TangTiShiKuangModel xiangmu = arg2.TangTiShiKuangModel;
                        if (xiangmu.IsXuanZe)
                        {
                            bool xuanze = ZiYuanModel.ShiYuFou(xiangmu.Msg);
                            xiangmu.FanHuiJieGuo = xuanze;
                        }
                        else
                        {
                            ZiYuanModel.TiShiKuang(xiangmu.Msg);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

 

        private void button2_Click(object sender, EventArgs e)
        {
            ZongPeiZhiFrm frm = new ZongPeiZhiFrm();
            frm.ShowDialog(this);
        }

        public void ShuaXin()
        {
            List<TDModel> lis= Lei.JieMianLei.Cerate().DataJiHe.TDLisState.Values.ToList();
         
            ZiYuanModel.It.FanXingGaiBing(() => {
                for (int i = 0; i < lis.Count; i++)
                {
                    if (liskj.ContainsKey(lis[i].TDID))
                    {
                        liskj[lis[i].TDID].ShuXin();
                    }
                }

            });
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
    }
}
