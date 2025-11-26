using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public int Time { get; set; } = 1000;

        /// <summary>
        /// 总设备Model
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;

        public string JiaoBenName { get; set; } = "";

        public string JiaoBenNeiRong { get; set; } = "";

        public string MiaoSu { get; set; } = "";

        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.ABSJia;

        public JiCunQiModel JiCunQi { get; set; } = null;

        public CunModel FuZhi()
        {
            CunModel model= new CunModel();
            model.IsDu = IsDu; 
            model.Time = Time;
            model.IsZhengZaiCe = IsZhengZaiCe; 
            model.JiCunQi = JiCunQi; 
            model.ZongSheBeiId = ZongSheBeiId; 
            model.JiaoBenName = JiaoBenName;
            model.MiaoSu = MiaoSu;
            return model;
        }
    }


    /// <summary>
    /// 存的类型
    /// </summary>
    public enum CunType
    {
        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("两个数相加取绝对值，参数:a#b")]
        ABSJia,
        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("两个数相减取绝对值，参数:a#b")]
        ABSJian,
        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("用于播放音乐，参数:播放音乐路径")]
        BoFangYingYue,       
        [Description("截取字符串数据的长度，参数:a#kaishiwei#count")]
        JieQuStrShuJuChang,
        [Description("模拟绘图的")]
        MoLiTuPian,
        [Description("写脚本的")]
        XieJiaoBen,
        DuShuJu,
    }
}
