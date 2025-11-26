using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiBanSaoMaQi.Model
{
    /// <summary>
    /// 扫码model
    /// </summary>
    public class SaoMaModel
    {
        /// <summary>
        /// 结束指令
        /// </summary>
        public string JieGuoSaoMaZhiLing { get; set; } = "";
        /// <summary>
        /// 开始指令
        /// </summary>
        public string KaiShiSaoMaZhiLing { get; set; }="";
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
        /// 解析格式
        /// </summary>
        public int JieXiType { get; set; } = 1;
        /// <summary>
        /// 总设备Model
        /// </summary>
        public int FaGeShi { get; set; }
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
        public int SheBeiID { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public string ChangDu { get; set; } = "";

        /// <summary>
        /// 1是长度 2是采用包含
        /// </summary>
        public JieXieDataType JieXieDataType { get; set; } = JieXieDataType.ChangDu;
    }

    public enum JieXieDataType
    { 
        ChangDu,
        FenGe,
        BaoHan,
    }
}
