using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YiBanSaoMaQi.Model;

namespace SuanShuSheBei.Frm
{
    public partial class JiaoBenKJ : UserControl
    {
        public JiaoBenKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(CunModel model)
        {
            this.textBox5.Text = model.JiaoBenName;
            this.textBox6.Text = model.MiaoSu;
            this.textBox1.Text = model.JiaoBenNeiRong;
        }

        public CunModel GetModel()
        {
            CunModel model = new CunModel();
            model.JiaoBenNeiRong = this.textBox1.Text;
            model.JiaoBenName= this.textBox5.Text;
            model.MiaoSu= this.textBox6.Text;
            return model;
        }
    }
}
