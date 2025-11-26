using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public bool IsXinXieYi { get; set; } = false;
        /// <summary>
        /// 总设备Model
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;

        public int JieShouCount { get; set; } = 0;
           
        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XieSuiPian;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model= new CunModel();
            model.IsDu = IsDu; 
            model.Time = Time;
            model.IsZhengZaiCe = IsZhengZaiCe; 
            model.JiCunQi = JiCunQi; 
            model.ZongSheBeiId = ZongSheBeiId;
            model.JieShouCount = JieShouCount;
            model.JiCunQi = JiCunQi.FuZhi();
            model.IsXinXieYi = IsXinXieYi;
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
        [Description("erweima#huanxingtype#gongweihao#jieshoucount")]
        XieCheLianHuanXing,
        /// <summary>
        /// 寄存器关 
        /// </summary>
        [Description("erweima#ceshimingceng#xuhao#gongweihao#jieshoucount")]
        XieCheLianXunXu,


        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("josn文件")]
        XieSuiPian,
    }


    public class JiuHuanXingModel
    {
        // {"方法名":2,"参数":{"序列号":"N72--CG-250924000695","型号":"N72 12.3","工位号":0,"测试序号":0}}
        public int 方法名 { get; set; } = 2;

        public HuanXingModel 参数 { get; set; } = new HuanXingModel();
    }

    public class HuanXingModel
    {
        public string 序列号 { get; set; } = "";
        public string 型号 { get; set; } = "";
        public int 工位号 { get; set; } = 0;
        public int 测试序号 { get; set; } = 0;
    }
    public class XieShiJueModel
    {
        /// <summary>
        /// 1是检测 2是换型
        /// </summary>
        public int function { get; set; } = 1;

        /// <summary>
        /// 位置
        /// </summary>
        public XiangXiModel position { get; set; } = new XiangXiModel();
    }

    public class XiangXiModel
    {
        public string sn { get; set; } = "";
        public string id { get; set; } = "";
        public int flowname { get; set; } = 0;
        public int testsn { get; set; } =0;
    }
}
