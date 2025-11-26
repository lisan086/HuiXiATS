using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.GongNengLei;
using ATSJianMianJK.Log;
using ATSJianMianJK.XiTong.Model;
using BaseUI.DaYIngMoBan.Lei;
using CommLei.JiChuLei;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Lei.GongNengLei.ShiXian
{
    public class GongWeiLei : ABSGongNengLei
    {
        private bool ZongKaiGuan = true;
        private bool IsKeYiHuanXing = false;
        private SheBeiZhanModel SheBeiZhanModel;

        private bool IsLaoHuaTingZhi = true;
        /// <summary>
        /// 大于0  表示调试
        /// </summary>
        private int MaTDID = -1;

        private bool ShouDongLaoHua = false;

   
        
        public override void IniData(SheBeiZhanModel model)
        {
            ZongKaiGuan = true;
            SheBeiZhanModel = model;
         
            Thread thread = new Thread(Work);
            thread.IsBackground = true;
            thread.DisableComObjectEagerCleanup();
            thread.Start();

            Thread thread1 = new Thread(HuanXing);
            thread1.IsBackground = true;
            thread1.DisableComObjectEagerCleanup();
            thread1.Start();
        }



        private void Work()
        {
            if (SheBeiID < 0)
            {
                return;
            }
            DateTime jinzhantime = DateTime.Now;
         
            while (ZongKaiGuan)
            {
                try
                {

                    bool jinzhanzhi = DataJiHe.Cerate().GetGWPiPei(SheBeiID, CaoZuoType.ZongQingQiuTest);
                    if (jinzhanzhi||ShouDongLaoHua)
                    {
                        if ((DateTime.Now - jinzhantime).TotalMilliseconds >= 500||ShouDongLaoHua)
                        {
                            KongZhiBuZhou(KongZhiType.LaoHuaQian,"");
                            KongZhiBuZhou(KongZhiType.ALLFuWei, "1");
                            DengDaiTime(2000);
                            SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongQingQiuTest, SheBeiID, 1);
                            QiTaJinZhan();
                            WriteLog(RiJiEnum.TDLiuCheng, $"老化结束，PLC请求清零");
                            SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongQingQiuTest, SheBeiID, 1);
                            jinzhantime = DateTime.Now;
                        }
                    }
                    else
                    {
                        jinzhantime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(RiJiEnum.TDLiuChengRed, $"老化{ex}");
                }
                Thread.Sleep(50);
            }
        }

        private void HuanXing()
        {
            if (SheBeiID < 0)
            {
                return;
            }

            while (ZongKaiGuan)
            {
                if (IsKeYiHuanXing==false)
                {
                    Thread.Sleep(30);
                    continue;
                }
                try
                {

                    if (IsKeYiHuanXing)
                    {
                     
                        FaHuanXingZhiLing(MaTDID);
                        IsTiaoShi = false;
                        MaTDID = -1;
                        IsKeYiHuanXing = false;
                    }

                  
                }
                catch (Exception ex)
                {
                    WriteLog(RiJiEnum.TDLiuChengRed, $"唤醒{ex}");
                }
                Thread.Sleep(10);
            }
        }

        private void QiTaJinZhan()
        {
            int shenwenhege = 0;
            int laohuahege = 0;
            int gaowen = ChangYong.TryInt(SheBeiZhanModel.QiTa,5000);
            int iskekao = KongZhiBuZhou(KongZhiType.JianCeKeKao, "");
            if (iskekao == 1)
            {
                KongZhiBuZhou(KongZhiType.LaoHuaZhunBei, "");
                if (ZongKaiGuan)//升温过程
                {
                    KongZhiBuZhou(KongZhiType.KaiTestJDQ, "");
                    IsKeYiHuanXing = true;
                    DengDaiTime(gaowen);
                    if (MaTDID < 0)
                    {
                        double shenwentime = SheBeiZhanModel.ShengWenTime * 60;
                        DateTime dateTime = DateTime.Now;
                        for (; ZongKaiGuan && IsLaoHuaTingZhi;)
                        {
                            int wendu = PanDuanWenDu();
                            if (wendu == 1)
                            {
                                shenwenhege = 1;
                                break;
                            }
                            else if (wendu == 2)
                            {
                                shenwenhege = 2;
                                break;
                            }
                            else if (wendu == 3)
                            {
                                shenwenhege = 4;
                                break;
                            }
                            if ((DateTime.Now - dateTime).TotalSeconds >= shenwentime)
                            {
                                shenwenhege = 3;
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        if (shenwenhege == 1)
                        {
                            WriteLog(RiJiEnum.TDLiuCheng, "升温结束,正式老化");
                            SetMaTDSate(1, "升温结束");
                        }
                        else if (shenwenhege == 2)
                        {
                            //温度不合格

                            SetMaTDSate(4, "温度过高");
                            WriteLog(RiJiEnum.TDLiuCheng, "温度过高,老化结束，等温度低了 在老化");
                        }
                        else if (shenwenhege == 3)
                        {
                            SetMaTDSate(4, "温度过低超时");

                            WriteLog(RiJiEnum.TDLiuCheng, "温度过低超时，请检查炉子是不是出现温度传感器异常");
                        }
                        else if (shenwenhege == 4)
                        {

                            SetMaTDSate(4, "高温设备没有运行");
                            WriteLog(RiJiEnum.TDLiuCheng, "高温设备没有运行");
                        }
                    }
                    else
                    {
                        shenwenhege = 1;
                    }
                }
                if (ZongKaiGuan)
                {
                    if (shenwenhege == 1)
                    {
                        WriteLog(RiJiEnum.TDLiuCheng, "开始老化测试,检测相关参数");
                        SetMaTDSate(2, "开始老化");
                        SheBeiZhanModel.MiaoSu = "正式老化";
                        double zongtestime = SheBeiZhanModel.SetTestTime * 60;
                        if (MaTDID >= 0)
                        {
                            zongtestime = 30;
                        }

                        DateTime dateTime = DateTime.Now;
                        DateTime jilutime = DateTime.Now;

                        for (; ZongKaiGuan && IsLaoHuaTingZhi;)
                        {
                            bool iskeyijilu = false;
                            int wendu = PanDuanWenDu();
                            if (wendu == 2)
                            {
                                laohuahege = 2;
                                break;
                            }
                            if (wendu == 3)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, "高温设备没有运行");
                                SetMaTDSate(4, "高温设备没有运行");
                                laohuahege = 2;
                                break;
                            }
                            if (wendu == 4)
                            {
                                SetMaTDSate(4, "最低温度跳出");
                                WriteLog(RiJiEnum.TDLiuCheng, "最低温度跳出");
                                laohuahege = 2;
                                break;
                            }
                            if ((DateTime.Now - jilutime).TotalSeconds >= SheBeiZhanModel.MeiGeCaiJiTime)
                            {
                                iskeyijilu = true;
                                jilutime = DateTime.Now;
                            }
                            bool zhen = LaoHuaShuJuPanDuan(iskeyijilu);
                            if (zhen == false)
                            {
                                laohuahege = 3;
                                break;
                            }
                            if ((DateTime.Now - dateTime).TotalSeconds >= zongtestime)
                            {
                                laohuahege = 1;
                                break;
                            }
                            SheBeiZhanModel.TestTime = (DateTime.Now - dateTime).TotalMinutes;
                            Thread.Sleep(50);
                        }
                    }
                    if (IsLaoHuaTingZhi == false)
                    {
                        laohuahege = 4;
                    }
                }
                KongZhiBuZhou(KongZhiType.ALLFuWei, "");
                if (shenwenhege == 1)
                {
                    bool iszonghege = true;
                    for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
                    {
                        MaTDModel ma = SheBeiZhanModel.LisMaTD[i];
                        if (string.IsNullOrEmpty(ma.BanMa) == false)
                        {
                            if (ma.IsShuJuHeGe != 4)
                            {
                                ma.IsShuJuHeGe = laohuahege==1?3:4;
                                ma.DiBuMiaoSu = laohuahege == 1?"测试合格":"测试不合格";
                            }
                            foreach (var item in ma.JiLuBuHe)
                            {
                                ShangMes(2, ma.BanMa, ma.IsShuJuHeGe == 3, "", item);
                            }
                            bool zhen = ShangMes(3, ma.BanMa, ma.IsShuJuHeGe == 3, "");
                            ma.IsShangChuanHeGe = zhen;
                            if (zhen == false)
                            {
                                ma.IsShuJuHeGe = 4;
                            }
                            if (ma.IsShuJuHeGe==4)
                            {
                                iszonghege = false;
                            }
                        }

                    }
                   
                    SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongJieGuo, SheBeiID, iszonghege ? 2 : 3);
                    SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongWanCheng, SheBeiID, 2);
                    WriteLog(RiJiEnum.TDLiuCheng, $"检测完成");
                }
                else
                {
                    SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongJieGuo, SheBeiID, 3);
                    SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongWanCheng, SheBeiID, 3);
                    WriteLog(RiJiEnum.TDLiuCheng, $"检测失败");
                   
                }
            }
            else
            {
                KongZhiBuZhou(KongZhiType.ALLFuWei, "");
                SetMaTDSate(4, "通讯有问题");
            }
            KongZhiBuZhou(KongZhiType.LaoHuaJieSu, "");
          
        }
        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(15);
           
        }
      
        /// <summary>
        ///  0表示未测试 1表示正在升温 2表示正在测试 3表示测试合格 4表示不合格
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dibumiaosu"></param>
        private void SetMaTDSate(int state,string dibumiaosu)
        {
            for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
            {
                MaTDModel ma = SheBeiZhanModel.LisMaTD[i];
                if (string.IsNullOrEmpty(ma.BanMa) == false)
                {
                    ma.DiBuMiaoSu = dibumiaosu;
                    ma.IsShuJuHeGe = state;
                }
                else
                {
                    if (state==0)
                    {
                        ma.DiBuMiaoSu = dibumiaosu;
                        ma.IsShuJuHeGe = state;
                    }
                }
            }
        }
 
   

        private void FaSongKongZhiZhiLing(MaTDModel ma, CaoZuoType caoZuoType, bool ischixu)
        {
            if (ischixu)
            {
                List<YeWuDataModel> model = ma.LisKongZhi;
                foreach (var item in model)
                {
                    if (item.CaoZuoType == caoZuoType)
                    {
                        if (item.IsXunHuanHuanXian == 1 || item.CiShu == 0)
                        {
                            item.CiShu = 1;
                            SheBeiJiHe.Cerate().XieRuPeiZhiData(item, SheBeiID, 2);
                        }
                    }
                }
            }
            else
            {
                List<YeWuDataModel> model = ma.LisKongZhi;
                foreach (var item in model)
                {
                    if (item.CaoZuoType == caoZuoType)
                    {
                        SheBeiJiHe.Cerate().XieRuPeiZhiData(item, SheBeiID, 2);
                    }
                }
             
            }
        }

        private void FaHuanXingZhiLing(int matd)
        {
          
            for (; ZongKaiGuan&& IsKeYiHuanXing; )
            {
                for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
                {
                    MaTDModel ma = SheBeiZhanModel.LisMaTD[i];
                    bool iskeyigongzuo = false;
                    if (matd < 0)
                    {
                        if (string.IsNullOrEmpty(ma.BanMa) == false)
                        {
                            if (ma.IsShuJuHeGe != 4)
                            {
                                iskeyigongzuo = true;
                            }
                            else
                            {
                                FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieGuanCanJDQ, true);
                            }
                        }
                    }
                    else
                    {
                        if (matd == ma.MaTDID)
                        {
                            iskeyigongzuo = true;
                        }
                    }
                    if (iskeyigongzuo)
                    {


                        FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieKaiCanJDQ, true);
                      
                        DengDaiTime(1000);
                        DateTime shjian = DateTime.Now;
                        for (; ZongKaiGuan && IsKeYiHuanXing; )
                        {
                            FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieCanCMD, true);                      
                            if ((DateTime.Now- shjian).TotalMilliseconds>= SheBeiZhanModel.MeiCiCANJianGeTime)
                            {
                                break;
                            }
                            if (SheBeiZhanModel.JinGeCanSendTime > 0)
                            {
                                Thread.Sleep(SheBeiZhanModel.JinGeCanSendTime);
                            }
                        }
                        FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieGuanCanJDQ, true);
                      
                    }
                }
            }
          
        }
 

        private void DengDaiTime(int haomaio)
        {
            DateTime dateTime = DateTime.Now;
            for (; ZongKaiGuan;)
            {
             
                if ((DateTime.Now - dateTime).TotalMilliseconds >= haomaio)
                {
                    

                    break;
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 1表示满足 2表示温度过高 0表示还不满足 4是表示温度小于最低温度 3表示设备没有运行
        /// </summary>
        /// <param name="shouci"></param>
        /// <returns></returns>
        private int PanDuanWenDu()
        {
            if (SheBeiZhanModel.IsYunXing==0)
            {
                return 3;
            }
            int state = 0;
            float wendu = DataJiHe.Cerate().GetGWZhiFloat(SheBeiID, CaoZuoType.ZongDuWenDu, 0);
            if (wendu<=SheBeiZhanModel.ZuiDiWenDuLow)
            {
                return 4;
            }
            if (wendu >= SheBeiZhanModel.WenDuLow && wendu <= SheBeiZhanModel.WenDuUp)
            {
                state = 1;
            }
            if (wendu > SheBeiZhanModel.WenDuUp)
            {
                state = 2;
            }
            return state;
        }

      

        private bool LaoHuaShuJuPanDuan(bool isjilu)
        {
            bool iscunzai = false;
          
            for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
            {
                MaTDModel ma = SheBeiZhanModel.LisMaTD[i];
                if (string.IsNullOrEmpty(ma.BanMa) == false)
                {

                    List<YeWuDataModel> model = SheBeiZhanModel.LisMaTD[i].LisData;
                    bool iszonghege = true;
                    bool isdayushangxian = false;
                    foreach (var item in model)
                    {
                        item.WenDu = SheBeiZhanModel.ShiShiWenDu;
                        double shuju = ChangYong.TryDouble(item.Value.JiCunValue, 0);
                        WriteLog(RiJiEnum.TDXieJiLu, $"{ma.BanMa}:{item.ToString()}");
                        if (shuju < item.XiaXian || shuju > item.ShangXian)
                        {
                            iszonghege = false;

                            item.IsHeGe = false;
                        }
                        else
                        {
                            item.IsHeGe = true;
                        }
                        if (shuju>=item.BaoHuZhi)
                        {
                            WriteLog(RiJiEnum.TDXieJiLu, $"{ma.BanMa}:大于等于保护上限，{shuju} 保护{item.BaoHuZhi}");
                            isdayushangxian = true;
                            iszonghege = false;
                            item.IsHeGe = false;
                        }
                    }
                    if (isdayushangxian)
                    {
                        FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieGuanShangDianJDQ, false);
                        FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieGuanHuanXingJDQ, false);
                        SheBeiZhanModel.LisMaTD[i].JiShuCiShu = SheBeiZhanModel.LisMaTD[i].CiShu;
                    }
                    if (isjilu)
                    {
                        if (iszonghege == false)
                        {
                            SheBeiZhanModel.LisMaTD[i].JiShuCiShu++;
                        }
                        else
                        {
                            if (SheBeiZhanModel.LisMaTD[i].JiShuCiShu < SheBeiZhanModel.LisMaTD[i].CiShu)
                            {
                                SheBeiZhanModel.LisMaTD[i].JiShuCiShu = 0;
                            }
                        }
                        if (SheBeiZhanModel.LisMaTD[i].JiShuCiShu <= SheBeiZhanModel.LisMaTD[i].CiShu)
                        {
                            iscunzai = true;
                            SheBeiZhanModel.LisMaTD[i].IsShuJuHeGe = 2;
                            SheBeiZhanModel.LisMaTD[i].DiBuMiaoSu = "正在测试";
                            foreach (var item in model)
                            {
                                SheBeiZhanModel.LisMaTD[i].JiLuBuHe.Add(item.FuZhi());
                            }

                        }
                        else
                        {
                            if (SheBeiZhanModel.LisMaTD[i].IsShuJuHeGe != 4)
                            {
                                FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieGuanShangDianJDQ, false);
                                FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieGuanHuanXingJDQ, false);
                                SheBeiZhanModel.LisMaTD[i].IsShuJuHeGe = 4;
                                SheBeiZhanModel.LisMaTD[i].DiBuMiaoSu = "测试不合格";
                                foreach (var item in model)
                                {
                                    SheBeiZhanModel.LisMaTD[i].JiLuBuHe.Add(item.FuZhi());
                                }
                            }
                        }
                    }
                    else
                    {
                        iscunzai = true;
                    }
                }

            }
            if (iscunzai == false)
            {
                WriteLog(RiJiEnum.TDLiuChengRed, "老化炉里没有待测产品");
            }
            return iscunzai;

        }




        /// <summary>
        /// 1表示满足 2表示温度过高 0表示还不满足
        /// </summary>
        /// <param name="kongZhiType"></param>
        /// <param name="dongzuo"></param>
        /// <returns></returns>
        private int KongZhiBuZhou(KongZhiType kongZhiType,object dongzuo )
        {
            if (kongZhiType==KongZhiType.HaiYuan)
            {
                WriteLog(RiJiEnum.TDLiuCheng, "系统状态还原");
                MaTDID = -1;
                IsKeYiHuanXing = false;
                IsLaoHuaTingZhi = false;
                IsTiaoShi = false;
                SheBeiZhanModel.TestState = 0;
                SheBeiZhanModel.TestTime = 0;
                SheBeiZhanModel.IsKeYiShouDongTest = 0;
                SheBeiZhanModel.MiaoSu = "系统状态还原";
                for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
                {
                    SheBeiZhanModel.LisMaTD[i].Clear();
                    List<YeWuDataModel> model = SheBeiZhanModel.LisMaTD[i].LisKongZhi;
                    foreach (var item in model)
                    {
                        item.CiShu = 0;
                    }
                }
                return 1;
            }
            else if (kongZhiType == KongZhiType.SaoMaFuZhi)
            {
                WriteLog(RiJiEnum.TDLiuCheng, "开始确认扫码");
                if (dongzuo is List<string>)
                {
                    List<string> lis = dongzuo as List<string>;
                    for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
                    {
                        if (i < lis.Count)
                        {
                            SheBeiZhanModel.LisMaTD[i].BanMa = lis[i];
                            if (string.IsNullOrEmpty(SheBeiZhanModel.LisMaTD[i].BanMa) == false)
                            {
                                SheBeiZhanModel.IsKeYiShouDongTest = 1;
                                ShangMes(1, SheBeiZhanModel.LisMaTD[i].BanMa, true,"");
                            }
                        }
                    }
                }

                SheBeiZhanModel.MiaoSu = "扫码完成";
                return 1;
            }
            else if (kongZhiType == KongZhiType.ShouDongTiaoShi)
            {
               
                WriteLog(RiJiEnum.TDLiuCheng, "手动调试");
                int matd = ChangYong.TryInt(dongzuo, -1);
                SheBeiZhanModel.TestTime = 0;
                for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
                {
                    SheBeiZhanModel.LisMaTD[i].Clear();
                    if (SheBeiZhanModel.LisMaTD[i].MaTDID == matd)
                    {
                        MaTDID = matd;
                        SheBeiZhanModel.LisMaTD[i].BanMa = "调试码1111";
                        SheBeiZhanModel.MiaoSu = $"手动调试:{SheBeiZhanModel.LisMaTD[i].TDName},{MaTDID}";
                        if (string.IsNullOrEmpty(SheBeiZhanModel.LisMaTD[i].BanMa) == false)
                        {
                            ShangMes(1, SheBeiZhanModel.LisMaTD[i].BanMa, true, "");
                        }
                    }

                }
                if (MaTDID > 0)
                {
                    SheBeiZhanModel.MiaoSu = "手动调试";
                    IsTiaoShi = true;
                    ShouDongLaoHua = true;
                }
                return 1;
            }
            else if (kongZhiType == KongZhiType.LaoHuaQian)
            {
                SheBeiZhanModel.TestState = 2;
                if (ShouDongLaoHua)
                {
                    ShouDongLaoHua = false;
                    WriteLog(RiJiEnum.TDLiuCheng, $"手动老化测试");
                }
                else
                {
                    WriteLog(RiJiEnum.TDLiuCheng, $"收到PLC老化请求");
                }
                SheBeiZhanModel.MiaoSu = "准备老化";
                SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongXieWenDu, SheBeiID, 2);
                return 1;
            }
            else if (kongZhiType == KongZhiType.ALLFuWei)
            {
                if (string.IsNullOrEmpty(dongzuo.ToString()))
                {
                    WriteLog(RiJiEnum.TDLiuCheng, "老化结束准备复位");
                    SheBeiZhanModel.MiaoSu = "老化结束准备复位";
                }
                else
                {
                    WriteLog(RiJiEnum.TDLiuCheng, "测试前复位");
                    SheBeiZhanModel.MiaoSu = "测试前复位";
                }
                SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongFuWei, SheBeiID, 2);
              
                return 1;
            }
            else if (kongZhiType == KongZhiType.LaoHuaJieSu)
            {
                IsKeYiHuanXing = false;
                SheBeiZhanModel.TestState = 1;
                WriteLog(RiJiEnum.TDLiuCheng, "老化结束");
                SheBeiZhanModel.MiaoSu = "老化结束";
                SheBeiZhanModel.IsKeYiShouDongTest = 0;
                return 1;
            }
            else if (kongZhiType == KongZhiType.LaoHuaZhunBei)
            {
                IsLaoHuaTingZhi = true;
                SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongDianYuan, SheBeiID, 2);
                WriteLog(RiJiEnum.TDLiuCheng, "老化准备，启动电源");
                SheBeiZhanModel.MiaoSu = "老化开启大电源";
                return 1;
            }
            else if (kongZhiType == KongZhiType.JianCeKeKao)
            {
                WriteLog(RiJiEnum.TDLiuCheng, "检测通信是否可靠");
                SheBeiZhanModel.MiaoSu = "检测通信是否可靠";

                bool zhenkekao = true;
                bool iscunzai = false;
                if (MaTDID < 0)
                {
                    for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
                    {
                        MaTDModel ma = SheBeiZhanModel.LisMaTD[i];
                        if (string.IsNullOrEmpty(ma.BanMa) == false)
                        {

                            List<YeWuDataModel> model = SheBeiZhanModel.LisMaTD[i].LisData;

                            foreach (var item in model)
                            {
                                iscunzai = true;
                                if (item.Value.IsKeKao == false)
                                {
                                    zhenkekao = false;
                                    break;
                                }
                            }

                        }

                    }
                }
                else
                {
                    iscunzai = true;
                    zhenkekao = true;
                    WriteLog(RiJiEnum.TDLiuCheng, "老化调试");
                }

                if (iscunzai)
                {
                    if (zhenkekao)
                    {
                        WriteLog(RiJiEnum.TDLiuCheng, "检测通信可靠");
                        SheBeiZhanModel.MiaoSu = "检测通信可靠";
                        return 1;
                    }
                }
                WriteLog(RiJiEnum.TDLiuCheng, "检测通信不可靠");
                SheBeiZhanModel.MiaoSu = "检测通信不可靠";
                return 2;

            }
            else if (kongZhiType == KongZhiType.KaiTestJDQ)
            {
                SheBeiZhanModel.MiaoSu = "开始升温";
                WriteLog(RiJiEnum.TDLiuCheng, "开始升温");
                SetMaTDSate(1, "开始升温");
                WriteLog(RiJiEnum.TDLiuCheng, "打开测试继电器");
              

                for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
                {
                    MaTDModel ma = SheBeiZhanModel.LisMaTD[i];
                    if (string.IsNullOrEmpty(ma.BanMa) == false)
                    {
                        FaSongKongZhiZhiLing(ma,CaoZuoType.MaXieKaiShangDianJDQ,false);                   
                    }

                }
                for (int i = 0; i < SheBeiZhanModel.LisMaTD.Count; i++)
                {
                    MaTDModel ma = SheBeiZhanModel.LisMaTD[i];
                    bool iskeyigongzuo = false;
                    if (MaTDID < 0)
                    {
                        if (string.IsNullOrEmpty(ma.BanMa) == false)
                        {
                            iskeyigongzuo = true;
                        }
                    }
                    else
                    {
                        if (MaTDID == ma.MaTDID)
                        {
                            iskeyigongzuo = true;
                        }
                    }
                    if (iskeyigongzuo)
                    {
                        FaSongKongZhiZhiLing(ma, CaoZuoType.MaXieKaiHuanXingJDQ, true);
                    }
                }
                return 1;

            }
            return 0;
        }

        private enum KongZhiType
        {
            HaiYuan,
            SaoMaFuZhi,
            ShouDongTiaoShi,
            LaoHuaQian,
            ALLFuWei,
          
            JianCeKeKao,
            LaoHuaZhunBei,
            KaiTestJDQ,
            LaoHuaJieSu,
        }
        public override void CaoZuo(DoType doType, JieMianCaoZuoModel model)
        {
            if (doType == DoType.SaoMaQueRen)
            {
                KongZhiBuZhou(KongZhiType.HaiYuan,"");
                KongZhiBuZhou(KongZhiType.SaoMaFuZhi, model.CanShu);
            }
            else if (doType == DoType.ShouDongSaoMaWeiTiaoShi)
            {
                if (SheBeiZhanModel.TestState != 2)
                {
                    Task.Factory.StartNew(() =>
                    {
                        KongZhiBuZhou(KongZhiType.HaiYuan, "");
                        KongZhiBuZhou(KongZhiType.ShouDongTiaoShi, model.CanShu);

                    });
                }
             
            }
            else if (doType == DoType.TingZhiLaoHua)
            {
                for (int i = 0; i < 2; i++)
                {
                    SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongTingZhiPLCTest, SheBeiID, 2);
                    Thread.Sleep(100);
                }
             
                KongZhiBuZhou(KongZhiType.HaiYuan, "");
               
            }
            else if (doType == DoType.ShouDongLaoHua)
            {
                if (SheBeiZhanModel.TestState != 2)
                {
                    ShouDongLaoHua = true;
                    IsTiaoShi = false;
                    SheBeiZhanModel.MiaoSu = "老化测试";
                    SheBeiJiHe.Cerate().XieRuPeiZhiData(CaoZuoType.ZongQiDongPLCTest, SheBeiID, 2);
                }

            }
        }
    }
}
