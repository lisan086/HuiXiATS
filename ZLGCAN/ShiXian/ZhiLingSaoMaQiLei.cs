using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.Model;
using YiBanSaoMaQi.Frm;
using YiBanSaoMaQi.Model;
using ZLGXSCAN.Frm;
using ZLGXSCAN.ShiXian;


namespace YiBanSaoMaQi.ShiXian
{
    public class ZhiLingSaoMaQiLei : ABSNSheBei
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
                return "CAN通信";
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

      

        private Dictionary<int, FanXingJiHeLei<List<CunModel>>> SengData = new Dictionary<int, FanXingJiHeLei<List<CunModel>>>();

      

        public ZhiLingSaoMaQiLei()
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
                PeiZhiLei.XieIOEvent += ZhongShengIO_XieIOEvent;
                List<SaoMaModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (SengData.ContainsKey(sModels[i].SheBeiID) == false)
                    {
                       
                        SengData.Add(sModels[i].SheBeiID, new FanXingJiHeLei<List<CunModel>>());
                        Thread xiancheng = new Thread(ReadWork);
                        xiancheng.IsBackground = true;
                        xiancheng.DisableComObjectEagerCleanup();
                        xiancheng.Start(sModels[i]);
                        Thread xiancheng1 = new Thread(YiZhiXie);
                        xiancheng1.IsBackground = true;
                        xiancheng1.DisableComObjectEagerCleanup();
                        xiancheng1.Start(sModels[i]);

                    }
                }
            }
        }

        private void ZhongShengIO_XieIOEvent(JiCunQiModel obj)
        {
            XieShuJu(new List<JiCunQiModel>() { obj });
        }

        public override void Open()
        {
            foreach (var item in PeiZhiLei.DataMoXing.LisSheBei)
            {
                CANOpen(item);
            }
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang,"打开周立功设备");
        }
        public override void Close()
        {
            ZongKaiGuan = false;
            Thread.Sleep(100);
            List<SaoMaModel> lisd = PeiZhiLei.DataMoXing.LisSheBei;
            foreach (var item in lisd)
            {
                try
                {
                    Method.ZCAN_CloseDevice(item.Device_handle);
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, ex.Message);
                }

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
                JieRu(canshus);
            }
        }
        private CunModel GetDiZhi(JiCunQiModel jicunqi)
        {
            CunModel sModels = PeiZhiLei.DataMoXing.GetModel(jicunqi);
            if (sModels!=null)
            {              
                PeiZhiLei.DataMoXing.SetZhengZaiValue(sModels.JiCunQi.WeiYiBiaoShi, 0);               
                return sModels;
            }
            return null;
        }
        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
                CunModel ipcom = GetDiZhi(item);
                if (ipcom != null)
                {
                    if (SengData.ContainsKey(ipcom.ZongSheBeiId))
                    {
                        SengData[ipcom.ZongSheBeiId].Add(new List<CunModel>() { ipcom });
                    }
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel xinr = PeiZhiLei.DataMoXing.IsChengGong(models.WeiYiBiaoShi);
            if (xinr != null)
            {
                if (xinr.IsZhengZaiCe == 1)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else if (xinr.IsZhengZaiCe >= 2)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                }
                else
                {
                    models.Value = "";
                    models.IsZuiZhongJieGuo = JieGuoType.JingXingZhong;
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
            jieMianFrmModel.Form = new TiaoShiOrPeiZhiFrm(PeiZhiLei);
            return jieMianFrmModel;
        }

        public override List<JiCunQiModel> GetShuJu()
        {

            List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;

            return Get;
        }

        private void YiZhiXie(object zhi)
        {
            //xieid(0x01),zhiling(00 00),index(0))*xieid(0x01),zhiling(00 00),index(0))
            SaoMaModel zsmodel = zhi as SaoMaModel;

            int yanshi = 200;         
            DateTime chongliantime = DateTime.Now;

            List<string> fengezhiling = ChangYong.JieGeStr(zsmodel.YiZhiFaDeZhiLing,'*');
            List<List<string>> ZhiLings = new List<List<string>>();
            for (int i = 0; i < fengezhiling.Count; i++)
            {
                string[] jiexiezhi = fengezhiling[i].Split(',');
                if (jiexiezhi.Length >= 4)
                {
                    if (string.IsNullOrEmpty(jiexiezhi[0]) == false && string.IsNullOrEmpty(jiexiezhi[1])==false && string.IsNullOrEmpty(jiexiezhi[2])==false)
                    {
                        ZhiLings.Add(new List<string>() { jiexiezhi[0], jiexiezhi[1], jiexiezhi[2], jiexiezhi[3] });
                    }
                }
            }
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                if (zsmodel.IsYiZhiXie == -1)
                {
                    Thread.Sleep(50);
                    continue;
                }
                #region 写
                try
                {
                    if (ZhiLings.Count>0)
                    {
                        foreach (var item in ZhiLings)
                        {
                            if (item.Count>=4)
                            {
                                if (zsmodel.IsYiZhiXie == ChangYong.TryInt(item[2], 0))
                                {
                                    IsXieChengGong(item[0], item[1], item[2], item[3], zsmodel);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{zsmodel.SheBeiID}:写报错:{ex}");
                }
                #endregion

                Thread.Sleep(yanshi);
            }
        }
        private void ReadWork(object zhi)
        {
            SaoMaModel zsmodel = zhi as SaoMaModel;
           
            int yanshi = 5;
            FanXingJiHeLei<List<CunModel>> sengData = SengData[zsmodel.SheBeiID];

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
                    if ((DateTime.Now - chongliantime).TotalMilliseconds > 1500)
                    {
                        CANOpen(zsmodel);
                        chongliantime = DateTime.Now;
                    }
                }
                #region 写
                try
                {
                    int  shuju = sengData.GetCount();
                    if (shuju>0)
                    {
                        List<CunModel> duixiang = new List<CunModel>();
                        duixiang = sengData.GetModel_Head_RomeHead();
                        int count = duixiang.Count;
                        for (int i = 0; i < count; i++)
                        {
                           
                            if (duixiang[i]!=null)
                            {                           
                                WriteRec(duixiang[i], zsmodel);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"{zsmodel.SheBeiID}:写报错:{ex}");
                }
                #endregion
             
                Thread.Sleep(yanshi);
            }
        }

        public void WriteRec(CunModel model, SaoMaModel moggf)
        {
            if (model.IsDu == CunType.XieZlgCanDanGeXieRec)
            {
                if (moggf.TX)
                {
                    try
                    {
                        //(xieid(0x01), zhiling(00 00), index(0), huiid(0x01), IsFD(0表示不是FD 1表示FD发送))
                        string xieruzhi = ChangYong.TryStr(model.JiCunQi.Value, "");
                        List<string> datas = ChangYong.JieGeStr(xieruzhi, ',');
                        if (datas.Count >= 5)
                        {
                         
                            bool zhen = IsXieChengGong(datas[0], datas[1], datas[2], datas[4], moggf);
                            if (zhen)
                            {
                                List<uint> recds = new List<uint>();
                                if (!(string.IsNullOrEmpty(datas[3])|| datas[3]=="-1"))
                                {
                                    try
                                    {
                                        uint recid = (uint)Convert.ToInt32(datas[3], 16); //帧ID
                                        recds.Add(recid);
                                    }
                                    catch 
                                    {

                                      
                                    }
                                
                                }
                                object shijian = DuShuJu(moggf, model, datas[2], recds, 1,false);
                                if (shijian != null)
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, shijian);
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                                }
                                else
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "读数超时");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                                }
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "写数据有问题");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                            }
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "数据格式有问题");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        }

                    }
                    catch
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "传来的数据有问题");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }

                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "设备通信故障");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }

            }
            else if (model.IsDu == CunType.XieZlgBuXieZhiDu)
            {
                if (moggf.TX)
                {
                    //(index,huiid*huiid*huiid...,count)
                    List<string> lis = ChangYong.TryStr(model.JiCunQi.Value, "").Split(',').ToList();
                    if (lis.Count >= 3)
                    {
                        List<uint> recds = new List<uint>();
                        if (string.IsNullOrEmpty(lis[1])==false)
                        {
                            string[] shuju = lis[1].Split('*');
                            for (int i = 0; i < shuju.Length; i++)
                            {
                                try
                                {
                                    uint recid = (uint)Convert.ToInt32(shuju[1], 16); //帧ID
                                    recds.Add(recid);
                                }
                                catch
                                {


                                }
                            }
                           
                        }
                      
                        int count = ChangYong.TryInt(lis[2],1);
                        object shijian = DuShuJu(moggf, model, lis[0], recds, count, false);
                        if (shijian != null)
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, shijian);
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "读数超时");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        }
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "写数据有问题");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "设备通信故障");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }
            }
            else  if (model.IsDu == CunType.XieZlgCanDaiKai)
            {
                int todao = ChangYong.TryInt(model.JiCunQi.Value, 0);
                {
                    bool zhens = InitCAN(moggf, todao);
                    if (zhens == false)
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "NG");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{moggf.Name}:打开初始化CAN:{todao} 失败");
                        return;
                    }

                }
                {

                    bool zhen = CANStart(todao, moggf);
                    if (zhen)
                    {
                        PeiZhiLei.DataMoXing.SetHeGe(moggf.SheBeiID, true);
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{moggf.Name}:打开启动CAN:{todao} 成功");
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "NG");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{moggf.Name}:打开启动CAN:{todao} 失败");
                    }
                    return;
                }

            }
            else if (model.IsDu == CunType.XieZlgCanGuan)
            {
                int todao = ChangYong.TryInt(model.JiCunQi.Value, 0);
                GuaanBiCAN(moggf, todao);
                PeiZhiLei.DataMoXing.SetHeGe(moggf.SheBeiID, false);
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{moggf.Name}:关闭CAN:{todao} 成功");
            }
            else if (model.IsDu == CunType.XieZlgYiZhiXie)
            {
                moggf.IsYiZhiXie = ChangYong.TryInt(model.JiCunQi.Value, -1);
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                return;

            }
            else if (model.IsDu == CunType.XieZlgCanDuoGeWuRec)
            {
                //(xieid, zhiling, index, isFD * xieid, zhiling, index, isFD..#jici#yanshi)
                if (moggf.TX)
                {
                    try
                    {
                        //(xieid, zhiling, index, isFD * xieid, zhiling, index, isFD..#jici#yanshi)
                        string xieruzhi = ChangYong.TryStr(model.JiCunQi.Value, "");
                        string[] xinhao = xieruzhi.Split('#');
                        if (xinhao.Length >= 3)
                        {
                            string[] zhilings = xinhao[0].Split('*');
                            if (zhilings.Length >= 1)
                            {
                                List<List<string>> ZhiLings = new List<List<string>>();
                                for (int i = 0; i < zhilings.Length; i++)
                                {
                                    string[] jiexiezhi = zhilings[i].Split(',');
                                    if (jiexiezhi.Length >= 4)
                                    {
                                        if (string.IsNullOrEmpty(jiexiezhi[0]) == false && string.IsNullOrEmpty(jiexiezhi[1]) == false && string.IsNullOrEmpty(jiexiezhi[2]) == false)
                                        {
                                            ZhiLings.Add(new List<string>() { jiexiezhi[0], jiexiezhi[1], jiexiezhi[2], jiexiezhi[3] });
                                        }
                                    }
                                }

                                if (ZhiLings.Count > 0)
                                {
                                    int yanshi = ChangYong.TryInt(xinhao[2],1);
                                    int cishu= ChangYong.TryInt(xinhao[1], 1);
                                    Task.Factory.StartNew(() => {
                                        for (int i = 0; i < cishu; i++)
                                        {
                                            for (int c = 0; c < ZhiLings.Count; c++)
                                            {
                                                List<string> item = ZhiLings[c];
                                                IsXieChengGong(item[0], item[1], item[2], item[3], moggf);
                                            }
                                            Thread.Sleep(yanshi);
                                        }
                                        
                                    });
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                                }
                                else
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "指令无法解析");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                                }
                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "指令不存在");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                            }
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "数据格式有问题");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        }
                      
                    }
                    catch
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "传来的数据有问题");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }

                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "设备通信故障");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }

            }
            else if (model.IsDu == CunType.XieZlgCanDuByte)
            {
                //(xieid,zhiling,index,isFD)* #fajichi#yanshi#index#huiid
                if (moggf.TX)
                {
                    try
                    {                       
                        string xieruzhi = ChangYong.TryStr(model.JiCunQi.Value, "");
                        string[] xinhao = xieruzhi.Split('#');
                        if (xinhao.Length >= 5)
                        {
                            string[] zhilings = xinhao[0].Split('*');
                            if (zhilings.Length >= 1)
                            {
                                List<List<string>> ZhiLings = new List<List<string>>();
                                for (int i = 0; i < zhilings.Length; i++)
                                {
                                    string[] jiexiezhi = zhilings[i].Split(',');
                                    if (jiexiezhi.Length >= 4)
                                    {
                                        if (string.IsNullOrEmpty(jiexiezhi[0]) == false && string.IsNullOrEmpty(jiexiezhi[1]) == false && string.IsNullOrEmpty(jiexiezhi[2]) == false)
                                        {
                                            ZhiLings.Add(new List<string>() { jiexiezhi[0], jiexiezhi[1], jiexiezhi[2], jiexiezhi[3] });
                                        }
                                    }
                                }
                                if (ZhiLings.Count > 0)
                                {
                                   
                                    int cishu = ChangYong.TryInt(xinhao[1], 1);
                                    Task.Factory.StartNew(() => {
                                        for (int i = 0; i < cishu; i++)
                                        {
                                            for (int c = 0; c < ZhiLings.Count; c++)
                                            {
                                                List<string> item = ZhiLings[c];
                                                IsXieChengGong(item[0], item[1], item[2], item[3], moggf);
                                            }
                                            Thread.Sleep(1);
                                        }

                                    });
                                  

                                }                             
                            }
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                            List<uint> recds = new List<uint>();
                            if (!(string.IsNullOrEmpty(xinhao[4]) || xinhao[4] == "-1"))
                            {
                                try
                                {
                                    uint recid = (uint)Convert.ToInt32(xinhao[4], 16); //帧ID
                                    recds.Add(recid);
                                }
                                catch
                                {


                                }

                            }
                            object shijian = DuShuJu(moggf, model, xinhao[3], recds, ChangYong.TryInt(xinhao[2],2000), true);

                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "数据格式有问题");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        }
                      
                    }
                    catch
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "传来的数据有问题");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                    }

                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "设备通信故障");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }

            }   
            else
            {
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "没有找到");
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
            }

        }

   
        private void GuaanBiCAN(SaoMaModel maModel,int tongdaoindex)
        {
            try
            {
                if (maModel.Canshuju.ContainsKey(tongdaoindex))
                {
                    can_release(true, maModel, tongdaoindex, maModel.Canshuju[tongdaoindex]);
                    maModel.Canshuju.Remove(tongdaoindex);
                }

            }
            catch 
            {

             
            }
        }

        private bool InitCAN(SaoMaModel model,int tongdaoindex)
        {
         
            IntPtr ptr = model.Ptr;
           
            Define type = model.DeviceType;
            bool netDevice = type == Define.ZCAN_CANETTCP || type == Define.ZCAN_CANETUDP || type == Define.ZCAN_CANWIFI_TCP ||
                type == Define.ZCAN_CANFDNET_400U_TCP || type == Define.ZCAN_CANFDNET_400U_UDP ||
                type == Define.ZCAN_CANFDNET_200U_TCP || type == Define.ZCAN_CANFDNET_200U_UDP || type == Define.ZCAN_CANFDNET_800U_TCP ||
                type == Define.ZCAN_CANFDNET_800U_UDP;
            bool canfdnetDevice = type == Define.ZCAN_CANFDNET_400U_TCP || type == Define.ZCAN_CANFDNET_400U_UDP ||
                type == Define.ZCAN_CANFDNET_200U_TCP || type == Define.ZCAN_CANFDNET_200U_UDP || type == Define.ZCAN_CANFDNET_800U_TCP ||
                type == Define.ZCAN_CANFDNET_800U_UDP;
            bool pcieCanfd = type == Define.ZCAN_PCIECANFD_100U ||
                type == Define.ZCAN_PCIECANFD_200U ||
                type == Define.ZCAN_PCIECANFD_400U ||
                type == Define.ZCAN_PCIECANFD_200U_EX;
            bool usbCanfd = type == Define.ZCAN_USBCANFD_100U ||
                type == Define.ZCAN_USBCANFD_200U ||
                type == Define.ZCAN_USBCANFD_400U ||
                type == Define.ZCAN_USBCANFD_MINI ||
                type == Define.ZCAN_USBCANFD_800U;
            bool canfdDevice = usbCanfd || pcieCanfd;

            if (0 == (UInt64)ptr)
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang,$"{model.Name}:设置指定路径属性失败");
                return false;
            }
            //获取指定的托管对象
            IProperty property_ = (IProperty)Marshal.PtrToStructure((IntPtr)((UInt64)ptr), typeof(IProperty));
            if (!setCANFDStandard(0, model,tongdaoindex)) //设置CANFD标准
            {
              
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:设置CANFD标准失败");
                return false;
            }
            bool result = true;
            if (usbCanfd)
            {
                if (type == Define.ZCAN_USBCANFD_200U || type == Define.ZCAN_USBCANFD_400U || type == Define.ZCAN_USBCANFD_800U)
                {

                    if (!setDataMerge(model,tongdaoindex))
                    {
                      
                        ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:合并设置失败");
                        return false;
                    }
                }
                result = setFdBaudrate(model.HaoBoTeLu, model.BaiBoTeLu,model,tongdaoindex);
                if (!result)
                {
               
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:设置波特率失败");
                    return false;
                }
            }

            ZCAN_CHANNEL_INIT_CONFIG config_ = new ZCAN_CHANNEL_INIT_CONFIG();
            if (true && !netDevice)
            {
                config_.canfd.mode = (byte)0;  //工作模式，0正常，1只听
                if (usbCanfd)
                {
                    config_.can_type = (uint)Define.TYPE_CANFD;
                }
                else if (pcieCanfd)
                {
                    config_.can_type = (uint)Define.TYPE_CANFD;
                    config_.can.filter = 0;
                    config_.can.acc_code = 0;
                    config_.can.acc_mask = 0xFFFFFFFF;
                    config_.can.mode = 0;
                }
                else
                {
                    config_.can_type = (uint)Define.TYPE_CAN;
                    config_.can.filter = 0;
                    config_.can.acc_code = 0;
                    config_.can.acc_mask = 0xFFFFFFFF;
                    config_.can.mode = 0;
                }
            }

            IntPtr pConfig = Marshal.AllocHGlobal(Marshal.SizeOf(config_));
            Marshal.StructureToPtr(config_, pConfig, true);

           IntPtr  channel_handle_ = Method.ZCAN_InitCAN(model.Device_handle, (uint)tongdaoindex, pConfig);
            Marshal.FreeHGlobal(pConfig);


            if (0 == (int)channel_handle_)
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:初始化CAN失败");
            
                return false;
            }

            if (usbCanfd)
            {
                if (!setResistanceEnable(true,model,tongdaoindex))
                {
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:使能终端电阻失败");
                   
                    can_release(true,model,tongdaoindex,channel_handle_);
                    return false;
                }
            }
            if (model.Canshuju.ContainsKey(tongdaoindex) == false)
            {
                model.Canshuju.Add(tongdaoindex, channel_handle_);
            }
            else
            {
                model.Canshuju[tongdaoindex]=channel_handle_;
            }
           return true;
        }

        /// <summary>
        /// CAN设备启动
        /// </summary>
        /// <returns></returns>
        private bool CANStart(int tongdaoindex,SaoMaModel model)
        {
            if (model.Canshuju.ContainsKey(tongdaoindex)==false)
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:没有找到该通道对应的参数");
                return false;
            }
            if (Method.ZCAN_StartCAN(model.Canshuju[tongdaoindex]) != 1)
            {
                
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{model.Name}:启动CAN失败");
                return false;
            }
  
            return true;
        }
        /// <summary>
        /// 释放CAN
        /// </summary>
        private bool can_release(bool isgunbi,SaoMaModel mode,int todaoindex,IntPtr intPtr)
        {
            if (isgunbi)
            {
                if (todaoindex == mode.IsYiZhiXie)
                {
                    mode.IsYiZhiXie = -1;
                }
            }
            if (Method.ZCAN_ResetCAN(intPtr) != 1)
            {
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{mode.Name}:复位失败");

                return false;
            }
            
            return true;

        }
        //设置终端电阻使能
        private bool setResistanceEnable(bool checkeds,SaoMaModel model,int tongdaoindex)
        {
            string path = tongdaoindex + "/initenal_resistance";
            string value = (checkeds ? "1" : "0");
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            return 1 == Method.ZCAN_SetValue(model.Device_handle, path, Encoding.ASCII.GetBytes(value));
        }
        private bool setFdBaudrate(UInt32 abaud, UInt32 dbaud,SaoMaModel model,int tongdaoindex)
        {
            string path = tongdaoindex + "/canfd_abit_baud_rate";
            string value = abaud.ToString();
            if (1 != Method.ZCAN_SetValue(model.Device_handle, path, Encoding.ASCII.GetBytes(value)))
            {
                return false;
            }
            path = tongdaoindex+ "/canfd_dbit_baud_rate";
            value = dbaud.ToString();
            if (1 != Method.ZCAN_SetValue(model.Device_handle, path, Encoding.ASCII.GetBytes(value)))
            {
                return false;
            }
            return true;
        }

        //设置开启合并接收
        private bool setDataMerge(SaoMaModel model, int tongdaoindex)
        {
            byte merge_ = 0;
            string path = tongdaoindex + "/set_device_recv_merge";
            string value = merge_.ToString();
            return 1 == Method.ZCAN_SetValue(model.Device_handle, path, Encoding.ASCII.GetBytes(value));
        }
        //设置CANFD标准
        private bool setCANFDStandard(int canfd_standard,SaoMaModel model, int tongdaoindex)
        {
            string path = tongdaoindex + "/canfd_standard";
            string value = canfd_standard.ToString();
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            uint ret = Method.ZCAN_SetValue(model.Device_handle, path, Encoding.ASCII.GetBytes(value));
            return (ret == 1);
        }
        private bool CANOpen(SaoMaModel shebeimodel)
        {
            try
            {
                //DeviceInfo dev = new DeviceInfo(Define.ZCAN_USBCANFD_100U, 1);
                shebeimodel.Device_handle = Method.ZCAN_OpenDevice((uint)shebeimodel.DeviceType, shebeimodel.DeviceIndex, 0);
               // IntPtr dec = Method.ZCAN_OpenDevice((uint)41,0, 0);
                if (0 == (int)shebeimodel.Device_handle)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"{shebeimodel.Name}:打开设备失败,请检查设备类型和设备索引号是否正确");
                    PeiZhiLei.DataMoXing.SetHeGe(shebeimodel.SheBeiID,false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"{shebeimodel.Name}:{ex}");
                PeiZhiLei.DataMoXing.SetHeGe(shebeimodel.SheBeiID, false);
                return false;
            }
           
            shebeimodel.Ptr = Method.GetIProperty(shebeimodel.Device_handle);
            PeiZhiLei.DataMoXing.SetHeGe(shebeimodel.SheBeiID, true);
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{shebeimodel.Name}:打开设备成功");
            return true;
        }
        private object DuShuJu(SaoMaModel moggf, CunModel model,string index, List<uint> recids,int count,bool isbyte)
        {
            try
            {
                int yanshi = moggf.ChaoShiTime;
                if (isbyte)
                {
                    yanshi = count;
                }

                int intindex = ChangYong.TryInt(index, 0);
                {
                    ZCAN_Receive_Data[] can_data = new ZCAN_Receive_Data[10000];
                    ZCAN_ReceiveFD_Data[] canfd_data = new ZCAN_ReceiveFD_Data[10000];
                    ZCANDataObj[] data_obj = new ZCANDataObj[10000];
                    ZCAN_LIN_MSG[] lin_data = new ZCAN_LIN_MSG[10000];
                    uint len = 0;
                    DateTime shijian = DateTime.Now;
                    List<string> StrShuJu = new List<string>();
                    List<byte[]> ByteShuJu = new List<byte[]>();
                    for (; ZongKaiGuan;)
                    {
                        if (moggf.IsFenKaiShou != 1)
                        { //分开接收

                            len = Method.ZCAN_GetReceiveNum(moggf.Canshuju[intindex], 0);

                            if (len > 0)
                            {
                                int size = Marshal.SizeOf(typeof(ZCAN_Receive_Data));
                                IntPtr ptr = Marshal.AllocHGlobal((int)len * size);
                                len = Method.ZCAN_Receive(moggf.Canshuju[intindex], ptr, len, 50);
                                try
                                {
                                    for (int i = 0; i < len; ++i)
                                    {
                                        can_data[i] = (ZCAN_Receive_Data)Marshal.PtrToStructure(
                                           (IntPtr)((Int64)ptr + i * size), typeof(ZCAN_Receive_Data));
                                        if (isbyte == false)
                                        {
                                            if (count == 1)
                                            {
                                                if (recids.Count == 0)
                                                {
                                                    string jieguo = $"{can_data[i].frame.can_id.ToString("X2")},{ChangYong.ByteOrString(can_data[i].frame.data, " ")}";
                                                    Marshal.FreeHGlobal(ptr);
                                                    return jieguo;
                                                }
                                                else
                                                {
                                                    if (recids.IndexOf( can_data[i].frame.can_id)>=0)
                                                    {
                                                        string jieguo = $"{can_data[i].frame.can_id.ToString("X2")},{ChangYong.ByteOrString(can_data[i].frame.data, " ")}";
                                                        Marshal.FreeHGlobal(ptr);
                                                        return jieguo;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (recids.Count == 0)
                                                {
                                                    string jieguo = $"{can_data[i].frame.can_id.ToString("X2")},{ChangYong.ByteOrString(can_data[i].frame.data, " ")}";
                                                    StrShuJu.Add(jieguo);
                                                }
                                                else
                                                {
                                                    if (recids.IndexOf(can_data[i].frame.can_id) >= 0)
                                                    {
                                                        string jieguo = $"{can_data[i].frame.can_id.ToString("X2")},{ChangYong.ByteOrString(can_data[i].frame.data, " ")}";
                                                        StrShuJu.Add(jieguo);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (recids.Count <= 0)
                                            {
                                                ByteShuJu.Add(can_data[i].frame.data);
                                            }
                                            else
                                            {
                                                if (recids.IndexOf(can_data[i].frame.can_id)>=0)
                                                {
                                                    ByteShuJu.Add(can_data[i].frame.data);
                                                }
                                            }

                                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, ByteShuJu);
                                          
                                        }
                                    }
                                }
                                catch
                                {


                                }

                                Marshal.FreeHGlobal(ptr);
                            }

                            len = Method.ZCAN_GetReceiveNum(moggf.Canshuju[intindex], 1);
                            if (len > 0)
                            {
                                int size = Marshal.SizeOf(typeof(ZCAN_ReceiveFD_Data));
                                IntPtr ptr = Marshal.AllocHGlobal((int)len * size);
                                len = Method.ZCAN_ReceiveFD(moggf.Canshuju[intindex], ptr, len, 50);
                                try
                                {
                                    for (int i = 0; i < len; ++i)
                                    {
                                        canfd_data[i] = (ZCAN_ReceiveFD_Data)Marshal.PtrToStructure(
                                            (IntPtr)((Int64)ptr + i * size), typeof(ZCAN_ReceiveFD_Data));
                                        if (isbyte == false)
                                        {
                                            if (count == 1)
                                            {
                                                if (recids.Count <= 0)
                                                {
                                                    string jieguo = $"{canfd_data[i].frame.can_id.ToString("X2")},{ChangYong.ByteOrString(canfd_data[i].frame.data, " ")}";
                                                    Marshal.FreeHGlobal(ptr);
                                                    return jieguo;
                                                }
                                                else
                                                {
                                                    if (recids.IndexOf(canfd_data[i].frame.can_id)>=0)
                                                    {
                                                        string jieguo = $"{canfd_data[i].frame.can_id.ToString("X2")},{ChangYong.ByteOrString(canfd_data[i].frame.data, " ")}";
                                                        Marshal.FreeHGlobal(ptr);
                                                        return jieguo;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (recids.Count <= 0)
                                                {
                                                    string jieguo = $"{canfd_data[i].frame.can_id.ToString("X2")},{ChangYong.ByteOrString(canfd_data[i].frame.data, " ")}";
                                                    StrShuJu.Add(jieguo);
                                                }
                                                else
                                                {
                                                    if (recids.IndexOf(canfd_data[i].frame.can_id) >= 0)
                                                    {
                                                        string jieguo = $"{canfd_data[i].frame.can_id.ToString("X2")},{ChangYong.ByteOrString(canfd_data[i].frame.data, " ")}";
                                                        StrShuJu.Add(jieguo);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (recids.Count <= 0)
                                            {
                                                ByteShuJu.Add(canfd_data[i].frame.data);
                                            }
                                            else
                                            {
                                                if (recids.IndexOf(canfd_data[i].frame.can_id) >= 0)
                                                {
                                                    ByteShuJu.Add(canfd_data[i].frame.data);
                                                }
                                            }

                                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, ByteShuJu);
                                        }
                                    }
                                }
                                catch
                                {


                                }


                                Marshal.FreeHGlobal(ptr);
                            }


                        }
                        else
                        { //合并接收
                            len = Method.ZCAN_GetReceiveNum(moggf.Canshuju[intindex], 2); //合并接收类型type为2
                            if (len > 0)
                            {
                                int size = Marshal.SizeOf(typeof(ZCANDataObj));
                                IntPtr ptr = Marshal.AllocHGlobal((int)len * size);
                                len = Method.ZCAN_ReceiveData(moggf.Canshuju[intindex], ptr, len, 50);         //传设备的句柄
                                try
                                {
                                    for (int i = 0; i < len; ++i)
                                    {
                                        data_obj[i] = (ZCANDataObj)Marshal.PtrToStructure(
                                            (IntPtr)((Int64)ptr + i * size), typeof(ZCANDataObj));
                                        if (isbyte == false)
                                        {
                                            if (count == 1)
                                            {
                                                if (recids.Count <= 0)
                                                {
                                                    string jieguo = $"{data_obj[i].zcanCANFDData.frame.can_id.ToString("X2")},{ChangYong.ByteOrString(data_obj[i].zcanCANFDData.frame.data, " ")}";
                                                    Marshal.FreeHGlobal(ptr);
                                                    return jieguo;
                                                }
                                                else
                                                {
                                                    if (recids.IndexOf(data_obj[i].zcanCANFDData.frame.can_id)>=0)
                                                    {
                                                        string jieguo = $"{data_obj[i].zcanCANFDData.frame.can_id.ToString("X2")},{ChangYong.ByteOrString(data_obj[i].zcanCANFDData.frame.data, " ")}";
                                                        Marshal.FreeHGlobal(ptr);
                                                        return jieguo;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (recids.Count <= 0)
                                                {
                                                    string jieguo = $"{data_obj[i].zcanCANFDData.frame.can_id.ToString("X2")},{ChangYong.ByteOrString(data_obj[i].zcanCANFDData.frame.data, " ")}";
                                                    StrShuJu.Add(jieguo);
                                                }
                                                else
                                                {
                                                    if (recids.IndexOf(data_obj[i].zcanCANFDData.frame.can_id)>=0)
                                                    {
                                                        string jieguo = $"{data_obj[i].zcanCANFDData.frame.can_id.ToString("X2")},{ChangYong.ByteOrString(data_obj[i].zcanCANFDData.frame.data, " ")}";
                                                        StrShuJu.Add(jieguo);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (recids.Count <= 0)
                                            {
                                                ByteShuJu.Add(data_obj[i].zcanCANFDData.frame.data);
                                            }
                                            else
                                            {
                                                if (recids.IndexOf(data_obj[i].zcanCANFDData.frame.can_id)>=0)
                                                {
                                                    ByteShuJu.Add(data_obj[i].zcanCANFDData.frame.data);
                                                }
                                            }
                                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, ByteShuJu);

                                        }
                                    }
                                }
                                catch
                                {


                                }
                                Marshal.FreeHGlobal(ptr);
                            }

                        }

                        if (isbyte==false)
                        {
                            if (StrShuJu.Count >= count)
                            {
                                return ChangYong.FenGeDaBao(StrShuJu, "|");
                            }
                        }
                      
                        bool zhens = ChaoShiTime(shijian, model, moggf, yanshi);
                        if (zhens)
                        {
                            return null;
                        }

                    }
                }

            }
            catch(Exception ex)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"读数据出错:{ex}");
            }
            return null;
        }
        private bool ChaoShiTime(DateTime kaishi, CunModel model, SaoMaModel moggf,int changshitime)
        {
            if ((DateTime.Now - kaishi).TotalMilliseconds >= changshitime)
            {
            
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{moggf.Name}:{model.IsDu} 数据超时");
                return true;
            }
            return false;
        }

      

        private bool IsXieChengGong(string xieids,string zhiling,string strindex,string isfd,SaoMaModel shebeimodel)
        {
            try
            {
                int index = ChangYong.TryInt(strindex,0);
                uint xieid = (uint)Convert.ToInt32(xieids, 16); //帧ID
                int frame_type_index = 0;  //帧类型：0：标准帧 1:扩展帧
                int protocol_index =ChangYong.TryInt(isfd,0)==1?1:0;    //协议 0：CAN,1:CANDF
                int send_type_index = 0;  //发送类型
                int canfd_exp_index = 1; //是否加速 1：加速
                uint result = 0; //发送的帧数
                if (0 == protocol_index) //can
                {
                    byte[] data = ChangYong.HexStringToByte(zhiling);
                    ZCAN_Transmit_Data can_data = new ZCAN_Transmit_Data();
                    //  
                    can_data.frame.can_id = MakeCanId(xieid, frame_type_index, 0, 0);

                    can_data.frame.data = new byte[8];
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (i<8)
                        {
                            can_data.frame.data[i]=data[i];
                        }
                    }
                    can_data.frame.can_dlc = (byte)can_data.frame.data.Length;
                    can_data.transmit_type = (uint)send_type_index;
                    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(can_data));
                    Marshal.StructureToPtr(can_data, ptr, true);
                    bool iscunzai = false;
                    if (shebeimodel.Canshuju.ContainsKey(index))
                    {
                        result = Method.ZCAN_Transmit(shebeimodel.Canshuju[index], ptr, 1);
                        iscunzai = true;
                    }
                    Marshal.FreeHGlobal(ptr);
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{shebeimodel.Name} 写数据:{xieids},{ChangYong.ByteOrString(can_data.frame.data, " ")},{iscunzai},{result}");
                }
                else //canfd
                {
                    byte[] data = ChangYong.HexStringToByte(zhiling);
                    ZCAN_TransmitFD_Data canfd_data = new ZCAN_TransmitFD_Data();
                    canfd_data.frame.can_id = MakeCanId(xieid, frame_type_index, 0, 0);
                    canfd_data.frame.data = new byte[64];
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (i < 64)
                        {
                            canfd_data.frame.data[i] = data[i];
                        }
                    }
                    canfd_data.frame.len = (byte)canfd_data.frame.data.Length;
                    canfd_data.transmit_type = (uint)send_type_index;
                    canfd_data.frame.flags = (byte)((canfd_exp_index != 0) ? 1 : 0);
                    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(canfd_data));
                    Marshal.StructureToPtr(canfd_data, ptr, true);
                    bool iscunzai = false;
                    if (shebeimodel.Canshuju.ContainsKey(index))
                    {
                        result = Method.ZCAN_TransmitFD(shebeimodel.Canshuju[index], ptr, 1);
                        iscunzai = true;
                    }
                    Marshal.FreeHGlobal(ptr);

                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{shebeimodel.Name} 写数据:{xieids},{ChangYong.ByteOrString(canfd_data.frame.data," ")},{iscunzai},{result}");
                }
                if (result != 1)
                {
                    string error = AddErr();
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang,$"{shebeimodel.Name} 写数据:{error}");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                ChuFaMsg(MsgDengJi.SheBeiCuoWu,$"{shebeimodel.Name} 写数据:{e}");
            }
            return false;
        }

   
        private uint MakeCanId(uint id, int eff, int rtr, int err)//1:extend frame 0:standard frame
        {
            uint ueff = (uint)(!!(Convert.ToBoolean(eff)) ? 1 : 0);
            uint urtr = (uint)(!!(Convert.ToBoolean(rtr)) ? 1 : 0);
            uint uerr = (uint)(!!(Convert.ToBoolean(err)) ? 1 : 0);
            return id | ueff << 31 | urtr << 30 | uerr << 29;
        }

        private string AddErr()
        {
            ZCAN_CHANNEL_ERROR_INFO pErrInfo = new ZCAN_CHANNEL_ERROR_INFO();
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(pErrInfo));
            Marshal.StructureToPtr(pErrInfo, ptr, true);
           
            Marshal.FreeHGlobal(ptr);

            string errorInfo = String.Format("错误码：{0:D1}", pErrInfo.error_code);
            return errorInfo;
        }
        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            CunModel cunModel = PeiZhiLei.DataMoXing.GetModel(jiCunQiModel);
            if (cunModel != null)
            {
                if (cunModel.IsDu == CunType.XieZlgBuXieZhiDu)
                {
                    SaoMaModel models = PeiZhiLei.DataMoXing.GetSheBeiModel(cunModel);
                    if (models != null)
                    {
                        XieZlgBuXieZhiDuKJ kj = new XieZlgBuXieZhiDuKJ();
                        kj.SetCanShu(models.IndexCount);
                        return kj;
                    }
                }
                else if (cunModel.IsDu == CunType.XieZlgCanDaiKai)
                {
                    SaoMaModel models = PeiZhiLei.DataMoXing.GetSheBeiModel(cunModel);
                    if (models != null)
                    {
                        XieZlgCanDanGeXieRecKJ kj = new XieZlgCanDanGeXieRecKJ();
                        kj.SetCanShu(models.IndexCount,true);
                        return kj;
                    }
                }
                else if (cunModel.IsDu == CunType.XieZlgCanGuan)
                {
                    SaoMaModel models = PeiZhiLei.DataMoXing.GetSheBeiModel(cunModel);
                    if (models != null)
                    {
                        XieZlgCanDanGeXieRecKJ kj = new XieZlgCanDanGeXieRecKJ();
                        kj.SetCanShu(models.IndexCount, true);
                        return kj;
                    }
                }
                else if (cunModel.IsDu == CunType.XieZlgCanDanGeXieRec)
                {
                    SaoMaModel models = PeiZhiLei.DataMoXing.GetSheBeiModel(cunModel);
                    if (models != null)
                    {
                        XieZlgCanDanGeXieRecKJ kj = new XieZlgCanDanGeXieRecKJ();
                        kj.SetCanShu(models.IndexCount, false);
                        return kj;
                    }
                }
                else if (cunModel.IsDu == CunType.XieZlgYiZhiXie)
                {
                    SaoMaModel models = PeiZhiLei.DataMoXing.GetSheBeiModel(cunModel);
                    if (models != null)
                    {
                        XieZlgCanDanGeXieRecKJ kj = new XieZlgCanDanGeXieRecKJ();
                        kj.SetCanShu(models.IndexCount+1, true);
                        return kj;
                    }
                }
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
            model.IsXuYaoPanDuan = false;
            for (int i = 0; i < PeiZhiLei.DataMoXing.LisSheBei.Count; i++)
            {
                SaoMaModel item = PeiZhiLei.DataMoXing.LisSheBei[i];
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
