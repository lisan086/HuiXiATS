using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianCeXianTi.GongNengLei.GNKJ;
using ATSJianCeXianTi.GongNengLei.Model;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.Model;
using SSheBei.ZongKongZhi;

namespace ATSJianCeXianTi.GongNengLei
{
    public class DengDaiTuiChuLei : ABSGongNengLei
    {
        private bool IsTiaoChu = true;
      
        protected override ZhiJieGuo ZhiXing(TestModel model, DengDaiModel canshumodel)
        {
            ZhiJieGuo jieguomodel = new ZhiJieGuo();                     
            XieRuMolde xieRuMolde=GetXieModel(model, canshumodel);        
            LeiXingType panDuanType = ChangYong.GetMeiJuZhi<LeiXingType>(model.LeiXing);
            switch (panDuanType)
            {
                case LeiXingType.DuChiXuBuXie:
                    {                      
                        for (; IsTiaoChu;)
                        {
                            Thread.Sleep(3);
                            JiaoYanJieGuoModel jieguo = ZongSheBeiKongZhi.Cerate().GetIsChengGong(xieRuMolde);
                            if (jieguo.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                            {
                                jieguomodel = PanDuanHeGe(model, jieguo.Value);
                                if (jieguomodel.IsHeGe)
                                {
                                    jieguomodel.RecZhi = jieguo.Value;
                                    jieguomodel.IsHeGe = true;
                                    break;
                                }

                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.BuKeKaoJieGuo)
                            {
                                jieguomodel.RecZhi = jieguo.Value;
                                jieguomodel.IsString =1;
                                jieguomodel.IsHeGe = false;
                                break;
                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.MeiZhaoDaoJiGuo)
                            {
                                jieguomodel.RecZhi = jieguo.Value;
                                jieguomodel.IsString = 1;
                                jieguomodel.IsHeGe = false;
                                break;
                            }
                        }
                    }
                    break;           
                case LeiXingType.DuChiXuXie:
                    {                  
                        for (; IsTiaoChu;)
                        {
                            SheBeiJiHe.Cerate().XieShuJu(TDID, xieRuMolde);
                            DateTime shijian = DateTime.Now;
                            bool ischengg = false;
                            for (; IsTiaoChu;)
                            {
                                Thread.Sleep(3);
                                JiaoYanJieGuoModel jieguo = ZongSheBeiKongZhi.Cerate().GetIsChengGong(xieRuMolde);
                                if (jieguo.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                                {
                                    jieguomodel = PanDuanHeGe(model, jieguo.Value);
                                    if (jieguomodel.IsHeGe)
                                    {
                                        jieguomodel.RecZhi = jieguo.Value;
                                        jieguomodel.IsHeGe = true;
                                        ischengg = true;
                                        break;
                                    }

                                }
                                else if (jieguo.IsZuiZhongJieGuo == JieGuoType.BuKeKaoJieGuo)
                                {
                                    ischengg = true;
                                    jieguomodel.RecZhi = jieguo.Value;
                                    jieguomodel.IsString = 1;
                                    jieguomodel.IsHeGe = false;
                                    break;
                                }
                                else if (jieguo.IsZuiZhongJieGuo == JieGuoType.MeiZhaoDaoJiGuo)
                                {
                                    ischengg = true;
                                    jieguomodel.RecZhi = jieguo.Value;
                                    jieguomodel.IsString = 1;
                                    jieguomodel.IsHeGe = false;
                                    break;
                                }

                            }
                            if (ischengg)
                            {
                                break;
                            }

                        }
                    }
                    break;
                case LeiXingType.DuNoChiXuBuXie:
                    {
                        int cishu = model.CiShu;
                        if (cishu <= 0)
                        {
                            cishu = 1;
                        }
                     
                        double timeout = ChangYong.TryDouble(canshumodel.GNCanShu, 3);
                        for (int i = 0; i < cishu; i++)
                        {

                            DateTime dateTime = DateTime.Now;
                            bool ischenggong = false;
                            for (; IsTiaoChu;)
                            {
                                Thread.Sleep(3);
                                JiaoYanJieGuoModel jieguo = ZongSheBeiKongZhi.Cerate().GetIsChengGong(xieRuMolde);
                                if (jieguo.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                                {
                                    jieguomodel = PanDuanHeGe(model, jieguo.Value);
                                    if (jieguomodel.IsHeGe)
                                    {
                                        ischenggong = true;
                                        jieguomodel.RecZhi = jieguo.Value;
                                        jieguomodel.IsHeGe = true;
                                        break;
                                    }

                                }
                                else if (jieguo.IsZuiZhongJieGuo == JieGuoType.BuKeKaoJieGuo)
                                {

                                    jieguomodel.RecZhi = jieguo.Value;
                                    jieguomodel.IsString = 1;
                                    jieguomodel.IsHeGe = false;
                                    break;
                                }
                                else if (jieguo.IsZuiZhongJieGuo == JieGuoType.MeiZhaoDaoJiGuo)
                                {

                                    jieguomodel.RecZhi = jieguo.Value;
                                    jieguomodel.IsString = 1;
                                    jieguomodel.IsHeGe = false;
                                    break;
                                }
                                if ((DateTime.Now - dateTime).TotalSeconds > timeout)
                                {
                                    jieguomodel.RecZhi = "ChaoShi";
                                    jieguomodel.IsString = 1;
                                    jieguomodel.IsHeGe = false;
                                    break;
                                }
                            }
                            if (ischenggong)
                            {
                                break;
                            }
                        }

                    }
                    break;
                case LeiXingType.DuNoChiXuXie:
                    {                    
                        double timeout = ChangYong.TryDouble(canshumodel.GNCanShu, 3);
                        double time = timeout;
                        DateTime dateTime = DateTime.Now;
                        int cishu = model.CiShu;
                        if (cishu <= 0)
                        {
                            cishu = 1;
                        }
                        for (int i = 0; i < cishu; i++)
                        {
                            bool ischenggong = false;
                            for (; IsTiaoChu;)
                            {
                                SheBeiJiHe.Cerate().XieShuJu(TDID, xieRuMolde);
                                DateTime shijian = DateTime.Now;
                                bool ischengg = false;
                                for (; IsTiaoChu;)
                                {
                                    Thread.Sleep(3);
                                    JiaoYanJieGuoModel jieguo = ZongSheBeiKongZhi.Cerate().GetIsChengGong(xieRuMolde);
                                    if (jieguo.IsZuiZhongJieGuo != JieGuoType.ChengGongJiGuo)
                                    {
                                        if (jieguo.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                                        {
                                            jieguomodel = PanDuanHeGe(model, jieguo.Value);
                                            if (jieguomodel.IsHeGe)
                                            {
                                                ischengg = true;

                                                break;

                                            }
                                        }
                                        else if (jieguo.IsZuiZhongJieGuo == JieGuoType.BuKeKaoJieGuo)
                                        {
                                            ischengg = true;
                                            jieguomodel.RecZhi = jieguo.Value;
                                            jieguomodel.IsString = 1;
                                            jieguomodel.IsHeGe = false;
                                            break;
                                        }
                                        else if (jieguo.IsZuiZhongJieGuo == JieGuoType.MeiZhaoDaoJiGuo)
                                        {
                                            ischengg = true;
                                            jieguomodel.RecZhi = jieguo.Value;
                                            jieguomodel.IsString = 1;
                                            jieguomodel.IsHeGe = false;
                                            break;
                                        }
                                        break;
                                    }

                                    if ((DateTime.Now - dateTime).TotalMilliseconds > 500)
                                    {
                                        break;
                                    }
                                }
                                if (ischengg)
                                {
                                    ischenggong = true;
                                    break;
                                }

                                if ((DateTime.Now - dateTime).TotalSeconds > time)
                                {
                                    jieguomodel.RecZhi = "ChaoShi";
                                    jieguomodel.IsString = 1;
                                    jieguomodel.IsHeGe = false;
                                    break;
                                }
                            }
                            if (ischenggong)
                            {
                                break;
                            }
                        }
                    }
                    break;
                default:
                    {
                      
                        jieguomodel.RecZhi ="不存在该功能";
                        jieguomodel.IsString = 1;
                        jieguomodel.IsHeGe = false;
                    }
                    break;
            }
                
            return jieguomodel;
        }

        public override string GetTestBiaoZhi()
        {
            return "等待退出";
        }

  

        protected override void IniData()
        {
            IsTiaoChu = true;
        }
   

        public override void TiaoChu(int biaozhi)
        {
            if (biaozhi == 1)
            {
                IsTiaoChu = false;
            }
        }

     
      
        
        public override List<string> GetLeiXing()
        {
            List<string> fangshis = ChangYong.MeiJuLisName(typeof(LeiXingType));

            return fangshis;
        }

        protected override List<string> GetMeiYouPanDuanFangShi()
        {
            List<string> fangshis = new List<string>();
            fangshis.Add(PanDuanType.不判断.ToString());
            return fangshis;
        }

        public override int GetXuYaoSheBei()
        {
            return 1;
        }

     

        public override void UIFanHuiJieGuo(ZhiJieGuo model)
        {
            
        }

        private enum LeiXingType
        {
            DuChiXuBuXie,
            DuChiXuXie,
            DuNoChiXuBuXie,
            DuNoChiXuXie,           
        }
       
    }
}
