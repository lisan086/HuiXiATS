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

        public SheBeiType SheBeiType { get; set; } = SheBeiType.GongWeiZhan;
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
        /// 进站mes
        /// </summary>
        /// <returns></returns>
        protected bool JinZhanJiaoYanMes(string ketima, string qitama, bool isshouzan)
        {
            ShangChuanDataModel mesmodel = new ShangChuanDataModel();
            mesmodel.GuoChengMa = ketima;
            mesmodel.KaiShiModel.IsShouZhan = isshouzan;
            mesmodel.KaiShiModel.QiTaZhi = qitama;
            mesmodel.ShangChuanType = ShangChuanType.KaiShi;
            mesmodel.TDID = SheBeiID;
            mesmodel.TDName = SheBeiName;
            LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
            if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.Pass)
            {
                WriteLog(RiJiEnum.MesData, $"{ketima}入站成功:{jieguo.NeiRong}");
                return true;
            }
            else
            {
                WriteLog(RiJiEnum.MesData, $"{ketima}入站失败:{jieguo.NeiRong}");
                return false;
            }

        }

        protected bool BangMaJiaoYanMes(string ketima, List<string> mas)
        {
            ShangChuanDataModel mesmodel = new ShangChuanDataModel();
            mesmodel.GuoChengMa = ketima;
            mesmodel.TDID = SheBeiID;
            mesmodel.TDName = SheBeiName;
            mesmodel.BangMaModel.MaS.AddRange(mas);
            mesmodel.ShangChuanType = ShangChuanType.BangMa;
            LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
            if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.Pass)
            {
                WriteLog(RiJiEnum.MesData, $"{ketima}绑码成功:{jieguo.NeiRong}");
                return true;
            }
            else
            {
                WriteLog(RiJiEnum.MesData, $"{ketima}绑码失败:{jieguo.NeiRong}");
                return false;
            }
        }

        protected bool BuZhouShuJuMes(YeWuDataModel yeWuData, string ketima)
        {
            ShangChuanDataModel mesmodel = new ShangChuanDataModel();
            mesmodel.GuoChengMa = ketima;
            mesmodel.TDID = SheBeiID;
            mesmodel.TDName = SheBeiName;
            mesmodel.BuZhouModel.BuZhouShuJu = yeWuData;
            mesmodel.BuZhouModel.JieGuo = yeWuData.IsShuJuHeGe;
            mesmodel.ShangChuanType = ShangChuanType.BuZhouShangChuan;
            LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
            if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.Pass)
            {
                WriteLog(RiJiEnum.MesData, $"{ketima}步骤数据成功:{jieguo.NeiRong}");
                return true;
            }
            else
            {
                WriteLog(RiJiEnum.MesData, $"{ketima}步骤数据失败:{jieguo.NeiRong}");
                return false;
            }
        }

        protected bool ChuZhanMes(string ketima, bool zongjieguo)
        {
            ShangChuanDataModel mesmodel = new ShangChuanDataModel();
            mesmodel.GuoChengMa = ketima;
            mesmodel.TDID = SheBeiID;
            mesmodel.TDName = SheBeiName;
            mesmodel.JieSuModel.IsHeGe = zongjieguo;
            mesmodel.JieSuModel.JieSuTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            mesmodel.ShangChuanType = ShangChuanType.JieSu;
            LianWanModel jieguo = ShangChuanMesLei.Cerate().ShangMes(mesmodel);
            if (jieguo.FanHuiJieGuo == JinZhanJieGuoType.Pass)
            {
                WriteLog(RiJiEnum.MesData, $"{ketima}出站成功:{jieguo.NeiRong}");
                return true;
            }
            else
            {
                WriteLog(RiJiEnum.MesData, $"{ketima}出站失败:{jieguo.NeiRong}");
                return false;
            }
        }
        protected void ChuFaEvent(EventType eventType, JieMianShiJianModel model)
        {
            model.GWID = SheBeiID;
            JieMianCaoZuoLei.CerateDanLi().ChuFaJieMianEvent(eventType, model);
        }

        /// <summary>
        /// 写入数据   1-清零值 2-是合格值 3-不合格值  0是需要参数
        /// </summary>
        /// <param name="caoZuoType"></param>
        /// <param name="zhi"></param>
        /// <param name="leixing"></param>
        protected void XieShuJu(CaoZuoType caoZuoType, object zhi, int leixing, bool isjiaoyan = true)
        {
            if (isjiaoyan)
            {
                SheBeiJiHe.Cerate().XieRuJiaoYanData(caoZuoType, zhi, leixing, SheBeiID);
            }
            else
            {
                SheBeiJiHe.Cerate().XieRuWuJiaoYanData(caoZuoType, zhi, leixing, SheBeiID);
            }
        }

    }
}
