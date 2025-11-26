using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using ATSJianMianJK.Mes;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei.GongNengLei
{
    public abstract  class ABSGongNengLei
    {
        public int SheBeiID { get; set; } = -1;

        public string SheBeiName { get; set; } = "";

        public bool IsTiaoShi { get; set; } = false;

        public SheBeiType SheBeiType { get; set; } = SheBeiType.JuanChengLoaHua;
        public abstract void IniData(SheBeiZhanModel model);


        public abstract void Close();

        public virtual void CaoZuo(DoType doType,JieMianCaoZuoModel model)
        { 

        }

        protected  void WriteLog(RiJiEnum riji, string msg)
        {
            RiJiLog.Cerate().Add(riji, $"{SheBeiName}:{msg}", SheBeiID);
        }

        /// <summary>
        /// 1  是开始 2是步骤上传 3是结束
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mazhi"></param>
        /// <param name="isjieguo"></param>
        /// <param name="qitama"></param>
        /// <param name="testmodel"></param>
        /// <returns></returns>
        protected bool ShangMes(int type, string mazhi, bool isjieguo,string qitama, YeWuDataModel testmodel = null)
        {
            if (IsTiaoShi == false)
            {
                if (type == 1)
                {
                    ShangChuanDataModel mesmodel = new ShangChuanDataModel();
                    mesmodel.GuoChengMa = mazhi;
                    mesmodel.KaiShiModel.IsShouZhan = false;
                    mesmodel.KaiShiModel.QiTaZhi = qitama;
                    mesmodel.ShangChuanType = ShangChuanType.KaiShi;
                    mesmodel.TDID = SheBeiID;
                    mesmodel.TDName = SheBeiName;
                    LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
                    if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.Pass)
                    {
                        WriteLog(RiJiEnum.MesData, $"{mazhi}入站成功:{jieguo.NeiRong}");
                        return true;
                    }
                    else
                    {
                        WriteLog(RiJiEnum.MesData, $"{mazhi}入站失败:{jieguo.NeiRong}");
                        return false;
                    }
                }
                else if (type == 2)
                {
                    ShangChuanDataModel mesmodel = new ShangChuanDataModel();
                    mesmodel.GuoChengMa = mazhi;
                    mesmodel.TDID = SheBeiID;
                    mesmodel.TDName = SheBeiName;
                    mesmodel.BuZhouModel.BuZhouShuJu = testmodel;
                    mesmodel.BuZhouModel.JieGuo = testmodel.IsHeGe;
                    mesmodel.ShangChuanType = ShangChuanType.BuZhouShangChuan;
                    LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
                    if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.Pass)
                    {
                        WriteLog(RiJiEnum.MesData, $"{mazhi}步骤数据成功:{jieguo.NeiRong}");
                        return true;
                    }
                    else
                    {
                        WriteLog(RiJiEnum.MesData, $"{mazhi}步骤数据失败:{jieguo.NeiRong}");
                        return false;
                    }
                }
                else if (type==3)
                {
                    ShangChuanDataModel mesmodel = new ShangChuanDataModel();
                    mesmodel.GuoChengMa = mazhi;
                    mesmodel.TDID = SheBeiID;
                    mesmodel.TDName = SheBeiName;
                    mesmodel.JieSuModel.IsHeGe = isjieguo;
                    mesmodel.ShangChuanType = ShangChuanType.JieSu;
                    LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
                    if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.Pass)
                    {
                        WriteLog(RiJiEnum.MesData, $"{mazhi}出站成功:{jieguo.NeiRong}");
                        return true;
                    }
                    else
                    {
                        WriteLog(RiJiEnum.MesData, $"{mazhi}出站失败:{jieguo.NeiRong}");
                        return false;
                    }
                }
            }
            else
            {
                WriteLog(RiJiEnum.MesData, $"调试设备");
            }
            return true;
        }




    

       
    }
}
