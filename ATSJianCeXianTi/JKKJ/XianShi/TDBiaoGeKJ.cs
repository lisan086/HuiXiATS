using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using ATSJianCeXianTi.PeiFangFrm;
using ATSJianMianJK;
using CommLei.JiChuLei;

namespace ATSJianCeXianTi.JKKJ.XianShi
{
    public partial class TDBiaoGeKJ : UserControl
    {
        /// <summary>
        /// 通道ID
        /// </summary>
        private int TdID = -1;
        private string PeiFangName = "";

        private TDModel TDModel;
        private ZiYuanModel ZiYuanModel;

        private LaoHuaType LaoHuaType = LaoHuaType.WuLaoHua;


        private List<string> CeXuLie { get; set; } = new List<string>();
        private List<string> XuanZeXuLie { get; set; } = new List<string>();

        private Dictionary<int, DataGridViewRow> BaoCunRow = new Dictionary<int, DataGridViewRow>();
     
    
        /// <summary>
        /// 1 表示在工作
        /// </summary>
        private TDStateType State = TDStateType.ZhengChangKongXian;
     
     
        public TDBiaoGeKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.commBoxE2.Items.Clear();
            List<string> lis = ChangYong.MeiJuLisName(typeof(LaoHuaType));
            for (int i = 0; i < lis.Count; i++)
            {
                this.commBoxE2.Items.Add(lis[i]);
            }
            if (this.commBoxE2.Items.Count > 0)
            {
                this.commBoxE2.Text = LaoHuaType.WuLaoHua.ToString();
            }
          
            this.commBoxE2.SelectedIndexChanged += CommBoxE2_SelectedIndexChanged;        
            this.outLockBar1.SetXianShi(0);
        }

        public void SetCanShu(TDModel dModel)
        {
            TdID = dModel.TDID;
            TDModel = dModel;
            PeiFangName = dModel.PeiZhiCanShu.PeiFangName;
            this.tdLanKJ1.IniData(dModel.TDID, dModel.TDName);
        }

        public void SetCanShu(ZiYuanModel ziyuan)
        {
            ZiYuanModel = ziyuan;
            this.tdLanKJ1.SetQuanXian(TdID,ziyuan.QuanXian);
            QiHuan();
        }

        public void SetJiaoDian()
        {
            this.Focus();
        }

        /// <summary>
        /// 设置开关参数
        /// </summary>
        /// <param name="dModel"></param>
        /// <param name="tdid"></param>
        public void ShuaXin()
        {
            State = TDModel.GanYingModel.TDState;
          
            if (TDModel.GanYingModel.PeiFangGengGai)
            {
                TDModel.GanYingModel.PeiFangGengGai = false;
                SetPeiFang(Lei.JieMianLei.Cerate().GetPeiFang(TdID));
            }
            if (LaoHuaType != LaoHuaType.WuLaoHua)
            {
                if (LaoHuaType == LaoHuaType.CiShuLaoHua)
                {
                    this.tdLanKJ1.ZhongBuMiaoSu = $"老化中:{LaoHuaType},需数:{TDModel.ShouDongCanShu.LaoHuaCiShu} PASS数:{TDModel.ShouDongCanShu.LaoHuaHeGeCiShu} NG数:{TDModel.ShouDongCanShu.LaoHuaBuHeGeCiShu}";
                }
                else if(LaoHuaType == LaoHuaType.HeGeLaoHua)
                {
                    this.tdLanKJ1.ZhongBuMiaoSu = $"老化中:{LaoHuaType},需数:{TDModel.ShouDongCanShu.LaoHuaCiShu} 合格次数:{TDModel.ShouDongCanShu.LaoHuaHeGeCiShu}";
                }
            }
            if (State == TDStateType.TiaoShiGongZuo || State == TDStateType.TiaoShiKongXian)
            {
                this.tdLanKJ1.DiBuMiaoSu = "调试中";
            }
            else
            {
                this.tdLanKJ1.DiBuMiaoSu = "正常测试";
            }
            if (TDModel.GanYingModel.IsDianJian)
            {
                if (this.button8.BackColor != Color.Orange)
                {
                    this.button8.BackColor = Color.Orange;
                }
            }
            else
            {
                if (this.button8.BackColor != Color.White)
                {
                    this.button8.BackColor = Color.White;
                }
            }
            this.tdLanKJ1.PeiFangName = TDModel.PeiZhiCanShu.PeiFangName;
            this.tdLanKJ1.ShuaXin();
        }
        public void JiaZaiPeiFang()
        {
            JieMianModel model = new JieMianModel();
            model.TDID = TdID;
            model.CaoZuo = true;
            model.PeiFangName = PeiFangName;
            ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.JiaZaiPeiFang, model);
        }
        /// <summary>
        /// 设置测试项目
        /// </summary>
        /// <param name="tdid"></param>
        /// <param name="model"></param>
        public void SetTestXiangMu(int tdid, XiangMuModel model, string haomiao)
        {

            if (TdID == tdid)
            {

                switch (model.BuZhouType)
                {
                    case BuZhouType.ZhunBeiJianCe:
                        {
                            ClearXiangMu();
                        }
                        break;
                    case BuZhouType.KaiShiJianCe:
                        {
                            ClearXiangMu();
                        }
                        break;
                    case BuZhouType.DXiangMuJianCe:
                        {
                            if (model.IsZongXiang)
                            {
                                if (BaoCunRow.ContainsKey(model.TestModel.WeiZhi))
                                {
                                    FuZhi(model.TestModel, BaoCunRow[model.TestModel.WeiZhi], true);
                                    GaiBianSe(BaoCunRow[model.TestModel.WeiZhi], model.TestModel.IsHeGe);
                                    BaoCunRow[model.TestModel.WeiZhi].Selected = true;
                                    if (BaoCunRow[model.TestModel.WeiZhi].Visible)
                                    {
                                        this.jiHeDataGrid1.CurrentCell = BaoCunRow[model.TestModel.WeiZhi].Cells[1];
                                    }
                                }
                            }


                        }
                        break;
                    case BuZhouType.DXiangMuJieSu:
                        {
                            if (model.IsZongXiang)
                            {
                                if (BaoCunRow.ContainsKey(model.TestModel.WeiZhi))
                                {
                                    FuZhi(model.TestModel, BaoCunRow[model.TestModel.WeiZhi], true);
                                    GaiBianSe(BaoCunRow[model.TestModel.WeiZhi], model.TestModel.IsHeGe);
                                }
                            }

                        }
                        break;
                    case BuZhouType.ZongJieSu:
                        {


                        }
                        break;

                    default:
                        break;
                }

            }
        }




        private void CommBoxE2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LaoHuaType = this.commBoxE2.GetCanShu<LaoHuaType>();
            int cishu = ChangYong.TryInt(this.textBox1.Text, 1);
            if (cishu <= 0)
            {
                cishu = 1;
            }
            if (LaoHuaType != LaoHuaType.WuLaoHua)
            {
                this.tdLanKJ1.ZhongBuMiaoSu = $"老化中:{LaoHuaType}";
            }
            else
            {
                this.tdLanKJ1.ZhongBuMiaoSu = $"";
            }
            ChuFaAnNiu(CaoZuoAnNiu.LaoHuaTest, false, null, false, $"{LaoHuaType},{cishu}");
        }
        private void ChuFaAnNiu(CaoZuoAnNiu caoZuo, bool iskai, Control kj, bool isshangse, string xulie = "")
        {
            bool jieguo = AnNiuClick(caoZuo, iskai, xulie);
            if (isshangse)
            {
                if (jieguo)
                {
                    if (kj.BackColor == Color.White)
                    {
                        kj.BackColor = Color.Orange;
                    }
                    else
                    {
                        kj.BackColor = Color.White;
                    }
                }
                else
                {
                    kj.BackColor = Color.White;
                }
                if (caoZuo == CaoZuoAnNiu.TiaoShi)
                {
                    if (jieguo)
                    {
                        if (kj.BackColor != Color.White)
                        {
                            this.button2.Visible = true;
                            this.button4.Visible = true;
                            this.button5.Visible = true;                           
                        }
                        else
                        {
                            this.button2.Visible = false;
                            this.button4.Visible = false;
                            this.button5.Visible = false;
                            this.button2.BackColor = Color.Orange;
                         
                        }
                    }

                }

            }
        }

        private bool AnNiuClick(CaoZuoAnNiu caoZuo, bool arg2, string xulie)
        {

            switch (caoZuo)
            {
                case CaoZuoAnNiu.XuanZePeiFang:
                    {
                        PeiFangXuanZeFrm peiFangXuanZeFrm = new PeiFangXuanZeFrm();
                        peiFangXuanZeFrm.IsJingYongZuiXiao = false;
                        peiFangXuanZeFrm.SetCanShu("");
                        if (peiFangXuanZeFrm.ShowDialog(this) == DialogResult.OK)
                        {
                            PeiFangName = peiFangXuanZeFrm.PeiFangNames;
                            JiaZaiPeiFang();
                        }
                    }
                    break;
                case CaoZuoAnNiu.TiaoShi:
                    {
                        if (arg2)
                        {
                            if (State != TDStateType.ZhengChengGongZuo)
                            {
                                JieMianModel model = new JieMianModel();
                                model.TDID = TdID;
                                model.CaoZuo = arg2;
                                ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.CaoZuoTiaoShi, model);
                                if (arg2)
                                {
                                    int count = this.jiHeDataGrid1.Columns.Count;
                                    if (count >= 3)
                                    {
                                        this.jiHeDataGrid1.Columns[count - 2].Visible = true;
                                        this.jiHeDataGrid1.Columns[count - 3].Visible = true;
                                    }
                                }
                                return true;
                            }
                            else
                            {
                                ZiYuanModel.TiShiKuang("目前处于工作状态,不能调试");
                                return false;
                            }
                        }
                        else
                        {
                            if (State != TDStateType.TiaoShiGongZuo)
                            {
                                int count = this.jiHeDataGrid1.Columns.Count;
                                if (count >= 3)
                                {
                                    this.jiHeDataGrid1.Columns[count - 2].Visible = false;
                                    this.jiHeDataGrid1.Columns[count - 3].Visible = false;
                                }
                                XuanZeXuLie.Clear();
                                this.tdLanKJ1.ZhongBuMiaoSu = "";
                                JieMianModel models = new JieMianModel();
                                models.TDID = TdID;
                                models.CaoZuo = arg2;
                                ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.CaoZuoTiaoShi, models);

                                JieMianModel modelss = new JieMianModel();
                                modelss.TDID = TdID;
                                modelss.CaoZuo = true;
                                ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.CaoZuoNGTiaoChu, modelss);
                                return true;
                            }
                            else
                            {
                                ZiYuanModel.TiShiKuang("目前处于调试工作状态,不能进去正常模式");
                                return false;
                            }
                        }
                    }

                case CaoZuoAnNiu.ZanTing:
                    {
                        JieMianModel model = new JieMianModel();
                        model.TDID = TdID;
                        model.CaoZuo = arg2;
                        ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.CaoZuoZanTing, model);
                        return true;
                    }

                case CaoZuoAnNiu.NGTiaoChu:
                    {
                        JieMianModel models = new JieMianModel();
                        models.TDID = TdID;
                        models.CaoZuo = arg2;
                        ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.CaoZuoNGTiaoChu, models);
                        return true;
                    }

                case CaoZuoAnNiu.MES:
                    {
                        JieMianModel models = new JieMianModel();
                        models.TDID = TdID;
                        models.CaoZuo = arg2;
                        ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.CaoZuoMes, models);
                        return true;
                    }

                case CaoZuoAnNiu.ShouDongTest:
                    {

                        JieMianModel model = new JieMianModel();
                        model.TDID = TdID;
                        ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.ShouDongTest, model);
                        return true;
                    }

                case CaoZuoAnNiu.TuiChuTest:
                    {
                        bool istuichusuoyou = ZiYuanModel.ShiYuFou("是否退出所有");
                        if (istuichusuoyou)
                        {
                            JieMianModel model = new JieMianModel();
                            model.TDID = TdID;

                            ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.CaoZuoZongTiaoChu, model);
                        }
                        else
                        {
                            JieMianModel model = new JieMianModel();
                            model.TDID = TdID;

                            ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.DanBuTiaoChu, model);
                        }
                        return true;
                    }
                case CaoZuoAnNiu.JiGeCeShi:
                    {
                        JieMianModel model = new JieMianModel();
                        model.TDID = TdID;
                        model.PeiFangName = xulie;

                        ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.JiGeXuLie, model);
                        return true;
                    }
                case CaoZuoAnNiu.LaoHuaTest:
                    {
                        JieMianModel model = new JieMianModel();
                        model.TDID = TdID;
                        model.ZuoWei = ChangYong.TryInt(xulie.Split(',')[1], 1);
                        model.LaoHuaType = ChangYong.GetMeiJuZhi<LaoHuaType>(xulie.Split(',')[0]);
                        ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.LaoHuaTest, model);
                        return true;
                    }
                case CaoZuoAnNiu.DianJian:
                    {
                        JieMianModel model = new JieMianModel();
                        model.TDID = TdID;
                        model.CaoZuo = arg2;
                        ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.DianJianMoShi, model);
                        return true;
                    }
                default:
                    break;
            }
            return false;
        }

       

        private void QiHuan()
        {
            this.button9.Visible = false;                 
            this.button2.Visible = false;         
            this.button4.Visible = false;        
            this.button5.Visible = false;
        
            string msg = "";
          
            if (ZiYuanModel.QuanXian.IsYouQuanXian("调试功能", out msg))
            {
                this.button9.Visible = true;
            }
        }

     
     



        /// <summary>
        /// 设置配方
        /// </summary>
        /// <param name="tdid"></param>
        /// <param name="model"></param>
        private void SetPeiFang(ZongTestModel model)
        {
            BaoCunRow.Clear();
            CeXuLie.Clear();
            XuanZeXuLie.Clear();
            this.jiHeDataGrid1.Rows.Clear();

            for (int i = 0; i < model.ZhongJianModels.Count; i++)
            {
                if (model.ZhongJianModels[i].TestModel.IsTest == false)
                {
                    continue;
                }
                int index = this.jiHeDataGrid1.Rows.Add();
                CeXuLie.Add(string.Format($"{model.ZhongJianModels[i].TestModel.XuHaoID}:{model.ZhongJianModels[i].TestModel.ItemName}"));

                FuZhiData(model.ZhongJianModels[i], this.jiHeDataGrid1.Rows[index]);
            }

            PeiFangName = model.Name;


        }

      

        private void ClearXiangMu()
        {

            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                DataGridViewRow xintre = this.jiHeDataGrid1.Rows[i];
                xintre.Cells[3].Value = "";
                xintre.Cells[5].Value = "";
                xintre.Cells[6].Value = "";
                GaiBianSe(xintre, HeGeType.NoTest);
            }
        }

        private void FuZhiData(ZhongJianModel zongmodel, DataGridViewRow row)
        {
            if (zongmodel.TestModel.IsTest == false)
            {
                return;
            }
            //增加头部
            {
                FuZhi(zongmodel.TestModel, row);
            }

        }
        private void FuZhi(TestModel model, DataGridViewRow row, bool isfuzhi = false)
        {
            if (isfuzhi == false)
            {
                row.Cells[0].Value = $"{model.XuHaoID} {model.ItemName}";
                row.Cells[1].Value = model.LowStr;
                row.Cells[2].Value = model.UpStr;
                row.Cells[3].Value = "";
                row.Cells[4].Value = model.DanWei;
                row.Cells[5].Value = "";
                row.Cells[6].Value = "";
                row.Cells[7].Value = false;
                row.Cells[8].Value = "执行";
                row.Cells[9].Value = model.WeiZhi;
                row.Height = 35;
                BaoCunRow.Add(model.WeiZhi, row);
            }
            else
            {
                if (model.IsHeGe == HeGeType.ZhengZaiTest)
                {
                    row.Cells[3].Value = "";
                    row.Cells[5].Value = "Testing";
                    row.Cells[6].Value = "";
                }
                else
                {
                    row.Cells[3].Value = model.Value;
                    row.Cells[5].Value = model.IsHeGe == HeGeType.Pass ? "PASS" : "NG";
                    row.Cells[6].Value = model.TestTime;
                }
            }
        }

        private void GaiBianSe(DataGridViewRow tree, HeGeType hege)
        {

            if (hege == HeGeType.Pass)
            {
                tree.Cells[5].Style.BackColor = Color.Green;
            }
            else if (hege == HeGeType.NG)
            {
                tree.Cells[5].Style.BackColor = Color.Red;
            }
            else if (hege == HeGeType.ZhengZaiTest)
            {
                tree.Cells[5].Style.BackColor = Color.Yellow;
            }
            else if (hege == HeGeType.NoTest)
            {
                tree.Cells[5].Style.BackColor = Color.White;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                bool zhen = button.BackColor == Color.White;
                ChuFaAnNiu(CaoZuoAnNiu.TiaoShi, zhen, button, true);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                bool zhen = button.BackColor == Color.White;
                ChuFaAnNiu(CaoZuoAnNiu.ZanTing, zhen, button, true);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                bool zhen = true;
                ChuFaAnNiu(CaoZuoAnNiu.ShouDongTest, zhen, button, false);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {    
            List<string> xuanzexulie = new List<string>();
            foreach (var item in CeXuLie)
            {
                string[] fenge = item.Split(':');
                if (XuanZeXuLie.IndexOf(fenge[0]) >= 0)
                {
                    xuanzexulie.Add(item);
                }
            }
            Form1 form1 = new Form1();
            form1.SetCanShu(CeXuLie, xuanzexulie);
            if (form1.ShowDialog(this) == DialogResult.OK)
            {
            
                string XuLie = form1.XuLie;
                XuanZeXuLie = ChangYong.JieGeStr(XuLie, ',');
                if (XuanZeXuLie.Count > 0)
                {
                    this.tdLanKJ1.ZhongBuMiaoSu = $"调试几个:{XuLie}";
                }
                else
                {
                    this.tdLanKJ1.ZhongBuMiaoSu = "";
                }
                ChuFaAnNiu(CaoZuoAnNiu.JiGeCeShi, false, (sender as Button), false, XuLie);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                bool zhen = true;
                ChuFaAnNiu(CaoZuoAnNiu.TuiChuTest, zhen, button, false);
            }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                bool zhen = button.BackColor == Color.White;
                ChuFaAnNiu(CaoZuoAnNiu.NGTiaoChu, zhen, button, true);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                bool zhen = button.BackColor == Color.White;
                ChuFaAnNiu(CaoZuoAnNiu.MES, zhen, button, true);
            }
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 8)
                {
                    string gongnengma = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[9].Value.ToString();

                    int zuowei = ChangYong.TryInt(gongnengma, -1);
                    if (zuowei >= 0)
                    {
                        JieMianModel model = new JieMianModel();
                        model.TDID = TdID;
                        model.ZuoWei = zuowei;

                        Lei.JieMianLei.Cerate().CaoZuo(DoType.DanBuZhiXing, model);



                    }
                }
                if (e.ColumnIndex == 7)
                {
                    if (this.jiHeDataGrid1.Rows[e.RowIndex].Cells[9].Value != null && this.jiHeDataGrid1.Rows[e.RowIndex].Cells[7].Value != null)
                    {
                        string gongnengma = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[9].Value.ToString();

                        int zuowei = ChangYong.TryInt(gongnengma, -1);
                        if (zuowei >= 0)
                        {
                            bool iskai = false;
                            if (this.jiHeDataGrid1.Rows[e.RowIndex].Cells[7].Value.ToString().Contains("u") == false)
                            {
                                this.jiHeDataGrid1.Rows[e.RowIndex].Cells[7].Value = true;
                                iskai = true;
                                this.jiHeDataGrid1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                            }
                            else
                            {
                                this.jiHeDataGrid1.Rows[e.RowIndex].Cells[7].Value = false;
                                this.jiHeDataGrid1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                            }

                            JieMianModel model = new JieMianModel();
                            model.TDID = TdID;
                            model.CaoZuo = iskai;
                            model.ZuoWei = zuowei;
                            ATSJianCeXianTi.Lei.JieMianLei.Cerate().CaoZuo(DoType.DuanDian, model);
                        }
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClearXiangMu();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                bool zhen = button.BackColor == Color.White;
                ChuFaAnNiu(CaoZuoAnNiu.DianJian, zhen, button, false);
            }
        }
    }
    public enum CaoZuoAnNiu
    {
        XuanZePeiFang,
        TiaoShi,
        ZanTing,
        NGTiaoChu,
        MES,
        ShouDongTest,
        TuiChuTest,
        JiGeCeShi,
        LaoHuaTest,
        HuanBan,
        DianJian,
    }
}
