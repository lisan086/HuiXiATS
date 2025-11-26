using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSLaoHuaUI.PeiZhi.KJ;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using ZuZhuangUI.Model;

namespace ATSLaoHuaUI.PeiZhi.Frm
{
    public partial class TDMaFrm : BaseFuFrom
    {
        public TDMaFrm()
        {
            InitializeComponent();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="LisData"></param>      
        public void SetCanShu(List<MaTDModel> LisData)
        {
         
            List<MaTDKJ> liskjs = new List<MaTDKJ>();
            for (int i = 0; i < LisData.Count; i++)
            {
                MaTDKJ kj = new MaTDKJ();
                kj.SetCanShu(LisData[i]);
                liskjs.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(liskjs.ToArray());
        }

        public List<MaTDModel> GetModel()
        {
            List<MaTDModel> lismodel = new List<MaTDModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is MaTDKJ)
                {
                    MaTDKJ ks = this.flowLayoutPanel1.Controls[i] as MaTDKJ;
                    lismodel.Add(ChangYong.FuZhiShiTi(ks.GetCanShu()));
                }
            }
            return lismodel;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MaTDKJ kj = new MaTDKJ();
            kj.SetCanShu(new  MaTDModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int yuanid = ChangYong.TryInt(this.textBox1.Text,-1);
            int fuzhiid = ChangYong.TryInt(this.textBox2.Text, -1);
            List<MaTDModel> lismodel= GetModel();
            MaTDModel xinmodel = null;

            for (int i = 0; i < lismodel.Count; i++)
            {
                if (lismodel[i].MaTDID==yuanid)
                {
                    xinmodel = lismodel[i];
                    break;
                }
            }
            if (xinmodel != null)
            {
                bool iszhaodao = false;
                for (int i = 0; i < lismodel.Count; i++)
                {
                    if (lismodel[i].MaTDID == fuzhiid)
                    {
                        iszhaodao = true;
                       // lismodel[i] = ChangYong.FuZhiShiTi(xinmodel);
                        foreach (var item in this.flowLayoutPanel1.Controls)
                        {
                            if (item is MaTDKJ)
                            {
                                
                                MaTDKJ ks = this.flowLayoutPanel1.Controls[i] as MaTDKJ;
                                MaTDModel jieguo = ks.GetCanShu();
                                if (jieguo.MaTDID == fuzhiid)
                                {
                                    MaTDModel sssd = ChangYong.FuZhiShiTi(xinmodel);
                                    sssd.MaTDID = fuzhiid;
                                    sssd.TDName = lismodel[i].TDName;
                                    ks.SetCanShu(sssd);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
                if (iszhaodao==false)
                {
                  
                    MaTDKJ kj = new MaTDKJ();
                    MaTDModel xinmodels = ChangYong.FuZhiShiTi(xinmodel);
                    xinmodels.MaTDID = fuzhiid;
                    kj.SetCanShu(xinmodels);
                    this.flowLayoutPanel1.Controls.Add(kj);
                }
            }
        }
    }
}
