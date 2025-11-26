using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace YiBanSaoMaQi.Model
{
    /// <summary>
    /// 存数据model
    /// </summary>
    public class CunModel
    {
        /// <summary>
        /// 总设备Model
        /// </summary>
        public int ZongSheBeiId { get; set; }

        /// <summary>
        /// 总设备Model
        /// </summary>
        public int ZhiLingId { get; set; }

        public int ShuYuTongDaoID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";


        /// <summary>
        /// 名称
        /// </summary>
        public string ZhiLing { get; set; } = "";

        /// <summary>
        /// 名称
        /// </summary>
        public string ZhiLingJieSu { get; set; } = "";
    

        /// <summary>
        /// 0表示进行中 1表示完成 2表达失败
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;

        public ZhiLingType ZhiLingType { get; set; } = ZhiLingType.TZBFTongDao;


        /// <summary>
        /// true  表示读
        /// </summary>
        public bool IsData { get; set; } =false;

        public JiCunQiModel JiCunQi { get; set; } = null;

        public int Time { get; set; } = 0;
    }


}
