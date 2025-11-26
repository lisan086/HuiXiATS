using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;

namespace ModBuTCP.Model
{
    public  class SheBeiModel
    {
        public string IpOrCom { get; set; } = "192.168.10.105";
        public int Port { get; set; } = 502;
        public int SheBeiID { get; set; } = 1;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string SheBeiName { get; set; } ="";

        public bool Tx { get; set; } = false;

        public int XieYanShi { get; set; } = 20;

        public int DuYanShi { get; set; } = 500;

        /// <summary>
        /// 寄存器数据
        /// </summary>
        public List<DataCunModel> DataCunModels { get; set; } = new List<DataCunModel>();

     
    }
    public class DataCunModel
    {
        /// <summary>
        /// 这个要唯一名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 寄存地址
        /// </summary>
        public int JiCunDiZhi { get; set; } = 1;
      
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 1;
        /// <summary>
        /// 小数位
        /// </summary>
        public int XiaoShuWei { get; set; } = 2;

        /// <summary>
        /// 被除数
        /// </summary>
        public double BeiChuShu { get; set; } = -1;

        public int TeShuGongNengMa { get; set; } = 2;

        /// <summary>
        /// 设备地址,不需要配置
        /// </summary>
        public int SheBeiDiZhi { get; set; } = 1;


        public DataType DataType { get; set; } = DataType.Int;

        public YingYongType YingYongType { get; set; } = YingYongType.DuPuTong;

        public int ZiDiZhi { get; set; } = -1;


        /// <summary>
        /// 0表示正在写 1表示写成功 2表示写失败
        /// </summary>
        public int IsXieWan { get; set; } = 0;

        /// <summary>
        /// 设备ID,不需要配置
        /// </summary>
        public int SheBeiID { get; set; } = 1;

        public float BZhi { get; set; } = 0;

        /// <summary>
        /// 1表示回零
        /// </summary>
        public int IsHuiLing { get; set; } = 0;

        public int HuiLingYanShi { get; set; } = 500;
        public string HuiLingZhi { get; set; } = "";

        public JiCunQiModel JiCunQiModel { get; set; } = null;

        public DataCunModel FuZhi()
        {
            DataCunModel model = new DataCunModel();
            model.BeiChuShu =BeiChuShu;
            model.BZhi = BZhi;
            model.Count = Count;
            model.DataType = DataType;
            model.HuiLingYanShi = HuiLingYanShi;
            model.HuiLingZhi = HuiLingZhi;
            model.IsHuiLing = IsHuiLing;
            model.IsXieWan = IsXieWan;
            model.JiCunDiZhi = JiCunDiZhi;
            model.JiCunQiModel = JiCunQiModel.FuZhi();
            model.Name = Name;
            model.SheBeiDiZhi = SheBeiDiZhi;
            model.SheBeiID = SheBeiID;
            model.TeShuGongNengMa  = TeShuGongNengMa;
            model.XiaoShuWei = XiaoShuWei;
            model.YingYongType = YingYongType;
            model.ZiDiZhi = ZiDiZhi;
            return model;
        }
        public byte[] GetZhi(object objvalue)
        {
            switch(DataType)
            {
                case DataType.Int:
                    {
                        double value = 0;
                        if (double.TryParse(objvalue.ToString(), out value))
                        {
                            double zhi = value;
                            if (BeiChuShu == 0)
                            {
                                BeiChuShu = 1;

                            }
                            if (XiaoShuWei < 0)
                            {
                                XiaoShuWei = 0;

                            }
                            if (XiaoShuWei >= 15)
                            {
                                XiaoShuWei = 14;
                            }
                            double xinzhi = (zhi - BZhi) * BeiChuShu;
                            if (Count == 1)
                            {
                                short zuixin = (short)Math.Round(xinzhi, XiaoShuWei);
                                byte[] shuju2 = BitConverter.GetBytes(zuixin);
                                return shuju2;
                            }
                            else if (Count == 2)
                            {
                                int zuixin = (int)Math.Round(xinzhi, XiaoShuWei);
                                byte[] shuju2 = BitConverter.GetBytes(zuixin);
                                return shuju2;
                            }
                            else if (Count==4)
                            {
                                long zuixin = (long)Math.Round(xinzhi, XiaoShuWei);
                                byte[] shuju2 = BitConverter.GetBytes(zuixin);
                                return shuju2;
                            }
                          
                        }
                    }
                    break;
                case DataType.Float:
                    {
                        double value = 0;
                        if (double.TryParse(objvalue.ToString(), out value))
                        {
                            double zhi = value;
                            if (BeiChuShu == 0)
                            {
                                BeiChuShu = 1;

                            }
                            if (XiaoShuWei < 0)
                            {
                                XiaoShuWei = 0;

                            }
                            if (XiaoShuWei >= 15)
                            {
                                XiaoShuWei = 14;
                            }
                            double xinzhi = (zhi - BZhi) * BeiChuShu;
                          
                            if (Count == 2)
                            {
                                float zuixin = (float)Math.Round(xinzhi, XiaoShuWei);
                                byte[] shuju2 = BitConverter.GetBytes(zuixin);
                               
                               
                                return shuju2;
                            }
                            else if (Count == 4)
                            {
                                double zuixin = Math.Round(xinzhi, XiaoShuWei);
                                byte[] shuju2 = BitConverter.GetBytes(zuixin);
                               
                                return shuju2;
                            }

                        }
                    }
                    break;
                case DataType.String:
                    {
                        byte[] shuju = Encoding.ASCII.GetBytes(objvalue.ToString());
                        return shuju;
                    }
                case DataType.String16OrACII:
                    {
                        byte[] shuju = Encoding.ASCII.GetBytes(objvalue.ToString());
                        return shuju;
                    }       
                default:
                    break;
            }

            return null;
        }
    }


    public  class JiLuModel
    {
        public int SheBeiID { get; set; }

        /// <summary>
        /// 存数据
        /// </summary>
        public List<byte> ShuJu { get; set; } = new List<byte>();
        /// <summary>
        /// 最小寄存器偏移
        /// </summary>
        public int JiCunQiZuiXiaoPianYi { get; set; } = 0;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 0;

        public List<DataCunModel> Lis = new List<DataCunModel>();
     
    }
    public enum DataType
    {
        Int,
        Float,
        String,
        String16OrACII,
    }

    public enum YingYongType
    { 
        DuPuTong,
        XiePuTong,
        DuXieYiQi,
    }
}
