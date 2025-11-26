using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ModBuTCP.Model;
using CommLei.JiChuLei;
using CommLei.DataChuLi;
using System.Threading;
using CommLei.GongYeJieHe;
using ModBuTCP.Frm;

namespace ModBuFuWuQi.ShiXian
{
    public  class ModbusTCP
    {
        private PeiZhiLei PeiZhiLei;
        private bool _Runing = false;
        private bool _Runing1 = false;
        /// <summary>
        /// 1是正常消息 2是错误消息
        /// </summary>
        public event Action<int, string> MsgEvent;
        private SheBeiModel SheBeiModel;
        /// <summary>
        /// 服务套接字
        /// </summary>
        private Socket _skFuWusocket = null;
      
        private FanXingJiHeLei<DataCunModel> FaSongShuJu = new FanXingJiHeLei<DataCunModel>();
        public void SetCanShu(SheBeiModel shebeinodel, PeiZhiLei peiZhiLei)
        {
            SheBeiModel = shebeinodel;
            PeiZhiLei= peiZhiLei;
        }

        public bool Open()
        {
                   
            try
            {             
                string Ip = SheBeiModel.IpOrCom;
                IPAddress ipadd = new IPAddress(0);
                if (!IPAddress.TryParse(Ip, out ipadd))
                {
                    return false;
                }
                if (SheBeiModel.Port < 0)
                {
                    return false;
                }

                IPEndPoint ipport = new IPEndPoint(ipadd, SheBeiModel.Port);
                _skFuWusocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _skFuWusocket.Bind(ipport);
                _skFuWusocket.Listen(50);
                _Runing = true;
                Task.Factory.StartNew(JianTingDoing);
               

                ChuFaMsg(1,$"服务器监听成功:{ipport}");
                return true;
            }
            catch (Exception ex)
            {
                ChuFaMsg(1, $"[{SheBeiModel.SheBeiName}] Modbus TCP从站已开始在 {SheBeiModel.IpOrCom}:{SheBeiModel.Port} 失败:{ex}。");
                Close();
            }
            return false;
          
        }


        /// <summary>
        /// 关闭的方法
        /// </summary>
        public void Close()
        {
            _Runing = false;

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
            Thread.Sleep(60);
        }


        public void SengShuJu(DataCunModel msg)
        {
            FaSongShuJu.Add(msg);
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
                    _Runing1 = false;
                    Thread.Sleep(100);
                    IPEndPoint iPs = jubusocket.RemoteEndPoint as IPEndPoint;
                    ChuFaMsg(1,$"有客户端连接: {iPs}");
                    _Runing1 = true;
                    Task.Factory.StartNew(FuWuQiFaSong, jubusocket);
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
        private void FuWuQiFaSong(object ect)
        {
           
            List<byte> jieshoushuju = new List<byte>();
            Socket socket = null;
            if (ect is Socket)
            {
                socket=ect as Socket;
            }
            if (socket==null)
            {
                return;
            }
            while (_Runing1)
            {
                try//用于处理网络通信的逻辑片段，主要功能是从一个队列中取出数据并发送给已连接的客户端
                {
                    if (_skFuWusocket != null)
                    {
                        int count = FaSongShuJu.GetCount();

                        if (count > 0)
                        {
                            DataCunModel fasongmodel = FaSongShuJu.GetModel_Head_RomeHead();
                            if (fasongmodel != null)
                            {
                                SendKeHu(fasongmodel, socket);
                                FaSongShuJu.Romve_All();
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
                        if (socket!=null)
                        {                            
                            if (socket.Available > 0)
                            {
                                #region MyRegion
                                byte[] byt = new byte[socket.Available];
                                if (socket.Receive(byt, byt.Length, SocketFlags.Partial) > 0)
                                {
                                    jieshoushuju.AddRange(byt);
                                }
                                #endregion
                            }
                            if (jieshoushuju.Count>=SheBeiModel.DiZhi)
                            {
                                
                                List<DataCunModel> cuns = PeiZhiLei.DataMoXing.GetCunModels(SheBeiModel.SheBeiID);
                                for (int i = 0; i < cuns.Count; i++)
                                {
                                    if (cuns[i].YingYongType==YingYongType.DuQingQiuASCII)
                                    {
                                        cuns[i].IsXieWang = 1;
                                        cuns[i].JiCunQiModel.IsKeKao = true;
                                        cuns[i].JiCunQiModel.Value = Encoding.ASCII.GetString(jieshoushuju.ToArray());
                                        ChuFaMsg(1,$"接收客户端数据；{cuns[i].JiCunQiModel.Value}");
                                    }
                                }
                                jieshoushuju.Clear();
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


        //发送客户端
        private void SendKeHu(DataCunModel model, Socket sokect)
        {
            SocketError socketsd = SocketError.IsConnected;
            if (model.YingYongType==YingYongType.XieShuJuASCII)
            {
                string shujus = string.Format("{0}", model.JiCunQiModel.Value);
                byte[] shudd = Encoding.ASCII.GetBytes(shujus);
                int shuliang = sokect.Send(shudd, 0, shudd.Length, SocketFlags.Partial, out socketsd);
                ChuFaMsg(1,$"发送客户端数据:{shujus} {socketsd}");
            }
           
        }
        private void ChuFaMsg(int biaozhi,string msg)
        {
            if (MsgEvent!=null)
            {
                MsgEvent(biaozhi, msg);
            }
        }

       
    }
}
