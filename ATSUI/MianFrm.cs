using ATSJianMianJK;
using ATSJianMianJK.Frm;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.Mes;
using ATSJianMianJK.QuanXian;
using ATSJianMianJK.XiTong.Frm.FM;
using ATSJianMianJK.XiTong.Model;
using ATSJianMianJK.XiTong.XianShiDuFrm.Frm;
using ATSUI.KJ;
using ATSUI.Properties;
using BaseUI.FuFrom.XinWeiHuFrm;
using BaseUI.UC;
using CommLei.DataChuLi;
using Common.DataChuLi;
using JieMianLei.FuFrom;
using SSheBei.Model;
using SSheBei.PeiZhi;
using SSheBei.ZongKongZhi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ATSUI
{
    public partial class MianFrm : BaseFuFrom
    {
     
        private ATSJieMianJK ATSJieMianJK;
        public MianFrm(int zhuangtai)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            if (zhuangtai == 1)
            {
                登录ToolStripMenuItem.Visible = false;
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014)
            {
                return;
            }
            base.WndProc(ref m);
        }
        private void IniData()
        {
            string wenjian = string.Format("{0}{1}", Directory.GetCurrentDirectory(), @"\JieMianJK");
            if (Directory.Exists(wenjian) == false)
            {
                Directory.CreateDirectory(wenjian);
            }
            JieKouJiaZaiLei<ATSJieMianJK> zongxian = new JieKouJiaZaiLei<ATSJieMianJK>();
            List<ATSJieMianJK> lianjieqicom = zongxian.JiaZaiLisT(wenjian);

            if (lianjieqicom.Count > 0)
            {
                ATSJieMianJK = lianjieqicom[0];
                ZiYuanModel model = new ZiYuanModel();
                model.QuanXian = QuanXianLei.CerateDanLi();
                model.It = this.Controlinkove;
                model.TiShiKuang = (x) => { this.QiDongTiShiKuang(x); };
                model.HaoShiRenWu = (x) => { this.Waiting(x, "正在拼命的加载...", this, 10 * 60 * 1000); };
                model.ShiYuFou = (x) => { bool zhen = this.ShiOrFou(x); return zhen; };
                ATSJieMianJK.SetCanShu(model);
                Control control = ATSJieMianJK.LoadKJ();
                control.Dock = DockStyle.Fill;
                if (ATSJieMianJK.IsJieShouSouFang())
                {
                    this.ZengJiaSuoFang(control, true);
                }
                this.ucPanL1.Controls.Add(control);


            }


            SetBiaoTi();
            this.ZuiDaHua();
            RiJiLog.Cerate().RiJiLogEvent += MianFrm_RiJiLogEvent;
            ZongSheBeiKongZhi.Cerate().ZongXianMsgEvnt += MianFrm_ZongXianMsgEvnt;

            this.Waiting(() =>
            {
                Thread.Sleep(50);
                ZongSheBeiKongZhi.Cerate().IniChuShiHua();
                ZongSheBeiKongZhi.Cerate().Open();

                if (JiHeSheBei.Cerate().JiHeData == null)
                {
                    JiHeData shebei = new JiHeData();
                    shebei.FenPeiData();
                    JiHeSheBei.Cerate().JiHeData = shebei;
                }
                if (JiHeSheBei.Cerate().XieBuZhou==null)
                {
                    JiHeSheBei.Cerate().XieBuZhou = new XieBuZhou();
                }
              
                JiHeSheBei.Cerate().Open();
            }, "正在启动中....", this);
            this.timer1.Enabled = true;
        }

        private void MianFrm_ZongXianMsgEvnt(MsgDengJi dengJi, MsgModel e)
        {
            switch (dengJi)
            {
                case MsgDengJi.SheBeiZhengChang:
                    {
                        RiJiLog.Cerate().Add(RiJiEnum.SheBeiZhengChang, e.Msg, e.SheBeiID);
                    }
                    break;
                case MsgDengJi.SheBeiCuoWu:
                    {
                        RiJiLog.Cerate().Add(RiJiEnum.SheBeiCuoWu, e.Msg, e.SheBeiID);
                    }
                    break;
                case MsgDengJi.SheBeiTangChuang:
                    {
                        RiJiLog.Cerate().Add(RiJiEnum.SheBeiTangChuang, e.Msg, e.SheBeiID);
                    }
                    break;
                case MsgDengJi.SheBeiBaoWen:
                    {
                        RiJiLog.Cerate().Add(RiJiEnum.SheBeiBaoWen, e.Msg, e.SheBeiID);
                    }
                    break;
                case MsgDengJi.SheBeiXie:
                    {
                        RiJiLog.Cerate().Add(RiJiEnum.SheBeiXie, e.Msg, e.SheBeiID);
                    }
                    break;
                default:
                    break;
            }
        }

        private void MianFrm_RiJiLogEvent(List<RiJiModel> lismodel, bool istangchuang)
        {
            this.Controlinkove.FanXingGaiBing(() =>
            {
                if (istangchuang)
                {
                    if (lismodel.Count > 0)
                    {
                        this.QiDongTiShiKuang(lismodel[0].Msg);
                        if (ATSJieMianJK != null)
                        {
                            ATSJieMianJK.SetLog(lismodel);
                        }
                    }
                }
                else
                {
                    if (ATSJieMianJK != null)
                    {
                        ATSJieMianJK.SetLog(lismodel);
                    }
                }
            });

        }

        protected override void GuanBi()
        {

            try
            {
                if (ATSJieMianJK != null)
                {
                    ATSJieMianJK.Close();
                }
                RiJiLog.Cerate().Close();
                System.Environment.Exit(0);
            }
            catch
            {
                System.Environment.Exit(0);

            }
        }

        public void ShuaXinState()
        {
            bool istx = true;
            Dictionary<string, List<TxModel>> fenzu = ZongSheBeiKongZhi.Cerate().GetSheBeiTxState();
            foreach (var item in fenzu.Keys)
            {
               
                List<TxModel> kamodelh = fenzu[item];
                for (int j = 0; j < kamodelh.Count; j++)
                {
                    istx = kamodelh[j].ZongTX;
                    if (istx==false)
                    {
                        break;
                    }
                     
                }
                if (istx == false)
                {
                    break;
                }
            }
            this.sheBeiDengKJ1.ColorYanSe = istx ? Color.Green : Color.Red;
        }
        private void SetBiaoTi()
        {
            if (ATSJieMianJK != null)
            {
                this.labFbiaoTi.Text = $"{ATSJieMianJK.GetBiaoTi()},{ATSJieMianJK.GetBanBenHao()}:{QuanXianLei.CerateDanLi().GetDengLuMing()}";
                ATSJieMianJK.QieHuanYongHu();
             
            }
            else
            {
                this.labFbiaoTi.Text = $"   :{QuanXianLei.CerateDanLi().GetDengLuMing()}";
            }
        }

        private void MianFrm_Load(object sender, EventArgs e)
        {
            IniData();
        }



        private void 设备配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("设置设备", out msg);
            if (isyouquanxuan)
            {
                JiChuKJWeiHuFrm<PeiZhiKJ, JiaZaiSheBeiModel> frm = new JiChuKJWeiHuFrm<PeiZhiKJ, JiaZaiSheBeiModel>();
                frm.IsZhiXianShiX = true;
                frm.SetCanShu("设置设备", HCLisDataLei<JiaZaiSheBeiModel>.Ceratei().LisWuLiao);
                frm.ShowDialog(this);
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

        private void 设备调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("设备调试", out msg);
            if (isyouquanxuan)
            {
                SheBeiTiaoShiFrm frm = new SheBeiTiaoShiFrm();
                frm.IsJingYongZuiXiao = false;
                frm.ShowDialog(this);
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

        private void 登录配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ATSJieMianJK == null)
            {
                this.QiDongTiShiKuang("没有相应的界面");
                return;
            }
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("登录配置", out msg);
            if (isyouquanxuan)
            {
                QuanXianFrm frm = new QuanXianFrm();
                List<QuanXianModel> lis = new List<QuanXianModel>();
                lis.AddRange(ATSJieMianJK.GetQuanXian());
                lis.Add(new QuanXianModel() { GongNengDan = "流程配置" });
                lis.Add(new QuanXianModel() { GongNengDan = "流程调试" });
                lis.Add(new QuanXianModel() { GongNengDan = "日志配置" });
                lis.Add(new QuanXianModel() { GongNengDan = "写调试" });
                lis.Add(new QuanXianModel() { GongNengDan = "查看缓存" });
                frm.SetCanShu(lis);
                frm.ShowDialog(this);
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

        private void 切换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YongHuDengLuFrm frn = new YongHuDengLuFrm();
            frn.SetCanShu();
            if (frn.ShowDialog(this) == DialogResult.OK)
            {
                SetBiaoTi();

            }
        }

        private void 日志配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("日志配置", out msg);
            if (isyouquanxuan)
            {
                PeiZhiLogFrm peiZhiLogFrm = new PeiZhiLogFrm();
                if (peiZhiLogFrm.ShowDialog(this) == DialogResult.OK)
                {

                }
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

        private void 流程配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("流程配置", out msg);
            if (isyouquanxuan)
            {
               
                ZongPeiZhiFrm zongPeiZhiFrm = new ZongPeiZhiFrm();
                zongPeiZhiFrm.IsJingYongZuiXiao = false;
                zongPeiZhiFrm.SetCanShu();
                zongPeiZhiFrm.Show(this);
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

     

        private void 读IO显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("流程调试", out msg);
            if (isyouquanxuan)
            {
                ChaKanDuIOFrm duStateFrm = new ChaKanDuIOFrm();
                List<DuIOCanShuModel> shujus = new List<DuIOCanShuModel>();
                foreach (var item in JiHeSheBei.Cerate().JiHeData.TDIOShuJu.Keys)
                {
                    shujus.AddRange(JiHeSheBei.Cerate().JiHeData.TDIOShuJu[item]);
                }
                duStateFrm.IniData(shujus);
                duStateFrm.IsJingYongZuiXiao = false;
                duStateFrm.Show(this);
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

        private void 读参数显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("流程调试", out msg);
            if (isyouquanxuan)
            {
                ChaKanShuJuFrm duStateFrm = new ChaKanShuJuFrm();
                List<DuShuJuModel> shujus = new List<DuShuJuModel>();
                foreach (var item in JiHeSheBei.Cerate().JiHeData.TDZhiShuJu.Keys)
                {
                    shujus.AddRange(JiHeSheBei.Cerate().JiHeData.TDZhiShuJu[item]);
                }
                duStateFrm.IsJingYongZuiXiao = false;
                duStateFrm.IniData(shujus);
                duStateFrm.Show(this);
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

        private void 写调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("写调试", out msg);
            if (isyouquanxuan)
            {
                XieFrm duStateFrm = new XieFrm();
                List<XieSateModel> shujus = new List<XieSateModel>();
                foreach (var item in JiHeSheBei.Cerate().JiHeData.TDXieShuJu.Keys)
                {
                    shujus.AddRange(JiHeSheBei.Cerate().JiHeData.TDXieShuJu[item]);
                }
                duStateFrm.IsJingYongZuiXiao = false;
                duStateFrm.IniData(shujus);
                duStateFrm.Show(this);
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ATSJieMianJK != null)
            {
                ATSJieMianJK.ShuaXin();
            }
            ShuaXinState();

        }

        private void sheBeiDengKJ1_Click(object sender, EventArgs e)
        {
            SheBeiStateFrm tXKJ = new  SheBeiStateFrm();
            tXKJ.SetCanShu();
            tXKJ.Show();
        }

        private void 读缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool isyouquanxuan = QuanXianLei.CerateDanLi().IsYouQuanXian("查看缓存", out msg);
            if (isyouquanxuan)
            {
                ChaKanHuanCunFrm duStateFrm = new ChaKanHuanCunFrm();
                duStateFrm.IsJingYongZuiXiao = false;
                duStateFrm.Show(this);
            }
            else
            {
                this.QiDongTiShiKuang(msg);
            }
        }

        private void mES配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShangChuanMesLei.Cerate().GetPeiFrm();
        }

        
    }
}
