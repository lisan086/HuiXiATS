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

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class Form1:BaseFuFrom
    {
        public string XuLie { get; set; } = "";
        public Form1()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<string> lis,List<string> xuanze)
        {
            this.checkedListBox1.Items.Clear();
            for (int i = 0; i < lis.Count; i++)
            {
              
                int index = this.checkedListBox1.Items.Add(lis[i]);
                this.checkedListBox1.SetItemChecked(index, xuanze.IndexOf(lis[i]) >= 0);
            }
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                if (this.checkedListBox1.GetItemChecked(i))
                {
                    list.Add(this.checkedListBox1.GetItemText(this.checkedListBox1.Items[i]).Split(':')[0]);
                }
            }
            
            XuLie = ChangYong.FenGeDaBao(list, ",");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.checkedListBox1.Items.Count > 0)
            {
                if (this.checkedListBox1.SelectedIndex >= 0)
                {
                    bool zhuanta = this.checkedListBox1.GetItemChecked(this.checkedListBox1.SelectedIndex);
                    this.checkedListBox1.SetItemChecked(this.checkedListBox1.SelectedIndex, !zhuanta);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1_Click(null,null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.Items.Count > 0)
            {
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {                 
                    this.checkedListBox1.SetItemChecked(i, true);
                }
              
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.Items.Count > 0)
            {
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    this.checkedListBox1.SetItemChecked(i, false);
                }

            }
        }
    }
}
