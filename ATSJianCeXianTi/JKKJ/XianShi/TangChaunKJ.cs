using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.JKKJ.TangChuan;
using ATSJianCeXianTi.JKKJ.UIFrm;
using ATSJianCeXianTi.Model;
using ATSJianMianJK;
using ATSJianMianJK.XiTong.Frm.FM;
using CommLei.JieMianLei;

namespace ATSJianCeXianTi.JKKJ.PeiZhiKJ
{
    public partial class TangChaunKJ : UserControl
    {

        private int TDID = -1;

        private List<IFUIFrm> FUIFrm=new List<IFUIFrm>();
        private TDModel TDModel;
        private ZiYuanModel ZiYuanModel;
        public TangChaunKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(TDModel dModel)
        {
            TDID= dModel.TDID;
            TDModel= dModel;
            {
                MoRenFrm moRenFrm = new MoRenFrm();           
                moRenFrm.FanHuiJieGuoEvent += FUIFrm_FanHuiJieGuoEvent;
                moRenFrm.TopLevel = false;
                moRenFrm.FormBorderStyle = FormBorderStyle.None;
                moRenFrm.Dock = DockStyle.Fill;
                moRenFrm.Parent = this;
                moRenFrm.Show();
                FUIFrm.Add(moRenFrm);
            }
            {
                TuXingFrm moRenFrm = new TuXingFrm();
                moRenFrm.FanHuiJieGuoEvent += FUIFrm_FanHuiJieGuoEvent;
                moRenFrm.TopLevel = false;
                moRenFrm.FormBorderStyle = FormBorderStyle.None;
                moRenFrm.Dock = DockStyle.Fill;
                moRenFrm.Parent = this;
                moRenFrm.Show();
                FUIFrm.Add(moRenFrm);
            }
        }

        public void SetCanShu(int tdid,ZiYuanModel model)
        {
            if (TDID == tdid)
            {
                ZiYuanModel = model;
            }
        }

        private void FUIFrm_FanHuiJieGuoEvent(Model.ZhiJieGuo obj)
        {
            JieMianModel model = new JieMianModel();
            model.TDID = TDID;
            model.ZhiJieGuo = obj;
            Lei.JieMianLei.Cerate().CaoZuo(Model.DoType.UIFanHuiJieGuo, model);
        }
        public void SetXianShi(bool isxianshi,int typeid)
        {
            if (isxianshi)
            {
                for (int i = 0; i < FUIFrm.Count; i++)
                {
                    if (typeid == FUIFrm[i].TypeID)
                    {
                        FUIFrm[i].SetXianShi(isxianshi);
                    }
                    else
                    {
                        FUIFrm[i].SetXianShi(false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < FUIFrm.Count; i++)
                {
                    FUIFrm[i].SetXianShi(false);
                }
            }
        }
        public void SetCanShu(int tdid, TangChuanUIModel canshu)
        {
            if (TDID==tdid)
            {
                for (int i = 0; i < FUIFrm.Count; i++)
                {
                    if (canshu.Type == FUIFrm[i].TypeID)
                    {
                        if (FUIFrm[i] is Form)
                        {
                            Form form = (Form)FUIFrm[i];
                            form.BringToFront();
                            form.Show();
                        }
                        FUIFrm[i].SetCanShu(canshu);
                    }
                    else
                    {
                        if (FUIFrm[i] is Form)
                        {
                            Form form = (Form)FUIFrm[i];
                            form.Visible = false;
                        }
                      
                    }
                }
              
            }
        }
    }
}
