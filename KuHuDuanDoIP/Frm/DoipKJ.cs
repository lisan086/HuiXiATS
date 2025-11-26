using CommLei.JiChuLei;
using KuHuDuanDoIP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KuHuDuanDoIP.Frm
{
    public partial class DoipKJ : UserControl
    {
        private SheBeiModel SheBeiModel;
        public DoipKJ()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        public void SetCanShu(SheBeiModel model)
        {
            SheBeiModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = SheBeiModel.IP;
            this.textBox1.Text = SheBeiModel.DuanKou.ToString();
            this.textBox2.Text = SheBeiModel.SA;
            this.textBox3.Text=SheBeiModel.SheBeiID.ToString();
            this.textBox4.Text=SheBeiModel.ChaoTime.ToString();
            this.textBox5.Text = SheBeiModel.Name;
            this.textBox6.Text = SheBeiModel.MiYaoPath;
            this.textBox7.Text = SheBeiModel.XinTiaoZhiLing;
            this.textBox8.Text = SheBeiModel.WoShouZhiLing;
            for (int i = 0; i < SheBeiModel.GuoLuBaoWen.Count; i++)
            {
                FuZhi(SheBeiModel.GuoLuBaoWen[i]);
            }
        }

        public SheBeiModel GetModel()
        {
            SheBeiModel model = ChangYong.FuZhiShiTi(SheBeiModel);
            model.IP = this.txbMoShi.Text;
            model.DuanKou = ChangYong.TryInt(this.textBox1.Text,13400);
            model.SA=this.textBox2.Text;
            model.ChaoTime= ChangYong.TryFloat(this.textBox4.Text, 20);
            model.MiYaoPath=this.textBox6.Text;
            model.Name=this.textBox5.Text;
            model.XinTiaoZhiLing=this.textBox7.Text;
            model.WoShouZhiLing = this.textBox8.Text;
            model.SheBeiID= ChangYong.TryInt(this.textBox3.Text, 13400);
            List<string> gulu = new List<string>();
            for (int i = 0; i <this.dataGridView1.Rows.Count; i++)
            {
                string baowen =ChangYong.TryStr( this.dataGridView1.Rows[i].Cells[0].Value,"");
                if (string.IsNullOrEmpty(baowen)==false)
                {
                    if (gulu.IndexOf(baowen) < 0)
                    {
                        gulu.Add(baowen);
                    }
                }
               
            }
            model.GuoLuBaoWen = gulu;
            return model;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FuZhi("");
        }

        private void FuZhi(string baowen)
        {
            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = baowen;
            this.dataGridView1.Rows[index].Cells[1].Value = "删除";
            this.dataGridView1.Rows[index].Height = 32;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    this.dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

       

        private void button3_Click_1(object sender, EventArgs e)
        {
            string wenjian = this.textBox6.Text;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = wenjian;
            if (openFileDialog.ShowDialog(this)==DialogResult.OK)
            {
                this.textBox6.Text=openFileDialog.FileName;
            }
        }
    }
}
