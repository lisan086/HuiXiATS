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
        public string Ip{ get; set; } = "COM99";
       

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

        public string WangKaName { get; set; } = "";


        public List<int> GuanLianSBID { get; set; } = new List<int>();
 
    }
}
