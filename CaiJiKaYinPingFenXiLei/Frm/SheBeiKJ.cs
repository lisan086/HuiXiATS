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
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.Frm
{
    public partial class SheBeiKJ : UserControl
    {
        private SaoMaModel SaoMaModel;
        public SheBeiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(SaoMaModel model)
        {
            SaoMaModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = model.TaskName;
            this.textBox1.Text = model.CaiJiPort.ToString();

            this.textBox4.Text = model.SheBeiID.ToString();
            this.textBox5.Text = model.Name.ToString();
            this.textBox2.Text = model.CaiJiShuLiang.ToString();
            this.textBox3.Text = model.CaiYangLv.ToString();
            this.textBox6.Text = $"{model.LvBoPinLv}";
            this.textBox7.Text = $"{model.LvBoWidth}";
            this.textBox8.Text = $"{model.LinMinDuadVoltage}";
            this.textBox9.Text = $"{model.LinMinDuFaDaZengYi}";
            this.textBox10.Text = $"{model.CaiJiMiaoSu}";
            this.textBox11.Text = $"{model.linMinDuDaQiYa}";
            this.textBox12.Text = $"{model.GuiYiZhi}";
            this.checkBox1.Checked = model.IsLvBo;
            this.textBox13.Text = $"{model.FuZhi}";
            this.textBox14.Text= $"{model.ZaoYing}";
        }
        public SaoMaModel GetSaoMaModel() 
        {
            SaoMaModel model = ChangYong.FuZhiShiTi(SaoMaModel);
            model.TaskName = this.txbMoShi.Text;
            model.CaiJiPort = this.textBox1.Text;          
            model.SheBeiID = ChangYong.TryInt(this.textBox4.Text, 0);
            model.Name=this.textBox5.Text;
            model.CaiJiShuLiang = ChangYong.TryInt(this.textBox2.Text, 10);
            model.CaiYangLv = ChangYong.TryInt(this.textBox3.Text, 1000);
            model.LvBoPinLv = ChangYong.TryDouble(this.textBox6.Text, 0);
            model.LvBoWidth = ChangYong.TryDouble(this.textBox7.Text, 0);
            model.LinMinDuadVoltage = ChangYong.TryDouble(this.textBox8.Text, 0);
            model.LinMinDuFaDaZengYi = ChangYong.TryDouble(this.textBox9.Text, 0);
            model.CaiJiMiaoSu = ChangYong.TryDouble(this.textBox10.Text, 0);
            model.linMinDuDaQiYa = ChangYong.TryDouble(this.textBox11.Text, 0);
            model.IsLvBo = this.checkBox1.Checked;
            model.GuiYiZhi= ChangYong.TryDouble(this.textBox12.Text, 0);
            model.FuZhi = ChangYong.TryDouble(this.textBox13.Text, 0);
            model.ZaoYing= ChangYong.TryDouble(this.textBox14.Text, 0);
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
