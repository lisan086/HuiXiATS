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

namespace ATSUI.KJ
{
    public partial class TXKJ : UserControl
    {
        public TXKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(bool istx,string name)
        {
            this.sheBeiDengKJ1.ColorYanSe = istx ? Color.Green : Color.Red;
            this.sheBeiDengKJ1.StrName = name;
        }
    }
}
