using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Frm.FM;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;
using SSheBei.PeiZhi;

namespace ATSJianMianJK.XiTong.Frm.KJ
{
    public partial class XieJiCunQiKJ : UserControl
    {
      
        private XieModel XieModel;
        public XieJiCunQiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(XieModel xieModel)
        {
        
            List<string> genjus = ChangYong.MeiJuLisName(typeof(GenJuType));
            this.commBoxE2.Items.Clear();       
            for (int i = 0; i < genjus.Count; i++)
            {
                this.commBoxE2.Items.Add(genjus[i]);
            }
            if (this.commBoxE2.Items.Count > 0)
            {
                this.commBoxE2.SelectedIndex = 0;
            }
            XieModel = ChangYong.FuZhiShiTi(xieModel);
            this.xieJiHeKJ1.SetCanShu(XieModel.TiaoJianJiChu,false);
            this.xieJiHeKJ2.SetCanShu(XieModel.FuZhiJiChu,true);
            this.textBox5.Text = XieModel.DengDaiTime.ToString();
            this.textBox2.Text = XieModel.ShunXu.ToString();
           
            this.textBox4.Text = XieModel.QiTaLiang.ToString();
            this.commBoxE2.Text = XieModel.GenJuType.ToString();

            for (int i = 0; i < this.commBoxE1.Items.Count; i++)
            {
                string moshi = this.commBoxE1.Items[i].ToString().Split(':')[0];
                if (moshi== XieModel.IfPanDuanType.ToString())
                {
                    this.commBoxE1.SelectedIndex = i;
                    break;
                }
            }
         
           
        }
        public XieModel GetCanShu()
        {
            XieModel model = ChangYong.FuZhiShiTi(XieModel);
            model.TiaoJianJiChu = this.xieJiHeKJ1.GetCanShu();
            model.FuZhiJiChu = this.xieJiHeKJ2.GetCanShu();
            model.DengDaiTime = ChangYong.TryFloat(this.textBox5.Text, 0f);
            model.ShunXu = ChangYong.TryInt(this.textBox2.Text, 0);        
            model.QiTaLiang = this.textBox4.Text;
            model.IfPanDuanType= ChangYong.TryInt(this.commBoxE1.Text.Split(':')[0], 1);
            model.GenJuType = ChangYong.GetMeiJuZhi<GenJuType>(this.commBoxE2.Text);
  
            return model;
        }

        public int GetShuXu()
        {
            return ChangYong.TryInt(this.textBox2.Text, 0);
        }

    
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

    
       

       
    }
}
