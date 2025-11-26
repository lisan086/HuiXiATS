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
        public void SetCanShu(YeWuDataModel yeWuData,bool iszhi)
        {
            YeWuDataModel = yeWuData;
            this.label9.Text = YeWuDataModel.ItemName;
          
            if (iszhi == false)
            {
                {
                   
                    XiaoKJ xiaoKJ = new XiaoKJ();
                    xiaoKJ.SetCanShu(YeWuDataModel.GWID, "上限", yeWuData.QingQiuPiPei);
                    kj.Add(xiaoKJ);
                }
                {
                  
                    XiaoKJ xiaoKJ = new XiaoKJ();
                    xiaoKJ.SetCanShu(YeWuDataModel.GWID, "下限", yeWuData.QingQiuPiPei);
                    kj.Add(xiaoKJ);
                }
                {
                  
                    XiaoKJ xiaoKJ = new XiaoKJ();
                    xiaoKJ.SetCanShu(YeWuDataModel.GWID, "状态", yeWuData.QingQiuPiPei);
                    kj.Add(xiaoKJ);
                }
                {
                   
                    XiaoKJ xiaoKJ = new XiaoKJ();
                    xiaoKJ.SetCanShu(YeWuDataModel.GWID, "值数据", yeWuData.QingQiuPiPei);
                    kj.Add(xiaoKJ);
                }
            }
            else
            {
                {                
                    XiaoKJ xiaoKJ = new XiaoKJ();
                    xiaoKJ.SetCanShu(YeWuDataModel.GWID, "值数据", yeWuData.QingQiuPiPei);
                    kj.Add(xiaoKJ);
                }
            }
            this.flowLayoutPanel1.Controls.AddRange(kj.ToArray());
        }

        public void ShuaXin()
        {
            if (kj.Count == 1)
            {
                kj[0].ShuaXinShuJu(YeWuDataModel.Value);
            }
            else if (kj.Count >=4)
            {
                kj[0].ShuaXinShuJu(YeWuDataModel.Up);
                kj[1].ShuaXinShuJu(YeWuDataModel.Low);
                kj[2].ShuaXinShuJu(YeWuDataModel.State);
                kj[3].ShuaXinShuJu(YeWuDataModel.Value);
            }
        }
    }
}
