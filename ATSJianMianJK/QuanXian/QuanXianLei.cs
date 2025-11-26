using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianMianJK.QuanXian
{
    /// <summary>
    /// 权限类
    /// </summary>
    public class QuanXianLei
    {
        private QuanXianJK QuanXianJK;
        #region 单利
        private readonly static object _DuiXiang = new object();
        private static QuanXianLei _LogTxt = null;
        private QuanXianLei()
        {
            QuanXianJK = new ShiXianQXJK();
            QuanXianJK.IniData();
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static QuanXianLei CerateDanLi()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new QuanXianLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        /// <summary>
        /// 登录用的
        /// </summary>
        /// <param name="login"></param>
        /// <param name="mima"></param>
        /// <returns></returns>
        public bool DengLu(string login, string mima)
        {
            return QuanXianJK.DengLu(login, mima);
        }
      
        /// <summary>
        /// 校验权限的
        /// </summary>
        /// <param name="gongnengming"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool IsYouQuanXian(string gongnengming, out string msg)
        {
            msg = "";
            return QuanXianJK.IsYouQuanXian(gongnengming, out msg);
        }

        public string GetDengLuMing()
        {
            return QuanXianJK.GetDengLuMing();
        }
        public string GetLogin()
        {
            return QuanXianJK.GetYuanGongHao();
        }
    }
}
