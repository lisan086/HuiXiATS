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
using SSheBei.Model;
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.Frm
{
    public partial class TiaoShiKJ : UserControl
    {
        private CunType CunType = CunType.XiePing;
        private SaoMaModel SaoMaModel;
        private PeiZhiLei PeiZhiLei;
        public TiaoShiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(SaoMaModel maModel,PeiZhiLei peiZhiLei)
        {
            SaoMaModel=maModel;
            PeiZhiLei=peiZhiLei;
            this.label2.Text = $"{maModel.Name},{maModel.Ip},{maModel.WangKaName}";
            this.label2.BackColor = maModel.TX ? Color.Green : Color.Red;
        }

        public void ShuaXin()
        {
            if (SaoMaModel.TX)
            {
                if (this.label2.BackColor != Color.Green)
                {
                    this.label2.BackColor = Color.Green;
                }
            }
            else
            {
                if (this.label2.BackColor != Color.Red)
                {
                    this.label2.BackColor = Color.Red;
                }
            }
            CunModel model = PeiZhiLei.DataMoXing.IsChengGong(SaoMaModel.SheBeiID,CunType);
            if (model != null)
            {
                if (model.IsZhengZaiCe == 1)
                {
                    if (this.label4.Text != model.JiCunQi.Value.ToString())
                    {
                        this.label4.Text = model.JiCunQi.Value.ToString();
                    }

                }
                else if (model.IsZhengZaiCe == 0)
                {
                    if (this.label4.Text != "进行中")
                    {
                        this.label4.Text = "进行中";
                    }
                }
                else
                {
                    if (this.label4.Text != "超时或者不对")
                    {
                        this.label4.Text = "超时或者不对";
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
          
            CunModel model = PeiZhiLei.DataMoXing.IsChengGong(SaoMaModel.SheBeiID,CunType.KaiShiPing).FuZhi();
       
            PeiZhiLei.XieJiDianQi(model.JiCunQi);
        }

      
    }
}
