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
using ATSJianCeXianTi.JKKJ.XianShi;
using ATSJianCeXianTi.Model;
using ATSJianCeXianTi.PeiFangFrm;
using ATSJianMianJK;
using BaseUI.UC;
using CommLei.JiChuLei;

namespace ATSJianCeXianTi.JKKJ.PeiZhiKJ
{
    public partial class TDUIKJ : UserControl
    {
      
        /// <summary>
        /// 通道ID
        /// </summary>
        private int TdID = -1;
        private TDModel DModel = null;
        private bool IsQiDongZheDang = false;
        public TDUIKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true; 
        }


        public void IniData(TDModel tDModel)
        {
            TdID = tDModel.TDID;
            DModel = tDModel;
            IsQiDongZheDang = tDModel.PeiZhiCanShu.IsXianShiZheDang;
            {
                this.gzkj1.Dock = DockStyle.Fill;
                this.gzkj1.SetCanShu(tDModel);
                if (IsQiDongZheDang)
                {
                    this.gzkj1.SetJiaoDian();
                }
                this.gzkj1.Visible = IsQiDongZheDang;
            }
            {
                this.tdBiaoGeKJ1.Dock = DockStyle.Fill;
                this.tdBiaoGeKJ1.SetCanShu(tDModel);
                if (IsQiDongZheDang==false)
                {
                    this.tdBiaoGeKJ1.SetJiaoDian();
                }
                this.tdBiaoGeKJ1.Visible = !IsQiDongZheDang;
            }
        }

        public void IniData(int tdid,ZiYuanModel ziYuanModel)
        {
            if (tdid == TdID)
            {
                this.gzkj1.SetCanShu(ziYuanModel);
                this.tdBiaoGeKJ1.SetCanShu(ziYuanModel);
            }
        }

      
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (IsQiDongZheDang)
            {
                switch (keyData)
                {
                    case Keys.D:
                        {
                            this.gzkj1.Visible = !this.gzkj1.Visible;
                            if (this.gzkj1.Visible == false)
                            {
                                this.tdBiaoGeKJ1.Visible = true;
                                this.tdBiaoGeKJ1.SetJiaoDian();
                            }
                            else
                            {                            
                                this.tdBiaoGeKJ1.Visible = false;
                                this.gzkj1.SetJiaoDian();
                            }

                        }

                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        /// <summary>
        /// 设置开关参数
        /// </summary>
        /// <param name="dModel"></param>
        /// <param name="tdid"></param>
        public void ShuaXin( int tdid)
        {
            if (TdID == tdid)
            {
                this.gzkj1.ShuaXin();
                this.tdBiaoGeKJ1.ShuaXin();

            }
        }

        /// <summary>
        /// 设置测试项目
        /// </summary>
        /// <param name="tdid"></param>
        /// <param name="model"></param>
        public void SetTestXiangMu(int tdid, XiangMuModel model,string haomiao)
        {
            this.gzkj1.SetTestXiangMu(tdid, model, haomiao);
            this.tdBiaoGeKJ1.SetTestXiangMu(tdid, model, haomiao);
        }

      
    

        public void JiaZaiPeiFang()
        {
            this.tdBiaoGeKJ1.JiaZaiPeiFang();      
        }


       
    }
}
