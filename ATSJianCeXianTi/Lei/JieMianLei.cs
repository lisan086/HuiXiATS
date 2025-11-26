using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;

namespace ATSJianCeXianTi.Lei
{
    /// <summary>
    /// 给界面刷新用的
    /// </summary>
    public class JieMianLei
    {
       
        public DataJiHe DataJiHe { get; set; }
        public event Action<TangChuanUIModel, int, int> ChuLiUIEvent;
        /// <summary>
        /// 界面刷新事件
        /// </summary>
        public event Action<EventType, ShiJianModel> JieMianEvent;

        #region 单利
        private readonly static object _DuiXiang = new object();

        private static JieMianLei _LogTxt = null;



        private JieMianLei()
        {
           
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static JieMianLei Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new JieMianLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        /// <summary>
        /// 与界面沟通的事件
        /// </summary>
        public void ChuFaEvent(EventType eventType, ShiJianModel canshu,bool isyibu)
        {
            if (JieMianEvent!=null)
            {
                if (isyibu)
                {
                    JieMianEvent.BeginInvoke(eventType, canshu, null, null);
                }
                else
                {
                    JieMianEvent(eventType, canshu);
                }
            }
        }
        /// <summary>
        /// 界面操作
        /// </summary>
        /// <param name="doType"></param>
        /// <param name="mode"></param>
        public void CaoZuo(DoType doType, JieMianModel mode)
        {
            if (doType == DoType.Open)
            {
                DataJiHe = DataJiHe.Cerate();
                ZongYeWuLei.Cerate().Open();
            }
            else if (doType == DoType.Close)
            {
                ZongYeWuLei.Cerate().Close();
            }
            else if (doType == DoType.JiaZaiPeiFang)
            {
                ZongTestModel modess = null;
                bool zhen = DataJiHe.Cerate().JiaZaiPeiFang(mode.PeiFangName, mode.TDID, out modess);
                if (zhen)
                {
                    ZongYeWuLei.Cerate().CaoZuo(doType, mode);                 
                }

            }
            else
            {
                ZongYeWuLei.Cerate().CaoZuo(doType, mode);
            }
            
        }
        public ZongTestModel GetPeiFang(int tdid)
        {
            return DataJiHe.Cerate().GetTestPeiFang(tdid);
        }
        public List<TDModel> GetTongDao( )
        {
            List<TDModel> dModels = new List<TDModel>();
            foreach (var item in DataJiHe.Cerate().TDLisState.Keys)
            {
                dModels.Add(DataJiHe.Cerate().TDLisState[item]);
            }
            return dModels;
        }

        public void ChuFaUI(TangChuanUIModel lis, int type,int tdid)
        {
            if (ChuLiUIEvent != null)
            {
                ChuLiUIEvent(lis, type, tdid);
            }
        }
        public TDModel GetTongDao(int tdid)
        {
            List<TDModel> dModels = new List<TDModel>();
            foreach (var item in DataJiHe.Cerate().TDLisState.Keys)
            {
                return DataJiHe.Cerate().TDLisState[item];
            }
            return null;
        }


      
    }
}
