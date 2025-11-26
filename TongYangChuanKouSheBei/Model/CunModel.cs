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
        public int SheBeiID { get; set; }

        /// <summary>
        /// 子设备的id
        /// </summary>
        public int ZiID { get; set; }

        public int ZhiLingID { get; set; }

        public string Name { get; set; } = "";
        public int JiCunDiZhi { get; set; } = 0;

        public int ChangDu { get; set; } = 2;

        public double ChengShu { get; set; } = 1;

        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.DuShuJu;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();
        /// <summary>
        /// 0表示正在写 1表示写合格 2表示写不合格
        /// </summary>
        public int XieState { get; set; } = 0;

        public string MiaoSu { get; set; } = "";

        public string QiTaCanShu { get; set; } = "";
        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.JiCunDiZhi = JiCunDiZhi;
            model.JiCunQi = JiCunQi.FuZhi();
            model.ChangDu = ChangDu;
            model.ZiID = ZiID;
            model.SheBeiID = SheBeiID;
            model.Name = Name;
            model.ChengShu = ChengShu;         
            model.XieState = XieState; 
            model.MiaoSu = MiaoSu;
            model.ZhiLingID = ZhiLingID;
            model.QiTaCanShu = QiTaCanShu;
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
        DuShuJu,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("写数据")]
        XieShuJu,
        /// <summary>
        /// 读寄存器
        /// </summary>
        [Description("读写一起")]
        DuXieYiQi,
    }
}
