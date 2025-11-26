using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSheBei.Model;

namespace ATSJianCeXianTi.Model
{
    /// <summary>
    /// 检测内容
    /// </summary>
    public class ZongTestModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        ///需要测试的总项数量
        /// </summary>
        public int TestCount { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public int TDID { get; set; } = -1;

        public List<QieHuanPeiFangModel> PeiFangDuiYingMa { get; set; } = new List<QieHuanPeiFangModel>();

        /// <summary>
        /// 测试处理的对象
        /// </summary>
        public List<ZhongJianModel> ZhongJianModels { get; set; } = new List<ZhongJianModel>();

        public List<string> JiaJuNames { get; set; } = new List<string>();
        /// <summary>
        /// 测试处理的对象
        /// </summary>
        public List<PeiFangShouWeiModel> ShouWeiChuLi { get; set; } = new List<PeiFangShouWeiModel>();


        public string GetZhiLingName(ShouWeiType shouWeiType)
        {
            foreach (var item in ShouWeiChuLi)
            {
                if (item.ShouWeiType == shouWeiType)
                {
                    return item.ZhiLingName;
                }
            }
            return "";
        }
    }


    public class QieHuanPeiFangModel
    {
        public string Name { get; set; } = "";

        /// <summary>
        /// 1表示用于切换的 0表示用于校验的
        /// </summary>
        public int IsQieHuan { get; set; } = 0;
    }
    /// <summary>
    /// 中间的测试项目
    /// </summary>
    public class ZhongJianModel
    {
        /// <summary>
        /// 测试总项目
        /// </summary>
        public TestModel TestModel { get; set; } = new TestModel();
    }

    /// <summary>
    /// 测试model
    /// </summary>
    public class TestModel
    {
        /// <summary>
        /// 测试名称
        /// </summary>
        public string ItemName { get; set; } = "";
        /// <summary>
        /// 该项的功能
        /// </summary>
        public string GongNengType { get; set; } = "";
        /// <summary>
        /// 操作那个设备的ID
        /// </summary>
        public int SheBeiID { get; set; } = 0;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string SheBeiName { get; set; } = "";

        /// <summary>
        /// 发的命令
        /// </summary>
        public string CMDSend { get; set; } = "";

        /// <summary>
        /// 对应的命令参数
        /// </summary>
        public string CMDCanShu { get; set; } = "";

        /// <summary>
        /// 对应的命令参数
        /// </summary>
        public string TaskNo { get; set; } = "";
        /// <summary>
        /// 下限
        /// </summary>
        public string LowStr { get; set; } = "--";

        /// <summary>
        /// 上限  
        /// </summary>
        public string UpStr { get; set; } = "--";

        /// <summary>
        ///单位
        /// </summary>
        public string DanWei { get; set; } = "";

        /// <summary>
        /// 比较类型
        /// </summary>
        public string BiJiaoType { get; set; } = "";

        /// <summary>
        /// 功能的类型
        /// </summary>
        public string LeiXing { get; set; } = "";


        public int JianCeCiShu { get; set; } = 0;

        /// <summary>
        /// 获取的值
        /// </summary>
        public object Value { get; set; } = "";

        /// <summary>
        /// 保留位数
        /// </summary>
        public int BaoLiuWeiShu { get; set; } = 0;

    

        /// <summary>
        /// true表示需要测试
        /// </summary>
        public bool IsTest { get; set; } = false;

        /// <summary>
        /// true  表示要上传mes
        /// </summary>
        public bool IsMes { get; set; } = false;

        /// <summary>
        /// 1 表示复检
        /// </summary>
        public int IsFuJian { get; set; } = 0;

        /// <summary>
        /// 复检的位置
        /// </summary>
        public string FuJianWeiZhi { get; set; } = "";

        /// <summary>
        /// true  表示无阻设备
        /// </summary>
        public bool IsWuZhuSheBei { get; set; } = false;
        /// <summary>
        /// 复检次数
        /// </summary>
        public int FuJianCiShu { get; set; } = -1;

        /// <summary>
        /// 复检次数
        /// </summary>
        public int FuJianCiShuF { get; set; } = -1;

        /// <summary>
        /// 适用的通道
        /// </summary>
        public string ShiYingTDID { get; set; } ="";

        /// <summary>
        /// true 表示断点，只有在调试模式下才有用
        /// </summary>
        public bool IsDuanDian { get; set; } = false;

        /// <summary>
        /// 1表示合格 2表示不合格 3正在检测 4表示未测
        /// </summary>
        public HeGeType IsHeGe { get; set; } = HeGeType.NoTest;

       

        /// <summary>
        /// 序号ID
        /// </summary>
        public string XuHaoID { get; set; } = "";

        /// <summary>
        /// 1表示字符串
        /// </summary>
        public int IsZiFuChuan { get; set; } = 1;

        /// <summary>
        /// 1 在主项复检之前执行 2表示在子项之后执行 -1  表示总项
        /// </summary>
        public int ZiXiangShunXu { get; set; } = -1;

        /// <summary>
        /// 不合格测量多少次数
        /// </summary>
        public int CiShu { get; set; } = 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        public string KaiShiTime { get; set; } = "";

        /// <summary>
        /// 总时间
        /// </summary>
        public string JieSuTime { get; set; } = "";

        public string TestTime { get; set; } = "0";

        /// <summary>
        /// 后面自动赋值
        /// </summary>
        public int WeiZhi { get; set; } = -1;

        /// <summary>
        /// 乘数
        /// </summary>
        public float BeiChuShu { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public float KB { get; set; } = 0;

        

        /// <summary>
        /// 缓存值
        /// </summary>
        public List<CaoZuoHuanCunModel> HuanCunBiaoShi { get; set; } =new List<CaoZuoHuanCunModel>();

        public ZhiType IsZhiOK { get; set; } = ZhiType.Wu;
        /// <summary>
        /// 这里是导入导出文档
        /// </summary>
        /// <returns></returns>
        public List<string> GetData()
        {
            List<string> lis = new List<string>();
            lis.Add("TaskNo");
            lis.Add("ItemName");
            lis.Add("GongNengType");
            lis.Add("SheBeiName");
            lis.Add("CMDSend");
            lis.Add("CMDCanShu");
            lis.Add("LowStr");
            lis.Add("UpStr");
            lis.Add("DanWei");
            lis.Add("BiJiaoType");
            lis.Add("BaoLiuWeiShu");
            lis.Add("IsTest");
            lis.Add("IsMes");
            lis.Add("CiShu");
            lis.Add("BeiChuShu");
            lis.Add("HuanCunBiaoShi");
            lis.Add("IsZhiOK");
            lis.Add("KB");
            return lis;
        }

        public override string ToString()
        {
            return $"【ItemName:{ItemName},LowStr:{LowStr},UpStr:{UpStr},DanWei:{DanWei},Value:{Value},IsHeGe:{IsHeGe},TestTime:{TestTime},JianCeCiShu:{JianCeCiShu},IsMes:{IsMes},IsTest:{IsTest},BiJiaoType:{BiJiaoType}】";
        }

        /// <summary>
        /// 这里是测试的xlsx
        /// </summary>
        /// <returns></returns>
        public List<string> GetTestData()
        {
            List<string> lis = new List<string>();
            lis.Add("TaskNo");
            lis.Add("ItemName");
            lis.Add("LowStr");
            lis.Add("UpStr");
            lis.Add("Value");
            lis.Add("IsHeGe");
            lis.Add("DanWei");
            lis.Add("TestTime");
            lis.Add("IsTest");
            lis.Add("IsMes");
            lis.Add("KaiShiTime");
            lis.Add("JieSuTime");
            lis.Add("JianCeCiShu");
            return lis;
        }

        public void Clear()
        {
            Value = "";
            IsHeGe = HeGeType.NoTest;
            TestTime = "0";
            KaiShiTime = "";
            JieSuTime = "";
            JianCeCiShu = 0;
            FuJianCiShu = FuJianCiShuF;
        }
      
        public TestModel NewFuZhi()
        {
            TestModel model = new TestModel();
            model.BaoLiuWeiShu = BaoLiuWeiShu;
            model.BeiChuShu = BeiChuShu;
            model.BiJiaoType = BiJiaoType;
            model.CiShu= CiShu;
            model.CMDCanShu = CMDCanShu;
            model.CMDSend= CMDSend;
            model.DanWei= DanWei;
            model.GongNengType = GongNengType;
            model.HuanCunBiaoShi = new List<CaoZuoHuanCunModel>();
            for (int i = 0; i < HuanCunBiaoShi.Count; i++)
            {
                model.HuanCunBiaoShi.Add(HuanCunBiaoShi[i]);
            }
            model.IsDuanDian = IsDuanDian;
            model.IsHeGe= IsHeGe;
            model.IsMes= IsMes;
            model.IsTest= IsTest;
            model.IsZhiOK= IsZhiOK;
            model.IsZiFuChuan = IsZiFuChuan;
            model.ItemName = ItemName;
            model.KB = KB;
            model.LeiXing = LeiXing;
            model.LowStr= LowStr;
            model.SheBeiID = SheBeiID;
            model.SheBeiName = SheBeiName;
            model.TaskNo = TaskNo;
            model.TestTime = TestTime;
            model.Value = Value;
            model.UpStr = UpStr;
            model.WeiZhi = WeiZhi;
            model.XuHaoID = XuHaoID;
            model.ZiXiangShunXu = ZiXiangShunXu;
            model.KaiShiTime = KaiShiTime;
            model.JieSuTime = JieSuTime;
            model.JianCeCiShu = JianCeCiShu;     
            model.FuJianCiShu = FuJianCiShu;
            model.FuJianWeiZhi = FuJianWeiZhi;
            return model;
        }


        public void GetZuiZhongJieGuo()
        {
            if (IsZhiOK == ZhiType.取0K值)
            {
                UpStr = "OK";
                LowStr = "OK";
                if (IsHeGe == HeGeType.Pass)
                {
                    Value = "OK";
                }
                else
                {
                    Value = "NG";
                }
            }
            else if (IsZhiOK == ZhiType.取上限)
            {
           
                if (IsHeGe == HeGeType.Pass)
                {
                    Value = UpStr;
                }
                else
                {
                    Value = "NG";
                }
            }
            else if (IsZhiOK == ZhiType.取下限)
            {

                if (IsHeGe == HeGeType.Pass)
                {
                    Value = LowStr;
                }
                else
                {
                    Value = "NG";
                }
            }
        }
    }

    public enum HeGeType
    {
        Pass,
        NG,
        ZhengZaiTest,
        NoTest,
    }


    public class CaoZuoHuanCunModel
    {
        /// <summary>
        /// 这个参数是唯一的
        /// </summary>
        public string HuanCunName { get; set; } = "";
     
        public HuanCunCaoZuoType HuanCunCaoZuoType { get; set; } = HuanCunCaoZuoType.BaoCunHunCun;

        public int BiaoHao { get; set; } = 0;
        public CaoZuoHuanCunModel FuZhi()
        {
            CaoZuoHuanCunModel model = new CaoZuoHuanCunModel();
            model.HuanCunName = HuanCunName;
            model.BiaoHao = BiaoHao;
            model.HuanCunCaoZuoType=HuanCunCaoZuoType;
            return model;
        }
    }

    public enum HuanCunCaoZuoType
    {
        BaoCunHunCun,
        FuZhiHuanCun,
    }



    /// <summary>
    /// 执行执行开始和合格的命令
    /// </summary>
    public class PeiFangShouWeiModel
    {
        public ShouWeiType ShouWeiType { get; set; } = ShouWeiType.KaiShi;
        public string ZhiLingName { get; set; } = "";
    }


    public enum ShouWeiType
    {
        KaiShi,
        JieSu,
        HeGe,
        BuHeGe,
    }

    public enum ZhiType
    {
        Wu,
        取0K值, 
        取上限,
        取下限,
    }
}
