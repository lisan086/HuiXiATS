using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.PeiZhi.KJ
{
    public partial class XianShiShuJuKJ : UserControl
    {
        List<XiaoKJ> kj = new List<XiaoKJ>();
        private YeWuDataModel YeWuDataModel = new YeWuDataModel();
        public XianShiShuJuKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(YeWuDataModel yeWuData)
        {
            YeWuDataModel = yeWuData;
            this.label9.Text = YeWuDataModel.ItemName;
            {
               
                XiaoKJ xiaoKJ = new XiaoKJ();
                xiaoKJ.SetCanShu(YeWuDataModel.GWID, "值类型", yeWuData.QingQiuPiPei);
                kj.Add(xiaoKJ);
            }
           
            this.flowLayoutPanel1.Controls.AddRange(kj.ToArray());
        }

        public void ShuaXin()
        {
            if (kj.Count == 1)
            {
                kj[0].ShuaXinShuJu(YeWuDataModel.Value);
            }
           
        }
    }
}
