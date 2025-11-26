using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.XiTong.Frm.FM;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;
using static ATSJianMianJK.GongNengLei.XieBuZhou;

namespace ATSJianMianJK.XiTong.Frm.KJ
{
    public partial class TDDuIOKJ : UserControl
    {
      
        private List<DuIOCanShuModel> LisDuIOCanShuModels = new List<DuIOCanShuModel>();
        private List<DuShuJuModel> LisDuShuJuModels = new List<DuShuJuModel>();
        private List<XieSateModel> LisXieSateModels = new List<XieSateModel>();
    
      
        public TDDuIOKJ()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<DuIOCanShuModel> tdiocanshus, List<DuShuJuModel> tdshujucanshu, List<XieSateModel> tdxiecanshu, int id)
        {
         
         
            LisDuIOCanShuModels= ChangYong.FuZhiShiTi(tdiocanshus);
            LisDuShuJuModels= ChangYong.FuZhiShiTi(tdshujucanshu);
            LisXieSateModels= ChangYong.FuZhiShiTi(tdxiecanshu);
           
            this.textBox1.Text=id.ToString();
        }

      
        public List<DuIOCanShuModel> GetDuIOCanShuModel()
        {
            int id = ChangYong.TryInt(this.textBox1.Text, -1);
            List<DuIOCanShuModel> shuju = ChangYong.FuZhiShiTi(LisDuIOCanShuModels);
            shuju.ForEach((x) => { x.TDID = id; });
            return shuju;
        }
        public List<DuShuJuModel> GetDuShuJuModel()
        {
            int id = ChangYong.TryInt(this.textBox1.Text, -1);
            List<DuShuJuModel> shuju = ChangYong.FuZhiShiTi(LisDuShuJuModels);
            shuju.ForEach((x) => { x.TDID = id; });
            return shuju;
        }
        public List<XieSateModel> GetXieSateModel()
        {
            int id = ChangYong.TryInt(this.textBox1.Text, -1);
            List<XieSateModel> shuju = ChangYong.FuZhiShiTi(LisXieSateModels);
            shuju.ForEach((x) => { x.TDID = id; });
            return shuju;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //DuStateFrm frm=new DuStateFrm();
            //frm.SetCanShu(LisDuSateModels,new List<string>());
            //if (frm.ShowDialog(this)==DialogResult.OK)
            //{
            //    LisDuSateModels = ChangYong.FuZhiShiTi(frm.GetCanShu());
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DuIOFrm frm = new DuIOFrm();
            frm.SetCanShu(LisDuIOCanShuModels);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                LisDuIOCanShuModels = ChangYong.FuZhiShiTi(frm.GetCanShu());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DuCanShuFrm frm = new DuCanShuFrm();
            frm.SetCanShu(LisDuShuJuModels);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                LisDuShuJuModels = ChangYong.FuZhiShiTi(frm.GetCanShu());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            XieStateFrm frm = new XieStateFrm();
            frm.SetCanShu(LisXieSateModels);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                LisXieSateModels = ChangYong.FuZhiShiTi(frm.GetCanShu());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }
    }
}
