using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK;
using ATSJianMianJK.Log;
using ATSJuanChengZuZhuangUI.Model;
using BaseUI.DaYIngMoBan.Lei;
using BaseUI.UC;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.UI;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.UI
{
    public partial class ShouJieMian : UserControl
    {
        private int TDID = -1;
        private ZiYuanModel ZiYuan;
        Image OK = null;
        Image NG = null;
        public ShouJieMian()
        {
            InitializeComponent();
        }
        public void SetGW(int tdid, ZiYuanModel ziyuan,string namen, bool isyaotu =true)
        {
            TDID = tdid;
            ZiYuan = ziyuan;
            this.label2.Visible = true;
            this.label2.Text = namen;
            if (isyaotu)
            {
                try
                {
                    string wenjian = Application.StartupPath + @"\TuPian\OK.jpg";
                    if (File.Exists(wenjian))
                    {
                        Image sok = Image.FromFile(wenjian);
                        OK = (Image)sok.Clone();
                        sok.Dispose();
                    }
                }
                catch
                {


                }
                try
                {
                    string wenjian = Application.StartupPath + @"\TuPian\NG.jpg";
                    if (File.Exists(wenjian))
                    {
                        Image sok = Image.FromFile(wenjian);
                        NG = (Image)sok.Clone();
                        sok.Dispose();
                    }
                }
                catch
                {
                }
            }
            List<ShiYongMaModel> kis = HCLisDataLei<ShiYongMaModel>.Ceratei().LisWuLiao;
            List<ShiYongMaModel> shiyiong = new List<ShiYongMaModel>();
         
            if (kis.Count >= 1)
            {
                for (int i = kis.Count - 40; i < kis.Count; i++)
                {
                    if (i>=0)
                    {
                        shiyiong.Add(kis[i]);
                        int index = this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index].Cells[0].Value = kis[i].Ma;
                        this.dataGridView1.Rows[index].Cells[1].Value = kis[i].IsShiYong == 1 ? "已经使用" : "未使用";
                        this.dataGridView1.Rows[index].Cells[2].Value = "补打";
                        this.dataGridView1.Rows[index].Height = 32;
                    }
                }
              
            }
           
            HCLisDataLei<ShiYongMaModel>.Ceratei().LisWuLiao = shiyiong;
            HCLisDataLei<ShiYongMaModel>.Ceratei().BaoCun();
        }
        public void SetGW(ZiYuanModel ziyuan)
        {
            ZiYuan = ziyuan;

        }

        public void SetCanShu(EventType arg1, JieMianShiJianModel model)
        {
            if (TDID != model.GWID)
            {
                return;
            }
            switch (arg1)
            {
                case EventType.JinZhanErWeiMa:
                    {
                        this.pictureBox1.Visible = false;
                        this.label4.Text = ChangYong.TryStr(model.Value, "");

                        this.textBox1.Text = "";
                        this.textBox1.Focus();
                        this.textBox1.SelectAll();
                    }
                    break;
                case EventType.ShouZhanChuMa:
                    {
                        if (model.Value is List<string>)
                        {
                            List<ShiYongMaModel> lis2 = new List<ShiYongMaModel>();
                            List<string> lis = model.Value as List<string>;
                            for (int i = 0; i < lis.Count; i++)
                            {
                                int index = this.dataGridView1.Rows.Add();
                                this.dataGridView1.Rows[index].Cells[0].Value = lis[i];
                                this.dataGridView1.Rows[index].Cells[1].Value = "未使用";
                                this.dataGridView1.Rows[index].Cells[2].Value ="补打";                              
                                this.dataGridView1.Rows[index].Height = 32;
                                ShiYongMaModel modedl = new ShiYongMaModel();
                                modedl.IsShiYong = 0;
                                modedl.Ma = lis[i];
                                lis2.Add(modedl);
                            }

                            HCLisDataLei<ShiYongMaModel>.Ceratei().LisWuLiao.AddRange(lis2.ToArray());
                            HCLisDataLei<ShiYongMaModel>.Ceratei().BaoCun();
                        }
                    }
                    break;

                case EventType.TestZongJieGuo:
                    {
                        FuZhiTuPian(model.JieGuo);
                        string ma = ChangYong.TryStr(model.Value,"");
                        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                        {
                            if (this.dataGridView1.Rows[i].Cells[0].Value.ToString().Equals(ma))
                            {
                                this.dataGridView1.Rows[i].Cells[1].Value = "已使用";
                                break;
                            }
                        }
                        List<ShiYongMaModel> kis = HCLisDataLei<ShiYongMaModel>.Ceratei().LisWuLiao;
                        for (int i = 0; i < kis.Count; i++)
                        {
                            if (kis[i].Ma.Equals(ma))
                            {
                                kis[i].IsShiYong=1;
                                kis[i].ShiYongTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                break;
                            }
                        }
                        HCLisDataLei<ShiYongMaModel>.Ceratei().BaoCun();
                    }
                    break;
                default:
                    break;
            }
        }
        public void SetTanChuan(int tdid, string msg)
        {
            if (tdid == TDID)
            {
                ZiYuan.TiShiKuang(msg);
            }
        }

        public void SetLog(int tdid, string msg, bool ishong)
        {
            if (TDID == tdid)
            {
                this.ucJiLvContor1.LogAppend(ishong ? Color.Red : Color.Black, msg);
            }
        }




        private void FuZhiTuPian(bool jinzhanstate)
        {
            this.pictureBox1.BringToFront();
            if (jinzhanstate)
            {

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = OK == null ? null : (Image)OK.Clone();
            }
            else
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = NG == null ? null : (Image)NG.Clone();
            }

            this.pictureBox1.Visible = true;
        }

    

        private void button3_Click(object sender, EventArgs e)
        {
            string mesma = this.textBox1.Text.Trim();
            if (string.IsNullOrEmpty(mesma) == false)
            {
                bool iscunzai = MaGuanLi.CerateDanLi().JiaoYanShiFouGaiMa(mesma);
                if (iscunzai == false)
                {
                    this.QiDongTiShiKuang("没有找到该单号");
                    
                }
                else
                {
                    JieMianCaoZuoModel model = new JieMianCaoZuoModel();
                    model.CanShu = mesma;
                    model.GWID = TDID;
                    JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.SaoMaQueRen, model);
                    this.QiDongTiShiKuang("数据已发送");
                }
            }
            else
            {
                QiDongTiShiKuang("确认的过站码为空，请扫码");
            }

            this.textBox1.Text = "";
            this.textBox1.Focus();
            this.textBox1.SelectAll();
        }
        protected void QiDongTiShiKuang(string msg, int shijian = 5)
        {
            MsgBoxFrom msgBoxFrom = new MsgBoxFrom();
            msgBoxFrom.AddMsg(msg);
            msgBoxFrom.SetCanShu(IsQiDongZiDongGuanBi: true, "确定", "", shijian);
            msgBoxFrom.TopMost = true;
            msgBoxFrom.BringToFront();
            msgBoxFrom.ShowDialog();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3_Click(null, null);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 2)
                    {
                        string msg = "";
                        bool zhen = ZiYuan.QuanXian.IsYouQuanXian("补打", out msg);
                        if (zhen)
                        {
                            string lujing = MaGuanLi.CerateDanLi().GetMaWenJianPath();
                            ShuChuMaShuJuModel models = new ShuChuMaShuJuModel();
                            models.Ma1 = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                            DaYingLeiXin<ShuChuMaShuJuModel, ShuChuMaShuJuModel> daYingLeiXin = new DaYingLeiXin<ShuChuMaShuJuModel, ShuChuMaShuJuModel>(lujing);

                            daYingLeiXin.DaYingYuLan(models, new List<ShuChuMaShuJuModel>(), 1, "", true);
                        }
                        else
                        {
                            QiDongTiShiKuang(msg);
                        }
                      
                    }
                }
            }
        }
    }
}
