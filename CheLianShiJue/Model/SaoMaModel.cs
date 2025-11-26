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
        public int SheBeiID { get; set; }


        public int JieShouCount { get; set; } = 1;
        public bool IsXinXieYi { get; set; } = false;
    }
}
