using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace DBPLCS7.Model
{

    /// <summary>
    /// 记录最小寄存器的偏移数量
    /// </summary>
    public class JiLvDaXiaoModel
    {
        public DataType DataType { get; set; } = DataType.DataBlock;
        public int DBKuai { get; set; }

        public bool IsXianShi { get; set; } = true;

        /// <summary>
        /// 存数据
        /// </summary>
        public List<byte> ShuJu { get; set; } = new List<byte>();
        /// <summary>
        /// 最小寄存器偏移
        /// </summary>
        public int JiCunQiZuiXiaoPianYi { get; set; } = 0;

        /// <summary>
        /// 偏移的数量
        /// </summary>
        public int JiCunQiDuShuLiang { get; set; } = 0;

        /// <summary>
        /// 判定的寄存器
        /// </summary>
        public List<PLCJiCunQiModel> BangDianJiCunQi { get; set; } = new List<PLCJiCunQiModel>();
    }
}
