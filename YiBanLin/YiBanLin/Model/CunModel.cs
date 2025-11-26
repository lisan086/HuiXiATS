using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;

namespace YiBanLin.Model
{/// <summary>
 /// 存数据model
 /// </summary>
    public class CunModel
    {
        /// <summary>
        /// 总设备ID
        /// </summary>
        public int ZongSheBeiId { get; set; }

        /// <summary>
        /// 是否正在测量(0=测量中,1=测量成功,2=测量失败,3=测量失败)
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;

        /// <summary>
        /// 操作类型
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XieFanHuiLin0;

        /// <summary>
        /// 寄存器模型
        /// </summary>
        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();


        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.IsZhengZaiCe = IsZhengZaiCe;
            model.JiCunQi = JiCunQi.FuZhi();
            model.ZongSheBeiId = ZongSheBeiId;
            return model;
        }
    }

    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum CunType
    {

        /// <summary>
        /// 写通道lin0
        /// </summary>
        [Description("写Lin0,有返回 参数:xieid,shuju(00 01),recid id为16进制数 recid为-1表示任意返回")]
        XieFanHuiLin0,
        /// <summary>
        /// 写通道lin1
        /// </summary>
        [Description("写Lin1,有返回 参数:xieid,shuju(00 01),recid id为16进制数 recid为-1表示任意返回")]
        XieFanHuiLin1,
        /// <summary>
        ///开lin
        /// </summary>
        [Description("开Lin,无参数")]
        XieKaiLin,
        /// <summary>
        /// 关lin
        /// </summary>
        [Description("关Lin,无参数")]
        XieGuanLin,


    }
}
