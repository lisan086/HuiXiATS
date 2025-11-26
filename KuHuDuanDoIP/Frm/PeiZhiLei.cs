using CommLei.DataChuLi;
using CommLei.JiChuLei;
using KuHuDuanDoIP.Model;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KuHuDuanDoIP.Frm
{
    public class PeiZhiLei
    {
        public event Func<ZhiLingModel,int> XieIOEvent;

    

        public DataMoXing MoXing { get; set; } = null;
        /// <summary>
        /// true 表示是配置
        /// </summary>
        public bool IsPeiZhi { get; set; } = false;

        public bool IsGuanBi { get; set; } = false;
        /// <summary>
        /// true 表示调试
        /// </summary>
        public bool IsTiaoShi { get; set; } = false;
        /// <summary>
        /// 文件名
        /// </summary>
        public string WenJianName { get; set; } = "";

        public string JiLuPathName { get; set; } = "";
        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(int shebeiid,string shebeiname)
        {
            string zhenlujing = GetLuJing();
            MoXing = new DataMoXing();
            MoXing.SheBeiID = shebeiid;
            MoXing.SheBeiName = shebeiname;
            MoXing.IniData(zhenlujing);
        }
        public List<SheBeiModel> GetSheBei()
        {
            string zhenlujing = GetLuJing();
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(zhenlujing);
            List<SheBeiModel> LisSheBesis = JosnOrSModesl.GetLisTModel<SheBeiModel>();
            if (LisSheBesis == null)
            {
                LisSheBesis = new List<SheBeiModel>();
            }
            return LisSheBesis;
        }
        public void BaoCun(List<SheBeiModel> model)
        {
            string zhenlujing = GetLuJing();
            JosnOrSModel JosnOrSModel = new JosnOrSModel(zhenlujing);
            JosnOrSModel.XieTModel(model);
        }

        public int ChuFaXieJiLu(ZhiLingModel model)
        {
            if (XieIOEvent!=null)
            {
                int fanhui= XieIOEvent(model);
                return fanhui;
            }
            return 0;
        }
     

      
        private string GetLuJing()
        {
            string lujing = string.Format("{0}{1}", Directory.GetCurrentDirectory(), @"\SheBeiPeiZhi");
            if (Directory.Exists(lujing) == false)
            {
                Directory.CreateDirectory(lujing);
            }
            string zhenlujing = string.Format(@"{0}\{1}.txt", lujing, WenJianName);
            JiLuPathName= string.Format(@"{0}\{1}.txt", lujing,"DoIpL");
            return zhenlujing;
        }

    }

    class RWCmd
    {
        //构造函数
        public RWCmd()
        {

        }
        public string Cmd(string fileName, string value)
        {
            string lujings = ChangYong.GetWenJianName(fileName);
            int istaxte = lujings.ToLower().Contains("testsa")?1:2;
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            string strRst = "";
            if (process.Start())
            {
                process.StandardInput.WriteLine(value);
                process.StandardInput.AutoFlush = true; //自动聚焦
                process.StandardInput.Close(); //关闭输入
                //strRst = process.StandardOutput.ReadToEnd();
                //process.WaitForExit();
                while (!process.StandardOutput.EndOfStream)
                {
                    string lineStr = process.StandardOutput.ReadLine();
                    if (istaxte == 1)
                    {
                        if (lineStr.Contains("0x"))
                        {
                            strRst += lineStr.Replace("0x", "");
                        }
                    }
                    else if (istaxte==2)
                    {
                        if (lineStr.Contains("-Key]="))
                        {
                            strRst += lineStr.Split('=')[1];
                        }
                    }
                 
                }
            }
            process.Close();
            return strRst;
        }
    }
}
