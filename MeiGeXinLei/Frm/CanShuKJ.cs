using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;

namespace MeiGeXinLei.Frm
{
    public partial class CanShuKJ : UserControl,KJPeiZhiJK
    {
        public CanShuKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<string> liemings,bool isyingchang,bool iszhiyouyige)
        {
            if (isyingchang)
            {
                this.panel1.Visible = false;
                this.label11.Text = "无需参数";
            }
            else
            {
                if (iszhiyouyige)
                {
                    this.label1.Visible = false;
                    this.commBoxE2.Visible = false;
                    for (int i = 0; i < liemings.Count; i++)
                    {
                        this.commBoxE1.Items.Add(liemings[i]);
                    }
                    if (this.commBoxE1.Items.Count>0)
                    {
                        this.commBoxE1.SelectedIndex = 0;
                    }
                }
                else
                {
                    
                    for (int i = 0; i < liemings.Count; i++)
                    {
                        this.commBoxE1.Items.Add(liemings[i]);
                        this.commBoxE2.Items.Add(liemings[i]);
                    }
                    if (this.commBoxE1.Items.Count > 0)
                    {
                        this.commBoxE1.SelectedIndex = 0;
                        this.commBoxE2.SelectedIndex = 0;
                    }
                }
            }
        }
        public string GetCanShu()
        {
            if (this.panel1.Visible)
            {
                List<string> lieming= new List<string>();
                bool zhen1 = false;
                bool zhen2 = false;
                if (this.commBoxE1.Visible)
                {
                    lieming.Add(this.commBoxE1.Text);
                    zhen1 = true;
                }
                if (this.commBoxE2.Visible)
                {
                    lieming.Add(this.commBoxE2.Text);
                    zhen1 |= true;
                }
                if (zhen1 && zhen2)
                {
                    return ChangYong.FenGeDaBao(lieming,",");
                }
                else
                {
                    return lieming[0];
                }
            }
            return "";
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            if (canshu.Contains(","))
            {
                List<string> liang = ChangYong.JieGeStr(canshu,',');
                if (liang.Count>=2)
                {
                    this.commBoxE1.Text = liang[0];
                    this.commBoxE2.Text =liang[1];
                }
            }
            else
            {
                this.commBoxE1.Text= canshu;
            }
        }
    }
}
