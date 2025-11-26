using ATSJianCeXianTi.Model;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.Mes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Lei
{
    /// <summary>
    /// 总业务类
    /// </summary>
    public  class ZongYeWuLei
    {
        private Dictionary<int, TDTestLei> LisTD = new Dictionary<int, TDTestLei>();
        #region 单利
        private readonly static object _DuiXiang = new object();

        private static ZongYeWuLei _LogTxt = null;



        private ZongYeWuLei()
        {
            IniData();
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static ZongYeWuLei Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new ZongYeWuLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        public  void IniData()
        {
          
            if (LisTD.Count>0)
            {
                foreach (var item in LisTD.Keys)
                {
                    LisTD[item].Close();
                }
                LisTD.Clear();
            }
            List<int> tds = new List<int>();
            Dictionary<int, TDModel> TDs= DataJiHe.Cerate().TDLisState;
            foreach (var item in TDs.Keys)
            {
                TDModel model = TDs[item];
                TDTestLei tDTestLei = new TDTestLei(model);
                LisTD.Add(item,tDTestLei);
                ShangChuanMesLei.Cerate().ShuXinTdState(model.TDID, model.ShouDongCanShu.IsQiYongMes);
                tds.Add(item);
               
            }
          
            string msg = "";
            
            RiJiLog.Cerate().Add(RiJiEnum.MesData, msg, -1);
        }

        public void Open()
        {           
            SheBeiJiHe.Cerate().Open();
        }

        public void Close()
        {
            foreach (var item in LisTD.Keys)
            {
                LisTD[item].Close();
               
            }         
            SheBeiJiHe.Cerate().Close();
            DataJiHe.Cerate().Close();
        }


        /// <summary>
        /// 操作通道
        /// </summary>
        /// <param name="tDDoType"></param>
        /// <param name="model"></param>
        public void CaoZuo(DoType tDDoType, JieMianModel model)
        {
            if (LisTD.ContainsKey(model.TDID))
            {
                LisTD[model.TDID].CaoZuo(tDDoType, model);
            }
        }

       
    }
}
