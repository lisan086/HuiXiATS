using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianCeXianTi.Model;
using JieMianLei.FuFrom;

namespace ATSJianCeXianTi.JKKJ.UIFrm
{
    public partial class MoRenFrm : BaseFuFrom,IFUIFrm
    {
        private bool IsXianShi = false;
        private bool IsSaoMa { get; set; } = false;
        private ZhiJieGuo ZhiJieGuo { get; set; } = new ZhiJieGuo();

        public int TypeID
        {
            get
            {
                return 1;
            }
        }

        public MoRenFrm()
        {
            InitializeComponent();
            this.QuXiaoBiaoTi();
           
        }

        public event Action<ZhiJieGuo> FanHuiJieGuoEvent;

        public void SetCanShu(TangChuanUIModel canshu)
        {
            this.label1.Text = canshu.XingXi;
            try
            {
                this.panel1.BackgroundImage = Image.FromFile(canshu.LuJing);
            }
            catch
            {


            }
            IsSaoMa = canshu.IsSaoMa;
            this.panel1.Focus();
        }
        private void ChuFa()
        {
            if (FanHuiJieGuoEvent!=null)
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
                            button1_Click(null,null);
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
    }
}
