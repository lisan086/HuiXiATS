using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using ATSJuanChengZuZhuangUI.Model;

namespace ZuZhuangUI.Model
{
    /// <summary>
    /// 码管理
    /// </summary>
    public class MaGuanLiModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string MaName { get; set; } = "";

        /// <summary>
        /// 规则
        /// </summary>
        public List<MaGuiZeModel> LisGuiZe { get; set; } = new List<MaGuiZeModel>();


        public int Count { get; set; }=1;



    
    }

    public class MaGuiZeModel
    {
        /// <summary>
        /// 值
        /// </summary>
        public string Zhi { get; set; } = "";
        /// <summary>
        /// 值
        /// </summary>
        public MaaType MaaType { get; set; } = MaaType.GuDing;

        /// <summary>
        /// 长度
        /// </summary>
        public int ChangDu { get; set; } = 10;

        public int JilvWeiZhi { get; set; } = 0;
    }
    public enum MaaType
    { 
        RiQiNianYueRi,
        LiuShuiHao,
        GuDing,
    }


    public class QingKongJiLvModel
    {
        public int Tian { get; set; } = 1;
    }

   
}
