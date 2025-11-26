using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    public class JieMianShiJianModel
    {
        public int GWID { get; set; } = -1;
        public string MiaoSu { get; set; } = "";
        public string ErWeiMa { get; set; } = "";
        public bool JinChuZhanJieGuo { get; set; } = true;
      
        public ZhanChuZhanDataModel ZhanChuZhanDataModel { get; set; } = new ZhanChuZhanDataModel();
    }

    /// <summary>
    /// 事件的类型
    /// </summary>
    public enum EventType
    { 
     
        /// <summary>
        /// 开始进站
        /// </summary>
        ZhanKaiShiJinZhan,
        /// <summary>
        /// 进站结果
        /// </summary>
        ZhanJinZhanJieGuo,      
        /// <summary>
        /// 开始出站
        /// </summary>
        ZhanKaiShiChuZhan,
        /// <summary>
        /// 出站数据
        /// </summary>
        ZhanChuZhanData,
      
        /// <summary>
        /// 出站结果
        /// </summary>
        ZhanChuZhanJieGuo,
        /// <summary>
        /// 佛照扫码开始
        /// </summary>
        FoZhanSaoMaKaiShi,
        FoZhanDuQuMa,
        FoZhanJieSu,
        LeiDiaoJinZhan,
        LeiDiaoShuJu,
        LeiDiaoChuZhan,
        ICTKaiShi,
        ICTShuJu,
        ICTMaJieGuo,
        ICTZongJieGuo,
        AGVJinZhanKaiShi,
        AGVJinZhanJieGuo,
        AGVChuZhanKaiShi,
        AGVChuZhanJieGuo,
        ZXJinZhanKaiShi,
        ZXJinZhanJieSu,
        ZXChuZhanKaiShi,
        ZXChuZhanJieSu,
    }

  

    public class ZhanChuZhanDataModel
    {
        public YeWuDataModel YeWuDataModel { get; set; } = null;
    }
}
