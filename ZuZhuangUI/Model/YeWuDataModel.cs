using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    public class YeWuDataModel
    {
        /// <summary>
        /// 检测项名称
        /// </summary>
        public string ItemName { get; set; } = "";
        /// <summary>
        /// 检测码与编号
        /// </summary>
        public string CodeOrNo { get; set; } = "";

        public ShuJuLisModel Low { get; set; } = new ShuJuLisModel();
        public ShuJuLisModel Up { get; set; } = new ShuJuLisModel();

        public ShuJuLisModel State { get; set; } = new ShuJuLisModel();

        public ShuJuLisModel Value { get; set; } = new ShuJuLisModel();

        /// <summary>
        /// 工位ID
        /// </summary>
        public int GWID { get; set; } = 0;

        /// <summary>
        /// 1-PLC状态为准 2-plc和上位机判断为准 3是以上位机判断为准
        /// </summary>
        public int IsYiZhuangTaiWeiZhun { get; set; } = 1;

        /// <summary>
        /// 1表示要上传
        /// </summary>
        public int IsShangChuan { get; set; } = 1;

        public string QingQiuPiPei { get; set; } = "";

        public string QingLingZhi { get; set; } = "";

        public string PassZhi { get; set; } = "";
        public string  NGZhi{ get; set; } = "";

        /// <summary>
        /// 单位
        /// </summary>
        public string DanWei { get; set; } = "";

        /// <summary>
        /// 排序
        /// </summary>
        public int PaiXu { get; set; } = 0;

        public bool IsShuJuHeGe { get; set; } = true;

        public bool IsShangChuanHeGe { get; set; } = true;

        /// <summary>
        /// 操作数据
        /// </summary>
        public CaoZuoType CaoZuoType { get; set; } = CaoZuoType.JinZhanQingQiu;

        public override string ToString()
        {
            return $"[UpLimt:{Up.JiCunValue} LowLimt:{Low.JiCunValue} Value:{Value.JiCunValue} State:{IsShuJuHeGe} CaoZuoType:{CaoZuoType} ItemName:{ItemName} CodeOrNo:{CodeOrNo}]";
        }
    }

    public enum CaoZuoType
    {
        JinZhanQingQiu,
        JinZhanErWeiMa,
        JinZhanDuJieGuo,     
        JinZhanXieJieGuo,
        JinZhanWanCheng,
        ChuZhanQingQiu ,
        ChuZhanErWeiMa,
        ChuZhanDuJieGuo,
        ChuZhanXieJieGuo,
        ChuZhanWanCheng,
        DataShangChuan,
        /// <summary>
        /// 写心跳
        /// </summary>
        XieXinTiao,
    }

    public class ShuJuLisModel
    {
        /// <summary>
        ///对应的下限寄存器
        /// </summary>
        public string JCQStr { get; set; } = "";

        /// <summary>
        /// 寄存器的设备ID
        /// </summary>
        public int SheBeiID { get; set; } = -1;

        /// <summary>
        /// 测试参数
        /// </summary>
        public string JiCunValue { get; set; } = "";
    }
}
