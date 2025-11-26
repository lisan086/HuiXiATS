using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.Frm
{
    public partial class TiaoShiKJ : UserControl
    {
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
            this.label4.Text=maModel.Name;
            this.label4.BackColor = maModel.TX ? Color.Green : Color.Red;
        }

        public void ShuaXin()
        {
            if (SaoMaModel.TX)
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
         
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(SaoMaModel.SheBeiID,CunType.Du读数据);
            if (model.IsZhengZaiCe == 1)
            {
                if (this.textBox1.Text != model.JiCunQi.Value.ToString())
                {
                    this.textBox1.Text = model.JiCunQi.Value.ToString();
                }

            }
            else if (model.IsZhengZaiCe == 0)
            {
                if (this.textBox1.Text !="进行中")
                {
                    this.textBox1.Text = "进行中";
                }
            }
            else 
            {
                if (this.textBox1.Text != "超时或者不对")
                {
                    this.textBox1.Text = "超时或者不对";
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            PeiZhiLei.XieJiDianQi(PeiZhiLei.DataMoXing.GetModel(SaoMaModel.SheBeiID,CunType.Xie开启扫码));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PeiZhiLei.XieJiDianQi(PeiZhiLei.DataMoXing.GetModel(SaoMaModel.SheBeiID, CunType.Xie关闭扫码));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PeiZhiLei.XieJiDianQi(PeiZhiLei.DataMoXing.GetModel(SaoMaModel.SheBeiID, CunType.Xie开启设备));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PeiZhiLei.XieJiDianQi(PeiZhiLei.DataMoXing.GetModel(SaoMaModel.SheBeiID, CunType.Xie关闭设备));
        }
    }
}
