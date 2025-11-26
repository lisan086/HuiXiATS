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
        /// 1-PLC状态为准 2-plc和上位机判断为准 3是以上位机判断为准 4是相机对比
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

        public string ZhuanTaiPiPeiZhi { get; set; } = "";
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
        public CaoZuoType CaoZuoType { get; set; } = CaoZuoType.DataShangChuan;

        public string YongDeMaMingCheng { get; set; } = "";
        public override string ToString()
        {
            return $"[UpLimt:{Up.JiCunValue} LowLimt:{Low.JiCunValue} Value:{Value.JiCunValue} State:{IsShuJuHeGe} CaoZuoType:{CaoZuoType} ItemName:{ItemName} CodeOrNo:{CodeOrNo}]";
        }
    }

    public enum CaoZuoType
    {
        ShouZhan请求_单,
        ShouZhan过程码_单,
        ShouZhan绑码_多,
        ShouZhan写结果_多,
        ShouZhan写型号_单,
        Zhan进站请求_单,
        Zhan进站过程码_单,
        Zhan进站写结果_多,
        Zhan出站请求_单,
        Zhan出站过程码_单,
        Zhan出站写结果_多,
        Zhan写型号_单,
        Zhan绑码_多,
        Zhan测试工位_单,
        FoZhanSaoMa请求_单,
        FoZhanSaoMa启动扫码_多,
        FoZhanSaoMa读码上_单,
        FoZhanSaoMa读码下_单,
        FoZhanSaoMa计算扫码_单,
        FoZhanSaoMa写码结果_单,
        FoZhanSaoMa写PLC结果_多,
        FoZhanSaoMa写型号_单,
        DataShangChuan,  
        LeiDiaoKe进站请求_单,
        LeiDiaoKeMa进站数据_多,
        LeiDiaoKeXie进站激光触发_单,
        LeiDiaoKe进站写数据_单,
        LeiDiaoKe进站结果_多,          
        LeiDiaoKe出站请求_单,
        LeiDiaoKe出站PCB码_单,
        LeiDiaoKe出站码_单,
        LeiDiaoKeXieKeTiMa_单,
        LeiDiaoKe出站结果_多,
        LeiDiaoPCB进站请求_单,
        LeiDiaoPCB进站结果_多,
        LeiDiaoPLB进站码_单,  
        LeiDiaoShiChang_单,
        ICTChuZhan请求_单,
        ICTChuZhanJieGuo_单,
        AGVChuJinZhan请求_单,
        AGVChuJinZhanMa_单,
        AGVChuJinZhanJieGuo_多,
        AGVChuQiuQuWeiZhi_单,
        AGVChuQuWeiZhi_单,
        AGVChuChuZhan请求_单,
        AGVChuChuZhanMa_单,
        AGVChuZhanWeiZhi_单,
        AGVChuChuZhanLie_单,
        AGVChuChuZhanCeng_单,
        AGVChuChuZhanPai_单,
        AGVChuChuZhanJieGuo_多,
        AGVChuChuZhanManPan_单,
        AGVChuQuZouHuoWu_单,
        AGVJinJinZhan请求_单,
        AGVJinJinZhan码_单,
        AGVJinJinZhan标志_单,
        AGVJinJinZhan结果_多,
        AGVJinChuZhan请求_单,
        AGVJinChuZhan码_单,
        AGVJinKongPan请求_单,
        AGVJinYunLai_单,
        AGVJinChuZhan结果_多,
        ZXJinZhan请求_单,
        ZXJinZhanMa_单,
        ZXJinZhan结果_单,
        ZXQiDong_单,
        ZXXie箱号_单,
        ZXJieGuo_多,
        ZXJieSu_单,
        ZXJieGuoMa_单,
        ZXZhongLiang_单,
        ZXChuZhan请求_单,
        ZXChuZhanMa_单,
        ZXChuZhanPai_单,
        ZXChuZhanLie_单,
        ZXChuZhanCeng_单,
        ZXChuZhanJieGuo_多,
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
        public object JiCunValue { get; set; } = "";
        public bool IsStr { get; set; } = false;

        public int StrKaiShi { get; set; } = -1;
        public int StrCount { get; set; } = -1;
    }
}
