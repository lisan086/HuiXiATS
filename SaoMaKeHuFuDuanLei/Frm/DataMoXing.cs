using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using YiBanSaoMaQi.Model;
using CommLei.DataChuLi;
using CommLei.GongYeJieHe;
using Common.DataChuLi;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using SaoMaKeHuFuDuanLei.Frm;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;


namespace YiBanSaoMaQi.Frm
{
    /// <summary>
    /// 模型数据
    /// </summary>
    public class DataMoXing
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public int SheBeiID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string SheBeiName { get; set; } = "";
        /// <summary>
        /// 读寄存器
        /// </summary>
        public List<JiCunQiModel> LisDu = new List<JiCunQiModel>();
        /// <summary>
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> LisXie = new List<JiCunQiModel>();

        /// <summary>
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> LisDuXie = new List<JiCunQiModel>();

        /// <summary>
        /// 设备
        /// </summary>
        public List<IPFuWuPeiModel> LisSheBei = new List<IPFuWuPeiModel>();

        public List<SaoMaMoXingModel> YongYuBaoCun { get; set; } = new List<SaoMaMoXingModel>();
        public Dictionary<int, FanXingJiHeLei<SaoMaMoXingModel>> JianCeXuHao = new Dictionary<int, FanXingJiHeLei<SaoMaMoXingModel>>();
        /// <summary>
        /// 写标识的对应 key表示寄存器的唯一表示
        /// </summary>
        public Dictionary<string, CunModel> JiLu = new Dictionary<string, CunModel>();

        private List<string> KeyS = new List<string>();

        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            LisDu.Clear();
            LisXie.Clear();
            JiLu.Clear();

            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<IPFuWuPeiModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<IPFuWuPeiModel>();
            }
            LisSheBei.Sort((x,y) => {
                if (x.PaiXu > y.PaiXu)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
            for (int c = 0; c < LisSheBei.Count; c++)
            {
                IPFuWuPeiModel iPFuWuPeiModel = LisSheBei[c];
                if (iPFuWuPeiModel.IsFuWuDuan != 1)
                {
                    int xuhao = iPFuWuPeiModel.PaiXu;
                    if (iPFuWuPeiModel.IsQingQiu == 1)
                    {
                        if (JianCeXuHao.ContainsKey(xuhao) == false)
                        {
                            JianCeXuHao.Add(xuhao, new FanXingJiHeLei<SaoMaMoXingModel>());
                        }
                    }
                    if (iPFuWuPeiModel.IsFuWuDuan == 2)
                    {
                        List<string> lis = ChangYong.MeiJuLisName(typeof(CunType));
                        for (int i = 0; i < lis.Count; i++)
                        {
                            JiCunQiModel model = new JiCunQiModel();
                            model.SheBeiID = SheBeiID;
                            if (lis[i].ToLower().Contains("wenjian") == false)
                            {
                                if (lis[i].ToLower().Contains("du"))
                                {
                                    model.WeiYiBiaoShi = $"{iPFuWuPeiModel.ZhanDianName}_{lis[i]}";
                                    model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                                    model.DuXie = 1;
                                    LisDu.Add(model);
                                    LisDuXie.Add(model);
                                }
                                else
                                {

                                    model.WeiYiBiaoShi = $"{iPFuWuPeiModel.ZhanDianName}_{lis[i]}";

                                    model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                                    model.DuXie = 2;
                                    LisXie.Add(model);
                                    LisDuXie.Add(model);
                                }
                                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                {
                                    CunModel cunModel = new CunModel();
                                    cunModel.ZongSheBeiId = iPFuWuPeiModel.IsTongYiGe;
                                    cunModel.IsDu = ChangYong.GetMeiJuZhi<CunType>(lis[i]);
                                    cunModel.JiCunQi = model;
                                    JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                }
                            }
                        }
                    }
                    else if (iPFuWuPeiModel.IsFuWuDuan == 4)
                    {
                        List<string> lis = ChangYong.MeiJuLisName(typeof(CunType));
                        for (int i = 0; i < lis.Count; i++)
                        {
                            JiCunQiModel model = new JiCunQiModel();
                            model.SheBeiID = SheBeiID;
                            if (lis[i].ToLower().Contains("wenjian"))
                            {
                                if (lis[i].ToLower().Contains("du"))
                                {

                                    model.WeiYiBiaoShi = $"{iPFuWuPeiModel.ZhanDianName}_{lis[i]}";

                                    model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                                    model.DuXie = 1;
                                    LisDu.Add(model);
                                    LisDuXie.Add(model);
                                }
                                else
                                {

                                    model.WeiYiBiaoShi = $"{iPFuWuPeiModel.ZhanDianName}_{lis[i]}";

                                    model.MiaoSu = ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(lis[i]));
                                    model.DuXie = 2;
                                    LisXie.Add(model);
                                    LisDuXie.Add(model);
                                }
                                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                                {
                                    CunModel cunModel = new CunModel();
                                    cunModel.ZongSheBeiId = iPFuWuPeiModel.IsTongYiGe;
                                    cunModel.IsDu = ChangYong.GetMeiJuZhi<CunType>(lis[i]);
                                    cunModel.JiCunQi = model;
                                    JiLu.Add(model.WeiYiBiaoShi, cunModel);
                                }
                            }
                        }
                    }
                }
            }
           
            KeyS = JiLu.Keys.ToList();
        }

   
        public void SetState(int zongid,bool state)
        {
       
           
        }

        public void SetJiCunQiValue(CunModel xianmodel, string shuju)
        {
            if (JiLu.ContainsKey(xianmodel.JiCunQi.WeiYiBiaoShi))
            {
                CunModel model = JiLu[xianmodel.JiCunQi.WeiYiBiaoShi];
                model.JiCunQi.IsKeKao = true;
                model.JiCunQi.Value = shuju;

            }

        }

        public bool SetDuJiCunQiValue(int zongid,List<string> shuju)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                try
                {
                    if (JiLu.ContainsKey(KeyS[i]))
                    {
                        if (JiLu[KeyS[i]].ZongSheBeiId == zongid)
                        {
                            if (JiLu[KeyS[i]].IsDu==CunType.DuWenJianState)
                            {
                                JiLu[KeyS[i]].IsZhengZaiCe = 1;
                                if (string.IsNullOrEmpty(JiLu[KeyS[i]].JiCunQi.Value.ToString()))
                                {
                                    JiLu[KeyS[i]].JiCunQi.Value = shuju;
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                catch
                {


                }
            }

            return false;
        }

        public void SetDuState(int zongid)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                try
                {
                    if (JiLu.ContainsKey(KeyS[i]))
                    {
                        if (JiLu[KeyS[i]].ZongSheBeiId == zongid)
                        {
                            if (JiLu[KeyS[i]].IsDu == CunType.DuWenJianState)
                            {
                                JiLu[KeyS[i]].JiCunQi.Value = "";
                            }
                        }
                    }
                }
                catch
                {


                }
            }

        }
        public void SetZhuangTaiZhi(CunModel models,int state)
        {
            if (JiLu.ContainsKey(models.JiCunQi.WeiYiBiaoShi))
            {
                CunModel model = JiLu[models.JiCunQi.WeiYiBiaoShi];           
                model.IsZhengZaiCe = state;
                if (state == 0)
                {
                    model.JiCunQi.Value = "";
                }
            }
          

        }

        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CunModel IsChengGong(string weiyibiaoshi)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel model = JiLu[weiyibiaoshi];             
                return model;
            }


            return null;
        }
        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CunModel GetModel(JiCunQiModel weiyibiaoshi)
        {
            if (JiLu.ContainsKey(weiyibiaoshi.WeiYiBiaoShi))
            {
                CunModel model = JiLu[weiyibiaoshi.WeiYiBiaoShi];
                return model.FuZhi();
            }


            return null;
        }

        public IPFuWuPeiModel GetSheBei(CunModel model)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].IsTongYiGe==model.ZongSheBeiId)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }
        public List<SaoMaMoXingModel> GetShuJu()
        {
            List<SaoMaMoXingModel> lis = new List<SaoMaMoXingModel>();
            int count = YongYuBaoCun.Count;
            
            for (int i = 0; i < count; i++)
            {
                try
                {
                    if (YongYuBaoCun[i].JieSu == 0)
                    {
                        lis.Add(YongYuBaoCun[i]);
                    }
                }
                catch 
                {

                   
                }
            }
            return lis;
        }

        public List<CunModel> GetCunModel(int shebeiid)
        {
            List<CunModel> lis = new List<CunModel>();
            int count = YongYuBaoCun.Count;

            for (int i = 0; i < KeyS.Count; i++)
            {
                try
                {
                    if (JiLu.ContainsKey(KeyS[i]))
                    {
                        if (JiLu[KeyS[i]].ZongSheBeiId == shebeiid)
                        {
                            lis.Add(JiLu[KeyS[i]]);
                        }
                    }
                }
                catch
                {


                }
            }
            return lis;
        }

        public void QingLi()
        {
            if (YongYuBaoCun.Count>500)
            {
                int count = 300;
                for (int i = 0; i < count; i++)
                {
                    for (int c = 0; c < YongYuBaoCun.Count; c++)
                    {
                        if (YongYuBaoCun[c].JieSu!=0)
                        {
                            YongYuBaoCun.RemoveAt(c);
                            break;
                        }
                    }
                }
            }
        }
    }




    public  class FangWenFuWuDuan
    {
        private PeiZhiLei PeiZhiLei { get; set; }
        public event Action<string> MsgEvent;
        private List<IPFuWuPeiModel> LisModels = new List<IPFuWuPeiModel>();
        private bool IsFuWuQi { get; set; } = false;
        private FuWuQi FuWuQi;
        private List<KeHuDuan> KeHuDuan=new List<KeHuDuan>();

        #region 单例
        private static FangWenFuWuDuan _LogTxt = null;

        private readonly static object _DuiXiang = new object();
        private FangWenFuWuDuan()
        {


        }
        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static FangWenFuWuDuan Ceratei()
        {
            if (_LogTxt == null)
            {
                lock (_DuiXiang)
                {
                    if (_LogTxt == null)
                    {
                        _LogTxt = new FangWenFuWuDuan();
                    }
                }
            }
            return _LogTxt;
        }
        #endregion

     
        /// <summary>
        /// 打开方法 返回true表示成功，返回false失败
        /// </summary>
        public void Open(List<IPFuWuPeiModel> models,PeiZhiLei peiZhiLei)
        {
            PeiZhiLei = peiZhiLei;
            LisModels = models;
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].IsFuWuDuan==1)
                {

                    FuWuQi = new FuWuQi();
                    FuWuQi.MsgEvent += XieLog;
                    FuWuQi.PeiZhiLei = peiZhiLei;
                    FuWuQi.Open(models);
                    break;
                }
            }
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].IsFuWuDuan == 2|| models[i].IsFuWuDuan == 4)
                {
                    KeHuDuan KeHuDuan1 = new KeHuDuan();
                    KeHuDuan1.MsgEvent += XieLog;
                    KeHuDuan1.Open(models[i], peiZhiLei);
                    KeHuDuan.Add(KeHuDuan1);
                }
            }

        }


        /// <summary>
        /// 关闭的方法
        /// </summary>
        public void Close()
        {
            foreach (var item in KeHuDuan)
            {
                item.Close();
            }
            if (FuWuQi != null)
            {
                FuWuQi.Close();
            }



        }

        /// <summary>
        /// 发送数据，需要传ip，端口，消息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="duankou"></param>
        /// <param name="msg"></param>
        public void SendShuJuFuWu(List<SaoMaShuJuModel> shuju, int leixing, string biaoshi, bool ishege,string zhanhao)
        {

            if (string.IsNullOrEmpty(zhanhao))
            {
                if (KeHuDuan.Count>0)
                {
                    KeHuDuan[0].SendShuJuFuWu(shuju, leixing, biaoshi, ishege);
                }
              
            }
            else
            {
                for (int i = 0; i < KeHuDuan.Count; i++)
                {
                    if (KeHuDuan[i].ZhanHao == zhanhao)
                    {
                        KeHuDuan[i].SendShuJuFuWu(shuju, leixing, biaoshi, ishege);
                        break;
                    }
                }

            }


        }

        public SengModel GetKeHuDuanShuJu(string zhanhao)
        {
            for (int i = 0; i < KeHuDuan.Count; i++)
            {
                if (KeHuDuan[i].ZhanHao == zhanhao)
                {
                    return KeHuDuan[i].GetShuJu();
                }
            }
           
            return null;
        }
        private void XieLog(string msg)
        {
            if (MsgEvent != null)
            {
                MsgEvent(msg);
            }
        }
    }


    /// <summary>
    /// 发送的数据
    /// </summary>
    public class SengModel
    {
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; } = "";

        /// <summary>
        /// 表示哪个站  
        /// </summary>
        public string ZhanDian { get; set; } = "";

        /// <summary>
        /// 1表示增加数据 2表示心跳 3请求扫码数据 4表示做完 
        /// </summary>
        public int ShuJuType { get; set; } = 0;

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; } = "";

        public string BiaoZhi { get; set; } = "";

        /// <summary>
        /// 1表示合格
        /// </summary>
        public int IsHeGe { get; set; } = 0;

        /// <summary>
        /// 参数  如果是ShuJuType 3 
        /// </summary>
        public List<SaoMaShuJuModel> Value { get; set; } = new List<SaoMaShuJuModel>();

        public List<string> GetHaoMa()
        {
            List<string> lis = new List<string>();
        
            for (int i = 0; i < Value.Count; i++)
            {
                lis.Add($"{Value[i].ShunXuID}!{Value[i].ShangMa}!{Value[i].HouMa}!{BiaoZhi}");
            }
            return lis;
        }
    }

   

    public class SaoMaShuJuModel       
    {
        public int ShunXuID { get; set; }

        public string ShangMa { get; set; } = "";

        public string HouMa { get; set; } = "";
        /// <summary>
        /// 1表示合格
        /// </summary>
        public int IsHeGe { get; set; } = 1;

        /// <summary>
        /// 不合格序号
        /// </summary>
        public int BuHeGeXuHao { get; set; } = -1;

        public SaoMaShuJuModel FuZhi()
        {
            SaoMaShuJuModel model = new SaoMaShuJuModel();
            model.BuHeGeXuHao = BuHeGeXuHao;
            model.HouMa = HouMa;
            model.IsHeGe = IsHeGe;
            model.ShangMa = ShangMa;
            model.ShunXuID = ShunXuID;
            return model;
        }
    }

 
   

    internal class FuWuQi
    {
        public PeiZhiLei PeiZhiLei { get; set; }
        public event Action<string> MsgEvent;
        private Encoding Encoding = Encoding.UTF8;
       
        /// <summary>
        /// 服务套接字
        /// </summary>
        private Socket _skFuWusocket = null;
        private Dictionary<string, IPFuWuPeiModel> LianJieSokct = new Dictionary<string, IPFuWuPeiModel>();
        List<byte> KaiTou = new List<byte>();
        List<byte> JieSu = new List<byte>();
        private FanXingJiHeLei<SengModel> Xie = new FanXingJiHeLei<SengModel>();

        /// <summary>
        /// 用于监听IP端口
        /// </summary>
        private Task _ThJianTingIP;
        /// <summary>
        /// 用于监听IP端口和工作线程管理
        /// </summary>
        private bool _Runing = true;

        public FuWuQi()
        {
            KaiTou = Encoding.GetBytes("*&{").ToList();
            JieSu = Encoding.GetBytes("}*&").ToList();
        }

        /// <summary>
        /// 打开方法 返回true表示成功，返回false失败
        /// </summary>
        public bool Open(List<IPFuWuPeiModel> lismodel)
        {
            try
            {
                LianJieSokct.Clear();
                List<IPFuWuPeiModel> models = lismodel;
                for (int i = 0; i < models.Count; i++)
                {
                    if (models[i].IsFuWuDuan !=1)
                    {
                        if (LianJieSokct.ContainsKey(models[i].ZhanDianName) == false)
                        {
                            LianJieSokct.Add(models[i].ZhanDianName, models[i]);
                        }
                    }
                }

                for (int i = 0; i < models.Count; i++)
                {
                    if (models[i].IsFuWuDuan==1)
                    {
                        models[i].Tx = false;
                        string Ip = models[i].IP;
                        IPAddress ipadd = new IPAddress(0);
                        if (!IPAddress.TryParse(Ip, out ipadd))
                        {
                            return false;
                        }
                        if (models[i].Port < 0)
                        {
                            return false;
                        }

                        IPEndPoint ipport = new IPEndPoint(ipadd, models[i].Port);
                        _skFuWusocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _skFuWusocket.Bind(ipport);
                        _skFuWusocket.Listen(50);
                        _Runing = true;
                        _ThJianTingIP = Task.Factory.StartNew(JianTingDoing);
                        Task.Factory.StartNew(FuWuQiFaSong);
                        
                        models[i].Tx = true;
                        XieLog($"服务器监听成功:{ipport}");                       
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                XieLog($"服务器监听失败:{ex}");              
                return false;
            }
            XieLog($"服务器监听失败");
         
            return false;
        }

     

        /// <summary>
        /// 关闭的方法
        /// </summary>
        public void Close()
        {
            _Runing = false;

            Thread.Sleep(60);
            foreach (var item in LianJieSokct.Keys)
            {
                try
                {
                    if (LianJieSokct[item].Socket != null)
                    {
                        LianJieSokct[item].Socket.Close();
                        LianJieSokct[item].Socket.Dispose();
                    }
                }
                catch
                {


                }
            }

            if (_skFuWusocket != null)
            {
                try
                {
                    _skFuWusocket.Close();
                    _skFuWusocket.Dispose();
                }
                catch
                {


                }

            }
        }



        /// <summary>
        /// 监听连接对象
        /// </summary>
        private void JianTingDoing()
        {
            while (_Runing)
            {
                try
                {
                    Socket jubusocket = _skFuWusocket.Accept();
                    IPEndPoint iPs = jubusocket.RemoteEndPoint as IPEndPoint;
                    XieLog( $"服务器接收客户端连接等待发连接信息: {iPs.Address.ToString()}");
                    Task.Factory.StartNew((x) => {
                        DateTime shijin = DateTime.Now;

                        Socket socket = x as Socket;
                        SerialFeiDataJieXi serialFeiDataJieXi = new SerialFeiDataJieXi();
                        while (_Runing)
                        {
                            if (socket.Available > 0)
                            {
                                #region MyRegion
                                byte[] byt = new byte[socket.Available];
                                if (socket.Receive(byt, byt.Length, SocketFlags.Partial) > 0)
                                {
                                    serialFeiDataJieXi.AddByteList(byt);
                                }
                                #endregion
                            }
                            serialFeiDataJieXi.JieXiWanMeiData(KaiTou, JieSu, false, null, false);
                            if (serialFeiDataJieXi.DataCount > 0)
                            {
                                byte[] shuju = serialFeiDataJieXi.GetShiShiData();
                                if (shuju.Length >= 4)
                                {
                                    List<byte> shujusl = shuju.ToList();
                                    for (int i = 0; i < 2; i++)
                                    {
                                        shujusl.RemoveAt(0);
                                    }
                                    int count1s = shujusl.Count;
                                    shujusl.RemoveAt(count1s - 1);
                                    count1s = shujusl.Count;
                                    shujusl.RemoveAt(count1s - 1);
                                    SengModel model = ChangYong.HuoQuJsonToShiTi<SengModel>(Encoding.GetString(shujusl.ToArray()));
                                  
                                    XieLog($"接收到客户端发来连接的数据: {iPs.Address.ToString()} {Encoding.UTF8.GetString(shuju.ToArray())}");
                                    if (model != null)
                                    {
                                        if (model.ShuJuType == 2)
                                        {
                                            if (LianJieSokct.ContainsKey(model.ZhanDian))
                                            {
                                                LianJieSokct[model.ZhanDian].IP = (socket.RemoteEndPoint as IPEndPoint).Address.ToString();
                                                LianJieSokct[model.ZhanDian].Socket = socket;
                                                LianJieSokct[model.ZhanDian].JiShiQi = DateTime.Now;
                                                LianJieSokct[model.ZhanDian].Tx = true;
                                                shujusl.Clear();
                                                serialFeiDataJieXi.Clear();
                                                break;
                                            }
                                        }
                                        shujusl.Clear();
                                    }
                                }

                            }
                            if ((DateTime.Now - shijin).TotalMilliseconds > 3 * 1000)
                            {
                                socket.Close();
                                socket.Dispose();
                                break;
                            }
                        }
                    }, jubusocket);
                }
                catch
                {

                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 工作线程
        /// </summary>
        /// <param name="obj"></param>
        private void FuWuQiFaSong()
        {
           
            DateTime shijin = DateTime.Now;
            DateTime shijins = DateTime.Now;         
            while (_Runing)
            {
                try//用于处理网络通信的逻辑片段，主要功能是从一个队列中取出数据并发送给已连接的客户端
                {
                    if (_skFuWusocket != null)
                    {
                        int count = Xie.GetCount();

                        if (count > 0)
                        {
                            SengModel fasongmodel= Xie.GetModel_Head_RomeHead();
                            if (fasongmodel != null)
                            {
                                foreach (var item in LianJieSokct.Keys)
                                {
                                    if (item == fasongmodel.ZhanDian)
                                    {
                                        if (LianJieSokct[item].Socket != null)
                                        {
                                            if (LianJieSokct[item].Socket.Connected)
                                            {
                                                SendKeHu(fasongmodel, LianJieSokct[item].Socket);
                                                XieLog($"服务器发送数据:{ChangYong.HuoQuJsonStr(fasongmodel)} {item}");
                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }
                }
                catch
                {

                }

                try//客户端发来的数据
                {
                    if (_skFuWusocket != null)
                    {
                        
                        //心跳包
                        foreach (var item in LianJieSokct.Keys)
                        {

                            if (LianJieSokct[item].Socket != null)
                            {
                                if (LianJieSokct[item].Socket.Connected)
                                {
                                    if (LianJieSokct[item].Socket.Available > 0)
                                    {
                                        #region MyRegion
                                        byte[] byt = new byte[LianJieSokct[item].Socket.Available];
                                        if (LianJieSokct[item].Socket.Receive(byt, byt.Length, SocketFlags.Partial) > 0)
                                        {

                                            LianJieSokct[item].JieXieQi.AddByteList(byt);
                                        }
                                        #endregion
                                    }

                                }
                                LianJieSokct[item].JieXieQi.JieXiWanMeiData(KaiTou, JieSu, false, null, false);
                                if (LianJieSokct[item].JieXieQi.DataCount > 0)
                                {
                                    //心跳包里各站点的数据
                                    byte[] shuju = LianJieSokct[item].JieXieQi.GetHeadAndRomveData();
                                    if (shuju.Length >= 4)
                                    {
                                        List<byte> shujusl = shuju.ToList();
                                        for (int i = 0; i < 2; i++)
                                        {
                                            shujusl.RemoveAt(0);
                                        }
                                        int count1s = shujusl.Count;
                                        shujusl.RemoveAt(count1s - 1);
                                        count1s = shujusl.Count;
                                        shujusl.RemoveAt(count1s - 1);
                                        //发送的数据
                                        SengModel modesl = ChangYong.HuoQuJsonToShiTi<SengModel>(Encoding.GetString(shujusl.ToArray()));
                                        if (modesl != null)
                                        {
                                            //更新数据
                                            if (modesl.ShuJuType == 3)//请求数据
                                            {
                                                SengModel modesdl= QingQiuShuJu(modesl);
                                                Xie.Add(modesdl);
                                            }
                                            else if (modesl.ShuJuType == 4)//到位
                                            {

                                                ZuoWan(modesl);
                                            }
                                            else if (modesl.ShuJuType == 1)//到位
                                            {
                                                JiaRu(modesl);
                                            }
                                            LianJieSokct[item].JiShiQi = DateTime.Now;
                                        }
                                    }

                                   XieLog($"接收到客户端发来的数据:{Encoding.GetString(shuju)} {item}");
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
                    //检测与客户端的连接是否仍然有效
                    if (_skFuWusocket != null)
                    {
                        foreach (var item in LianJieSokct.Keys)
                        {
                            if (LianJieSokct[item].Socket != null)
                            {
                                if ((DateTime.Now - LianJieSokct[item].JiShiQi).TotalMilliseconds > 20 * 1000)
                                {
                                    LianJieSokct[item].Socket.Close();
                                    LianJieSokct[item].Socket.Dispose();
                                    LianJieSokct[item].Socket = null;
                                    LianJieSokct[item].Tx = false;
                                }
                            }

                        }

                    }
                }
                catch
                {


                }

             

                Thread.Sleep(5);

            }


        }

        private void JiaRu(SengModel modesl)
        {
            List<int> keys = PeiZhiLei.DataMoXing.JianCeXuHao.Keys.ToList();
            if (keys.Count > 0)
            {
               List<IPFuWuPeiModel> shebei=  PeiZhiLei.DataMoXing.LisSheBei;
                for (int i = 0; i < shebei.Count; i++)
                {
                    IPFuWuPeiModel ifmodel = shebei[i];
                    if (ifmodel.PaiXu== keys[0]&& ifmodel.IsFuWuDuan!=1)
                    {
                        SaoMaMoXingModel saoMaMoXingModel = new SaoMaMoXingModel();
                        saoMaMoXingModel.JieSu = 0;
                        saoMaMoXingModel.PaiXu = ifmodel.PaiXu;
                        saoMaMoXingModel.BiaoShi = DateTime.Now.ToString("yyyyMMddHHmmss");
                        saoMaMoXingModel.XunShuHao = HCDanGeDataLei<ZiZengIDModel>.Ceratei().LisWuLiao.ID;
                        saoMaMoXingModel.YiDuiShuJu = new List<SaoMaShuJuModel>();
                        {
                            for (int c = 0; c < modesl.Value.Count; c++)
                            {
                                SaoMaShuJuModel model = modesl.Value[c];
                                model.IsHeGe = 1;
                                saoMaMoXingModel.YiDuiShuJu.Add(model);
                            }

                        }
                        if (ifmodel.IsFuWuDuan == 4)
                        {
                            Xie.Add(FaGeiICT(saoMaMoXingModel, ifmodel.ZhanDianName));
                            XieLog($"ICT接管数据；{ChangYong.HuoQuJsonStr(saoMaMoXingModel)}");
                        }
                        else
                        {
                            PeiZhiLei.DataMoXing.YongYuBaoCun.Add(saoMaMoXingModel);
                            if (PeiZhiLei.DataMoXing.JianCeXuHao.ContainsKey(ifmodel.PaiXu))
                            {
                                PeiZhiLei.DataMoXing.JianCeXuHao[ifmodel.PaiXu].Add(saoMaMoXingModel);
                            }
                            HCDanGeDataLei<ZiZengIDModel>.Ceratei().LisWuLiao.ID++;
                            HCDanGeDataLei<ZiZengIDModel>.Ceratei().BaoCun();
                            XieLog($"扫码设备增加一条数据:{ChangYong.HuoQuJsonStr(saoMaMoXingModel)}");
                        }
                      
                       
                        break;
                    }
                }

            }
            else
            {
                XieLog($"没有配置接收数据:{ChangYong.FenGeDaBao(keys," ")}");
            }
          
          
        }

        private SengModel QingQiuShuJu(SengModel modesl)
        {
            SengModel send = new SengModel();
            send.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            send.IP = ((IPEndPoint)_skFuWusocket.LocalEndPoint).Address.ToString();
            send.ShuJuType = 3;
            send.ZhanDian = modesl.ZhanDian;
            if (LianJieSokct.ContainsKey(modesl.ZhanDian))
            {
                int paxu = LianJieSokct[modesl.ZhanDian].PaiXu;
                if (PeiZhiLei.DataMoXing.JianCeXuHao.ContainsKey(paxu))
                {
                    FanXingJiHeLei<SaoMaMoXingModel> shuju = PeiZhiLei.DataMoXing.JianCeXuHao[paxu];
                    SaoMaMoXingModel shujuf = shuju.GetModel_Head_RomeHead();
                    if (shujuf != null)
                    {
                        send.BiaoZhi = shujuf.BiaoShi;
                        send.IsHeGe = 1;
                        shujuf.JieSu = 3;
                        for (int i = 0; i < shujuf.YiDuiShuJu.Count; i++)
                        {
                            send.Value.Add(shujuf.YiDuiShuJu[i].FuZhi());
                        }
                        if (shujuf.YiDuiShuJu.Count <= 0)
                        {
                            send.BiaoZhi = "";
                            send.IsHeGe = 0;
                        }
                    }
                    else
                    {
                        send.BiaoZhi = "没有数据";
                        send.IsHeGe = 0;
                    }
                }
                else
                {
                    send.BiaoZhi = "没有找到";
                    send.IsHeGe = 0;
                    send.Value = new List<SaoMaShuJuModel>();
                }
            }
            else
            {
                send.BiaoZhi = "";
                send.IsHeGe = 0;             
                send.Value = new List<SaoMaShuJuModel>();
            }
            return send;
        }

        private SengModel FaGeiICT(SaoMaMoXingModel modesl,string zhanming)
        {
            SengModel send = new SengModel();
            send.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            send.IP = ((IPEndPoint)_skFuWusocket.LocalEndPoint).Address.ToString();
            send.ShuJuType = 3;
            send.ZhanDian = zhanming;
            send.BiaoZhi = modesl.BiaoShi;
            send.IsHeGe = 1;
            modesl.JieSu = 3;
            for (int i = 0; i < modesl.YiDuiShuJu.Count; i++)
            {
                send.Value.Add(modesl.YiDuiShuJu[i].FuZhi());
            }
            if (modesl.YiDuiShuJu.Count <= 0)
            {
                send.BiaoZhi = "";
                send.IsHeGe = 0;
            }
           
            return send;
        }

        private void ZuoWan(SengModel model)
        {
            List<SaoMaMoXingModel> shujus = PeiZhiLei.DataMoXing.YongYuBaoCun;
            for (int i = 0; i < shujus.Count; i++)
            {
                if (shujus[i].BiaoShi.Equals(model.BiaoZhi))
                {
                    if (model.IsHeGe == 1)
                    {
                        int xiayige = -1;
                        int  leixing  =1;
                        string name = "";
                        int xianzaipaxu = GetPaiXu(model.ZhanDian, out xiayige,out leixing,out name);
                        if (xiayige > 0)
                        {
                            if (leixing == 4)
                            {
                                shujus[i].JieSu = 3;
                                shujus[i].PaiXu = xiayige;
                                Xie.Add(FaGeiICT(shujus[i], name));                          
                                XieLog($"ICT接管数据；{ChangYong.HuoQuJsonStr(shujus[i])}");
                            }
                            else
                            {
                                if (PeiZhiLei.DataMoXing.JianCeXuHao.ContainsKey(xiayige))
                                {
                                    shujus[i].JieSu = 0;
                                    shujus[i].PaiXu = xiayige;
                                    PeiZhiLei.DataMoXing.JianCeXuHao[xiayige].Add(shujus[i]);
                                    XieLog($"下一个工序接管；{ChangYong.HuoQuJsonStr(shujus[i])}");
                                }
                            }
                           
                        }
                        else
                        {
                            shujus[i].JieSu = 1;
                            XieLog($"结束；{ChangYong.HuoQuJsonStr(shujus[i])}");
                        }
                    }
                    else
                    {
                        shujus[i].JieSu = 1;
                        XieLog($"不合格结束；{ChangYong.HuoQuJsonStr(shujus[i])}");
                    }
                    break;
                }
            }
        }

        private int GetPaiXu(string zhandian, out int xiayigepaxu,out int xiayizhanname,out string name)
        {
            xiayizhanname = 1;
            xiayigepaxu =- 1;
            name = "";
            if (LianJieSokct.ContainsKey(zhandian))
            {
                int paxu = LianJieSokct[zhandian].PaiXu;
                for (int i = 0; i < PeiZhiLei.DataMoXing.LisSheBei.Count; i++)
                {
                    IPFuWuPeiModel ifmodel = PeiZhiLei.DataMoXing.LisSheBei[i];
                    if (ifmodel.IsFuWuDuan!=1&& ifmodel.IsQingQiu==1)
                    {
                        if (ifmodel.PaiXu> paxu)
                        {
                            xiayigepaxu = ifmodel.PaiXu;
                            xiayizhanname = ifmodel.IsFuWuDuan;
                            name = ifmodel.ZhanDianName;
                            break;
                        }                   
                    }
                }
                return paxu;
            }
            return -1;
        }
        //发送客户端
        private void SendKeHu(SengModel model, Socket sokect)
        {
            SocketError socketsd = SocketError.IsConnected;
            string shujus = string.Format("{0}{1}{0}", "*&", ChangYong.HuoQuJsonStr(model));
            byte[] shudd = Encoding.GetBytes(shujus);
            int shuliang = sokect.Send(shudd, 0, shudd.Length, SocketFlags.Partial, out socketsd);
            XieLog($"发送客户端数据:{shujus} {socketsd}");
        }

        private void XieLog(string msg)
        {
            if (MsgEvent!=null)
            {
                MsgEvent(msg);
            }
        }
    }

    /// <summary>
    /// 客户端
    /// </summary>
    internal class KeHuDuan
    {
        private PeiZhiLei PeiZhiLei;
        public event Action<string> MsgEvent;
        public string ZhanHao { get; set; } = "";
        private string IP = "";
        private int DuanKou = 9989;
        /// <summary>
        /// 服务套接字
        /// </summary>
        private Socket _skFuWusocket = null;

        private bool IsZhengChang = false;

        private int LeiXing = 1;

        private FanXingJiHeLei<SengModel> XuYaoFaSong = new FanXingJiHeLei<SengModel>();

        private FanXingJiHeLei<SengModel> Xie = new FanXingJiHeLei<SengModel>();

        private FanXingJiHeLei<SengModel> JieShou = new FanXingJiHeLei<SengModel>();

        private Encoding Encoding = Encoding.UTF8;

        private IPFuWuPeiModel iPFuWu;
        /// <summary>
        /// 用于监听IP端口和工作线程管理
        /// </summary>
        private bool _Runing = true;

        public KeHuDuan()
        {

        }

        /// <summary>
        /// 打开方法 返回true表示成功，返回false失败
        /// </summary>
        public bool Open(IPFuWuPeiModel model,PeiZhiLei peiZhiLei)
        {
            try
            {
                try
                {
                    PeiZhiLei = peiZhiLei;
                    ZhanHao = model.ZhanDianName;
                    IsZhengChang = false;
                    string Ip = model.IP;
                    IP = Ip;
                    iPFuWu = model;
                    LeiXing = model.IsFuWuDuan;
                    IPAddress ipadd = new IPAddress(0);
                    if (!IPAddress.TryParse(Ip, out ipadd))
                    {
                        return false;
                    }
                    if (model.Port < 0)
                    {
                        return false;
                    }
                    DuanKou = model.Port;
                    _skFuWusocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _skFuWusocket.ReceiveTimeout = 400;
                    _skFuWusocket.Connect(ipadd, model.Port);
                    if (_skFuWusocket.Connected)
                    {
                        IsZhengChang = true;

                    }

                   XieLog($"客户端连接服务器:{ipadd} {model.Port} {_skFuWusocket.Connected}");

                }
                catch
                {


                }
                Task _JieShouXianCheng = Task.Factory.StartNew(Working);
                if (model.IsFuWuDuan==4)
                {
                    Task.Factory.StartNew(XieWanJian);
                }
                return true;

            }
            catch
            {


            }

            return false;
        }

        /// <summary>
        /// 关闭的方法
        /// </summary>
        public void Close()
        {
            _Runing = false;

            Thread.Sleep(60);

            if (_skFuWusocket != null)
            {
                try
                {
                    _skFuWusocket.Close();
                    _skFuWusocket.Dispose();
                }
                catch
                {


                }

            }
        }
        private void XieWanJian()
        {
            while (_Runing)
            {
                try
                {
                    if (LeiXing==4)
                    {
                        SengModel shuju = JieShou.GetModel_Head();
                        if (shuju!=null)
                        {
                            if (shuju.IsHeGe == 1)
                            {
                                string lujing = string.Format(@"\\{0}", iPFuWu.DiZhi);
                                if (Directory.Exists(lujing) == false)
                                {
                                    Directory.CreateDirectory(lujing);
                                }
                                string filename = string.Format(@"{0}\{1}", lujing, "barcode.barcode");
                                if (File.Exists(filename) == false)
                                {
                                    SaoMaMoXingModel mashuju = GetZhengShiModel(shuju);
                                    if (mashuju != null)
                                    {
                                        mashuju.YiDuiShuJu.Sort((x, y) =>
                                        {
                                            if (x.ShunXuID > y.ShunXuID)
                                            {
                                                return 1;
                                            }
                                            else
                                            {
                                                return -1;
                                            }
                                        });
                                        try
                                        {
                                            using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                                            {
                                                string shujudd = mashuju.ToString();
                                                XieLog($"给ICT写码数据:{ChangYong.HuoQuJsonStr(shujudd)} ");
                                                byte[] bytes = Encoding.Default.GetBytes(shujudd);
                                                fileStream.Write(bytes, 0, bytes.Length);
                                            }
                                            mashuju.JieSu = 3;
                                            XuYaoFaSong.Add(shuju);
                                            JieShou.Romve_Head();
                                        }
                                        catch
                                        {


                                        }


                                    }
                                }
                            }
                            else
                            {
                                JieShou.Romve_Head();
                            }
                        }
                    }
                    int count = XuYaoFaSong.GetCount();
                    if (count>0)
                    {
                        for (int c = 0; c < count; c++)
                        {
                            SengModel sengModel = XuYaoFaSong.GetModel_iHead(c);
                            if (sengModel!=null)
                            {
                                List<string> shujusma = sengModel.GetHaoMa();

                                if (shujusma.Count > 0)
                                {
                                    List<string> qufen = GetWenJian(iPFuWu.DataLuJing, shujusma);
                                    if (qufen.Count== shujusma.Count)
                                    {
                                        qufen.Add(sengModel.BiaoZhi);
                                        bool zhen= PeiZhiLei.DataMoXing.SetDuJiCunQiValue(iPFuWu.IsTongYiGe, qufen);
                                        if (zhen)
                                        {
                                            XuYaoFaSong.Romve_Zhiding(c);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    XuYaoFaSong.Romve_Zhiding(c);
                                    break;
                                }
                            }
                        }
                    }
                 
                }
                catch
                {

                }
                Thread.Sleep(10);
            }
        }
        private List<string> GetWenJian(string lujing, List<string> shujusma)
        {
            List<string> sss = new List<string>();
            List<string> WenJian = Directory.GetFiles(lujing,"*.dcl").ToList();
            for (int i = 0; i < WenJian.Count; i++)
            {
                for (int c = 0; c < shujusma.Count; c++)
                {
                    string wenjian = ChangYong.GetWenJianName(WenJian[i]);
                    if (wenjian.StartsWith(shujusma[c]))
                    {
                        sss.Add(WenJian[i]);
                    }
                }
                if (sss.Count== shujusma.Count)
                {
                    break;
                }
            }
            return sss;
        }
        /// <summary>
        /// 发送数据，需要传ip，端口，消息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="duankou"></param>
        /// <param name="msg"></param>
        public void SendShuJuFuWu(List<SaoMaShuJuModel> shuju, int leixing,string biaoshi,bool ishege)
        {
            if (IsZhengChang)
            {
                SengModel model = new SengModel();
                model.IP = ((IPEndPoint)_skFuWusocket.LocalEndPoint).Address.ToString();
                model.Date = DateTime.Now.ToString();
                model.ZhanDian = ZhanHao;
                model.ShuJuType = leixing;
                model.BiaoZhi = biaoshi;
                model.IsHeGe = ishege ? 1 : 0;
                model.Value = shuju;
                Xie.Add(ChangYong.FuZhiShiTi(model));
            }

        }

        /// <summary>
        /// 工作线程
        /// </summary>
        /// <param name="obj"></param>
        private void Working()
        {
            List<byte> kaitou = Encoding.GetBytes("*&{").ToList();
            List<byte> jiesu = Encoding.GetBytes("}*&").ToList();
            SerialFeiDataJieXi serialFeiDataJieXi = new SerialFeiDataJieXi();
            DateTime shijin = DateTime.Now;
            DateTime xintiao = DateTime.Now;
            int cishu = 0;


            while (_Runing)
            {
                if (IsZhengChang == false)
                {
                    cishu = 0;
                    if ((DateTime.Now - shijin).TotalMilliseconds >= 1000)
                    {
                        ChongLian();
                        shijin = DateTime.Now;
                    }
                    Thread.Sleep(30);
                    continue;
                }
                if (IsZhengChang)
                {
                    if (cishu == 0)
                    {
                        cishu++;

                        SengModel model = new SengModel();
                        model.IP = ((IPEndPoint)_skFuWusocket.LocalEndPoint).Address.ToString();
                        model.Date = "";
                        model.ShuJuType = 2;
                        model.ZhanDian = ZhanHao;
                        SendFuWu(model);

                    }
                }
                if (_skFuWusocket != null)
                {
                    if (IsZhengChang)
                    {
                        if (_skFuWusocket.Connected == false)
                        {
                            IsZhengChang = false;
                        }
                        if (IsZhengChang)
                        {
                            try
                            {
                                if (_skFuWusocket.Available > 0)
                                {
                                    #region MyRegion
                                    byte[] byt = new byte[_skFuWusocket.Available];
                                    if (_skFuWusocket.Receive(byt, byt.Length, SocketFlags.Partial) > 0)
                                    {
                                        serialFeiDataJieXi.AddByteList(byt);
                                    }
                                    #endregion
                                }
                                serialFeiDataJieXi.JieXiWanMeiData(kaitou, jiesu, false, null, false);
                                if (serialFeiDataJieXi.DataCount > 0)
                                {
                                    byte[] shuju = serialFeiDataJieXi.GetHeadAndRomveData();
                                    if (shuju.Length >= 4)
                                    {
                                        List<byte> shujusl = shuju.ToList();
                                        for (int i = 0; i < 2; i++)
                                        {
                                            shujusl.RemoveAt(0);
                                        }
                                        int count1s = shujusl.Count;
                                        shujusl.RemoveAt(count1s - 1);
                                        count1s = shujusl.Count;
                                        shujusl.RemoveAt(count1s - 1);
                                        SengModel model = ChangYong.HuoQuJsonToShiTi<SengModel>(Encoding.GetString(shujusl.ToArray()));
                                        XieLog($"接收到服务器发来的数据:{Encoding.GetString(shuju)}");
                                        if (model != null)
                                        {
                                            if (model.ShuJuType == 3)
                                            {
                                                JieShou.Add(model);
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

                                int count = Xie.GetCount();
                                if (count > 0)
                                {
                                    if (count > 10)
                                    {
                                        count = 10;
                                    }
                                    for (int i = 0; i < count; i++)
                                    {
                                        SengModel model = Xie.GetModel_iHead(i);

                                        if (model != null)
                                        {
                                            if (_skFuWusocket != null)
                                            {
                                                if (_skFuWusocket.Connected)
                                                {
                                                    bool zhen = SendFuWu(model);
                                                    if (zhen)
                                                    {
                                                        Thread.Sleep(10);
                                                        xintiao = DateTime.Now;
                                                        Xie.Romve_Zhiding(i);
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }

                            }
                            catch
                            {


                            }
                            //if ((DateTime.Now - xintiao).TotalMilliseconds >= 10 * 1000)
                            if ((DateTime.Now - xintiao).TotalMilliseconds >= 9 * 1000)
                            {

                                SengModel model = new SengModel();
                                model.IP = ((IPEndPoint)_skFuWusocket.LocalEndPoint).Address.ToString(); ;
                                model.Date = "";
                                model.ShuJuType = 2;
                                model.ZhanDian = ZhanHao;
                                SendFuWu(model);

                                xintiao = DateTime.Now;
                            }
                        }
                    }
                }
                Thread.Sleep(20);

            }


        }
        private void ChongLian()
        {
          
            string ip = IP;
            int port = DuanKou;
           
            string Ip = ip;
            IPAddress ipadd = new IPAddress(0);

            XieLog( $"客户端连开始重连:{Ip} {port} {_skFuWusocket.Connected}");
            if (!IPAddress.TryParse(Ip, out ipadd))
            {
                return;
            }
            if (port < 0)
            {
                return;
            }
            try
            {
                if (_skFuWusocket != null)
                {
                    _skFuWusocket.Close();
                    _skFuWusocket.Dispose();
                    _skFuWusocket = null;
                }
            }
            catch
            {


            }
            try
            {

                _skFuWusocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _skFuWusocket.ReceiveTimeout = 400;
                _skFuWusocket.Connect(ipadd, port);
                if (_skFuWusocket.Connected)
                {
                    IsZhengChang = true;
                }
            }
            catch
            {


            }

        }

        private bool SendFuWu(SengModel model)
        {
            bool zhen = true;
            SocketError socketsd = SocketError.IsConnected;
            string shujus = string.Format("{0}{1}{0}", "*&", ChangYong.HuoQuJsonStr(model));
            byte[] shudd = Encoding.GetBytes(shujus);
            int shuliang = _skFuWusocket.Send(shudd, 0, shudd.Length, SocketFlags.Partial, out socketsd);
            if (socketsd != SocketError.Success)
            {
                zhen = false;

            }
            if (model.ShuJuType == 2)
            {
               XieLog( $"发送服务器心跳数据:{shujus} {zhen}");
            }
            else
            {
                XieLog($"发送服务器数据:{shujus} {zhen}");
            }
            return zhen;
        }

        private void XieLog(string msg)
        {
            if (MsgEvent != null)
            {
                MsgEvent(msg);
            }
        }
        private SaoMaMoXingModel GetZhengShiModel(SengModel shuju)
        {
            SaoMaMoXingModel mashuju = new SaoMaMoXingModel();
            mashuju.BiaoShi = shuju.BiaoZhi;
            mashuju.YiDuiShuJu = shuju.Value;
            return mashuju;
        }
        public SengModel GetShuJu()
        {
            int count = JieShou.GetCount();
            if (count > 0)
            {
                SengModel shuju = JieShou.GetModel_Head_RomeHead();
                return shuju;
            }
            return null;
        }
    }
}
