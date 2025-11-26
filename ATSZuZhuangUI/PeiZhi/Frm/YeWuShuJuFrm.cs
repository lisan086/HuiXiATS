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
using JieMianLei.FuFrom;
using ZuZhuangUI.Model;
using ZuZhuangUI.PeiZhi.KJ;

namespace ZuZhuangUI.PeiZhi.Frm
{
    public partial class YeWuShuJuFrm : BaseFuFrom
    {
        private bool IsShuJu = false;
        private string KaiShiTou = "";
        public YeWuShuJuFrm()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<YeWuDataModel> LisData, bool isshuju,string kaishitou)
        {
            KaiShiTou= kaishitou;
            IsShuJu = isshuju;
            if (isshuju)
            {
                List<DataKJ> liskjs = new List<DataKJ>();
                for (int i = 0; i < LisData.Count; i++)
                {
                    DataKJ kj = new DataKJ();
                    kj.SetCanShu(LisData[i]);
                    liskjs.Add(kj);
                }
                this.flowLayoutPanel1.Controls.AddRange(liskjs.ToArray());
            }
            else
            {
                this.button5.Visible = false;
                List<KongZhiKJ> liskjs = new List<KongZhiKJ>();
                for (int i = 0; i < LisData.Count; i++)
                {
                    KongZhiKJ kj = new KongZhiKJ();
                    kj.SetCanShu(LisData[i], KaiShiTou);
                    liskjs.Add(kj);
                }
                this.flowLayoutPanel1.Controls.AddRange(liskjs.ToArray());
            }
        }

        public List<YeWuDataModel> GetModel()
        {
            if (IsShuJu)
            {
                List<YeWuDataModel> lismodel = new List<YeWuDataModel>();
                for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
                {
                    if (this.flowLayoutPanel1.Controls[i] is DataKJ)
                    {
                        DataKJ ks = this.flowLayoutPanel1.Controls[i] as DataKJ;
                        lismodel.Add(ChangYong.FuZhiShiTi(ks.GetCanShu()));
                    }
                }
                return lismodel;
            }
            else
            {
                List<YeWuDataModel> lismodel = new List<YeWuDataModel>();
                for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
                {
                    if (this.flowLayoutPanel1.Controls[i] is KongZhiKJ)
                    {
                        KongZhiKJ ks = this.flowLayoutPanel1.Controls[i] as KongZhiKJ;
                        lismodel.Add(ChangYong.FuZhiShiTi(ks.GetCanShu()));
                    }
                }
                return lismodel;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (IsShuJu)
            {
                DataKJ kj = new DataKJ();
                kj.SetCanShu(new YeWuDataModel());
                this.flowLayoutPanel1.Controls.Add(kj);
            }
            else
            {
                KongZhiKJ kj = new KongZhiKJ();
                kj.SetCanShu(new YeWuDataModel(),KaiShiTou);
                this.flowLayoutPanel1.Controls.Add(kj);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (IsShuJu)
            {
                List<YeWuDataModel> lismodel = GetModel();
                FromSortZX(lismodel, true);
                try
                {
                    int index = 0;
                    for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
                    {
                        if (this.flowLayoutPanel1.Controls[i] is DataKJ)
                        {

                            DataKJ ks = this.flowLayoutPanel1.Controls[i] as DataKJ;
                            ks.SetCanShu(lismodel[index]);
                            index++;
                        }
                    }
                }
                catch
                {

                  
                }
               
            }
        }

        /// <summary>
        ///  从小到大排序
        /// </summary>
        /// <param name="lisObj">集合</param>
        /// <param name="IsSort">为true表示从小到大，为false则是从大到小</param>
        private void FromSortZX(List<YeWuDataModel> lisObj, bool IsSort)
        {
            if (lisObj.Count > 0)
            {
                try
                {
                    YeWuDataModel obj = null;
                    for (int i = 0; i < lisObj.Count; i++)
                    {
                        for (int j = i + 1; j < lisObj.Count; j++)
                        {
                            if (IsSort)
                            {
                                if (lisObj[i].PaiXu > lisObj[j].PaiXu)
                                {
                                    obj = lisObj[i];
                                    lisObj[i] = lisObj[j];
                                    lisObj[j] = obj;

                                }
                            }
                            else
                            {
                                if (lisObj[i].PaiXu < lisObj[j].PaiXu)
                                {
                                    obj = lisObj[i];
                                    lisObj[i] = lisObj[j];
                                    lisObj[j] = obj;

                                }
                            }
                        }
                    }
                }
                catch
                {


                }

            }
        }
    }
}
