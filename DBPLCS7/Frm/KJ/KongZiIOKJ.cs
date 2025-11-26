using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUI.UC;
using CommLei.JiChuLei;
using DBPLCS7.Model;

namespace DBPLCS7.Frm.KJ
{
    public partial class KongZiIOKJ : UserControl
    {
        public event Func<PLCJiCunQiModel, string> ZhiXingEvent;
        private PLCShBeiModel PLCShBeiModel = new PLCShBeiModel();
        public KongZiIOKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.duoIOKJ1.DianJiOKOrCloseEvent += ChaoChuLieIOKJ1_DianJiOKOrCloseEvent;
        }
        private void ChaoChuLieIOKJ1_DianJiOKOrCloseEvent(object kj, bool isdakai, JiLuModel e)
        {
          
            if (ZhiXingEvent != null)
            {
                for (int i = 0; i < PLCShBeiModel.JiCunQi.Count; i++)
                {
                    PLCJiCunQiModel item = PLCShBeiModel.JiCunQi[i];
                    string key = $"{item.DBKuan},{item.PianYiLiang},{item.Name}";
                    if (key.Equals(e.JiCunQiID))
                    {
                        PLCJiCunQiModel iyews = ChangYong.FuZhiShiTi(item);
                        iyews.JiCunQiModel.Value = isdakai ? item.IOLuZhi : item.IORedZhi;
                        string jieguo = ZhiXingEvent(iyews);
                        break;
                    }

                }
             
            }
        }
        public void SetCanShu(PLCShBeiModel pLCShBeiModel)
        {
            PLCShBeiModel = pLCShBeiModel;
            this.label1.Text = PLCShBeiModel.PLCName;
            List<JiLuModel> duios = new List<JiLuModel>();
            for (int i = 0; i < pLCShBeiModel.JiCunQi.Count; i++)
            {
                if (pLCShBeiModel.JiCunQi[i].IsIO==1)
                {
                    JiLuModel model = new JiLuModel();
                    model.DaKaiColor = Color.Green;
                    model.CloseColor = Color.Red;
                    model.IsChengGong = false;
                    model.MingCheng = pLCShBeiModel.JiCunQi[i].Name;
                    model.SetJiCunIDAndSheBeiID($"{pLCShBeiModel.JiCunQi[i].DBKuan},{pLCShBeiModel.JiCunQi[i].PianYiLiang},{pLCShBeiModel.JiCunQi[i].Name}");
                    duios.Add(model);
                }
              
            }
            duoIOKJ1.SetXieIO(duios);
        }


        public void ShuaXin()
        {
            foreach (var item in PLCShBeiModel.JiCunQi)
            {
                string key = $"{item.DBKuan},{item.PianYiLiang},{item.Name}";
                bool ischenggong =ChangYong.TryStr( item.Value,"").Equals(item.IOLuZhi);
                duoIOKJ1.SetYanSe(key, ischenggong);
            }
            duoIOKJ1.ShuaXin();
            if (PLCShBeiModel.Tx)
            {
                if (this.label1.BackColor != Color.Green)
                {
                    this.label1.BackColor = Color.Green;
                }
            }
            else
            {
                if (this.label1.BackColor != Color.Red)
                {
                    this.label1.BackColor = Color.Red;
                }
            }

        }


    }
}
