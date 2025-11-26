using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using CommLei.DataChuLi;
using SSheBei.Model;
using YiBanSaoMaQi.Model;

namespace YiBanSaoMaQi.Frm
{
    public class PeiZhiLei
    {

        public Dictionary<string, ManagementObject> WangKas = new Dictionary<string, ManagementObject>();
        public event Action<JiCunQiModel> XieIOEvent;

        private DataMoXing MoXing = null;
        /// <summary>
        /// 数据模型
        /// </summary>
        public DataMoXing DataMoXing { get { return MoXing; } }
        /// <summary>
        /// true 表示是配置
        /// </summary>
        public bool IsPeiZhi { get; set; } = false;
        /// <summary>
        /// true 表示调试
        /// </summary>
        public bool IsTiaoShi { get; set; } = false;
        /// <summary>
        /// 文件名
        /// </summary>
        public string WenJianName { get; set; } = "";


        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(int shebeiid, string shebeiname)
        {
            string zhenlujing = GetLuJing();
            MoXing = new DataMoXing();
            MoXing.SheBeiID = shebeiid;
            MoXing.SheBeiName = shebeiname;
            MoXing.IniData(zhenlujing);
        }

        /// <summary>
        /// 配置用的 获取配置设备
        /// </summary>
        /// <returns></returns>
        public List<SaoMaModel> GetSheBei()
        {

            string zhenlujing = GetLuJing();
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(zhenlujing);
            List<SaoMaModel> LisSheBesi = JosnOrSModesl.GetLisTModel<SaoMaModel>();
            if (LisSheBesi == null)
            {
                LisSheBesi = new List<SaoMaModel>();
            }
            return LisSheBesi;
        }

        public List<string> GetWangKa()
        {
            WangKas.Clear();
            List<string> wenjian = new List<string>();
            string manage = "SELECT * From Win32_NetworkAdapter";//  WHERE Name='本地连接'
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(manage);
            ManagementObjectCollection collection = searcher.Get();
            foreach (ManagementObject obj in collection)
            {

                PropertyDataCollection zhi = obj.Properties;

                foreach (PropertyData item in zhi)
                {
                    string zhis = item.Name;
                    object ssd = item.Value;
                    if (ssd != null && zhis == "NetConnectionID")//
                    {

                        wenjian.Add($"{ssd.ToString()}");

                        if (WangKas.ContainsKey(ssd.ToString()) == false)
                        {
                            WangKas.Add(ssd.ToString(), obj);
                        }
                    }
                }

            }
            return wenjian;
        }

        /// <summary>
        /// 配置用的 保存配置设备
        /// </summary>
        /// <param name="shebei"></param>
        public void BaoCun(List<SaoMaModel> shebei)
        {

            string zhenlujing = GetLuJing();
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(zhenlujing);
            JosnOrSModesl.XieTModel(shebei);
        }


      

        public void XieJiDianQi(JiCunQiModel model)
        {
            if (XieIOEvent != null)
            {
                XieIOEvent(model);
            }
        }


        private string GetLuJing()
        {
            string lujing = string.Format("{0}{1}", Directory.GetCurrentDirectory(), @"\SheBeiPeiZhi");
            if (Directory.Exists(lujing) == false)
            {
                Directory.CreateDirectory(lujing);
            }
            string zhenlujing = string.Format(@"{0}\{1}.txt", lujing, WenJianName);
            return zhenlujing;
        }
    }
}
