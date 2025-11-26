using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUI.FuFrom.XinWeiHuFrm;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using DBPLCS7.Model;
using SSheBei.Model;
using SSheBei.PeiZhi;

namespace DBPLCS7.Frm.KJ
{
    public partial class SheBeiKJ : UserControl
    {
        private PLCShBeiModel PLCShBeiModel;
        public SheBeiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(PLCShBeiModel model)
        {
            PLCShBeiModel = ChangYong.FuZhiShiTi(model);
            this.textBox2.Text = PLCShBeiModel.PLCName;
            this.commBoxE1.Items.Clear();
            List<string> lisd = ChangYong.MeiJuLisName(typeof(PCLType));
            for (int i = 0; i < lisd.Count; i++)
            {
                this.commBoxE1.Items.Add(lisd[i]);
            }
            this.commBoxE1.Text = PLCShBeiModel.PCLType.ToString();
            this.textBox4.Text = PLCShBeiModel.IP;
            this.textBox3.Text = PLCShBeiModel.Port.ToString();
            this.textBox6.Text = PLCShBeiModel.Rack.ToString();
            this.textBox1.Text = PLCShBeiModel.Slot.ToString();
            this.textBox5.Text = PLCShBeiModel.CaiJiYanShi.ToString();
            this.textBox7.Text = PLCShBeiModel.XieRuYanShi.ToString();
        }

        public PLCShBeiModel GetModel()
        {
            PLCShBeiModel model = ChangYong.FuZhiShiTi(PLCShBeiModel);
            model.PLCName = this.textBox2.Text;
            model.PCLType = ChangYong.GetMeiJuZhi<PCLType>(this.commBoxE1.Text);
            model.IP = this.textBox4.Text;
            model.Port = ChangYong.TryInt(this.textBox3.Text,502);
            model.Rack= ChangYong.TryShort(this.textBox6.Text, 0);
            model.Slot= ChangYong.TryShort(this.textBox1.Text, 0);
            model.CaiJiYanShi = ChangYong.TryInt(this.textBox5.Text, 5);
            model.XieRuYanShi= ChangYong.TryInt(this.textBox7.Text, 5);         
            return model;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            JiChuLieBiaoWeiHuFrom<PLCJiCunQiModel> frm = new JiChuLieBiaoWeiHuFrom<PLCJiCunQiModel>();
            Func<List<PLCJiCunQiModel>, PLCJiCunQiModel, int> queding = (x, y) => {
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i].Name.Equals(y.Name))
                    {
                        return i;
                    }
                }
                return -1;
            };
            Func<List<PLCJiCunQiModel>, PLCJiCunQiModel, int> shanchu = (x, y) => {
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i].Name.Equals(y.Name))
                    {
                        return i;
                    }
                }
                return -1;
            };
            Func<List<PLCJiCunQiModel>, string, List<PLCJiCunQiModel>> chaozhao = (x, y) => {
                List<PLCJiCunQiModel> lis = new List<PLCJiCunQiModel>();
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i].Name.Contains(y))
                    {
                        lis.Add(x[i]);
                    }
                }
                return lis;
            };
            frm.SetCanShu("设置读参数","名称",false, PLCShBeiModel.JiCunQi, queding, chaozhao, shanchu);
            frm.SetKJ<JiCunQiKJ>(new JiCunQiKJ());
            frm.ShowDialog(this);
            PLCShBeiModel.JiCunQi = ChangYong.FuZhiShiTi( frm.GetCanShu());
        }

    
    }
}
