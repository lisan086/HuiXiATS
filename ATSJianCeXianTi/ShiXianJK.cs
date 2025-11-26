using ATSJianCeXianTi.JKKJ;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using ATSJianMianJK;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.QuanXian;
using CommLei.JiChuLei;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSJianCeXianTi
{
    public class ShiXianJK : ATSJieMianJK
    {
        private ZiYuanModel ZiYuanModel;
        private ZhuKJ ZhuKJ = null;
        public override void Close()
        {
            if (ZhuKJ!=null)
            {
                ZhuKJ.Close();
            }
        }

        public override string GetBiaoTi()
        {
            return "ATS测试";
        }

        public override List<QuanXianModel> GetQuanXian()
        {
            List<QuanXianModel> quanXianModels = new List<QuanXianModel>();
            quanXianModels.Add(new QuanXianModel() { GongNengDan="配置通道"});
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "调试功能" });
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "切换配方" });
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "编辑配方" });
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "换班" });
            quanXianModels.Add(new QuanXianModel() { GongNengDan = "更新探针" });
            return quanXianModels;
        }

        public override bool IsJieShouSouFang()
        {
            return false;
        }

        public override void ShuaXin()
        {
            if (ZhuKJ!=null)
            {
                ZhuKJ.ShuaXin();
            }
        }
        public override string GetBanBenHao()
        {
            return "V1.1";
        }

        public override Control LoadKJ()
        {
          
            ZhuKJ = new ZhuKJ(ZiYuanModel);
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
