using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuHuDuanDoIP.Model
{
    public  class JiLuZhiLingModel
    {
        /// <summary>
        /// 指令名称
        /// </summary>
        public string ZhiLingName { get; set; } = "";

        /// <summary>
        /// 用于Ta地址
        /// </summary>
        public string TaDiZhi { get; set; } = "";

        /// <summary>
        /// 指令参数
        /// </summary>
        public string ZhiLingShuJu { get; set; } = "";

        /// <summary>
        /// 负载类型
        /// </summary>
        public string FuZaiLeiXing { get; set; } = "08 01";

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; } = "";
        /// <summary>
        /// 指令类型
        /// </summary>
        public ZhiLingType ZhiLingType { get; set; } = ZhiLingType.XieDoipYouFanHui;


        public void FuZhi(ZhiLingModel model)
        {
            ZhiLingName = model.ZhiLingName;
            TaDiZhi = model.TaDiZhi;
            ZhiLingShuJu = model.ZhiLingShuJu;
            FuZaiLeiXing = model.ZhiLingShuJu;
            IP= model.IP;
            ZhiLingType = model.ZhiLingType;
        }
    }
}
