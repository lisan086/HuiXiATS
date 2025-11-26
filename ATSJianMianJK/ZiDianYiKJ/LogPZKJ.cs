using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.Log;
using CommLei.JiChuLei;

namespace ATSJianMianJK.ZiDianYiKJ
{
    public partial class LogPZKJ : UserControl
    {
        public LogPZKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(GuiGeModel guiGe)
        {
            this.textBox1.Text = guiGe.LogName;
            this.textBox2.Text = guiGe.GuiZe;
            this.textBox3.Text = guiGe.TDID.ToString();
            this.checkBox1.Checked = guiGe.IsKaiQi;
            List<string> meijus = ChangYong.MeiJuLisName(typeof(RiJiEnum));
            this.commBoxE1.Items.Clear();
            for (int i = 0; i < meijus.Count; i++)
            {
                this.commBoxE1.Items.Add(meijus[i]);
            }
            if (this.commBoxE1.Items.Count>0)
            {
                this.commBoxE1.SelectedIndex = 0;
            }
            for (int i = 0; i < guiGe.RiJiEnum.Count; i++)
            {
                int index = this.jiHeDataGrid1.Rows.Add();
                this.jiHeDataGrid1.Rows[index].Cells[0].Value = guiGe.RiJiEnum[i].ToString();
                this.jiHeDataGrid1.Rows[index].Height = 32;
            }
        }

        public GuiGeModel GetGuiGeModel()
        {
            GuiGeModel model = new GuiGeModel();
            model.LogName = this.textBox1.Text;
            model.GuiZe = this.textBox2.Text;
            model.TDID = ChangYong.TryInt(this.textBox3.Text,-1);
            model.IsKaiQi = this.checkBox1.Checked;
            model.RiJiEnum.Clear();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                string mushi = this.jiHeDataGrid1.Rows[i].Cells[0].Value.ToString();
                RiJiEnum riji = ChangYong.GetMeiJuZhi<RiJiEnum>(mushi);
                model.RiJiEnum.Add(riji);
            }
            return model;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mingcheng = this.commBoxE1.Text;
            bool cunzai = false;
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                if (this.jiHeDataGrid1.Rows[i].Cells[0].Value.ToString().Equals(mingcheng))
                {
                    cunzai = true;
                    break;
                }
            }
            if (cunzai == false)
            {
                int index = this.jiHeDataGrid1.Rows.Add();
                this.jiHeDataGrid1.Rows[index].Cells[0].Value = mingcheng;
                this.jiHeDataGrid1.Rows[index].Height = 32;
            }
        }

        private void jiHeDataGrid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex==0)
                {
                    this.jiHeDataGrid1.Rows.RemoveAt(e.RowIndex);
                }
            }
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
