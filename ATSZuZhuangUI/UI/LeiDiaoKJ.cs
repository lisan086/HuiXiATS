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
using ATSJianMianJK;
using ATSJianMianJK.Log;
using ATSZuZhuangUI.Lei.GongNengLei.KongJian;
using CommLei.JiChuLei;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;

namespace ATSFoZhaoZuZhuangUI.UI
{
    public partial class LeiDiaoKJ : UserControl, IFTongYongKJ
    {
        Image OK = null;
        Image NG = null;
        private int TDID = -1;
        private ZiYuanModel ZiYuan;
        private SheBeiZhanModel SheBeiZhanModel;
        public LeiDiaoKJ()
        {
            InitializeComponent();
        }

        public void SetModel(SheBeiZhanModel zhanModel)
        {
            SheBeiZhanModel = zhanModel;
            TDID = SheBeiZhanModel.GWID;
            this.label2.Visible = true;
            this.label2.Text = zhanModel.LineCode;
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

        public void SetCanShu(EventType arg1, JieMianShiJianModel model)
        {
            if (TDID != model.GWID)
            {
                return;
            }
            switch (arg1)
            {
                case EventType.LeiDiaoJinZhan:
                    {
                        this.pictureBox1.Visible = false;
                        this.label1.Text = ChangYong.TryStr(model.ErWeiMa, "");
                        this.dataGridView1.Rows.Clear();
                        this.label4.Text = model.MiaoSu;
                    }
                    break; 
                case EventType.LeiDiaoShuJu:
                    {
                        YeWuDataModel modes = model.ZhanChuZhanDataModel.YeWuDataModel;
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
                        this.label4.Text = model.MiaoSu;
                    }
                    break;
                case EventType.LeiDiaoChuZhan:
                    {
                        this.label4.Text = model.MiaoSu;
                        FuZhiTuPian(model.JinChuZhanJieGuo);
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

        public void SetLog(int tdtd, RiJiModel riji)
        {
            if (TDID == tdtd)
            {
                this.ucJiLvContor1.LogAppend(riji.IsRed ? Color.Red : Color.Black, riji.Msg);
            }
        }

        public void ShuXin()
        {

        }

        public void SetCanShu(ZiYuanModel ziYuanModel)
        {
            ZiYuan = ziYuanModel;
        }

        public void Close()
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            JieMianCaoZuoModel jiemianshuju = new JieMianCaoZuoModel();
            jiemianshuju.GWID = SheBeiZhanModel.GWID;
            jiemianshuju.CaoZuoState = false;
            JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.ShouDongLeiDiao, jiemianshuju);
        }
    }
}
