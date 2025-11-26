
using BaseUI.UC;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhongWangSheBei.Model;
using ZhongWangSheBei.ShiXian;

namespace ZhongWangSheBei.Frm
{
    public partial class IOKJ : UserControl
    {
       
       
        private ZiSheBeiModel IPCom = null;

        private int ZongSheBeiID = -1;
      
        private PeiZhiLei PeiZhiLei;
     
        public IOKJ(PeiZhiLei peiZhiLei)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            PeiZhiLei = peiZhiLei;
            this.duoIOKJ1.DianJiOKOrCloseEvent += ChaoChuLieIOKJ1_DianJiOKOrCloseEvent;
        }
        public void SetCanShu(ZiSheBeiModel zSCOM,int zongshebeiid)
        {                  
            this.label1.Text = $"第{zongshebeiid},{zSCOM.ZiName}-{zSCOM.DiZhi}路";
            ZongSheBeiID = zongshebeiid;
            IPCom = zSCOM;          
            SetCanShu();
        }
        private void ChaoChuLieIOKJ1_DianJiOKOrCloseEvent(object kj, bool isdakai, JiLuModel e)
        {
            PeiZhiLei.XieJiDianQi(ZongSheBeiID, IPCom.ZSID, int.Parse(e.JiCunQiID), isdakai, false);
          
        }

        private  void SetCanShu()
        {
            List<JiLuModel> duios = new List<JiLuModel>();
            for (int i = 0; i < IPCom.JiLu; i++)
            {
                JiLuModel model = new JiLuModel();
                model.DaKaiColor = Color.Green;
                model.CloseColor = Color.Red;
                model.IsChengGong = false;
                model.MingCheng = (i + 1).ToString();
                model.SetJiCunIDAndSheBeiID((i + 1).ToString());
                duios.Add(model);
            }      
            duoIOKJ1.SetXieIO(duios);
        }

        public void ShuaXin()
        {
            foreach (var item in PeiZhiLei.DataMoXing.JiLu.Keys)
            {
                if (PeiZhiLei.DataMoXing.JiLu[item].ZongSheBeiId == ZongSheBeiID && PeiZhiLei.DataMoXing.JiLu[item].ZiSheBeiID == IPCom.ZSID)
                {
                    if (PeiZhiLei.DataMoXing.JiLu[item].IsDu == CunType.DuJiCunQi)
                    {
                        duoIOKJ1.SetYanSe((PeiZhiLei.DataMoXing.JiLu[item].JiCunDiZhi + 1).ToString(), PeiZhiLei.DataMoXing.JiLu[item].JiCunQi.Value.ToString().Equals("1"));
                    }
                }
            }
            duoIOKJ1.ShuaXin();
            if (IPCom.Tx)
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

       

        private void button2_Click(object sender, EventArgs e)
        {
            PeiZhiLei.XieJiDianQi(ZongSheBeiID, IPCom.ZSID,0, false,true);
        }
    }
}
