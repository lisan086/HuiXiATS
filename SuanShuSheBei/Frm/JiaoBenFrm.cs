using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JieMianLei.FuFrom;
using YiBanSaoMaQi.Model;

namespace SuanShuSheBei.Frm
{
    public partial class JiaoBenFrm : BaseFuFrom
    {
        public JiaoBenFrm()
        {
            InitializeComponent();
        }
        public void SetCanShu(List<CunModel> lis)
        {
            this.flowLayoutPanel1.Controls.Clear();
            List<JiaoBenKJ> liskj=new List<JiaoBenKJ>();
            for (int i = 0; i < lis.Count; i++)
            {
                JiaoBenKJ kj = new JiaoBenKJ();
                kj.SetCanShu(lis[i]);
                liskj.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(liskj.ToArray());
        }

        public List<CunModel> GetModel()
        {
            List<CunModel> lis = new List<CunModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is JiaoBenKJ)
                {
                    JiaoBenKJ kj = this.flowLayoutPanel1.Controls[i] as JiaoBenKJ;
                    lis.Add(kj.GetModel());
                }
            }
            return lis;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            JiaoBenKJ kj = new JiaoBenKJ();
            kj.SetCanShu(new CunModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
