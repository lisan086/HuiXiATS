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
using JieMianLei.FuFrom.KJ;
using YiBanLin.Model;

namespace YiBanLin.Frm
{
    public partial class SheBeiKJ : UserControl
    {
        private LinModel linkModel;
        public SheBeiKJ()
        {
            InitializeComponent();
            // 初始化下拉框选项
            comboBox1.Items.AddRange(new object[] {
            "0:从机模式",
            "1:主机模式"
        });
            comboBox1.SelectedIndex = 0;
        }

        public void SetCanShu(LinModel model)
        {
            linkModel = ChangYong.FuZhiShiTi(model);

            this.textBox5.Text = model.SheBeiID.ToString();
            this.textBox2.Text = model.Name;
            this.textBox1.Text = model.BaudRate.ToString();
            this.textBox3.Text=model.DuiYingLin.ToString();
            this.textBox4.Text=model.ChaoShiTime.ToString();
            // 设置主从模式
            for (int i = 0; i < this.comboBox1.Items.Count; i++)
            {
                string zhi = this.comboBox1.Items[i].ToString().Split(':')[0];
                if (zhi == model.MasterMode.ToString())
                {
                    this.comboBox1.SelectedIndex = i;
                    break;
                }
            }

           
        }


        public LinModel GetLinModel()
        {
            LinModel model = ChangYong.FuZhiShiTi(linkModel);


            model.Name = this.textBox2.Text;
            model.SheBeiID = ChangYong.TryInt(this.textBox5.Text, 0);
            model.BaudRate = ChangYong.TryInt(this.textBox1.Text, 19200);
            model.MasterMode = ChangYong.TryByte(this.comboBox1.Text.Split(':')[0], 0);
            model.DuiYingLin = ChangYong.TryInt(this.textBox3.Text,0);
            model.ChaoShiTime = ChangYong.TryInt(this.textBox4.Text, 5000);
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
