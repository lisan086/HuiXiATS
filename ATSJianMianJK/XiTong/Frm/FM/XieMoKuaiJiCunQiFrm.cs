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
using JieMianLei.FuFrom;

namespace ATSJianMianJK.XiTong.Frm.FM
{
    public partial class XieMoKuaiJiCunQiFrm : BaseFuFrom
    {
        
        public XieMoKuaiJiCunQiFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(JiChuXieDYModel xieDYModel, bool isfuzhi)
        {
            this.jiChuXieKJ1.SetCanShu(xieDYModel,isfuzhi);
        }

        public JiChuXieDYModel GetCanShu()
        {
            return this.jiChuXieKJ1.GetCanShu();
        }
    }
}
