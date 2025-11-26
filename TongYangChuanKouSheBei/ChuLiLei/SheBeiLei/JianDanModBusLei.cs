using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using ZhongWangSheBei.Model;

namespace XiangTongChuanKouSheBei.ChuLiLei.SheBeiLei
{
    internal class JianDanModBusLei : ABSZiSheBeiLei
    {
        private SerialDataGuDingJieXi SerialDataGuDingJieXi = new SerialDataGuDingJieXi();
        public override FanHuiModel GetSendCMD(CunModel rumodel)
        {
            FanHuiModel chumodel = new FanHuiModel();
            chumodel.XieTime = ZiSheBeiModel.XieRuTime;
            if (string.IsNullOrEmpty(rumodel.QiTaCanShu))
            {
                List<byte> xieshuju = new List<byte>();
                xieshuju.Add((byte)ZiSheBeiModel.DiZhi);
                xieshuju.Add(0x06);
                int dizhi = rumodel.JiCunDiZhi;
                byte[] dishi = GetBtyez(dizhi);
                xieshuju.AddRange(dishi);
                int xinzhi = GetSendData(rumodel.JiCunQi.Value, rumodel.ChengShu);
                byte[] dishsi = GetShiJianZhi(xinzhi, true).ToArray(); 
                xieshuju.AddRange(dishsi);
                byte[] shu = CRC.ToModbus(xieshuju, false);
                xieshuju.AddRange(shu);           
            }
         
            return chumodel;
        }

        public override void JieShouShuJu(byte[] shuju,int datachangdu)
        {
            SerialDataGuDingJieXi.AddByteList(shuju);
        }

        public override int JiaYanShuJu(DuZhiLingModel model)
        {
            SerialDataGuDingJieXi.JieXiWanMeiData(model.ShuJuChangDu + 5, new byte[] { (byte)ZiSheBeiModel.DiZhi, 0x03, (byte)(model.ShuJuChangDu) }, true, JiaoYanWenDu, false);
            if (SerialDataGuDingJieXi.DataCount > 0)
            {

                byte[] xindata = SerialDataGuDingJieXi.GetShiShiData();

                if (xindata != null && xindata.Length >= model.ShuJuChangDu + 3)
                {
                    List<byte> shujus = new List<byte>();
                    for (int i = 3; i < xindata.Length - 2; i++)
                    {
                        shujus.Add(xindata[i]);
                    }
                   SetJiCunQiDuValue(model.ZhiLingID,model.QiShiDiZhi, shujus);
                   
                }
                return 1;
            }

            return 2;
        }
      
        public override void ClearData()
        {
            SerialDataGuDingJieXi.Clear();
        }
        private void SetJiCunQiDuValue(int zhilingid,int qishidizhi, List<byte> shuju)
        {
            for (int i = 0; i < ZiSheBeiModel.LisJiCunQi.Count; i++)
            {
                CunModel model = ZiSheBeiModel.LisJiCunQi[i];
                if (model.ZhiLingID== zhilingid)
                {
                    if (model.IsDu.ToString().ToLower().StartsWith("du") )
                    {
                        model.JiCunQi.IsKeKao = true;
                        model.XieState = 1;
                        List<byte> shijidu = new List<byte>();
                        int weizhi = (model.JiCunDiZhi - qishidizhi)*2;
                        for (int c = weizhi; c < weizhi + model.ChangDu; c++)
                        {
                            if (c < shuju.Count)
                            {
                                shijidu.Add(shuju[c]);
                            }
                        }
                        double jieguozhi = GetValue(shijidu)* model.ChengShu;
                        model.JiCunQi.Value = jieguozhi;
                    }
                   
                }
            }
          
        }


        private bool JiaoYanWenDu(List<byte> canshu)
        {
            if (canshu == null || canshu.Count <= 1)
            {
                return false;
            }
            List<byte> data = canshu.GetRange(0, canshu.Count - 2);
            byte[] shuju = CRC.ToModbus(data, false);
            if (shuju != null && shuju.Length >= 2)
            {
                if (shuju[0] == canshu[canshu.Count - 2] && shuju[1] == canshu[canshu.Count - 1])
                {
                    return true;
                }
            }
            return true;
        }
        private byte[] GetBtyez(int value)
        {
            int hValue = (value >> 8) & 0xFF;

            int lValue = value & 0xFF;

            byte[] arr = new byte[] { (byte)hValue, (byte)lValue };
            return arr;
        }
        private double GetValue(List<byte> canshu)
        {
            if (canshu.Count == 2)
            {
                double sdvlaue = BitConverter.ToInt16(JiaoHuanByte(canshu.ToList()).ToArray(), 0);

                return sdvlaue;
            }
            else
            {
                double sdvlaue = BitConverter.ToInt32(JiaoHuanByte(canshu.ToList()).ToArray(), 0);

                return sdvlaue;
            }
        }
        private List<byte> JiaoHuanByte(List<byte> geshu)
        {
            List<byte> shuju = new List<byte>();

            for (int i = 0; i < geshu.Count; i += 2)
            {
                if (geshu.Count > i + 1)
                {
                    shuju.Add(geshu[i + 1]);
                    shuju.Add(geshu[i]);
                }
                else
                {
                    if (geshu.Count > i)
                    {
                        shuju.Add(geshu[i]);
                    }
                   
                }
            }
            return shuju;
        }
        private List<byte> GetShiJianZhi(int len, bool isgaoweizaiqian)
        {
            List<byte> shuju = new List<byte>();
            byte[] intBuff = BitConverter.GetBytes(len);
            if (isgaoweizaiqian)
            {
                if (intBuff.Length >= 2)
                {
                    shuju.Add(intBuff[1]);
                    shuju.Add(intBuff[0]);
                }
            }
            else
            {
                if (intBuff.Length >= 2)
                {
                    shuju.Add(intBuff[0]);
                    shuju.Add(intBuff[1]);
                }
            }
            return shuju;
        }
        private int GetSendData(object zhi, double xishu)
        {
            double zhenzhi = ChangYong.TryDouble(zhi, 0);

            double xinzhi = zhenzhi / xishu;

            return (int)xinzhi;
        }

     
    }
}
