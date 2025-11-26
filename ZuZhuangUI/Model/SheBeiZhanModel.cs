using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    public  class SheBeiZhanModel
    {
        public string LineCode { get; set; } = "AS1";
        /// <summary>
        /// 设备编码
        /// </summary>
        public string SheBeiBianMa { get; set; } = "";

        /// <summary>
        /// 1是表示真站点
        /// </summary>
        public SheBeiType IsZhengZhanDian { get; set; } = SheBeiType.GongWeiZhan;

        /// <summary>
        ///工位ID
        /// </summary>
        public int GWID { get; set; } = 1;

        /// <summary>
        /// 1表示首站
        /// </summary>
        public int IsShouZhan { get; set; } = 0;

        public bool IsMes { get; set; } = true;

        public string QiTaCanShu { get; set; } = "";

        /// <summary>
        /// 1表示全部上传
        /// </summary>
        public int IsQuanBuShangChuan { get; set; } = 1;

        /// <summary>
        /// 每个站的数据
        /// </summary>
        public List<YeWuDataModel> LisData { get; set; } = new List<YeWuDataModel>();

        /// <summary>
        /// 每个站的请求
        /// </summary>
        public List<YeWuDataModel> LisQingQiu { get; set; } = new List<YeWuDataModel>();
    }

    public enum SheBeiType
    { 
        GongWeiZhan,

    }
}
