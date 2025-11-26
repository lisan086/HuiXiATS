using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuZhuangUI.Model
{
    public  class SheBeiZhanModel
    {
        /// <summary>
        /// true 表示启动
        /// </summary>
        public bool IsQiDong { get; set; } = false;
        /// <summary>
        /// 箱子的名称
        /// </summary>
        public string GaoWenName { get; set; } = "高温箱1";

        /// <summary>
        /// 温度下限
        /// </summary>
        public double WenDuLow { get; set; } = 60;

        /// <summary>
        /// 温度下限
        /// </summary>
        public double ZuiDiWenDuLow { get; set; } = 20;


        /// <summary>
        /// 温度上限
        /// </summary>
        public double WenDuUp { get; set; } = 150;

     

        /// <summary>
        /// 工位id
        /// </summary>
        public int GWID { get; set; } = 1;

        /// <summary>
        /// 实时温度
        /// </summary>
        public double ShiShiWenDu { get; set; } = 0;

        /// <summary>
        /// 总测试时间单位为(min)
        /// </summary>
        public double ShengWenTime { get; set; } = 60;
        /// <summary>
        /// 总测试时间单位为(min)
        /// </summary>
        public double SetTestTime { get; set; } = 60;
        /// <summary>
        /// 测试时间
        /// </summary>
        public double TestTime { get; set; } = 0;

        public string XianTi { get; set; } = "";

        public int MeiCiCANJianGeTime { get; set; } = 5000;

        public string QiTa { get; set; } = "";

        /// <summary>
        /// 1 表示上传mes
        /// </summary>
        public int IsShangMes { get; set; } = 0;

        /// <summary>
        /// 设备属于哪个组
        /// </summary>
        public string SheBeiZu { get; set; } = "";
        /// <summary>
        /// 时间为妙
        /// </summary>
        public float MeiGeCaiJiTime { get; set; } = 1;

        public int JinGeCanSendTime { get; set; } = 100;

        public string MiaoSu { get; set; } = "空闲";
        public SheBeiType SheBeiType { get; set; } = SheBeiType.JuanChengLoaHua;

        /// <summary>
        /// 0表示未测试 1表示测试完成 2表示正在测试
        /// </summary>
        public int TestState { get; set; } = 0;

        /// <summary>
        /// 1表示可以手动测试
        /// </summary>
        public int IsKeYiShouDongTest { get; set; } = 0;

        public bool IsGuZhang { get; set; } = false;

        public int IsYunXing { get; set; } = 0;
        /// <summary>
        /// 每个站的数据
        /// </summary>
        public List<MaTDModel> LisMaTD { get; set; } = new List<MaTDModel>();

        /// <summary>
        /// 每个站的请求
        /// </summary>
        public List<YeWuDataModel> LisQingQiu { get; set; } = new List<YeWuDataModel>();
    }

    public enum SheBeiType
    {
        JuanChengLoaHua,
      
    }


    public class MaTDModel
    {
        /// <summary>
        /// 检测项名称
        /// </summary>
        public string TDName { get; set; } = "";

        public int MaTDID { get; set; } = 1;

        /// <summary>
        /// 绑定的码
        /// </summary>
        public string BanMa { get; set; } = "";

        /// <summary>
        /// 0表示未测试 1表示正在升温 2表示正在测试 3表示测试合格 4表示不合格
        /// </summary>
        public int IsShuJuHeGe { get; set; } = 0;

        public int CiShu { get; set; } = 10;

        public int JiShuCiShu { get; set; } = 0;

       

        /// <summary>
        /// true表示上传mes合格
        /// </summary>
        public bool IsShangChuanHeGe { get; set; } = true;

        /// <summary>
        /// 底部描述
        /// </summary>
        public string DiBuMiaoSu { get; set; } = "空闲中...";

        /// <summary>
        /// 每个站的数据
        /// </summary>
        public List<YeWuDataModel> LisData { get; set; } = new List<YeWuDataModel>();

        /// <summary>
        /// 每个站的请求
        /// </summary>
        public List<YeWuDataModel> LisKongZhi { get; set; } = new List<YeWuDataModel>();
        /// <summary>
        /// 工位id
        /// </summary>
        public int GWID { get; set; } = 1;


    
        public List<YeWuDataModel> JiLuBuHe { get; set; } = new List<YeWuDataModel>();
        public void Clear()
        {
            BanMa = "";
            JiShuCiShu = 0;
            IsShuJuHeGe = 0;
            DiBuMiaoSu = "空闲中...";
            IsShangChuanHeGe = true;
            JiLuBuHe.Clear();
        }
    }
}
