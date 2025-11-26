using ATSJianCeXianTi.Model;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.Model;
using SSheBei.ZongKongZhi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Lei
{
    public class SheBeiJiHe
    {
        /// <summary>
        /// 线程总开关
        /// </summary>
        private bool ZongKaiGuan = true;

      
        #region 单利
        private readonly static object _DuiXiang = new object();

        private static SheBeiJiHe _LogTxt = null;



        private SheBeiJiHe()
        {
            IniData();

        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static SheBeiJiHe Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new SheBeiJiHe();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        private void IniData()
        {
            JiHeSheBei.Cerate().JiHeData = DataJiHe.Cerate();
            JiHeSheBei.Cerate().XieBuZhou = new XieBuZhou();
            JiHeSheBei.Cerate().GaiBianEvent += SheBeiJiHe_GaiBianEvent;
           
        }

        private void SheBeiJiHe_GaiBianEvent(int tdid, IOType iotype, string ioname, bool state, string canshu)
        {
            if (iotype == IOType.IOPingZhengCount)
            {
                if (state)
                {
                    int type = ChangYong.TryInt(canshu,1);
                    JiShuGuanLiLei.Cerate().SetJiaJuCount(tdid, type);
                }
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            JiHeSheBei.Cerate().Open();
          
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(100);
            JiHeSheBei.Cerate().Close();
        }

       

        #region 配置的写
        public void XieShuJu(XieSateType xingType, int td)
        {
            XieSateModel model = DataJiHe.Cerate().GetXieModel(td, xingType);
            if (model != null)
            {
                JiHeSheBei.Cerate().XieChengGong(td, model.LisXies);
            }
        }

        public string XieShuSaoMa(string xiename,string duname,string shangcima, int td)
        {
            int cishu = 3;
            XieSateModel model = DataJiHe.Cerate().GetXieModel(td,xiename);
            if (model!=null)
            {
                for (int i = 0; i < cishu; i++)
                {
                    JiHeSheBei.Cerate().XieChengGong(td, model.LisXies);
                    double yanshishjian = model.Miao;
                    DateTime shijian = DateTime.Now;
                    for (; ZongKaiGuan;)
                    {
                        Thread.Sleep(3);
                        List<DuModel> lisd = DataJiHe.Cerate().GetDuZhi(td, duname);
                        if (lisd.Count == 0)
                        {
                            break;
                        }
                        string zhi = DataJiHe.Cerate().GetStr(lisd[0], "");
                        if (string.IsNullOrEmpty(zhi) == false)
                        {
                            if (i < cishu - 1)
                            {
                                if (zhi.Equals(shangcima) == false)
                                {
                                    return zhi;
                                }
                            }
                            else
                            {
                                return zhi;
                            }
                        }
                        if ((DateTime.Now - shijian).TotalMilliseconds >= yanshishjian*1000)
                        {
                            break;
                        }
                        Thread.Sleep(1);
                    }
                }
              
            }
           
            return "";
        }
        #endregion

   

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="cmdsend"></param>
        /// <param name="canshu"></param>
        public void XieShuJu(int tdid, XieRuMolde model)
        {
            XieRuMolde xieRuMolde =model.FuZhi();

            JiHeSheBei.Cerate().XieShuJu(tdid,model);
        }

        public JiaoYanJieGuoModel GetXieJieGuo(XieRuMolde model)
        {
           
            JiaoYanJieGuoModel jieguo = JiHeSheBei.Cerate().GetXieJieGuo(model);
            return jieguo;
        }

        public int GetSheBeiID(string sfname)
        {
            List<JiaZaiSheBeiModel> lis = HCLisDataLei<JiaZaiSheBeiModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].SheBeiName == sfname)
                {
                    return lis[i].SheBeiID;
                }
            }
            return -1;
        }

        public int GetSheBeiID(string sfname,string shebeizu,out bool iswuzhu)
        {
            iswuzhu = false;
            List<JiaZaiSheBeiModel> lis = HCLisDataLei<JiaZaiSheBeiModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].SheBeiName == sfname)
                {
                    if (lis[i].SheBeiZu.Contains("无"))
                    {
                        iswuzhu = true;
                        return lis[i].SheBeiID;
                    }
                    else
                    {
                        if (lis[i].SheBeiZu==shebeizu)
                        {
                            return lis[i].SheBeiID;
                        }
                    }
                }
            }
            return -1;
        }
    }
}
