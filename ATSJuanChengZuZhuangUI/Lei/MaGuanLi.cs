using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei
{
    /// <summary>
    /// 码的管理类
    /// </summary>
    public class MaGuanLi
    {
        private string LuJing { get; set; } = "";
        public MaGuanLiModel DanQianMa { get; set; } = null;
        #region 单利
        private readonly static object _DuiXiang = new object();
        private static MaGuanLi _LogTxt = null;
        private MaGuanLi()
        {
            GetLuJing();
            ShuaXinDanQian();
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
            JosnOrModel josnOrModel = new JosnOrModel(LuJing);
            List<MaGuanLiModel> lis = josnOrModel.GetLisTModel<MaGuanLiModel>();
            return lis;
        }

        public void ShuaXinDanQian()
        {
            DanQianMa = null;
            List<MaGuanLiModel> maGuanLiModels = GetMaZiLiao();
            for (int i = 0; i < maGuanLiModels.Count; i++)
            {
                if (maGuanLiModels[i].DanQianShiYong)
                {
                    DanQianMa = maGuanLiModels[i];
                    break;
                }
            }
        }


        public ShuChuMaShuJuModel GetMaShuJu(bool isbaocun=true)
        {
            if (DanQianMa == null)
            {
                RiJiLog.Cerate().Add(RiJiEnum.MesData, "没有设置二维码", -1);
                return null;
            }
            else
            {
                
                if (isbaocun)
                {
                    bool isyouriqi = IsYouRiQi();
                    if (isyouriqi)
                    {
                        string xianzairiqi = DateTime.Now.ToString("yyyyMMdd");
                        if (DanQianMa.DanTian.Equals(xianzairiqi)==false)
                        {
                            DanQianMa.DanTian = xianzairiqi;
                            DanQianMa.MuQianWeiZhi = 1;
                        }
                    }
                }
                List<string> mas = new List<string>();
                int muqianweizhi = DanQianMa.MuQianWeiZhi;
                int count = DanQianMa.YiGongYong;
                int weishu = DanQianMa.MouChangDu;             
                for (int i = 0; i < count; i++)
                {
                    int xindema = muqianweizhi;
                    if (xindema.ToString().Length<= weishu)
                    {
                        List<string> ms= ShengChengMa(xindema);
                        mas.Add($"{ChangYong.FenGeDaBao(ms, "")}");
                    }
                    muqianweizhi++;
                }
                ShuChuMaShuJuModel model = new ShuChuMaShuJuModel();
                Type sd=model.GetType();
                PropertyInfo[] shuxin = sd.GetProperties();
                foreach (var item in shuxin)
                {
                    for (int i = 0; i < shuxin.Length; i++)
                    {
                        if (item.Name.Equals($"Ma{i+1}"))
                        {
                            if (i< mas.Count)
                            {
                                item.SetValue(model, mas[i]);
                            }
                            break;
                        }
                    }
                }
                if (isbaocun)
                {
                    DanQianMa.MuQianWeiZhi = muqianweizhi;
                    SetDuiXiang(DanQianMa);
                }
                return model;

            }
        }

        /// <summary>
        /// true表示正确
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        public bool JiaoYanShiFouGaiMa(string ma)
        {
            if (DanQianMa!=null)
            {
                List<string> shuju = ShengChengMa(1);
                int jishu = 0;
                for (int i = 0; i < shuju.Count; i++)
                {
                    if (ma.Contains(shuju[i]))
                    {
                        jishu++;
                    }
                }
                if (jishu>=3)
                {
                    return true;
                }
              
            }
            return false;
        }

        public void SetDuiXiang(MaGuanLiModel model)
        {
            List<MaGuanLiModel> maGuanLiModels = GetMaZiLiao();
            bool iscunzai = false;
            for (int i = 0; i < maGuanLiModels.Count; i++)
            {
                if (maGuanLiModels[i].MaName.Equals(model.MaName))
                {
                    maGuanLiModels[i]= model;
                    iscunzai = true;
                    break;
                }
            }
            if (iscunzai==false)
            {
                maGuanLiModels.Add(model);
            }
            if (model.DanQianShiYong)
            {
                for (int i = 0; i < maGuanLiModels.Count; i++)
                {
                    if (maGuanLiModels[i].MaName.Equals(model.MaName) ==false)
                    {
                        maGuanLiModels[i].DanQianShiYong=false;
                       
                    }
                }
            }

            JosnOrModel josnOrModel = new JosnOrModel(LuJing);
            josnOrModel.XieTModel(maGuanLiModels);
            ShuaXinDanQian();
        }

        public string GetMaWenJianPath()
        {
            string text = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "dayingmoban.txt");
            return text;
        }
        private void GetLuJing()
        {

            string text = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "PeiZhi");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            string name = "MaGuanLiModel";
            string path = string.Format("{0}{1}Lis{2}.txt", text, "\\", name);

            LuJing = path;
        }

        private List<string> ShengChengMa(int liushuihao)
        {
            List<string> masw = new List<string>() { "", "", "", "" };
            List<string> mas = ChangYong.MeiJuLisName(typeof(MaaType));
            for (int i = 0; i < 4; i++)
            {
                string bianmane = i == 0 ? DanQianMa.ShouBuBianShi1 : i == 1 ? DanQianMa.ShouBuBianShi2 : i == 2 ? DanQianMa.ShouBuBianShi3 : DanQianMa.ShouBuBianShi4;
                if (mas.IndexOf(bianmane) >= 0)
                {
                    MaaType type = ChangYong.GetMeiJuZhi<MaaType>(bianmane);
                    switch (type)
                    {
                        case MaaType.RiQiNianYueRi:
                            {
                                masw[i] = DateTime.Now.ToString("yyyyMMdd");
                            }
                            break;
                        case MaaType.LiuShuiHao:
                            {
                                int weishu = DanQianMa.MouChangDu;
                                masw[i] = liushuihao.ToString().PadLeft(weishu, '0');
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    masw[i] = bianmane;
                }
            }
          
            return masw;
        }

        private bool IsYouRiQi()
        {
         
            List<string> mas = ChangYong.MeiJuLisName(typeof(MaaType));
            for (int i = 0; i < 4; i++)
            {
                string bianmane = i == 0 ? DanQianMa.ShouBuBianShi1 : i == 1 ? DanQianMa.ShouBuBianShi2 : i == 2 ? DanQianMa.ShouBuBianShi3 : DanQianMa.ShouBuBianShi4;
                if (mas.IndexOf(bianmane) >= 0)
                {
                    MaaType type = ChangYong.GetMeiJuZhi<MaaType>(bianmane);
                    switch (type)
                    {
                        case MaaType.RiQiNianYueRi:
                            {
                                return true;
                            }
                         
                      
                        default:
                            break;
                    }
                }
              
            }

            return false;
        }
    }
}
