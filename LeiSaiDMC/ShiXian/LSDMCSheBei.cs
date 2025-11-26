using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
using static System.Net.Mime.MediaTypeNames;

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
                return "雷赛DMC3000";
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
                if (PeiZhiLei!=null&& PeiZhiLei.LisSheBei.Count>0)
                {
                    bool zhen = true;
                    for (int i = 0; i < PeiZhiLei.LisSheBei.Count; i++)
                    {
                        if (PeiZhiLei.LisSheBei[i].TX==false)
                        {
                            return false;
                        }
                    }
                    return zhen;
                }
                return false;
            }
        }

        /// <summary>
        /// 写过来的指令
        /// </summary>
        private FanXingJiHeLei<List<JiCunQiModel>> SengData = new FanXingJiHeLei<List<JiCunQiModel>>();

        public LSDMCSheBei()
        {
            ZongKaiGuan = true;
            PeiZhiLei = new PeiZhiLei();
        }

        public override void IniData(bool ispeizhi)
        {
            PeiZhiLei.SheBeiID = SheBeiID;
            PeiZhiLei.IsPeiZhi = ispeizhi;
            PeiZhiLei.WenJianName = PeiZhiObjName;
            if (ispeizhi == false)
            {
                PeiZhiLei.IniData();
                PeiZhiLei.AnJianZhou += PeiZhiLei_AnJianZhou;
                List<LSModel> sModels = PeiZhiLei.LisSheBei;
                if (sModels.Count > 0)
                {
                  

                    Thread xiancheng = new Thread(ReadWork);
                    xiancheng.IsBackground = true;
                    xiancheng.DisableComObjectEagerCleanup();
                    xiancheng.Start();

                    Thread xie = new Thread(XieWork);
                    xie.IsBackground = true;
                    xie.DisableComObjectEagerCleanup();
                    xie.Start();
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
                short res = 0;

                ushort cardnum = 0;
                ushort[] cardids = new ushort[8];
                uint[] cardtypes = new uint[8];
                short num = LTDMC.dmc_board_init();
                if (num <= 0 || num > 8)
                {
                    ChuFaMsg(MsgDengJi.SheBeiTangChuang, $"初始卡失败 返回错误代号:{num}");
                    return;
                }
                res = LTDMC.dmc_get_CardInfList(ref cardnum, cardtypes, cardids);//获取控制卡信息
                if (res != 0)
                {
                    ChuFaMsg(MsgDengJi.SheBeiTangChuang, $"获取卡信息失败:{res}");
                    return;
                }
                bool EtherCardFlag = false;
                for (ushort i = 0; i < cardnum; i++)
                {
                    int _cardtype = (int)cardtypes[i];
                    List<LSModel> sModels = PeiZhiLei.LisSheBei;
                    for (int j = 0; j < sModels.Count; j++)
                    {
                        if (sModels[j].CardType == _cardtype)
                        {
                            sModels[j].CardNO = cardids[i];
                            EtherCardFlag = true;
                            sModels[j].TX = true;
                            PeiZhiLei.SetKaCardIO(cardids[i], sModels[j]);
                            break;
                        }
                    }

                }
                if (!EtherCardFlag)
                {
                    ChuFaMsg(MsgDengJi.SheBeiTangChuang, $"不存在EtherCAT总线卡");
                    return;
                }
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "控制卡链接成功");
            }
            catch(Exception ex)
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"控制卡链接失败:{ex}");

            }
     
          
            DengDaiOpen = true;
          

        }
        public override void Close()
        {
            ZongKaiGuan = false;
            try
            {
                LTDMC.dmc_board_close();
            }
            catch 
            {

               
            }

        }

        public override void XieShuJu(List<JiCunQiModel> canshus)
        {
            if (canshus == null || canshus.Count == 0)
            {
                return;
            }
            if (DengDaiOpen)
            {
                SengData.Add(canshus);
            }
        }

        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.SheBeiID = jicunqiid.SheBeiID;
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            JiCunQiModel zhen = PeiZhiLei.IsChengGong(jicunqiid);
            if (TongXin)
            {
                if (zhen != null)
                {

                    models.Value = zhen.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else
                {
                    models.Value = "没有找到";
                    models.IsZuiZhongJieGuo = JieGuoType.MeiZhaoDaoJiGuo;
                }
              
            }
            else
            {
                models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
            }
       
            return models;
        }



        public override List<JiCunQiModel> PeiZhiDuXie(int type)
        {
         
            if (PeiZhiLei != null)
            {
                return PeiZhiLei.PeiZhiDuXie();
            }
            return new List<JiCunQiModel>();
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
                return PeiZhiLei.QiTaJiCunQiDu;
            }
            return new List<JiCunQiModel>();
        }


        private void ReadWork()
        {
            int yanshi = 5;
            DateTime chongliantime = DateTime.Now;
            List<int> naxieyaodu = new List<int>();
            short redc = -1;
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                try
                {
                    List<LSModel> sModels = PeiZhiLei.LisSheBei;
                    for (int i = 0; i < sModels.Count; i++)
                    {
                        List<KaCanShuModel> liska = sModels[i].LisKaModels;
                        for (int c = 0; c < liska.Count; c++)
                        {
                            if (liska[c].IsZhou)
                            {
                                #region 轴的位置与轴超限报警
                                double pos = 0;
                                redc = LTDMC.dmc_get_encoder_unit(sModels[i].CardNO, liska[c].BitNoOrZhouHao, ref pos);
                                if (redc == 0)
                                {
                                    PeiZhiLei.FuZhiJiCunQi(DuCanShuType.位置, pos, liska[c]);
                                    bool tingzhi = PeiZhiLei.FuZhiJiCunQi(DuCanShuType.超限报警, pos, liska[c]);
                                    if (tingzhi)
                                    {
                                        XieDanZhouTingZhi(liska[i]);
                                    }
                                }
                             
                                #endregion
                                #region 获取轴速度
                                double speed = 0;
                                redc= LTDMC.dmc_read_current_speed_unit(sModels[i].CardNO, liska[c].BitNoOrZhouHao, ref speed);
                                if (redc==0)
                                {
                                    PeiZhiLei.FuZhiJiCunQi(DuCanShuType.速度, speed, liska[c]);
                                }

                                #endregion
                                #region 获取轴使能与轴状态
                                short shineng = LTDMC.dmc_read_sevon_pin(sModels[i].CardNO, liska[c].BitNoOrZhouHao);
                                PeiZhiLei.FuZhiJiCunQi(DuCanShuType.使能, shineng, liska[c]);
                                ushort Axis_State_machine = 0;
                                LTDMC.nmc_get_axis_state_machine(sModels[i].CardNO, liska[c].BitNoOrZhouHao, ref Axis_State_machine);
                                switch (Axis_State_machine)// 读取指定轴状态机
                                {
                                    case 0:
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.轴状态, "轴处于未启动状态", liska[c]);                                      
                                        break;
                                    case 1:
                                     
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.轴状态, "轴处于启动禁止状态", liska[c]);
                                        break;
                                    case 2:
                                   
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.轴状态, "轴处于准备启动状态", liska[c]);
                                        break;
                                    case 3:
                                      
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.轴状态, "轴处于启动状态", liska[c]);
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.使能, 0, liska[c]);
                                        break;
                                    case 4:
                                       
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.轴状态, "轴处于操作使能状态", liska[c]);
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.使能,1, liska[c]);
                                        break;
                                    case 5:
                                     
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.轴状态, "轴处于停止状态", liska[c]);
                                        break;
                                    case 6:
                                   
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.轴状态, "轴处于错误触发状态", liska[c]);
                                        break;
                                    case 7:
                                      
                                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.轴状态, "轴处于错误状态", liska[c]);
                                        break;
                                };

                                #endregion
                                #region 获取轴运行状态
                                short xs1 = LTDMC.dmc_check_done(sModels[i].CardNO, liska[c].BitNoOrZhouHao);
                                PeiZhiLei.FuZhiJiCunQi(DuCanShuType.运动状态, xs1, liska[c]);

                                #endregion
                              
                            }
                        }
                        #region 读IO
                        int xpeibi = (sModels[i].ZuiDaDuIO+1) / 32;
                        uint infanshui = LTDMC.dmc_read_inport(sModels[i].CardNO, 0);
                        List<bool> inios = OrIOB(infanshui);
                      
                        if (xpeibi > 0)
                        {
                            for (int g = 1; g <= xpeibi; g++)
                            {
                                uint infanhui1 = LTDMC.dmc_read_inport(sModels[i].CardNO, (ushort)g);
                                List<bool> inios1 = OrIOB(infanhui1);
                                inios.AddRange(inios1);
                            }
                        }
                        PeiZhiLei.FuZhiJiCunQiIO(DuCanShuType.DuIO, inios, liska);
                        #endregion
                        #region 写IO
                        int xso = (sModels[i].ZuiDaXieIO + 1) / 32;
                        uint infanshuixie = LTDMC.dmc_read_outport(sModels[i].CardNO, 0);
                        List<bool> iniosxie = OrIOB(infanshuixie);

                        if (xso > 0)
                        {
                            for (int g = 1; g <= xso; g++)
                            {
                                uint infanhui1 = LTDMC.dmc_read_outport(sModels[i].CardNO, (ushort)g);
                                List<bool> inios1 = OrIOB(infanhui1);
                                iniosxie.AddRange(inios1);
                            }
                        }
                        PeiZhiLei.FuZhiJiCunQiIO(DuCanShuType.XieIO, iniosxie, liska);
                        #endregion
                        #region 总线状态
                        ushort errcode = 0;
                        LTDMC.nmc_get_errcode(sModels[i].CardNO, 2, ref errcode);
                        PeiZhiLei.FuZhiJiCunQi(DuCanShuType.总线状态, errcode,null);
                        #endregion
                    }



                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"{SheBeiName}读数据发生错误:{ex}");
                   
                }
                             
                Thread.Sleep(yanshi);
            }
        }


        private void XieWork()
        {
            int yanshi = 5;          
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (PeiZhiLei.IsShuXinZhouPeiZhi)
                {
                    PeiZhiLei.IsShuXinZhouPeiZhi = false;
                    List<LSModel> sModels = PeiZhiLei.LisSheBei;
                    for (int i = 0; i < sModels.Count; i++)
                    {
                        foreach (var item in sModels[i].LisKaModels)
                        {
                            if (item.IsZhou)
                            {
                                SetYunDongCanShu(item.KaName);
                                SetHomeCanShu(item.KaName);
                            }
                        }
                       
                    }
                }

                try
                {
                    int count = SengData.GetCount();
                    if (count>0)
                    {
                        List<JiCunQiModel> lis = SengData.GetModel_Head_RomeHead();
                        if (lis.Count>0)
                        {
                            for (int i = 0; i < lis.Count; i++)
                            {
                                XieJiCunQi(lis[i]);
                            }
                           
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


        private void XieJiCunQi(JiCunQiModel jicunqi)
        {
            XieCaoZuoType xie = XieCaoZuoType.Wu;
            KaCanShuModel canshu = PeiZhiLei.GetHuoQu(jicunqi,out xie);
            switch (xie)
            {
                case XieCaoZuoType.ZhouX位置://轴去的相对位置
                    {
                        if (canshu != null)
                        {
                            short res = 0;
                            ushort movemode = 0;
                            double dist = ChangYong.TryDouble(jicunqi.Value, 0);
                            res = LTDMC.dmc_pmove_unit(canshu.CardNO, canshu.BitNoOrZhouHao, dist, movemode);
                            if (res != 0)
                            {
                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{canshu.KaName},相对运行出错 错误代号:{res}");
                            }
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu,"轴为空，不能相对运行");
                        }
                    }
                    break;
                case XieCaoZuoType.ZhouJ位置://轴去的绝对位置
                    {
                        if (canshu != null)
                        {
                            short res = 0;
                            ushort movemode = 1;
                            double dist = ChangYong.TryDouble(jicunqi.Value, 0);
                            res = LTDMC.dmc_pmove_unit(canshu.CardNO, canshu.BitNoOrZhouHao, dist, movemode);
                            if (res != 0)
                            {
                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{canshu.KaName},绝对运行出错 错误代号:{res}");
                            }
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, "轴为空，不能绝对运行");
                        }
                    }
                    break;
                case XieCaoZuoType.Zhou速度:
                    {
                        PeiZhiLei.IsShuXinZhouPeiZhi = true;
                    }
                    break;
                case XieCaoZuoType.Zhou恒速:
                    {
                        if (canshu != null)
                        {

                            int zhengxiang = ChangYong.TryInt(jicunqi.Value, 0);
                            if (zhengxiang <= 0)
                            {
                                zhengxiang = 0;
                            }
                            else
                            {
                                zhengxiang = 1;
                            }
                            short res = LTDMC.dmc_vmove(canshu.CardNO, canshu.BitNoOrZhouHao, (ushort)zhengxiang);
                            if (res != 0)
                            {
                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{canshu.KaName},恒速运行出错 错误代号:{res}");
                            }
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, "轴为空，不能恒速运行");
                        }
                    }
                    break;
                case XieCaoZuoType.Zhou使能:
                    {
                        if (canshu != null)
                        {
                            int zhengxiang = ChangYong.TryInt(jicunqi.Value, 0);
                            if (zhengxiang <= 0)
                            {
                                short res = LTDMC.nmc_set_axis_disable(canshu.CardNO, canshu.BitNoOrZhouHao);
                                if (res != 0)
                                {
                                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{canshu.KaName},失能出错 错误代号:{res}");
                                }
                            }
                            else
                            {
                                short res = LTDMC.nmc_set_axis_enable(canshu.CardNO, canshu.BitNoOrZhouHao);
                                if (res != 0)
                                {
                                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{canshu.KaName},使能出错 错误代号:{res}");
                                }
                            }
                           
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, "轴为空，不能使能");
                        }
                    }
                    break;
                case XieCaoZuoType.Zhou报警清除:
                    {
                        if (canshu != null)
                        {
                            short res = LTDMC.nmc_clear_axis_errcode(canshu.CardNO, canshu.BitNoOrZhouHao);//清除轴错误
                            if (res != 0)
                            {
                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{canshu.KaName},报警清除出错 错误代号:{res}");
                            }

                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, "轴为空，不能报警清除");
                        }
                       
                    }
                    break;
                case XieCaoZuoType.Zhou位置清零:
                    {
                        if (canshu != null)
                        {
                            short res = LTDMC.dmc_set_position_unit(canshu.CardNO, canshu.BitNoOrZhouHao,0);//清除轴错误
                            if (res != 0)
                            {
                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{canshu.KaName},位置清零出错 错误代号:{res}");
                            }

                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, "轴为空，不能位置清零");
                        }
                    }                  
                    break;
                case XieCaoZuoType.Zhou停止:
                    {
                        if (canshu != null)
                        {
                            XieDanZhouTingZhi(canshu);

                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, "轴为空，不能轴停止");
                        }
                    }
                    break;
                case XieCaoZuoType.硬件复位:
                    {
                        
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, "请勿操作，总线卡硬件复位进行中……");
                        DengDaiOpen = false;
                        Thread.Sleep(100);
                        double yanshityime = ChangYong.TryDouble(jicunqi.Value,15);
                        LTDMC.dmc_board_reset();
                        LTDMC.dmc_board_close();
                        DateTime times = DateTime.Now;
                        for (; ZongKaiGuan; )
                        {
                            if ((DateTime.Now- times).TotalMilliseconds> yanshityime*1000)
                            {
                                break;
                            }
                            Thread.Sleep(10);
                        }
                        if (ZongKaiGuan)
                        {
                            LTDMC.dmc_board_init();
                            DengDaiOpen = true;
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "总线卡硬件复位成功");
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "软件关闭中");
                        }

                    }
                    break;
                case XieCaoZuoType.热复位:
                    {                  
                        if (canshu!=null)
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "请勿操作，总线卡软件复位进行中……");
                            DengDaiOpen = false;
                            Thread.Sleep(100);
                            double yanshityime = ChangYong.TryDouble(jicunqi.Value, 15);
                            LTDMC.dmc_soft_reset(canshu.CardNO);

                            LTDMC.dmc_board_close();
                            DateTime times = DateTime.Now;
                            for (; ZongKaiGuan;)
                            {
                                if ((DateTime.Now - times).TotalMilliseconds > yanshityime * 1000)
                                {
                                    break;
                                }
                                Thread.Sleep(10);
                            }
                            if (ZongKaiGuan)
                            {
                                LTDMC.dmc_board_init();
                                DengDaiOpen = true;
                                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "总线卡软件复位完成，请确认总线状态");
                            }
                            else
                            {
                                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "软件关闭中");
                            }
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "没有卡号……");
                        }
                    }
                    break;
                case XieCaoZuoType.所有轴停止:
                    {
                        List<LSModel> sModels = PeiZhiLei.LisSheBei;
                        if (sModels.Count > 0)
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "紧急停止所有轴……");
                           
                            for (int i = 0; i < sModels.Count; i++)
                            {                          
                                LTDMC.dmc_emg_stop(sModels[i].CardNO);
                            }
                            
                           
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "没有卡号……");
                        }
                       
                    }
                    break;
                case XieCaoZuoType.XieIO:
                    {
                        if (canshu != null)
                        {
                            int zhi = ChangYong.TryInt(jicunqi.Value, 0) > 0 ? 1 : 0;
                            short rec = LTDMC.dmc_write_outbit(canshu.CardNO, (ushort)canshu.BitNoOrZhouHao, (ushort)zhi);
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{canshu.KaName} 写入io值:{zhi}");
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, "没有对应的配置");
                        }
                      
                    }
                    break;
                case XieCaoZuoType.Zhou回零:
                    {
                        if (canshu != null)
                        {
                            short res = 0;                                                
                            res = LTDMC.nmc_home_move(canshu.CardNO, canshu.BitNoOrZhouHao);
                            if (res != 0)
                            {
                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{canshu.KaName},轴回零出错 错误代号:{res}");
                            }
                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, "轴为空，不能回零");
                        }
                    }
                    break;
                case XieCaoZuoType.所有轴回零:
                    {
                        List<LSModel> sModels = PeiZhiLei.LisSheBei;
                        if (sModels.Count > 0)
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "紧急停止所有轴……");

                            for (int i = 0; i < sModels.Count; i++)
                            {
                                if (sModels[i].LisKaModels.Count>0)
                                {
                                    for (int c = 0; c < sModels[i].LisKaModels.Count; c++)
                                    {
                                        if (sModels[i].LisKaModels[c].IsZhou)
                                        {
                                            short res = LTDMC.nmc_home_move(sModels[i].LisKaModels[c].CardNO, sModels[i].LisKaModels[c].BitNoOrZhouHao);
                                            if (res != 0)
                                            {
                                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{sModels[i].LisKaModels[c].KaName},轴回零出错 错误代号:{res}");
                                            }
                                        }
                                    }
                                }
                               
                            }

                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "没有卡号，不能所有轴回零");
                        }

                    }
                    break;
                case XieCaoZuoType.Wu:
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 写单轴停止
        /// </summary>
        /// <param name="model"></param>
        private void XieDanZhouTingZhi(KaCanShuModel model)
        {

            short res = LTDMC.dmc_stop(model.CardNO, model.BitNoOrZhouHao, 1);
            if (res != 0)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"轴{model.KaName},位置停止出错 错误代号:{res}");
            }
        }

        /// <summary>
        /// 设置运动
        /// </summary>
        /// <param name="kaname"></param>
        private  void SetYunDongCanShu(string kaname)
        {
            List<KaCanShuModel> kas = PeiZhiLei.Keys;
            for (int i = 0; i < kas.Count; i++)
            {
                bool zhen = false;
                if (string.IsNullOrEmpty(kaname))
                {
                    zhen = true;
                }
                if (zhen == false)
                {
                    zhen = kas[i].KaName.Equals(kaname);
                }
                if (zhen==false)
                {
                    continue;
                }
                if (PeiZhiLei.ZhouPeiZhi.ContainsKey(kas[i]))
                {
                    ZhouPeiZhiModel zhiModel = PeiZhiLei.ZhouPeiZhi[kas[i]];
                    int res = LTDMC.dmc_set_s_profile(kas[i].CardNO, kas[i].BitNoOrZhouHao, 0, zhiModel.Spara);//设置S段时间（0-1s)
                    if (res == 0)
                    {
                        res = LTDMC.dmc_set_profile_unit(kas[i].CardNO, kas[i].BitNoOrZhouHao, zhiModel.Low_Vel, zhiModel.High_VelZ, zhiModel.Tacc, zhiModel.Tdec, zhiModel.Stop_Vel);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                        if (res == 0)
                        {
                            res = LTDMC.dmc_set_dec_stop_time(kas[i].CardNO, kas[i].BitNoOrZhouHao, zhiModel.StopTime);
                            if (res != 0)
                            {
                                ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"设置轴的停止时间 错误代码:{res}");
                            }

                        }
                        else
                        {
                            ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"设置轴的速度失败 错误代码:{res}");
                        }
                    }
                    else
                    {
                        ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"设置轴的S与T运行曲线失败 错误代码:{res}");
                    }
                }
            }

        }

        /// <summary>
        /// 设置回零参数
        /// </summary>
        /// <param name="kaname"></param>
        /// <returns></returns>
        private bool SetHomeCanShu(string kaname)
        {
            List<KaCanShuModel> kas = PeiZhiLei.Keys;
            for (int i = 0; i < kas.Count; i++)
            {
                bool zhen = false;
                if (string.IsNullOrEmpty(kaname))
                {
                    zhen = true;
                }
                if (zhen == false)
                {
                    zhen = kas[i].KaName.Equals(kaname);
                }
                if (zhen == false)
                {
                    continue;
                }
                if (PeiZhiLei.ZhouPeiZhi.ContainsKey(kas[i]))
                {
                    ZhouPeiZhiModel zhiModel = PeiZhiLei.ZhouPeiZhi[kas[i]];
                    int res = LTDMC.nmc_set_home_profile(kas[i].CardNO, kas[i].BitNoOrZhouHao, zhiModel.HomeMothod, zhiModel.HomeLow_Vel,zhiModel.HomeHigh_Vel,zhiModel.HomeTacc,zhiModel.HomeTdec,zhiModel.HomeOffsetpos);
                    if (res != 0)
                    {
                        ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"设置轴回零参数失败 错误代码:{res}");
                    }
                  
                }
            }

            return false;
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
        public override TxModel GetMeiGeTx()
        {
            TxModel model = new TxModel();
            model.SheBeiName = SheBeiName;
            model.SheBeiTD = SheBeiID;
            model.SheBeuZu = FenZu;
            bool ischengg = true;

            for (int i = 0; i < PeiZhiLei.LisSheBei.Count; i++)
            {
                LSModel item = PeiZhiLei.LisSheBei[i];
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
