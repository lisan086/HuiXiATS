using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATSJianMianJK.Log;
using ATSJianMianJK.Mes;
using ATSJianMianJK.XiTong.Frm.FM;
using ATSJianMianJK.XiTong.Model;
using CommLei.JiChuLei;
using Common.JiChuLei;
using JieMianLei.FuFrom.KJ;
using SSheBei.Model;
using SSheBei.ZongKongZhi;

namespace ATSJianMianJK.GongNengLei
{
    /// <summary>
    /// 集合设备
    /// </summary>
    public class JiHeSheBei
    {
        /// <summary>
        /// 改变数据类型
        /// </summary>
        public event Action<int,IOType,string, bool,string> GaiBianEvent;
        /// <summary>
        /// 集合数据
        /// </summary>
        public JiHeData JiHeData { get; set; }

        public XieBuZhou XieBuZhou { get; set; }

        public QuXiaoModel QuXiaoModel { get; set; } = new QuXiaoModel();
        /// <summary>
        /// 线程总开关
        /// </summary>
        private bool ZongKaiGuan = true;
    
        /// <summary>
        /// 工作开关
        /// </summary>
        private bool GongZuoWork = false;
        #region 单利
        private readonly static object _DuiXiang = new object();

        private static JiHeSheBei _LogTxt = null;



        private JiHeSheBei()
        {
            IniData();
        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static JiHeSheBei Cerate()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new JiHeSheBei();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

        private void IniData()
        {
            Thread thread = new Thread(Work);
            thread.IsBackground = true;
            thread.DisableComObjectEagerCleanup();
            thread.Start();
            Thread threadd = new Thread(DongZuo);
            threadd.IsBackground = true;
            threadd.DisableComObjectEagerCleanup();
            threadd.Start();
        }
        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            GongZuoWork = true;
        }
      
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            ZongKaiGuan = false;
            XieBuZhou.ZongKaiGuan = false;
            ZongSheBeiKongZhi.Cerate().Close();
        }

        private void Work()
        {
            DateTime JiShiTime = DateTime.Now;
            while (ZongKaiGuan)
            {
                if (GongZuoWork == false)
                {
                    Thread.Sleep(50);
                    continue;
                }
              
                try
                {
                    List<DataModel> lis = ZongSheBeiKongZhi.Cerate().GetShuJu();
                    foreach (var item in lis)
                    {
                        DataModel dataModel = item;
                        if (JiHeData!=null)
                        {
                            JiHeData.ShuXinData(dataModel.JiCunQiModels);
                        }                      
                    }
                  
                }
                catch (Exception ex)
                {
                    RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, $"设备集合:发生错误{ex}", -1);
                }
                Thread.Sleep(5);
            }
        }

        private void DongZuo()
        {
            DateTime dateTime = DateTime.Now;
            while (ZongKaiGuan)
            {
                if (GongZuoWork == false)
                {
                    Thread.Sleep(30);
                    continue;
                }
                if (JiHeData == null)
                {
                    Thread.Sleep(50);
                    continue;
                }
               
                try
                {
                    Parallel.ForEach(JiHeData.ZhuagTaiTongDaoKey, (x) => {
                       
                        try
                        {
                            TDZhuangTaiModel model = JiHeData.TDZhaungTai[x];
                            bool keyiyuanxi = JiHeData.GetIOBool(x, IOType.IOKeYiYunXin, true);
                            model.IsKeYiYunXing = keyiyuanxi;

                        }
                        catch 
                        {

                           
                        }
                        try
                        {
                            List<DuIOCanShuModel> duIOs = JiHeData.TDIOShuJu[x];
                            for (int i = 0; i < duIOs.Count; i++)
                            {
                                if (duIOs[i].IsKeYiEvent())
                                {
                                    bool xianzai = JiHeData.GetIOBool(x, duIOs[i].Name,false);
                                    if (duIOs[i].ShangYiCiValue != xianzai)
                                    {
                                        duIOs[i].ShangYiCiValue = xianzai;
                                        duIOs[i].DateTime = DateTime.Now;
                                        duIOs[i].GaiBian = 1;
                                    }
                                    else
                                    {
                                        bool iskeyichufa = duIOs[i].IsKeYiChuFa();
                                        if (iskeyichufa)
                                        {
                                            ChuFaGaiBianEvent(x, duIOs[i].Type, duIOs[i].Name, xianzai, duIOs[i].CanShu);
                                        }
                                    }
                                }
                               
                            }
                        }
                        catch
                        {


                        }
                        try
                        {
                            for (int i = 0; i < JiHeData.TDZhaungTai[x].ZiDongXieSates.Count; i++)
                            {
                                if (JiHeData.TDZhaungTai[x].ZiDongXieSates[i].IsMaZuXie())
                                {
                                    XieChengGong(x, JiHeData.TDZhaungTai[x].ZiDongXieSates[i].LisXies);
                                }
                            }                         
                        }
                        catch 
                        {

                          
                        }
                        
                    });
                   
                  
                }
                catch
                {


                }

                if ((DateTime.Now- dateTime).TotalSeconds>=60*10)
                {
                    QingLiNeiCunLei.QingLiHuanCun();
                  
                    dateTime = DateTime.Now;
                }
                Thread.Sleep(50);
            }
        }

        private void ChuFaGaiBianEvent(int tdid, IOType iOType, string duname, bool state,string canshu)
        {
            if (GaiBianEvent!=null)
            {
                GaiBianEvent(tdid, iOType, duname,state,canshu);
            }
        }

        #region 配置的写
        /// <summary>
        /// 写参数
        /// </summary>
        /// <param name="jicunqiname"></param>
        /// <param name="zhi"></param>
        public void XieShuJu(int tdid, XieRuMolde xieRuMolde)
        {
            if (JiHeData.TDZhaungTai.ContainsKey(tdid))
            {
                if (JiHeData.TDZhaungTai[tdid].IsKeYiYunXing==false)
                {
                    RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, "设备处于不可以运行状态,请维护", -1);
                    return;
                }
            }
           
            ZongSheBeiKongZhi.Cerate().XieCanShu(xieRuMolde);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="lismodel"></param>
        public void XieShuJu(int tdid, List<XieRuMolde> lismodel)
        {
            if (JiHeData.TDZhaungTai.ContainsKey(tdid))
            {
                if (JiHeData.TDZhaungTai[tdid].IsKeYiYunXing == false)
                {
                    RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, "设备处于不可以运行状态,请维护", -1);
                    return;
                }
            }
            ZongSheBeiKongZhi.Cerate().XieCanShu(lismodel);
        }
        /// <summary>
        /// 返回是否写完成
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JiaoYanJieGuoModel GetXieJieGuo(XieRuMolde model)
        {

            JiaoYanJieGuoModel jieguo = ZongSheBeiKongZhi.Cerate().GetIsChengGong(model);
            return jieguo;
        }


        /// <summary>
        /// 0表示数据为空不能写 1表示写成功 2是表示写失败 3表示写的类为空
        /// </summary>
        /// <param name="lsixie"></param>
        /// <returns></returns>
        public int XieChengGong(int tdid, List<XieModel> lsixie,bool isbuyueguo=true)
        {
            if (isbuyueguo)
            {
                if (JiHeData.TDZhaungTai.ContainsKey(tdid))
                {
                    if (JiHeData.TDZhaungTai[tdid].IsKeYiYunXing == false)
                    {
                        RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, "设备处于不可以运行状态,请维护", -1);
                        return 3;
                    }
                }
            }
            if (XieBuZhou==null)
            {
                return 3;
            }
            return XieBuZhou.XieChengGong(tdid,lsixie, QuXiaoModel);
        }

        /// <summary>
        /// 0表示数据为空不能写 1表示写成功 2是表示写失败 3表示写的类为空
        /// </summary>
        /// <param name="lsixie"></param>
        /// <returns></returns>
        public int XieChengGong(int tdid, List<XieModel> lsixie,Action<int,int,int ,bool> fanhui)
        {
            if (JiHeData.TDZhaungTai.ContainsKey(tdid))
            {
                if (JiHeData.TDZhaungTai[tdid].IsKeYiYunXing == false)
                {
                    RiJiLog.Cerate().Add(RiJiEnum.TDXieJiLu, "设备处于不可以运行状态,请维护", -1);
                    return 3;
                }
            }
            if (XieBuZhou == null)
            {
                return 3;
            }
            return XieBuZhou.XieChengGong(tdid, lsixie, QuXiaoModel, fanhui);
        }

        /// <summary>
        /// 0表示数据为空不能写 1表示写成功 2是表示写失败 3表示写的类为空
        /// </summary>
        /// <param name="lsixie"></param>
        /// <returns></returns>
        public int XieChengGong(int tdid, string xiepeizhiname, bool isbuyueguo = true)
        {
          
            XieSateModel xielie =JiHeData.GetXieModel(tdid,xiepeizhiname);
            if (xielie != null)
            {
               return  XieChengGong(xielie.TDID, xielie.LisXies);

            }
            return 0;
        }

        /// <summary>
        /// 写参数
        /// </summary>
        /// <param name="jicunqiname"></param>
        /// <param name="zhi"></param>
        public void XieShuJu( string jicunqiname,int shebeiid, object zhi)
        {
          
            XieRuMolde model = new XieRuMolde();
            model.JiCunQiWeiYiBiaoShi = jicunqiname;
            model.Zhi = zhi;
            model.SheBeiID = shebeiid;
         
            ZongSheBeiKongZhi.Cerate().XieCanShu(model);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="lismodel"></param>
        public void XieShuJu(List<XieRuMolde> lismodel)
        {
          
            ZongSheBeiKongZhi.Cerate().XieCanShu(lismodel);
        }


      
        #endregion




    }

    /// <summary>
    /// 取消参数
    /// </summary>
    public class QuXiaoModel
    {
        /// <summary>
        /// true 表示取消
        /// </summary>
        public bool QuXiao { get; set; } = false;

       
    }


    /// <summary>
    /// 写步骤的类
    /// </summary>
    public class XieBuZhou
    {
        /// <summary>
        /// 线程总开关
        /// </summary>
        public bool ZongKaiGuan = true;

        /// <summary>
        /// 0表示数据为空不能写 1表示写成功 2是表示写失败 3表示写的类为空
        /// </summary>
        /// <param name="lsixie"></param>
        /// <returns></returns>
        public virtual int XieChengGong(int tdid, List<XieModel> lsixie, QuXiaoModel quxiaocanshu, Action<int, int, int,bool> fanhui = null)
        {
            if (lsixie == null || lsixie.Count == 0)
            {            
                RiJiLog.Cerate().Add(RiJiEnum.XiTongXie, "写空的数据", tdid);
                return 0;
            }
            if (lsixie.Count > 0)
            {
                lsixie.Sort((x, y) =>
                {
                    if (x.ShunXu > y.ShunXu)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                });
            }
            StringBuilder sb = new StringBuilder();
            for (int dd = 0; dd < lsixie.Count; dd++)
            {

                XieModel xiemodel = lsixie[dd];
                if (quxiaocanshu.QuXiao)
                {
                    quxiaocanshu.QuXiao = false;
                    RiJiLog.Cerate().Add(RiJiEnum.XiTongXie, $"取消写数据:{sb.ToString()}", tdid);              
                    return 2;
                }           
                {
                    if (xiemodel.GenJuType == GenJuType.FuZhi)
                    {
                        if (xiemodel.FuZhiJiChu.Count > 0)
                        {
                            for (int c = 0; c < xiemodel.FuZhiJiChu.Count; c++)
                            {
                                if (xiemodel.FuZhiJiChu[c].IsPingBi != 1)
                                {
                                    if (fanhui != null)
                                    {
                                        fanhui(xiemodel.ShunXu, xiemodel.FuZhiJiChu[c].ShunXu, 1,true);
                                    }
                                    string jieguo = ZhiXingXie(xiemodel.FuZhiJiChu[c], tdid);
                                    sb.AppendLine(jieguo);
                                    if (fanhui != null)
                                    {
                                        fanhui(xiemodel.ShunXu, xiemodel.FuZhiJiChu[c].ShunXu, 2, true);
                                    }
                                }
                            }
                        }

                    }
                    else if (xiemodel.GenJuType == GenJuType.Wait)
                    {                     
                        if (fanhui != null)
                        {
                            fanhui(xiemodel.ShunXu, -1, 1, true);
                        }
                        float shijians = ChangYong.TryFloat(xiemodel.DengDaiTime, 0);
                        DateTime shijiann = DateTime.Now;
                        sb.AppendLine($"写入等待:{shijians}s");
                        for (; ZongKaiGuan;)
                        {
                            if ((DateTime.Now - shijiann).TotalMilliseconds >= shijians * 1000)
                            {
                                break;
                            }
                            if (quxiaocanshu.QuXiao)
                            {
                                quxiaocanshu.QuXiao = false;
                                return 2;
                            }
                            Thread.Sleep(1);
                        }
                        if (fanhui != null)
                        {
                            fanhui(xiemodel.ShunXu, -1, 2, true);
                        }
                    }
                    else if (xiemodel.GenJuType == GenJuType.If)
                    {
                        List<bool> zhensd = new List<bool>();
                        StringBuilder xinbuyf = new StringBuilder();
                        for (int c = 0; c < xiemodel.TiaoJianJiChu.Count; c++)
                        {
                            if (xiemodel.TiaoJianJiChu[c].IsPingBi != 1)
                            {
                                if (fanhui != null)
                                {
                                    fanhui(xiemodel.ShunXu, xiemodel.TiaoJianJiChu[c].ShunXu, 3, true);
                                }
                                string xiezhi2 = "";
                                bool ismanzu = PanDuanManZu(xiemodel.TiaoJianJiChu[c], tdid, out xiezhi2); xinbuyf.AppendLine($"if判断结果:{xiezhi2}");
                                zhensd.Add(ismanzu);
                                if (fanhui != null)
                                {
                                    fanhui(xiemodel.ShunXu, xiemodel.TiaoJianJiChu[c].ShunXu, 4,ismanzu);
                                }
                            }
                        }
                        bool zhenmanzu = false;
                        if (xiemodel.IfPanDuanType == 1)
                        {
                            bool manzus = true;
                            for (int i = 0; i < zhensd.Count; i++)
                            {
                                if (zhensd[i]==false)
                                {
                                    manzus = false;
                                    break;
                                }
                            }
                            if (zhensd.Count>0)
                            {
                                if (manzus)
                                {
                                    zhenmanzu = true;
                                }
                            }
                        }
                        else
                        {
                            bool manzus = false;
                            for (int i = 0; i < zhensd.Count; i++)
                            {
                                if (zhensd[i])
                                {
                                    manzus = true;
                                    break;
                                }
                            }
                            if (zhensd.Count > 0)
                            {
                                if (manzus)
                                {
                                    zhenmanzu = true;
                                }
                            }
                        }

                        if (zhenmanzu)
                        {
                            sb.AppendLine($"if判断满足:{zhenmanzu}");
                            for (int c = 0; c < xiemodel.FuZhiJiChu.Count; c++)
                            {
                                if (xiemodel.FuZhiJiChu[c].IsPingBi!=1)
                                {
                                    if (fanhui != null)
                                    {
                                        fanhui(xiemodel.ShunXu, xiemodel.FuZhiJiChu[c].ShunXu, 1, true);
                                    }
                                    string jieguo = ZhiXingXie(xiemodel.FuZhiJiChu[c], tdid);
                                    sb.AppendLine(jieguo);
                                    if (fanhui != null)
                                    {
                                        fanhui(xiemodel.ShunXu, xiemodel.FuZhiJiChu[c].ShunXu, 2, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                           
                            sb.AppendLine($"if判断不满足:{zhenmanzu}");

                        }

                    }
                    else if (xiemodel.GenJuType == GenJuType.DengDaiPanDuan)
                    {
                        string xiezhi2 = "";
                        float shijians = ChangYong.TryFloat(xiemodel.DengDaiTime, 0);
                        DateTime shijiann = DateTime.Now;
                        for (; ZongKaiGuan;)
                        {
                            List<bool> zhensd = new List<bool>();                            
                            for (int c = 0; c < xiemodel.TiaoJianJiChu.Count; c++)
                            {
                                if (xiemodel.TiaoJianJiChu[c].IsPingBi != 1)
                                {
                                    if (fanhui != null)
                                    {
                                        fanhui(xiemodel.ShunXu, xiemodel.TiaoJianJiChu[c].ShunXu, 3, true);
                                    }                                   
                                    bool ismanzu1 = PanDuanManZu(xiemodel.TiaoJianJiChu[c], tdid, out xiezhi2);                                    
                                    zhensd.Add(ismanzu1);
                                    if (fanhui != null)
                                    {
                                        fanhui(xiemodel.ShunXu, xiemodel.TiaoJianJiChu[c].ShunXu, 4,ismanzu1);
                                    }
                                }
                            }
                            if (zhensd.Count==0)
                            {
                                sb.AppendLine($"没有需要判读的数据 {(DateTime.Now - shijiann).TotalSeconds}s");
                                break;
                            }
                            bool zhenmanzu = false;
                            if (xiemodel.IfPanDuanType == 1)
                            {
                                bool manzus = true;
                                for (int i = 0; i < zhensd.Count; i++)
                                {
                                    if (zhensd[i] == false)
                                    {
                                        manzus = false;
                                        break;
                                    }
                                }
                                if (zhensd.Count > 0)
                                {
                                    if (manzus)
                                    {
                                        zhenmanzu = true;
                                    }
                                }
                            }
                            else
                            {
                                bool manzus = false;
                                for (int i = 0; i < zhensd.Count; i++)
                                {
                                    if (zhensd[i])
                                    {
                                        manzus = true;
                                        break;
                                    }
                                }
                                if (zhensd.Count > 0)
                                {
                                    if (manzus)
                                    {
                                        zhenmanzu = true;
                                    }
                                }
                            }
                            if (zhenmanzu)
                            {
                                sb.AppendLine($"等待判断满足:{zhenmanzu}");
                                for (int c = 0; c < xiemodel.FuZhiJiChu.Count; c++)
                                {
                                    if (xiemodel.FuZhiJiChu[c].IsPingBi != 1)
                                    {
                                        if (fanhui != null)
                                        {
                                            fanhui(xiemodel.ShunXu, xiemodel.FuZhiJiChu[c].ShunXu, 1, true);
                                        }
                                        string jieguo = ZhiXingXie(xiemodel.FuZhiJiChu[c], tdid);
                                        sb.AppendLine(jieguo);
                                        if (fanhui != null)
                                        {
                                            fanhui(xiemodel.ShunXu, xiemodel.FuZhiJiChu[c].ShunXu, 2,true);
                                        }
                                    }
                                }
                                break;
                            }
                         
                            if (quxiaocanshu.QuXiao)
                            {
                                quxiaocanshu.QuXiao = false;
                                return 2;
                            }

                            if ((DateTime.Now - shijiann).TotalMilliseconds >= shijians * 1000)
                            {
                                sb.AppendLine($"等待不满足超时退出 {(DateTime.Now - shijiann).TotalSeconds}s");
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        sb.AppendLine($"等待满足退出 时间为 {(DateTime.Now - shijiann).TotalSeconds}s");

                    }
                   
                }

            }

            RiJiLog.Cerate().Add(RiJiEnum.XiTongXie, sb.ToString(), tdid);
            return 1;
        }

        private string ZhiXingXie(JiChuXieDYModel jcmodel, int tdid)
        {
            StringBuilder sb = new StringBuilder();
            ZblLeiXing leiXing = jcmodel.ZBLModel.ZLeiXing;
            if (leiXing == ZblLeiXing.HuanCunLiang)
            {
                object canshu = GetYouBianObj(jcmodel.YBLModel, tdid)[0];
                HuanCunLei.Cerate().SetHuanCun(tdid, jcmodel.ZBLModel.ZBianLiangName, canshu);
                sb.AppendLine($"执行缓存赋值:{tdid} {jcmodel.ZBLModel.ZBianLiangName} 值:{canshu}");
            }
            else if (leiXing == ZblLeiXing.XieJC)
            {
                XieRuMolde model = GetXieModel(jcmodel.ZBLModel, jcmodel.YBLModel, tdid);
                JiHeSheBei.Cerate().XieShuJu(tdid, model);
                sb.AppendLine($"执行写名称:{model.SheBeiID} {model.JiCunQiWeiYiBiaoShi} 参数:{model.Zhi}");
            }

            return sb.ToString();
        }

        private bool PanDuanManZu(JiChuXieDYModel xiemodel, int tdid,out string msg)
        {       
            bool zhen=false;         
            ZhongJianType zhongJianType = xiemodel.ZhongJianType;
            object zcanshu = GetZouBianObj(xiemodel.ZBLModel,tdid);
            List<object> youbians = GetYouBianObj(xiemodel.YBLModel, tdid);        
            switch (zhongJianType)
            {
                case ZhongJianType.BaoHan:
                    {
                        string[] jiexie = youbians[0].ToString().Split('#');
                        for (int i = 0; i < jiexie.Length; i++)
                        {
                            if (zcanshu.ToString().Contains(jiexie[i]))
                            {
                                zhen = true;
                                break;
                            }
                        }
                    }
                    break;
                case ZhongJianType.DengYu:
                    {
                        string[] jiexie = youbians[0].ToString().Split('#');
                        for (int i = 0; i < jiexie.Length; i++)
                        {
                            if (zcanshu.ToString().Equals(jiexie[i]))
                            {
                                zhen = true;
                                break;
                            }
                        }
                    }
                    break;
                case ZhongJianType.DaYu:
                    {
                        float zuobian = ChangYong.TryFloat(zcanshu, 0f);
                        float youbian = ChangYong.TryFloat(youbians[0], 1f);
                        if (zuobian > youbian)
                        {
                            zhen = true;
                        }
                    }
                    break;
                case ZhongJianType.DaYuDengYu:
                    {
                        float zuobian = ChangYong.TryFloat(zcanshu, 0f);
                        float youbian = ChangYong.TryFloat(youbians[0], 1f);
                        if (zuobian >= youbian)
                        {
                            zhen = true;
                        }
                    }
                    break;
                case ZhongJianType.XiaoYu:
                    {
                        float zuobian = ChangYong.TryFloat(zcanshu, 0f);
                        float youbian = ChangYong.TryFloat(youbians[0], 1f);
                        if (zuobian < youbian)
                        {
                            zhen = true;
                        }
                    }
                    break;
                case ZhongJianType.XiaoYuDengYu:
                    {
                        float zuobian = ChangYong.TryFloat(zcanshu, 0f);
                        float youbian = ChangYong.TryFloat(youbians[0], 1f);
                        if (zuobian <= youbian)
                        {
                            zhen = true;
                        }
                    }
                    break;
                case ZhongJianType.LiangZheZhiJian:
                    {
                        float zuobian = ChangYong.TryFloat(zcanshu, 0f);
                        float youbian1 = ChangYong.TryFloat(youbians[0], 1f);
                        float youbian2 = ChangYong.TryFloat(youbians[1], 1f);
                        if (zuobian <= youbian2&& zuobian>=youbian1)
                        {
                            zhen = true;
                        }
                    }
                    break;
               
                default:
                    break;
            }
            msg = $"左边值:{zcanshu} 右边值:{youbians[0]} {youbians[0]} 中间判断:{zhongJianType}";
            return zhen;
        }

        private XieRuMolde GetXieModel(ZBLModel zbianliang, YBLModel ybianliang, int tdid)
        {
            XieRuMolde model = new XieRuMolde();
            model.JiCunQiWeiYiBiaoShi = zbianliang.ZBianLiangName;
            model.SheBeiID = zbianliang.SheBeiID;
            object canshu = GetYouBianObj(ybianliang, tdid)[0];          
            model.Zhi = canshu;
            return model;
        }

        private List<object> GetYouBianObj(YBLModel xiemodel,int tdid)
        {
            List<object> canshu = new List<object>();
            canshu.Add("");
            canshu.Add("");
            if (xiemodel.ZLeiXing == YblLeiXing.ChangLiangZhi)
            {
                canshu[0] = xiemodel.YouCanShu;
            }
            else if (xiemodel.ZLeiXing == YblLeiXing.HuanCunLiang)
            {
                canshu[0] = HuanCunLei.Cerate().GetHuanCun(tdid, xiemodel.YouCanShu, "");
            }
            else if (xiemodel.ZLeiXing == YblLeiXing.DuJC)
            {
                object xiezhi = JiHeSheBei.Cerate().JiHeData.GetXieZhi(xiemodel.YouCanShu, xiemodel.SheBeiID, "");
                canshu[0] = xiezhi;
            }
            else if(xiemodel.ZLeiXing == YblLeiXing.LiangGeZhi)
            {
                string[] jiege = xiemodel.YouCanShu.Split('#');
                canshu[0] = jiege[0];
                if (jiege.Length>1)
                {
                    canshu[1] = jiege[1];
                }
            }
            return canshu;
        }


        private object GetZouBianObj(ZBLModel xiemodel, int tdid)
        {
            ZblLeiXing leiXing = xiemodel.ZLeiXing;
         
            object zcanshu = "";
            if (leiXing == ZblLeiXing.HuanCunLiang)
            {
                zcanshu = HuanCunLei.Cerate().GetHuanCun(tdid, xiemodel.ZBianLiangName, ".");
            }
            else if (leiXing == ZblLeiXing.DuJC)
            {
                zcanshu = JiHeSheBei.Cerate().JiHeData.GetXieZhi(xiemodel.ZBianLiangName, xiemodel.SheBeiID, "");
            }
            else if (leiXing == ZblLeiXing.DuIOKuai)
            {
                zcanshu = JiHeSheBei.Cerate().JiHeData.GetIOBool(tdid, xiemodel.ZBianLiangName, false);
            }
            else if (leiXing == ZblLeiXing.DuXieJCRec|| leiXing==ZblLeiXing.XieJC)
            {
               
                XieRuMolde xieRuMolde =new XieRuMolde();
                xieRuMolde.SheBeiID = xiemodel.SheBeiID;
                xieRuMolde.JiCunQiWeiYiBiaoShi = xiemodel.ZBianLiangName;
                xieRuMolde.Zhi = "";
                JiHeSheBei.Cerate().XieShuJu(tdid, xieRuMolde);
                for (; ;)
                {
                    Thread.Sleep(2);
                    JiaoYanJieGuoModel jieguo = ZongSheBeiKongZhi.Cerate().GetIsChengGong(xieRuMolde);
                    if (jieguo.IsZuiZhongJieGuo != JieGuoType.JingXingZhong)
                    {
                        if (jieguo.IsZuiZhongJieGuo == JieGuoType.BuKeKaoJieGuo)
                        {
                            zcanshu = "";
                            break;
                        }
                        else if (jieguo.IsZuiZhongJieGuo == JieGuoType.MeiZhaoDaoJiGuo)
                        {
                            zcanshu = "";
                            break;
                        }
                        else if (jieguo.IsZuiZhongJieGuo == JieGuoType.ShiBaiJiGuo)
                        {
                            zcanshu = "";
                            break;
                        }
                        else if (jieguo.IsZuiZhongJieGuo == JieGuoType.ChengGongJiGuo)
                        {
                            zcanshu = jieguo.Value;
                            break;

                        }

                    }
         
                }
            }
            return zcanshu;
        }
    }

  


}
