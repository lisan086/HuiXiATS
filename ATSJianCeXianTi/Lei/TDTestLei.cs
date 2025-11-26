using ATSJianCeXianTi.GongNengLei;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.Mes;
using ATSJianMianJK.XiTong.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Lei
{
   
    /// <summary>
    /// 通道测试类
    /// </summary>
    internal class TDTestLei
    {
        /// <summary>
        /// 线程开关
        /// </summary>
        protected bool ZongKaiGuan = true;
        /// <summary>
        /// 检测项目调用的函数
        /// </summary>
        private JianCeDuiXiang JianCeDuiXiang = null;
    
        /// <summary>
        /// 测试的内容
        /// </summary>
        private ZongTestModel ZongTestModel = null;

        private List<string> BuHeGeXiangMu = new List<string>();
       
        /// <summary>
        /// 通道参数
        /// </summary>
        private TDModel TDModel = new TDModel();
        private BaoCunXlxs DaoChuTxlxs = new BaoCunXlxs();
      
        public TDTestLei(TDModel tDmodel)
        {
            JianCeDuiXiang = new JianCeDuiXiang(tDmodel.TDID);
            TDModel = tDmodel;
            HuanCunLei.Cerate().Clear(tDmodel.TDID);
            JianCeDuiXiang.TDID=tDmodel.TDID;
            JianCeDuiXiang.ChuLiUIEvent += JianCeDuiXiang_ChuLiUIEvent;
            Task.Factory.StartNew(() => {Thread.Sleep(3000); JiHeSheBei.Cerate().XieChengGong(TDModel.TDID, TDModel.PeiZhiCanShu.MesDuState); });
            Thread thread = new Thread(Work);
            thread.IsBackground = true;
            thread.DisableComObjectEagerCleanup();
            thread.Start();
           
        }

        private void JianCeDuiXiang_ChuLiUIEvent(TangChuanUIModel canshu, int type, int tdid)
        {
            JieMianLei.Cerate().ChuFaUI(canshu,type,tdid);
        }

        /// <summary>
        /// 自动测试用的
        /// </summary>
        private void Work()
        {
            int tdid = TDModel.TDID;
            int jitingcount = 0;
            int zhanting = 0;
            DateTime JiShiTime = DateTime.Now;
            int shoci = 0;
            int ff = 0;
            while (ZongKaiGuan)
            {
                if (TDModel.GanYingModel.IsJiTing)
                {
                    if (jitingcount == 0)
                    {
                        jitingcount = 1;
                        LogMsg(RiJiEnum.TDXieJiLu, $"急停");
                    }
                    Thread.Sleep(10);
                    continue;
                }
                if (TDModel.ShouDongCanShu.IsZhanTing)
                {
                    if (zhanting == 0)
                    {
                        zhanting = 1;
                        LogMsg(RiJiEnum.TDXieJiLu, $"暂停");
                    }
                    Thread.Sleep(10);
                    continue;
                }
                if (jitingcount == 1)
                {
                    jitingcount = 0;
                    LogMsg(RiJiEnum.TDXieJiLu, $"取消急停");
                }
                if (zhanting == 1)
                {
                    zhanting = 0;
                    LogMsg(RiJiEnum.TDXieJiLu, $"取消暂停");
                }
                try
                {              
                    if (TDModel.ShouDongCanShu.IsTiaoShi)
                    {
                        for (; TDModel.ShouDongCanShu.IsTiaoShi && ZongKaiGuan;)
                        {
                            KongXianFuWei(TDStateType.TiaoShiKongXian);
                            if (TDModel.GanYingModel.IsJiTing)
                            {
                                if (jitingcount == 0)
                                {
                                    jitingcount = 1;
                                    LogMsg(RiJiEnum.TDXieJiLu, $"急停");
                                }
                                TDModel.ShouDongCanShu.SDTest = false;
                                Thread.Sleep(10);
                                continue;
                            }
                            if (TDModel.ShouDongCanShu.IsZhanTing)
                            {
                                if (zhanting == 0)
                                {
                                    zhanting = 1;
                                    LogMsg(RiJiEnum.TDXieJiLu, $"暂停");
                                }

                                Thread.Sleep(10);
                                continue;
                            }
                            if (TDModel.GanYingModel.QiDongTest || TDModel.ShouDongCanShu.SDTest)
                            {
                                if (TDModel.ShouDongCanShu.SDTest)
                                {
                                    TDModel.ShouDongCanShu.SDTest = false;
                                    LogMsg(RiJiEnum.TDLiuCheng, $"手动测试");
                                }
                                if (TDModel.GanYingModel.QiDongTest)
                                {
                                    TDModel.GanYingModel.QiDongTest = false;
                                    LogMsg(RiJiEnum.TDLiuCheng, $"自动测试");
                                }
                                bool mazu = JianSuoManZu(TDStateType.TiaoShiGongZuo);
                                if (mazu == false)
                                {
                                  
                                    continue;
                                }                             
                                JianCeKaiShi(ZongTestModel.ZhongJianModels);
                                TDModel.ShouDongCanShu.SDTest = false;

                            }
                            if (TDModel.ShouDongCanShu.IsTiaoChuJianCe)
                            {
                                TDModel.ShouDongCanShu.IsTiaoChuJianCe = false;
                                LogMsg(RiJiEnum.TDXieJiLu, $"手动退出检测");
                            }
                            Thread.Sleep(1);
                        }
                        if (TDModel.ShouDongCanShu.IsTiaoShi == false)
                        {                   
                            LogMsg(RiJiEnum.TDXieJiLu, $"调试退出");
                        }
                    }
                    else
                    {
                        KongXianFuWei(TDStateType.ZhengChangKongXian);
                        if (TDModel.GanYingModel.QiDongTest)
                        {
                            if (shoci == 1)
                            {
                                if (ff==0)
                                {
                                    ff = 1;
                                    LogMsg(RiJiEnum.TDLiuCheng, $"处于正在测试");
                                }
                               
                                continue;
                            }
                         
                            bool mazu = JianSuoManZu(TDStateType.ZhengChengGongZuo);
                            if (mazu == false)
                            {
                                HuiWei(false, true);
                                continue;
                            }
                            shoci = 1;
                            LogMsg(RiJiEnum.TDLiuCheng, $"处于自动模式下");
                            JianCeKaiShi(ZongTestModel.ZhongJianModels);                         
                            HuiWei(TDModel.FuWeiCanShu.JieGuo==1, true);
                        }
                        else
                        {
                            shoci = 0;
                            ff = 0;
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    LogMsg(RiJiEnum.TDCuoWuRedLog, $"发生错误{ex}");
                }
                Thread.Sleep(10);
            }
        }

      

        /// <summary>
        /// 收集日记
        /// </summary>
        /// <param name="denji"></param>
        /// <param name="msg"></param>
        private void LogMsg(RiJiEnum denji,string msg)
        {
            RiJiLog.Cerate().Add(denji,$"{TDModel.TDName}:{msg}", TDModel.TDID);
        }

        /// <summary>
        /// true表示满足需求
        /// </summary>
        private bool JianSuoManZu(TDStateType stateType)
        {
            string shangcima = TDModel.FuWeiCanShu.MaZhi;
            {
                TDModel.FuWeiCanShu.Clear();
                TDModel.ShouDongCanShu.IsTiaoChuJianCe = false;
                TDModel.ShouDongCanShu.IsJiXuCeShi = false;            
                TDModel.GanYingModel.QiDongTest = false;
                TDModel.GanYingModel.TDState = stateType;
                BuHeGeXiangMu.Clear();
            }
          
            ChuFaTDShiJian(BuZhouType.ZhunBeiJianCe, "准备检测,开始扫码", new TestModel());
            if (ZongTestModel == null)
            {
                LogMsg(RiJiEnum.TDLiuChengRed, $"没有选择配方,条件不满足");
                ChuFaTDShiJian(BuZhouType.ZongJieSu, "没有选择配方,条件不满足", null);
                return false;
            }
            HuanCunLei.Cerate().Clear(TDModel.TDID);

            if (TDModel.PeiZhiCanShu.IsSaoMa)
            {
                string ma = SheBeiJiHe.Cerate().XieShuSaoMa(TDModel.PeiZhiCanShu.SaoMaName, TDModel.PeiZhiCanShu.DuSaoMaZhi, shangcima, TDModel.TDID);
                TDModel.FuWeiCanShu.MaZhi = ma;
                if (string.IsNullOrEmpty(TDModel.FuWeiCanShu.MaZhi))
                {
                    LogMsg(RiJiEnum.TDLiuChengRed, $"扫码为空");
                    ChuFaTDShiJian(BuZhouType.ZongJieSu, "扫码为空", null);
                    return false;

                }

                HuanCunLei.Cerate().SetHuanCunMa(TDModel.TDID, ma);
                LogMsg(RiJiEnum.TDLiuCheng, $"扫码成功:{TDModel.FuWeiCanShu.MaZhi} ");
                if (TDModel.PeiZhiCanShu.IsZiDongQiePeiFang == 1)
                {
                    string yongdiaohao = "";
                    string iscunzai = DataJiHe.Cerate().IsCunZaiPeiFang(TDModel.GanYingModel.QieHuanXingHao,TDModel.PeiZhiCanShu.QieHuanType,out yongdiaohao);
                    if (string.IsNullOrEmpty(iscunzai))
                    {
                        ChuFaTDShiJian(BuZhouType.ZongJieSu, "自动切换配方不存在", null);
                        LogMsg(RiJiEnum.TDLiuCheng, $"自动切换配方不存在:{TDModel.GanYingModel.QieHuanXingHao} ");
                        return false;
                    }
                    else
                    {
                        if (ZongTestModel.Name != iscunzai)
                        {
                            JieMianModel model = new JieMianModel();
                            model.TDID = TDModel.TDID;
                            model.CaoZuo = true;
                            model.PeiFangName = iscunzai;
                            JieMianLei.Cerate().CaoZuo(DoType.JiaZaiPeiFang, model);
                        }
                    }
                    {
                        if (ZongTestModel != null)
                        {
                            ZongTestModel model = ZongTestModel;
                            bool ishege = false;
                            foreach (var item in model.PeiFangDuiYingMa)
                            {
                                if (item.IsQieHuan==1)
                                {
                                    continue;
                                }
                                if (ma.Contains(item.Name))
                                {
                                    ishege = true;
                                    break;
                                }
                            }
                            if (ishege==false)
                            {
                                LogMsg(RiJiEnum.TDLiuChengRed, $"{ma} 码不符合要求:{ChangYong.FenGeDaBao(model.PeiFangDuiYingMa," ")}");
                                ChuFaTDShiJian(BuZhouType.ZongJieSu, "码不符合要求", null);
                                return false;
                            }
                        }
                    }
                   
                }
            }
            if (ZongTestModel == null)
            {
                ChuFaTDShiJian(BuZhouType.ZongJieSu, "配方不存在", null);
                LogMsg(RiJiEnum.TDLiuChengRed, $"没有选择配方,条件不满足");
                return false;
            }
            bool jinzhan=  ShangMes(1, TDModel.FuWeiCanShu.MaZhi);
            if (jinzhan==false)
            {
                ChuFaTDShiJian(BuZhouType.ZongJieSu, "上传不合格", null);
                return false;
            }
         
            ChuFaTDShiJian(BuZhouType.KaiShiJianCe, "开始检测", new TestModel());
            return true;
        }

        /// <summary>
        /// 空闲的状态
        /// </summary>
        private void KongXianFuWei(TDStateType stateType)
        {
            if (TDModel.GanYingModel.TDState!= stateType)
            {
                TDModel.GanYingModel.TDState = stateType;
            }

        }
        /// <summary>
        /// 执行回位
        /// </summary>
        private void HuiWei(bool ishege,bool isxuyao) 
        {
            TDModel.GanYingModel.QiDongTest = false;        
            if (isxuyao)
            {
                if (ishege)
                {
                  
                    JiHeSheBei.Cerate().XieChengGong(TDModel.TDID, ZongTestModel.GetZhiLingName(ShouWeiType.HeGe));

                }
                else
                {
                    JiHeSheBei.Cerate().XieChengGong(TDModel.TDID, ZongTestModel.GetZhiLingName(ShouWeiType.BuHeGe));
                }
               
            }
           
        }

        /// <summary>
        /// 项目检测开始
        /// </summary>
        /// <param name="lismodel"></param>
        /// <param name="zongshu"></param>
        private void JianCeKaiShi(List<ZhongJianModel> lismodel)
        {
            bool islaohua = false;
            int laohuacishu = 1;

            if (TDModel.ShouDongCanShu.LaoHuaTest != LaoHuaType.WuLaoHua)
            {
                laohuacishu = TDModel.ShouDongCanShu.LaoHuaCiShu;
                TDModel.ShouDongCanShu.LaoHuaShengYuCiShu = laohuacishu;
                TDModel.ShouDongCanShu.LaoHuaBuHeGeCiShu = 0;
                TDModel.ShouDongCanShu.LaoHuaHeGeCiShu = 0;
                islaohua = true;
            }
            for (int ff = 0; ff < laohuacishu; ff++)
            {
                bool isbuhege = true;
                List<TestModel> lismodels = new List<TestModel>();
                //初始化
                {
                    ClearData();
                    if (islaohua)
                    {
                        if (ff > 0)
                        {
                            TDModel.FuWeiCanShu.Clear();
                            ChuFaTDShiJian(BuZhouType.ZhunBeiJianCe, "准备检测", new TestModel(), false);
                            ChuFaTDShiJian(BuZhouType.KaiShiJianCe, "开始检测", new TestModel());
                        }
                    }
                }
                //测试项测试
                {
                    bool isduandian = false;                 
                    for (int c = 0; c < lismodel.Count; c++)
                    {
                        if (GetTiaoChu())
                        {
                            break;
                        }
                        if (TDModel.ShouDongCanShu.IsZhanTing)
                        {
                            c--;
                            Thread.Sleep(1);
                            continue;
                        }
                        ZhongJianModel model = lismodel[c];
                        if (model.TestModel.IsTest == false)
                        {
                            continue;
                        }
                        if (model.TestModel.IsWuZhuSheBei)
                        {
                            if (string.IsNullOrEmpty(model.TestModel.ShiYingTDID)==false)
                            {
                                int tdid = ChangYong.TryInt(model.TestModel.ShiYingTDID,-1);
                                if (tdid!=TDModel.TDID)
                                {
                                    continue;
                                }
                            }
                        }
                        if (TDModel.ShouDongCanShu.IsTiaoShi)
                        {
                            if (TDModel.ShouDongCanShu.JiGeXuHao.Count > 0)
                            {
                                if (TDModel.ShouDongCanShu.JiGeXuHao.IndexOf(ChangYong.TryInt(model.TestModel.XuHaoID, 0)) < 0)
                                {
                                    continue;
                                }
                            }
                        }
                        isduandian = model.TestModel.IsDuanDian;
                        if (model.TestModel.IsTest)
                        {
                            if (string.IsNullOrEmpty(model.TestModel.KaiShiTime))
                            {
                                model.TestModel.KaiShiTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            }
                            ZhiJieGuo xuhao = JianCeHuanJie(model);
                            model.TestModel.JianCeCiShu++;
                            bool isdaboshangchuan = true;
                            int fangongweizhi = -1;
                            if (xuhao.IsHeGe == false)
                            {
                                bool isfujian = model.TestModel.IsFuJian == 1;
                                if (isfujian)
                                {
                                    int cishu = model.TestModel.FuJianCiShu;
                                    if (cishu > 0)
                                    {
                                        model.TestModel.FuJianCiShu--;
                                        fangongweizhi = GetFanGongWeiZhi(c, model.TestModel.FuJianWeiZhi, lismodel);
                                        if (fangongweizhi >= 0)
                                        {
                                            isdaboshangchuan = false;
                                        }
                                    }
                                }
                            }
                            if (isdaboshangchuan)
                            {
                                model.TestModel.JieSuTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                model.TestModel.TestTime = (DateTime.Now - ChangYong.TryDate(model.TestModel.KaiShiTime, DateTime.Now)).TotalSeconds.ToString("0.000");
                                TestModel fubenmodel = model.TestModel.NewFuZhi();
                                TestModel shangchuanmodel = model.TestModel.NewFuZhi();
                                shangchuanmodel.GetZuiZhongJieGuo();
                                lismodels.Add(fubenmodel);
                                bool isceshihege= model.TestModel.IsHeGe == HeGeType.Pass;
                                if (isbuhege)
                                {
                                    isbuhege = isceshihege;
                                }
                                bool zhenmse = ShangMes(2, TDModel.FuWeiCanShu.MaZhi, shangchuanmodel);
                                if (zhenmse == false)
                                {
                                    isbuhege = false;
                                }
                                if (isbuhege == false)
                                {
                                    if (isceshihege==false)
                                    {
                                        BuHeGeXiangMu.Add($"{model.TestModel.ItemName},{model.TestModel.Value}");
                                    }
                                    if (TDModel.ShouDongCanShu.IsNGTiaoChu)
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                c = fangongweizhi - 1;
                                if (c < 0)
                                {
                                    c = -1;
                                }
                            }
                        }
                        if (TDModel.ShouDongCanShu.IsTiaoShi)
                        {
                            if (isduandian)
                            {
                                for (; ZongKaiGuan;)
                                {

                                    if (GetTiaoChu())
                                    {
                                        isduandian = false;
                                        break;
                                    }
                                    if (model.TestModel.IsDuanDian == false)
                                    {
                                        isduandian = false;

                                        break;
                                    }
                                    Thread.Sleep(1);
                                }
                            }
                        }

                    }
                }
                //测试结果
                {
                    bool istuichu = GetTiaoChu() == false;
                    if (istuichu)
                    {
                        TDModel.FuWeiCanShu.JieGuo = isbuhege ? 1 : 2;
                        bool zhenmes = ShangMes(3, TDModel.FuWeiCanShu.MaZhi);
                        if (zhenmes == false)
                        {
                            TDModel.FuWeiCanShu.JieGuo = 2;
                            isbuhege = false;
                        }

                        if (TDModel.ShouDongCanShu.LaoHuaTest != LaoHuaType.WuLaoHua)
                        {
                            if (TDModel.ShouDongCanShu.LaoHuaTest == LaoHuaType.CiShuLaoHua)
                            {
                                TDModel.ShouDongCanShu.LaoHuaShengYuCiShu--;
                                if (TDModel.FuWeiCanShu.JieGuo == 1)
                                {
                                    TDModel.ShouDongCanShu.LaoHuaHeGeCiShu++;
                                }
                                else
                                {
                                    TDModel.ShouDongCanShu.LaoHuaBuHeGeCiShu++;
                                }
                            }
                            else if (TDModel.ShouDongCanShu.LaoHuaTest == LaoHuaType.HeGeLaoHua)
                            {
                                if (TDModel.FuWeiCanShu.JieGuo == 1)
                                {
                                    TDModel.ShouDongCanShu.LaoHuaShengYuCiShu--;
                                    TDModel.ShouDongCanShu.LaoHuaHeGeCiShu++;

                                }
                                else
                                {
                                    ff--;
                                    TDModel.ShouDongCanShu.LaoHuaBuHeGeCiShu++;
                                }
                            }
                        }
                    }
                    else
                    {
                        TDModel.FuWeiCanShu.JieGuo = 2;
                        isbuhege = false;
                    }
                    Task.Factory.StartNew(() =>
                    {
                        DaoChuTxlxs.DaoChu(lismodels, TDModel.FuWeiCanShu.MaZhi, isbuhege,TDModel.TDName);
                        JiShuGuanLiLei.Cerate().SetPeiFangCount(TDModel.TDID, isbuhege);
                        JiShuGuanLiLei.Cerate().SetTDCount(TDModel.TDID, isbuhege);
                    });
                    ChuFaTDShiJian(BuZhouType.ZongJieSu, "", new TestModel());
                    JiHeSheBei.Cerate().XieChengGong(TDModel.TDID, ZongTestModel.GetZhiLingName(ShouWeiType.JieSu));
                  
                    if (istuichu)
                    {
                        break;
                    }
                }

            }

        }

        /// <summary>
        /// 空表示下一方走 否则返回指定行
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private ZhiJieGuo JianCeHuanJie(ZhongJianModel model)
        {
            ZhiJieGuo jieguomodel = new ZhiJieGuo();
            jieguomodel = JianCeDanXiangMu(model.TestModel);
            return jieguomodel;
           
        }


        private int GetFanGongWeiZhi(int index,string weizhi, List<ZhongJianModel> lismodel)
        {
            for (int i = 0; i < lismodel.Count; i++)
            {
                if (i< index)
                {
                    if (lismodel[i].TestModel.IsTest)
                    {
                        List<string> weis = ChangYong.JieGeStr(lismodel[i].TestModel.FuJianWeiZhi,',');
                        if (weis.IndexOf(weizhi)>=0)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 单项检测
        /// </summary>
        /// <param name="model"></param>
        /// <param name="zongshu"></param>
        /// <returns></returns>
        private ZhiJieGuo JianCeDanXiangMu(TestModel model)
        {

            model.IsHeGe = HeGeType.ZhengZaiTest;
            LogMsg(RiJiEnum.TDLiuCheng, $"开始检测:{model.ItemName}");

            ChuFaTDShiJian(BuZhouType.DXiangMuJianCe, $"{model.ItemName}:开始检测", model);
            ZhiJieGuo jieguo = JianCeDuiXiang.JianCe(model.NewFuZhi());
            model.Value = jieguo.RecZhi;
            model.IsHeGe = jieguo.IsHeGe ? HeGeType.Pass : HeGeType.NG;
            model.TestTime = jieguo.TestTime;
            model.IsZiFuChuan = jieguo.IsString;
          
            ChuFaTDShiJian(BuZhouType.DXiangMuJieSu, $"{model.ItemName}:检测结束,{model.Value}", model);
            LogMsg(RiJiEnum.TDLiuCheng, $"结束检测:{model.ItemName} 数据:{model.ToString()}");
            return jieguo;
        }

        /// <summary>
        /// 触发界面显示参数
        /// </summary>
        /// <param name="jiancekaishi"></param>
        /// <param name="zonghege"></param>
        /// <param name="baifenbi"></param>
        /// <param name="buhexiangmus"></param>
        /// <param name="dilanmiasu"></param>
        /// <param name="model"></param>
        private void ChuFaTDShiJian(BuZhouType jiancekaishi,string dilanmiasu, TestModel model,bool isyibu=true)
        {
            float baifenbi = 0;
            if (model != null)
            {
                if (jiancekaishi == BuZhouType.ZongJieSu)
                {
                    baifenbi = 100;
                }
                else if (jiancekaishi == BuZhouType.ZhunBeiJianCe || jiancekaishi == BuZhouType.KaiShiJianCe)
                {
                    baifenbi = 0;
                }
                else
                {
                    float zongshu = ZongTestModel.TestCount;
                    float zhanbbi = model.WeiZhi;
                    if (zhanbbi < 0)
                    {
                        zhanbbi = 1;
                    }
                    baifenbi = zhanbbi / zongshu;
                }

            }
            XiangMuModel xiangMuModel = new XiangMuModel();
            xiangMuModel.BuHeGeXiangMu = BuHeGeXiangMu;     
            xiangMuModel.BuZhouType = jiancekaishi;
            xiangMuModel.TestModel = model;
            xiangMuModel.ZhiXingBaiFenBi = baifenbi;
            xiangMuModel.ZongJieGuo = TDModel.FuWeiCanShu.JieGuo==1;
            xiangMuModel.ErWeiMa = TDModel.FuWeiCanShu.MaZhi;
            xiangMuModel.DiLanMiaoSu = dilanmiasu;
            xiangMuModel.TDID = TDModel.TDID;
            if (model != null)
            {
                xiangMuModel.IsZongXiang = model.ZiXiangShunXu == -1;
            }
            else
            {
                xiangMuModel.IsZongXiang = true;
            }
            ShiJianModel shijianmodel = new ShiJianModel();
            shijianmodel.XiangMuModel = xiangMuModel;
            JieMianLei.Cerate().ChuFaEvent(EventType.TestXiangMu, shijianmodel, isyibu);
            if (jiancekaishi== BuZhouType.ZongJieSu)
            {
                TDModel.FuWeiCanShu.IsKaiShiTime = false;
            }
        }

        private bool ShangMes(int type,string mazhi,TestModel testmodel=null)
        {
            if (type == 1)
            {
                ShangChuanDataModel mesmodel = new ShangChuanDataModel();
                mesmodel.GuoChengMa = mazhi;
                mesmodel.KaiShiModel.IsShouZhan = false;
                mesmodel.KaiShiModel.QiTaZhi ="";
                mesmodel.ShangChuanType = ShangChuanType.KaiShi;
                mesmodel.TDID = TDModel.TDID;
                mesmodel.TDName = TDModel.TDName;
                LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
                if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.NG)
                {                 
                    LogMsg(RiJiEnum.MesData, $"{mazhi} 过站失败:{jieguo.NeiRong}");
                    if (TDModel.GanYingModel.IsDianJian)
                    {
                        LogMsg(RiJiEnum.MesData, $"处于点检状态");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    LogMsg(RiJiEnum.MesData, $"{mazhi} MES进站成功:{jieguo.NeiRong}");

                    return true;
                }
              
            }
            else if (type == 2)
            {
                if (testmodel.IsMes)
                {
                    if (TDModel.PeiZhiCanShu.BuChuanGuoZhan != 1)
                    {
                        ShangChuanDataModel mesmodel = new ShangChuanDataModel();
                        mesmodel.GuoChengMa = mazhi;
                        mesmodel.TDID = TDModel.TDID;
                        mesmodel.TDName = TDModel.TDName;
                        mesmodel.BuZhouModel.BuZhouShuJu = testmodel;
                        mesmodel.BuZhouModel.JieGuo = testmodel.IsHeGe == HeGeType.Pass;
                        mesmodel.ShangChuanType = ShangChuanType.BuZhouShangChuan;
                        LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
                        LogMsg(RiJiEnum.MesData, $"{jieguo.FanHuiJieGuo}:{jieguo.NeiRong}");
                        if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.NG)
                        {
                            if (TDModel.GanYingModel.IsDianJian)
                            {
                                LogMsg(RiJiEnum.MesData, $"处于点检状态");
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                           
                            return true;
                        }
                    }
                    else
                    {
                        LogMsg(RiJiEnum.MesData, $"不需要过程数据");
                        return true;
                    }
                }
                else
                {
                    LogMsg(RiJiEnum.MesData, $"没有启动MES上传");
                    return true;
                }
            }
            else if (type==3)
            {
                if (TDModel.PeiZhiCanShu.BuChuanGuoZhan != 1)
                {
                    ShangChuanDataModel mesmodel = new ShangChuanDataModel();
                    mesmodel.GuoChengMa = mazhi;
                    mesmodel.TDID = TDModel.TDID;
                    mesmodel.TDName = TDModel.TDName;
                    mesmodel.JieSuModel.KaiShiTime = TDModel.FuWeiCanShu.KaiShiTime;
                    mesmodel.JieSuModel.JieSuTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    mesmodel.JieSuModel.IsHeGe = TDModel.FuWeiCanShu.JieGuo == 1;
                    mesmodel.ShangChuanType = ShangChuanType.JieSu;
                    LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
                    LogMsg(RiJiEnum.MesData, $"{mazhi}:{jieguo.NeiRong}");
                    if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.NG)
                    {
                        if (TDModel.GanYingModel.IsDianJian)
                        {
                            LogMsg(RiJiEnum.MesData, $"处于点检状态");
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        LogMsg(RiJiEnum.MesData, $"{mazhi} MES出站成功:{jieguo.NeiRong}");
                        return true;
                    }
                }
                else
                {
                    LogMsg(RiJiEnum.MesData, $"不需要结束数据");
                    return true;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取是否急停是否手动跳出
        /// </summary>
        /// <returns></returns>
        private bool GetTiaoChu()
        {
            bool istiaochu3 = TDModel.GanYingModel.IsJiTing || TDModel.ShouDongCanShu.IsTiaoChuJianCe;
            return istiaochu3;
        }

        /// <summary>
        /// 测试前清理数据
        /// </summary>
        private void ClearData()
        {
            JiHeSheBei.Cerate().XieChengGong(TDModel.TDID, ZongTestModel.GetZhiLingName(ShouWeiType.KaiShi));
            if (ZongTestModel != null)
            {
                for (int i = 0; i < ZongTestModel.ZhongJianModels.Count; i++)
                {
                    ZongTestModel.ZhongJianModels[i].TestModel.Clear();

                }
            }
            JianCeDuiXiang.KaiShi();
            BuHeGeXiangMu.Clear();
        }

        /// <summary>
        /// 操作通道
        /// </summary>
        /// <param name="tDDoType"></param>
        /// <param name="model"></param>
        public void CaoZuo(DoType tDDoType, JieMianModel model)
        {
            switch (tDDoType)
            {
                case DoType.JiaZaiPeiFang:
                    {
                        ZongTestModel = DataJiHe.Cerate().GetTestPeiFang(TDModel.TDID);                      
                    }
                    break;
                case DoType.DuanDian:
                    {
                        if (ZongTestModel != null)
                        {
                            for (int i = 0; i < ZongTestModel.ZhongJianModels.Count; i++)
                            {
                                if (ZongTestModel.ZhongJianModels[i].TestModel.WeiZhi == model.ZuoWei)
                                {
                                    ZongTestModel.ZhongJianModels[i].TestModel.IsDuanDian = model.CaoZuo;
                                    break;
                                }
                              
                            }
                          
                        }
                    }
                    break;
                case DoType.DanBuZhiXing:
                    {
                        if (ZongTestModel != null)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                ZhongJianModel zhongJianModel = null;
                                for (int i = 0; i < ZongTestModel.ZhongJianModels.Count; i++)
                                {
                                    if (ZongTestModel.ZhongJianModels[i].TestModel.WeiZhi == model.ZuoWei)
                                    {
                                        zhongJianModel = ZongTestModel.ZhongJianModels[i];
                                        break;
                                    }

                                }
                                if (zhongJianModel!=null)
                                {
                                  
                                    JianCeDanXiangMu(zhongJianModel.TestModel);
                                  
                                }
                            
                            });
                        }
                    }
                    break;
                case DoType.CaoZuoZongTiaoChu:
                    {
                        TDModel.ShouDongCanShu.IsTiaoChuJianCe = true;
                        JianCeDuiXiang.ZhiXingTiaoChu(1);
                    }
                    break;
                case DoType.DanBuTiaoChu:
                    {
                        JianCeDuiXiang.ZhiXingTiaoChu(1);
                    }
                    break;
                case DoType.JiGeXuLie:
                    {
                        if (string.IsNullOrEmpty(model.PeiFangName) == false)
                        {
                            List<int> xuhao = ChangYong.JieGeInt(model.PeiFangName, ',');
                            TDModel.ShouDongCanShu.JiGeXuHao = xuhao;
                        }
                        else
                        {
                            TDModel.ShouDongCanShu.JiGeXuHao.Clear();
                        }
                    }
                    break;
                case DoType.CaoZuoZanTing:
                    {
                        TDModel.ShouDongCanShu.IsZhanTing = model.CaoZuo;
                    }
                    break;
                case DoType.CaoZuoMes:
                    {
                        TDModel.ShouDongCanShu.IsQiYongMes = model.CaoZuo;
                        ShangChuanMesLei.Cerate().ShuXinTdState(TDModel.TDID, TDModel.ShouDongCanShu.IsQiYongMes);
                        
                    }
                    break;
                case DoType.CaoZuoTiaoShi:
                    {
                        TDModel.ShouDongCanShu.IsTiaoShi = model.CaoZuo;
                        if (TDModel.ShouDongCanShu.IsTiaoShi==false)
                        {
                            TDModel.ShouDongCanShu.JiGeXuHao.Clear();
                        }
                    }
                    break;
                case DoType.CaoZuoNGTiaoChu:
                    {
                        TDModel.ShouDongCanShu.IsNGTiaoChu = model.CaoZuo;
                    }
                    break;
                case DoType.CaoZuoDuanDianTiaoChu:
                    {
                        TDModel.ShouDongCanShu.IsJiXuCeShi = true;
                       
                    }
                    break;
                case DoType.ShouDongTest:
                    {
                        TDModel.ShouDongCanShu.SDTest = true;
                    }
                    break;
                case DoType.LaoHuaTest:
                    {
                        TDModel.ShouDongCanShu.LaoHuaTest = model.LaoHuaType;
                        if (TDModel.ShouDongCanShu.LaoHuaTest == LaoHuaType.WuLaoHua)
                        {
                            TDModel.ShouDongCanShu.LaoHuaCiShu = 0;
                            TDModel.ShouDongCanShu.LaoHuaShengYuCiShu = 0;
                        }
                        else
                        {
                            TDModel.ShouDongCanShu.LaoHuaCiShu = model.ZuoWei;
                        }
                    }
                    break;
                case DoType.UIFanHuiJieGuo:
                    {
                        JianCeDuiXiang.UIFanHuiJieGuo(model.ZhiJieGuo);
                    }
                    break;
                case DoType.DianJianMoShi:
                    {
                       bool moshi = model.CaoZuo;
                        if (moshi)
                        {
                            Task.Factory.StartNew(() => { JiHeSheBei.Cerate().XieChengGong(TDModel.TDID, TDModel.PeiZhiCanShu.MesDuState); });
                        }
                        else
                        {
                            Task.Factory.StartNew(() => { JiHeSheBei.Cerate().XieChengGong(TDModel.TDID, TDModel.PeiZhiCanShu.MesXieState); });
                        }

                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 软件关闭时候用的
        /// </summary>
        public void Close()
        {
            ZongKaiGuan = false;
            JianCeDuiXiang.Close();
            Thread.Sleep(100);
        }



    }
}
