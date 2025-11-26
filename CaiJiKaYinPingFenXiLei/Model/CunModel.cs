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
        public int IsZhengZaiCe { get; set; } = 0;
          
        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XieKaiShiCaiJi;

     

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model= new CunModel();
            model.IsDu = IsDu; 
           
            model.IsZhengZaiCe = IsZhengZaiCe; 
          
            model.ZongSheBeiId = ZongSheBeiId;
        
            model.JiCunQi = JiCunQi.FuZhi();
          
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
        [Description("写开始采集测试")]
        XieKaiShiCaiJi,
        /// <summary>
        /// 寄存器关
        /// </summary>
        [Description("写采集噪音")]
        XieCaiJiZaoYing,
        [Description("读频率")]
        DuShuJuPingLv,
        [Description("读SNR")]
        DuShuJuXinZaoBi,
        [Description("读灵敏度")]
        DuShuJuLinMinDu,
        [Description("读幅值")]
        DuShuJuFuZhi,
    }


   
}
