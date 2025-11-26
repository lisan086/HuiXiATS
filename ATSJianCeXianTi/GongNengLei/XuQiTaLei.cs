using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.GongNengLei;

namespace ATSJianCeXianTi.GongNengLei
{
    public  class XuQiTaLei
    {

        public virtual List<XuQiuModel> GetFangFaMing()
        {
            return new List<XuQiuModel>();
        }

        public virtual void FanHuiCanShu(TestModel model,int tdid)
        {
            List<object> canshu = new List<object>();
            if (model.HuanCunBiaoShi.Count > 0)
            {
                for (int i = 0; i < model.HuanCunBiaoShi.Count; i++)
                {
                    if (model.HuanCunBiaoShi[i].HuanCunCaoZuoType == HuanCunCaoZuoType.FuZhiHuanCun)
                    {
                        canshu.Add(HuanCunLei.Cerate().GetHuanCun(tdid, model.HuanCunBiaoShi[i].HuanCunName,""));
                    }
                    
                }
            }
            if (canshu.Count > 0)
            {
                model.CMDCanShu = string.Format(model.CMDCanShu.ToString(), canshu.ToArray());
            }
        }

        public virtual void FuZhiCanShu(TestModel model,int tdid,object value)
        {
            if (model.HuanCunBiaoShi.Count > 0)
            {
                for (int i = 0; i < model.HuanCunBiaoShi.Count; i++)
                {
                    if (model.HuanCunBiaoShi[i].HuanCunCaoZuoType == HuanCunCaoZuoType.BaoCunHunCun)
                    {
                        HuanCunLei.Cerate().SetHuanCun(tdid, model.HuanCunBiaoShi[i].HuanCunName, value);
                    }
                    
                }
            }
        }
        public virtual void ZhiXingZhiHou(string hanshuming,object canshu)
        { 

        }
        public virtual void ZhiXingZhiQian(string hanshuming, object canshu)
        {

        }
    }
}
