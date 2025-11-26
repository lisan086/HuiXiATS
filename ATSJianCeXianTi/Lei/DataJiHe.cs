using ATSJianCeXianTi.JKKJ.PeiZhiKJ;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Lei
{
    public class DataJiHe : JiHeData
    {
        private DateTime JiShi = DateTime.Now;
        /// <summary>
        /// 通道状态 Key表示通道ID
        /// </summary>
        public Dictionary<int, TDModel> TDLisState = new Dictionary<int, TDModel>();

        /// <summary>
        /// 对应的key参数
        /// </summary>
        private List<int> TDKey = new List<int>();

        /// <summary>
        /// 存测试配方用的
        /// </summary>
        private Dictionary<int, ZongTestModel> TdTest = new Dictionary<int, ZongTestModel>();

        #region 单利
        private readonly static object _DuiXiang = new object();

        private static DataJiHe _LogTxt = null;



        private DataJiHe()
        {
            FenPeiData();
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static DataJiHe Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new DataJiHe();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion
        /// <summary>
        /// 子分配
        /// </summary>
        protected override void ZiFenPeiData()
        {
            List<TDModel> lismodel = HCLisDataLei<TDModel>.Ceratei().LisWuLiao;
            for (int i = 0; i < lismodel.Count; i++)
            {
                TDModel models = ChangYong.FuZhiShiTi(lismodel[i]);
                if (TDLisState.ContainsKey(models.TDID) == false)
                {
                    models.FuWeiCanShu = new FuWeiModel();
                    models.GanYingModel = new GanYingModel();
                    models.ShouDongCanShu = new ShouDongKongZhiModel();
                    TDLisState.Add(models.TDID, models);
                }
            }
            TDKey = TDLisState.Keys.ToList();
        }

        protected override void ZiClose()
        {
            TDLisState.Clear();
            TDKey.Clear();
            TdTest.Clear();
        }
        protected override void ZiShuaXinData(List<JiCunQiModel> lismodel)
        {
            for (int i = 0; i < TDKey.Count; i++)
            {
                if (TDLisState.ContainsKey(TDKey[i]))
                {
                    TDLisState[TDKey[i]].GanYingModel.IsJiTing = GetIOBool(TDKey[i], TDLisState[TDKey[i]].PeiZhiCanShu.JiTingName, false);
                    TDLisState[TDKey[i]].GanYingModel.QiDongTest = GetIOBool(TDKey[i], TDLisState[TDKey[i]].PeiZhiCanShu.QiDongName, false);
                    TDLisState[TDKey[i]].GanYingModel.WuPingZhuangTai = GetIOBool(TDKey[i], TDLisState[TDKey[i]].PeiZhiCanShu.WuPingStateName, false);
                    TDLisState[TDKey[i]].GanYingModel.QieHuanXingHao = GetShouGeStr(TDKey[i], TDLisState[TDKey[i]].PeiZhiCanShu.QieHuanXingHaoName, "");
                    TDLisState[TDKey[i]].GanYingModel.IsDianJian = GetIOBool(TDKey[i], TDLisState[TDKey[i]].PeiZhiCanShu.DuDianJianMoShi, false);
                }
            }
            try
            {
                if ((DateTime.Now- JiShi).TotalMilliseconds>=1500)
                {
                    Task.Factory.StartNew(() => {
                        JiShuGuanLiLei.Cerate().BaoCun();
                    });
                   
                    JiShi = DateTime.Now;
                }
            }
            catch
            {

             
            }
        }






        /// <summary>
        /// 加载配方
        /// </summary>
        public bool JiaZaiPeiFang(string peifangname, int tdid, out ZongTestModel model)
        {
            model = null;
            if (TDLisState.ContainsKey(tdid))
            {
                TDLisState[tdid].PeiZhiCanShu.PeiFangName = peifangname;
                PeiFangLei peiFangLei = new PeiFangLei();
                model = peiFangLei.JiaZaiPeiFang(peifangname);
                if (model != null)
                {
                    TDLisState[tdid].GanYingModel.PeiFangGengGai = true;
                    int xuehao = 1;
                    for (int i = 0; i < model.ZhongJianModels.Count; i++)
                    {
                        model.ZhongJianModels[i].TestModel.WeiZhi = xuehao;
                        model.ZhongJianModels[i].TestModel.XuHaoID = $"{xuehao}";
                        xuehao++;
                        bool iswuzu = false;
                        model.ZhongJianModels[i].TestModel.SheBeiID = SheBeiJiHe.Cerate().GetSheBeiID(model.ZhongJianModels[i].TestModel.SheBeiName, TDLisState[tdid].PeiZhiCanShu.SheBeiZu,out iswuzu);
                        model.ZhongJianModels[i].TestModel.IsWuZhuSheBei = iswuzu;
                    }
                    if (TdTest.ContainsKey(tdid) == false)
                    {
                        TdTest.Add(tdid, ChangYong.FuZhiShiTi(model));
                    }
                    else
                    {
                        TdTest[tdid] = ChangYong.FuZhiShiTi(model);
                    }
                    List<TDModel> lismodel = HCLisDataLei<TDModel>.Ceratei().LisWuLiao;
                    for (int i = 0; i < lismodel.Count; i++)
                    {
                        if (lismodel[i].TDID == tdid)
                        {
                            lismodel[i].PeiZhiCanShu.PeiFangName = peifangname;
                            lismodel[i].PeiZhiCanShu.JiaJuHao= model.JiaJuNames.Count>0 ? model.JiaJuNames[0]:"";
                            JiShuGuanLiLei.Cerate().IniData(tdid, lismodel[i].PeiZhiCanShu.PeiFangName, lismodel[i].PeiZhiCanShu.JiaJuHao);
                            break;
                        }
                    }
                    HCLisDataLei<TDModel>.Ceratei().BaoCun();

                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// 获取测试配方
        /// </summary>
        /// <param name="tdid"></param>
        /// <returns></returns>
        public ZongTestModel GetTestPeiFang(int tdid)
        {
            if (TdTest.ContainsKey(tdid))
            {
                return ChangYong.FuZhiShiTi(TdTest[tdid]);
            }
            return null;
        }


        public string IsCunZaiPeiFang(string ma,int qiehuantyype,out string yongdiaohao)
        {
            yongdiaohao = "";
            if (qiehuantyype == 1)
            {
                PeiFangLei peiFangLei = new PeiFangLei();
                List<string> lis = peiFangLei.GetPeiFangNames();
                for (int i = 0; i < lis.Count; i++)
                {
                    ZongTestModel model = peiFangLei.JiaZaiPeiFang(lis[i]);
                    if (model.PeiFangDuiYingMa[i].IsQieHuan == 1)
                    {
                        if (model.PeiFangDuiYingMa.Equals(ma))
                        {
                            yongdiaohao = ma;
                            return lis[i];
                        }
                    }
                }
            }
            else if (qiehuantyype==2)
            {
                PeiFangLei peiFangLei = new PeiFangLei();
                List<string> lis = peiFangLei.GetPeiFangNames();
                for (int i = 0; i < lis.Count; i++)
                {
                    ZongTestModel model = peiFangLei.JiaZaiPeiFang(lis[i]);
                    foreach (var item in model.PeiFangDuiYingMa)
                    {
                        if (item.IsQieHuan == 1)
                        {
                            if (item.Name.Contains(ma))
                            {
                                yongdiaohao = ma;
                                return lis[i];
                            }
                        }
                    }
                   
                }
            }       
            return "";
        }
       
    }
}
