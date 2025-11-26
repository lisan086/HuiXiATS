using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using Common.DataChuLi;
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

        private bool IsKaiGuan = false;

        public override string SheBeiType
        {
            get
            {
                return "脚本管理";
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
            List<SaoMaModel> sModels = PeiZhiLei.DataMoXing.LisSheBei;
            StringBuilder msg = new StringBuilder();
            foreach (var item in sModels)
            {
                PeiZhiLei.DataMoXing.SetHeGe(item.SheBeiID, true);
                msg.AppendLine($"{item.Name}:打开成功");
            }
            DengDaiOpen = true;
            ChuFaMsg(MsgDengJi.SheBeiZhengChang, msg.ToString());

        }
        public override void Close()
        {
            ZongKaiGuan = false;
           
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
            Dictionary<string, CunModel> sModels = PeiZhiLei.DataMoXing.JiLu;
            if (sModels.ContainsKey(jicunqi.WeiYiBiaoShi))
            {
                CunModel models = sModels[jicunqi.WeiYiBiaoShi];
                if (models.IsDu==CunType.DuShuJu)
                {
                    return null;
                }

                PeiZhiLei.DataMoXing.SetZhengZaiValue(models,0);
                CunModel xinmodel = models.FuZhi();
                xinmodel.JiCunQi= jicunqi;
                return xinmodel;
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
            CunModel xinr = PeiZhiLei.DataMoXing.GetModel(jicunqiid);
            if (xinr != null)
            {
                if (xinr.IsZhengZaiCe == 1)
                {
                    models.Value = xinr.JiCunQi.Value;
                    models.IsZuiZhongJieGuo = JieGuoType.ChengGongJiGuo;
                }
                else if (xinr.IsZhengZaiCe == 3)
                {
                    models.Value = "ChaoShi";
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
                                
                                WriteRec(duixiang[i]);
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

        public void WriteRec(CunModel model)
        {
            PeiZhiLei.DataMoXing.SetZhengZaiValue(model,0);

            if (model != null)
            {
                switch (model.IsDu)
                {
                    case CunType.ABSJia:
                        {
                            string[] fenge = model.JiCunQi.Value.ToString().Split('#');
                            if (fenge.Length >= 2)
                            {
                                bool zhena = string.IsNullOrEmpty(fenge[0]) == false;
                                bool zhenb = string.IsNullOrEmpty(fenge[1]) == false;
                                if (zhena && zhenb)
                                {
                                    double a = ChangYong.TryDouble(fenge[0], 0);
                                    double b = ChangYong.TryDouble(fenge[1], 0);
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, Math.Abs(a + b));
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                                }
                                else
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "参数值有的空");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                                }

                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "参数没有#");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                            }
                            return;
                        }
                    case CunType.ABSJian:
                        {
                            string[] fenge = model.JiCunQi.Value.ToString().Split('#');
                            if (fenge.Length >= 2)
                            {
                                bool zhena = string.IsNullOrEmpty(fenge[0]) == false;
                                bool zhenb = string.IsNullOrEmpty(fenge[1]) == false;
                                if (zhena && zhenb)
                                {
                                    double a = ChangYong.TryDouble(fenge[0], 0);
                                    double b = ChangYong.TryDouble(fenge[1], 0);
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, Math.Abs(a - b));
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                                }
                                else
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "参数值有的空");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                                }

                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "参数没有#");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                            }
                            return;
                        }
                    case CunType.BoFangYingYue:
                        {
                            string lujing = model.JiCunQi.Value.ToString();
                            if (File.Exists(lujing))
                            {
                                try
                                {
                                    System.Media.SoundPlayer sp = new SoundPlayer();
                                    sp.SoundLocation = lujing;
                                    sp.Load();
                                    sp.Play();
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "OK");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                                }
                                catch (Exception ex)
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, $"播放异常:{ex.Message}");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);

                                }

                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, $"文件不存在:{lujing}");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                            }

                            return;
                        }               
                    case CunType.MoLiTuPian:
                        {
                            int index = ChangYong.TryInt(model.JiCunQi.Value.ToString(),0);
                            {
                                IsKaiGuan = false;
                                Thread.Sleep(10);
                                int todao = ChangYong.TryInt(model.JiCunQi.Value, 0);
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "OK");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                                IsKaiGuan = true;
                                Task.Factory.StartNew(() =>
                                {
                                    int intindex = todao;
                                    int shebeiid = model.ZongSheBeiId;
                                    List<byte[]> shuju = new List<byte[]>();
                                    DateTime shijian = DateTime.Now;
                                    SuiJiShuLei suiJiShu = new SuiJiShuLei();
                                    int kaishiweiy = 10;
                                    try
                                    {
                                      
                                        for (; IsKaiGuan&&ZongKaiGuan;)
                                        {
                                            List<byte> changshengshuju = new List<byte>();
                                            changshengshuju.Add(0x00);
                                            changshengshuju.Add(0x00);
                                            changshengshuju.Add(0xCC);
                                            byte[] shujsux= CRC.ShiOrByte2(150,true);
                                            changshengshuju.AddRange(shujsux);
                                            kaishiweiy += suiJiShu.SuiJiData(1, 3);
                                            byte[] shujsuy = CRC.ShiOrByte2(kaishiweiy, true);
                                            changshengshuju.AddRange(shujsuy);                                                                             
                                            changshengshuju.Add(0x00);
                                            string jieguo = $"{ChangYong.ByteOrString(changshengshuju, " ")}";

                                            shuju.Add(changshengshuju.ToArray());
                                            PeiZhiLei.DataMoXing.SetJiCunQiValue(model, shuju);
                                            ChuFaMsg(MsgDengJi.SheBeiBaoWen, $"记录点:{jieguo}");
                                            Thread.Sleep(200);
                                            if ((DateTime.Now- shijian).TotalSeconds>=20)
                                            {
                                                IsKaiGuan = false;
                                                PeiZhiLei.DataMoXing.SetJiCunQiValue(shebeiid,1);
                                            }
                                        }

                                    }
                                    catch
                                    {

                                    }
                                });
                            }
                        }
                        break;                  
                    case CunType.JieQuStrShuJuChang:
                        {
                            string[] fenge = model.JiCunQi.Value.ToString().Split('#');
                            if (fenge.Length >= 3)
                            {
                                string shujua = fenge[0];
                                int kaishi = ChangYong.TryInt(fenge[1],0);
                                int count = ChangYong.TryInt(fenge[2], 0);
                                try
                                {
                                    string jiequzhi = shujua.Substring(kaishi, count);
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, jiequzhi);
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);
                                }
                                catch 
                                {
                                    PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "参数截取过长");
                                    PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                                }
                              

                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, "参数没有#");
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                            }
                            return;
                        }
                    case CunType.XieJiaoBen:
                        {
                            string canshu = model.JiCunQi.Value.ToString();
                            bool ishege = false;
                            object jieguo= PeiZhiLei.DataMoXing.ZhiXingJiaoBen(model,canshu,out ishege);
                            if (ishege)
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, jieguo);
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 1);

                            }
                            else
                            {
                                PeiZhiLei.DataMoXing.SetJiCunQiValue(model, jieguo);
                                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
                            }
                            return;
                        }
                    default:
                        break;
                }


            }
            else
            {
                PeiZhiLei.DataMoXing.SetZhengZaiValue(model, 2);
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
