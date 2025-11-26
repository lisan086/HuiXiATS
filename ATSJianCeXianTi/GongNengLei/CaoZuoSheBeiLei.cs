using ATSJianCeXianTi.GongNengLei.Model;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using Common.JieMianLei;
using SSheBei.ABSSheBei;
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
    public class CaoZuoSheBeiLei : ABSGongNengLei
    {
        private bool IsTiaoChu = true;
        protected override ZhiJieGuo ZhiXing(TestModel model, DengDaiModel canshumodel)
        {
            ZhiJieGuo jieguomodel = new ZhiJieGuo();
            XieRuMolde xieRuMolde = GetXieModel(model, canshumodel);
            LeiXingType leiXingType = ChangYong.GetMeiJuZhi<LeiXingType>(model.LeiXing);
            if (leiXingType == LeiXingType.XieCaoZuo)
            {
                int cishu = model.CiShu;
                if (cishu <= 0)
                {
                    cishu = 1;
                }
                for (int i = 0; i < cishu; i++)
                {
                    SheBeiJiHe.Cerate().XieShuJu(TDID, xieRuMolde);
                    for (; IsTiaoChu;)
                    {
                        Thread.Sleep(2);
                        JiaoYanJieGuoModel jieguo = ZongSheBeiKongZhi.Cerate().GetIsChengGong(xieRuMolde);
                        if (jieguo.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                        {
                            if (jieguo.IsZuiZhongJieGuo == JieGuoType.BuKeKaoJieGuo)
                            {
                                jieguomodel.RecZhi = "结果不可靠";
                                jieguomodel.IsHeGe = false;
                                jieguomodel.IsString = 1;
                                break;
                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.MeiZhaoDaoJiGuo)
                            {
                                jieguomodel.RecZhi = "没找到";
                                jieguomodel.IsString = 1;
                                jieguomodel.IsHeGe = false;
                                return jieguomodel;
                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.ShiBaiJiGuo)
                            {
                                jieguomodel.RecZhi = jieguo.Value;
                                jieguomodel.IsString = 1;

                                break;
                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                            {
                                jieguomodel = PanDuanHeGe(model, jieguo.Value);
                                if (jieguomodel.IsHeGe)
                                {
                                    return jieguomodel;
                                }
                                else
                                {
                                    break;
                                }

                            }

                        }


                    }
                }


            }
            else if (leiXingType == LeiXingType.DuCaoZuo)
            {
                int cishu = model.CiShu;
                if (cishu <= 0)
                {
                    cishu = 1;
                }
                for (int i = 0; i < cishu; i++)
                {
                 
                    for (; IsTiaoChu;)
                    {
                        Thread.Sleep(2);
                        JiaoYanJieGuoModel jieguo = ZongSheBeiKongZhi.Cerate().GetIsChengGong(xieRuMolde);
                        if (jieguo.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                        {
                            if (jieguo.IsZuiZhongJieGuo == JieGuoType.BuKeKaoJieGuo)
                            {
                                jieguomodel.RecZhi = "结果不可靠";
                                jieguomodel.IsHeGe = false;
                                jieguomodel.IsString = 1;
                                break;
                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.MeiZhaoDaoJiGuo)
                            {
                                jieguomodel.RecZhi = "没找到";
                                jieguomodel.IsString = 1;
                                jieguomodel.IsHeGe = false;
                                return jieguomodel;
                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.ShiBaiJiGuo)
                            {
                                jieguomodel.RecZhi = jieguo.Value;
                                jieguomodel.IsString = 1;

                                break;
                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                            {
                                jieguomodel = PanDuanHeGe(model, jieguo.Value);
                                if (jieguomodel.IsHeGe)
                                {
                                    return jieguomodel;
                                }
                                else
                                {
                                    break;
                                }

                            }

                        }


                    }
                }
            }
            else
            {
                jieguomodel.IsHeGe = false;
                jieguomodel.IsString = 1;
                jieguomodel.RecZhi = "该功能不存在";
            }
            return jieguomodel;
        }

        public override string GetTestBiaoZhi()
        {
            return "操作设备";
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
            List<string> fangshis = ChangYong.MeiJuLisName(typeof(LeiXingType));

            return fangshis;
        }

        protected override List<string> GetMeiYouPanDuanFangShi()
        {
            return new List<string>();
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
            XieCaoZuo,
            DuCaoZuo,
        }
    }
}
