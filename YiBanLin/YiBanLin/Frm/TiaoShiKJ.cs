using CommLei.JiChuLei;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YiBanLin.Model;
using YiBanLin.ShiXian;

namespace YiBanLin.Frm
{
    public partial class TiaoShiKJ : UserControl
    {
        private CunType CunType = CunType.XieFanHuiLin0;
        private LinModel cuModel;
        private PeiZhiLei PeiZhiLei;
        public TiaoShiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(LinModel cunModel,PeiZhiLei peiZhiLei)
        {
            cuModel = cunModel;
            PeiZhiLei=peiZhiLei;
            this.label4.Text = $"{cuModel.Name}Lin调试";
            this.label4.BackColor = cunModel.Tx ? Color.Green : Color.Red;
        }

        public void ShuaXin()
        {
            if (cuModel.Tx)
            {
                if (this.label4.BackColor != Color.Green)
                {
                    this.label4.BackColor = Color.Green;
                }
            }
            else
            {
                if (this.label4.BackColor != Color.Red)
                {
                    this.label4.BackColor = Color.Red;
                }
            }
            CunModel model = PeiZhiLei.DataMoXing.GetModel(cuModel.SheBeiID,CunType);
            if (model != null)
            {
                if (model.IsZhengZaiCe == 1)
                {
                    if (this.label2.Text != $"{CunType}{ model.JiCunQi.Value.ToString()}")
                    {
                        this.label2.Text = $"{CunType}{ model.JiCunQi.Value.ToString()}";
                    }

                }
                else if (model.IsZhengZaiCe == 0)
                {
                    if (this.label2.Text != $"{CunType}进行中")
                    {
                        this.label2.Text = $"{CunType}进行中";
                    }
                }
                else
                {
                    if (this.label2.Text != $"{CunType}超时或者不对")
                    {
                        this.label2.Text = $"{CunType}超时或者不对";
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CunType = CunType.XieFanHuiLin0;
            CunModel cun = ChangYong.FuZhiShiTi(PeiZhiLei.DataMoXing.GetModel(cuModel.SheBeiID,CunType));
            cun.JiCunQi.Value = textBox2.Text;
            PeiZhiLei.XieJiDianQi(cun.JiCunQi);
           
        }

        private void button2_Click(object sender, EventArgs e)
        {


            {
                CunModel cun = PeiZhiLei.DataMoXing.GetModel(cuModel.SheBeiID, CunType.XieGuanLin);
                cun.JiCunQi.Value = textBox2.Text;
                PeiZhiLei.XieJiDianQi(cun.JiCunQi);
            }
            {
                CunModel cun = PeiZhiLei.DataMoXing.GetModel(cuModel.SheBeiID, CunType.XieKaiLin);
                cun.JiCunQi.Value = textBox2.Text;
                PeiZhiLei.XieJiDianQi(cun.JiCunQi);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            CunType = CunType.XieFanHuiLin1;
            CunModel cun = PeiZhiLei.DataMoXing.GetModel(cuModel.SheBeiID, CunType);
            cun.JiCunQi.Value = textBox2.Text;
            PeiZhiLei.XieJiDianQi(cun.JiCunQi);

        }
    }
}
