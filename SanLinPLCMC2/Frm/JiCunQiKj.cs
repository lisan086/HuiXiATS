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
using ModBuTCP.Model;
using SSheBei.Model;

namespace ModBuTCP.Frm
{
    public partial class JiCunQiKj : UserControl
    {
        private DataCunModel YeWuSheBeiModel;
        public JiCunQiKj()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(DataCunModel model)
        {
            this.commBoxE1.Items.Clear();
            List<string> lisd = ChangYong.MeiJuLisName(typeof(DataType));
            for (int i = 0; i < lisd.Count; i++)
            {
                this.commBoxE1.Items.Add(lisd[i]);
            }
            this.commBoxE2.Items.Clear();
            List<string> lisds = ChangYong.MeiJuLisName(typeof(YingYongType));
            for (int i = 0; i < lisds.Count; i++)
            {
                this.commBoxE2.Items.Add(lisds[i]);
            }
            YeWuSheBeiModel = ChangYong.FuZhiShiTi(model);
            this.textBox2.Text = YeWuSheBeiModel.Name.ToString();
            this.textBox1.Text = YeWuSheBeiModel.JiCunDiZhi.ToString();
            this.textBox4.Text = YeWuSheBeiModel.Count.ToString();
            this.commBoxE1.Text = model.DataType.ToString();
            this.textBox3.Text = YeWuSheBeiModel.BeiChuShu.ToString();
            this.textBox5.Text = YeWuSheBeiModel.XiaoShuWei.ToString();
      
            this.textBox6.Text=YeWuSheBeiModel.ZiDiZhi.ToString();
            this.commBoxE2.Text= model.YingYongType.ToString();
            this.textBox7.Text=model.IsHuiLing.ToString();
            this.textBox8.Text = model.HuiLingYanShi.ToString();
            this.textBox9.Text= model.HuiLingZhi.ToString();
        }

        public DataCunModel GetCanShu()
        {
            DataCunModel model = ChangYong.FuZhiShiTi(YeWuSheBeiModel);
            model.Name = this.textBox2.Text;
            model.BeiChuShu = ChangYong.TryFloat(this.textBox3.Text,0);
            model.JiCunDiZhi = (short)ChangYong.TryInt(this.textBox1.Text, 0);
            model.DataType = ChangYong.GetMeiJuZhi<DataType>(this.commBoxE1.Text);
            model.Count = (short)ChangYong.TryInt(this.textBox4.Text, 0);
            model.XiaoShuWei = ChangYong.TryInt(this.textBox5.Text, 0);
            model.ZiDiZhi = ChangYong.TryInt(this.textBox6.Text, 0);
            model.YingYongType = ChangYong.GetMeiJuZhi<YingYongType>(this.commBoxE2.Text);
            model.IsHuiLing = ChangYong.TryInt(this.textBox7.Text, 0);
            model.HuiLingYanShi = ChangYong.TryInt(this.textBox8.Text, 0);
            model.HuiLingZhi = this.textBox9.Text;
            return model;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }
    }
}
