using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using JieMianLei.FuFrom.KJ;
using ModBuFuWuQi.ShiXian;
using ModBuTCP.Frm;
using ModBuTCP.Model;
using SSheBei.ABSSheBei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;

namespace ModBuTCP.ShiXian
{
    public class ModBusTCPLei : ABSNSheBei
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
                return "ModbusTCP设备";
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
        /// 连接器
        /// </summary>
        private Dictionary<int, ModbusTCP> LisLianJieQi = new Dictionary<int, ModbusTCP>();  
        public ModBusTCPLei()
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
                List<SheBeiModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < sModels.Count; i++)
                {
                    if (LisLianJieQi.ContainsKey(sModels[i].SheBeiID) == false)
                    {
                        ModbusTCP modbus = new ModbusTCP();
                        modbus.MsgEvent += Modbus_MsgEvent;
                        modbus.SetCanShu(sModels[i]);
                        LisLianJieQi.Add(sModels[i].SheBeiID, modbus);
                     
                        Thread xiancheng = new Thread(ReadWork);
                        xiancheng.IsBackground = true;
                        xiancheng.DisableComObjectEagerCleanup();
                        xiancheng.Start(sModels[i]);

                    }
                }
            }
        }

        private void Modbus_MsgEvent(int arg1, string arg2)
        {
            ChuFaMsg(arg1==1?MsgDengJi.SheBeiBaoWen: MsgDengJi.SheBeiCuoWu, arg2);
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
                bool zhen = LisLianJieQi[item].Open();
                PeiZhiLei.DataMoXing.SetTx(item, zhen);
              
            }
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, "服务器已经打开");

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
                for (int i = 0; i < canshus.Count; i++)
                {
                   PeiZhiLei.DataMoXing.GetModel(canshus[i]);
                   
                }                   
            }
        }

        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
          
            DataCunModel zhen = PeiZhiLei.DataMoXing.IsChengGong(jicunqiid);
            if (zhen!=null)
            {
                if (zhen.JiCunQiModel.IsKeKao)
                {
                    models.Value = zhen.JiCunQiModel.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else
                {
                    models.Value = zhen.JiCunQiModel.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.BuKeKaoJieGuo;
                }
            }
            else
            {
                models.Value = "未找到";
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
                List<JiCunQiModel> Getdu = PeiZhiLei.DataMoXing.LisDuXie;
                shuju = Getdu;

            }
            return shuju;
        }



        public override JieMianFrmModel GetFrm(bool istiaoshi)
        {
            PeiZhiLei.IsTiaoShi = istiaoshi;
            JieMianFrmModel jieMianFrmModel = new JieMianFrmModel();
            jieMianFrmModel.SheBeiName = SheBeiName;
            jieMianFrmModel.SheBeiID = SheBeiID;
            jieMianFrmModel.Form = new PeiZhiFrm(PeiZhiLei);
            return jieMianFrmModel;
        }

        public override List<JiCunQiModel> GetShuJu()
        {

            List<JiCunQiModel> Get = PeiZhiLei.DataMoXing.LisDu;

            return Get;
        }

        public override object GetLianJieQi(int id)
        {
            if (LisLianJieQi.ContainsKey(id))
            {
                return LisLianJieQi[id];
            }
            return null;
        }
        private void ReadWork(object zhi)
        {
            SheBeiModel zsmodel = zhi as SheBeiModel;
            ModbusTCP lianjieqi = LisLianJieQi[zsmodel.SheBeiID];
            int yanshi = 5;
         
            DateTime jishicongnian = DateTime.Now;
       

         
            while (ZongKaiGuan)
            {
                if (DengDaiOpen == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                lianjieqi.UpdatePlcReadData();
                Thread.Sleep(yanshi);
            }
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
                SheBeiModel item = PeiZhiLei.DataMoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.Tx;
                zmodel.ZiSheBeiID = item.SheBeiID;
                zmodel.ZiSheBeiName = item.SheBeiName;
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
