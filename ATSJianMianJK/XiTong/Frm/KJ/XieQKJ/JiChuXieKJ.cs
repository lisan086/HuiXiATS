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
using BaseUI.FuFrom.XinWeiHuFrm;
using CommLei.JiChuLei;
using SSheBei.PeiZhi;

namespace ATSJianMianJK.XiTong.Frm.KJ.XieQKJ
{
    public partial class JiChuXieKJ : UserControl
    {
        public JiChuXieKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void SetCanShu(JiChuXieDYModel xieDYModel,bool isfuzhi)
        {
            this.commBoxE3.Items.Clear();
            this.commBoxE2.Items.Clear();
            this.commBoxE1.Items.Clear();
            this.commBoxE1.SetCanShu<YblLeiXing>();
            if (isfuzhi == false)
            {
                this.commBoxE2.SetCanShu<ZblLeiXing>();
            }
            else
            {
                List<string> lisfuzhid = ChangYong.MeiJuLisName(typeof(ZblLeiXing));
                for (int i = 0; i < lisfuzhid.Count; i++)
                {
                    bool isyaojia = false;
                    if (lisfuzhid[i] == "HuanCunLiang"|| lisfuzhid[i] == "XieJC")
                    {
                        isyaojia = true;
                    }
                    if (isyaojia)
                    {
                        this.commBoxE2.Items.Add(lisfuzhid[i]);
                    }
                }
            }
            List<string> lisfuzhi =ChangYong.MeiJuLisName(typeof( ZhongJianType));
            for (int i = 0; i < lisfuzhi.Count; i++)
            {
                bool isyaojia = false;
                if (isfuzhi)
                {
                    if (lisfuzhi[i] == "FuZhi")
                    {
                        isyaojia = true;
                    }
                }
                else
                {
                    if (lisfuzhi[i] != "FuZhi")
                    {
                        isyaojia = true;
                    }
                }
                if (isyaojia)
                {
                    this.commBoxE3.Items.Add(lisfuzhi[i]);
                }
            }
           
            this.textBox1.Text = xieDYModel.ZBLModel.ZBianLiangName;
            this.textBox7.Text = xieDYModel.ZBLModel.SheBeiID.ToString();
            this.commBoxE2.Text= xieDYModel.ZBLModel.ZLeiXing.ToString();
            this.commBoxE3.Text=xieDYModel.ZhongJianType.ToString();
            this.commBoxE1.Text = xieDYModel.YBLModel.ZLeiXing.ToString();
            this.textBox3.Text = xieDYModel.YBLModel.YouCanShu;
            this.textBox2.Text = xieDYModel.YBLModel.SheBeiID.ToString();
            this.checkBox1.Checked = xieDYModel.IsPingBi == 1;
        }

        public JiChuXieDYModel GetCanShu()
        {
            JiChuXieDYModel jiChuXieDYModel = new JiChuXieDYModel();
            jiChuXieDYModel.ZhongJianType = ChangYong.GetMeiJuZhi<ZhongJianType>(this.commBoxE3.Text); 
            jiChuXieDYModel.ZBLModel.ZBianLiangName = this.textBox1.Text;
            jiChuXieDYModel.ZBLModel.SheBeiID = ChangYong.TryInt(this.textBox7.Text,-1);
            jiChuXieDYModel.ZBLModel.ZLeiXing = this.commBoxE2.GetCanShu<ZblLeiXing>();
            jiChuXieDYModel.YBLModel.YouCanShu = this.textBox3.Text;
            jiChuXieDYModel.YBLModel.SheBeiID= ChangYong.TryInt(this.textBox2.Text, -1);
            jiChuXieDYModel.YBLModel.ZLeiXing = this.commBoxE1.GetCanShu<YblLeiXing>();
            jiChuXieDYModel.IsPingBi = this.checkBox1.Checked ? 1 : 0;
            return jiChuXieDYModel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = this.textBox1.Text;
            int shebeiid= ChangYong.TryInt(this.textBox7.Text, -1);
            ZblLeiXing zblLeiXing = this.commBoxE2.GetCanShu<ZblLeiXing>();
            switch (zblLeiXing)
            {
                case ZblLeiXing.HuanCunLiang:
                    {
                        XuanZeDuFrm frm = new XuanZeDuFrm();
                        frm.SetCanShu(name,4);
                        if (frm.ShowDialog(this)==DialogResult.OK)
                        {
                            this.textBox1.Text = frm.MingZi;
                        }
                    } 
                    break;
                case ZblLeiXing.DuIOKuai:
                    {
                        XuanZeDuFrm frm = new XuanZeDuFrm();
                        frm.SetCanShu(name, 1);
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {
                            this.textBox1.Text = frm.MingZi;
                        }
                    }
                    break;
                case ZblLeiXing.XieJC:
                    {
                        XuanZeJiCunQiFrm xuanZeJiCunQiFrm = new XuanZeJiCunQiFrm();
                        xuanZeJiCunQiFrm.SetCanShu(name, shebeiid, 2);
                        if (xuanZeJiCunQiFrm.ShowDialog(this) == DialogResult.OK)
                        {
                            this.textBox1.Text = xuanZeJiCunQiFrm.JiCunQiWeiYiBiaoShi;
                            int xinshebeiid = xuanZeJiCunQiFrm.SheBeiID;
                            this.textBox7.Text = xinshebeiid.ToString();
                        }
                    }
                    break;
                case ZblLeiXing.DuXieJCRec:
                    {
                        XuanZeJiCunQiFrm xuanZeJiCunQiFrm = new XuanZeJiCunQiFrm();
                        xuanZeJiCunQiFrm.SetCanShu(name, shebeiid, 2);
                        if (xuanZeJiCunQiFrm.ShowDialog(this) == DialogResult.OK)
                        {
                            this.textBox1.Text = xuanZeJiCunQiFrm.JiCunQiWeiYiBiaoShi;
                            int xinshebeiid = xuanZeJiCunQiFrm.SheBeiID;
                            this.textBox7.Text = xinshebeiid.ToString();
                        }
                    }
                    break;
                case ZblLeiXing.DuJC:
                    {
                        XuanZeJiCunQiFrm xuanZeJiCunQiFrm = new XuanZeJiCunQiFrm();
                        xuanZeJiCunQiFrm.SetCanShu(name, shebeiid, 1);
                        if (xuanZeJiCunQiFrm.ShowDialog(this) == DialogResult.OK)
                        {
                            this.textBox1.Text = xuanZeJiCunQiFrm.JiCunQiWeiYiBiaoShi;
                            int xinshebeiid = xuanZeJiCunQiFrm.SheBeiID;
                            this.textBox7.Text = xinshebeiid.ToString();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = this.textBox3.Text;
            int shebeiid = ChangYong.TryInt(this.textBox2.Text, -1);
            YblLeiXing zblLeiXing = this.commBoxE1.GetCanShu<YblLeiXing>();
            switch (zblLeiXing)
            {
                case YblLeiXing.HuanCunLiang:
                    {
                        XuanZeDuFrm frm = new XuanZeDuFrm();
                        frm.SetCanShu(name, 4);
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {
                            this.textBox3.Text = frm.MingZi;
                        }
                    }
                    break;
                case YblLeiXing.DuJC:
                    {
                        XuanZeJiCunQiFrm xuanZeJiCunQiFrm = new XuanZeJiCunQiFrm();
                        xuanZeJiCunQiFrm.SetCanShu(name, shebeiid, 1);
                        if (xuanZeJiCunQiFrm.ShowDialog(this) == DialogResult.OK)
                        {
                            this.textBox3.Text = xuanZeJiCunQiFrm.JiCunQiWeiYiBiaoShi;
                            int xinshebeiid = xuanZeJiCunQiFrm.SheBeiID;
                            this.textBox2.Text = xinshebeiid.ToString();
                        }
                    }
                    break;
                case YblLeiXing.ChangLiangZhi:
                    break;
                case YblLeiXing.LiangGeZhi:
                    break;
                default:
                    break;
            }
          
        }
    }
}
