using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using ATSJianMianJK.Mes;
using CommLei.JiChuLei;
using ZuZhuangUI.Lei.GongNengLei;
using ZuZhuangUI.Lei.GongNengLei.ShiXian;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei
{
    public  class ZongYeWuLei
    {
        private Dictionary<int, ABSGongNengLei> GongZhanLeilist = new Dictionary<int, ABSGongNengLei>();     
        #region 单例
        private static ZongYeWuLei _LogTxt = null;
        private readonly static object _DuiXiang = new object();

        public Dictionary<string, StringBuilder> CunCuoWuJiLu { get; internal set; }

        private ZongYeWuLei()
        {
           
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static ZongYeWuLei Ceratei()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)//线程锁
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

        //初始化
        public  void IniData()
        {

            //站点
            GongZhanLeilist.Clear();
           
            List<SheBeiZhanModel> zhan = DataJiHe.Cerate().LisSheBeiBianHao;//获取所有站点编号列表         
            for (int i = 0; i < zhan.Count; i++)
            {
                if (zhan[i].SheBeiType == SheBeiType.JuanChengLoaHua)
                {
                    ABSGongNengLei aBSGongNengLei = new GongWeiLei();
                    aBSGongNengLei.SheBeiID = zhan[i].GWID;
                    aBSGongNengLei.SheBeiName = zhan[i].GaoWenName;
                    aBSGongNengLei.SheBeiType = zhan[i].SheBeiType;
                    if (GongZhanLeilist.ContainsKey(zhan[i].GWID) == false)
                    {
                        aBSGongNengLei.IniData(zhan[i]);
                        GongZhanLeilist.Add(zhan[i].GWID, aBSGongNengLei);
                        ShangChuanMesLei.Cerate().ShuXinTdState(zhan[i].GWID, zhan[i].IsShangMes==1);
                    }
                }

            }       
        }

    

        /// <summary>
        /// 关闭，清理内存
        /// </summary>
        public void Close()
        {
            foreach (var item in GongZhanLeilist.Keys)
            {
                GongZhanLeilist[item].Close();

            }        
            Thread.Sleep(100);
         
        }


        /// <summary>
        /// 停止工作与开启工作
        /// </summary>
        /// <param name="istngzhi"></param>
        public void CaoZuoGongNeng(DoType doType,JieMianCaoZuoModel model)
        {
            if (model!=null)
            {
                if (GongZhanLeilist.ContainsKey(model.GWID))
                {
                    GongZhanLeilist[model.GWID].CaoZuo(doType, model);
                }
            }
        }
    }
}
