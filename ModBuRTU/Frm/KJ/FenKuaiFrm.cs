using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.DataChuLi;
using JieMianLei.FuFrom;
using SundyChengZhong.Model;

namespace ModBusRTU.Frm.KJ
{
    public partial class FenKuaiFrm : BaseFuFrom
    {
        int shuju = 0;
        public FenKuaiFrm()
        {
            InitializeComponent();
        }

        public void SetCanShu(List<CZJiCunQiModel> kamodes)
        {
            this.flowLayoutPanel1.Controls.Clear();
            int shuliang = 50;
            List<List<CZJiCunQiModel>> fenshujus = new List<List<CZJiCunQiModel>>();
            fenshujus.Add(new List<CZJiCunQiModel>());
            for (int i = 0; i < kamodes.Count; i++)
            {
                fenshujus[fenshujus.Count - 1].Add(kamodes[i]);
                if (fenshujus[fenshujus.Count - 1].Count>= shuliang)
                {
                    fenshujus.Add(new List<CZJiCunQiModel>());
                }
            }
            List<FenKuaiKJ> liskj = new List<FenKuaiKJ>();
            for (int i = 0; i < fenshujus.Count; i++)
            {
                FenKuaiKJ fenKuaiKJ = new FenKuaiKJ();
                fenKuaiKJ.SetCanShu($"分块:{i+1}", fenshujus[i]);
                liskj.Add(fenKuaiKJ);
            }
            this.flowLayoutPanel1.Controls.AddRange(liskj.ToArray());
            shuju = fenshujus.Count;
        }

        public List<CZJiCunQiModel> GetShuJu()
        {
            List<CZJiCunQiModel> shujus = new List<CZJiCunQiModel>();
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                if (this.flowLayoutPanel1.Controls[i] is FenKuaiKJ)
                {
                    FenKuaiKJ fen = this.flowLayoutPanel1.Controls[i] as FenKuaiKJ;
                    shujus.AddRange(fen.GetShuJu());
                }
            }
            return shujus;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FenKuaiKJ fenKuaiKJ = new FenKuaiKJ();
            fenKuaiKJ.SetCanShu($"分块:{shuju + 1}", new List<CZJiCunQiModel>());
            this.flowLayoutPanel1.Controls.Add(fenKuaiKJ);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
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
                lieming.Add("SheBeiDiZhi", "SheBeiDiZhi");
                lieming.Add("BZhi", "BZhi");
                lieming.Add("XieGNM", "XieGNM");
                lieming.Add("DuGNM", "DuGNM");
                lieming.Add("MiaoSu", "MiaoSu");
                List<CZJiCunQiModel> list = duRuExclWenDan.ShuChuExcelOrLis<CZJiCunQiModel>(wens, lieming, 0, 0);
                bool flag2 = list != null && list.Count > 0;
                if (flag2)
                {
                    SetCanShu(list);
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
                lieming.Add("SheBeiDiZhi", new List<object>());
                lieming.Add("BZhi", new List<object>());
                lieming.Add("XieGNM", new List<object>());
                lieming.Add("DuGNM", new List<object>());
                lieming.Add("MiaoSu", new List<object>());
                List<CZJiCunQiModel> cunmodekl = GetShuJu();
                for (int i = 0; i < cunmodekl.Count; i++)
                {
                    lieming["Name"].Add(cunmodekl[i].Name);
                    lieming["JiCunDiZhi"].Add(cunmodekl[i].JiCunDiZhi);
                    lieming["Count"].Add(cunmodekl[i].Count);
                    lieming["XiaoShuWei"].Add(cunmodekl[i].XiaoShuWei);
                    lieming["BeiChuShu"].Add(cunmodekl[i].BeiChuShu);
                    lieming["SheBeiDiZhi"].Add(cunmodekl[i].SheBeiDiZhi);
                    lieming["BZhi"].Add(cunmodekl[i].BZhi);
                    lieming["XieGNM"].Add(cunmodekl[i].XieGNM);
                    lieming["DuGNM"].Add(cunmodekl[i].DuGNM);
                    lieming["MiaoSu"].Add(cunmodekl[i].MiaoSu);
                }
                duRuExclWenDan.DaoChuExc(wens, lieming);
                this.QiDongTiShiKuang("导出成功");
            }
        }
    }
}
