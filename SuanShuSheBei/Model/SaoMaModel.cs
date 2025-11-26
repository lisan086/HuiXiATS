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
        /// 超时时间（ms）
        /// </summary>
        public int Time { get; set; } = 1000;
      
        /// <summary>
        ///
        /// </summary>
        public bool TX { get; set; } = true;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; }

        public List<CunModel> LisJiaoBen { get; set; } = new List<CunModel>();
    }
}
