using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoTaiKeTXLei.Modle;
using SSheBei.Model;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace BoTaiKeTXLei.Frm
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
        /// 设备名称
        /// </summary>
        public string PeiZhiZhiLingLu { get; set; } = "";
        /// <summary>
        /// 读寄存器
        /// </summary>
        public List<JiCunQiModel> LisDu = new List<JiCunQiModel>();
        /// <summary>
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> LisXie = new List<JiCunQiModel>();

        /// <summary>
        /// 设备
        /// </summary>
        public List<YiQiModel> LisSheBei = new List<YiQiModel>();

        public List<string> IPS = new List<string>();

        /// <summary>
        /// 写标识的对应 key表示寄存器的唯一表示
        /// </summary>
        public Dictionary<string, SendModel> JiLu = new Dictionary<string, SendModel>();

        private List<string> KeyS = new List<string>();
        public Dictionary<string, string> Data = new Dictionary<string, string>();
        public Dictionary<string, List<byte>> Data1 = new Dictionary<string, List<byte>>();
        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            LisDu.Clear();
            LisXie.Clear();
            JiLu.Clear();
            Data.Clear();
            IPS.Clear();
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<YiQiModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<YiQiModel>();
            }
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                IPS.Add(LisSheBei[i].IP);
            }
            List<string> mingling = ChangYong.MeiJuLisName(typeof(CunType));
            for (int i = 0; i < mingling.Count; i++)
            {
                JiCunQiModel model = new JiCunQiModel();
                model.SheBeiID = SheBeiID;
                model.WeiYiBiaoShi = mingling[i];
                model.MiaoSu =ChangYong.GetEnumDescription(ChangYong.GetMeiJuZhi<CunType>(mingling[i]));
                model.DuXie = 2;
                LisXie.Add(model);
                if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                {
                    SendModel sendModel = new SendModel();
                    sendModel.JiCunQiModel = model;
                    sendModel.IsABO = ChangYong.GetMeiJuZhi<CunType>(mingling[i]);
                    JiLu.Add(model.WeiYiBiaoShi, sendModel);
                }
             
            }    
            KeyS = JiLu.Keys.ToList();
        }


        public void SetHeGe(string zongid,bool hege)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].IP == zongid)
                {
                    LisSheBei[i].TX = hege;                 
                    break;
                }
            }

        }

        public void SetJiCunQiValue(SendModel model, string shuju)
        {
            if (JiLu.ContainsKey(model.JiCunQiModel.WeiYiBiaoShi))
            {
                SendModel cunModel = JiLu[model.JiCunQiModel.WeiYiBiaoShi];
                cunModel.JiCunQiModel.IsKeKao = true;
                cunModel.JiCunQiModel.Value = shuju;
            }

        }
        public void SetJiCunQiValue(SendModel model, List<byte> shusju)
        {
            List<string> shuju = Log(shusju);
            if (Data.ContainsKey(model.Name) == false)
            {
                Data.Add(model.Name, shuju[1]);
            }
            else
            {
                Data[model.Name] = shuju[1];
            }
            if (JiLu.ContainsKey(model.JiCunQiModel.WeiYiBiaoShi))
            {
                SendModel cunModel = JiLu[model.JiCunQiModel.WeiYiBiaoShi];
                cunModel.JiCunQiModel.IsKeKao = true;
                cunModel.JiCunQiModel.Value = shuju[0];             
            }
     

        }
        public void SetZhengZaiValue(JiCunQiModel model, int sate)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                SendModel cunModel = JiLu[model.WeiYiBiaoShi];
                cunModel.IsZhengZaiCe = sate;
                if (sate == 0)
                {
                    cunModel.JiCunQiModel.Value = "";
                    if (cunModel.IsABO != CunType.XieGetZiShuJu)
                    {
                        Data.Clear();
                        Data1.Clear();
                    }
                }

            }

        }
        public string GetData()
        {
            if (Data1.ContainsKey("lg"))
            {
                return ChangYong.ByteOrString(Data1["lg"]," ");
            }
            return "";
        }
        public object QuShuJu(string model)
        {
            string[] fenfg = ChangYong.TryStr(model,"").Split('#');
            if (fenfg.Length>=3)
            {
                if (Data.ContainsKey(fenfg[0]))
                {
                    object type = ChangYong.GetMeiJuZhi(typeof(QuZhiType), fenfg[1]);
                    if (type != null)
                    {
                        QuZhiType quZhiType = (QuZhiType)type;
                        if (quZhiType == QuZhiType.FG)
                        {
                            string[] canshu = fenfg[2].Split('*');
                            if (canshu.Length >= 2)
                            {
                                int weishu = ChangYong.TryInt(canshu[1], 0);
                                if (weishu < 0)
                                {
                                    weishu = 0;
                                }
                                if (canshu[0].Contains("空"))
                                {
                                    string[] dd = Data[fenfg[0]].Split(new string[] { " " }, StringSplitOptions.None);
                                    if (dd.Length > weishu)
                                    {
                                        return dd[weishu];
                                    }
                                }
                                else
                                {
                                    string[] dd = Data[fenfg[0]].Split(new string[] { canshu[0] }, StringSplitOptions.None);
                                    if (dd.Length > weishu)
                                    {
                                        return dd[weishu];
                                    }
                                }

                            }
                        }
                        else if (quZhiType == QuZhiType.QC)
                        {
                            string[] canshu = fenfg[2].Split('*');
                            if (canshu.Length >= 2)
                            {
                                int weishu1 = ChangYong.TryInt(canshu[0], 0);
                                if (weishu1 < 0)
                                {
                                    weishu1 = 0;
                                }
                                int weishu2 = ChangYong.TryInt(canshu[1], 0);
                                if (weishu2 < 0)
                                {
                                    weishu2 = 0;
                                }
                                try
                                {
                                    return Data[fenfg[0]].Substring(weishu1, weishu2);
                                }
                                catch
                                {


                                }


                            }
                        }
                        else if (quZhiType == QuZhiType.ZYJQ)
                        {
                            string[] canshu = fenfg[2].Split('*');
                            if (canshu.Length >= 2)
                            {
                                string weishu1 = canshu[0];
                                string weishu2 = canshu[1];

                                try
                                {
                                    return ChangYong.StrDataCut(Data[fenfg[0]], weishu1, weishu2, 2);
                                }
                                catch
                                {


                                }


                            }
                        }
                        else if (quZhiType==QuZhiType.LCFG)
                        {
                            string[] canshu = fenfg[2].Split('*');
                            if (canshu.Length >= 3)
                            {
                                string weishu1 = canshu[0];
                                string weishu2 = canshu[1];
                                int weishu3 = ChangYong.TryInt(canshu[2], 0);
                                if (weishu3 < 0)
                                {
                                    weishu3 = 0;
                                }

                                try
                                {
                                    if (canshu[0].Contains("空"))
                                    {
                                        string[] dd = Data[fenfg[0]].Split(new string[] { " " }, StringSplitOptions.None);
                                        if (dd.Length > weishu3)
                                        {
                                            string sss = dd[weishu3];
                                            if (string.IsNullOrEmpty(weishu2))
                                            {
                                                return sss;
                                            }
                                            else
                                            {

                                                string[] ddw = sss.Split(new string[] { weishu2 }, StringSplitOptions.None);

                                                return ddw.Length > 1 ? ddw[0] : ddw[0];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string[] dd = Data[fenfg[0]].Split(new string[] { canshu[0] }, StringSplitOptions.None);
                                        if (dd.Length > weishu3)
                                        {
                                            string sss = dd[weishu3];
                                            if (string.IsNullOrEmpty(weishu2))
                                            {
                                                return sss;
                                            }
                                            else
                                            {

                                                string[] ddw = sss.Split(new string[] { weishu2 }, StringSplitOptions.None);

                                                return ddw.Length > 1 ? ddw[0] : ddw[0];
                                            }
                                        }
                                    }

                                }
                                catch
                                {


                                }


                            }
                        }
                        else
                        {
                            return Data[fenfg[0]];
                        }
                    }
                    else
                    {
                        return Data[fenfg[0]];
                    }
                  
                }
            }
            return null;
        }

        public object QuShuJuN(string model,int iserjinzhi)
        {
            string[] fenfg = ChangYong.TryStr(model, "").Split('#');
            if (fenfg.Length >= 3)
            {
                if (Data1.Keys.Count > 0)
                {
                    int weishu1 = ChangYong.TryInt(fenfg[1], 0);
                    if (weishu1 < 0)
                    {
                        weishu1 = 0;
                    }
                    int weishu2 = ChangYong.TryInt(fenfg[2], 0);
                    if (weishu2 < 0)
                    {
                        weishu2 = 0;
                    }
                    List<byte> shug = Data1["lg"];
                    try
                    {
                        if (iserjinzhi == 1)
                        {
                            List<byte> lis = shug.GetRange(weishu1, weishu2);
                            return ChangYong.ByteOrString(lis," ");
                        }
                        else
                        {
                            List<byte> lis = shug.GetRange(weishu1, weishu2);
                            return Encoding.ASCII.GetString(lis.ToArray()).Trim().Replace("\0", "");
                        }
                    }
                    catch
                    {


                    }
                }
            
            }
            return "";
        }



        /// <summary>
        /// 1是成功 0是未测完 3 不存在 其他表示超时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SendModel IsChengGong(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                SendModel cunModel = JiLu[model.WeiYiBiaoShi];
                return cunModel;
            }
            return null;
        }

        public SendModel GetModel(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                SendModel cunModel = JiLu[model.WeiYiBiaoShi];
                return cunModel.FuZhi();
            }
            return null;
        }
        public SendModel GetModel(CunType cunType,bool isfuzhi=true)
        {
            for (int i = 0; i < KeyS.Count; i++)
            {
                SendModel cunModel = JiLu[KeyS[i]];
                if (cunModel.IsABO == cunType)
                {
                    if (isfuzhi)
                    {
                        SendModel xinmodel = cunModel.FuZhi();

                        return xinmodel;
                    }
                    else
                    {
                        return cunModel;
                    }
                }
            }
           
            return null;
        }

        public YiQiModel GetSheBeiModel(SendModel model)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].IP == model.IP)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }
        public YiQiModel GetSheBeiModel(string ip)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].IP == ip)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }

      
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public byte[] SendDate(SendModel sendModel,int zongchang)
        {
            

            StringBuilder ChongGouParam = new StringBuilder();
            ChongGouParam.Append(sendModel.CMD);
            ChongGouParam.Append(sendModel.Status);
            if (true)
            {
                ChongGouParam.Append(sendModel.Param);
            }         
            Byte[] ShuJu =ChangYong.HexStringToByte(ChongGouParam.ToString());

            List<byte> shujulist = ShuJu.ToList();

            int chazhi = zongchang - shujulist.Count;
            for (int i = 0; i < chazhi; i++)
            {
                shujulist.Add(0);
            }
            return shujulist.ToArray();
        }

        public List<string> Log(List<byte> shujulist)
        {
            if (shujulist.Count >= 232)
            {
                List<string> lis= new List<string>();           
                lis.Add($"{ChangYong.ByteOrString(shujulist.Skip(0).Take(8).ToArray<byte>()," ")}");
                byte[] banbenid = shujulist.Skip(8).Take(224).ToArray<byte>();
                String banben = Encoding.ASCII.GetString(banbenid).Trim().Replace("\0","");
                lis.Add($"{banben}");
                if (Data1.ContainsKey("lg"))
                {
                    Data1.Add("lg", banbenid.ToList());
                }
                else
                {
                    Data1["lg"] = banbenid.ToList();
                }
                
                return lis;

            }
            else
            {
                if (Data1.ContainsKey("lg"))
                {
                    Data1.Add("lg", new List<byte>());
                }
                else
                {
                    Data1["lg"].Clear();
                }

                return new List<string>() { "",""};
            }
        }

        private string ExecuteCommand(string workingDirectory, string command,int type,int timeout)
        {
            if (timeout<=0)
            {
                timeout = 25 * 1000;
            }
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.WorkingDirectory = workingDirectory;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.Arguments = $"/C {command}";
                p.Start();



                StringBuilder output = new StringBuilder();
                p.OutputDataReceived += (sender, args) =>
                {
                    try
                    {
                        output.AppendLine(args.Data);
                    }
                    catch
                    {


                    }

                };

                p.ErrorDataReceived += (sender, args) =>
                {
                    try
                    {
                        output.AppendLine(args.Data);
                    }
                    catch
                    {


                    }
                };

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit(timeout);
                p.CancelErrorRead();
                p.CancelOutputRead();
                p.Close();
                return output.ToString();
            }
        }


        public void BaoCunJiLu(string lujing, SendModel model)
        {
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            List<SendModel> lis = JosnOrSModel.GetLisTModel<SendModel>();
            if (lis == null)
            {
                lis = new List<SendModel>();
            }
            bool zhen = false;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i].Name.Equals(model.Name))
                {
                    lis[i]= model;
                    zhen = true;
                    break;
                }
            }
            if (zhen==false)
            {
         
                lis.Add(model);
            }
            JosnOrSModel.XieTModel(lis);
        }

        public List<SendModel> GetJiLuData(string lujing)
        {
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            List<SendModel> lis = JosnOrSModel.GetLisTModel<SendModel>();
            if (lis == null)
            {
                lis = new List<SendModel>();
            }
            return lis;
        }
    }
}
