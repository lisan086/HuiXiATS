using ATSJianMianJK.QuanXian;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianMianJK.Frm
{
    public partial class QuanXianFrm : BaseFuFrom
    {
        private List<QuanXianModel> QuanXianModels = new List<QuanXianModel>();
        public QuanXianFrm()
        {
            InitializeComponent();
        }
        public  void SetCanShu(List<QuanXianModel> lismodel)
        {
            QuanXianModels = ChangYong.FuZhiShiTi(lismodel);
            List<RenWuModel> lis = HCLisDataLei<RenWuModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lis.Count; i++)
            {
                FuZhi(lis[i],-1);
            }
            foreach (var item in QuanXianModels)
            {
                this.listBox1.Items.Add(item.GongNengDan);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.label4.Text = "";
            RenWuModel model = new RenWuModel();
            model.Login = this.textBox1.Text.Trim();
            model.MiMa= this.textBox2.Text.Trim();
            model.XinMing= this.textBox3.Text.Trim();
            if (string.IsNullOrEmpty(model.Login))
            {
                this.QiDongTiShiKuang("登录名不能为空");
                return;
            }
            if (string.IsNullOrEmpty(model.XinMing))
            {
                this.QiDongTiShiKuang("名称不能为空");
                return;
            }
            List<RenWuModel> lis = HCLisDataLei<RenWuModel>.Ceratei().LisWuLiao;
            bool zhen = false;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].Login==model.Login)
                {
                    zhen = true;
                    bool isyaoxiugai = this.ShiOrFou($"是否要修改该登录名的密码：{model.Login}");
                    if (isyaoxiugai)
                    {
                        lis[i].MiMa = model.MiMa;
                        lis[i].XinMing = model.XinMing;
                        HCLisDataLei<RenWuModel>.Ceratei().BaoCun();
                        FuZhi(model,i);
                    }
                }
            }
            if (zhen==false)
            {
                HCLisDataLei<RenWuModel>.Ceratei().LisWuLiao.Add(ChangYong.FuZhiShiTi(model));
                HCLisDataLei<RenWuModel>.Ceratei().BaoCun();
                FuZhi(model,-1);
                this.QiDongTiShiKuang("保存成功");
            }
        }

        private void FuZhi(RenWuModel model,int index)
        {
            if (index < 0)
            {
                index = this.jiHeDataGrid1.Rows.Add();

            }
            else
            {
                for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
                {
                    if (this.jiHeDataGrid1.Rows[i].Tag is RenWuModel)
                    {
                        RenWuModel modsel = this.jiHeDataGrid1.Rows[i].Tag as RenWuModel;
                        if (modsel.Login== model.Login)
                        {
                            index = i;
                            break;
                        }
                    }
                }
            }
            this.jiHeDataGrid1.Rows[index].Cells[0].Value = model.Login;
            this.jiHeDataGrid1.Rows[index].Cells[1].Value = model.MiMa;
            this.jiHeDataGrid1.Rows[index].Cells[2].Value = model.XinMing;
            this.jiHeDataGrid1.Rows[index].Cells[3].Value = "删除";
            this.jiHeDataGrid1.Rows[index].Tag = model;
            this.jiHeDataGrid1.Rows[index].Height = 32;
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.ColumnIndex==3)
                {
                    if (this.jiHeDataGrid1.Rows[e.RowIndex].Tag is RenWuModel)
                    {
                        RenWuModel modsel = this.jiHeDataGrid1.Rows[e.RowIndex].Tag as RenWuModel;
                        List<RenWuModel> lis = HCLisDataLei<RenWuModel>.Ceratei().LisWuLiao;
                        for (int i = 0; i < lis.Count; i++)
                        {
                            if (lis[i].Login== modsel.Login)
                            {
                                lis.RemoveAt(i);
                                break;
                            }
                        }
                        HCLisDataLei<RenWuModel>.Ceratei().BaoCun();
                        this.jiHeDataGrid1.Rows.RemoveAt(e.RowIndex);
                        this.QiDongTiShiKuang("删除成功");
                    }
                }
            }
        }

        private void jiHeDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    if (this.jiHeDataGrid1.Rows[e.RowIndex].Tag is RenWuModel)
                    {
                        this.listBox2.Items.Clear();
                        RenWuModel modsel = this.jiHeDataGrid1.Rows[e.RowIndex].Tag as RenWuModel;
                        foreach (var item in modsel.QuanXianS)
                        {
                            if (item.IsYou)
                            {
                                this.listBox2.Items.Add(item.GongNengDan);
                            }
                        }
                        this.label4.Text = modsel.Login;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.label4.Text=="")
            {
                this.QiDongTiShiKuang("没有选中登录名");
                return;
            }
            if (this.listBox1.Items.Count>0)
            {
                if (this.listBox1.SelectedItems.Count>0)
                {
                    for (int i = 0; i < this.listBox1.SelectedItems.Count; i++)
                    {
                        if (IsHanYou(this.listBox1.SelectedItems[i].ToString())==false)
                        {
                            this.listBox2.Items.Add(this.listBox1.SelectedItems[i].ToString());
                        }
                    }
                    List<RenWuModel> lis = HCLisDataLei<RenWuModel>.Ceratei().LisWuLiao;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].Login==this.label4.Text)
                        {
                            lis[i].QuanXianS = GetCaiDan();
                        }
                    }
                    HCLisDataLei<RenWuModel>.Ceratei().BaoCun();
                }
            }
        }
        private List<QuanXianModel> GetCaiDan()
        {
            List<QuanXianModel> lismodel = new List<QuanXianModel>();
            for (int i = 0; i < this.listBox1.Items.Count; i++)
            {
                QuanXianModel model = new QuanXianModel();
                model.GongNengDan = this.listBox1.Items[i].ToString();
                model.IsYou = IsHanYou(model.GongNengDan);
                lismodel.Add(model);
            }
            return lismodel;
        }
        private bool IsHanYou(string gongenngdan)
        {
            for (int i = 0; i < this.listBox2.Items.Count; i++)
            {
                if (this.listBox2.Items[i].ToString().Equals(gongenngdan))
                {
                    return true;
                }
            }
            return false;
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.label4.Text == "")
            {
                this.QiDongTiShiKuang("没有选中登录名");
                return;
            }
            if (this.listBox2.Items.Count > 0)
            {
                if (this.listBox2.SelectedItem!=null)
                {
                    this.listBox2.Items.Remove(this.listBox2.SelectedItem);
                    List<RenWuModel> lis = HCLisDataLei<RenWuModel>.Ceratei().LisWuLiao;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].Login == this.label4.Text)
                        {
                            lis[i].QuanXianS = GetCaiDan();
                        }
                    }
                    HCLisDataLei<RenWuModel>.Ceratei().BaoCun();
                }
            }
        }
    }
}
