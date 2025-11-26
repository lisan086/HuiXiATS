using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianCeXianTi.Model;

using CommLei.DataChuLi;


namespace ATSJianCeXianTi.Lei
{
    /// <summary>
    /// 计数管理
    /// </summary>
    public class JiShuGuanLiLei
    {

        #region 单利
        private readonly static object _DuiXiang = new object();

        private static JiShuGuanLiLei _LogTxt = null;



        private JiShuGuanLiLei()
        {
           
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static JiShuGuanLiLei Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new JiShuGuanLiLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion
        public void IniData(int tds, string peifangname, string jiajuhao)
        {
            {
                List<TDTianShuModel> lisdatas = HCLisDataLei<TDTianShuModel>.Ceratei().LisWuLiao;
                bool zhens = false;
                for (int i = 0; i < lisdatas.Count; i++)
                {
                    if (lisdatas[i].TDID == tds)
                    {
                        if (lisdatas[i].Time == DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            zhens = true;
                            break;
                        }
                    }

                }
                if (zhens == false)
                {
                    TDTianShuModel models = new TDTianShuModel();
                    models.TDID = tds;
                    models.Time = DateTime.Now.ToString("yyyy-MM-dd");
                    HCLisDataLei<TDTianShuModel>.Ceratei().LisWuLiao.Add(models);
                }
            }
            {
                if (string.IsNullOrEmpty(jiajuhao) == false)
                {
                    List<GongZhuanModel> lis = HCLisDataLei<GongZhuanModel>.Ceratei().LisWuLiao;
                    bool iscunzai = false;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].TDID == tds)
                        {
                            if (lis[i].GongZhuangName == jiajuhao)
                            {
                                lis[i].IsZhengZaiYong = 1;
                                iscunzai = true;
                            }
                            else
                            {
                                lis[i].IsZhengZaiYong = 0;
                            }
                        }

                    }
                    if (iscunzai == false)
                    {
                        GongZhuanModel model = new GongZhuanModel();
                        model.TDID = tds;
                        model.GongZhuangName = jiajuhao;
                        model.IsZhengZaiYong = 1;
                        model.JianLiTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        model.ZhenModels.Add(new ZhenModel() { IsGaoPin = 1 });
                        model.ZhenModels.Add(new ZhenModel() { IsGaoPin = 2 });
                        HCLisDataLei<GongZhuanModel>.Ceratei().LisWuLiao.Add(model);

                    }
                }
            }
            {
                List<TDJiShuModel> lis = HCLisDataLei<TDJiShuModel>.Ceratei().LisWuLiao;
                bool zhens = false;
                for (int c = 0; c < lis.Count; c++)
                {
                    if (lis[c].TDID == tds)
                    {

                        zhens = true;
                        break;

                    }
                }
                if (zhens == false)
                {
                    TDJiShuModel models = new TDJiShuModel();
                    models.TDID = tds;
                    HCLisDataLei<TDJiShuModel>.Ceratei().LisWuLiao.Add(models);

                }
            }
            {
                if (string.IsNullOrEmpty(peifangname) == false)
                {
                    List<PeiFangJiShuModel> lis = HCLisDataLei<PeiFangJiShuModel>.Ceratei().LisWuLiao;
                    bool iscunzai = false;
                    for (int i = 0; i < lis.Count; i++)
                    {
                        if (lis[i].TDID == tds)
                        {
                            if (lis[i].PeiFangName == peifangname)
                            {
                                iscunzai = true;
                                lis[i].IsZhengYong = 1;
                            }
                            else
                            {
                                lis[i].IsZhengYong = 0;
                            }
                        }
                    }
                    if (iscunzai == false)
                    {
                        PeiFangJiShuModel model = new PeiFangJiShuModel();
                        model.TDID = tds;
                        model.PeiFangName = peifangname;
                        model.IsZhengYong = 1;
                        HCLisDataLei<PeiFangJiShuModel>.Ceratei().LisWuLiao.Add(model);
                    }
                }
            }
        }


        public void SetHuanBan(int tdtds)
        {
            {
                List<TDJiShuModel> lis = HCLisDataLei<TDJiShuModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lis.Count; i++)
                {
                    if (lis[i].TDID == tdtds)
                    {
                        lis[i].HBHeGeCount = 0;
                        lis[i].HBNGCount = 0;
                      
                        break;
                    }
                }
            }
        }
        public void BaoCun()
        {

            HCLisDataLei<GongZhuanModel>.Ceratei().BaoCun();


            HCLisDataLei<TDJiShuModel>.Ceratei().BaoCun();


            HCLisDataLei<PeiFangJiShuModel>.Ceratei().BaoCun();
            HCLisDataLei<TDTianShuModel>.Ceratei().BaoCun();
        }

        /// <summary>
        /// 1是高频针
        /// </summary>
        /// <param name="tdtds"></param>
        /// <param name="zhentype"></param>
        public void SetJiaJuCount(int tdtds, int zhentype)
        {
            List<GongZhuanModel> lis = HCLisDataLei<GongZhuanModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].IsZhengZaiYong == 1 && lis[i].TDID == tdtds)
                {
                    lis[i].SetShuJu(zhentype);
                    break;
                }
            }
        }

        public void SetTDCount(int tdtds, bool ishege)
        {
            {
                List<TDJiShuModel> lis = HCLisDataLei<TDJiShuModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lis.Count; i++)
                {
                    if (lis[i].TDID == tdtds)
                    {
                        if (ishege)
                        {
                            lis[i].ZHeGeCount++;
                            lis[i].HBHeGeCount++;
                        }
                        else
                        {
                            lis[i].HBNGCount++;
                            lis[i].ZNGCount++;
                        }
                        break;
                    }
                }
            }
            {

                List<TDTianShuModel> lisdatas = HCLisDataLei<TDTianShuModel>.Ceratei().LisWuLiao;
                bool zhens = false;
                for (int i = 0; i < lisdatas.Count; i++)
                {
                    if (lisdatas[i].TDID == tdtds)
                    {
                        if (lisdatas[i].Time == DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            lisdatas[i].SetShuJu(ishege);
                            zhens = true;
                            break;
                        }
                    }

                }
                if (zhens == false)
                {
                    TDTianShuModel models = new TDTianShuModel();
                    models.TDID = tdtds;
                    models.Time = DateTime.Now.ToString("yyyy-MM-dd");
                    models.SetShuJu(ishege);
                    HCLisDataLei<TDTianShuModel>.Ceratei().LisWuLiao.Add(models);
                }

            }
        }

        public void SetPeiFangCount(int tdtds, bool ishege)
        {
            List<PeiFangJiShuModel> lis = HCLisDataLei<PeiFangJiShuModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].TDID == tdtds && lis[i].IsZhengYong == 1)
                {
                    lis[i].SetShuJu(ishege);
                    break;
                }
            }
        }

        public List<ZhenModel> GetTangZhengCount(int tdtds)
        {
            List<GongZhuanModel> lis = HCLisDataLei<GongZhuanModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].IsZhengZaiYong == 1 && lis[i].TDID == tdtds)
                {
                    return lis[i].ZhenModels;
                }
            }
            return new List<ZhenModel>();
        }

        public TDJiShuModel GetTDCount(int tdtds)
        {
            List<TDJiShuModel> lisdatas = HCLisDataLei<TDJiShuModel>.Ceratei().LisWuLiao;
          
            for (int i = 0; i < lisdatas.Count; i++)
            {
                if (lisdatas[i].TDID == tdtds)
                {
                    return lisdatas[i];
                }

            }
            return null;
        }

        public PeiFangJiShuModel GetPeiFangCount(int tdtds)
        {
            List<PeiFangJiShuModel> lisdatas = HCLisDataLei<PeiFangJiShuModel>.Ceratei().LisWuLiao;

            for (int i = 0; i < lisdatas.Count; i++)
            {
                if (lisdatas[i].TDID == tdtds&& lisdatas[i].IsZhengYong==1)
                {
                    return lisdatas[i];
                }

            }
            return null;
        }

        public void SetTangZheng(int tdtds,int type)
        {
            List<GongZhuanModel> lis = HCLisDataLei<GongZhuanModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].IsZhengZaiYong == 1 && lis[i].TDID == tdtds)
                {
                     lis[i].GengHuanTangZheng(type);
                }
            }
          
        }
    }
}
