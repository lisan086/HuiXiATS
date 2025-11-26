using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using ModBuTCP.Model;
using SSheBei.Model;

namespace ModBuTCP.Frm
{
    public partial class PLCZPeiZhiFrm : BaseFuFrom
    {
        public PLCZPeiZhiFrm()
        {
            InitializeComponent();
        }
        protected override void GuanBi()
        {
            this.DialogResult = DialogResult.Cancel;
            base.GuanBi();
        }
        public void SetShuJu(List<DataCunModel> JiCunQis)
        {
            List<JiCunQiKj> kjs = new List<JiCunQiKj>();
            for (int i = 0; i < JiCunQis.Count; i++)
            {
                JiCunQiKj kj = new JiCunQiKj();
                kj.SetCanShu(JiCunQis[i]);
                kjs.Add(kj);
            }
            this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
        }

        public List<DataCunModel> GetShuJu()
        {
            List<DataCunModel> models = new List<DataCunModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is JiCunQiKj)
                {
                    JiCunQiKj ks = this.flowLayoutPanel1.Controls[i] as JiCunQiKj;
                    models.Add(ChangYong.FuZhiShiTi(ks.GetCanShu()));
                }
            }
            return models;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JiCunQiKj kj = new JiCunQiKj();
            kj.SetCanShu(new DataCunModel());
            this.flowLayoutPanel1.Controls.Add(kj);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog wenjian = new OpenFileDialog();
            if (wenjian.ShowDialog(this) == DialogResult.OK)
            {
                this.flowLayoutPanel1.Controls.Clear();
                string wens = wenjian.FileName;
                DuRuExclWenDan duRuExclWenDan = new DuRuExclWenDan();
                Dictionary<string, string> lieming = new Dictionary<string, string>();
                lieming.Add("Name", "Name");
                lieming.Add("JiCunDiZhi", "JiCunDiZhi");
                lieming.Add("Count", "Count");
                lieming.Add("XiaoShuWei", "XiaoShuWei");
                lieming.Add("BeiChuShu", "BeiChuShu");
                lieming.Add("DataType", "DataType");
                lieming.Add("ZiDiZhi", "ZiDiZhi");
                List<DataCunModel> list = duRuExclWenDan.ShuChuExcelOrLis<DataCunModel>(wens, lieming, 0, 0);
                bool flag2 = list != null && list.Count > 0;
                if (flag2)
                {
                    List<JiCunQiKj> kjs = new List<JiCunQiKj>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        JiCunQiKj kj = new JiCunQiKj();
                        kj.SetCanShu(list[i]);
                        kjs.Add(kj);
                    }
                    this.flowLayoutPanel1.Controls.AddRange(kjs.ToArray());
                }
               
            }

        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.xlsx|*.xlsx";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
               
                string wens = saveFileDialog.FileName;
                DuRuExclWenDan duRuExclWenDan = new DuRuExclWenDan();
                Dictionary<string, List<object>> lieming = new Dictionary<string, List<object>>();
                lieming.Add("Name", new List<object>());
                lieming.Add("JiCunDiZhi", new List<object>());
                lieming.Add("Count", new List<object>());
                lieming.Add("XiaoShuWei", new List<object>());
                lieming.Add("BeiChuShu", new List<object>());
                lieming.Add("DataType", new List<object>());
                lieming.Add("ZiDiZhi", new List<object>());
                List<DataCunModel> cunmodekl = GetShuJu();
                for (int i = 0; i < cunmodekl.Count; i++)
                {
                    lieming["Name"].Add(cunmodekl[i].Name);
                    lieming["JiCunDiZhi"].Add(cunmodekl[i].JiCunDiZhi);
                    lieming["Count"].Add(cunmodekl[i].Count);
                    lieming["XiaoShuWei"].Add(cunmodekl[i].XiaoShuWei);
                    lieming["BeiChuShu"].Add(cunmodekl[i].BeiChuShu);
                    lieming["DataType"].Add(cunmodekl[i].DataType);
                    lieming["ZiDiZhi"].Add(cunmodekl[i].ZiDiZhi);
                }
                duRuExclWenDan.DaoChuExc(wens, lieming);
                this.QiDongTiShiKuang("导出成功");
            }
        }
    }
}
