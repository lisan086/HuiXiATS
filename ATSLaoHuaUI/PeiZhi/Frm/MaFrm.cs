using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK;
using JieMianLei.FuFrom;

namespace ATSLaoHuaUI.PeiZhi.Frm
{
    public partial class MaFrm : BaseFuFrom
    {

        private List<TextBox> LisBoxs = new List<TextBox>();
        public int TDID { get; set; } = -1;

        public List<string> SaoMaShuJu { get; set; } = new List<string>();
        public MaFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(string name,int tdid,int count,List<string> mashu)
        {
            TDID = tdid;
            this.labFbiaoTi.Text = name;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();
            if (count > 0)
            {

                float bili = (1f / 6f) * 100f;
             
                for (int i = 0; i < 12; i++)
                {
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, bili));
                }
                this.tableLayoutPanel1.RowCount = 12;
            
                for (int i = 0; i < 3; i++)
                {
                    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
                }
                this.tableLayoutPanel1.ColumnCount = 3;
                for (int i = 0; i < count; i++)
                {
                    Panel panel = new Panel();
                    panel.Dock = DockStyle.Fill;
                    {
                        Label lab = new Label();
                        lab.AutoSize = false;
                        lab.Font = new Font("微软姚黑",12);
                        lab.Size = new Size(60,38);
                        lab.Text = $"码{i+1}";
                        lab.Dock =DockStyle.Left;
                        lab.TextAlign = ContentAlignment.MiddleCenter;

                        TextBox textBox = new TextBox();
                        textBox.Dock =DockStyle.Fill;
                        textBox.Font = new Font("微软姚黑", 12);
                        textBox.Text = mashu[i];
                        panel.Controls.Add(textBox);
                        panel.Controls.Add(lab);
                        LisBoxs.Add(textBox);
                        textBox.KeyDown += TextBox_KeyDown;
                    }
                    this.tableLayoutPanel1.Controls.Add(panel);
                }

             
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox)
            {
              
                if (e.KeyCode == Keys.Enter)
                {
                    string erweima =(sender as TextBox).Text ;
                    int zaiweizhi = -1;
                    for (int i = 0; i < LisBoxs.Count; i++)
                    {
                        if (LisBoxs[i].Equals((sender as TextBox)))
                        {
                            zaiweizhi = i;
                            break;
                        }

                    }
                    if (string.IsNullOrEmpty(erweima) == false)
                    {
                       
                        List<string> xiangtongs = GetMas(zaiweizhi);
                        int index = xiangtongs.IndexOf(erweima);
                        if (index >= 0)
                        {
                            this.QiDongTiShiKuang($"与{index + 1}码相同");
                            (sender as TextBox).Text = "";
                            (sender as TextBox).SelectAll();
                            (sender as TextBox).Focus();
                        }
                        else
                        {

                            SetJiaoDian(zaiweizhi + 1);
                        }
                    }
                    else
                    {

                        SetJiaoDian(zaiweizhi + 1);
                    }
                }
            }
        }

        private void SetJiaoDian(int index)
        {
            if (index < LisBoxs.Count)
            {
               
                LisBoxs[index].SelectAll();
                LisBoxs[index].Focus();
            }
            else
            {
                this.button1.Focus();
            }
        }

        private List<string> GetMas(int index=-1)
        {
            if (index < 0)
            {
                List<string> lis = new List<string>();
                for (int i = 0; i < LisBoxs.Count; i++)
                {
                    string wenben = LisBoxs[i].Text.Trim();
                    lis.Add(wenben);
                }
                return lis;
            }
            else
            {
                List<string> lis = new List<string>();
                for (int i = 0; i < LisBoxs.Count; i++)
                {
                    if (i < index)
                    {
                        string wenben = LisBoxs[i].Text.Trim();
                        lis.Add(wenben);
                    }
                }
                return lis;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaoMaShuJu.Clear();
            List<string> lis = GetMas();
            bool iskong = false;
            for (int i = 0; i < lis.Count; i++)
            {           
                if (string.IsNullOrEmpty(lis[i]) ==false)
                {
                    iskong = true;
                    if (SaoMaShuJu.IndexOf(lis[i])>=0)
                    {
                        this.QiDongTiShiKuang($"码{i+1}与码{SaoMaShuJu.IndexOf(lis[i])+1}相同");
                        return;
                    }
                }
                SaoMaShuJu.Add(lis[i]);
            }
            if (iskong==false)
            {
                this.QiDongTiShiKuang("没有输入数据");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void MaFrm_Activated(object sender, EventArgs e)
        {
            SetJiaoDian(0);
        }
    }
}
