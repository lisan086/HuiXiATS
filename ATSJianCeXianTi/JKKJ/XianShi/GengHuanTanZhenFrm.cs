using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.Lei;
using JieMianLei.FuFrom;

namespace ATSJianCeXianTi.JKKJ.XianShi
{
    public partial class GengHuanTanZhenFrm : BaseFuFrom
    {
        private int TDID = 0;
        public GengHuanTanZhenFrm(int tdid)
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
            TDID = tdid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            bool  GaoPing = this.checkBox1.Checked;
            if (GaoPing)
            {
                JiShuGuanLiLei.Cerate().SetTangZheng(TDID,1);
            }
            bool DiPing = this.checkBox2.Checked;
            if (DiPing)
            {
                JiShuGuanLiLei.Cerate().SetTangZheng(TDID, 2);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
