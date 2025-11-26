using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.XiTong.Frm.KJ;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;


namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class ZongPeiZhiFrm : BaseFuFrom
    {
      
        public ZongPeiZhiFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu()
        {
           
          
            Dictionary<int, List<DuIOCanShuModel>> ioshumodel = new Dictionary<int, List<DuIOCanShuModel>>();
            Dictionary<int, List<DuShuJuModel>> sushujumodel = new Dictionary<int, List<DuShuJuModel>>();
            Dictionary<int, List<XieSateModel> >xiemodesl = new Dictionary<int, List<XieSateModel>>();
            {
              
               
            }
            {
                List<DuIOCanShuModel> lismodel = HCLisDataLei<DuIOCanShuModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lismodel.Count; i++)
                {
                    DuIOCanShuModel model = ChangYong.FuZhiShiTi(lismodel[i]);
                    int tdid = model.TDID;
                    if (ioshumodel.ContainsKey(tdid) == false)
                    {
                        ioshumodel.Add(tdid, new List<DuIOCanShuModel>());
                    }
                    ioshumodel[tdid].Add(model);
                   
                }
            }
            {
                List<DuShuJuModel> lismodel = HCLisDataLei<DuShuJuModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lismodel.Count; i++)
                {
                    DuShuJuModel model = ChangYong.FuZhiShiTi(lismodel[i]);
                    int tdid = model.TDID;
                    if (sushujumodel.ContainsKey(tdid) == false)
                    {
                        sushujumodel.Add(tdid, new List<DuShuJuModel>());
                    }
                    sushujumodel[tdid].Add(model);
                   
                }
            }
            {
                List<XieSateModel> lismodel = HCLisDataLei<XieSateModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lismodel.Count; i++)
                {
                    XieSateModel model = ChangYong.FuZhiShiTi(lismodel[i]);
                    int tdid = model.TDID;
                    if (xiemodesl.ContainsKey(tdid) == false)
                    {
                        xiemodesl.Add(tdid, new List<XieSateModel>());
                    }
                    xiemodesl[tdid].Add(model);
                
                }
            }
            List<TDDuIOKJ> kjs=new List<TDDuIOKJ>();
            for (int i = 1; i < 21; i++)
            {
                bool zhen = false;
              
                List<DuIOCanShuModel> iomodels = new List<DuIOCanShuModel>();
                List<DuShuJuModel> shujumodes = new List<DuShuJuModel>();
                List<XieSateModel> xiemodesls = new List<XieSateModel>();
                {
                   
                }
                {
                  
                    if (ioshumodel.ContainsKey(i))
                    {
                        iomodels.AddRange(ioshumodel[i].ToArray());
                        zhen = true;
                    }
                }
                {

                    if (sushujumodel.ContainsKey(i))
                    {
                        shujumodes.AddRange(sushujumodel[i].ToArray());
                        zhen = true;
                    }
                }
                {

                    if (xiemodesl.ContainsKey(i))
                    {
                        xiemodesls.AddRange(xiemodesl[i].ToArray());
                        zhen = true;
                    }
                }
                if (zhen)
                {
                    TDDuIOKJ kj = new TDDuIOKJ();
                    kj.SetCanShu(iomodels,shujumodes,xiemodesls, i);
                    kjs.Add(kj);
                }
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TDDuIOKJ kj = new TDDuIOKJ();
            kj.SetCanShu(new List<DuIOCanShuModel>(), new List<DuShuJuModel>(), new List<XieSateModel>(), -1);
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            List<DuIOCanShuModel> iomodels = new List<DuIOCanShuModel>();
            List<DuShuJuModel> shujumodes = new List<DuShuJuModel>();
            List<XieSateModel> xiemodesls = new List<XieSateModel>();
            foreach (Control item in this.flowLayoutPanel1.Controls)
            {
                if(item is TDDuIOKJ)
                {
                    TDDuIOKJ tDDuIOKJ= (TDDuIOKJ)item;
                  
                    iomodels.AddRange(ChangYong.FuZhiShiTi(tDDuIOKJ.GetDuIOCanShuModel()).ToArray());
                    shujumodes.AddRange(ChangYong.FuZhiShiTi(tDDuIOKJ.GetDuShuJuModel()).ToArray());
                    xiemodesls.AddRange(ChangYong.FuZhiShiTi(tDDuIOKJ.GetXieSateModel()).ToArray());
                }
            }
          
            HCLisDataLei<DuIOCanShuModel>.Ceratei().LisWuLiao = iomodels;
            HCLisDataLei<DuShuJuModel>.Ceratei().LisWuLiao = shujumodes;
            HCLisDataLei<XieSateModel>.Ceratei().LisWuLiao = xiemodesls;
         
            HCLisDataLei<DuIOCanShuModel>.Ceratei().BaoCun();
            HCLisDataLei<DuShuJuModel>.Ceratei().BaoCun();
            HCLisDataLei<XieSateModel>.Ceratei().BaoCun();
            this.QiDongTiShiKuang("保存成功");
            JiHeSheBei.Cerate().JiHeData.FenPeiData(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HuanCunPeiFrm huanCunPeiFrm = new HuanCunPeiFrm();
            huanCunPeiFrm.Show(this);
        }
    }
}
