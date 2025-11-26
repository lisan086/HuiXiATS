using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using ATSJianMianJK.QuanXian;
using ATSJianMianJK;
using System.Windows.Forms;
using ZuZhuangUI.UI;

namespace ZuZhuangUI
{
    public class ShiXianJK : ATSJieMianJK
    {
        private ZiYuanModel ZiYuanModel;
        private ZhuJieMian ZhuKJ = null;
        public override void Close()
        {
            if (ZhuKJ != null)
            {
                ZhuKJ.Close();
            }
        }

        public override string GetBiaoTi()
        {
            return "老化";
        }

        public override List<QuanXianModel> GetQuanXian()
        {
            List<QuanXianModel> quanXianModels = new List<QuanXianModel>();
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "配置通道" });
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "手动调试" });
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "切换配方" });
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "调试" });
       
            return quanXianModels;
        }

        public override bool IsJieShouSouFang()
        {
            return false;
        }

        public override string GetBanBenHao()
        {
            return "V1.1";
        }

        public override Control LoadKJ()
        {

            ZhuKJ = new ZhuJieMian(ZiYuanModel);
            return ZhuKJ;
        }

        public override void QieHuanYongHu()
        {
            if (ZhuKJ != null)
            {
                ZhuKJ.QieHuanYongHu();
            }
        }

        public override void SetCanShu(ZiYuanModel ziYuanModel)
        {
            ZiYuanModel = ziYuanModel;
        }

        public override void SetLog(List<RiJiModel> lismodel)
        {
            if (ZhuKJ != null)
            {
                ZhuKJ.SetLog(lismodel);
            }
        }


    }
}
