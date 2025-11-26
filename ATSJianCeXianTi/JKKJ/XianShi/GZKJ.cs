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
using ATSJianCeXianTi.JKKJ.XianShi;
using ATSJianCeXianTi.Model;
using ATSJianCeXianTi.PeiFangFrm;
using ATSJianMianJK;
using ATSJianMianJK.QuanXian;
using CommLei.JiChuLei;
using JieMianLei.UI;
using static System.Windows.Forms.AxHost;

namespace ATSJianCeXianTi.JKKJ.PeiZhiKJ
{
    public partial class GZKJ : UserControl
    {
        private string PeiFangName = "";
      
        private int TDID = -1;
        private TDModel TDModel;
        private ZiYuanModel quanXianLei1 = null;
        public GZKJ()
        {
            InitializeComponent();
            try
            {
                string wenjian = Application.StartupPath + @"\TuPian\BeiJing.png";
                if (File.Exists(wenjian))
                {
                    this.panel1.BackgroundImage = Image.FromFile(wenjian);
                }
            }
            catch 
            {

               
            }
            this.DoubleBuffered = true;
        }

        public void SetCanShu(TDModel dModel)
        {
            TDID=dModel.TDID;
            TDModel = dModel;
            this.tdLanKJ1.IniData(dModel.TDID, dModel.TDName);
        }

        public void SetCanShu(ZiYuanModel quanXianLei)
        {
            quanXianLei1 = quanXianLei;
            this.label3.Enabled = false;
            if (quanXianLei1.QuanXian.IsYouQuanXian("切换配方", out string msg))
            {
                this.label3.Enabled = true;

            }
        }
        public void SetJiaoDian()
        {
            this.label1.Focus();
        }
      
      
        /// <summary>
        /// 设置测试项目
        /// </summary>
        /// <param name="tdid"></param>
        /// <param name="model"></param>
        public void SetTestXiangMu(int tdid, XiangMuModel model,string haomiao)
        {
            if (TDID == tdid)
            {
                switch (model.BuZhouType)
                {
                    case BuZhouType.ZhunBeiJianCe:
                        {
                            this.label1.Visible = false;
                           
                        }
                        break;
                    case BuZhouType.KaiShiJianCe:
                        {

                            this.label1.Visible = false;
                        }
                        break;
                    case BuZhouType.DXiangMuJianCe:
                        {
                            this.textBox1.Text = model.DiLanMiaoSu;
                        }
                        break;
                    case BuZhouType.DXiangMuJieSu:
                        {
                            this.textBox1.Text = model.DiLanMiaoSu;
                        }
                        break;
                    case BuZhouType.ZongJieSu:
                        {
                            if (model.ZongJieGuo == false)
                            {

                                this.label1.BackColor = Color.Red;
                                this.label1.Text = "NG";
                                this.label1.Visible = true;
                            }
                            else
                            {

                                this.label1.BackColor = Color.Green;
                                this.label1.Text = "OK";
                                this.label1.Visible = true;

                            }
                            this.ucJiLvContor1.LogAppend(model.ZongJieGuo?Color.Green:Color.Red,$"{model.ErWeiMa} {ChangYong.FenGeDaBao(model.BuHeGeXiangMu," ")} 用时:{haomiao}s");
                        }
                        break;

                    default:
                        break;
                }

            }
        }

        public void ShuaXin()
        {
            if (PeiFangName != TDModel.PeiZhiCanShu.PeiFangName)
            {
                this.label3.Text = TDModel.PeiZhiCanShu.PeiFangName;             
                PeiFangName = TDModel.PeiZhiCanShu.PeiFangName;
            }
            if (this.label2.Text != TDModel.FuWeiCanShu.MaZhi)
            {
                this.label2.Text = TDModel.FuWeiCanShu.MaZhi;
            }
            string wupian = string.Format("{0}  {1}", TDModel.GanYingModel.GetTDState(), TDModel.GanYingModel.WuPingZhuangTai ? "有产品" : "无产品");

            if (this.label4.Text != wupian)
            {
                this.label4.Text = wupian;
            }
            this.tdLanKJ1.ShuaXin();
        }
     
        private void label3_Click(object sender, EventArgs e)
        {
            if (this.label3.Enabled)
            {
                PeiFangXuanZeFrm peiFangXuanZeFrm = new PeiFangXuanZeFrm();
                peiFangXuanZeFrm.IsJingYongZuiXiao = false;
                peiFangXuanZeFrm.SetCanShu("");
                if (peiFangXuanZeFrm.ShowDialog(this) == DialogResult.OK)
                {
                  
                    JieMianModel model = new JieMianModel();
                    model.TDID = TDID;
                    model.CaoZuo = true;
                    model.PeiFangName = peiFangXuanZeFrm.PeiFangNames;
                    ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.JiaZaiPeiFang, model);
                }
            }
            else
            {
              
                QiDongTiShiKuang("切换配方的权限不够");
            }
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
      
    }
}
