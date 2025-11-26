using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommLei.JiChuLei;

namespace CheLianMes
{
    public class XingDeMesQingQiuLei
    {
     
      
        private Dictionary<string, HttpQingQiuPostLei> HttpSheBeiLei = new Dictionary<string, HttpQingQiuPostLei>();
      

        private Encoding Encoding = Encoding.UTF8;

        #region 单例
        private readonly static object DanYi = new object();
        private static XingDeMesQingQiuLei jieRuFuWuQi = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        private XingDeMesQingQiuLei()
        {

        }

        /// <summary>
        /// 单例类，必须KaiqiRiZhi设置为True才能写日志
        /// </summary>
        /// <returns>返回NewXieRiZhiLog</returns>
        public static XingDeMesQingQiuLei Cerate()
        {
            if (jieRuFuWuQi == null)
            {
                lock (DanYi)
                {
                    if (jieRuFuWuQi == null)
                    {
                        jieRuFuWuQi = new XingDeMesQingQiuLei();
                    }
                }
            }
            return jieRuFuWuQi;
        }
        #endregion


        public void SetShuJu(List<string> zhan)
        {
            HttpSheBeiLei.Clear();
            for (int i = 0; i < zhan.Count; i++)
            {
                HttpQingQiuPostLei lianjiemodel = new HttpQingQiuPostLei();

                if (HttpSheBeiLei.ContainsKey(zhan[i]) == false)
                {
                    HttpSheBeiLei.Add(zhan[i], lianjiemodel);
                }
            }
        }
        /// <summary>
        /// 非文件的请求 true 表示服务端传大量数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="model"></param>
        /// <param name="shifouxiazaidawenjian"></param>
        /// <returns></returns>
        public OutingQiuModel<V> QiuQiuHttp<T, V>(T model, string zhanname,string wangzhi) where V : new() where T : InQingQiuModel
        {
            if (HttpSheBeiLei.ContainsKey(zhanname))
            {
                string rec = ChangYong.HuoQuJsonStr(model);
                HttpQingQiuPostLei shebeis = HttpSheBeiLei[zhanname];
                shebeis.SetWanZhi(wangzhi);
                string jieguo = shebeis.QiuQiuHttp(rec);
                OutingQiuModel<V> shuju = GetModelXin<V>(jieguo, rec);

                return shuju;
            }
            else
            {
                OutingQiuModel<V> shuju = new OutingQiuModel<V>();
                shuju.Msg = $"没有找到设备名称的连接服务端:{zhanname}";
                return shuju;
            }


        }


     
        private OutingQiuModel<V> GetModelXin<V>(string data, string fasongmsg) where V : new()
        {
            OutingQiuModel<V> outBaseModel = new OutingQiuModel<V>();
            if (string.IsNullOrEmpty(data))
            {
                outBaseModel.ChengGong = false;
                outBaseModel.Msg = $"发送数据:{fasongmsg}   接收数据:{data}";
                return outBaseModel;
            }

            try
            {
                V duixiang = ChangYong.HuoQuJsonToShiTi<V>(data);
                outBaseModel.Msg = $"发送数据:{fasongmsg}  接收数据:{data}";
                if (duixiang == null)
                {

                    outBaseModel.ChengGong = false;
                    outBaseModel.Msg = $"发送数据:{fasongmsg}  接收数据:{data}";
                }
                else
                {
                    outBaseModel.ChengGong = true;
                    outBaseModel.ShuJu = duixiang;
                }
            }
            catch (Exception ex)
            {
                outBaseModel = new OutingQiuModel<V>();
                outBaseModel.ChengGong = false;
                outBaseModel.Msg = $"发送数据:{fasongmsg}  接收数据:{data} 解析josn出错:{ex}";
            }

            return outBaseModel;
        }


        public void Close()
        {
           
            

        }





    }

 
   



    /// <summary>
    /// 这是客户端调用的类
    /// </summary>
    public class HttpQingQiuPostLei
    {


        /// <summary>
        /// http请求参数
        /// </summary>
        private HttpRequestModel _Model;

        /// <summary>
        /// 请求的Cookie;
        /// </summary>
        private CookieContainer _Cookie { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpQingQiuPostLei()
        {
            _Model = new HttpRequestModel();
            _Cookie = new CookieContainer();

          
        }


        /// <summary>
        /// 非文件的请求 true 表示服务端传大量数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="model"></param>
        /// <param name="shifouxiazaidawenjian"></param>
        /// <returns></returns>
        public string QiuQiuHttp(string model)
        {


            string shuju = PostQingQiu(_Model.StrUri, model);

            return shuju;

        }








        /// <summary>
        /// 参考用的
        /// </summary>
        public void SetWanZhi(string uri)
        {
            _Model.StrUri = uri;

        }






        #region 请求方法


        /// <summary>
        /// 请求与相应 这是Get请求  和小数据的post请求 请求地址
        /// </summary>
        /// <param name="srtRequest">网址</param>
        /// <returns>返回相应</returns>
        private string RequestAndRespone(string srtRequest)
        {

            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(srtRequest);
            http.Method = this._Model.RequestMethod;
            http.ContentType = this._Model.ContentType;
            http.Proxy = null;
            http.KeepAlive = true;
            string res = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)http.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), _Model.EncodWay);
                res = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch
            {
                res = "无法连接远程服务器，客户端请求失败";
            }
            return res;
        }

        /// <summary>
        /// 这是上传文件的请求 data表示文件的大小
        /// </summary>
        /// <param name="srtRequest"></param>
        /// <param name="data"></param>
        /// <param name="ChuangShu"></param>
        /// <returns></returns>
        private string ShangChuanShiPing(string srtRequest, long data, Action<Stream> ChuangShu)
        {
            if (data == 0 || ChuangShu == null || string.IsNullOrEmpty(srtRequest))
            {
                return "客户端上传的内容为空";
            }
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(srtRequest);

            http.ContentLength = data;

            http.Timeout = 300 * 1000;
            http.CookieContainer = _Cookie;
            http.ReadWriteTimeout = 300 * 1000;
            http.UserAgent = _Model.UserAgent;
            http.Accept = _Model.Accept;

            http.ContentType = _Model.ContentType;
            http.Method = _Model.JieShouMethod;

            Stream dataStream = null;
            try
            {
                dataStream = http.GetRequestStream();

            }
            catch
            {
                if (dataStream != null)
                {
                    dataStream.Close();
                    dataStream.Dispose();
                }

                return "无法连接服务器，客户端请求失败";

            }

            ChuangShu(dataStream);

            dataStream.Close();
            dataStream.Dispose();
            //读取返回消息
            string res = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)http.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                response.Close();
            }

            return res;



        }

        /// <summary>
        /// 大数据的post请求 srtRequest地址 后面是数据
        /// </summary>
        /// <param name="srtRequest"></param>
        /// <param name="udr"></param>
        /// <returns></returns>
        private string PostQingQiu(string srtRequest, string udr)
        {
            CookieContainer cookie = new CookieContainer();
            _Cookie = new CookieContainer();
            string content = "";
            try
            {
                HttpWebRequest httpRequset = (HttpWebRequest)HttpWebRequest.Create(srtRequest);//创建http 请求
                httpRequset.CookieContainer = cookie;//设置cookie
                httpRequset.Method = _Model.RequestMethod;//POST 提交              
                httpRequset.UserAgent = _Model.UserAgent;
                httpRequset.Accept = _Model.Accept;//text/xml;charset=utf-8
                httpRequset.ContentType = _Model.ContentType;//以上信息在监听请求的时候都有的直接复制过来//text/xml;charset=utf-8
                byte[] bytes = _Model.EncodWay.GetBytes(udr); //System.Text.Encoding.UTF8.GetBytes(postString);
                httpRequset.ContentLength = bytes.Length;
                Stream stream = httpRequset.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();//以上是POST数据的写入
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequset.GetResponse();//获得 服务端响应
                using (Stream responsestream = httpResponse.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responsestream, System.Text.Encoding.UTF8))
                    {
                        content = sr.ReadToEnd();
                    }
                }
                _Cookie = cookie;

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return content;//拿到cookie
        }


        #endregion

        internal class HttpRequestModel
        {
            /// <summary>
            /// 请求模式，默认为POST
            /// </summary>
            public string RequestMethod { get; set; }


            /// <summary>
            /// 接收模式，默认为GET
            /// </summary>
            public string JieShouMethod { get; set; }

            /// <summary>
            /// 设置http头标，默认为 "text/xml;charset=utf-8 "
            /// </summary>
            public string ContentType { get; set; }

            /// <summary>
            /// 设置http头标，默认为 "text/xml;charset=utf-8 "
            /// </summary>
            public string ContentType1 { get; set; }

            /// <summary>
            /// 获取返回的编码方式,默认为Encoding.Default
            /// </summary>
            public Encoding EncodWay { get; set; }

            /// <summary>
            /// 请求用户的信息
            /// </summary>
            public string UserAgent { get; set; }

            /// <summary>
            /// 接收的格式
            /// </summary>
            public string Accept { get; set; }


            /// <summary>
            /// q请求的地址
            /// </summary>
            public string StrUri { get; set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            public HttpRequestModel()
            {
                RequestMethod = "POST";

                EncodWay = Encoding.UTF8;
                JieShouMethod = "POST";


                //  UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36";
                Accept = "application/json, application/xhtml+xml, */*";//text/xml;charset=utf-8// text/plain, */*; q=0.01
                                                                        //    Accept1 = "text/plain, */*; q=0.01";
                                                                        // Accept = "text/plain, application/xhtml+xml, */*";//text/xml;charset=utf-8
                                                                        //   ContentType = "application/json";
                ContentType = "application/json; charset=UTF-8";
                // ContentType = "application/json; charset=UTF-8";

                //  ContentType1 = "application/json; charset=UTF-8";

                StrUri = "";
            }
        }
    }
}
