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

namespace KuHuDuanDoIP.Frm
{
    public partial class WuCanShuKJ : UserControl, KJPeiZhiJK
    {
        public WuCanShuKJ()
        {
            InitializeComponent();
        }

        public string GetCanShu()
        {
            return "";
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {
            
        }
    }
}
