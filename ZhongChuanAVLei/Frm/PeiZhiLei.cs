using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZhongWangSheBei.Model;

namespace ZhongWangSheBei.Frm
{
    
    public class PeiZhiLei
    {
       

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
        public void IniData(int shebeiid,string shebeiname)
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
        public List<ZSModel> GetSheBei()
        {
           
            string zhenlujing = GetLuJing(); 
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(zhenlujing);
            List<ZSModel> LisSheBesi = JosnOrSModesl.GetLisTModel<ZSModel>();
            if (LisSheBesi == null)
            {
                LisSheBesi = new List<ZSModel>();
            }
            return LisSheBesi;
        }

        /// <summary>
        /// 配置用的 保存配置设备
        /// </summary>
        /// <param name="shebei"></param>
        public void BaoCun(List<ZSModel> shebei)
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
