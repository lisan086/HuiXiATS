using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;
using SSheBei.PeiZhi;

namespace ATSJianMianJK.XiTong.Frm.KJ
{
    public partial class DuJiCunQiKJ : UserControl
    {
        private DuModel DuModel;
        public DuJiCunQiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu( DuModel duModel,List<string> lis)
        {
            if (duModel==null)
            {
                duModel = new DuModel();
            }
            DuModel = ChangYong.FuZhiShiTi(duModel);
          
            this.commBoxE2.Items.Clear();
           
            for (int i = 0; i < lis.Count; i++)
            {
                this.commBoxE2.Items.Add(lis[i]);
            }
            this.textBox1.Text = duModel.JiCunQiName;
            this.textBox2.Text = duModel.SheBeiID.ToString();
            this.textBox3.Text = duModel.PiPeiValue;
            this.commBoxE2.Text = duModel.PiPeiType.ToString();
            this.textBox4.Text = duModel.Type;
        }

        public DuModel GetModel()
        {
            DuModel model = ChangYong.FuZhiShiTi(DuModel);
            model.PiPeiValue = this.textBox3.Text;
            model.JiCunQiName = this.textBox1.Text;
            model.Type = this.textBox4.Text;
            model.SheBeiID = ChangYong.TryInt(this.textBox2.Text,-1);
            model.PiPeiType =ChangYong.GetMeiJuZhi< PiPeiType >( this.commBoxE2.Text);
            return model;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int shebeiid = ChangYong.TryInt(this.textBox2.Text,-1);
            XuanZeJiCunQiFrm xuanZeJiCunQiFrm = new XuanZeJiCunQiFrm();
            xuanZeJiCunQiFrm.SetCanShu(this.textBox1.Text,shebeiid,1);
            if (xuanZeJiCunQiFrm.ShowDialog(this)==DialogResult.OK)
            {
                this.textBox1.Text = xuanZeJiCunQiFrm.JiCunQiWeiYiBiaoShi;
                int xinshebeiid = xuanZeJiCunQiFrm.SheBeiID;
                this.textBox2.Text = xinshebeiid.ToString();
            }
        }
    }
}
