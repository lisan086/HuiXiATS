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
        private CunType CunType = CunType.XieCANYouZhiRec;
        private SaoMaModel SheBeiID ;
        private PeiZhiLei PeiZhiLei;
        public TiaoShiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(PeiZhiLei peiZhiLei,SaoMaModel sshid)
        {
            this.commBoxE1.SetCanShu<CunType>();
          
            SheBeiID = sshid;
            PeiZhiLei =peiZhiLei;
            
            if (sshid != null)
            {
                this.label2.Text = $"{sshid.Name}";
                this.label2.BackColor = sshid.TX ? Color.Green : Color.Red;
            }
            
        }

        public void ShuaXin()
        {
            this.label2.BackColor = SheBeiID.TX ? Color.Green : Color.Red;
            CunModel model = PeiZhiLei.DataMoXing.IsChengGong(SheBeiID.SheBeiID, CunType);
            if (model.IsZhengZaiCe == 1)
            {
                if (this.label4.Text != model.JiCunQi.Value.ToString())
                {
                    this.label4.Text = model.JiCunQi.Value.ToString();
                }

            }          
            else if (model.IsZhengZaiCe == 0)
            {
                if (this.label4.Text !="进行中")
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
       

        private void commBoxE1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.label3.Text = ChangYong.GetEnumDescription(this.commBoxE1.GetCanShu<CunType>());
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CunType = this.commBoxE1.GetCanShu<CunType>();
            JiCunQiModel model = PeiZhiLei.DataMoXing.GetTiaoShiModel(SheBeiID.SheBeiID, CunType);
            model.Value = this.textBox6.Text;
            PeiZhiLei.XieJiDianQi(model);
        }
    }
}
