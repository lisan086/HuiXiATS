using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KuHuDuanDoIP.Frm;
using SSheBei.Model;

namespace KuHuDuanDoIP.Model
{
    /// <summary>
    /// 指令参数
    /// </summary>
    public class ZhiLingModel
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
        /// 0表示正在写 1表示写完 2表示写失败 3表示超时失败
        /// </summary>
        public int IsXieWan { get; set; } = 0;
  
        /// <summary>
        /// 指令类型
        /// </summary>
        public ZhiLingType ZhiLingType { get; set; } = ZhiLingType.XieDoipYouFanHui;

        public JiCunQiModel JiCunQiModel { get; set; } = new JiCunQiModel();

        public ZhiLingModel FuZhi()
        {
            ZhiLingModel zhiLingModel = new ZhiLingModel();
            zhiLingModel.FuZaiLeiXing = FuZaiLeiXing;
            zhiLingModel.IP = IP;  
            zhiLingModel.IsXieWan = IsXieWan;
            zhiLingModel.JiCunQiModel = JiCunQiModel.FuZhi();
            zhiLingModel.TaDiZhi = TaDiZhi;
            zhiLingModel.ZhiLingShuJu = ZhiLingShuJu;
            zhiLingModel.ZhiLingType = ZhiLingType;
            return zhiLingModel;
        }
        public void SetCanShu(string canshu)
        {
            string[] data = canshu.Split('#');
            if (canshu.Length>4)
            {
                ZhiLingName=data[0];
                FuZaiLeiXing = data[1];
                TaDiZhi = data[2];
                ZhiLingShuJu= data[3];
                IP= data[4];
            }
        }
    }
    public enum ZhiLingType
    {
        /// <summary>
        /// 写不需要解析数据的
        /// </summary>
        [Description("写数据又返回的,参数为:ZhiLingName#FuZaiLeiXing#TaDiZhi#ZhiLingShuJu#IP")]
        XieDoipYouFanHui,
        /// <summary>
        /// 写打开TCP链接
        /// </summary>
        [Description("打开连接")]
        XieDoipOpenTCP,
        /// <summary>
        /// 关闭TCP链接
        /// </summary>
        [Description("关闭连接")]
        XieDoipGuanBiTCP,
        /// <summary>
        /// 关闭TCP链接
        /// </summary>
        [Description("获取子数据 参数:")]
        XieDoipGetZiData,
     
    
        /// <summary>
        /// 写秘钥
        /// </summary>
        [Description("写秘钥:")]
        XieDoipMiYao,
    }

    public enum QuZhiType
    { 
        QC
    }
}
