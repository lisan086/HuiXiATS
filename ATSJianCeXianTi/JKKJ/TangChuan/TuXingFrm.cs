using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.JKKJ.UIFrm;
using ATSJianCeXianTi.Model;
using JieMianLei.FuFrom;

namespace ATSJianCeXianTi.JKKJ.TangChuan
{
    public partial class TuXingFrm : BaseFuFrom, IFUIFrm
    {
        private bool IsXianShi = false;
        private bool IsSaoMa { get; set; } = false;
        private ZhiJieGuo ZhiJieGuo { get; set; } = new ZhiJieGuo();
        private TangChuanUIModel TangChuanUIModel = new TangChuanUIModel();
        public int TypeID
        {
            get
            {
                return 2;
            }
        }

        public TuXingFrm()
        {
            InitializeComponent();
            this.QuXiaoBiaoTi();
        }

        public event Action<ZhiJieGuo> FanHuiJieGuoEvent;

        public void SetCanShu(TangChuanUIModel canshu)
        {
            TangChuanUIModel = canshu;
            this.huaTuKJ1.SetShuJu(TangChuanUIModel.LisShuJu);
        }
        private void ChuFa()
        {
            if (FanHuiJieGuoEvent != null)
            {
                FanHuiJieGuoEvent(ZhiJieGuo);
            }
        }

        public Form GetFrm()
        {
            return this;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (IsXianShi)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        {
                            button1_Click(null, null);
                        }
                        break;
                    case Keys.Space:
                        {
                            button2_Click(null, null);
                        }
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void SetXianShi(bool isxianshi)
        {
            IsXianShi = isxianshi;
            if (IsXianShi)
            {
                this.timer1.Enabled = true;
            }
            else
            {
                this.timer1.Enabled = false;
                TangChuanUIModel.LisShuJu.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ZhiJieGuo.IsHeGe = true;
            ZhiJieGuo.RecZhi = "OK";
            ChuFa();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ZhiJieGuo.IsHeGe = false;
            ZhiJieGuo.RecZhi = "NG";
            ChuFa();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            this.huaTuKJ1.ShuaXin();
        }
    }
}
