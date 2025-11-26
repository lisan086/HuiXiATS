using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianCeXianTi.GongNengLei.GNKJ;
using ATSJianCeXianTi.GongNengLei.Model;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.Mes;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using SSheBei.ZongKongZhi;

namespace ATSJianCeXianTi.GongNengLei
{
    public class TangChuanLei : ABSGongNengLei
    {
        private int BiaoZhi = 0;
        private bool IsTiaoChu = true;
        private ZhiJieGuo ZhiJieGuo = new ZhiJieGuo();
      

        protected override ZhiJieGuo ZhiXing(TestModel model, DengDaiModel canshumodel)
        {
          
            LeiXingType panDuanType = ChangYong.GetMeiJuZhi<LeiXingType>(model.LeiXing);
            if (panDuanType == LeiXingType.PuTongTangChuan)
            {
                List<string> lis = ChangYong.JieGeStr(canshumodel.CanShu, '#');
                if (lis.Count >= 3)
                {
                    TangChuanUIModel canshu = new TangChuanUIModel();
                    canshu.TDID = TDID;
                    canshu.IsSaoMa = lis[2] == "1";
                    canshu.LowStr = model.LowStr;
                    canshu.LuJing = lis[0];
                    canshu.UpStr = model.UpStr;
                    canshu.XingXi = lis[1];
                    ChuFaUI(canshu, 1);
                    Thread.Sleep(100);
                    for (; IsTiaoChu;)
                    {
                        if (BiaoZhi == 1)
                        {
                            break;
                        }
                        Thread.Sleep(1);
                    }
                    ChuFaUI(canshu, 2);
                    if (BiaoZhi == 1)
                    {
                        model.Value = ZhiJieGuo.RecZhi;
                        PanDuanHeGe(model, model.Value);
                    }
                    ZhiJieGuo jieguomodel = new ZhiJieGuo();
                    jieguomodel.RecZhi = model.Value;
                    jieguomodel.IsHeGe = ZhiJieGuo.IsHeGe;
                    jieguomodel.IsString = model.IsZiFuChuan;
                    return jieguomodel;
                }
                else
                {
                    ZhiJieGuo jieguomodel = new ZhiJieGuo();
                    jieguomodel.RecZhi = "传的数据有问题 没有#";
                    jieguomodel.IsHeGe = false;
                    jieguomodel.IsString = 1;
                    return jieguomodel;
                }
            }
            else if (panDuanType == LeiXingType.HuTuTangChuan)
            {
                string  ducanshu = ChangYong.TryStr(canshumodel.GNCanShu,"");
                if (string.IsNullOrEmpty(ducanshu)==false)
                {
                    XieRuMolde xieRuMolde = GetXieModel(model, canshumodel);
                   
                    ZhiJieGuo jieguomodel = new ZhiJieGuo();
                    SheBeiJiHe.Cerate().XieShuJu(TDID, xieRuMolde);
                  
                    TangChuanUIModel canshu = new TangChuanUIModel();
                    {
                        canshu.TDID= TDID;
                        canshu.IsSaoMa = false;
                        canshu.LowStr = model.LowStr;
                        canshu.LuJing = "";
                        canshu.UpStr = model.UpStr;
                        canshu.XingXi = "";
                        canshu.Type = 2;
                        canshu.LisShuJu = new List<JKKJ.PeiZhi.ZhuXingModel>();
                        ChuFaUI(canshu, 1);
                        Thread.Sleep(300);
                    }
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
                                jieguomodel.IsHeGe = false;
                                break;
                            }
                            else if (jieguo.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                            {
                                if (jieguo.Value is List<byte[]>)
                                {
                                    List<byte[]> shujus = jieguo.Value as List<byte[]>;
                                    int count = shujus.Count;
                                    int shucount = canshu.LisShuJu.Count;
                                    if (count > shucount)
                                    {
                                        int shengxincount = count - shucount;
                                        List<byte[]> xindian = shujus.GetRange(shucount, shengxincount);
                                        for (int i = 0; i < xindian.Count; i++)
                                        {
                                            try
                                            {
                                                JKKJ.PeiZhi.ZhuXingModel modes = new JKKJ.PeiZhi.ZhuXingModel();
                                                modes.DianWei = xindian[i][2];
                                                modes.XValue = CRC.GetInt(new List<byte>() { xindian[i][4], xindian[i][3] }, 0);
                                                modes.YValue = CRC.GetInt(new List<byte>() { xindian[i][6], xindian[i][5] }, 0);
                                                canshu.LisShuJu.Add(modes);
                                            }
                                            catch 
                                            {

                                               
                                            }
                                          
                                        }
                                    }
                                }
                            }

                        }

                        bool isjiesu = DataJiHe.Cerate().GetIOBool(TDID, ducanshu, false);
                        if (isjiesu)
                        {
                            double shuju = 0;
                            bool ishege = canshu.FenXiDianWei(out shuju);
                            jieguomodel.RecZhi = shuju;
                            jieguomodel.IsString = 1;
                            jieguomodel.IsHeGe = ishege;
                            break;
                        }
                        if (BiaoZhi == 1)
                        {
                            jieguomodel.RecZhi = ZhiJieGuo.RecZhi;
                            jieguomodel.IsString = 1;
                            jieguomodel.IsHeGe = ZhiJieGuo.IsHeGe;
                            break;
                        }

                    }
                    JiLvLog(canshu.ToString());
                    ChuFaUI(canshu, 2);
                    return jieguomodel;
                }
                else
                {
                    ZhiJieGuo jieguomodel = new ZhiJieGuo();
                    jieguomodel.RecZhi = "传的数据有问题 没有#";
                    jieguomodel.IsHeGe = false;
                    jieguomodel.IsString = 1;
                    return jieguomodel;
                }
            }
            else
            {
                ZhiJieGuo jieguomodel = new ZhiJieGuo();
                jieguomodel.RecZhi = "传来的类型不支持";
                jieguomodel.IsHeGe = false;
                jieguomodel.IsString = 1;
                return jieguomodel;
            }
          
          
        }

        public override string GetTestBiaoZhi()
        {
            return "弹窗";
        }

     
        protected override void IniData()
        {
            IsTiaoChu = true;
            BiaoZhi = 0;
            ZhiJieGuo = new ZhiJieGuo();
        }

        public override void TiaoChu(int biaozhi)
        {
            if (biaozhi == 1)
            {
                IsTiaoChu = false;
            }
        }

        public override void UIFanHuiJieGuo(ZhiJieGuo model)
        {
            ZhiJieGuo.IsHeGe = model.IsHeGe;
            ZhiJieGuo.IsString = model.IsString;
            ZhiJieGuo.IsXiaYiBu = model.IsXiaYiBu;
            ZhiJieGuo.RecZhi = model.RecZhi;
            BiaoZhi = 1;
        }

        public override List<string> GetLeiXing()
        {
            return ChangYong.MeiJuLisName(typeof(LeiXingType));
        }

        protected override List<string> GetMeiYouPanDuanFangShi()
        {
            List<string> kis = new List<string>();
           
            return kis;
        }

        public override int GetXuYaoSheBei()
        {
            return 3;
        }

       

        private enum LeiXingType
        {
            PuTongTangChuan,
            HuTuTangChuan,
        }
    }
}
