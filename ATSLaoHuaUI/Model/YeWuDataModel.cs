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


        public ShuJuLisModel Value { get; set; } = new ShuJuLisModel();

        /// <summary>
        /// 工位ID
        /// </summary>
        public int GWID { get; set; } = 0;

        /// <summary>
        /// 工位ID
        /// </summary>
        public int MaTD { get; set; } = -1;

        /// <summary>
        /// 1-PLC状态为准 2-plc和上位机判断为准 3是以上位机判断为准
        /// </summary>
        public int IsYiZhuangTaiWeiZhun { get; set; } = 1;


        public double BaoHuZhi { get; set; } = 3;
        /// <summary>
        /// 1表示要上传
        /// </summary>
        public int IsShangChuan { get; set; } = 1;

        public string QingQiuPiPei { get; set; } = "";

        public string QingLingZhi { get; set; } = "";

        public string PassZhi { get; set; } = "";
        public string NGZhi { get; set; } = "";

        public string DanWei { get; set; } = "";

        /// <summary>
        /// 排序
        /// </summary>
        public int PaiXu { get; set; } = 0;

        public double ShangXian { get; set; } = 10;

        public double XiaXian { get; set; } = 1;

        /// <summary>
        /// true 表示要回读
        /// </summary>
        public bool IsHuiDu { get; set; } = false;

        public string TestTime { get; set; } = "";

        /// <summary>
        /// true 表示要回读
        /// </summary>
        public bool IsXianShi { get; set; } = true;

        /// <summary>
        /// 1表示需要循环唤醒
        /// </summary>
        public int IsXunHuanHuanXian { get; set; } = 0;

        public int CiShu { get; set; } = 0;

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool IsHeGe { get; set; } = false;

        public double WenDu { get; set; } = 0;
        /// <summary>
        /// 操作数据
        /// </summary>
        public CaoZuoType CaoZuoType { get; set; } = CaoZuoType.ZongQingQiuTest;

        public override string ToString()
        {
            return $"[{TestTime}-->Value:{Value.JiCunValue} LowLimit:{XiaXian} UpLimit:{ShangXian} IsHeGe:{IsHeGe} WenDu:{WenDu} ItemName:{ItemName} CodeOrNo:{CodeOrNo}]";
        }

        public YeWuDataModel FuZhi()
        {
            YeWuDataModel model = new YeWuDataModel();
            model.BaoHuZhi = BaoHuZhi;
            model.CodeOrNo = CodeOrNo;
            model.DanWei = DanWei;
            model.GWID = GWID;
            model.IsShangChuan = IsShangChuan;
            model.ItemName = ItemName;
            model.MaTD = MaTD;
            model.PaiXu = PaiXu;
            model.ShangXian = ShangXian;
            model. CaoZuoType = CaoZuoType;
            model.Value = Value.FuZhi();
            model.XiaXian = XiaXian;
            model.IsHeGe = IsHeGe;
            model.WenDu = WenDu;
            model.TestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return model;
        }
    }

    public enum CaoZuoType
    {
        ZongQingQiuTest,
        MaXieJieGuo,
        MaXieKaiShangDianJDQ,
        MaXieGuanShangDianJDQ,
        MaXieKaiCanJDQ,
        MaXieGuanCanJDQ,
        MaXieCanCMD,
        MaXieKaiHuanXingJDQ,
        MaXieGuanHuanXingJDQ,
        MaDataShangChuan,        
        ZongQingXieXinTiao,
        ZongFuWei,
        ZongDianYuan,
        ZongDuWenDu,
        ZongXieWenDu,
        ZongJieGuo,
        ZongWanCheng,
        ZongGuZhan,
        ZongQiDongPLCTest,
        ZongTingZhiPLCTest,
        ZongYunXingState,
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

        public bool IsKeKao { get; set; } = false;

        public ShuJuLisModel FuZhi()
        {
            ShuJuLisModel model = new ShuJuLisModel();
            model.JCQStr = JCQStr;
            model.IsKeKao = IsKeKao;
            model.JiCunValue = JiCunValue;
            model.SheBeiID = SheBeiID;
            return model;
        }
    }
}
