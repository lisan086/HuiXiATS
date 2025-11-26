using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.XiTong.Model;
using ATSJianMianJK.XiTong.XianShiDuFrm.KJ;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using JieMianLei.FuFrom.KJ;

namespace ATSJianMianJK.XiTong.XianShiDuFrm.Frm
{
    public partial class XieFrm : BaseFuFrom
    {
     
        private List<XieSateModel> XieSateModels=new List<XieSateModel>();
        public XieFrm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public  void IniData(List<XieSateModel> kamodel)
        {
            XieSateModels=kamodel;
           
            List<int> tdids = new List<int>();
            for (int i = 0; i < kamodel.Count; i++)
            {
                if (tdids.IndexOf(kamodel[i].TDID) < 0)
                {
                    tdids.Add(kamodel[i].TDID);
                }
            }
            this.commBoxE1.Items.Clear();
            for (int i = 0; i < tdids.Count; i++)
            {
                this.commBoxE1.Items.Add(tdids[i]);
            }
            if (this.commBoxE1.Items.Count > 0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
           
        }

        private void Kj_XieEvent(XieSateModel obj)
        {
          
        }

        private void Kj_BaoCunEvent(XieSateModel obj)
        {
            if (obj==null)
            {
                return;
            }
            List<XieSateModel> kjs = HCLisDataLei<XieSateModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < kjs.Count; i++)
            {
                if (kjs[i].Name.Equals(obj.Name)&& kjs[i].TDID== obj.TDID)
                {
                    kjs[i] = ChangYong.FuZhiShiTi(obj);
                }
            }
            HCLisDataLei<XieSateModel>.Ceratei().BaoCun();
        }

       

        private void commBoxE1_SelectedIndexChanged(object sender, EventArgs e)
        {
         

            Thread.Sleep(150);
            this.flowLayoutPanel1.Controls.Clear();
            int id = ChangYong.TryInt(this.commBoxE1.Text, -1);
            List<XieKuaiKJ> kjDu = new List<XieKuaiKJ>();
            for (int i = 0; i < XieSateModels.Count; i++)
            {
                if (XieSateModels[i].TDID == id)
                {
                    XieKuaiKJ kj = new XieKuaiKJ();
                    kj.SetCanShu(XieSateModels[i]);
                  
                    kjDu.Add(kj);
                }
            }

            this.flowLayoutPanel1.Controls.AddRange(kjDu.ToArray());
            
        }

     

     
    }
}
