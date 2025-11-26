using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    public  class SheBeiZhanModel
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string LineCode { get; set; } = "AS1";
        /// <summary>
        ///工位ID
        /// </summary>
        public int GWID { get; set; } = 1;

        /// <summary>
        /// true 表示上传mes
        /// </summary>
        public bool IsMes { get; set; } = true;

        /// <summary>
        /// 其他参数
        /// </summary>
        public string QiTaCanShu { get; set; } = "";


        /// <summary>
        /// 1表示全部上传 其他表示遇到不合格就不上传
        /// </summary>
        public int IsQuanBuShangChuan { get; set; } = 1;

        /// <summary>
        /// 设备类型
        /// </summary>
        public SheBeiType IsZhengZhanDian { get; set; } = SheBeiType.GongWeiZhan;

        /// <summary>
        /// 型号
        /// </summary>
        public string XingHao { get; set; } = "";

        public string MaName { get; set; } = "";

        public string BangDingMa { get; set; } = "";

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
        [Description("Zhan")]
        GongWeiZhan,
        [Description("ShouZhan")]
        ShouZhan,
        [Description("FoZhanSaoMa")]
        FoZhanSaoMaZhan,
        [Description("LeiDiao")]
        FoZhanLeiDiao,
        [Description("ICT")]
        ICTChuanShuJu,
        [Description("AGVChu")]
        AGVChuKou,
        [Description("AGVJin")]
        AGVJinKou,
        [Description("ZX")]
        ZhuangXiang,
    }
}
