using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoTaiKeTXLei.Frm;
using BoTaiKeTXLei.Modle;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.LianJieQi;
using SSheBei.Model;
using CommLei.DataChuLi;
using System.Reflection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace BoTaiKeTXLei.ShiXian
{
    public class BoTaiKeLei : ABSNSheBei
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
                return "泊泰克类";
            }
        }

        public override string BanBenHao
        {
            get
            {
                return "V1.0";
            }
        }
        private SerialDataGuDingJieXi SerialDataGuDingJieXic = new SerialDataGuDingJieXi();
        private SerialDataGuDingJieXi SerialDataGuDingJieXif = new SerialDataGuDingJieXi();
        private SerialDataGuDingJieXi SerialDataGuDingJieXib = new SerialDataGuDingJieXi();
        /// <summary>
        /// 通信不起作用
        /// </summary>
        public override bool TongXin
        {
            get
            {
                bool zhen = false;
                if (LisLianJieQi.Count > 0)
                {
                    zhen = true;
                }
                foreach (var item in LisLianJieQi.Keys)
                {
                    if (LisLianJieQi[item].TongXinState == false)
                    {
                        return false;
                    }
                }
                return zhen;
            }
        }

        /// <summary>
        /// 连接器
        /// </summary>
        private Dictionary<string, ABSLianJieQi> LisLianJieQi = new Dictionary<string, ABSLianJieQi>();

        ///// <summary>
        ///// 连接器
        ///// </summary>
        //private Dictionary<int, bool> LisLianJieQiState = new Dictionary<int, bool>();

        private FanXingJiHeLei<List<SendModel>> SengData = new FanXingJiHeLei<List<SendModel>>();

    

        public BoTaiKeLei()
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
                List<YiQiModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (LisLianJieQi.ContainsKey(sModels[i].IP) == false)
                    {
                        SetLianJieQiModel model = new SetLianJieQiModel();
                        model.IPOrCOMStr = sModels[i].IP;
                        model.SpeedOrPort = sModels[i].DuanKou;
                        ABSLianJieQi lianjieqi = null;
                        lianjieqi = new TCPLianJieQiN();
                        lianjieqi.SetCanShu(model);
                        LisLianJieQi.Add(sModels[i].IP, lianjieqi);
                        
                    
                    }
                }

                Thread xiancheng = new Thread(ReadWork);
                xiancheng.IsBackground = true;
                xiancheng.DisableComObjectEagerCleanup();
                xiancheng.Start();
            }
        }

        private void ZhongShengIO_XieIOEvent(SendModel obj,int biaozhi)
        {
            SendModel mogg = PeiZhiLei.DataMoXing.GetModel(obj.IsABO);
            if (mogg != null)
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(mogg.JiCunQiModel, 0);
                SendModel xinmodel = obj.FuZhi();
                xinmodel.JiCunQiModel = mogg.JiCunQiModel.FuZhi();
                SengData.Add(new List<SendModel>() { xinmodel });
            }
        }

        public override void Open()
        {
            //StringBuilder msg = new StringBuilder();
            //foreach (var item in LisLianJieQi.Keys)
            //{
            //    ResultModel rm = LisLianJieQi[item].Open();
            //    if (rm.IsSuccess)
            //    {
            //        PeiZhiLei.DataMoXing.SetHeGe(item,true);
            //        LisLianJieQiState[item] = true;
            //    }
            //    msg.AppendLine(rm.Msg);
            //}
            DengDaiOpen = true;
           // ChuFaMsg(MsgDengJi.SheBeiZhengChang, msg.ToString());

        }
        public override void Close()
        {
            ZongKaiGuan = false;
            if (LisLianJieQi.Count > 0)
            {
                Thread.Sleep(100);
                Parallel.ForEach(LisLianJieQi.Keys, (x) => {
                    if (LisLianJieQi.ContainsKey(x))
                    {
                        LisLianJieQi[x].Close();
                    }
                });
                Thread.Sleep(200);
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
        private SendModel GetDiZhi(JiCunQiModel jicunqi)
        {
            try
            {
                SendModel sModels = PeiZhiLei.DataMoXing.GetModel(jicunqi);
                if (sModels!=null)
                {
                   
                    return sModels.FuZhi();
                }
               
            }
            catch 
            {

               
            }
       
            return null;
        }
        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
                SendModel ipcom = GetDiZhi(item);
                if (ipcom!=null)
                {
                    ipcom.SetCanShu(ChangYong.TryStr( item.Value.ToString(),""));
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(item, 0);
                    SengData.Add(new List<SendModel>() { ipcom });
                  
                }

            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            SendModel xinr = PeiZhiLei.DataMoXing.IsChengGong(jicunqiid);
            if (xinr != null)
            {             
                if (xinr.IsZhengZaiCe == 1)
                {
                    models.Value = xinr.JiCunQiModel.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 3)
                {
                    models.Value = xinr.JiCunQiModel.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 2)
                {
                    models.Value = xinr.JiCunQiModel.Value;
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
                shuju = ChangYong.FuZhiShiTi(Get);
            }
            else if (type == 2)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisXie;
                shuju = ChangYong.FuZhiShiTi(Get);
            }
            else if (type == 3)
            {
                List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;
                List<JiCunQiModel> xieget = PeiZhiLei.DataMoXing.LisXie;
                xieget.AddRange(Get);
                shuju = ChangYong.FuZhiShiTi(xieget);
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


        private void ReadWork()
        {
          
            DateTime chongliantime = DateTime.Now;

            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
               
                #region 写
                try
                {
                    int shuju = SengData.GetCount();
                    if (shuju > 0)
                    {
                        List<SendModel> duixiang = new List<SendModel>();
                        duixiang = SengData.GetModel_Head_RomeHead();

                        int count = duixiang.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (duixiang[i] != null)
                            {
                                WriteRec(duixiang[i]);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"写数据:写报错:{ex}");
                }
                #endregion
               

                Thread.Sleep(5);
            }
        }

        public void WriteRec(SendModel model)
        {
            if (model.IsABO == CunType.XieOpenBoTaiKe)
            {
                int cishu = 10;
                bool zhen = false;
                for (int i = 0; i < cishu; i++)
                {                  
                    foreach (var item in LisLianJieQi.Keys)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            bool isyaolianjie = true;
                            if (i > 1)
                            {
                                if (LisLianJieQi[item].TongXinState)
                                {
                                    isyaolianjie = false;
                                }
                            }
                            if (isyaolianjie)
                            {
                                ResultModel rm = LisLianJieQi[item].Open();
                                PeiZhiLei.DataMoXing.SetHeGe(item, rm.IsSuccess);
                                ChuFaMsg(MsgDengJi.SheBeiZhengChang, rm.Msg);
                            }
                        });

                    }                 
                    bool zhen1 = true;                
                    if (zhen1)
                    {
                        zhen = true;
                        break;
                    }
                }
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, zhen ? "OK" : "NG");
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, zhen ? 1 : 2);
            }
            else if (model.IsABO == CunType.XieCloseBoTaiKe)
            {           
                foreach (var item in LisLianJieQi.Keys)
                {
                    ResultModel rm = LisLianJieQi[item].Close();
                    PeiZhiLei.DataMoXing.SetHeGe(item, false);
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, rm.Msg);
                }               
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "OK");
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 1);
            }
            else if (model.IsABO == CunType.XieBoTaiKeFanHui8 || model.IsABO == CunType.XieBoTaiKeWuFanHui)
            {
                YiQiModel yiqimodel = PeiZhiLei.DataMoXing.GetSheBeiModel(model);
                if (yiqimodel != null)
                {
                    int lenf = yiqimodel.DuZiFuShu;
                    int chaoshi = yiqimodel.ChaoTime;
                    SerialDataGuDingJieXib.Clear();
                    SerialDataGuDingJieXic.Clear();
                    SerialDataGuDingJieXif.Clear();
                    try
                    {
                        ABSLianJieQi lianjieqi = LisLianJieQi[yiqimodel.IP];
                        for (int i = 0; i < 15; i++)
                        {
                            if (lianjieqi.TongXinState == false)
                            {
                                lianjieqi.Open();
                            }
                            else
                            {
                                break;
                            }
                        }
                        byte[] data = PeiZhiLei.DataMoXing.SendDate(model, yiqimodel.XieZiFuShu);
                        ResultModel jieguo = lianjieqi.Send(data);
                      
                        if (model.IsABO == CunType.XieBoTaiKeWuFanHui)
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "OK");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 1);
                            ChuFaMsg(MsgDengJi.SheBeiZhengChang, jieguo.Msg);
                            PeiZhiLei.DataMoXing.BaoCunJiLu(PeiZhiLei.JiLuPathName, model);
                            return;
                        }
                        {
                            StringBuilder ChongGouParam = new StringBuilder();
                            ChongGouParam.Append(model.CMD);

                            byte[] fff = ChangYong.HexStringToByte(ChongGouParam.ToString());
                            byte[] zhengc = new byte[] { 0x00, 0, 0, 0 };
                            byte[] ccd = new byte[] { 0x02, 0, 0, 0 };
                            byte[] fuzhengc = new byte[] { 0x01, 0, 0, 0 };

                            List<byte> zc = new List<byte>();
                            zc.AddRange(fff);
                            zc.AddRange(zhengc);

                            List<byte> cc = new List<byte>();
                            cc.AddRange(fff);
                            cc.AddRange(ccd);

                            List<byte> fc = new List<byte>();
                            fc.AddRange(fff);
                            fc.AddRange(fuzhengc);
                            PeiZhiLei.DataMoXing.BaoCunJiLu(PeiZhiLei.JiLuPathName, model);
                            DateTime shijian = DateTime.Now;
                            List<byte> datalis = new List<byte>();
                        
                            for (; ZongKaiGuan;)
                            {
                                ResultModel rm = lianjieqi.Read();
                                if (rm.IsSuccess)
                                {
                                    SerialDataGuDingJieXib.AddByteList(rm.TData);
                                    SerialDataGuDingJieXic.AddByteList(rm.TData);
                                    SerialDataGuDingJieXif.AddByteList(rm.TData);

                                    datalis.AddRange(rm.TData);
                                }

                                SerialDataGuDingJieXic.JieXiWanMeiData(lenf, zc.ToArray(), false, null, false);

                                SerialDataGuDingJieXib.JieXiWanMeiData(lenf, fc.ToArray(), false, null, false);

                                SerialDataGuDingJieXif.JieXiWanMeiData(lenf, cc.ToArray(), false, null, false);
                                if (SerialDataGuDingJieXic.DataCount > 0)
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, SerialDataGuDingJieXic.GetShiShiData().ToList());
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 1);
                                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.JiCunQiModel.WeiYiBiaoShi}:发送报文:{ChangYong.ByteOrString(data, " ")}\r\n回报文:{ChangYong.ByteOrString(datalis, " ")}  {(DateTime.Now - shijian).TotalMilliseconds}ms");

                                    return;
                                }
                                if (SerialDataGuDingJieXib.DataCount > 0)
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, SerialDataGuDingJieXib.GetShiShiData().ToList());
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 1);
                                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.JiCunQiModel.WeiYiBiaoShi}:发送报文:{ChangYong.ByteOrString(data, " ")}\r\n回报文:{ChangYong.ByteOrString(datalis, " ")}  {(DateTime.Now - shijian).TotalMilliseconds}ms");
                                    return;
                                }


                                if ((DateTime.Now - shijian).TotalMilliseconds >= chaoshi)
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "ChaoShi");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 3);
                                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.JiCunQiModel.WeiYiBiaoShi}:发送报文:{ChangYong.ByteOrString(data, " ")}\r\n回报文超时:{ChangYong.ByteOrString(datalis, " ")}  {(DateTime.Now - shijian).TotalMilliseconds}ms");
                                    return;
                                }
                                if (SerialDataGuDingJieXif.DataCount > 0)
                                {
                                    SerialDataGuDingJieXib.Clear();
                                    SerialDataGuDingJieXic.Clear();
                                    SerialDataGuDingJieXif.Clear();
                                    Thread.Sleep(1000);

                                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.JiCunQiModel.WeiYiBiaoShi}:发送报文:{ChangYong.ByteOrString(data, " ")}\r\n回报文:{ChangYong.ByteOrString(datalis, " ")}  {(DateTime.Now - shijian).TotalMilliseconds}ms");

                                    continue;
                                }
                                Thread.Sleep(1);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model, $"{ex}");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 2);
                    }
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "NOYiQi");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 2);
                }
            }
            else if (model.IsABO == CunType.XieGetZiShuJu)
            {
                object shuju = PeiZhiLei.DataMoXing.QuShuJu(model.JiCunQiModel.Value.ToString());
                if (shuju != null)
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, shuju.ToString());
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 1);
                    ChuFaMsg(MsgDengJi.SheBeiZhengChang, $"{shuju}");
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "NOData");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 2);
                }
            }
            else
            {
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "NOJiCunQi");
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQiModel, 2);
                ChuFaMsg(MsgDengJi.SheBeiZhengChang, "NOJiCunQi");
            }
            
        }


        public override KJPeiZhiJK GetCanShuKJ(string jicunweiyibiaoshi)
        {
            JiCunQiModel jiCunQiModel = new JiCunQiModel();
            jiCunQiModel.WeiYiBiaoShi = jicunweiyibiaoshi;
            SendModel cunModel = PeiZhiLei.DataMoXing.GetModel(jiCunQiModel);
            if (cunModel != null)
            {
                CanShuKJ kj = new CanShuKJ();
                kj.SetData(cunModel.IsABO,PeiZhiLei.DataMoXing.IPS,PeiZhiLei.DataMoXing.GetJiLuData(PeiZhiLei.JiLuPathName));
                return kj;
            }
            return base.GetCanShuKJ(jicunweiyibiaoshi);
        }
        public override void Clear(bool isquanbu, JiCunQiModel model)
        {
           
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
                YiQiModel item = PeiZhiLei.DataMoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.TX;
                zmodel.ZiSheBeiID = item.SheBeiID;
                zmodel.ZiSheBeiName = item.IP;
                if (zmodel.Tx == false)
                {
                    ischengg = false;
                }

                model.LisTx.Add(zmodel);

            }
            model.ZongTX = ischengg;
            return model;
        }
    }

    /// <summary>
    /// TCP连接器
    /// </summary>
    public class TCPLianJieQiN : ABSLianJieQi
    {
        #region 变量区

        /// <summary>
        /// 表示发送报文的客户端
        /// </summary>
        public TcpClient TcpClient;
        /// <summary>
        /// 表示报文链接流
        /// </summary>
        public NetworkStream NetworkStream;
        /// <summary>
        /// 存放rjmodel
        /// </summary>
        private SetLianJieQiModel RJmodel;
        /// <summary>
        /// 描述
        /// </summary>
        private string ComMiaoSu = "";
      
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>     
        public TCPLianJieQiN()
        {

        }



        /// <summary>
        /// 名称
        /// </summary>
        public override string Name
        {
            get
            {
                return ComMiaoSu;
            }
        }
        /// <summary>
        /// true表示通信成功
        /// </summary>
        public override bool TongXinState
        {
            get
            {
                if (TcpClient != null && TcpClient.Connected)
                {

                    return true;
                }
                else
                {

                    return false;
                }

            }
        }

        /// <summary>
        /// 关闭连接器
        /// </summary>
        /// <returns></returns>
        public override ResultModel Close()
        {
            ResultModel rm = new ResultModel();
            try
            {
                if (NetworkStream != null)
                {
                    NetworkStream.Close();
                    NetworkStream.Dispose();
                    NetworkStream = null;
                    rm.SetValue(true, string.Format("{0}:关闭成功", Name));
                }

            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:关闭出现异常:{1}", Name, ex.Message));


            }
            try
            {
                if (TcpClient != null)
                {
                    TcpClient.Close();
                    TcpClient.Dispose();
                    TcpClient = null;
                    rm.SetValue(true, string.Format("{0}:关闭成功", Name));
                }

            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:关闭出现异常:{1}", Name, ex.Message));


            }

            return rm;
        }

        /// <summary>
        /// 打开连接器
        /// </summary>
        /// <returns></returns>
        public override ResultModel Open()
        {
            ResultModel rm = new ResultModel();
            try
            {

                ChongLian();

                if (TcpClient.Connected)
                {
                   // NetworkStream = TcpClient.GetStream();
                    rm.SetValue(true, string.Format("{0}:打开成功", ComMiaoSu));

                }
                else
                {

                    rm.SetValue(false, string.Format("{0}:打开失败", ComMiaoSu));
                }

            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:打开异常:{1}", ComMiaoSu, ex.Message));


            }
            return rm;
        }

        /// <summary>
        /// 连接器读数
        /// </summary>
        /// <returns></returns>
        public override ResultModel Read()
        {
            ResultModel rm = new ResultModel();
            try
            {
                if (TongXinState)
                {
                    if (NetworkStream.DataAvailable)
                    {
                        List<byte> ReceivDate = new List<byte>();
                        byte[] receiveBuffer = new byte[1024];
                        int bytesRead = NetworkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                        if (bytesRead > 0)
                        {
                            for (int i = 0; i < bytesRead; i++)
                            {
                                ReceivDate.Add(receiveBuffer[i]);
                            }

                        }
                        rm.SetValue(true, string.Format("{0}:读取数据成功:{1}", ComMiaoSu, ChangYong.ByteOrString(ReceivDate, " ")), ReceivDate.ToArray());

                    }


                }
                else
                {
                    rm.SetValue(false, string.Format("{0}:连接口是关闭状态，不能读取数据", ComMiaoSu));
                }
            }
            catch (Exception ex)
            {

                rm.SetValue(false, string.Format("{0}:读取数据异常;信息:{1}", ComMiaoSu, ex.Message));


            }


            return rm;
        }

        /// <summary>
        /// 连接器发送数据
        /// </summary>
        /// <param name="bytData"></param>
        /// <returns></returns>
        public override ResultModel Send(byte[] bytData)
        {
            ResultModel rm = new ResultModel();
            try
            {
                if (TongXinState)
                {
                   
                    NetworkStream.Write(bytData, 0, bytData.Length);
                    NetworkStream.Flush();
                    rm.SetValue(true, string.Format("{0}:发送数据成功:{1}", Name, ChangYong.ByteOrString(bytData, " ")));
                }
                else
                {
                    rm.SetValue(false, string.Format("{0}:连接口是关闭状态，不能发送数据:{1}", Name, ChangYong.ByteOrString(bytData, " ")));
                }
            }
            catch (Exception ex)
            {
                rm.SetValue(false, string.Format("{0}:发送数据异常:{1};信息:{2}", Name, ChangYong.ByteOrString(bytData, " "), ex.Message));


            }
            return rm;
        }
        /// <summary>
        /// 连接器设置参数
        /// </summary>
        /// <param name="model"></param>
        public override void SetCanShu(SetLianJieQiModel model)
        {
            this.RJmodel = model;
            ComMiaoSu = string.Format("{0}*{1}TCP", RJmodel.IPOrCOMStr, RJmodel.SpeedOrPort);
            this.TcpClient = new TcpClient();

        }
        /// <summary>
        /// 重连
        /// </summary>
        /// <returns></returns>
        public override bool ChongLian()
        {
            try
            {
                Close();
                this.TcpClient = new TcpClient();
                TcpClient.Connect(new IPEndPoint(IPAddress.Parse(RJmodel.IPOrCOMStr), RJmodel.SpeedOrPort));
                if (TcpClient.Connected)
                {
                    NetworkStream = TcpClient.GetStream();
                   
                    return true;
                }
            }
            catch
            {


            }
            return false;
        }
    }
}
