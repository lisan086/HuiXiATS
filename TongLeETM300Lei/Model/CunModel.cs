using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace ZhongWangSheBei.Model
{
    /// <summary>
    /// 存数据model
    /// </summary>
    public  class CunModel
    {
        /// <summary>
        /// 总设备Model
        /// </summary>
        public int ZongSheBeiId { get; set; }


        /// <summary>
        /// 地址号
        /// </summary>
        public int ZDiZhi { get; set; } = 1;

        /// <summary>
        /// 地址
        /// </summary>
        public int JiCunDiZhi { get; set; }

        public int ChangDu { get; set; } = 2;

        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.DuDianYuanState;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public double DianLiuChengShu { get; set; } = 0.01;

        public double DianYaChengShu { get; set; } = 0.001;

        public double GongLuChengShu { get; set; } = 0.001;
        public int QiShiDiZhi { get; set; } = 16;
        /// <summary>
        /// 0表示正在写 1表示写合格 2表示写不合格
        /// </summary>
        public int XieState { get; set; } = 0;

        public int PianYiLiang { get; set; } = 0;
        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.JiCunDiZhi = JiCunDiZhi;
            model.JiCunQi = JiCunQi.FuZhi();
            model.ChangDu = ChangDu;
            model.ZDiZhi = ZDiZhi;
            model.GongLuChengShu = GongLuChengShu;
            model.ZongSheBeiId = ZongSheBeiId;
            model.DianLiuChengShu = DianLiuChengShu;
            model.DianYaChengShu = DianYaChengShu;
            model.XieState = XieState;
            model.QiShiDiZhi = QiShiDiZhi;
            model.PianYiLiang = PianYiLiang;
            return model;
        }
    }


    /// <summary>
    /// 存的类型
    /// </summary>
    public enum CunType
    {
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读电源状态")]
        DuDianYuanState,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读实时电流")]
        DuShiShiDianLiu,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读实时电压")]
        DuShiShiDianYa,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读实时功率")]
        DuShiShiGongLu,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读设置电压")]
        DuSetDianYa,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读设置电流")]
        DuSetDianLiu,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("写设置电流 参数为:double数据")]
        XieSetDianLiu,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("写设置电压 参数为:double数据")]
        XieSetDianYa,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("写开，无参数")]
        XieDianYuanON,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("写关，无参数")]
        XieDianYuanOFF,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("写配置开，无参数")]
        XieSetDianYuanON,
    }
}
