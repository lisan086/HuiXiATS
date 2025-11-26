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
        /// 0表示进行中 1表示成功 2表示失败 3也是失败
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;


        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XieCANYouZhiRec;

        public int ChaoTime { get; set; } = 1000;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.IsZhengZaiCe = IsZhengZaiCe;
            model.ZongSheBeiId = ZongSheBeiId;
            model.ChaoTime= ChaoTime;
            model.JiCunQi = JiCunQi.FuZhi();
            return model;
        }
    }


    /// <summary>
    /// 存的类型
    /// </summary>
    public enum CunType
    {
       
        [Description("写can有返回的 xieid,shuju(00 01),recid")]
        XieCANYouZhiRec,
        [Description("写can 用于只读 recid")]
        XieCANZhiDu,
        [Description("写can 不需要返回 xieid,shuju(00 01)")]
        XieCANWuZhiRec,
        [Description("连接can 不需要参数")]
        XieLianJieCAN,
        [Description("关闭can 不需要参数")]
        XieGuanCAN,
    }
}
