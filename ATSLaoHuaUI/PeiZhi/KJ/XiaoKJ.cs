using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.PeiZhi.KJ
{
    public partial class XiaoKJ : UserControl
    {
        private int TDID = -1;
        public XiaoKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(int gongid,string mingcheng,string pipeizhi)
        {
            TDID=gongid;
            this.label3.Text = mingcheng;   
            this.label2.Text = $"匹配值:{pipeizhi}";
         
        }

        public void ShuaXinShuJu(ShuJuLisModel model)
        {
           
            this.label1.Text = model.JiCunValue;
        }
    }
}
