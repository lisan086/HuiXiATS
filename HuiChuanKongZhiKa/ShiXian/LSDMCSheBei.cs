using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using JieMianLei.UC;
using LeiSaiDMC.Frm;
using LeiSaiDMC.Model;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static LeiSaiDMC.ShiXian.Imc60;

namespace LeiSaiDMC.ShiXian
{
    /// <summary>
    /// 雷赛DMC设备
    /// </summary>
    public  class LSDMCSheBei : ABSNSheBei
    {

      
        private PeiZhiLei PeiZhiLei;
        /// <summary>
        /// 线程总开关
        /// </summary>
        private bool ZongKaiGuan = false;
        /// <summary>
        /// true  表示线程开始工作
        /// </summary>
        private bool DengDaiOpen = false;
        public override string SheBeiType
        {
            get
            {
                return "汇川的控制卡";
            }
        }

        public override string BanBenHao
        {
            get
            {
                return "V1.0";
            }
        }

        /// <summary>
        /// 通信不起作用
        /// </summary>
        public override bool TongXin
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 写过来的指令
        /// </summary>
        private Dictionary<int, FanXingJiHeLei<CunModel>> SengData = new Dictionary<int, FanXingJiHeLei<CunModel>>();

        public LSDMCSheBei()
        {
            ZongKaiGuan = true;
            PeiZhiLei = new PeiZhiLei();
        }

        public override void IniData(bool ispeizhi)
        {
            PeiZhiLei.IsPeiZhi = ispeizhi;
            PeiZhiLei.WenJianName = PeiZhiObjName;

            if (ispeizhi == false)
            {
                PeiZhiLei.IniData(SheBeiID, SheBeiName);
                PeiZhiLei.XieIOEvent += PeiZhiLei_AnJianZhou;
                List<LSModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (SengData.ContainsKey(sModels[i].SheBeiID) == false)
                    {
                     
                        SengData.Add(sModels[i].SheBeiID, new FanXingJiHeLei<CunModel>());
                        Thread xiancheng = new Thread(ReadWork);
                        xiancheng.IsBackground = true;
                        xiancheng.DisableComObjectEagerCleanup();
                        xiancheng.Start(sModels[i]);
                        Thread xiancheng1 = new Thread(XieWork);
                        xiancheng1.IsBackground = true;
                        xiancheng1.DisableComObjectEagerCleanup();
                        xiancheng1.Start(sModels[i]);
                    }
                }
            }
        }

        private void PeiZhiLei_AnJianZhou(JiCunQiModel model)
        {
            XieShuJu(new List<JiCunQiModel>() { model });
        }

        public override void Open()
        {
            try
            {
                bool ishege = true;
                short nCardNum = 0;
                short[] pCardIndex = new short[4]; // 用于接收所有卡号
                UInt32 ret = Imc60.IMC_GetCardsNum(ref nCardNum, pCardIndex);
                if (ret != 0)
                {
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang,$"获取轴卡失败,错误代码为0x{ret.ToString("x8")}");
                    ishege = false;
                }
                if (ishege)
                {
                    if (nCardNum<=0)
                    {
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"没有找到卡");
                        ishege = false;
                    }
                }
                if (ishege)
                {
                    List<short> liska = pCardIndex.ToList();
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"搜索到卡号:{ChangYong.FenGeDaBao<short>(liska," ")}");
                    List<LSModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                    for (int i = 0; i < sModels.Count; i++)
                    {
                        short kaid = sModels[i].CardNO;
                        if (liska.IndexOf(kaid) >= 0)
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{sModels[i].Name}:找到该卡号:{kaid}");
                            bool zhen=  IniKa(sModels[i]);
                            PeiZhiLei.DataMoXing.SetTX(sModels[i].SheBeiID, zhen);
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{sModels[i].Name}:没有找到该卡号:{kaid}");
                            PeiZhiLei.DataMoXing.SetTX(sModels[i].SheBeiID,false);
                        }
                    }
                }

                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "控制卡链接成功");
            }
            catch(Exception ex)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"汇川控制卡连接:{ex}");
            }           
            DengDaiOpen = true;
          

        }
        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(100);
            try
            {
                List<LSModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    try
                    {
                        short kaid = sModels[i].CardNO;
                        uint ret = Imc60.IMC_DelEcatComm(kaid);
                    }
                    catch 
                    {

                      
                    }
                  
                }
               
                
            }
            catch 
            {

               
            }
            try
            {
                List<LSModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    try
                    {
                        short kaid = sModels[i].CardNO;
                        uint ret = Imc60.IMC_CloseCard(kaid);
                    }
                    catch
                    {


                    }

                }


            }
            catch
            {


            }
        }

        private bool IniKa(LSModel model)
        {
            //【1】开卡
            uint ret = Imc60.IMC_OpenCard(model.CardNO);//根据卡号开卡，默认是从0开始，在PC端分配板卡资源
            if (ret != 0)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:初始化卡:{model.CardNO}失败,错误代码为0x:{ret.ToString("x8")}");
            
                return false;
            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:初始化卡:{model.CardNO}成功");             
            }
            //【2】下载设备参数
            ret = Imc60.IMC_DownLoadDeviceConfig(model.CardNO, model.DevWenJianXML);//网络组态配置文件下发到板卡，用于建立通讯
            if (ret != 0)
            {
             
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:下载设备参数失败{model.DevWenJianXML},错误代码为0x:{ret.ToString("x8")}");
                return false;
            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:下载设备参数成功{model.DevWenJianXML}");
            }
            //【3】获取总线配置(检索eni文件中的板卡网口配置，网口1和网口2是否都enbale)
            short masterCfg = 0;
            ret = Imc60.IMC_GetMasterCfgXml(model.CardNO, ref masterCfg);//从下载到板卡的组态文件中解析出EtherCat网口的配置
            if (ret != 0)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:检索IMC_GetMasterCfgXml 失败{model.DevWenJianXML},错误代码为0x:{ret.ToString("x8")}");
                return false;
            }
            else
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:检索IMC_GetMasterCfgXml 成功{model.DevWenJianXML}");
            }
            //【4】启动主站(网口0是通用EtherCat网口)，网口0——Enable之后，bit0置1，就是0x01=01b

            if ((masterCfg & 0x1) != 0)
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:正在启动EtherCAT0...请稍候...");
            
                uint masterSts = 0;
                ret = Imc60.IMC_GetEcatMasterSts(model.CardNO, ref masterSts);
                if (ret != 0)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:获取Ecat0状态失败,错误代码为0x:{ret.ToString("x8")}");
                 
                    return false;
                }
                else
                {
                    if (masterSts != 6)
                    {
                        ret = Imc60.IMC_ScanCardEcat(model.CardNO);      //默认阻塞式启动EtherCAT，包括配置了Local bus后建立Local bus通讯
                        if (ret != 0)
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:启动EtherCAT失败,错误代码为0x:{ret.ToString("x8")}");
                        
                            return false;
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:启动EtherCAT0成功");
                         
                            //【5】下载系统参数（板卡以及轴参数配置信息）
                         //   ret = Imc60.IMC_DownLoadSystemConfig(model.CardNO, model.SysWenJianXML);//当板卡建立了与从站的通讯之后，就能知道网络的资源，然后配置各资源参数并下发
                            if (ret != 0)
                            {
                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:下载系统参数失败{model.SysWenJianXML},错误代码为0x:{ret.ToString("x8")}");
                               
                                return false;
                            }
                            else
                            {
                                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:下载系统参数成功{model.SysWenJianXML}");
                               
                            }
                        }
                    }
                    else
                    {
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:启动EtherCAT0成功");
                      
                    }
                }
            }


            if ((masterCfg & 0x2) != 0)//网口1是高速IO口，网口1——Enable之后，bit1置1，就是0x02=10b
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:正在启动EtherCAT1...请稍候...");
                uint HmasterSts = 0;
                ret = Imc60.IMC_H_GetEcatMasterSts(model.CardNO, ref HmasterSts);
                if (ret != 0)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:获取Ecat1状态失败,错误代码为0x:{ret.ToString("x8")}");
                 
                    return false;
                }
                else
                {
                    if (HmasterSts != 6)
                    {
                        ret = Imc60.IMC_H_ScanCardEcat(model.CardNO);      //默认阻塞式启动EtherCAT
                        if (ret != 0)
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:启动EtherCAT1失败,错误代码为0x:{ret.ToString("x8")}");
                         
                            return false;
                        }
                    }
                    else
                    {
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:启动EtherCAT1成功");
                       
                    }
                }
            }
            //【6】扫描卡内资源，初始化ComboBox
            Imc60.TMasterInfo tMasterInfo = new Imc60.TMasterInfo();//实例化板卡外设硬件资源
            ret = Imc60.IMC_GetEcatMasterInfo(model.CardNO, ref tMasterInfo);
            if (ret != 0)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:扫描系统资源失败,错误代码为0x:{ret.ToString("x8")}");
             
                return false;
            }
            else
            {
                List<ZhouModel> liszhous = model.LisZhouModel;
                for (int i = 0; i < liszhous.Count; i++)
                {
                    int zhouhao = liszhous[i].ZhouNO;
                    if (zhouhao < tMasterInfo.axisCnt && zhouhao >= 0)
                    {
                        liszhous[i].IsZaiXian = true;
                    }
                    else
                    {
                        liszhous[i].IsZaiXian = false;
                    }
                }

                //ret= Imc60.IMC_StartEcatComm(model.CardNO);
                //if (ret!=0)
                //{
                //    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{model.Name}:建立与从卡连接失败,错误代码为0x:{ret.ToString("x8")}");

                //    return false;
                //}
                //IO
                if (model.IsDuanXianBaoChi)
                {
                 
                    ret = Imc60.IMC_SetEcatDoStsHold(model.CardNO, 1);
                }
                
            }
            return true;
        }

        public override void XieShuJu(List<JiCunQiModel> canshus)
        {
            if (canshus == null || canshus.Count == 0)
            {
                return;
            }
            if (DengDaiOpen)
            {
                for (int i = 0; i < canshus.Count; i++)
                {
                    CunModel cunmodel = PeiZhiLei.DataMoXing.GetCunModel(canshus[i],true);
                    if (cunmodel!=null)
                    {
                        if (cunmodel.CanShuType.ToString().ToLower().StartsWith("xie")|| cunmodel.CanShuType.ToString().Equals("DuXieIO"))
                        {                        
                            if (SengData.ContainsKey(cunmodel.SheBeiID))
                            {
                                PeiZhiLei.DataMoXing.SetSate(cunmodel, 0);
                                cunmodel.JiCunQi = canshus[i];
                                SengData[cunmodel.SheBeiID].Add(cunmodel);
                            }
                        }
                    }
                }
               
            }
        }

        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.SheBeiID = jicunqiid.SheBeiID;
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            CunModel zhen = PeiZhiLei.DataMoXing.GetCunModel(jicunqiid,false);
            if (zhen!=null)
            {
                if (zhen.JiCunQi.IsKeKao)
                {
                    if (zhen.IsWanCheng == 1)
                    {
                        models.Value = zhen.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                    }
                    else if (zhen.IsWanCheng == 2)
                    {
                        models.Value = zhen.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                    else if (zhen.IsWanCheng == 3)
                    {
                        models.Value = zhen.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                    else
                    {
                        models.Value = zhen.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.JingXingZhong;
                    }
                }
                else
                {
                    models.Value = "数据不可靠";
                    models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                }
              
            }
            else
            {
                models.Value = "没有找到";
                models.IsZuiZhongJieGuo = JieGuoType.MeiZhaoDaoJiGuo;
            }
       
            return models;
        }



        public override List<JiCunQiModel> PeiZhiDuXie(int type)
        {
            List<JiCunQiModel> shuju = new List<JiCunQiModel>();
            if (type == 1)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;
                shuju = Get;
            }
            else if (type == 2)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisXie;
                shuju = Get;
            }
            else if (type == 3)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDuXie;

                shuju = Get;
            }
            return shuju;
        }



        public override JieMianFrmModel GetFrm(bool istiaoshi)
        {
            PeiZhiLei.IsTiaoShi = istiaoshi;
            JieMianFrmModel jieMianFrmModel = new JieMianFrmModel();
            jieMianFrmModel.SheBeiName = SheBeiName;
            jieMianFrmModel.SheBeiID = SheBeiID;
            jieMianFrmModel.Form = new PeiZhiJieMian(PeiZhiLei);
            return jieMianFrmModel;
        }

        public override List<JiCunQiModel> GetShuJu()
        {
            if (PeiZhiLei != null)
            {
                return PeiZhiLei.DataMoXing.LisDu;
            }
            return new List<JiCunQiModel>();
        }


        private void ReadWork(object shebei)
        {
            LSModel zsmodel = null;
            if (shebei is LSModel)
            {
                zsmodel= (LSModel)shebei;
            }
            if (zsmodel==null)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"设备为空");
                return;
            }
            int yanshi = 5;
            DateTime chongliantime = DateTime.Now;
          
           
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (zsmodel.TX==false)
                {
                    if ((DateTime.Now- chongliantime).TotalMilliseconds>=1500)
                    {
                        IniKa(zsmodel);
                        chongliantime = DateTime.Now;
                    }
                    Thread.Sleep(10);
                    continue;
                }
                try
                {
                
                    {
                        //StringBuilder sb = new StringBuilder();
                        //List<ZhouModel> liszhous = zsmodel.LisZhouModel;
                        //for (int i = 0; i < liszhous.Count; i++)
                        //{
                        //    if (liszhous[i].IsZaiXian)
                        //    {
                        //        {
                        //            double[] nTimerAxPrfPos = { 0 };
                        //            uint ret = Imc60.IMC_GetAxEncPos(zsmodel.CardNO, liszhous[i].ZhouNO, nTimerAxPrfPos, 1);//轴编码器位置
                        //            if (ret != 0)
                        //            {
                        //                sb.AppendLine($"轴状态监控，查询轴反馈位置失败,错误代码为0x:{ret.ToString("x8")}");

                        //            }
                        //            else
                        //            {
                        //                PeiZhiLei.DataMoXing.SetZhouDu(liszhous[i],IOType.Zhou位置, nTimerAxPrfPos[0]);
                        //            }
                        //        }
                        //        {
                        //            double[] nTimerAxPrfPos = { 0 };
                        //            uint ret = Imc60.IMC_GetAxEncPos(zsmodel.CardNO, liszhous[i].ZhouNO, nTimerAxPrfPos, 1);//轴编码器位置
                        //            if (ret != 0)
                        //            {
                        //                sb.AppendLine($"轴状态监控，查询轴反馈位置失败,错误代码为0x:{ret.ToString("x8")}");

                        //            }
                        //            else
                        //            {
                        //                PeiZhiLei.DataMoXing.SetZhouDu(liszhous[i], IOType.Zhou位置, nTimerAxPrfPos[0]);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    StringBuilder sb = new StringBuilder();
                    {
                        //轴位置
                        {
                           
                            if (zsmodel.LisZhouModel.Count > 0)
                            {
                                short cosunt = (short)zsmodel.LisZhouModel.Count;
                                double[] nTimerAxPrfPos = new double[cosunt];
                                uint ret = Imc60.IMC_GetAxEncPos(zsmodel.CardNO, zsmodel.LisZhouModel[0].ZhouNO, nTimerAxPrfPos, cosunt);//轴编码器位置
                                if (ret != 0)
                                {
                                    sb.AppendLine($"轴状态监控，查询轴反馈位置失败,错误代码为0x:{ret.ToString("x8")}");

                                }
                                else
                                {
                                    for (int i = 0; i < zsmodel.LisZhouModel.Count; i++)
                                    {
                                        PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou位置, nTimerAxPrfPos[i]);
                                        if (nTimerAxPrfPos[i]> zsmodel.LisZhouModel[i].JuLiZuiGao|| nTimerAxPrfPos[i]< zsmodel.LisZhouModel[i].JuLiZuiDi)
                                        {
                                            Imc60.IMC_StopMove(zsmodel.CardNO, zsmodel.LisZhouModel[i].ZhouNO, 1);
                                        }
                                       
                                    }
                                 
                                }
                            }
                        }
                        //轴目标位置
                        {
                           
                            if (zsmodel.LisZhouModel.Count > 0)
                            {
                                short cosunt = (short)zsmodel.LisZhouModel.Count;
                                double[] nTimerAxPrfPos = new double[cosunt];
                                uint ret = Imc60.IMC_GetAxPrfPos(zsmodel.CardNO, zsmodel.LisZhouModel[0].ZhouNO, nTimerAxPrfPos, cosunt);//轴编码器位置
                                if (ret != 0)
                                {
                                    sb.AppendLine($"轴状态监控，查询轴反馈位置失败,错误代码为0x:{ret.ToString("x8")}");

                                }
                                else
                                {
                                    for (int i = 0; i < zsmodel.LisZhouModel.Count; i++)
                                    {
                                        PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou目标位置, nTimerAxPrfPos[i]);
                                    }

                                }
                            }
                        }
                        //轴速度
                        {
                         
                            if (zsmodel.LisZhouModel.Count > 0)
                            {
                                short cosunt = (short)zsmodel.LisZhouModel.Count;
                                double[] nTimerAxPrfPos = new double[cosunt];
                                uint ret = Imc60.IMC_GetAxEncVel(zsmodel.CardNO, zsmodel.LisZhouModel[0].ZhouNO, nTimerAxPrfPos, cosunt);//轴编码器位置
                                if (ret != 0)
                                {
                                    sb.AppendLine($"轴状态监控，查询轴反馈速度失败,错误代码为0x0x:{ret.ToString("x8")}");

                                }
                                else
                                {
                                    for (int i = 0; i < zsmodel.LisZhouModel.Count; i++)
                                    {
                                        PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou速度, nTimerAxPrfPos[i]);
                                        int shifouyuan = nTimerAxPrfPos[i] != 0?1:0;
                                        PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou运行状态, shifouyuan);
                                    }

                                }
                            }
                        }
                        //Zhou状态
                        {
                         
                            if (zsmodel.LisZhouModel.Count > 0)
                            {
                                short cosunt = (short)zsmodel.LisZhouModel.Count;
                                int[] nTimerAxPrfPos = new int[cosunt];
                                uint ret = Imc60.IMC_GetAxSts(zsmodel.CardNO, zsmodel.LisZhouModel[0].ZhouNO, nTimerAxPrfPos, cosunt);//轴编码器位置
                                if (ret != 0)
                                {
                                    sb.AppendLine($"轴状态监控，查询轴状态失败,错误代码为0x:{ret.ToString("x8")}");

                                }
                                else
                                {
                                    for (int i = 0; i < zsmodel.LisZhouModel.Count; i++)
                                    {
                                        //轴报警
                                        {
                                            int jise = ((nTimerAxPrfPos[i] & 0x01) == 0x01)?1:0;
                                            PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou报警, jise);
                                            if (jise==1)
                                            {
                                                Imc60.IMC_StopMove(zsmodel.CardNO, zsmodel.LisZhouModel[i].ZhouNO, 1);
                                            }
                                        }
                                        //轴报警
                                        {
                                            int jise = ((nTimerAxPrfPos[i] & 0x400) == 0x400) ? 1 : 0;
                                            PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou在线, jise);
                                           
                                        }
                                        //使能
                                        {
                                            int jise = ((nTimerAxPrfPos[i] & 0x02) == 0x02) ? 1 : 0;
                                            PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou使能, jise);
                                        }
                                        //到位信号i
                                        {
                                            int jise = ((nTimerAxPrfPos[i] & 0x08) == 0x08) ? 1 : 0;
                                            PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou到位, jise);
                                        }
                                        {
                                            int jise = ((nTimerAxPrfPos[i] & 0x200) == 0x200) ? 1 : 0;
                                            PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou急停, jise);
                                        }
                                        {
                                            int jise = ((nTimerAxPrfPos[i] & 0x2000) == 0x2000) ? 1 : 0;
                                            PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou原点, jise);
                                        }
                                        {
                                            int jise = ((nTimerAxPrfPos[i] & 0x04) == 0x04) ? 1 : 0;
                                            PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou忙, jise);
                                        }
                                    }

                                }
                            }
                        }
                        //轴模式
                        {
                           
                            if (zsmodel.LisZhouModel.Count > 0)
                            {
                                short cosunt = (short)zsmodel.LisZhouModel.Count;
                                short[] nTimerAxPrfPos = new short[cosunt];
                                uint ret = Imc60.IMC_GetAxPrfMode(zsmodel.CardNO, zsmodel.LisZhouModel[0].ZhouNO, nTimerAxPrfPos, cosunt);//轴编码器位置
                                if (ret != 0)
                                {
                                    sb.AppendLine($"轴状态监控，查询轴规划模式失败,错误代码为:{ret.ToString("x8")}");

                                }
                                else
                                {
                                    for (int i = 0; i < zsmodel.LisZhouModel.Count; i++)
                                    {
                                        PeiZhiLei.DataMoXing.SetZhouDu(zsmodel.LisZhouModel[i], IOType.Zhou模式,GetMoShi( nTimerAxPrfPos[i]));
                                    }

                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"{zsmodel.Name}读数据发生错误:{ex}");
                   
                }
                             
                Thread.Sleep(yanshi);
            }
        }


        private void XieWork(object shebei)
        {
            LSModel zsmodel = null;
            if (shebei is LSModel)
            {
                zsmodel = (LSModel)shebei;
            }
            if (zsmodel == null)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"设备为空");
                return;
            }
            FanXingJiHeLei<CunModel> Zhiling = SengData[zsmodel.SheBeiID];
            int yanshi = 5;          
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                try
                {
                    int count = Zhiling.GetCount();
                    if (count > 0)
                    {
                        CunModel lis = Zhiling.GetModel_Head_RomeHead();
                        if (lis != null)
                        {
                            XieJiCunQi(lis, zsmodel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{SheBeiName}写数据发生错误:{ex}");

                }
                Thread.Sleep(yanshi);
            }
        }


        private void XieJiCunQi(CunModel shuju, LSModel zsmodel)
        {
            if (shuju.CanShuType==CanShuType.XieShuJu)
            {
                SendDataModel jiexiemodel = ChangYong.HuoQuJsonToShiTi<SendDataModel>(ChangYong.HuoQuJsonStr( shuju.JiCunQi.Value));
                if (jiexiemodel != null)
                {
                    if (jiexiemodel.GongNengType == GongNengType.XieZhou正常配置)
                    {
                        List<ZhouModel> liscunzai = new List<ZhouModel>();
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID,jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                liscunzai.Add(models);
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"有的轴不存在:{jiexiemodel.Zhous[i].ZhouNo}");
                                PeiZhiLei.DataMoXing.SetSate(shuju,2);
                                return;
                            }
                        }
                        for (int i = 0; i < liscunzai.Count; i++)
                        {
                            liscunzai[i].SuDu = jiexiemodel.Zhous[i].SuDu;
                            liscunzai[i].JiaSuDu = jiexiemodel.Zhous[i].JiaSuDu;
                        }
                       
                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);

                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou回零配置)
                    {
                        List<ZhouModel> liscunzai = new List<ZhouModel>();
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                liscunzai.Add(models);
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"回零配置有的轴不存在:{jiexiemodel.Zhous[i].ZhouNo}");
                                PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                                return;
                            }
                        }
                        for (int i = 0; i < liscunzai.Count; i++)
                        {
                            liscunzai[i].HomeSuDu = jiexiemodel.Zhous[i].SuDu;
                            liscunzai[i].HomeJiaSuDu = jiexiemodel.Zhous[i].JiaSuDu;
                            liscunzai[i].HuiLingMoShi = jiexiemodel.Zhous[i].HuiLingType;
                        }

                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);

                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou停止)
                    {
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                Imc60.IMC_StopMove(zsmodel.CardNO, models.ZhouNO, 1);
                            }
                           
                        }
                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);
                        
                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou不使能)
                    {
                       
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                Imc60.IMC_ServoOff(zsmodel.CardNO, models.ZhouNO, 1);
                            }

                        }
                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);
                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou使能)
                    {
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                Imc60.IMC_ServoOn(zsmodel.CardNO, models.ZhouNO, 1);
                            }

                        }
                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);
                       
                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou回零)
                    {

                        List<ZhouModel> liscunzai = new List<ZhouModel>();
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                liscunzai.Add(models);
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"有的轴不存在:{jiexiemodel.Zhous[i].ZhouNo}");
                                PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                                return;
                            }
                        }
                        for (int i = 0; i < liscunzai.Count; i++)
                        {
                            Imc60.THomingPara tHomingPara = new Imc60.THomingPara();
                            if (jiexiemodel.Zhous[i].IsCaiYongPeiZhi == 1)
                            {
                                tHomingPara.homeMethod = (short)liscunzai[i].HuiLingMoShi;
                                tHomingPara.offset = 0;
                                tHomingPara.highVel = (uint)liscunzai[i].HomeSuDu;
                                tHomingPara.lowVel = (uint)(tHomingPara.highVel * 0.2);
                                tHomingPara.acc = (uint)liscunzai[i].HomeJiaSuDu;
                            }
                            else
                            {
                                tHomingPara.homeMethod = (short)jiexiemodel.Zhous[i].HuiLingType;
                                tHomingPara.offset = 0;
                                tHomingPara.highVel = (uint)jiexiemodel.Zhous[i].SuDu;
                                tHomingPara.lowVel = (uint)(tHomingPara.highVel * 0.2);
                                tHomingPara.acc = (uint)jiexiemodel.Zhous[i].JiaSuDu;
                            }
                            uint rec = Imc60.IMC_StartHoming(zsmodel.CardNO, (short)liscunzai[i].ZhouNO, ref tHomingPara);
                        }

                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);



                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou退出回零)
                    {
                        List<ZhouModel> liscunzai = new List<ZhouModel>();
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                liscunzai.Add(models);
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"有的轴不存在:{jiexiemodel.Zhous[i].ZhouNo}");
                                PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                                return;
                            }
                        }
                        for (int i = 0; i < liscunzai.Count; i++)
                        {
                            uint ret = Imc60.IMC_FinishHoming(zsmodel.CardNO, (short)liscunzai[i].ZhouNO); //0：表示平滑停止 1：表示急停
                        }

                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);

                      
                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou相对位置运动)
                    {

                        List<ZhouModel> liscunzai = new List<ZhouModel>();
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                liscunzai.Add(models);
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"有的轴不存在:{jiexiemodel.Zhous[i].ZhouNo}");
                                PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                                return;
                            }
                        }
                        for (int i = 0; i < liscunzai.Count; i++)
                        {
                            double ptpVel = 0;
                            double ptpAcc = 0;
                            double ptpDec = 0;
                            if (jiexiemodel.Zhous[i].IsCaiYongPeiZhi == 1)
                            {
                                ptpVel = liscunzai[i].SuDu;
                                ptpAcc= liscunzai[i].JiaSuDu;
                                ptpDec = liscunzai[i].JiaSuDu;
                            }
                            else
                            {
                                ptpVel = jiexiemodel.Zhous[i].SuDu;
                                ptpAcc = jiexiemodel.Zhous[i].JiaSuDu;
                                ptpDec = jiexiemodel.Zhous[i].JiaSuDu;
                               
                            }                       
                            uint ret = Imc60.IMC_SetAxMvPara(zsmodel.CardNO, (short)liscunzai[i].ZhouNO, ptpVel, ptpAcc, ptpDec);//ptp运动参数，这里的运动参数是运动指令过程自由执行的参数
                            if (ret == 0)
                            {
                                double weizhi = jiexiemodel.Zhous[i].WeiZhi;
                              
                                //【3】启动PTP
                                double ptpPos = weizhi;
                                Int16 posType = 1;
                                ret = Imc60.IMC_StartPtpMove(zsmodel.CardNO, (short)liscunzai[i].ZhouNO, ptpPos, posType);//ptp开始运动

                            }
                          
                        }

                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);

                     

                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou绝对位置运动)
                    {
                        List<ZhouModel> liscunzai = new List<ZhouModel>();
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                liscunzai.Add(models);
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"有的轴不存在:{jiexiemodel.Zhous[i].ZhouNo}");
                                PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                                return;
                            }
                        }
                        for (int i = 0; i < liscunzai.Count; i++)
                        {
                            double ptpVel = 0;
                            double ptpAcc = 0;
                            double ptpDec = 0;
                            if (jiexiemodel.Zhous[i].IsCaiYongPeiZhi == 1)
                            {
                                ptpVel = liscunzai[i].SuDu;
                                ptpAcc = liscunzai[i].JiaSuDu;
                                ptpDec = liscunzai[i].JiaSuDu;
                            }
                            else
                            {
                                ptpVel = jiexiemodel.Zhous[i].SuDu;
                                ptpAcc = jiexiemodel.Zhous[i].JiaSuDu;
                                ptpDec = jiexiemodel.Zhous[i].JiaSuDu;

                            }
                            uint ret = Imc60.IMC_SetAxMvPara(zsmodel.CardNO, (short)liscunzai[i].ZhouNO, ptpVel, ptpAcc, ptpDec);//ptp运动参数，这里的运动参数是运动指令过程自由执行的参数
                            if (ret == 0)
                            {
                                double weizhi = jiexiemodel.Zhous[i].WeiZhi;

                                //【3】启动PTP
                                double ptpPos = weizhi;
                                Int16 posType = 0;
                                ret = Imc60.IMC_StartPtpMove(zsmodel.CardNO, (short)liscunzai[i].ZhouNO, ptpPos, posType);//ptp开始运动

                            }

                        }

                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 1);

                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou恒速运动)
                    {

                        List<ZhouModel> liscunzai = new List<ZhouModel>();
                        for (int i = 0; i < jiexiemodel.Zhous.Count; i++)
                        {
                            ZhouModel models = PeiZhiLei.DataMoXing.GetZhou(zsmodel.SheBeiID, jiexiemodel.Zhous[i].ZhouNo);
                            if (models != null)
                            {
                                liscunzai.Add(models);
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"有的轴不存在:{jiexiemodel.Zhous[i].ZhouNo}");
                                PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                                return;
                            }
                        }
                        for (int i = 0; i < liscunzai.Count; i++)
                        {
                            uint ret = Imc60.IMC_StartJogMove(zsmodel.CardNO, (short)liscunzai[i].ZhouNO, jiexiemodel.Zhous[i].SuDu);//ptp运动参数，这里的运动参数是运动指令过程自由执行的参数

                        }

                     


                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou取消急停)
                    {
                        uint ret = Imc60.IMC_SetEmgTrigLevelInv(zsmodel.CardNO, 1);
                        if (ret==0)
                        {
                           
                            PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                            PeiZhiLei.DataMoXing.SetSate(shuju, 1);
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "NG");
                            PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                        }


                    }
                    else if (jiexiemodel.GongNengType == GongNengType.XieZhou急停)
                    {
                        uint ret = Imc60.IMC_SetEmgTrigLevelInv(zsmodel.CardNO, 0);
                        if (ret == 0)
                        {
                            PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "OK");
                            PeiZhiLei.DataMoXing.SetSate(shuju, 1);

                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "NG");
                            PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                        }


                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"该功能还没有实现:{ChangYong.HuoQuJsonStr(jiexiemodel)}");
                        PeiZhiLei.DataMoXing.SetSate(shuju, 2);
                    }
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, $"数据解析有问题:{shuju.JiCunQi.Value}");
                    PeiZhiLei.DataMoXing.SetSate(shuju, 2);              
                }
            }
            else
            {
                PeiZhiLei.DataMoXing.SetXieJiCunQiZhi(shuju, "不支持该类型");
                PeiZhiLei.DataMoXing.SetSate(shuju, 2);
               
            }
        
        }

        private string GetMoShi(int moshi)
        {
            if (moshi == 0)
            {
                return $"{moshi}:未处于任何规划模式";
            }
            else if (moshi == 1)
            {
                return $"{moshi}:点对点规划模式";
            }
            else if (moshi == 2)
            {
                return $"{moshi}:Jog规划模式";
            }
            else if (moshi == 3)
            {
                return $"{moshi}:电子齿轮规划模式";
            }
            else if (moshi == 4)
            {
                return $"{moshi}:电子凸轮规划模式";
            }
            else if (moshi == 5)
            {
                return $"{moshi}:PVT规划模式";
            }
            else if (moshi == 6)
            {
                return $"{moshi}:龙门规划模式";
            }
            else if (moshi == 7)
            {
                return $"{moshi}:手轮规划模式";
            }
            else if (moshi == 9)
            {
                return $"{moshi}:点位连续规划模式";
            }
            else if (moshi == 11)
            {
                return $"{moshi}:插补同步轴规划模式";
            }
            else if (moshi == 15)
            {
                return $"{moshi}:回零模式";
            }
            else if (moshi == 17)
            {
                return $"{moshi}:坐标系插补规划模式(多轴模式)";
            }
            else if (moshi == 18)
            {
                return $"{moshi}:多轴同步规划模式(多轴模式)";
            }
            else if (moshi == 19)
            {
                return $"{moshi}:绑定 PT 规划模式(多轴模式)";
            }
            return "";
        }
      

      

    

        /// <summary>
        /// 根据返回的值转换相应的IO
        /// </summary>
        /// <param name="rec"></param>
        private List<bool> OrIOB(uint rec)
        {
            List<bool> ios = new List<bool>();
            string shuju = Convert.ToString(rec, 2).PadLeft(32, '0');
            int count = shuju.Length;
            for (int i = count - 1; i >= 0; i--)
            {
                ios.Add(shuju[i] == '1');
            }
            return ios;
        }

        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            CunModel cunModel = PeiZhiLei.DataMoXing.GetCunModel(jiCunQiModel, false);
            if (cunModel != null && cunModel.CanShuType.ToString().ToLower().Contains("xie"))
            {
                //CanShuKJ kj = new CanShuKJ();

                //kj.SetShuJu(cunModel.JiLu,cunModel.IsDu.ToString().Contains("全")==false);
                //return kj;
            }
            return base.GetCanShuKJ(jicunweiyibiaoshi);
        }
        public override TxModel GetMeiGeTx()
        {
            TxModel model = new TxModel();
            model.SheBeiName = SheBeiName;
            model.SheBeiTD = SheBeiID;
            model.SheBeuZu = FenZu;
            bool ischengg = true;

            for (int i = 0; i < PeiZhiLei.DataMoXing.LisSheBei.Count; i++)
            {
                LSModel item = PeiZhiLei.DataMoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.TX;
                zmodel.ZiSheBeiID = item.SheBeiID;
                zmodel.ZiSheBeiName = item.Name;
                if (zmodel.Tx == false)
                {
                    ischengg = false;
                }

                model.LisTx.Add(zmodel);

            }
            model.ZongTX = ischengg;
            return model;
        }
        public override void Clear(bool isquanbu, JiCunQiModel model)
        {
            
        }
    }
}
