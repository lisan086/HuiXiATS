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
        /// 子设备Model
        /// </summary>
        public int ZiSheBeiID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public int JiCunDiZhi { get; set; }

        /// <summary>
        /// 地址号
        /// </summary>
        public int ZDiZhi { get; set; } = 1;

        public int JiLu { get; set; }

        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.DuJiCunQi;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public double ChengShu { get; set; } = 1;
        public int QiShiDiZhi { get; set; } = 1;
        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.JiCunDiZhi = JiCunDiZhi;
            model.JiCunQi = JiCunQi.FuZhi();
            model.JiLu = JiLu;
            model.ZDiZhi = ZDiZhi;
            model.ZiSheBeiID = ZiSheBeiID;
            model.ZongSheBeiId = ZongSheBeiId;
            model.ChengShu = ChengShu;
            model.QiShiDiZhi = QiShiDiZhi;
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
        [Description("读数据")]
        DuJiCunQi,     
    }
}
