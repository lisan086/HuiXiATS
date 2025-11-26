using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Model;
using ATSJianMianJK.XiTong.XianShiDuFrm.Frm;
using CommLei.JiChuLei;

namespace ATSJianMianJK.XiTong.XianShiDuFrm.KJ
{
    public partial class DuKuaiIOKJ : UserControl
    {
      
        private Action<List<DuModel>> WeiTuo;
        private DuIOCanShuModel DuIOCanShuModel;
        public DuKuaiIOKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(DuIOCanShuModel dushuju)
        {
            DuIOCanShuModel = ChangYong.FuZhiShiTi(dushuju);
            this.label3.Text = dushuju.Type.ToString();
            this.label5.Text = dushuju.Name.ToString();
          
        }
        public void ShuXinData(DuIOCanShuModel shujus)
        {
            if (shujus.Name == this.label5.Text)
            {
                bool zhi = shujus.GetBool(false);
                if (zhi)
                {
                    if (this.label2.BackColor != Color.Green)
                    {
                        this.label2.BackColor = Color.Green;
                    }
                }
                else
                {
                    if (this.label2.BackColor != Color.Red)
                    {
                        this.label2.BackColor = Color.Red;
                    }
                }
            
                if (WeiTuo != null)
                {
                    WeiTuo(shujus.LisJiCunQi);
                }
               
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (WeiTuo != null)
            {
                WeiTuo = null;
            }
            DuMingXiFrom fm = new DuMingXiFrom();
            fm.SetCanShu(DuIOCanShuModel.LisJiCunQi);
            WeiTuo = fm.ShuaXin;
            fm.ShowDialog(this);
            WeiTuo = null;
        }
    }
}
