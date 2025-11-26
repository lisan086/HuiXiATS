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
using CommLei.JiChuLei;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.UI
{
    public partial class ZhanKJ : UserControl
    {
        Image OK = null;
        Image NG = null;
        private int TDID = -1;
        private ZiYuanModel ZiYuan;
        public ZhanKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.pictureBox1.Image = null;
            this.pictureBox1.Visible = false;
         
        }

        public void SetGW(int tdid,ZiYuanModel ziyuan,string mname,bool isyaotu=true)
        {
            TDID = tdid;
            ZiYuan=ziyuan;
            this.label2.Visible = true;
            this.label2.Text = mname;
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

        public void SetCanShu(EventType arg1, JieMianShiJianModel model)
        {
            if (TDID!= model.GWID)
            {
                return;
            }
            switch (arg1)
            {
                case EventType.JinZhanErWeiMa:
                    {
                        this.pictureBox1.Visible = false;
                        this.label1.Text = ChangYong.TryStr(model.Value, "");
                        this.dataGridView1.Rows.Clear();
                    }
                    break;
                case EventType.JinZhan:
                    {
                        if (model.JieGuo)
                        {
                            this.label4.Text = ChangYong.TryStr(model.Value, "");
                        }
                        else
                        {

                            FuZhiTuPian(false);
                            this.label4.Text = ChangYong.TryStr(model.Value, "");

                        }
                    }
                    break;
                case EventType.ChuZhan:
                    {
                        if (model.JieGuo)
                        {
                            this.label4.Text = ChangYong.TryStr(model.Value, "");
                        }
                        else
                        {

                           // FuZhiTuPian(false);
                            this.label4.Text = ChangYong.TryStr(model.Value, "");
                        }
                    }
                    break;
                case EventType.ChuZhanErWeiMa:
                    {
                        this.pictureBox1.Visible = false;
                        this.label1.Text = ChangYong.TryStr(model.Value, "");
                        this.dataGridView1.Rows.Clear();
                    }
                    break;
                case EventType.ChuZhanData:
                    {
                        if (model.Value is YeWuDataModel)
                        {
                            YeWuDataModel modes = model.Value as YeWuDataModel;
                            int index = this.dataGridView1.Rows.Add();
                            this.dataGridView1.Rows[index].Cells[0].Value = modes.ItemName;
                            this.dataGridView1.Rows[index].Cells[1].Value = modes.Low.JiCunValue;
                            this.dataGridView1.Rows[index].Cells[2].Value = modes.Up.JiCunValue;
                            this.dataGridView1.Rows[index].Cells[3].Value = modes.Value.JiCunValue;
                            this.dataGridView1.Rows[index].Cells[4].Value = modes.IsShuJuHeGe ? "√" : "×";
                            this.dataGridView1.Rows[index].Cells[5].Value = modes.IsShangChuanHeGe ? "√" : "×";
                            this.dataGridView1.Rows[index].Height = 32;
                            if (modes.IsShuJuHeGe == false || modes.IsShangChuanHeGe == false)
                            {
                                this.dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                            }
                            else
                            {
                                this.dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Green;
                            }
                        }
                    }
                    break;
                case EventType.TestZongJieGuo:
                    {
                        FuZhiTuPian(model.JieGuo);
                    }
                    break;
                default:
                    break;
            }
        }
        public void SetTanChuan(int tdid,string msg)
        {
            if (tdid == TDID)
            {
                ZiYuan.TiShiKuang(msg);
            }
        }
      
        public void SetLog(int tdid,string msg,bool ishong)
        {
            if (TDID == tdid)
            {
                this.ucJiLvContor1.LogAppend(ishong?Color.Red:Color.Black, msg);
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
