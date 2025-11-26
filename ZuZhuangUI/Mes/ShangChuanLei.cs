using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using ZuZhuangUI.Model;

namespace ZuZhuangUI.Mes
{
    public class ShangChuanLei
    {
        public ABSShangChuanLei ABSShangChuanLei = null;
        #region 单利
        private readonly static object _DuiXiang = new object();

        private static ShangChuanLei _LogTxt = null;



        private ShangChuanLei()
        {
            ABSShangChuanLei = new RiJiMesLei();
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static ShangChuanLei Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new ShangChuanLei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion
       
    }
    /// <summary>
    /// 上传
    /// </summary>
    public class ABSShangChuanLei
    {

       
        public virtual LianWanModel KaiShiTiJiaoMa(SheBeiZhanModel model, string erweima,bool isshouzhan)
        {
            LianWanModel jieguo = new LianWanModel();
            jieguo.FanHuiJieGuo = JinZhanJieGuoType.Pass;
            jieguo.NeiRong = "开始提交扫码用的父类";

            return jieguo;
        }
        public virtual LianWanModel GetQiTaXinXi(SheBeiZhanModel model, object shuju, int biaozhi)
        {
            LianWanModel jieguo = new LianWanModel();
            jieguo.FanHuiJieGuo = JinZhanJieGuoType.Pass;
            jieguo.NeiRong = "获取数据用的父类";

            return jieguo;
        }

        /// <summary>
        /// 所有步骤上传
        /// </summary>
        /// <param name="erweima"></param>
        /// <param name="lismode"></param>
        /// <param name="ishege"></param>
        /// <returns></returns>
        public virtual LianWanModel BuZhouShangChuan(SheBeiZhanModel tdid, string erweima, YeWuDataModel lismode)
        {
            LianWanModel model = new LianWanModel();
            model.FanHuiJieGuo = JinZhanJieGuoType.Pass;
            model.NeiRong = "测试的数据用的父类";

            return model;
        }

        public virtual LianWanModel JieSu(SheBeiZhanModel tdid, string erweima, bool iszonghege)
        {
            LianWanModel model = new LianWanModel();
            model.FanHuiJieGuo = JinZhanJieGuoType.Pass;
            model.NeiRong = "结束数据用的父类";

            return model;

        }



    }


    public class RiJiMesLei: ABSShangChuanLei
    {
      
        private Dictionary<string, RiJiMesModel> AddNeiRong = new Dictionary<string, RiJiMesModel>();


        private FanXingJiHeLei<RiJiMesModel> WanZhengShuJu = new FanXingJiHeLei<RiJiMesModel>();

        public readonly object Suo = new object();
       
        private bool _bRuning = false;


        public RiJiMesLei()
        {
            _bRuning = true;
            Thread xiancheng = new Thread(Working);
            xiancheng.IsBackground = true;
            xiancheng.DisableComObjectEagerCleanup();
            xiancheng.Start();
        }


        public override LianWanModel KaiShiTiJiaoMa(SheBeiZhanModel model, string erweima, bool isshouzhan)
        {
            LianWanModel jieguomodel = new LianWanModel();
            lock (Suo)
            {
                ClearHuanCun();
                string key = $"{model.LineCode}:{erweima}";
                if (AddNeiRong.ContainsKey(key))
                {
                    AddNeiRong[key].Clear();
                }
                else
                {
                    RiJiMesModel rijimodels = new RiJiMesModel();
                    AddNeiRong.Add(key, rijimodels);
                  
                   
                }
                RiJiMesModel rijimodel = AddNeiRong[erweima];
                rijimodel.ZhanName = model.LineCode;
                rijimodel.Ma = erweima;
                rijimodel.IsWanCeng = 0;
                rijimodel.ZongJieGuo = true;
                jieguomodel.NeiRong = $"{erweima}:在{model.LineCode}进站成功";
                rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);
            }       
            jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
           
            return jieguomodel;
        }

        public override LianWanModel JieSu(SheBeiZhanModel tdid, string erweima, bool iszonghege)
        {
            LianWanModel jieguomodel = new LianWanModel();
            bool ischengg = false;
            string key = $"{tdid.LineCode}:{erweima}";
            if (AddNeiRong.ContainsKey(key))
            {
                ischengg = true;
            }
            if (ischengg)
            {
                RiJiMesModel rijimodel = AddNeiRong[key];
                bool jieguo = rijimodel.ZongJieGuo;
                if (jieguo)
                {
                    jieguomodel.NeiRong = $"{erweima}:在{tdid.LineCode}出站成功";
                    jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                }
                else
                {
                    jieguomodel.NeiRong = $"{erweima}:在{tdid.LineCode}出站失败,有数据不合格";
                    jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
                }
                rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);                        
                rijimodel.IsWanCeng = 2;             
                WanZhengShuJu.Add(rijimodel);
            }
            else
            {
                RiJiMesModel rijimodel = new RiJiMesModel();
                rijimodel.ZhanName = tdid.LineCode;
                rijimodel.Ma = erweima;
                rijimodel.IsWanCeng = 2;
                rijimodel.ZongJieGuo = false;
                jieguomodel.NeiRong = $"{erweima}:在{tdid.LineCode}出站失败,没有找到该入站信息";
                rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
                WanZhengShuJu.Add( rijimodel );
            }         
            return jieguomodel;
           
        }
        public override LianWanModel GetQiTaXinXi(SheBeiZhanModel model, object shuju, int biaozhi)
        {
            return base.GetQiTaXinXi(model, shuju, biaozhi);
        }

        public override LianWanModel BuZhouShangChuan(SheBeiZhanModel tdid, string erweima, YeWuDataModel lismode)
        {
            LianWanModel jieguomodel = new LianWanModel();

            string key = $"{tdid.LineCode}:{erweima}";
            bool ischengg = false;
            if (AddNeiRong.ContainsKey(key))
            {
                ischengg = true;
            }
            if (ischengg)
            {
                RiJiMesModel rijimodel = AddNeiRong[key];
                if (lismode.IsShuJuHeGe==false)
                {
                    rijimodel.ZongJieGuo = false;
                }
                jieguomodel.NeiRong = $"{erweima}:在{tdid.LineCode}上传数据:{lismode.ToString()}";
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.Pass;
                rijimodel.StringBuilder.AppendLine(jieguomodel.NeiRong);
            }
            else
            {
                jieguomodel.FanHuiJieGuo = JinZhanJieGuoType.NG;
                jieguomodel.NeiRong = $"{erweima}:在{tdid.LineCode}没有找到入站信息";
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
                    if (count>0)
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
        private  void Close()
        {
            _bRuning = false;
            Thread.Sleep(10);
            ClearZiDian();
        }
      


     


        /// <summary>
        /// 删除字典
        /// </summary>
        private  void ClearZiDian()
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
                string wenjianname =$"{lujing}\\{model.Ma}_{pass}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";

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
            string path = string.Format(@"{0}\{1}\{2}\{3}", Path.GetFullPath("."), "Mes",shebeiname, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path; ;
        }

        private void  ClearHuanCun()
        {
            try
            {
              
                if (AddNeiRong.Count > 0)
                {
                    List<string> keys = new List<string>();
                    foreach (var item in AddNeiRong.Keys)
                    {                      
                        RiJiMesModel model = AddNeiRong[item];
                        if (model.IsWanCeng==3)
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
   
}
