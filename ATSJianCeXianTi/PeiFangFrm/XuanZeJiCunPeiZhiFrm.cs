using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using JieMianLei.FuFrom.KJ;
using SSheBei.ABSSheBei;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class XuanZeJiCunPeiZhiFrm : BaseFuFrom
    {
       
        private PeiFangLei PeiFangLei;

        private KJPeiZhiJK KJPeiZhiJK;
        public ZhongJianModel ZhongJianModel { get; set; } = new ZhongJianModel();
       
        public XuanZeJiCunPeiZhiFrm(PeiFangLei peizhilei)
        {
            InitializeComponent();
            this.IsZhiXianShiX = true;
            PeiFangLei = peizhilei;
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            
            base.OnPreviewKeyDown(e);
        }
        public void SetCanShu(ZhongJianModel testmodel,string biaozhi)
        {
            List<string> meijus = ChangYong.MeiJuLisName(typeof(ZhiType));
            for (int i = 0; i < meijus.Count; i++)
            {
                this.comboBox3.Items.Add(meijus[i]);
            }
            ZhongJianModel = ChangYong.FuZhiShiTi(testmodel);
            if (string.IsNullOrEmpty(biaozhi))
            {            
               // this.checkBox1.Visible = false;
            }
                 
            List<string> panduanfangshis=  PeiFangLei.JianCeDui.GetPanDuanFangShi(ZhongJianModel.TestModel.GongNengType);
            for (int i = 0; i < panduanfangshis.Count; i++)
            {
                this.comboBox2.Items.Add(panduanfangshis[i]);
            }
            List<string> leixingas = PeiFangLei.JianCeDui.GetLeiXing(ZhongJianModel.TestModel.GongNengType);
            for (int i = 0; i < leixingas.Count; i++)
            {
                this.comboBox1.Items.Add(leixingas[i]);
            }
            this.comboBox2.SelectedIndex = 0;
            this.comboBox1.SelectedIndex = 0;

            this.button3.Text = $"{ZhongJianModel.TestModel.SheBeiID}:{ZhongJianModel.TestModel.SheBeiName}";
            this.button4.Text = ZhongJianModel.TestModel.CMDSend;
         
            this.textBox2.Text = ZhongJianModel.TestModel.LowStr;
            this.textBox3.Text = ZhongJianModel.TestModel.UpStr;
            this.textBox4.Text = ZhongJianModel.TestModel.DanWei;        
            this.comboBox2.Text = ZhongJianModel.TestModel.BiJiaoType;
            this.textBox6.Text = ZhongJianModel.TestModel.CiShu.ToString();
          
            this.textBox8.Text = ZhongJianModel.TestModel.BaoLiuWeiShu.ToString();
            this.comboBox1.Text = ZhongJianModel.TestModel.LeiXing;
            this.textBox5.Text = GetMiaoSu(ZhongJianModel.TestModel.CMDSend);
            this.textBox9.Text = ZhongJianModel.TestModel.BeiChuShu.ToString();
           
            this.textBox11.Text = ZhongJianModel.TestModel.KB.ToString();
            this.comboBox3.Text = ZhongJianModel.TestModel.IsZhiOK.ToString();
      
            JiaZaiKJ(ZhongJianModel.TestModel.SheBeiID, this.button4.Text, ZhongJianModel.TestModel.GongNengType, ZhongJianModel.TestModel.CMDCanShu);

            for (int i = 0; i < ZhongJianModel.TestModel.HuanCunBiaoShi.Count; i++)
            {
                FuZhi(ZhongJianModel.TestModel.HuanCunBiaoShi[i]);
            }
        }

        private string GetMiaoSu(string weiyibiaoshi)
        {
            try
            {
                string weiyibiaoshiww = this.button3.Text;
                string[] shuds = weiyibiaoshiww.Split(':');
                int shebid = ChangYong.TryInt(shuds[0], -1);
                List<JiCunQiModel> lismodel = PeiFangLei.JianCeDui.GetCMDSend(ZhongJianModel.TestModel.GongNengType, shebid, false);
                for (int i = 0; i < lismodel.Count; i++)
                {
                    if (weiyibiaoshi.Equals(lismodel[i].WeiYiBiaoShi))
                    {
                        return lismodel[i].MiaoSu;
                    }
                }
            }
            catch 
            {

                
            }
       
            return "";
        }

  

        private void button1_Click(object sender, EventArgs e)
        {
            ZhongJianModel.TestModel.CMDSend = this.button4.Text;
            ZhongJianModel.TestModel.CMDCanShu = KJPeiZhiJK.GetCanShu();        
            ZhongJianModel.TestModel.LowStr = this.textBox2.Text;
            ZhongJianModel.TestModel.UpStr = this.textBox3.Text;
            ZhongJianModel.TestModel.DanWei = this.textBox4.Text;
            ZhongJianModel.TestModel.LeiXing = this.comboBox1.Text;
            ZhongJianModel.TestModel.KB = ChangYong.TryFloat(this.textBox11.Text, -1);
            ZhongJianModel.TestModel.BiJiaoType = this.comboBox2.Text;
           
            ZhongJianModel.TestModel.CiShu = ChangYong.TryInt(this.textBox6.Text, 1);
        
            ZhongJianModel.TestModel.BeiChuShu = ChangYong.TryFloat(this.textBox9.Text,1);
            string[] fenge = this.button3.Text.Split(':');
            ZhongJianModel.TestModel.SheBeiID = ChangYong.TryInt(fenge[0], -1);
            if (fenge.Length >= 2)
            {
                ZhongJianModel.TestModel.SheBeiName = fenge[1];
            }         
            ZhongJianModel.TestModel.BaoLiuWeiShu = ChangYong.TryInt(this.textBox8.Text, -1);
            ZhongJianModel.TestModel.IsZhiOK = ChangYong.GetMeiJuZhi<ZhiType>(this.comboBox3.Text);
            ZhongJianModel.TestModel.HuanCunBiaoShi = GetCanShu();
            if (string.IsNullOrEmpty(ZhongJianModel.TestModel.CMDSend))
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

      



        private void Claer(bool isqingli)
        {
            this.textBox5.Text = "";
          //  this.textBox1.Text = "";
            this.textBox2.Text = "--";
            this.textBox3.Text = "--";
            this.textBox4.Text = "";
            this.comboBox2.Text = "";
            this.textBox6.Text = "";
            //this.textBox7.Text = "";
            if (isqingli)
            {
                this.button4.Text = "";
            }
            this.textBox8.Text = "-1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ZiXiangFrm frm = new ZiXiangFrm();
            //frm.SetCanShu(ZhongJianModel.ZiTestModels,PeiFangLei);
            //frm.ShowDialog(this);
            //ZhongJianModel.ZiTestModels = ChangYong.FuZhiShiTi(frm.GetModels());
        }

    

        private void button3_Click(object sender, EventArgs e)
        {
            string yiqian = this.button3.Text;
            List<string> shebeis = PeiFangLei.JianCeDui.GetSheBei(ZhongJianModel.TestModel.GongNengType);
            XuanZseFrm xuanZseFrm = new XuanZseFrm();
            xuanZseFrm.SetCanShu(shebeis,this.button3.Text);
            if (xuanZseFrm.ShowDialog(this)==DialogResult.OK)
            {
                this.button3.Text = xuanZseFrm.JieGuo;
                if (yiqian.Equals(this.button3.Text)==false)
                {
                    Claer(true);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string yiqian = this.button4.Text;
            string canshu = "";
            if (KJPeiZhiJK != null)
            {
                canshu= KJPeiZhiJK.GetCanShu();
            }
            int shebeiid =ChangYong.TryInt( this.button3.Text.Split(':')[0],-1);
            XuanZeJiCunQiFrm xuanZeJiCunQiFrm=new XuanZeJiCunQiFrm();
            xuanZeJiCunQiFrm.SetCanShu( PeiFangLei.JianCeDui.GetCMDSend(ZhongJianModel.TestModel.GongNengType, shebeiid, this.checkBox2.Checked), yiqian, this.checkBox2.Checked);
            if (xuanZeJiCunQiFrm.ShowDialog(this) == DialogResult.OK)
            {
                this.button4.Text = xuanZeJiCunQiFrm.WeiYiBiaoShi;
             
                this.textBox5.Text = xuanZeJiCunQiFrm.MiaoSu;
                JiaZaiKJ(shebeiid,this.button4.Text,ZhongJianModel.TestModel.GongNengType, canshu);


            }
        }

        private void JiaZaiKJ( int shebeiid, string weiyicanshu,string gongneng,string canshu)
        {
            this.panel4.Controls.Clear();
            KJPeiZhiJK = PeiFangLei.JianCeDui.GetJKKJ(gongneng, shebeiid, weiyicanshu);
            Control control= KJPeiZhiJK.GetPeiZhiKJ(weiyicanshu);
            control.Dock = DockStyle.Fill;
            this.panel4.Controls.Add(control);
            KJPeiZhiJK.SetCanShu(canshu);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FuZhi(new CaoZuoHuanCunModel());
        }

        private void FuZhi(CaoZuoHuanCunModel model,int index=-1)
        {
            if (index < 0)
            {
                index = this.jiHeDataGrid1.Rows.Add();
            }
            this.jiHeDataGrid1.Rows[index].Cells[0].Value = model.HuanCunCaoZuoType;
            this.jiHeDataGrid1.Rows[index].Cells[1].Value = model.HuanCunName;
            this.jiHeDataGrid1.Rows[index].Cells[2].Value = model.BiaoHao;
            this.jiHeDataGrid1.Rows[index].Cells[3].Value = "删除";
            this.jiHeDataGrid1.Rows[index].Height = 32;
        }

        private List<CaoZuoHuanCunModel> GetCanShu()
        {
            List<CaoZuoHuanCunModel> caoZuoHuans = new List<CaoZuoHuanCunModel>();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                CaoZuoHuanCunModel model = new CaoZuoHuanCunModel();
                model.BiaoHao = ChangYong.TryInt(this.jiHeDataGrid1.Rows[i].Cells[2].Value, 0);
                model.HuanCunCaoZuoType = ChangYong.GetMeiJuZhi<HuanCunCaoZuoType>(this.jiHeDataGrid1.Rows[i].Cells[0].Value.ToString());
                model.HuanCunName = this.jiHeDataGrid1.Rows[i].Cells[1].Value.ToString();
                caoZuoHuans.Add(model);
            }
            return caoZuoHuans;
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.jiHeDataGrid1.Rows.Count>0)
            {
                if (e.RowIndex>=0)
                {
                    if (e.ColumnIndex <3)
                    {
                        int i=e.RowIndex;
                        CaoZuoHuanCunModel model = new CaoZuoHuanCunModel();
                        model.BiaoHao = ChangYong.TryInt(this.jiHeDataGrid1.Rows[i].Cells[2].Value, 0);
                        model.HuanCunCaoZuoType = ChangYong.GetMeiJuZhi<HuanCunCaoZuoType>(this.jiHeDataGrid1.Rows[i].Cells[0].Value.ToString());
                        model.HuanCunName = this.jiHeDataGrid1.Rows[i].Cells[1].Value.ToString();
                        HuanCunFrm huanCunFrm = new HuanCunFrm();
                        huanCunFrm.SetCanShu(model,PeiFangLei);
                        if (huanCunFrm.ShowDialog(this)==DialogResult.OK)
                        {
                            FuZhi(huanCunFrm.CaoZuoHuanCunModel,i);
                        }
                    }                  
                    else if (e.ColumnIndex == 3)
                    {
                        this.jiHeDataGrid1.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
    }
}
