using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using SSheBei.PeiZhi;
using ZhongWangSheBei.Frm;
using ZhongWangSheBei.Model;

namespace XiangTongChuanKouSheBei.Frm
{
    public partial class DuJiCunQiFrm : BaseFuFrom
    {
        private int Type = 1;
        public DuJiCunQiFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(List< ZiSheBeiModel> ziSheBeiModel)
        {
            Type = 1;
            List< PeiKJ >liskj= new List< PeiKJ >();
            for (int i = 0; i < ziSheBeiModel.Count; i++)
            {
                PeiKJ kj = new PeiKJ();
                kj.SetCanShu(ziSheBeiModel[i]);
                liskj.Add( kj );
            }
            this.flowLayoutPanel1.Controls.AddRange(liskj.ToArray());
        }
        public void SetCanShu(List<CunModel> ziSheBeiModel)
        {
            Type = 2;
            List<JiCunQiKJ> liskj = new List<JiCunQiKJ>();
            for (int i = 0; i < ziSheBeiModel.Count; i++)
            {
                JiCunQiKJ kj = new JiCunQiKJ();
                kj.SetCanShu(ziSheBeiModel[i]);
                liskj.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(liskj.ToArray());
        }

        public List<ZiSheBeiModel> GetSheBei()
        {
            List < ZiSheBeiModel > lis= new List<ZiSheBeiModel >();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is PeiKJ)
                {
                    PeiKJ kj = this.flowLayoutPanel1.Controls[i] as PeiKJ;
                    lis.Add(kj.GetCanShu());
                }
            }
            return lis;
        }
        public List<CunModel> GetJiCunQi()
        {
            List<CunModel> lis = new List<CunModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is JiCunQiKJ)
                {
                    JiCunQiKJ kj = this.flowLayoutPanel1.Controls[i] as JiCunQiKJ;
                    lis.Add(kj.GetCanShu());
                }
            }
            return lis;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Type == 1)
            {
                PeiKJ kj = new PeiKJ();
                kj.SetCanShu(new ZiSheBeiModel());
                this.flowLayoutPanel1.Controls.Add(kj);
            }
            else if (Type == 2)
            {
                JiCunQiKJ kj = new JiCunQiKJ();
                kj.SetCanShu(new CunModel());
                this.flowLayoutPanel1.Controls.Add(kj);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int shebeiid = ChangYong.TryInt(this.textBox5.Text,-1);
            if (Type == 1)
            {
                if (shebeiid<0)
                {
                    this.QiDongTiShiKuang("设备ID不能小于0");
                    return;
                }
                List<ZiSheBeiModel> lis = GetSheBei();
                if (lis.Count>0)
                {
                    ZiSheBeiModel model = null;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].ZiID== shebeiid)
                        {
                            model = ChangYong.FuZhiShiTi(lis[i]);
                            break;
                        }
                    }
                    if (model!=null)
                    {
                        PeiKJ kj = new PeiKJ();
                        kj.SetCanShu(model);
                        this.flowLayoutPanel1.Controls.Add(kj);
                    }
                }
            }
            else if (Type == 2)
            {
                List<CunModel> lis =GetJiCunQi();
                if (lis.Count > 0)
                {
                    CunModel model = null;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].ZhiLingID == shebeiid)
                        {
                            model = ChangYong.FuZhiShiTi(lis[i]);
                            break;
                        }
                    }
                    if (model != null)
                    {
                        JiCunQiKJ kj = new JiCunQiKJ();
                        kj.SetCanShu(model);
                        this.flowLayoutPanel1.Controls.Add(kj);
                    }
                }
            }
        }
    }
}
