using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;
using YiBanSaoMaQi.Frm;
using YiBanSaoMaQi.Model;

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
                return "CA410色彩分析仪";
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
        private Dictionary<int, ABSLianJieQi> LisLianJieQi = new Dictionary<int, ABSLianJieQi>();

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
                    if (LisLianJieQi.ContainsKey(sModels[i].SheBeiID) == false)
                    {
                        SetLianJieQiModel model = new SetLianJieQiModel();
                        model.IPOrCOMStr = sModels[i].IpOrCom;
                        model.SpeedOrPort = sModels[i].Port;
                        ABSLianJieQi lianjieqi = null;
                        if (model.IPOrCOMStr.Contains("COM"))
                        {
                            lianjieqi = new RJ232OuLianJieQi();
                            lianjieqi.SetCanShu(model);
                        }
                        else
                        {
                            lianjieqi = new RJ45TcpLianJieQi();
                            lianjieqi.SetCanShu(model);
                        }
                        LisLianJieQi.Add(sModels[i].SheBeiID, lianjieqi);

                        SengData.Add(sModels[i].SheBeiID, new FanXingJiHeLei<List<CunModel>>());
                        Thread xiancheng = new Thread(ReadWork);
                        xiancheng.IsBackground = true;
                        xiancheng.DisableComObjectEagerCleanup();
                        xiancheng.Start(sModels[i]);

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
            StringBuilder msg = new StringBuilder();
            foreach (var item in LisLianJieQi.Keys)
            {
                ResultModel rm = LisLianJieQi[item].Open();
                if (rm.IsSuccess)
                {
                    PeiZhiLei.DataMoXing.SetHeGe(item, true);
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetHeGe(item, false);
                }
                msg.AppendLine(rm.Msg);
            }
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, msg.ToString());

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
        
        private void JieRu(List<JiCunQiModel> canshus)
        {
            foreach (var item in canshus)
            {
                CunModel ipcom =PeiZhiLei.DataMoXing.GetModel(item);

                if (ipcom != null)
                {
                    if (ipcom.IsDu.ToString().ToLower().StartsWith("du"))
                    {
                        continue;
                    }
                   
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(ipcom.JiCunQi.WeiYiBiaoShi,0);
                    if (LisLianJieQi.ContainsKey(ipcom.ZongSheBeiId))
                    {
                        CunModel cunmodel = ipcom.FuZhi();
                        cunmodel.JiCunQi = item.FuZhi();
                        SengData[ipcom.ZongSheBeiId].Add(new List<CunModel>() { cunmodel });
                    }
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel xinr = PeiZhiLei.DataMoXing.GetModel(jicunqiid);
            if (xinr != null)
            {
                if (xinr.JiCunQi.IsKeKao)
                {
                    if (xinr.IsZhengZaiCe == 1)
                    {
                        models.Value = xinr.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                    }
                    else if (xinr.IsZhengZaiCe == 3)
                    {
                        models.Value = xinr.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                    else if (xinr.IsZhengZaiCe == 2)
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
                    models.Value = "通讯不可靠";
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
            jieMianFrmModel.Form = new TiaoShiOrPeiZhiFrm(PeiZhiLei);
            return jieMianFrmModel;
        }

        public override List<JiCunQiModel> GetShuJu()
        {

            List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;

            return Get;
        }


        private void ReadWork(object zhi)
        {
            SaoMaModel zsmodel = zhi as SaoMaModel;
            ABSLianJieQi lianjieqi = LisLianJieQi[zsmodel.SheBeiID];
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
                if ((DateTime.Now- chongliantime).TotalMilliseconds>=2000)
                {
                    if (lianjieqi.TongXinState==false)
                    {
                        lianjieqi.ChongLian();
                    }
                    chongliantime = DateTime.Now;
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
                         
                            if (duixiang[i]!= null)
                            {                               
                                WriteRec(lianjieqi, duixiang[i], zsmodel);
                                Thread.Sleep(zsmodel.XieTime);
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

        public void WriteRec(ABSLianJieQi lianjieqi, CunModel model,SaoMaModel shebeimodel)
        {
            if (model != null)
            {
                if (model.IsDu == CunType.XiePeiZhi)
                {
                    bool isyou = false;
                    string[] fengezhiling = model.JiCunQi.Value.ToString().Split('*');
                    if (fengezhiling.Length > 0)
                    {
                        for (int i = 0; i < fengezhiling.Length; i++)
                        {
                            string[] fesss = fengezhiling[i].Split('#');
                            if (fesss.Length >= 2)
                            {
                                isyou = true;
                                byte[] data = Encoding.ASCII.GetBytes(fesss[0]);
                                data = data.ByteAdd(new byte[] { 0x0D });
                                bool isfanhui = fesss[1] == "1";
                                int duchangshi = shebeimodel.DuChaoShiTime;                         
                                List<byte> shuju = ZhenShiXie(model, lianjieqi, data, 2, duchangshi);
                                bool jieguo = true;
                                if (isfanhui)
                                {
                                    jieguo = PeiZhiLei.DataMoXing.SetJiCunQiValue(model, shuju.ToArray());
                                }

                                if (jieguo == false)
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, $"发送数据出错:{fesss[0]}");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                                    return;
                                }
                            }

                        }
                        if (isyou)
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "数据有问题");
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送:{model.JiCunQi.Value} ");
                        }
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "没有指令");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送:{model.JiCunQi.Value} ");
                    }


                }
                else if (model.IsDu == CunType.XieTestShuJu|| model.IsDu == CunType.XieJunYunDu)
                {
                    string[] zhis = model.JiCunQi.Value.ToString().Split('#');
                    if (zhis.Length >= 2)
                    {
                        byte[] data = Encoding.ASCII.GetBytes(zhis[0]);
                        data = data.ByteAdd(new byte[] { 0x0D });
                        int jieshoucount = ChangYong.TryInt(zhis[1], 10);
                        int duchangshi = shebeimodel.DuChaoShiTime;
                        List<byte> shuju = ZhenShiXie(model, lianjieqi, data, jieshoucount, duchangshi);
                        bool jieguo = PeiZhiLei.DataMoXing.SetJiCunQiValue(model, shuju.ToArray());
                        if (jieguo == false)
                        {
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);

                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                        }
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "数据有问题");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                        ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"发送:{model.JiCunQi.Value} ");
                    }

                }
                else if (model.IsDu == CunType.XieQingLi)
                {
                    PeiZhiLei.DataMoXing.QingLing(model.ZongSheBeiId);
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "OK");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                }
                else if (model.IsDu == CunType.XieQiuSeYu)
                {
                    double shuju = 0;
                    bool jieguo= PeiZhiLei.DataMoXing.QiuSeYu(model,out shuju);
                    if (jieguo)
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, shuju.ToString());
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "还差一些值");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                    }
                }
                else if (model.IsDu == CunType.XieQiuDuiBiDu)
                {
                    double shuju = 0;
                    bool jieguo = PeiZhiLei.DataMoXing.QiuSeDuiBiDui(model, out shuju);
                    if (jieguo)
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, shuju.ToString());
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "还差一些值");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                    }
                }
                else if (model.IsDu == CunType.XieQiuJunYuanDu)
                {
                    double shuju = 0;
                    bool jieguo = PeiZhiLei.DataMoXing.QiuJunYunDu(model, out shuju);
                    if (jieguo)
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, shuju.ToString());
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "还差一些值");
                        PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 1);
                    }
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model.JiCunQi.WeiYiBiaoShi, "没有找到相应的配置");
                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
                }

            }
            else
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model.JiCunQi.WeiYiBiaoShi, 2);
            }
        }

        private List<byte> ZhenShiXie(CunModel model, ABSLianJieQi lianjieqi, byte[] zhiling,int jieshoucount,int ducaoshi)
        {          
            lianjieqi.Send(zhiling);
            DateTime shijian = DateTime.Now;
            List<byte> datalis = new List<byte>();
            for (; ZongKaiGuan;)
            {
                ResultModel rm = lianjieqi.Read();
                if (rm.IsSuccess)
                {
                    datalis.AddRange(rm.TData);
                }
                if (datalis.Count > jieshoucount)
                {
              
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.JiCunQi.WeiYiBiaoShi}发送:{ChangYong.ByteOrString(zhiling, " ")} 接收:{ChangYong.ByteOrString(datalis, " ")} 时间差:{(DateTime.Now - shijian).TotalMilliseconds}ms");
                    break;
                }
                if ((DateTime.Now - shijian).TotalMilliseconds >= ducaoshi)
                {
                    ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"{model.JiCunQi.WeiYiBiaoShi}发送:{ChangYong.ByteOrString(zhiling, " ")} 接收:{ChangYong.ByteOrString(datalis, " ")} 时间差:{(DateTime.Now - shijian).TotalMilliseconds}ms   超时");
                    break;
                }
                Thread.Sleep(1);
            }

            return datalis;
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

    //
    // 摘要:
    //     232与485的通信连接器
    public class RJ232OuLianJieQi : ABSLianJieQi
    {
        //
        // 摘要:
        //     串口对象
        private SerialPort SerPortCOM;

        //
        // 摘要:
        //     串口参数对象
        private SetLianJieQiModel SerportModel;

        private int ShouCi = 0;

        private bool IsZhenCOM = false;

        //
        // 摘要:
        //     描述
        private string _ComMiaoSu = "";

        //
        // 摘要:
        //     连接器的名称
        public override string Name => _ComMiaoSu;

        //
        // 摘要:
        //     true表示通讯上
        public override bool TongXinState
        {
            get
            {
                bool result = false;
                if (SerPortCOM != null)
                {
                    result = SerPortCOM.IsOpen;
                }

                return result;
            }
        }

        //
        // 摘要:
        //     构造函数
        public RJ232OuLianJieQi()
        {
        }

        //
        // 摘要:
        //     发送数据的方法,如果ResultModel.IsChengGong==true,
        //
        // 参数:
        //   bytData:
        //     发送数据
        //
        // 返回结果:
        //     返回一个 ResultModel对象
        public override ResultModel Send(byte[] bytData)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                if (TongXinState)
                {
                    Clear();
                    SerPortCOM.Write(bytData, 0, bytData.Length);
                    resultModel.SetValue(issuccess: true, string.Format("{0}:数据成功发送:{1}", _ComMiaoSu, ChangYong.ByteOrString(bytData, " ")));
                }
                else
                {
                    resultModel.SetValue(issuccess: false, string.Format("{0}:串口未打开,不能发送数据:{1}", _ComMiaoSu, ChangYong.ByteOrString(bytData, " ")));
                }
            }
            catch (Exception ex)
            {
                resultModel.SetValue(issuccess: false, string.Format("{0}：发送数据异常信息:{1},{2}", _ComMiaoSu, ex.Message, ChangYong.ByteOrString(bytData, " ")));
            }

            return resultModel;
        }

        //
        // 摘要:
        //     读取数据方法,如果ResultModel.IsChengGong==true
        //
        // 返回结果:
        //     返回一个 ResultModel对象
        public override ResultModel Read()
        {
            ResultModel resultModel = new ResultModel();
            resultModel.IsSuccess = false;
            try
            {
                if (TongXinState)
                {
                    if (SerPortCOM.BytesToRead > 0)
                    {
                        int bytesToRead = SerPortCOM.BytesToRead;
                        byte[] array = new byte[bytesToRead];
                        if (SerPortCOM.Read(array, 0, array.Length) > 0)
                        {
                            resultModel.SetValue(issuccess: true, string.Format("{0}:数据成功接收:{1}", _ComMiaoSu, ChangYong.ByteOrString(array, " ")), array);
                        }
                    }
                }
                else
                {
                    resultModel.SetValue(issuccess: false, $"{_ComMiaoSu}:接收数据失败,串口未打开 ");
                }
            }
            catch (Exception ex)
            {
                resultModel.SetValue(issuccess: false, string.Format($"{_ComMiaoSu}:数据接收异常:{ex.Message}"));
            }

            return resultModel;
        }

        //
        // 摘要:
        //     打开串口的方法,如果ResultModel.IsChengGong==true,打开成功
        //
        // 返回结果:
        //     返回一个 ResultModel对象
        public override ResultModel Open()
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                if (IsZhenCOM)
                {
                    SerPortCOM.Open();
                    if (SerPortCOM.IsOpen)
                    {
                        resultModel.SetValue(issuccess: true, $"{_ComMiaoSu}:打开成功");
                    }
                    else
                    {
                        resultModel.SetValue(issuccess: false, $"{_ComMiaoSu}:打开失败");
                    }
                }
                else
                {
                    resultModel.SetValue(issuccess: false, $"{_ComMiaoSu}:配置的COM有问题");
                }
            }
            catch (Exception ex)
            {
                resultModel.SetValue(issuccess: false, $"{_ComMiaoSu}:打开出现异常:{ex.Message}");
            }

            return resultModel;
        }

        //
        // 摘要:
        //     关闭连接
        //
        // 返回结果:
        //     返回ResultModel对象
        public override ResultModel Close()
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                if (SerPortCOM != null)
                {
                    SerPortCOM.Close();
                    resultModel.SetValue(issuccess: true, $"{_ComMiaoSu}:关闭成功");
                }
            }
            catch (Exception ex)
            {
                resultModel.SetValue(issuccess: true, $"{_ComMiaoSu}:关闭出现异常,信息:{ex.Message}");
            }

            return resultModel;
        }

        private void Clear()
        {
            if (TongXinState)
            {
                SerPortCOM.DiscardInBuffer();
                SerPortCOM.DiscardOutBuffer();
            }
        }

        //
        // 摘要:
        //     设置连接的参数
        //
        // 参数:
        //   model:
        public override void SetCanShu(SetLianJieQiModel model)
        {
            SerportModel = model;
            _ComMiaoSu = string.Format("{0}:{1},{2}*{3}*{4}串口", model.IPOrCOMStr, model.SpeedOrPort, 8, 1, "无校验");
            if (string.IsNullOrEmpty(SerportModel.IPOrCOMStr))
            {
                IsZhenCOM = false;
            }
            else
            {
                IsZhenCOM = true;
            }

            if (IsZhenCOM)
            {
                SerPortCOM = new SerialPort();
                SerPortCOM.PortName = SerportModel.IPOrCOMStr;
                SerPortCOM.BaudRate = SerportModel.SpeedOrPort;
                SerPortCOM.DataBits = 7;
                SerPortCOM.StopBits = StopBits.Two;
                SerPortCOM.Parity = Parity.Even;
                SerPortCOM.ReadBufferSize = 500;
            }
        }

        //
        // 摘要:
        //     重连接用的
        public override bool ChongLian()
        {
            try
            {
                if (ShouCi == 0)
                {
                    ShouCi++;
                    try
                    {
                        if (SerPortCOM != null)
                        {
                            SerPortCOM.Close();
                            SerPortCOM.Dispose();
                            SerPortCOM = null;
                        }
                    }
                    catch
                    {
                    }
                }

                SerPortCOM = new SerialPort();
                if (string.IsNullOrEmpty(SerportModel.IPOrCOMStr))
                {
                    IsZhenCOM = false;
                }
                else
                {
                    IsZhenCOM = true;
                }

                if (IsZhenCOM)
                {
                    SerPortCOM.PortName = SerportModel.IPOrCOMStr;
                    SerPortCOM.BaudRate = SerportModel.SpeedOrPort;
                    SerPortCOM.DataBits = 8;
                    SerPortCOM.StopBits = StopBits.One;
                    SerPortCOM.Parity = Parity.None;
                    SerPortCOM.ReadBufferSize = 500;
                    SerPortCOM.Open();
                    if (SerPortCOM.IsOpen)
                    {
                        ShouCi = 0;
                        return true;
                    }
                }
            }
            catch
            {
            }

            return false;
        }
    }
}
