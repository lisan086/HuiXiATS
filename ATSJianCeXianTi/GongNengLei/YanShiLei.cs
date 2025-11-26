using ATSJianCeXianTi.GongNengLei.Model;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using SSheBei.ZongKongZhi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSJianCeXianTi.GongNengLei
{
    public  class YanShiLei : ABSGongNengLei
    {
        private bool IsTiaoChu = true;
   

        protected override ZhiJieGuo ZhiXing(TestModel model, DengDaiModel canshumodel)
        {
            DateTime time = DateTime.Now;
            double shijian = ChangYong.TryDouble(canshumodel.CanShu,0);
            for (; IsTiaoChu; )
            {
                if ((DateTime.Now- time).TotalMilliseconds>= shijian)
                {
                    break;
                }
                Thread.Sleep(1);
            }
           
            ZhiJieGuo jieguomodel = new ZhiJieGuo();
            jieguomodel.RecZhi= canshumodel.CanShu;
            jieguomodel.IsHeGe = true;
            jieguomodel.IsString = 2;
            return jieguomodel;
        }

        public override string GetTestBiaoZhi()
        {
            return "延时动作";
        }

      

        protected override void IniData()
        {
            IsTiaoChu = true;
        }

        public override void TiaoChu(int biaozhi)
        {
            if (biaozhi==1)
            {
                IsTiaoChu = false;
            }
        }

        public override List<string> GetLeiXing()
        {
            return new List<string>() { "无" };
        }

        protected override List<string> GetMeiYouPanDuanFangShi()
        {
            List<string> kis =new List<string>();
            List<string> fangshis = ChangYong.MeiJuLisName(typeof(PanDuanType));
            for (int i = 0; i < fangshis.Count; i++)
            {
                if (fangshis[i].Equals("不判断") == false)
                {
                    kis.Add(fangshis[i]);
                }
            }
            return kis;
        }

        public override int GetXuYaoSheBei()
        {
            return 2;
        }

       

        public override void UIFanHuiJieGuo(ZhiJieGuo model)
        {
           
        }
    }
}
