using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.JiChuLei;

namespace ATSJianMianJK.QuanXian
{
    /// <summary>
    /// 权限接口
    /// </summary>
    internal interface QuanXianJK
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        void IniData();

        /// <summary>
        /// true表示登录成功
        /// </summary>
        /// <param name="zhanghao"></param>
        /// <param name="mima"></param>
        /// <returns></returns>
        bool DengLu(string zhanghao, string mima);

        /// <summary>
        /// true  表示有权限 传进来功能名称
        /// </summary>
        /// <param name="gongnengming"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool IsYouQuanXian(string gongnengming, out string msg);

        /// <summary>
        /// 获取登录名
        /// </summary>
        /// <returns></returns>
        string GetDengLuMing();

        string GetYuanGongHao();
    }

    internal class ShiXianQXJK : QuanXianJK
    {
        /// <summary>
        /// 人物权限的缓存
        /// </summary>
        private List<RenWuModel> LisRenWu = new List<RenWuModel>();
        /// <summary>
        /// 当前登录的人物
        /// </summary>
        private RenWuModel DanQianRenWuModel;
        public bool DengLu(string zhanghao, string mima)
        {
            for (int i = 0; i < LisRenWu.Count; i++)
            {
                if (LisRenWu[i].Login.Equals(zhanghao) && LisRenWu[i].MiMa.Equals(mima))
                {
                    DanQianRenWuModel = ChangYong.FuZhiShiTi(LisRenWu[i]);
                    return true;
                }
            }
            return false;
        }

        public string GetDengLuMing()
        {
            if (DanQianRenWuModel!=null)
            {
                return DanQianRenWuModel.XinMing;
            }
            return "";
        }

        public string GetYuanGongHao()
        {
            if (DanQianRenWuModel != null)
            {
                return DanQianRenWuModel.Login;
            }
            return "";
        }

        public void IniData()
        {
            LisRenWu = ChangYong.FuZhiShiTi(HCLisDataLei<RenWuModel>.Ceratei().LisWuLiao);
            {
                RenWuModel renwu = new RenWuModel();
                renwu.Login = "admin";
                renwu.MiMa = "admin123";
                renwu.XinMing = "最高管理员";
                renwu.ZhiWu = "最高管理员";
                renwu.RenWuID = 1;
                {
                    QuanXianModel model = new QuanXianModel();
                    model.GongNengDan = "全有";
                    model.IsYou = true;
                    renwu.QuanXianS.Add(model);
                }
                LisRenWu.Add(renwu);
            }

        }

        public bool IsYouQuanXian(string gongnengming, out string msg)
        {
            msg = "";
            if (DanQianRenWuModel == null || DanQianRenWuModel.QuanXianS.Count == 0)
            {
                msg = "该用户没有权限";
                return false;
            }
            for (int i = 0; i < DanQianRenWuModel.QuanXianS.Count; i++)
            {
                if (DanQianRenWuModel.QuanXianS[i].IsYou)
                {
                    if (DanQianRenWuModel.QuanXianS[i].GongNengDan.Equals("全有"))
                    {
                        return true;
                    }
                }
            }
            for (int i = 0; i < DanQianRenWuModel.QuanXianS.Count; i++)
            {
                if (DanQianRenWuModel.QuanXianS[i].IsYou)
                {
                    if (DanQianRenWuModel.QuanXianS[i].GongNengDan.Equals(gongnengming))
                    {
                        return true;
                    }
                }
            }
            msg = string.Format("{0}:没有该【{1}】权限", DanQianRenWuModel.XinMing, gongnengming);
            return false;
        }
    }

    /// <summary>
    /// 登录人物
    /// </summary>
    public class RenWuModel
    {

        public int RenWuID { get; set; } = 0;
        /// <summary>
        /// 登录名
        /// </summary>
        public string Login { get; set; } = "";
        /// <summary>
        /// 登录名
        /// </summary>
        public string MiMa { get; set; } = "";
        /// <summary>
        /// 姓名
        /// </summary>
        public string XinMing { get; set; } = "";

        /// <summary>
        /// 职位
        /// </summary>
        public string ZhiWu { get; set; } = "";

        /// <summary>
        /// 拥有的功能块
        /// </summary>
        public List<QuanXianModel> QuanXianS { get; set; } = new List<QuanXianModel>();
    }
    /// <summary>
    /// 功能菜单
    /// </summary>
    public class QuanXianModel
    {
        /// <summary>
        /// 功能名称
        /// </summary>
        public string GongNengDan { get; set; } = "";
        /// <summary>
        /// true表示全有
        /// </summary>
        public bool IsYou { get; set; } = false;
    }
}
