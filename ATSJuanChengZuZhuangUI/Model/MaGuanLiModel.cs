using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    public class MaGuanLiModel
    {
        public string MaName { get; set; } = "";
        /// <summary>
        /// 首部标识
        /// </summary>
        public string ShouBuBianShi1 { get; set; } = "";

        /// <summary>
        /// 首部标识
        /// </summary>
        public string ShouBuBianShi2 { get; set; } = "";
        /// <summary>
        /// 首部标识
        /// </summary>
        public string ShouBuBianShi3 { get; set; } = "";
        /// <summary>
        /// 首部标识
        /// </summary>
        public string ShouBuBianShi4 { get; set; } = "";

        /// <summary>
        /// 紧跟着长度
        /// </summary>
        public int MouChangDu { get; set; } = 6;
        /// <summary>
        /// 当前位置
        /// </summary>
        public int MuQianWeiZhi { get; set; } = 0;
        /// <summary>
        ///一次要用多少个
        /// </summary>
        public int YiGongYong { get; set; } = 1;

        /// <summary>
        /// true表示当前使用
        /// </summary>
        public bool DanQianShiYong { get; set; } = false;

        public string DanTian { get; set; }=DateTime.Now.ToString("yyyyMMdd");
    }

    public enum MaaType
    { 
        RiQiNianYueRi,
        LiuShuiHao,
    }
}
