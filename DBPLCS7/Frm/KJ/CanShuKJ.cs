using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSheBei.ABSSheBei;

namespace DBPLCS7.Frm.KJ
{
    public partial class CanShuKJ : UserControl, KJPeiZhiJK
    {
        public CanShuKJ()
        {
            InitializeComponent();
        }
        public void SetData(List<string> lis)
        {
            this.commBoxE1.Items.Clear();
            for (int i = 0; i < lis.Count; i++)
            {
                this.commBoxE1.Items.Add(lis[i]);
            }
            if (this.commBoxE1.Items.Count>0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
        }
        public string GetCanShu()
        {
            return this.commBoxE1.Text;
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            this.commBoxE1.Text = canshu;
        }
    }
}
