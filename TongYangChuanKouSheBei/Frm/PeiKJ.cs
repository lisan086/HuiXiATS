using CommLei.JiChuLei;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XiangTongChuanKouSheBei.Frm;
using ZhongWangSheBei.Model;

namespace ZhongWangSheBei.Frm
{
    public partial class PeiKJ : UserControl
    {
        private ZiSheBeiModel ZSCOMModel;
        public PeiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(ZiSheBeiModel model)
        {
            this.comboBox2.Items.Clear();
            List<string> meiju = ChangYong.MeiJuLisName(typeof(SheBeiType));
            for (int i = 0; i < meiju.Count; i++)
            {
                this.comboBox2.Items.Add(meiju[i]);
            }
            if (this.comboBox2.Items.Count > 0)
            {
                this.comboBox2.SelectedIndex = 0;
            }
            ZSCOMModel = ChangYong.FuZhiShiTi(model);
           
            this.textBox1.Text = ZSCOMModel.DuChaoShiTime.ToString();
            this.textBox8.Text= ZSCOMModel.ZiID.ToString();        
            this.textBox4.Text = ZSCOMModel.Name.ToString();         
            this.textBox2.Text = ZSCOMModel.DiZhi.ToString();
            this.textBox5.Text = ZSCOMModel.XieRuTime.ToString();
            this.comboBox2.Text= ZSCOMModel.SheBeiType.ToString();
            for (int i = 0; i < model.ZhiLingS.Count; i++)
            {
                FuZhi(model.ZhiLingS[i]);
            }
        }
        public ZiSheBeiModel GetCanShu()
        {
            ZiSheBeiModel model = ChangYong.FuZhiShiTi(ZSCOMModel);
            model.DuChaoShiTime = ChangYong.TryInt(this.textBox1.Text,300);
            model.ZiID= ChangYong.TryInt(this.textBox8.Text, 1);
            model.Name = this.textBox4.Text;
            model.DiZhi = ChangYong.TryInt(this.textBox2.Text, 1);
            model.XieRuTime = ChangYong.TryInt(this.textBox8.Text, 1);
            model.SheBeiType = ChangYong.GetMeiJuZhi<SheBeiType>(this.comboBox2.Text);
            model.ZhiLingS.Clear();
            model.ZhiLingS = GetModel();
            return model;
        }
        private void FuZhi(DuZhiLingModel model)
        {
             int i= this.dataGridView1.Rows.Add();

            this.dataGridView1.Rows[i].Cells[0].Value = model.ToString();
            this.dataGridView1.Rows[i].Cells[1].Value = "删除";
            this.dataGridView1.Rows[i].Height = 32;
            this.dataGridView1.Rows[i].Tag = model;
      
        }

        private List<DuZhiLingModel> GetModel()
        {
            List<DuZhiLingModel> lis = new List<DuZhiLingModel>();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1.Rows[i].Tag is DuZhiLingModel)
                {
                    DuZhiLingModel model = this.dataGridView1.Rows[i].Tag as DuZhiLingModel;
                    lis.Add(model);
                }
             
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

        private void button2_Click(object sender, EventArgs e)
        {
            FuZhi(new DuZhiLingModel());
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        int i = e.RowIndex;
                        if (this.dataGridView1.Rows[i].Tag is DuZhiLingModel)
                        {
                            DuZhiLingModel model = this.dataGridView1.Rows[i].Tag as DuZhiLingModel;
                            CunSheFrm frm = new CunSheFrm();
                            frm.SetCanShu(model);
                            if (frm.ShowDialog(this)==DialogResult.OK)
                            {
                                DuZhiLingModel shidmodel= frm.GetCanShu();
                                this.dataGridView1.Rows[i].Tag = shidmodel;

                                this.dataGridView1.Rows[i].Cells[0].Value = shidmodel.ToString();
                            }
                        }
                    }
                    else if (e.ColumnIndex == 1)
                    { 
                        this.dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DuJiCunQiFrm frm = new DuJiCunQiFrm();
            frm.SetCanShu(ZSCOMModel.LisJiCunQi);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                ZSCOMModel.LisJiCunQi = ChangYong.FuZhiShiTi(frm.GetJiCunQi());
            }
        }
    }
}
