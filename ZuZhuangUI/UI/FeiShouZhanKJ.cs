using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK;
using BaseUI.UC;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.UI
{
    public partial class FeiShouZhanKJ : UserControl
    {
        Image OK = null;
        Image NG = null;
        private int TDID = -1;
        private ZiYuanModel ZiYuan;
        public FeiShouZhanKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.pictureBox1.Image = null;
            this.pictureBox1.Visible = false;
         
        }

        public void SetGW(int tdid,ZiYuanModel ziyuan,bool isyaotu=false)
        {
            TDID = tdid;
            ZiYuan=ziyuan;
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
        }
        public void SetGW(ZiYuanModel ziyuan)
        {
            ZiYuan = ziyuan;
           
        }
        public void SetTanChuan(int tdid,string msg)
        {
            if (tdid == TDID)
            {
                ZiYuan.TiShiKuang(msg);
            }
        }
        public void SetErWeiMa(int tdid, string erweima)
        {
            if (TDID == tdid)
            {
                this.pictureBox1.Visible = false;
                this.label1.Text = erweima;
                this.dataGridView1.Rows.Clear();
            }
        }
        public void SetLog(int tdid,string msg,bool ishong)
        {
            if (TDID == tdid)
            {
                this.ucJiLvContor1.LogAppend(ishong?Color.Red:Color.Black, msg);
            }
        }
        public void SetJinZhan(int tdid,bool jinzhanstate,string miaosu)
        {
            if (TDID == tdid)
            {
                if (jinzhanstate)
                {
                    this.label1.Text = miaosu;
                }
                else
                {
                              
                    FuZhiTuPian(false);
                    this.label1.Text = miaosu;
                  
                }
            }
        }

        public void SetShuJu(int tdid,YeWuDataModel model)
        {
            if (TDID == tdid)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value= model.ItemName;
                this.dataGridView1.Rows[index].Cells[1].Value = model.Low.JiCunValue;
                this.dataGridView1.Rows[index].Cells[2].Value = model.Up.JiCunValue;
                this.dataGridView1.Rows[index].Cells[3].Value = model.Value.JiCunValue;
                this.dataGridView1.Rows[index].Cells[4].Value = model.IsShuJuHeGe? "√" : "×";
                this.dataGridView1.Rows[index].Cells[5].Value = model.IsShangChuanHeGe? "√" : "×";
                this.dataGridView1.Rows[index].Height = 32;
                if (model.IsShuJuHeGe==false|| model.IsShangChuanHeGe==false)
                {
                    this.dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        public void SetChuZhan(int tdid, bool jinzhanstate, string miaosu)
        {
            if (TDID == tdid)
            {
                if (jinzhanstate)
                {
                    this.label1.Text = miaosu;
                }
                else
                {

                    FuZhiTuPian(false);
                    this.label1.Text = miaosu;

                }
            }
        }

        public void SetJieSu(int tdid,bool jieguo)
        {
            if (TDID == tdid)
            {
                FuZhiTuPian(jieguo);
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
    }
}
