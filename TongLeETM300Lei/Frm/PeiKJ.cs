using CommLei.JiChuLei;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhongWangSheBei.Model;

namespace ZhongWangSheBei.Frm
{
    public partial class PeiKJ : UserControl
    {
        private ZSModel ZSCOMModel;
        public PeiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(ZSModel model)
        {
            AddShuJu();
            ZSCOMModel = ChangYong.FuZhiShiTi(model);
            txbMoShi.Text = ZSCOMModel.IpOrCom;
            this.textBox1.Text = ZSCOMModel.Port.ToString();
            this.textBox8.Text= ZSCOMModel.DianYaChengShu.ToString();
            this.textBox3.Text = ZSCOMModel.SheBeiID.ToString();
            this.textBox4.Text = ZSCOMModel.Name.ToString();
            this.textBox9.Text = ZSCOMModel.GongLuChengShu.ToString();
            this.textBox2.Text = ZSCOMModel.QiShiDiZhi.ToString();
            this.textBox5.Text =ChangYong.FenGeDaBao( ZSCOMModel.DiZhi,",");
            this.textBox6.Text = ZSCOMModel.ChangDu.ToString();
            this.textBox7.Text = ZSCOMModel.DianLiuChengShu.ToString();
            this.textBox11.Text = ZSCOMModel.SetDianLiu.ToString();
            this.textBox10.Text = ZSCOMModel.SetDianYa.ToString();
            this.textBox12.Text= ZSCOMModel.ChaoShiTime.ToString();
            this.textBox13.Text=ZSCOMModel.XieRuTime.ToString();
            this.textBox14.Text= ZSCOMModel.QieHuanTime.ToString();
            for (int i = 0; i < model.DuiYingDiZhi.Count; i++)
            {
                FuZhi(model.DuiYingDiZhi[i]);
            }
        }
        public ZSModel GetCanShu()
        {
            ZSModel model = ChangYong.FuZhiShiTi(ZSCOMModel);
            model.IpOrCom = txbMoShi.Text;
            model.Port = ChangYong.TryInt(this.textBox1.Text, 38400);
            model.DianYaChengShu = ChangYong.TryDouble(this.textBox8.Text, 0.001);
            model.SheBeiID = ChangYong.TryInt(this.textBox3.Text,1);
            model.Name = this.textBox4.Text;
            model.GongLuChengShu = ChangYong.TryDouble(this.textBox9.Text, 1);
            model.QiShiDiZhi = ChangYong.TryInt(this.textBox2.Text, 1);
            model.DiZhi =ChangYong.JieGeInt(this.textBox5.Text,',');
            model.ChangDu = ChangYong.TryInt(this.textBox6.Text, 1);
            model.DianLiuChengShu = ChangYong.TryDouble(this.textBox7.Text, 1);
            model.SetDianLiu = ChangYong.TryDouble(this.textBox11.Text, 1);
            model.SetDianYa = ChangYong.TryDouble(this.textBox10.Text, 1);
            model.ChaoShiTime= ChangYong.TryInt(this.textBox12.Text, 1);
            model.XieRuTime = ChangYong.TryInt(this.textBox13.Text, 1);
            model.QieHuanTime = ChangYong.TryInt(this.textBox14.Text, 1);
            model.ZhiLing.Clear();
            model.DuiYingDiZhi.Clear();
            model.DuiYingDiZhi = GetModel();
            return model;
        }
        private void FuZhi(string model)
        {
            string[] fengws = model.Split(':');
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                string name = ChangYong.TryStr(this.dataGridView1.Rows[i].Cells[0].Value,"");
                if (name.Equals(fengws[0]))
                {
                    this.dataGridView1.Rows[i].Cells[0].Value = fengws[0];
                    this.dataGridView1.Rows[i].Cells[1].Value = fengws.Length>1?fengws[1]:"";
                    this.dataGridView1.Rows[i].Cells[2].Value = fengws.Length > 2 ? fengws[2] : "";
                    this.dataGridView1.Rows[i].Cells[3].Value = fengws.Length > 3 ? fengws[3] : "";
                    break;
                }
            }
          
        }

        private List<string> GetModel()
        {
            List<string> lis = new List<string>();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                string zhi = $"{this.dataGridView1.Rows[i].Cells[0].Value}:{this.dataGridView1.Rows[i].Cells[1].Value}:{this.dataGridView1.Rows[i].Cells[2].Value}:{this.dataGridView1.Rows[i].Cells[3].Value}";
              
                lis.Add(zhi);
            }
            return lis;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

      

        private void AddShuJu()
        {
            this.dataGridView1.Rows.Clear();
            List<string> lisxie = ChangYong.MeiJuLisName(typeof(CunType));
            for (int i = 0; i < lisxie.Count; i++)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = lisxie[i];
                this.dataGridView1.Rows[index].Cells[1].Value = "";
                this.dataGridView1.Rows[index].Cells[2].Value = "";
                this.dataGridView1.Rows[index].Cells[3].Value = "";
               
                this.dataGridView1.Rows[index].Height = 32;
            }
        }
    }
}
