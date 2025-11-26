using ATSJianCeXianTi.GongNengLei.GNKJ;
using ATSJianCeXianTi.GongNengLei.Model;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.Log;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.Model;
using SSheBei.ZongKongZhi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.Lei
{
  

    /// <summary>
    /// 处理功能
    /// </summary>
    public abstract class ABSGongNengLei
    {

        /// <summary>
        ///  表示弹窗事件
        /// </summary>
        public event Action<TangChuanUIModel, int> ChuLiUIEvent;
        /// <summary>
        /// 通道id
        /// </summary>
        public int TDID { get; set; } = -1;
        /// <summary>
        /// 执行测试项目
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tiaozhunmodel"></param>
        /// <returns></returns>
        public ZhiJieGuo ZhiXingJieGuo(TestModel model)
        {
            IniData();
            DengDaiModel dendaimodel = ChangYong.HuoQuJsonToShiTi<DengDaiModel>(model.CMDCanShu);
            if (dendaimodel == null)
            {
                ZhiJieGuo jieguomodel = new ZhiJieGuo();
                jieguomodel.RecZhi = "参数有问题";
                jieguomodel.IsString = 1;
                jieguomodel.IsHeGe = false;
                return jieguomodel;
            }
            else
            {
                ZhiJieGuo jieguomodel = ZhiXing(model, dendaimodel);
                return jieguomodel;
            }
        }
        #region 需要实现的类
        /// <summary>
        /// 实现类执行的
        /// </summary>
        /// <returns></returns>
        protected abstract ZhiJieGuo ZhiXing(TestModel model, DengDaiModel canshumodel);

        /// <summary>
        /// 执行开始
        /// </summary>
        protected abstract void IniData();

        /// <summary>
        /// 执行跳出 1表示整体跳出
        /// </summary>
        public abstract void TiaoChu(int biaozhi);

        /// <summary>
        /// 获取测试标志
        /// </summary>
        /// <returns></returns>
        public abstract string GetTestBiaoZhi();
        /// <summary>
        /// 获取判断方式
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetLeiXing();

        /// <summary>
        /// 1表示需要设备不需要无设备 2表示不需要设备需要无设备 3表示需要设备与无设备
        /// </summary>
        /// <returns></returns>
        public abstract int GetXuYaoSheBei();

        /// <summary>
        /// 没有判断的方式
        /// </summary>
        /// <returns></returns>
        protected abstract List<string> GetMeiYouPanDuanFangShi();

        public abstract void UIFanHuiJieGuo(ZhiJieGuo model);
        #endregion

        #region 配方用的方法
        /// <summary>
        /// 获取判断方式
        /// </summary>
        /// <returns></returns>
        public List<string> GetPanDuanFangShi()
        {
            List<string> list = GetMeiYouPanDuanFangShi();
            List<string> fangshis = ChangYong.MeiJuLisName(typeof(PanDuanType));
            List<string> xuyaolis = new List<string>();
            for (int i = 0; i < fangshis.Count; i++)
            {
                if (list.IndexOf(fangshis[i]) < 0)
                {
                    xuyaolis.Add(fangshis[i]);
                }
            }

            return xuyaolis;
        }

        public List<string> GetSheBei()
        {
            int isxuyao = GetXuYaoSheBei();
            List<string> cmdlis = new List<string>();
            if (isxuyao == 1)
            {
                List<JiaZaiSheBeiModel> lis = HCLisDataLei<JiaZaiSheBeiModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lis.Count; i++)
                {
                    cmdlis.Add(string.Format("{0}:{1}", lis[i].SheBeiID, lis[i].SheBeiName));
                }
            }
            else if (isxuyao==2)
            {
                cmdlis.Add(string.Format("{0}:{1}", -1,"无设备"));
            }
            else if (isxuyao == 3)
            {
                List<JiaZaiSheBeiModel> lis = HCLisDataLei<JiaZaiSheBeiModel>.Ceratei().LisWuLiao;
                for (int i = 0; i < lis.Count; i++)
                {
                    cmdlis.Add(string.Format("{0}:{1}", lis[i].SheBeiID, lis[i].SheBeiName));
                }
                cmdlis.Add(string.Format("{0}:{1}", -1, "无设备"));
            }
            return cmdlis;
        }

        /// <summary>
        /// 获取发送的命令
        /// </summary>
        /// <returns></returns>
        public List<JiCunQiModel> GetCMDSend(int id, bool isduxie)
        {
            int isxuyao = GetXuYaoSheBei();
            List<JiCunQiModel> cmdlis = new List<JiCunQiModel>();
            if (isxuyao == 1)
            {
                if (id >= 0)
                {
                    List<JiCunQiModel> lis = ZongSheBeiKongZhi.Cerate().GetPeiZhiJiCunQi(id, isduxie ? 1 : 2);
                    cmdlis = ChangYong.FuZhiShiTi(lis);
                }
            }
            else if (isxuyao == 2)
            {
                JiCunQiModel model = new JiCunQiModel();
                model.WeiYiBiaoShi = "无设备用的";
                model.MiaoSu = "数据";
                model.DuXie = 3;
                cmdlis.Add(model);
            }
            else if (isxuyao == 3)
            {
                if (id >= 0)
                {
                    List<JiCunQiModel> lis = ZongSheBeiKongZhi.Cerate().GetPeiZhiJiCunQi(id, isduxie ? 1 : 2);
                    cmdlis = ChangYong.FuZhiShiTi(lis);
                }
                else
                {
                    JiCunQiModel model = new JiCunQiModel();
                    model.WeiYiBiaoShi = "无设备用的";
                    model.MiaoSu = "数据";
                    model.DuXie = 3;
                    cmdlis.Add(model);
                }
              
            }
            return cmdlis;
        }

        /// <summary>
        /// 获取参数控件
        /// </summary>
        /// <param name="shebeiid"></param>
        /// <param name="weiyibiaoshi"></param>
        /// <returns></returns>
        public  KJPeiZhiJK GetKJ(int shebeiid, string weiyibiaoshi)
        {
            DengDaiTuiChuKJ kJ = new DengDaiTuiChuKJ();
            kJ.SetCanShu(shebeiid, weiyibiaoshi);
            return kJ;       
        }
        #endregion

        #region 界面用的方法
      
        protected enum PanDuanType
        {
            不判断,
            String等于,
            String包含,
            String前包含,
            String后包含,
            String上下限包含与,
            String上下限包含或,
            Shu大于等于,
            Shu小于等于,
            Shu大于,
            Shu小于,
            Shu两者之间,
        }

        protected void ChuFaUI(TangChuanUIModel lis, int type)
        {
            if (ChuLiUIEvent != null)
            {
                ChuLiUIEvent(lis, type);
            }
        }
        #endregion

        #region 判断的结果
        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="model"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected  ZhiJieGuo PanDuanHeGe(TestModel model, object value)
        {
            ZhiJieGuo zhiJieGuo = new ZhiJieGuo();
            PanDuanType panDuanType = ChangYong.GetMeiJuZhi<PanDuanType>(model.BiJiaoType);
            if (panDuanType == PanDuanType.Shu两者之间)
            {
                zhiJieGuo.IsString = 2;
                object fanhuizhi = GetZhi(model, value, true);
                zhiJieGuo.RecZhi = fanhuizhi;
                double shangshus = ChangYong.TryDouble(model.UpStr, 0);
                double xiaxian = ChangYong.TryDouble(model.LowStr, 0);
                double zhi = ChangYong.TryDouble(fanhuizhi, 0);
                if (zhi <= shangshus && zhi >= xiaxian)
                {
                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;

            }
            else if (panDuanType == PanDuanType.Shu大于等于)
            {
                zhiJieGuo.IsString = 2;
                object fanhuizhi = GetZhi(model, value, true);
                zhiJieGuo.RecZhi = fanhuizhi;
                double shangshus = ChangYong.TryDouble(model.UpStr, 0);
                double zhi = ChangYong.TryDouble(fanhuizhi, 0);
                if (zhi >= shangshus)
                {
                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.Shu大于)
            {
                zhiJieGuo.IsString = 2;
                object fanhuizhi = GetZhi(model, value, true);
                zhiJieGuo.RecZhi = fanhuizhi;
                double shangshus = ChangYong.TryDouble(model.UpStr, 0);
                double zhi = ChangYong.TryDouble(fanhuizhi, 0);
                if (zhi > shangshus)
                {
                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.Shu小于)
            {
                zhiJieGuo.IsString = 2;
                object fanhuizhi = GetZhi(model, value, true);
                zhiJieGuo.RecZhi = fanhuizhi;
                double shangshus = ChangYong.TryDouble(model.UpStr, 0);
                double zhi = ChangYong.TryDouble(fanhuizhi, 0);
                if (zhi < shangshus)
                {
                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.Shu小于等于)
            {
                zhiJieGuo.IsString = 2;
                object fanhuizhi = GetZhi(model, value, true);
                zhiJieGuo.RecZhi = fanhuizhi;
                double shangshus = ChangYong.TryDouble(model.UpStr, 0);
                double zhi = ChangYong.TryDouble(fanhuizhi, 0);
                if (zhi <= shangshus)
                {
                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.String等于)
            {
                object fanhizhi = GetZhi(model, value, false);
                zhiJieGuo.IsString = 1;
                zhiJieGuo.RecZhi = fanhizhi;
                if (fanhizhi.ToString().Equals(model.UpStr))
                {

                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.String包含)
            {
                object fanhizhi = GetZhi(model, value, false);
                zhiJieGuo.IsString = 1;
                zhiJieGuo.RecZhi = fanhizhi;
                if (fanhizhi.ToString().Contains(model.UpStr))
                {

                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.String上下限包含或)
            {
                object fanhizhi = GetZhi(model, value, false);
                zhiJieGuo.IsString = 1;
                zhiJieGuo.RecZhi = fanhizhi;
                if (fanhizhi.ToString().Contains(model.UpStr) || fanhizhi.ToString().Contains(model.LowStr))
                {

                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.String上下限包含与)
            {
                object fanhizhi = GetZhi(model, value, false);
                zhiJieGuo.IsString = 1;
                zhiJieGuo.RecZhi = fanhizhi;
                if (fanhizhi.ToString().Contains(model.UpStr) && fanhizhi.ToString().Contains(model.LowStr))
                {

                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.String前包含)
            {
                object fanhizhi = GetZhi(model, value, false);
                zhiJieGuo.IsString = 1;
                zhiJieGuo.RecZhi = fanhizhi;
                if (fanhizhi.ToString().StartsWith(model.UpStr) )
                {

                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else if (panDuanType == PanDuanType.String后包含)
            {
                object fanhizhi = GetZhi(model, value, false);
                zhiJieGuo.IsString = 1;
                zhiJieGuo.RecZhi = fanhizhi;
                if (fanhizhi.ToString().EndsWith(model.UpStr) )
                {

                    zhiJieGuo.IsHeGe = true;
                }
                else
                {
                    zhiJieGuo.IsHeGe = false;
                }
                return zhiJieGuo;
            }
            else
            {
                object fanhizhi = GetZhi(model, value, false);
                zhiJieGuo.IsString = 1;
                zhiJieGuo.IsHeGe = true;
                zhiJieGuo.RecZhi = fanhizhi;
                return zhiJieGuo;
            }

        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="csnhu"></param>
        /// <param name="isshuzi"></param>
        protected  object GetZhi(TestModel model, object csnhu, bool isshuzi)
        {
            if (isshuzi)
            {
                double beichushu = ChangYong.TryDouble(model.BeiChuShu, 1);
                double canshu2 = ChangYong.TryDouble(csnhu, 0) * beichushu + model.KB;
                if (model.BaoLiuWeiShu >= 0)
                {
                    if (model.BaoLiuWeiShu > 14)
                    {
                        model.BaoLiuWeiShu = 14;
                    }
                    model.Value = Math.Round(canshu2, model.BaoLiuWeiShu).ToString($"F{model.BaoLiuWeiShu}");
                }
                else
                {
                    model.Value = canshu2;
                }

            }
            else
            {
                model.Value = ChangYong.TryStr(csnhu, "");
            }
            return model.Value;
        }
        #endregion

        protected  XieRuMolde GetXieModel(TestModel tsmodel, DengDaiModel canshumodel)
        {
            XieRuMolde xieRuMolde = new XieRuMolde();
            xieRuMolde.JiCunQiWeiYiBiaoShi = tsmodel.CMDSend;
            xieRuMolde.Zhi = canshumodel.CanShu;
            xieRuMolde.SheBeiID = tsmodel.SheBeiID;
            return xieRuMolde;
        }
        protected void JiLvLog(string msg)
        {
            RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu,msg,TDID);
        }
    }
}
