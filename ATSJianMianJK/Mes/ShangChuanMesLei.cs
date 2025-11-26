using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATSJianMianJK.Log;
using CommLei.GongYeJieHe;
using CommLei.JiChuLei;
using Common.DataChuLi;
using JieMianLei.FuFrom;

namespace ATSJianMianJK.Mes
{
    /// <summary>
    /// 上传Mes类
    /// </summary>
    public class ShangChuanMesLei
    {

        private Dictionary<int, bool> TDShangChuanState = new Dictionary<int, bool>();
        private XuShangChuanLei XuShangChuanLei = null;
        #region 单利
        private readonly static object _DuiXiang = new object();

        private static ShangChuanMesLei _LogTxt = null;



        private ShangChuanMesLei()
        {
            SetShangChuanLei();
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static ShangChuanMesLei Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new ShangChuanMesLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        /// <summary>
        /// 刷新通道的上传状态
        /// </summary>
        /// <param name="tdid"></param>
        /// <param name="isshangchuan"></param>
        public void ShuXinTdState(int tdid, bool isshangchuan)
        {
            if (TDShangChuanState.ContainsKey(tdid))
            {
                TDShangChuanState[tdid] = isshangchuan;
            }
            else
            {
                TDShangChuanState.Add(tdid, isshangchuan);
            }

        }



        private void SetShangChuanLei()
        {

            string mulu = string.Format("{0}{1}", Directory.GetCurrentDirectory(), @"\MesQu");
            if (Directory.Exists(mulu) == false)
            {
                Directory.CreateDirectory(mulu);
            }
            JieKouJiaZaiLei<XuShangChuanLei> wenss = new JieKouJiaZaiLei<XuShangChuanLei>();
            List<Type> shebeistypes = wenss.JiaZaiLisType(mulu);
            foreach (var item in shebeistypes)
            {
                try
                {
                    XuShangChuanLei = (XuShangChuanLei)Activator.CreateInstance(item); XuShangChuanLei.SetCanShu(1);
                }
                catch
                {


                }

            }

            if (XuShangChuanLei == null)
            {
                XuShangChuanLei = new RiJiMesLei();
            }

        }

        /// <summary>
        /// 上传mes
        /// </summary>
        /// <param name="mesmodel"></param>
        /// <returns></returns>
        public LianWanModel ShangMes(ShangChuanDataModel mesmodel)
        {

            if (mesmodel == null)
            {
                LianWanModel model = new LianWanModel();
                model.FanHuiJieGuo = JinZhanJieGuoType.NG;
                model.NeiRong = "传来的数据对象为空";
                return model;
            }
            if (TDShangChuanState.ContainsKey(mesmodel.TDID))
            {
                bool isshangchuan = TDShangChuanState[mesmodel.TDID];
                if (isshangchuan)
                {
                    if (mesmodel.ShangChuanType == ShangChuanType.KaiShi)
                    {
                        LianWanModel lianWan = XuShangChuanLei.KaiShiTiJiaoMa(mesmodel);
                        RiJiLog.Cerate().Add(RiJiEnum.MesData, lianWan.NeiRong, mesmodel.TDID);
                        return lianWan;
                    }
                    else if (mesmodel.ShangChuanType == ShangChuanType.BuZhouShangChuan)
                    {
                        LianWanModel lianWan = XuShangChuanLei.BuZhouShangChuan(mesmodel);
                        RiJiLog.Cerate().Add(RiJiEnum.MesData, lianWan.NeiRong, mesmodel.TDID);
                        return lianWan;
                    }
                    else if (mesmodel.ShangChuanType == ShangChuanType.JieSu)
                    {
                        LianWanModel lianWan = XuShangChuanLei.JieSu(mesmodel);
                        RiJiLog.Cerate().Add(RiJiEnum.MesData, lianWan.NeiRong, mesmodel.TDID);
                        return lianWan;
                    }
                    else if (mesmodel.ShangChuanType == ShangChuanType.HuoQuXinXi)
                    {
                        LianWanModel lianWan = XuShangChuanLei.GetQiTaXinXi(mesmodel);
                        RiJiLog.Cerate().Add(RiJiEnum.MesData, lianWan.NeiRong, mesmodel.TDID);
                        return lianWan;
                    }
                    else if (mesmodel.ShangChuanType == ShangChuanType.BangMa)
                    {
                        LianWanModel lianWan = XuShangChuanLei.ZhongJianBangMa(mesmodel);
                        RiJiLog.Cerate().Add(RiJiEnum.MesData, lianWan.NeiRong, mesmodel.TDID);
                        return lianWan;
                    }
                    else
                    {
                        LianWanModel model = new LianWanModel();
                        model.FanHuiJieGuo = JinZhanJieGuoType.NG;
                        model.NeiRong = "类型不对";
                        RiJiLog.Cerate().Add(RiJiEnum.MesData, model.NeiRong, mesmodel.TDID);
                        return model;

                    }
                }
                else
                {
                    LianWanModel model = new LianWanModel();
                    model.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                    model.NeiRong = "不需要上传Mes";
                    model.FanHuiCanShu = "2";
                    RiJiLog.Cerate().Add(RiJiEnum.MesData, model.NeiRong, mesmodel.TDID);
                    return model;
                }
            }
            else
            {
                LianWanModel model = new LianWanModel();
                model.FanHuiJieGuo = JinZhanJieGuoType.NG;
                model.NeiRong = "没有找到该上传数据的通道";
                RiJiLog.Cerate().Add(RiJiEnum.MesData, model.NeiRong, mesmodel.TDID);
                return model;
            }

        }

        public void Close()
        {
            if (XuShangChuanLei != null)
            {
                XuShangChuanLei.Close();
            }
        }

        public void GetPeiFrm()
        {
            XuShangChuanLei.GetPeiZhiForm();
        }
    }


    /// <summary>
    /// 上传
    /// </summary>
    public abstract class XuShangChuanLei
    {

        public abstract void  SetCanShu(int TDID);

        /// <summary>
        /// 开始提交码
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public abstract LianWanModel KaiShiTiJiaoMa(ShangChuanDataModel models);

        /// <summary>
        /// 获取mes的信息
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public abstract LianWanModel GetQiTaXinXi(ShangChuanDataModel models);


        /// <summary>
        /// 所有步骤上传
        /// </summary>
        /// <param name="erweima"></param>
        /// <param name="lismode"></param>
        /// <param name="ishege"></param>
        /// <returns></returns>
        public abstract LianWanModel BuZhouShangChuan(ShangChuanDataModel models);

        /// <summary>
        /// 结束
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public abstract LianWanModel JieSu(ShangChuanDataModel models);

        /// <summary>
        /// 用于绑码
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public abstract LianWanModel ZhongJianBangMa(ShangChuanDataModel models);

        /// <summary>
        /// 关闭
        /// </summary>
        public abstract void Close();

        public abstract void GetPeiZhiForm();
     
    }


    public class RiJiMesLei : XuShangChuanLei
    {

        private SuiJiShuLei SuiJiShuLei;
        private Dictionary<string, RiJiMesModel> AddNeiRong = new Dictionary<string, RiJiMesModel>();


        private FanXingJiHeLei<RiJiMesModel> WanZhengShuJu = new FanXingJiHeLei<RiJiMesModel>();

        public readonly object Suo = new object();

        private bool _bRuning = false;


        public RiJiMesLei()
        {
            SuiJiShuLei = new SuiJiShuLei();
            _bRuning = true;
            Thread xiancheng = new Thread(Working);
            xiancheng.IsBackground = true;
            xiancheng.DisableComObjectEagerCleanup();
            xiancheng.Start();
        }


        public override LianWanModel KaiShiTiJiaoMa(ShangChuanDataModel model)
        {
            LianWanModel jieguomodel = new LianWanModel();
            lock (Suo)
            {
                ClearHuanCun();
                string key = $"{model.TDID}:{model.GuoChengMa}";
                if (AddNeiRong.ContainsKey(key))
                {
                    AddNeiRong[key].Clear();
                }
                else
                {
                    RiJiMesModel rijimodels = new RiJiMesModel();
                    AddNeiRong.Add(key, rijimodels);
                }
                RiJiMesModel rijimodel = AddNeiRong[key];
                rijimodel.ZhanName = model.TDName;
                rijimodel.Ma = model.GuoChengMa;
                rijimodel.IsWanCeng = 0;
                rijimodel.ZongJieGuo = true;
                if (model.KaiShiModel.IsShouZhan)
                {
                    if (string.IsNullOrEmpty(model.KaiShiModel.QiTaZhi) == false)
                    {
                        jieguomodel.NeiRong = $"{model.GuoChengMa}与{model.KaiShiModel.QiTaZhi} 绑码成功";
                        rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);
                        rijimodel.IsWanCeng = 2;
                        WanZhengShuJu.Add(rijimodel);
                    }
                    else
                    {
                        jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}进站成功";
                        rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);
                    }
                }
                else
                {
                    jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}进站成功";
                    rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);
                }
              
            }
            jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;

            return jieguomodel;
        }

        public override LianWanModel JieSu(ShangChuanDataModel model)
        {
            LianWanModel jieguomodel = new LianWanModel();
            bool ischengg = false;
            string key = $"{model.TDID}:{model.GuoChengMa}";
            if (AddNeiRong.ContainsKey(key))
            {
                ischengg = true;
            }
            if (ischengg)
            {
                RiJiMesModel rijimodel = AddNeiRong[key];
                rijimodel.ZongJieGuo = model.JieSuModel.IsHeGe;
                bool jieguo = rijimodel.ZongJieGuo;
                if (jieguo)
                {
                    jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}出站成功";
                    jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                }
                else
                {
                    jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}出站失败,有数据不合格";
                    jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
                }
                rijimodel.StringBuilder.AppendLine($"{jieguomodel.NeiRong}  ,{model.JieSuModel.CanShu}");
                rijimodel.IsWanCeng = 2;
                WanZhengShuJu.Add(rijimodel);
            }
            else
            {
                RiJiMesModel rijimodel = new RiJiMesModel();
                rijimodel.ZhanName = model.TDName;
                rijimodel.Ma = model.GuoChengMa;
                rijimodel.IsWanCeng = 2;
                rijimodel.ZongJieGuo = false;
                jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}出站失败,没有找到该入站信息";
                rijimodel.StringBuilder.AppendLine($"{jieguomodel.NeiRong}  ,{model.JieSuModel.CanShu}");
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
                WanZhengShuJu.Add(rijimodel);
            }
            return jieguomodel;

        }
        public override LianWanModel GetQiTaXinXi(ShangChuanDataModel model)
        {
            if (model.HuoQuXinXiModel.CanShu == "zxqq")
            {
                LianWanModel lianWanModel = new LianWanModel();
                lianWanModel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                lianWanModel.FanHuiCanShu = SuiJiShuLei.SuiJiData(1000, 9999).ToString();
                lianWanModel.NeiRong = $"发送装箱启动";
                return lianWanModel;
            }
            else if (model.HuoQuXinXiModel.CanShu == "agvweizhi")
            {
                LianWanModel lianWanModel = new LianWanModel();
                lianWanModel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                lianWanModel.FanHuiCanShu = "2";
                lianWanModel.NeiRong = $"发送装箱启动";
                return lianWanModel;
            }
            else
            {
                LianWanModel lianWanModel = new LianWanModel();
                lianWanModel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                lianWanModel.FanHuiCanShu = SuiJiShuLei.SuiJiData(1000, 9999).ToString();
                lianWanModel.NeiRong = $"PCB校验成功";
                return lianWanModel;
            }
        }

        public override LianWanModel BuZhouShangChuan(ShangChuanDataModel model)
        {
            LianWanModel jieguomodel = new LianWanModel();

            string key = $"{model.TDID}:{model.GuoChengMa}";
            bool ischengg = false;
            if (AddNeiRong.ContainsKey(key))
            {
                ischengg = true;
            }
            else
            {
                RiJiMesModel rijimodel =new RiJiMesModel();
                rijimodel.ZhanName = model.TDName;
                rijimodel.Ma = model.GuoChengMa;
                rijimodel.IsWanCeng = 0;
                rijimodel.ZongJieGuo = true;
                AddNeiRong.Add(key, rijimodel);
                ischengg = true;
            }
            if (ischengg)
            {
                RiJiMesModel rijimodel = AddNeiRong[key];
              
                jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}上传数据:{model.BuZhouModel.BuZhouShuJu.ToString()}";
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);
            }
            else
            {
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
                jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}没有找到入站信息";
               
            }
            return jieguomodel;
        }

        public override LianWanModel ZhongJianBangMa(ShangChuanDataModel model)
        {
            LianWanModel jieguomodel = new LianWanModel();

            string key = $"{model.TDID}:{model.GuoChengMa}";
            bool ischengg = false;
            if (AddNeiRong.ContainsKey(key))
            {
                ischengg = true;
            }
            if (ischengg)
            {
                RiJiMesModel rijimodel = AddNeiRong[key];

                jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}绑码:{ChangYong.FenGeDaBao( model.BangMaModel.MaS," ")}";
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);
            }
            else
            {
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
                jieguomodel.NeiRong = $"{model.GuoChengMa}:在{model.TDName}绑码时,没有找到入站信息";
             
            }
            return jieguomodel;
        }
        private void Working()
        {

            while (_bRuning)
            {

                try
                {
                    int count = WanZhengShuJu.GetCount();
                    if (count > 0)
                    {
                        RiJiMesModel model = WanZhengShuJu.GetModel_Head_RomeHead();
                        XieRiZhiMuLu(model);
                        model.IsWanCeng = 3;
                    }



                }
                catch
                {


                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 关闭日志，只有在程序关闭时调用
        /// </summary>
        public override  void Close()
        {
            _bRuning = false;
            Thread.Sleep(10);
            ClearZiDian();
        }






        /// <summary>
        /// 删除字典
        /// </summary>
        private void ClearZiDian()
        {
            AddNeiRong.Clear();
            WanZhengShuJu.Romve_All();
        }



        private void XieRiZhiMuLu(RiJiMesModel model)
        {
            if (_bRuning)
            {
                string lujing = GetLuJing(model.ZhanName);
                string pass = model.ZongJieGuo ? "PASS" : "NG";
                string wenjianname = $"{lujing}\\{model.Ma}_{pass}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";

                using (FileStream xieruliu = new FileStream(wenjianname, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    try
                    {
                        byte[] shuju = Encoding.UTF8.GetBytes(model.StringBuilder.ToString());
                        xieruliu.Write(shuju, 0, shuju.Length);
                    }
                    catch
                    {



                    }


                }

            }
        }


        private string GetLuJing(string shebeiname)
        {
            string path = string.Format(@"{0}\{1}\{2}\{3}", Path.GetFullPath("."), "Mes", shebeiname, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path; ;
        }

        private void ClearHuanCun()
        {
            try
            {

                if (AddNeiRong.Count > 0)
                {
                    List<string> keys = new List<string>();
                    foreach (var item in AddNeiRong.Keys)
                    {
                        RiJiMesModel model = AddNeiRong[item];
                        if (model.IsWanCeng == 3)
                        {
                            keys.Add(item);
                        }
                    }
                    for (int i = 0; i < keys.Count; i++)
                    {
                        AddNeiRong.Remove(keys[i]);
                    }
                }
            }
            catch
            {


            }

        }

        public override void SetCanShu(int TDID)
        {
           
        }

        public override void GetPeiZhiForm()
        {
           
        }
    }

    public class RiJiMesModel
    {
        public string Ma { get; set; } = "";

        public string ZhanName { get; set; } = "";

        /// <summary>
        /// 1表示开始 2表示完成 3表示写完 
        /// </summary>
        public int IsWanCeng { get; set; } = 0;

        public bool ZongJieGuo { get; set; } = false;

        public StringBuilder StringBuilder { get; set; } = new StringBuilder();

        public void Clear()
        {
            StringBuilder.Clear();
            IsWanCeng = 0;
            ZongJieGuo = false;
            ZhanName = "";
        }
    }

    /// <summary>
    /// 联网返回参数
    /// </summary>
    public class LianWanModel
    {
        /// <summary>
        /// true表示返回正确
        /// </summary>
        public JinZhanJieGuoType FanHuiJieGuo { get; set; } = JinZhanJieGuoType.NG;

        /// <summary>
        /// 返回的内容
        /// </summary>
        public string NeiRong { get; set; } = "";

        /// <summary>
        /// 返回的参数
        /// </summary>
        public string FanHuiCanShu { get; set; } = "";
    }


    /// <summary>
    /// 上传的数据
    /// </summary>
    public class ShangChuanDataModel
    {
        /// <summary>
        /// 上传类型
        /// </summary>
        public ShangChuanType ShangChuanType { get; set; } = ShangChuanType.KaiShi;

        /// <summary>
        /// 通道ID
        /// </summary>
        public int TDID { get; set; } = -1;

        /// <summary>
        /// 通道名称
        /// </summary>
        public string TDName { get; set; } = "";

        public string GuoChengMa { get; set; } = "";

        /// <summary>
        /// 开始需要的数据
        /// </summary>
        public KaiShiModel KaiShiModel { get; set; } = new KaiShiModel();

        /// <summary>
        /// 步骤数据
        /// </summary>
        public BuZhouModel BuZhouModel { get; set; } = new BuZhouModel();


        public JieSuModel JieSuModel { get; set; } = new JieSuModel();

        public HuoQuXinXiModel HuoQuXinXiModel { get; set; } = new HuoQuXinXiModel();

        public BangMaModel BangMaModel { get; set; } = new BangMaModel();
    }

    public class KaiShiModel
    {

        public bool IsShouZhan { get; set; } = false;
        public string QiTaZhi { get; set; } = "";
    }

    public class BuZhouModel
    {
        /// <summary>
        /// 步骤数据
        /// </summary>
        public object BuZhouShuJu { get; set; } = "";

        /// <summary>
        /// true表示合格结果
        /// </summary>
        public bool JieGuo { get; set; } = false;
    }

    public class JieSuModel
    {
        public bool IsHeGe { get; set; } = false;

        public string KaiShiTime { get; set; } = "";

        public string JieSuTime { get; set; } = "";

        public string CanShu { get; set; } = "";
    }

    public class HuoQuXinXiModel
    {
        public string CanShu { get; set; } = "";
    }

    public class BangMaModel
    {
        public List<string> MaS { get; set; } = new List<string>();
    }
    public enum ShangChuanType
    { 
     
        KaiShi,
        BuZhouShangChuan,
        JieSu,
        HuoQuXinXi,
        BangMa,
    }
    public enum JinZhanJieGuoType
    {
        Pass,
        NG,
    }
}
