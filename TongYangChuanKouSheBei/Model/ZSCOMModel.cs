using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SSheBei.Model;

namespace ZhongWangSheBei.Model
{
    public class ZSModel
    {     
        /// <summary>
        /// ip与COM口
        /// </summary>
        public string IpOrCom { get; set; } = "COM99";
    
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 38400;

        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }

        public int QieHuanTime { get; set; } = 50;

        public List<ZiSheBeiModel> LisSheBei { get; set; }=new List<ZiSheBeiModel>();

    }


    public class ZiSheBeiModel
    {
        public string Name { get; set; } = "";
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }

        /// <summary>
        /// 子设备的id
        /// </summary>
        public int ZiID { get; set; }

        /// <summary>
        /// 设备地址
        /// </summary>
        public int DiZhi { get; set; }

        /// <summary>
        /// 读超时time
        /// </summary>
        public int DuChaoShiTime { get; set; } = 300;

        /// <summary>
        /// 写入时间
        /// </summary>
        public int XieRuTime { get; set; } = 50;

        /// <summary>
        /// true 表示通信成功
        /// </summary>
        public bool Tx { get; set; } = false;

        /// <summary>
        /// 通信地址
        /// </summary>
        public int TxCiShu { get; set; } = 5;

        public SheBeiType SheBeiType { get; set; } = SheBeiType.JianDanModBusRTU;

 

        public List<DuZhiLingModel> ZhiLingS { get; set; } = new List<DuZhiLingModel>();

        public List<CunModel> LisJiCunQi { get; set; } = new List<CunModel>();
    }


    public class DuZhiLingModel
    {
        public int ZhiLingID { get; set; }
        public int QiShiDiZhi { get; set; } = 0;
        public string DuZhiLing { get; set; } = "";

        public int ShuJuChangDu { get; set; } = 10;
        public byte[] SendZhiLing { get; set; } = null;
        public override string ToString()
        {
            return $"{ZhiLingID}-{QiShiDiZhi}-{ShuJuChangDu}-{DuZhiLing}";
        }
    }

    public enum SheBeiType
    {
        /// <summary>
        /// 简单的modbusrtu协议
        /// </summary>
        JianDanModBusRTU,
        /// <summary>
        /// 色彩分析类
        /// </summary>
        CA410SeCaiFenXiLei,
    }
}
