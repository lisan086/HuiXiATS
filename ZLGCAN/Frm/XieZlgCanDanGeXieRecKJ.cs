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
using YiBanSaoMaQi.Model;

namespace ZLGXSCAN.Frm
{
    public partial class XieZlgCanDanGeXieRecKJ : UserControl,KJPeiZhiJK
    {
     
        public XieZlgCanDanGeXieRecKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(int tongdaoindex,bool iszhixianindex)
        {
            if (iszhixianindex)
            {
                this.panel2.Visible = false;
            }
            else
            {
                this.panel2.Visible = true;
            }
            this.commBoxE1.Items.Clear();
            for (int i = 0; i < tongdaoindex+1; i++)
            {
                this.commBoxE1.Items.Add(i);

            }
            if (this.commBoxE1.Items.Count>0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
        }
     
        public string GetCanShu()
        {
            if (this.panel2.Visible)
            {
                List<string> list = new List<string>();
                list.Add(this.textBox5.Text);
                list.Add(this.textBox4.Text);
                list.Add(this.commBoxE1.Text);
                list.Add(this.textBox1.Text);
                list.Add(this.checkBox4.Checked?"1":"0");
                return ChangYong.FenGeDaBao(list, ",");
            }
            else
            {
                return this.commBoxE1.Text;
            }
           
         
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            if (canshu.Contains(","))
            {
                List<string> list = ChangYong.JieGeStr(canshu, ',');
                if (list.Count >= 5)
                {
                    this.textBox5.Text = list[0];
                    this.textBox4.Text = list[1];
                    this.commBoxE1.Text = list[2];
                    this.textBox1.Text = list[3];
                    this.checkBox4.Checked = list[4] == "1";
                }
            }
            else
            {
                this.commBoxE1.Text = canshu;
            }
        }
    }
}
