using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Model;

namespace ATSLaoHuaUI.UI
{
    public partial class GongWeiKJ : UserControl
    {
        private ZiYuanModel ZY;
        private SheBeiZhanModel SheBeiZhan;

        private int KongZhiShuaXin = 0;
        public GongWeiKJ()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
          
        }
        public void SetCanShu(SheBeiZhanModel model)
        {
            SheBeiZhan = model;
            this.label1.Text = SheBeiZhan.GaoWenName;
            this.label1.BackColor = Color.Green;
            this.gaoWenXiangZiKJ1.SetCanShu(model);
        }
        public void SetCanShu(ZiYuanModel ziYuanModel)
        {
            ZY = ziYuanModel;
            this.gaoWenXiangZiKJ1.SetCanShu(ziYuanModel);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (ZY.QuanXian.IsYouQuanXian("手动调试", out msg))
            {
                if (SheBeiZhan.TestState != 2)
                {
                    bool zhen= ZY.ShiYuFou("是否要手动测试");
                    if (zhen)
                    {
                        if (SheBeiZhan.IsKeYiShouDongTest == 1)
                        {
                            JieMianCaoZuoModel model = new JieMianCaoZuoModel();
                            model.GWID = SheBeiZhan.GWID;
                            model.CanShu = "";
                            JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.ShouDongLaoHua, model);
                        }
                        else
                        {
                            ZY.TiShiKuang("请扫码确认");
                        }
                    }
                }
                else
                {
                    ZY.TiShiKuang("设备正在工作");
                }
            }
            else
            {

                ZY.TiShiKuang(msg);
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (KongZhiShuaXin==1)
            {
                return;
            }
            string msg = "";
            if (ZY.QuanXian.IsYouQuanXian("调试", out msg))
            {
                int modshi = this.checkBox2.Checked ? 1 : 0;
                this.gaoWenXiangZiKJ1.SetMoShi(modshi);
            }
            else
            {
                KongZhiShuaXin = 1;
                this.checkBox2.Checked = false;
                this.gaoWenXiangZiKJ1.SetMoShi(0);
              
                ZY.TiShiKuang(msg);
                KongZhiShuaXin = 0;
            }

        }


        public void ShuaXin()
        {
            try
            {
                this.gaoWenXiangZiKJ1.ShuaXin();
                if (SheBeiZhan.IsGuZhang)
                {
                    if (this.label1.BackColor != Color.Red)
                    {
                        this.label1.BackColor = Color.Red;
                    }
                }
                else
                {
                    if (this.label1.BackColor != Color.Green)
                    {
                        this.label1.BackColor = Color.Green;
                    }
                }
            }
            catch
            {

            }
        }
     

        private void button2_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (ZY.QuanXian.IsYouQuanXian("手动调试", out msg))
            {
                bool zhen = ZY.ShiYuFou("是否要停止测试");
                if (zhen)
                {
                    JieMianCaoZuoModel model = new JieMianCaoZuoModel();
                    model.GWID = SheBeiZhan.GWID;
                    model.CanShu = "";
                    JieMianCaoZuoLei.CerateDanLi().JieMianCuoZuo(DoType.TingZhiLaoHua, model);
                }
            }
            else
            {

                ZY.TiShiKuang(msg);
            }
        }
    }
}
