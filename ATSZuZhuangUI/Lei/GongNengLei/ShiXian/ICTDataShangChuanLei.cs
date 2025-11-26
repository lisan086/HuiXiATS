using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using ATSJianMianJK.Log;
using ATSJuanChengZuZhuangUI.Model;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using SSheBei.Model;
using ZuZhuangUI.Lei;
using ZuZhuangUI.Lei.GongNengLei;
using ZuZhuangUI.Model;

namespace ATSFoZhaoZuZhuangUI.Lei.GongNengLei.ShiXian
{
    public class ICTDataShangChuanLei : ABSGongNengLei
    {
      
        private bool ZongKaiGuan = true;
        private SheBeiZhanModel SheBeiZhanModel;
     
        public override void CaoZuo(DoType doType, JieMianCaoZuoModel model)
        {
           
        }

        public override void Close()
        {
            ZongKaiGuan = false;
        }

        public override void IniData(SheBeiZhanModel model)
        {
            SheBeiZhanModel = model;
            {
                Thread thread = new Thread(ChuZhanQingQiu);
                thread.IsBackground = true;
                thread.DisableComObjectEagerCleanup();
                thread.Start();              
            }
            
        }

        private void ChuZhanQingQiu()
        {
            if (SheBeiID < 0)
            {
                return;
            }
     
            while (ZongKaiGuan)
            {
              
                //处理出站
                {
                    try
                    {

                        YeWuDataModel jinzhanzhi = DataJiHe.Cerate().GetShouGeModel(SheBeiID, CaoZuoType.ICTChuZhan请求_单,false);
                        if (jinzhanzhi != null)
                        {
                            if (jinzhanzhi.Value.JiCunValue is List<string>)
                            {
                                WriteLog(RiJiEnum.TDLiuCheng, $"收到ICT出站请求");
                                Thread.Sleep(500);
                                List<string> sjdu = jinzhanzhi.Value.JiCunValue as List<string>;
                                ChuZhanJiaoYan(sjdu);
                                Thread.Sleep(300);
                                WriteLog(RiJiEnum.TDLiuCheng, $"出站结束");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        WriteLog(RiJiEnum.TDLiuChengRed, $"出站{ex}");
                    }
                }
                Thread.Sleep(10);
            }
        }


        private void ChuZhanJiaoYan(List<string> sjdu)
        {
            {
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                ChuFaEvent(EventType.ICTKaiShi, modess);
            }
          
            int count = sjdu.Count;
            WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:出站开始上传数据:{ChangYong.FenGeDaBao(sjdu," ")}");
            bool zongjieguodd = true;
            for (int c = 0; c < count - 1; c++)
            {
                bool zongjieguo = false;
                string ma = "";
                List<YeWuDataModel> lise = GetShuJu(sjdu[c],out zongjieguo,out ma);
                for (int i = 0; i < lise.Count; i++)
                {
                    if (lise[i].IsShangChuan == 1)
                    {
                       
                        lise[i].IsShangChuanHeGe = BuZhouShuJuMes(lise[i], ma);                     
                        {
                            //JieMianShiJianModel datamodel = new JieMianShiJianModel();
                            //datamodel.GWID = SheBeiID;
                            //datamodel.ErWeiMa = ma;
                            //datamodel.ZhanChuZhanDataModel.YeWuDataModel = lise[i];
                            //ChuFaEvent(EventType.ICTShuJu, datamodel);
                        }
                        if (lise[i].IsShangChuanHeGe==false|| zongjieguo==false)
                        {
                            zongjieguodd = false;
                        }
                    }
                }
                {
                    bool shangchuan = ChuZhanMes(ma, zongjieguo, "");
                    JieMianShiJianModel datamodel = new JieMianShiJianModel();
                    datamodel.GWID = SheBeiID;
                    datamodel.ErWeiMa = ma;
                    datamodel.JinChuZhanJieGuo = shangchuan && zongjieguo;
                    string jieguo = datamodel.JinChuZhanJieGuo? "上传成功": "上传失败";
                    datamodel.MiaoSu = $"{ma} {jieguo}";
                    ChuFaEvent(EventType.ICTMaJieGuo, datamodel);
                    if (shangchuan==false)
                    {
                        zongjieguodd = false;
                    }
                }
            }
            {
                int biaozhis = zongjieguodd ? 1 : 0;
                XieShuJu(CaoZuoType.ICTChuZhanJieGuo_单, $"{sjdu[count-1]}*{biaozhis}", 0, false);
                WriteLog(RiJiEnum.TDLiuCheng, $"{SheBeiName}:出站开始写数据:{sjdu[count-1]}*{biaozhis}");
                JieMianShiJianModel modess = new JieMianShiJianModel();
                modess.GWID = SheBeiID;
                modess.JinChuZhanJieGuo = zongjieguodd;
                ChuFaEvent(EventType.ICTZongJieGuo, modess);
            }
        }

        private List<YeWuDataModel> GetShuJu(string lujing,out bool zongjieguo,out string ma)
        {
            zongjieguo = false;
            ma = "";
            List<YeWuDataModel> sss = new List<YeWuDataModel>();
            try
            {
                string[] shuju = File.ReadAllLines(lujing);
                for (int i = 0; i < shuju.Length; i++)
                {
                    if (i == 0)
                    {
                        string[] fenss = shuju[i].Split(',');
                        if (fenss.Length >= 5)
                        {
                            zongjieguo = fenss[0].ToLower().Contains("p");
                            string[]  mas = fenss[4].Split('!');
                            if (mas.Length>=4)
                            {
                                ma = $"{mas[1]},{mas[2]}";
                            }
                        }
                    }
                    else
                    {
                        if (shuju[i].Contains(","))
                        {
                            string[] fenss = shuju[i].Split(',');
                            if (fenss.Length >= 12)
                            {                              
                                YeWuDataModel shujumodel = new YeWuDataModel();
                                shujumodel.CaoZuoType = CaoZuoType.DataShangChuan;
                                shujumodel.CodeOrNo = fenss[0];
                                shujumodel.DanWei = "";
                                shujumodel.GWID = SheBeiID;
                                shujumodel.IsShangChuan = 1;
                                shujumodel.ItemName= fenss[1];
                                shujumodel.Low.JiCunValue = $"{fenss[3]}+{fenss[3]}*{fenss[5]}";
                                shujumodel.Up.JiCunValue = $"{fenss[3]}+{fenss[3]}*{fenss[4]}";
                                shujumodel.Value.JiCunValue = fenss[10];
                                if (zongjieguo)
                                {
                                    shujumodel.IsShuJuHeGe = zongjieguo;
                                }
                                else
                                {
                                    double shujus= ChangYong.TryDouble(fenss[11].Replace("%",""), 0);
                                    double shangxian = ChangYong.TryDouble(fenss[4], 0);
                                    double xiaxian = -ChangYong.TryDouble(fenss[5], 0);
                                    if (shujus >= xiaxian && shujus <= shangxian)
                                    {
                                        shujumodel.IsShuJuHeGe = true;
                                    }
                                    else
                                    {
                                        shujumodel.IsShuJuHeGe = false;
                                    }
                                }
                                sss.Add(shujumodel);
                            }
                        }
                    }
                }
            }
            catch
            {


            }
           
            return sss;
        }
    }
}
