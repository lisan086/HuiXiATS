using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.JiChuLei;
using LeiSaiDMC.Model;
using SSheBei.Model;

namespace LeiSaiDMC.Frm.KJ
{
    public partial class ZhouKJ : UserControl
    {
        private LSModel LSModel;
        private List<CunModel> lisshuju;
        private PeiZhiLei PeiZhiLei;
        private ZhouModel ZhouModel;
     
        public ZhouKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(LSModel ka,ZhouModel zhoumodel,List<CunModel> lismodel, PeiZhiLei peiZhi)
        {
            LSModel = ka;
            ZhouModel = zhoumodel;
            lisshuju = lismodel;
            PeiZhiLei = peiZhi;
            
            this.label8.Text = $"{zhoumodel.ZhouName}";
        }

        public void ShuaXinData()
        {
            if (LSModel.TX)
            {
                if (this.label8.BackColor != Color.Green)
                {
                    this.label8.BackColor = Color.Green;
                }
            }
            else
            {
                if (this.label8.BackColor != Color.Red)
                {
                    this.label8.BackColor = Color.Red;
                }
            }
            {
                if (ZhouModel!=null)
                {
                    string peizhisudu = $"{ZhouModel.HomeSuDu},{ZhouModel.SuDu}";
                    if (textBox4.Text!= peizhisudu)
                    {
                        textBox4.Text= peizhisudu;
                    }
                    string jiasudu = $"{ZhouModel.HomeJiaSuDu},{ZhouModel.JiaSuDu}";
                    if (textBox5.Text != jiasudu)
                    {
                        textBox5.Text = jiasudu;
                    }
                    string huiling = $"{ZhouModel.HuiLingMoShi}";
                    if (textBox7.Text != jiasudu)
                    {
                        textBox7.Text = jiasudu;
                    }
                }
               
            }
            for (int i = 0; i < lisshuju.Count; i++)
            {
                IOType xieCaoZuoType = lisshuju[i].IOTYpe;
                switch (xieCaoZuoType)
                {
                    case IOType.Zhou位置:
                        {
                            string weizhi = $"位置:{lisshuju[i].JiCunQi.Value}";
                            if (label2.Text.Equals(weizhi)==false)
                            {
                                label2.Text = weizhi;
                            }                    
                        }
                        break;
                    case IOType.Zhou速度:
                        {
                            string weizhi = $"速度:{lisshuju[i].JiCunQi.Value}";
                            if (label3.Text.Equals(weizhi) == false)
                            {
                                label3.Text = weizhi;
                            }
                        }
                        break;
                    case IOType.Zhou使能:
                        {
                            int shineng = ChangYong.TryInt(lisshuju[i].JiCunQi.Value, 0);
                            if (shineng == 1)
                            {
                                if (label1.BackColor != Color.Green)
                                {
                                    label1.BackColor = Color.Green;
                                    this.button1.Text = "不使能";
                                }
                            }
                            else
                            {
                                if (label1.BackColor != Color.Red)
                                {
                                    label1.BackColor = Color.Red;
                                    this.button1.Text = "使能";
                                }
                            }
                        }
                        break;                
                    case IOType.Zhou运行状态:
                        {
                            int shineng = ChangYong.TryInt(lisshuju[i].JiCunQi.Value, 1);
                            if (shineng == 0)
                            {
                                string weizhi = $"状态:停止";
                                if (label4.Text.Equals(weizhi) == false)
                                {
                                    label4.Text = weizhi;
                                }
                            }
                            else
                            {
                                string weizhi = $"状态:运行";
                                if (label4.Text.Equals(weizhi) == false)
                                {
                                    label4.Text = weizhi;
                                }
                            }
                        }
                        break;
                    case IOType.Zhou报警:
                        {
                            int shineng = ChangYong.TryInt(lisshuju[i].JiCunQi.Value, 0);
                            if (shineng == 1)
                            {
                                if (label5.BackColor != Color.Red)
                                {
                                    label5.BackColor = Color.Red;
                                }
                            }
                            else
                            {
                                if (label5.BackColor != Color.Green)
                                {
                                    label5.BackColor = Color.Green;
                                }
                            }
                        }
                        break;
                    case IOType.Zhou急停:
                        {
                            int shineng = ChangYong.TryInt(lisshuju[i].JiCunQi.Value, 0);
                            if (shineng == 1)
                            {
                                if (label10.BackColor != Color.Red)
                                {
                                    label10.BackColor = Color.Red;
                                }
                            }
                            else
                            {
                                if (label10.BackColor != Color.Green)
                                {
                                    label10.BackColor = Color.Green;
                                }
                            }
                        }
                        break;
                    case IOType.Zhou在线:
                        {
                            int shineng = ChangYong.TryInt(lisshuju[i].JiCunQi.Value, 0);
                            if (shineng == 1)
                            {
                                if (label11.BackColor != Color.Green)
                                {
                                    label11.BackColor = Color.Green;
                                }
                            }
                            else
                            {
                                if (label11.BackColor != Color.Red)
                                {
                                    label11.BackColor = Color.Red;
                                }
                            }
                        }
                        break;
                    case IOType.Zhou模式:
                        {
                            string shineng = ChangYong.TryStr(lisshuju[i].JiCunQi.Value, "");
                            if (label12.Text!= shineng)
                            {
                                label12.Text= shineng;
                            }
                           
                        }
                        break;
                    case IOType.Zhou到位:
                        {
                            int shineng = ChangYong.TryInt(lisshuju[i].JiCunQi.Value, 0);
                            if (shineng == 1)
                            {
                                if (label13.BackColor != Color.Green)
                                {
                                    label13.BackColor = Color.Green;
                                }
                            }
                            else
                            {
                                if (label13.BackColor != Color.White)
                                {
                                    label13.BackColor = Color.White;
                                }
                            }
                        }
                        break;
                    case IOType.Zhou忙:
                        {
                            int shineng = ChangYong.TryInt(lisshuju[i].JiCunQi.Value, 0);
                            if (shineng !=1)
                            {
                                if (label14.BackColor != Color.Green)
                                {
                                    label14.BackColor = Color.Green;
                                }
                            }
                            else
                            {
                                if (label14.BackColor != Color.Red)
                                {
                                    label14.BackColor = Color.Red;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
         
        }
        private void button4_Click(object sender, EventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();
          
            danzhou.GongNengType = GongNengType.XieZhou相对位置运动;
        
            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo= ZhouModel.ZhouNO, IsCaiYongPeiZhi=1, WeiZhi=ChangYong.TryDouble(this.textBox1.Text,0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Contains("不"))
            {
                SendDataModel danzhou = new SendDataModel();

                danzhou.GongNengType = GongNengType.XieZhou不使能;

                danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0) } };
                CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
                if (model != null)
                {
                    model.JiCunQi.Value = danzhou;
                    PeiZhiLei.XieJiDianQi(model.JiCunQi);
                }
              
            }
            else
            {
                SendDataModel danzhou = new SendDataModel();

                danzhou.GongNengType = GongNengType.XieZhou使能;

                danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0) } };
                CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
                if (model != null)
                {
                    model.JiCunQi.Value = danzhou;
                    PeiZhiLei.XieJiDianQi(model.JiCunQi);
                }
        
            }
        }

      

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();

            danzhou.GongNengType = GongNengType.XieZhou恒速运动;

            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0), JiaSuDu = ChangYong.TryDouble(this.textBox2.Text, 0), SuDu =-ChangYong.TryDouble(this.textBox1.Text, 0), HuiLingType = ChangYong.TryInt(this.textBox6.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
      
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            button8_Click(sender,e);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();

            danzhou.GongNengType = GongNengType.XieZhou恒速运动;

            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0), JiaSuDu = ChangYong.TryDouble(this.textBox2.Text, 0), SuDu = ChangYong.TryDouble(this.textBox1.Text, 0), HuiLingType = ChangYong.TryInt(this.textBox6.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
        
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();

            danzhou.GongNengType = GongNengType.XieZhou相对位置运动;

            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = -ChangYong.TryDouble(this.textBox1.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();

            danzhou.GongNengType = GongNengType.XieZhou停止;

            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0), JiaSuDu = ChangYong.TryDouble(this.textBox2.Text, 0), SuDu = ChangYong.TryDouble(this.textBox3.Text, 0), HuiLingType = ChangYong.TryInt(this.textBox6.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
     
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();

            danzhou.GongNengType = GongNengType.XieZhou回零;

            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0), JiaSuDu = ChangYong.TryDouble(this.textBox2.Text, 0), SuDu = ChangYong.TryDouble(this.textBox3.Text, 0), HuiLingType = ChangYong.TryInt(this.textBox6.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
   
        }

      
     

        private void button7_Click_1(object sender, EventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();

            danzhou.GongNengType = GongNengType.XieZhou回零配置;

            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0), JiaSuDu = ChangYong.TryDouble(this.textBox2.Text, 0), SuDu = ChangYong.TryDouble(this.textBox3.Text, 0), HuiLingType= ChangYong.TryInt(this.textBox6.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
     
          
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();

            danzhou.GongNengType = GongNengType.XieZhou正常配置;

            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0), JiaSuDu= ChangYong.TryDouble(this.textBox2.Text, 0) , SuDu= ChangYong.TryDouble(this.textBox3.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
          
        }

        private void label10_Click(object sender, EventArgs e)
        {
            if (label10.BackColor == Color.Red)
            {
                SendDataModel danzhou = new SendDataModel();

                danzhou.GongNengType = GongNengType.XieZhou取消急停;

                danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0) } };
                CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
                if (model != null)
                {
                    model.JiCunQi.Value = danzhou;
                    PeiZhiLei.XieJiDianQi(model.JiCunQi);
                }
           
            }
            else
            {
                SendDataModel danzhou = new SendDataModel();

                danzhou.GongNengType = GongNengType.XieZhou急停;

                danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0) } };
                CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
                if (model != null)
                {
                    model.JiCunQi.Value = danzhou;
                    PeiZhiLei.XieJiDianQi(model.JiCunQi);
                }
               
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();
         
            danzhou.GongNengType = GongNengType.XieZhou退出回零;

            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SendDataModel danzhou = new SendDataModel();
            danzhou.GongNengType = GongNengType.XieZhou绝对位置运动;
            danzhou.Zhous = new List<ZhouCanShuModel>() { new ZhouCanShuModel() { ZhouNo = ZhouModel.ZhouNO, IsCaiYongPeiZhi = 1, WeiZhi = ChangYong.TryDouble(this.textBox1.Text, 0) } };
            CunModel model = PeiZhiLei.DataMoXing.GetCunModel(LSModel.SheBeiID, CanShuType.XieShuJu);
            if (model != null)
            {
                model.JiCunQi.Value = danzhou;
                PeiZhiLei.XieJiDianQi(model.JiCunQi);
            }
        }

     
    }
}
