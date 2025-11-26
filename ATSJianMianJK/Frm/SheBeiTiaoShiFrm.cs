using BaseUI.UC;
using CommLei.JieMianLei;
using JieMianLei.FuFrom;
using SSheBei.Model;
using SSheBei.ZongKongZhi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.Properties;
using CommLei.JiChuLei;
using System.Xml.Linq;

namespace ATSJianMianJK.Frm
{
    public partial class SheBeiTiaoShiFrm : BaseFuFrom
    {
        private JieJueChuangTiPingJie JieJueChuangTiPingJie;
        public SheBeiTiaoShiFrm()
        {
            InitializeComponent();
          
            this.DoubleBuffered = true;
            JieJueChuangTiPingJie = new JieJueChuangTiPingJie();
        }
        protected override void GuanBi()
        {
            if (this.panel1.Controls.Count>0)
            {
                foreach (Control control in panel1.Controls)
                {
                    if (control is Form)
                    {
                        Form form = (Form)control;                     
                        form.Close();
                    }
                }
            }
            base.GuanBi();
        }
        private void IniData()
        {

            List<JieMianFrmModel> kamodel =ZongSheBeiKongZhi.Cerate().GetSheBeiCanShu();
            Dictionary<string, List<JieMianFrmModel>> fenzu = new Dictionary<string, List<JieMianFrmModel>>();
            for (int i = 0; i < kamodel.Count; i++)
            {
                if (fenzu.ContainsKey(kamodel[i].FenZu)==false)
                {
                    fenzu.Add(kamodel[i].FenZu,new List<JieMianFrmModel>());
                }
                fenzu[kamodel[i].FenZu].Add(kamodel[i]);
            }
            foreach (var item in fenzu.Keys)
            {
                IconPanel duiyi1 = new IconPanel(Color.Transparent);
                this.chouTiKJ1.AddBand(item, item, duiyi1, null, Color.Transparent, Color.Black);
                duiyi1.BackColor = Color.Transparent;
                List<JieMianFrmModel> kamodelh=fenzu[item];
                for (int j = 0; j < kamodelh.Count; j++)
                {
                    string name = $"{kamodelh[j].SheBeiID}:{kamodelh[j].SheBeiName}";
                    string xianshiwenben = $"{kamodelh[j].SheBeiID}:{kamodelh[j].SheBeiName}";
                    duiyi1.AddIcon(name, xianshiwenben, Resources.monitor, Color.Transparent, Color.Black);
                }
            }
            


        }

        private void SheBeiTiaoShiFrm_Load(object sender, EventArgs e)
        {
            this.chouTiKJ1.ChuFaXinHaoEvent += chouTiKJ1_ChuFaXinHaoEvent;
            IniData();
        }
        private void chouTiKJ1_ChuFaXinHaoEvent(int DaIndex, int ZiIndex, bool IsRoot, string Text, string sshiwenben)
        {
            string[] wneben = sshiwenben.Split(':');
            if (wneben.Length>=2)
            {
                JieMianFrmModel kamodel = ZongSheBeiKongZhi.Cerate().GetTiaoShiFrm(ChangYong.TryInt(wneben[0],-1));
                if (kamodel != null)
                {
                    FromDX fromDX = new FromDX();
                    fromDX.DX_kongjian = this.panel1;
                    kamodel.Form.Name = sshiwenben;
                    fromDX.DX_chuangti = kamodel.Form;
                    fromDX.DX_mingcheng = kamodel.Form.Name;
                    JieJueChuangTiPingJie.OpenFrom(fromDX);
                }
                else
                {
                    this.QiDongTiShiKuang("没有取到该设备id对应的界面"+ wneben[0]);
                }
            }
         
            
        }
    }
}
