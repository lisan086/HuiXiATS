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
            ZSCOMModel = ChangYong.FuZhiShiTi(model);
            txbMoShi.Text = ZSCOMModel.IpOrCom;
            this.textBox1.Text = ZSCOMModel.Port.ToString();
            this.textBox2.Text = ZSCOMModel.QieHuanTime.ToString();
            this.textBox3.Text = ZSCOMModel.SheBeiID.ToString();
            for (int i = 0; i < model.LisJiLu.Count; i++)
            {
                FuZhi(model.LisJiLu[i]);
            }
        }
        public ZSModel GetCanShu()
        {
            ZSModel model = ChangYong.FuZhiShiTi(ZSCOMModel);
            model.IpOrCom = txbMoShi.Text;
            model.Port = ChangYong.TryInt(this.textBox1.Text, 38400);
            model.QieHuanTime = ChangYong.TryInt(this.textBox2.Text, 80);
            model.SheBeiID = ChangYong.TryInt(this.textBox3.Text,1);
            model.LisJiLu.Clear();
            model.LisJiLu = GetModel();
            return model;
        }
        private void FuZhi(ZiSheBeiModel model)
        {
            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = model.ZSID;
            this.dataGridView1.Rows[index].Cells[1].Value = model.DiZhi;
            this.dataGridView1.Rows[index].Cells[2].Value = model.JiLu;
            this.dataGridView1.Rows[index].Cells[3].Value = model.ZiName;
            this.dataGridView1.Rows[index].Cells[4].Value = model.ChengShu.ToString();
            this.dataGridView1.Rows[index].Cells[5].Value = model.QiShiDiZhi.ToString();
            this.dataGridView1.Rows[index].Cells[6].Value = "删除";
            this.dataGridView1.Rows[index].Height = 32;
        }

        private List<ZiSheBeiModel> GetModel()
        {
            List<ZiSheBeiModel> lis = new List<ZiSheBeiModel>();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                ZiSheBeiModel model = new ZiSheBeiModel();
                model.ZSID = ChangYong.TryInt(this.dataGridView1.Rows[i].Cells[0].Value, 1);
                model.DiZhi = ChangYong.TryInt(this.dataGridView1.Rows[i].Cells[1].Value, 1);
                model.JiLu = ChangYong.TryInt(this.dataGridView1.Rows[i].Cells[2].Value, 1);
                model.ZiName = ChangYong.TryStr(this.dataGridView1.Rows[i].Cells[3].Value, "");
                model.ChengShu = ChangYong.TryDouble(this.dataGridView1.Rows[i].Cells[4].Value, 1);
                model.QiShiDiZhi = ChangYong.TryInt(this.dataGridView1.Rows[i].Cells[5].Value, 0);
                lis.Add(model);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 6)
                {
                    this.dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FuZhi(new ZiSheBeiModel());
        }
    }
}
