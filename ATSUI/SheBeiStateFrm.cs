using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSUI.KJ;
using ATSUI.Properties;
using BaseUI.UC;
using CommLei.JiChuLei;
using CommLei.JieMianLei;
using JieMianLei.FuFrom;
using SSheBei.Model;
using SSheBei.ZongKongZhi;

namespace ATSUI
{
    public partial class SheBeiStateFrm : BaseFuFrom
    {
        public SheBeiStateFrm()
        {
            InitializeComponent();
            this.IsJingYongZuiXiao = true;
            this.chouTiKJ1.ChuFaXinHaoEvent += chouTiKJ1_ChuFaXinHaoEvent;
        }

        public void SetCanShu()
        {
            this.TopMost = true;
            Dictionary<string, List<TxModel>> fenzu = ZongSheBeiKongZhi.Cerate().GetSheBeiTxState();
            foreach (var item in fenzu.Keys)
            {
                IconPanel duiyi1 = new IconPanel(Color.Transparent);
                this.chouTiKJ1.AddBand(item, item, duiyi1, null, Color.Transparent, Color.Black);
                duiyi1.BackColor = Color.Transparent;
                List<TxModel> kamodelh = fenzu[item];
                for (int j = 0; j < kamodelh.Count; j++)
                {
                    string name = $"{kamodelh[j].SheBeiTD}:{kamodelh[j].SheBeiName}";
                    string xianshiwenben = $"{kamodelh[j].SheBeiTD}:{kamodelh[j].SheBeiName}";
                    duiyi1.AddIcon(name, xianshiwenben,kamodelh[j].ZongTX? Resources.绿色: Resources.红色, Color.Transparent, Color.Black);
                }
            }
        }

        private void chouTiKJ1_ChuFaXinHaoEvent(int DaIndex, int ZiIndex, bool IsRoot, string Text, string sshiwenben)
        {
            string[] wneben = sshiwenben.Split(':');
            if (wneben.Length >= 2)
            {
                this.fGenSui1.Controls.Clear();
                int shebeiid = ChangYong.TryInt(wneben[0], -1);
                Dictionary<string, List<TxModel>> fenzu = ZongSheBeiKongZhi.Cerate().GetSheBeiTxState();
                foreach (var item in fenzu.Keys)
                {
                   
                    List<TxModel> kamodelh = fenzu[item];
                    for (int j = 0; j < kamodelh.Count; j++)
                    {
                        if (kamodelh[j].SheBeiTD==shebeiid)
                        {
                            List<ZiTxModel> txs = kamodelh[j].LisTx;
                            foreach (var item1 in txs)
                            {
                                TXKJ kj = new TXKJ();
                                kj.SetCanShu(item1.Tx,$"{item1.ZiSheBeiID}:{item1.ZiSheBeiName}");
                                this.fGenSui1.Controls.Add(kj);
                            }
                            return;
                        }
                      
                    }
                }
            }


        }
    }
}
