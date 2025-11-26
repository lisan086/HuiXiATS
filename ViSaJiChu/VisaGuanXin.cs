using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NationalInstruments.Visa;

namespace ViSaJiChu
{
    public  class VisaGuanXin
    {
        private List<string> Keys = new List<string>();
        private Dictionary<string, VisaModel> VisaSheBei=new Dictionary<string, VisaModel>();
      
        public event Action<string, int> MsgEvent;
       
        /// <summary>
        /// 获取VisaName
        /// </summary>
        /// <returns></returns>
        public  List<string> GetVisaNames(out bool zhenmeiyou)
        {
           
            try
            {
                zhenmeiyou = true;
                List<string> wenjian = new List<string>();
                using (var rmSession = new ResourceManager())
                {
                    var resources = rmSession.Find("(ASRL|GPIB|TCPIP|USB)?*");
                    foreach (string s in resources)
                    {                 
                        wenjian.Add(s);
                    }
                }

                return wenjian;
            }
            catch (System.ArgumentException ex)
            {
                zhenmeiyou = false;
                ChuFaMsg( string.Format("查找设备 {0},{1}", ex.Source, ex.Message),2);
            }
            return new List<string>();

        }


     

        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="strResourceName"></param>
        public  bool OpenResourceN(string strResourceName)
        {
            using (var rmSession = new ResourceManager())
            {
                if (Keys.IndexOf(strResourceName)<0)
                {
                    Keys.Add(strResourceName);
                }
                try
                {
                    MessageBasedSession mbSession = (MessageBasedSession)rmSession.Open(strResourceName);
                    mbSession.TimeoutMilliseconds = 2000;
                    if (VisaSheBei.ContainsKey(strResourceName) == false)
                    {
                        VisaModel visaModel = new VisaModel();
                        visaModel.IsTX = true;
                        visaModel.SheBeiName = strResourceName;
                        visaModel.Visa = mbSession;
                        VisaSheBei.Add(strResourceName, visaModel);
                     
                    }
                    else
                    {
                        VisaSheBei[strResourceName].Visa=mbSession;
                        VisaSheBei[strResourceName].IsTX = true;
                    }
                    ChuFaMsg(string.Format("{0}建立连接成功", strResourceName), 1);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    ChuFaMsg(string.Format("建立连接 {0},{1}", ex.Source, ex.Message), 2);

                }
                catch (Exception ex)
                {
                    ChuFaMsg(string.Format("建立连接 {0},{1}", ex.Source, ex.Message),2);
                }
                if (VisaSheBei.ContainsKey(strResourceName) == false)
                {
                    VisaModel visaModel = new VisaModel();
                    visaModel.IsTX = false;
                    visaModel.SheBeiName = strResourceName;
                    visaModel.Visa = null;
                    VisaSheBei.Add(strResourceName, visaModel);
                   
                }
                else
                {
                    if (VisaSheBei[strResourceName].Visa!=null)
                    {
                        try
                        {
                            VisaSheBei[strResourceName].Visa.Clear();
                        }
                        catch 
                        {

                        }
                        try
                        {
                            VisaSheBei[strResourceName].Visa.Dispose();
                        }
                        catch
                        {

                        }

                       
                    }
                    VisaSheBei[strResourceName].Visa=null;
                    VisaSheBei[strResourceName].IsTX = false;
                }
                ChuFaMsg(string.Format("{0}建立连接失败", strResourceName), 1);
                return false;
            }
        }

        public void ShuaXinShuJu()
        {
            //bool zhenyou = true;
            //List<string> visanames = GetVisaNames(out zhenyou);
            //if (zhenyou)
            //{
            //    List<string> lis = Keys;
            //    for (int i = 0; i < lis.Count; i++)
            //    {
            //        if (visanames.IndexOf(lis[i]) < 0)
            //        {
            //            if (VisaSheBei[lis[i]].CiShu > 5)
            //            {
            //                if (VisaSheBei[lis[i]].IsTX)
            //                {
            //                    VisaSheBei[lis[i]].IsTX = false;
            //                    try
            //                    {
            //                        VisaSheBei[lis[i]].Visa.Clear();
            //                    }
            //                    catch
            //                    {

                                   
            //                    }
            //                    try
            //                    {
            //                        VisaSheBei[lis[i]].Visa.Dispose();
            //                    }
            //                    catch
            //                    {


            //                    }
            //                }
                          
            //            }
            //            else
            //            {
            //                VisaSheBei[lis[i]].CiShu++;
            //            }
            //        }
            //        else
            //        {
            //            VisaSheBei[lis[i]].CiShu = 0;
            //            if (VisaSheBei[lis[i]].IsTX==false)
            //            {
            //                bool zhen = OpenResourceN(lis[i]);
            //                if (zhen)
            //                {
            //                    ChuFaMsg($"{lis[i]}:重连成功", 1);
            //                }
            //            }                                         
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 写入有返回的，读取和写失败会返回null,否则饭回数据
        /// </summary>
        /// <param name="visaname"></param>
        /// <param name="strcommand"></param>    
        /// <returns></returns>
        public  bool Write(string name,string cmd)
        {

            if (VisaSheBei.ContainsKey(name))
            {
                if (VisaSheBei[name].IsTX == false)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        OpenResourceN(name);
                        if (VisaSheBei[name].IsTX)
                        {
                            break;
                        }
                        Thread.Sleep(50);
                    }

                }
                try
                {
                    if (VisaSheBei[name].IsTX)
                    {
                        VisaSheBei[name].Visa.RawIO.Write(cmd);
                        VisaSheBei[name].CiShu = 0;
                        return true;
                    }
                    else
                    {
                        ChuFaMsg(string.Format(" {0}写入时 通信失败", name), 2);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    VisaSheBei[name].CiShu++;
                    ChuFaMsg(string.Format(" {0}写入时:{1}", name,ex), 2);
                    if (VisaSheBei[name].CiShu >= 6)
                    {
                        VisaSheBei[name].IsTX = false;
                    }
                }
               
            }
            else
            {
                ChuFaMsg(string.Format(" {0}写入时:该设备不存在", name),2);
            }
            return true;
        }

        /// <summary>
        /// 写入有返回的，读取和写失败会返回null,否则饭回数据
        /// </summary>
        /// <param name="visaname"></param>
        /// <param name="strcommand"></param>    
        /// <returns></returns>
        public string  Read(string name)
        {
            if (VisaSheBei.ContainsKey(name))
            {
                if (VisaSheBei[name].IsTX == false)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        OpenResourceN(name);
                        if (VisaSheBei[name].IsTX)
                        {
                            break;
                        }
                        Thread.Sleep(50);
                    }

                }
                try
                {
                    if (VisaSheBei[name].IsTX)
                    {
                        string jieguo = VisaSheBei[name].Visa.RawIO.ReadString();
                        VisaSheBei[name].CiShu = 0;
                        return jieguo;
                    }
                    else
                    {
                        ChuFaMsg(string.Format(" {0}读取时 通信失败", name), 2);
                        return "";
                    }
                }
                catch(Exception ex)
                {
                    VisaSheBei[name].CiShu++;
                    ChuFaMsg(string.Format(" {0}读取时:{1}", name,ex), 2);
                    if (VisaSheBei[name].CiShu >= 6)
                    {
                        VisaSheBei[name].IsTX = false;
                    }
                }

            }
            else
            {
                ChuFaMsg(string.Format(" {0}读取时:该设备不存在", name), 2);
                
            }
            return "";
        }

        public  void Close()
        {
            List<string> kjs=  Keys;
            Thread.Sleep(10);
            foreach (var item in kjs)
            {
                try
                {
                    VisaSheBei[item].Visa.Dispose();
                    Thread.Sleep(10);
                }
                catch
                {

                }

            }

        }


        public bool IsTX(string name)
        {
            if (VisaSheBei.ContainsKey(name))
            {
                try
                {
                   
                    return VisaSheBei[name].IsTX;
                }
                catch
                {


                }

            }
            else
            {
                ChuFaMsg(string.Format(" {0}通信时:该设备不存在", name), 2);

            }
            return false;
        }
        private void ChuFaMsg(string msg,int biaozhi)
        {
            if (MsgEvent != null)
            {
                MsgEvent(msg, biaozhi);
            }
        }
    }

    public class VisaModel
    {
        public string SheBeiName { get; set; } = "";

        public bool IsTX { get; set; } = false;

        public int CiShu { get; set; } = 0;
        public MessageBasedSession Visa { get; set; } = null;
    }
}
