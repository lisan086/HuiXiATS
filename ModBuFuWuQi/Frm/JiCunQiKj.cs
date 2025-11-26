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
using ModBuTCP.Model;
using SSheBei.Model;

namespace ModBuTCP.Frm
{
    public partial class JiCunQiKj : UserControl
    {
        private DataCunModel YeWuSheBeiModel;
        public JiCunQiKj()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void SetCanShu(DataCunModel model)
        {
            this.commBoxE1.Items.Clear();
            List<string> lisd = ChangYong.MeiJuLisName(typeof(DataType));
            for (int i = 0; i < lisd.Count; i++)
            {
                this.commBoxE1.Items.Add(lisd[i]);
            }
            this.commBoxE2.Items.Clear();
            List<string> lisds = ChangYong.MeiJuLisName(typeof(YingYongType));
            for (int i = 0; i < lisds.Count; i++)
            {
                this.commBoxE2.Items.Add(lisds[i]);
            }
            YeWuSheBeiModel = ChangYong.FuZhiShiTi(model);
            this.textBox2.Text = YeWuSheBeiModel.Name.ToString();
            this.textBox1.Text = YeWuSheBeiModel.JiCunDiZhi.ToString();
            this.textBox4.Text = YeWuSheBeiModel.Count.ToString();
            this.commBoxE1.Text = model.DataType.ToString();
       
      
           
            this.commBoxE2.Text= model.YingYongType.ToString();
           
        }

        public DataCunModel GetCanShu()
        {
            DataCunModel model = ChangYong.FuZhiShiTi(YeWuSheBeiModel);
            model.Name = this.textBox2.Text;
          
            model.JiCunDiZhi = (short)ChangYong.TryInt(this.textBox1.Text, 0);
            model.DataType = ChangYong.GetMeiJuZhi<DataType>(this.commBoxE1.Text);
            model.Count = (short)ChangYong.TryInt(this.textBox4.Text, 0);
          
           
            model.YingYongType = ChangYong.GetMeiJuZhi<YingYongType>(this.commBoxE2.Text);
           
        
            return model;
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
