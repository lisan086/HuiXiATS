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

namespace MeiGeXinLei.Frm
{
    public partial class MingLingPeiKJ : UserControl
    {
        private ZhiLingModel SaoMaModel;
        public MingLingPeiKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(ZhiLingModel model)
        {
            SaoMaModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = model.MingCheng;
            this.textBox1.Text = model.ZhiLingID.ToString();
            this.textBox4.Text = model.ZhiLing.ToString();
            this.textBox5.Text = model.ZhiLingJieSu.ToString();
            List<string> lis = ChangYong.MeiJuLisName(typeof(ZhiLingType));
            this.commBoxE1.Items.Clear();
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].Contains("QiTa") || lis[i].Contains("ZiJieGuo"))
                {
                    continue;
                }
                this.commBoxE1.Items.Add(lis[i]);
            }
            this.commBoxE1.Text=model.ZhiLingType.ToString();
            foreach (var item in model.LisData)
            {
                FuZhi(item);
            }
        }
        public ZhiLingModel GetSaoMaModel()
        {
            ZhiLingModel model = ChangYong.FuZhiShiTi(SaoMaModel);
            model.MingCheng = this.txbMoShi.Text;
            model.ZhiLingID = ChangYong.TryInt(this.textBox1.Text, 0);
            model.ZhiLing = this.textBox4.Text;
            model.ZhiLingJieSu = this.textBox5.Text;
            model.ZhiLingType = ChangYong.GetMeiJuZhi<ZhiLingType>(this.commBoxE1.Text);
            model.LisData = GetModel();
            return model;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }
        private List<DataSdModel> GetModel()
        {
            List<DataSdModel> lis = new List<DataSdModel>();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                DataSdModel model = new DataSdModel();
                model.TongDao = ChangYong.TryInt(this.dataGridView1.Rows[i].Cells[0].Value, 1);
                model.Name = ChangYong.TryStr(this.dataGridView1.Rows[i].Cells[1].Value, "");
                model.CanShu = ChangYong.TryStr(this.dataGridView1.Rows[i].Cells[2].Value, "");
                lis.Add(model);
            }
            return lis;
        }

        private void FuZhi(DataSdModel model)
        {
            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = model.TongDao;
            this.dataGridView1.Rows[index].Cells[1].Value = model.Name;
            this.dataGridView1.Rows[index].Cells[2].Value = model.CanShu;
            this.dataGridView1.Rows[index].Cells[3].Value = "删除";
            this.dataGridView1.Rows[index].Height = 32;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                {
                    this.dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FuZhi(new DataSdModel());
        }
    }
}
