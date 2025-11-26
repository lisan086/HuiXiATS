using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YiBanSaoMaQi.Model
{
    /// <summary>
    /// 扫码model
    /// </summary>
    public class SaoMaModel
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
        /// 超时时间（ms）
        /// </summary>
        public int Time { get; set; } = 1000;

        /// <summary>
        ///
        /// </summary>
        public bool TX { get; set; } = false;



        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; } = 1;

        public List<ZhiLingModel> LisZhiLing { get; set; } = new List<ZhiLingModel>();
    }

    public class ZhiLingModel
    {

        public string MingCheng { get; set; } = "";
        public int ZhiLingID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ZhiLing { get; set; } = "";

        public ZhiLingType ZhiLingType { get; set; } = ZhiLingType.TZBFTongDao;

        /// <summary>
        /// 名称
        /// </summary>
        public string ZhiLingJieSu { get; set; } = "";

        public List<DataSdModel> LisData { get; set; } = new List<DataSdModel>();

    
    }

    public class DataSdModel
    {
        public int TongDao { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 名称
        /// </summary>
        public string CanShu { get; set; } = "";
    }
    public enum ZhiLingType
    {
        BFTongDao,
        TZBFTongDao,
        KaiCai,
        CJJieGuo,
        ZiJieGuo,
        QiTa,
    }

    public enum QiTaLeiType
    {
        GW,
        XinZaoBi,
        ZuoFenLiDu,
        YouFenLiDu,
    }
}
