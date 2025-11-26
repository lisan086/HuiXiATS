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
        /// 0-进行中 1-成功 2-失败 3-失败
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;


        /// <summary>
        /// true  表示读
        /// </summary>
        public CunType IsDu { get; set; } = CunType.XieZlgCanDanGeXieRec;

        public int ChaoTime { get; set; } = 1000;

        public JiCunQiModel JiCunQi { get; set; } = new JiCunQiModel();

        public CunModel FuZhi()
        {
            CunModel model = new CunModel();
            model.IsDu = IsDu;
            model.IsZhengZaiCe = IsZhengZaiCe;
            model.ZongSheBeiId = ZongSheBeiId;
            model.ChaoTime = ChaoTime;
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
        [Description("描述:[写一般单个指令Can能返回,返回的数据是huiid,zhiling]\r\n 参数:[(xieid,zhiling,index,huiid,isFD),xieid:表示写入的ID 如0x30 zhiling:表示写入指令 如 00 05等 index:用的那个通道 huiid:表示返回的id 如0x25,-1表示任意返回ID isFD:0表示can指令 1表示CANFD指令]")]
        XieZlgCanDanGeXieRec,
        [Description("描述:[用于不写去读CAN,返回的数据是huiid,zhiling|huiid,zhiling]\r\n 参数:[(index,huiid*huiid*huiid...,count)] ,index:用的那个通道 huiid:表示返回的id 如0x25]")]
        XieZlgBuXieZhiDu,
        [Description("描述:[写一般多个指令Can无返回,返回的数据是OK]\r\n 参数:[(xieid,zhiling,index,isFD*xieid,zhiling,index,isFD..#jici#yanshi),xieid:表示写入的ID 如0x30 zhiling:表示写入指令 如 00 05等 index:用的那个通道 isFD:0表示can指令 1表示CANFD指令]")]
        XieZlgCanDuoGeWuRec,
        [Description("[index] 1表示通道1一直写 0表示通道0一直写 -2表示不写")]
        XieZlgYiZhiXie,   
        [Description("[index] 初始化那个通道 关闭CAN")]
        XieZlgCanGuan,
        [Description("[index] 初始化那个通道与启动CAN")]
        XieZlgCanDaiKai,
        [Description("描述:[写一般单个指令Can能返回,返回的数据是Lsit<byte[]>]\r\n 参数:[(xieid,zhiling,index,isFD)* #fajichi#yanshi#index#huiid],xieid:表示写入的ID 如0x30 zhiling:表示写入指令 如 00 05等 index:用的那个通道]")]
        XieZlgCanDuByte,
    }
}
