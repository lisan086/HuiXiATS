using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.Mes;
using ATSJianMianJK.QuanXian;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.DataChuLi;
using JieMianLei.FuFrom;

namespace CheLianMes
{
    public class CheLianShangMes : XuShangChuanLei
    {
        private Dictionary<string, List<TestModel>> CeShiData = new Dictionary<string, List<TestModel>>();
        private PeiZhiMesWenDanModel CanShuModel = new PeiZhiMesWenDanModel();
        public CheLianShangMes()
        { 

        }
        public override void SetCanShu(int TDID)
        {
            IniGetModelLei<PeiZhiMesWenDanModel> dushuju = new IniGetModelLei<PeiZhiMesWenDanModel>("PeiZhiMesWenDanModel");

            CanShuModel = dushuju.GetTModel();

            XingDeMesQingQiuLei.Cerate().SetShuJu(new List<string>() { CanShuModel .TongDaoName});
        }


        public override LianWanModel BuZhouShangChuan(ShangChuanDataModel models)
        {
            string key = $"{models.TDID}:{models.TDName}";
            TestModel shuju = ChangYong.HuoQuJsonToShiTi<TestModel>(ChangYong.HuoQuJsonStr(models.BuZhouModel.BuZhouShuJu));
            LianWanModel recmodels = new LianWanModel();
            if (shuju == null)
            {
                recmodels.FanHuiJieGuo = JinZhanJieGuoType.NG;
                recmodels.NeiRong = "没有实例化结果";
            }
            else
            {
                recmodels.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                recmodels.NeiRong = "已经打包";
                if (shuju.IsMes)
                {
                    shuju.JieGuos= models.BuZhouModel.JieGuo.ToString();
                    if (CeShiData.ContainsKey(key) == false)
                    {
                        CeShiData.Add(key,new List<TestModel>());
                    }
                    CeShiData[key].Add(shuju);
                }
              
            }
            return recmodels;
        }

        public override void Close()
        {
          
        }

        public override LianWanModel GetQiTaXinXi(ShangChuanDataModel models)
        {
            return new LianWanModel();
        }

        public override LianWanModel JieSu(ShangChuanDataModel models)
        {
            string key= $"{models.TDID}:{models.TDName}";
            if (CeShiData.ContainsKey(key))
            {
                InGuoZhanDataModel guozhanmodel = new InGuoZhanDataModel();
                guozhanmodel.workStation = CanShuModel.SheBeiBianHao;
                guozhanmodel.empCode = QuanXianLei.CerateDanLi().GetDengLuMing();
                guozhanmodel.endProductSn = models.GuoChengMa;
                guozhanmodel.beginTime = models.JieSuModel.KaiShiTime;
                guozhanmodel.endTime = models.JieSuModel.JieSuTime;
                List<TestModel> shuju = CeShiData[key];
                {
                    GuoZhanDatasModel model = new GuoZhanDatasModel();
                    model.ComponentName = "总结果";
                    model.ConfirmResult = models.JieSuModel.IsHeGe ? "OK" : "NG";
                    model.ConfirmResultCode = "";
                    model.ReportResult = models.JieSuModel.IsHeGe ? "OK" : "NG";
                    model.ReportResultCode = "";
                    guozhanmodel.details.Add(model);

                }
                if (shuju.Count > 0)
                {
                    for (int i = 0; i < shuju.Count; i++)
                    {
                        bool jieguos = shuju[i].JieGuos.ToLower().Contains("u");
                        GuoZhanDatasModel model = new GuoZhanDatasModel();
                        model.ComponentName = shuju[i].ItemName;
                        model.ConfirmResult = jieguos ? "OK" : "NG";
                        model.ConfirmResultCode = shuju[i].Value.ToString();
                        model.ReportResult = jieguos ? "OK" : "NG";
                        model.ReportResultCode = shuju[i].Value.ToString();
                        guozhanmodel.details.Add(model);
                    }
                }
                OutingQiuModel<OutGuoZhanJieGuoModel> jieguo = XingDeMesQingQiuLei.Cerate().QiuQiuHttp<InGuoZhanDataModel, OutGuoZhanJieGuoModel>(guozhanmodel, CanShuModel.TongDaoName, CanShuModel.QianZhanWangZhi);

                LianWanModel jieguomodel = new LianWanModel();
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
                jieguomodel.NeiRong = jieguo.Msg;
                if (jieguo != null)
                {
                    if (jieguo.ChengGong)
                    {
                        if (jieguo.ShuJu.success.ToLower().Contains("u"))
                        {
                            jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                        }
                    }
                }
                return jieguomodel;
            }
            else
            {
                return new LianWanModel() { FanHuiJieGuo = JinZhanJieGuoType.NG, NeiRong=$"没有找到该数据:{key}" };
            }
        }

        public override LianWanModel KaiShiTiJiaoMa(ShangChuanDataModel models)
        {
            InQianGongXuJianCeModel qianmodel = new InQianGongXuJianCeModel();
            qianmodel.workStation = CanShuModel.SheBeiBianHao;
            qianmodel.empCode = QuanXianLei.CerateDanLi().GetDengLuMing();
            qianmodel.endProductSn = models.GuoChengMa;
            OutingQiuModel<OutQianGongXuJieGuoModel> jieguo = XingDeMesQingQiuLei.Cerate().QiuQiuHttp<InQianGongXuJianCeModel, OutQianGongXuJieGuoModel>(qianmodel, CanShuModel.TongDaoName, CanShuModel.QianZhanWangZhi);
         
            LianWanModel jieguomodel = new LianWanModel();
            jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
            jieguomodel.NeiRong = jieguo.Msg;
            if (jieguo != null)
            {
                if (jieguo.ChengGong)
                {
                    if (jieguo.ShuJu.success.ToLower().Contains("u"))
                    {
                        jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                    }
                }
            }
            return jieguomodel;
        }

      
        public override LianWanModel ZhongJianBangMa(ShangChuanDataModel models)
        {
            return new LianWanModel();
        }

        public override void GetPeiZhiForm()
        {
            PeiZhiFrm peiZhiFrm = new PeiZhiFrm();
            peiZhiFrm.SetCanShu();
            peiZhiFrm.TopMost = true;
            peiZhiFrm.BringToFront();
            peiZhiFrm.Show();
           
        }
    }

    /// <summary>
    /// 测试model
    /// </summary>
    public class TestModel
    {
        /// <summary>
        /// 测试名称
        /// </summary>
        public string ItemName { get; set; } = "";


        /// <summary>
        /// 对应的命令参数
        /// </summary>
        public string TaskNo { get; set; } = "";
        /// <summary>
        /// 下限
        /// </summary>
        public string LowStr { get; set; } = "--";

        /// <summary>
        /// 上限  
        /// </summary>
        public string UpStr { get; set; } = "--";

        /// <summary>
        ///单位
        /// </summary>
        public string DanWei { get; set; } = "";

 

        /// <summary>
        /// 获取的值
        /// </summary>
        public object Value { get; set; } = "";

     




        /// <summary>
        /// true  表示要上传mes
        /// </summary>
        public bool IsMes { get; set; } = false;

        public string JieGuos { get; set; } = "";

        /// <summary>
        /// 序号ID
        /// </summary>
        public string XuHaoID { get; set; } = "";

   

        /// <summary>
        /// 开始时间
        /// </summary>
        public string KaiShiTime { get; set; } = "";

        /// <summary>
        /// 总时间
        /// </summary>
        public string JieSuTime { get; set; } = "";

        public string TestTime { get; set; } = "0";


    }
}
