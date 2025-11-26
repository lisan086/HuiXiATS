using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.QuanXian;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei
{
    /// <summary>
    /// 界面操作类
    /// </summary>
    public class JieMianCaoZuoLei
    {
        private DataJiHe _dataJiHe=DataJiHe.Cerate();
        public DataJiHe DataJiHe { get { return _dataJiHe; } }
        /// <summary>
        /// 用于界面的事件
        /// </summary>
        public event Action<EventType , JieMianShiJianModel> JieMianEvent;

        #region 单利
        private readonly static object _DuiXiang = new object();
        private static JieMianCaoZuoLei _LogTxt = null;
        private JieMianCaoZuoLei()
        {

        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static JieMianCaoZuoLei CerateDanLi()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new JieMianCaoZuoLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        #region 内部触发
        /// <summary>
        /// 触发界面的事件
        /// </summary>
        /// <param name="model"></param>
        public void ChuFaJieMianEvent(EventType eventType, JieMianShiJianModel model, bool istongbu = false)
        {
            if (istongbu)
            {
                if (JieMianEvent != null)
                {
                   
                    JieMianEvent(eventType, model);
                }
            }
            else
            {
                if (JieMianEvent != null)
                {
                 
                    JieMianEvent.BeginInvoke(eventType, model, null, null);
                }
            }

        }

        #endregion
        #region 界面调用的方法
        /// <summary>
        /// 界面可以操作方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="canshu"></param>
        /// <returns></returns>
        public void JieMianCuoZuo(DoType type, JieMianCaoZuoModel canshu)
        {
            if (type == DoType.Open)
            {
               
                ZongYeWuLei.Ceratei().IniData();
                SheBeiJiHe.Cerate().Open();

            }
            else if (type == DoType.Close)
            {
                _dataJiHe.Close();
                ZongYeWuLei.Ceratei().Close();
                SheBeiJiHe.Cerate().Close();
            }
            else if (type == DoType.HuanPeiFang)
            {
                ZongYeWuLei.Ceratei().Close();
                _dataJiHe.QiHuanPeiFang(canshu.CanShu.ToString());
                ZongYeWuLei.Ceratei().IniData();
            }
            else
            {
                ZongYeWuLei.Ceratei().CaoZuoGongNeng(type, canshu);
            }
        }

        #endregion

    }
}
