using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using ATSJuanChengZuZhuangUI.Model;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using Common.DataChuLi;
using Common.JiChuLei;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei
{
    /// <summary>
    /// 码的管理类
    /// </summary>
    public class MaGuanLi
    {
        private Dictionary<int, FanXingJiHeLei<List<ShuChuMaModel>>> ShengChengMa = new Dictionary<int, FanXingJiHeLei<List<ShuChuMaModel>>>();
        private Dictionary<int,bool> IsXuQingLing { get; set; } = new Dictionary<int, bool>();
        private int Day = 0;
        private string LuJing { get; set; } = "";
        private  Dictionary<int, MaGuanLiModel> DanQianMa { get; set; } = new Dictionary<int, MaGuanLiModel>();
        private readonly static object _DuiXiang1 = new object();
        #region 单利
        private readonly static object _DuiXiang = new object();
        private static MaGuanLi _LogTxt = null;
        private MaGuanLi()
        {
          
          
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static MaGuanLi CerateDanLi()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new MaGuanLi();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion
        /// <summary>
        /// 获取所有码的资料
        /// </summary>
        /// <returns></returns>
        public List<MaGuanLiModel> GetMaZiLiao()
        {         
            List<MaGuanLiModel> lis = HCLisDataLei<MaGuanLiModel>.Ceratei().LisWuLiao;
            return lis;
        }

        public bool SetDanQian(int tdid, string mingcheng)
        {
            ShengChengMa.Clear();
            IsXuQingLing.Clear();
            DanQianMa.Clear();
            List<MaGuanLiModel> maGuanLiModels = GetMaZiLiao();
            for (int i = 0; i < maGuanLiModels.Count; i++)
            {
                if (maGuanLiModels[i].MaName.Equals(mingcheng))
                {
                    if (DanQianMa.ContainsKey(tdid)==false)
                    {
                        DanQianMa.Add(tdid, maGuanLiModels[i]);
                        IsXuQingLing.Add(tdid,false);
                        ShengChengMa.Add(tdid,new FanXingJiHeLei<List<ShuChuMaModel>>());
                    }
                    return true;
                }
            }

            return false;
        }


        public List<ShuChuMaModel> GetMaShuJu(int tdid, bool isbaocun=true)
        {
            IsYaoQingLi();
            if (DanQianMa.ContainsKey(tdid) == false)
            {
                RiJiLog.Cerate().Add(RiJiEnum.XiTongXie, "没有设置二维码", -1);
                return new List<ShuChuMaModel>();
            }
            else
            {
                List<ShuChuMaModel> mas = new List<ShuChuMaModel>();
                MaGuanLiModel xin = DanQianMa[tdid];
                int count = xin.Count;
                if (IsXuQingLing[tdid])
                {
                    IsXuQingLing[tdid] = false;
                    for (int i = 0; i < xin.LisGuiZe.Count; i++)
                    {
                        if (xin.LisGuiZe[i].MaaType == MaaType.LiuShuiHao)
                        {
                            xin.LisGuiZe[i].JilvWeiZhi = 0;
                        }
                    }
                }
                int xujia = 0;
                for (int i = 0; i < count; i++)
                {
                    ShuChuMaModel mamodel = new ShuChuMaModel();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in xin.LisGuiZe)
                    {
                        string shuju = "";
                        if (item.MaaType == MaaType.GuDing)
                        {
                            shuju = item.Zhi;
                            sb.Append(item.Zhi);
                        }
                        else if (item.MaaType == MaaType.LiuShuiHao)
                        {
                            xujia++;
                            if (isbaocun)
                            {
                                item.JilvWeiZhi++;
                            }
                            int xindema = isbaocun? item.JilvWeiZhi: xujia;
                            if (xindema.ToString().Length <= item.ChangDu)
                            {
                                shuju = xindema.ToString().PadLeft(item.ChangDu, '0');
                                sb.Append(shuju);
                            }
                            else
                            {
                                RiJiLog.Cerate().Add(RiJiEnum.XiTongXie, "流水号已经用完", -1);
                                return new List<ShuChuMaModel>();
                            }



                        }
                        else if (item.MaaType == MaaType.RiQiNianYueRi)
                        {
                            shuju = DateTime.Now.ToString("yyyyMMdd");
                            sb.Append(shuju);
                        }

                        mamodel.LisMa.Add(shuju);
                    }
                    mamodel.Ma = sb.ToString();
                    mas.Add(mamodel);
                }
                if (isbaocun)
                {
                    ShengChengMa[tdid].Add(mas);
                    BaoCun();
                }
                return mas;

            }
        }


        public bool JiaoYanMaDuiFou(ShuChuMaModel ma,int tdid)
        {
            if (ShengChengMa.ContainsKey(tdid))
            {
                FanXingJiHeLei<List<ShuChuMaModel>> lisd = ShengChengMa[tdid];
                List<ShuChuMaModel> lise = lisd.GetModel_Head_RomeHead();
                if (lise != null)
                {
                    for (int i = 0; i < lise.Count; i++)
                    {
                        ShuChuMaModel shuju = lise[i];
                        string xianzai = shuju.ToString();
                        string yuanlais = ma.ToString();
                        if (xianzai.Equals(yuanlais))
                        {
                            return true;
                        }
                    }
                    RiJiLog.Cerate().Add(RiJiEnum.XiTongXie, $"数据不一致:{ChangYong.HuoQuJsonStr(lise)},{ChangYong.HuoQuJsonStr(ma)}", -1);
                    return false;
                }
                else
                {
                    RiJiLog.Cerate().Add(RiJiEnum.XiTongXie, "还没有增加数据", -1);
                    return false;
                }
            }
            RiJiLog.Cerate().Add(RiJiEnum.XiTongXie, "码管理不存在该数据", -1);
            return false;
        }

        private void BaoCun()
        {
            HCLisDataLei<MaGuanLiModel>.Ceratei().BaoCun();
        }

       
        public void ZengJiaBaoCun(MaGuanLiModel model)
        {
            List<MaGuanLiModel> lis = HCLisDataLei<MaGuanLiModel>.Ceratei().LisWuLiao;
            bool zhen = false;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].MaName.Equals(model.MaName))
                {
                    lis[i].LisGuiZe = model.LisGuiZe;
                    zhen = true;
                    break;
                }
            }
            if (zhen==false)
            {
                HCLisDataLei<MaGuanLiModel>.Ceratei().LisWuLiao.Add(model);
            }
             HCLisDataLei<MaGuanLiModel>.Ceratei().BaoCun();
        }
        private void IsYaoQingLi()
        {
            lock (_DuiXiang1)
            {
                bool isbaocun = false;
                Day = DateTime.Now.Day;
                int tianshu = HCDanGeDataLei<QingKongJiLvModel>.Ceratei().LisWuLiao.Tian;
                if (tianshu != Day)
                {
                    List<int> shuju = DanQianMa.Keys.ToList();
                    for (int i = 0; i < shuju.Count; i++)
                    {
                        IsXuQingLing[shuju[i]] = true;
                    }
                    isbaocun = true;
                }
               
                HCDanGeDataLei<QingKongJiLvModel>.Ceratei().LisWuLiao.Tian = Day;
                if (isbaocun)
                {
                    HCDanGeDataLei<QingKongJiLvModel>.Ceratei().BaoCun();
                }
               
            }
           
        }
    }
}
