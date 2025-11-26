using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Frm.KJ;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;


namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class XieJiCunQiFrm : BaseFuFrom
    {
       
        public XieJiCunQiFrm()
        {
            InitializeComponent();
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.Cancel;
            base.GuanBi();
        }
        public void SetCanShu( List<XieModel> lis)
        {
        
            this.flowLayoutPanel1.Controls.Clear();
            List<XieJiCunQiKJ> kjs = new List<XieJiCunQiKJ>();
            for (int i = 0; i < lis.Count; i++)
            {
                XieJiCunQiKJ kJ = new XieJiCunQiKJ();
                kJ.SetCanShu( lis[i]);
                kjs.Add(kJ);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        public List<XieModel> GetCanShu()
        {
            List<XieModel> lian = new List<XieModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is XieJiCunQiKJ)
                {
                    XieJiCunQiKJ kj = this.flowLayoutPanel1.Controls[i] as XieJiCunQiKJ;
                    lian.Add(ChangYong.FuZhiShiTi(kj.GetCanShu()));
                }
            }
            return lian;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XieJiCunQiKJ kJ = new XieJiCunQiKJ();
            kJ.SetCanShu( new XieModel());
         
            this.flowLayoutPanel1.Controls.Add(kJ);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<XieModel> canshu= GetCanShu();
            FromSortZX(canshu,true);
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is XieJiCunQiKJ)
                {
                    XieJiCunQiKJ kj = this.flowLayoutPanel1.Controls[i] as XieJiCunQiKJ;
                    kj.SetCanShu(canshu[i]);
                }
            }
        }
        /// <summary>
        ///  从小到大排序
        /// </summary>
        /// <param name="lisObj">集合</param>
        /// <param name="IsSort">为true表示从小到大，为false则是从大到小</param>
        private void FromSortZX(List<XieModel> lisObj, bool IsSort)
        {
            if (lisObj.Count > 0)
            {
                try
                {
                    XieModel obj = null;
                    for (int i = 0; i < lisObj.Count; i++)
                    {
                        for (int j = i + 1; j < lisObj.Count; j++)
                        {
                            if (IsSort)
                            {
                                if (lisObj[i].ShunXu > lisObj[j].ShunXu)
                                {
                                    obj = lisObj[i];
                                    lisObj[i] = lisObj[j];
                                    lisObj[j] = obj;

                                }
                            }
                            else
                            {
                                if (lisObj[i].ShunXu < lisObj[j].ShunXu)
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
