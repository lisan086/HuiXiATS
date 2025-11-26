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

namespace ZhongWangSheBei.Frm
{
    public partial class CanShuKJ : UserControl,KJPeiZhiJK
    {
        public CanShuKJ()
        {
            InitializeComponent();
        }

        public void SetShuJu(int jilu,bool iskong)
        {
            this.listView1.Visible = iskong;
            this.listView1.Items.Clear();
            for (int i = 0; i < jilu; i++)
            {
                ListViewItem item = new ListViewItem((i + 1).ToString());
               
                this.listView1.Items.Add(item);
            }
            if (this.listView1.Visible == false)
            {
                this.label11.Text = this.label11.Text + "不需要参数";
            }
            else
            {
                this.label11.Visible = false;
            }
        }
        public string GetCanShu()
        {
            if (this.listView1.Visible==false)
            {
                return "";
            }
            List<string> list = new List<string>();
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                if (this.listView1.Items[i].Checked)
                {
                    list.Add(this.listView1.Items[i].SubItems[0].Text);
                }
            }
            return ChangYong.FenGeDaBao(list,"|");
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            List<string> jies = ChangYong.JieGeStr(canshu,'|');
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                string wenben = this.listView1.Items[i].SubItems[0].Text;
                if (jies.IndexOf(wenben)>=0)
                {
                    this.listView1.Items[i].Checked = true ;
                }
            }
        }

      

      

    

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.Items.Count > 0)
            {
                if (this.listView1.SelectedItems.Count >= 0)
                {
                    bool zhuanta = this.listView1.SelectedItems[0].Checked;
                    this.listView1.SelectedItems[0].Checked = !zhuanta;
                }
            }
        }
    }
}
