using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using SaoMaKeHuFuDuanLei.Frm;
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
                return "传递服务器";
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

     

        private FanXingJiHeLei<CunModel> SengData = new FanXingJiHeLei<CunModel>();


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
                List<IPFuWuPeiModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
                FangWenFuWuDuan.Ceratei().MsgEvent += ZhiLingSaoMaQiLei_MsgEvent; ;

                Thread xiancheng = new Thread(ReadWork);
                xiancheng.IsBackground = true;
                xiancheng.DisableComObjectEagerCleanup();
                xiancheng.Start();
            }
        }

        private void ZhiLingSaoMaQiLei_MsgEvent(string obj)
        {
            ChuFaMsg(MsgDengJi.SheBeiBaoWen, obj);
        }

        private void ZhongShengIO_XieIOEvent(JiCunQiModel obj)
        {
            Clear(false,obj);
            XieShuJu(new List<JiCunQiModel>() { obj });
        }

        public override void Open()
        {
           
            List<IPFuWuPeiModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
            FangWenFuWuDuan.Ceratei().Open(sModels,PeiZhiLei);
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang,"设备开启");

        }
        public override void Close()
        {
            ZongKaiGuan = false;
            FangWenFuWuDuan.Ceratei().Close();

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
            CunModel model = PeiZhiLei.DataMoXing.GetModel(jicunqi);
            if (model!=null)
            {
                if (model.IsDu==CunType.DuWenJianState)
                {
                    return null;
                }
                CunModel models = model.FuZhi();
                PeiZhiLei.DataMoXing.SetZhuangTaiZhi(models, 0);
                models.JiCunQi = jicunqi;
                return models;
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
                    SengData.Add(ipcom);
                }
            }
        }
        public override JiaoYanJieGuoModel JiaoYanChengGong(JiCunQiModel jicunqiid)
        {
            JiaoYanJieGuoModel models = new JiaoYanJieGuoModel();
            models.WeiYiBiaoShi = jicunqiid.WeiYiBiaoShi;
            models.SheBeiID = jicunqiid.SheBeiID;
            CunModel xinr = PeiZhiLei.DataMoXing.IsChengGong(jicunqiid.WeiYiBiaoShi);
            if (xinr != null)
            {
                if (xinr.IsDu == CunType.DuWenJianState)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else
                {
                    if (xinr.IsZhengZaiCe == 1)
                    {
                        models.Value = xinr.JiCunQi.Value;
                        models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                    }
                    else if (xinr.IsZhengZaiCe == 3)
                    {
                        models.Value = "";
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                    else if (xinr.IsZhengZaiCe == 2)
                    {
                        models.Value = "";
                        models.IsZuiZhongJieGuo = JieGuoType.ShiBaiJiGuo;
                    }
                    else
                    {
                        models.Value = "";
                        models.IsZuiZhongJieGuo = JieGuoType.JingXingZhong;
                    }
                }
            }
            else
            {
                models.Value = "";
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


        private void ReadWork()
        {
         

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
                    int  shuju = SengData.GetCount();
                    if (shuju>0)
                    {
                       
                        CunModel duixiang = SengData.GetModel_Head_RomeHead();

                        WriteRec(duixiang);

                    }
                }
                catch (Exception ex)
                {
                    ChuFaMsg(MsgDengJi.SheBeiCuoWu, $"写报错:{ex}");
                }
                #endregion
             
                Thread.Sleep(10);
            }
        }

        public void WriteRec(CunModel model)
        {
            IPFuWuPeiModel shebeimodel = PeiZhiLei.DataMoXing.GetSheBei(model);
            if (shebeimodel != null)
            {
                if (model.IsDu == CunType.XieMaShuJu)
                {
                    string[] shuju = model.JiCunQi.Value.ToString().Split('*');
                    if (shuju.Length > 0)
                    {
                        List<SaoMaShuJuModel> lisshuju = new List<SaoMaShuJuModel>();
                        for (int i = 0; i < shuju.Length; i++)
                        {
                            string[] youfenge = shuju[i].Split(',');
                            if (youfenge.Length >= 3)
                            {
                                SaoMaShuJuModel modeld = new SaoMaShuJuModel();
                                modeld.HouMa = youfenge[2];
                                modeld.ShangMa = youfenge[1];
                                modeld.ShunXuID = ChangYong.TryInt(youfenge[0], 0);
                                lisshuju.Add(modeld);
                            }
                        }
                        FangWenFuWuDuan.Ceratei().SendShuJuFuWu(lisshuju, 1, "", true, shebeimodel.ZhanDianName);
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model, $"OK,{ChangYong.HuoQuJsonStr(lisshuju)}");
                        PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 1);
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "NG");
                        PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 2);
                    }
                }
                else if (model.IsDu == CunType.XieQingQiuShuJu)
                {
                    FangWenFuWuDuan.Ceratei().SendShuJuFuWu(new List<SaoMaShuJuModel>(), 3, "",true, shebeimodel.ZhanDianName);
                    DateTime now = DateTime.Now;
                    for (; ZongKaiGuan;)
                    {
                        SengModel mosdel = FangWenFuWuDuan.Ceratei().GetKeHuDuanShuJu(shebeimodel.ZhanDianName);
                        if (mosdel != null)
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, ChangYong.HuoQuJsonStr(mosdel));
                            PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 1);
                            break;
                        }
                        if ((DateTime.Now - now).TotalMilliseconds >= 3000)
                        {
                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "获取数据超时");
                            PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 2);
                            break;
                        }
                    }
                }
                else if (model.IsDu == CunType.XieZuoWanShuJu)
                {
                    string[] biaoshi = model.JiCunQi.Value.ToString().Split('*');
                    if (biaoshi.Length >= 2)
                    {
                        FangWenFuWuDuan.Ceratei().SendShuJuFuWu(new List<SaoMaShuJuModel>(), 4, biaoshi[0], biaoshi[1].Contains("1"), shebeimodel.ZhanDianName);
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "OK");
                        PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 1);
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "数据不对");
                        PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 2);
                    }
                }
                else if (model.IsDu == CunType.XieWenJianData)
                {
                    PeiZhiLei.DataMoXing.SetDuState(model.ZongSheBeiId);
                    string[] biaoshi = model.JiCunQi.Value.ToString().Split('*');
                    if (biaoshi.Length >= 2)
                    {
                        FangWenFuWuDuan.Ceratei().SendShuJuFuWu(new List<SaoMaShuJuModel>(), 4, biaoshi[0], biaoshi[1].Contains("1"), shebeimodel.ZhanDianName);
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "OK");
                        PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 1);
                    }
                    else
                    {
                        PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "NG");
                        PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 2);
                    }
                   
                }
                else
                {
                    PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 2);
                }
            }
            else
            {
                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "设备不存在");
                PeiZhiLei.DataMoXing.SetZhuangTaiZhi(model, 2);
            }
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

            for (int i = 0; i < PeiZhiLei.DataMoXing.LisSheBei.Count; i++)
            {
                IPFuWuPeiModel item =PeiZhiLei.DataMoXing.LisSheBei[i];
                ZiTxModel zmodel = new ZiTxModel();
                zmodel.Tx = item.Tx;
                zmodel.ZiSheBeiID = SheBeiID;
                zmodel.ZiSheBeiName = item.ZhanDianName;
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
}
