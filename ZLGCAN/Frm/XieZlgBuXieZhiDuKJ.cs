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
using SSheBei.ABSSheBei;

namespace ZLGXSCAN.Frm
{
    public partial class XieZlgBuXieZhiDuKJ : UserControl, KJPeiZhiJK
    {
        public XieZlgBuXieZhiDuKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(int tongdaoindex)
        {
         
            this.commBoxE1.Items.Clear();
            for (int i = 0; i < tongdaoindex ; i++)
            {
                this.commBoxE1.Items.Add(i);

            }
            if (this.commBoxE1.Items.Count > 0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
        }

        public string GetCanShu()
        {
            List<string> list = new List<string>();
            list.Add(this.commBoxE1.Text);
            list.Add(this.textBox5.Text);              
            list.Add(this.textBox1.Text);
            return ChangYong.FenGeDaBao(list, ",");
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            List<string> list = ChangYong.JieGeStr(canshu, ',');
            if (list.Count >= 3)
            {
                this.textBox5.Text = list[1];      
                this.commBoxE1.Text = list[0];
                this.textBox1.Text = list[2];
            }
        }
    }
}
