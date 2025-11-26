using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.LianJieQi;
using SSheBei.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViSaJiChu;
using ViSAKu.Model;

namespace ViSAKu.Frm
{
    public class PeiZhiLei
    {

        public event Func<JiCunQiModel,string> XieIOEvent;

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

        public VisaGuanXin VisaGuanXin { get; set; }


        public List<string> LianJieNames { get; set; } = new List<string>();
        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(int shebeiid, string shebeiname)
        {
            MoXing = new DataMoXing();
            MoXing.SheBeiID = shebeiid;
            MoXing.SheBeiName = shebeiname;
            string lujing = GetLuJing();
            MoXing.IniData(lujing);
        }

        /// <summary>
        /// 获取设备(用于配置)
        /// </summary>
        /// <returns></returns>
        public List<SheBeiVisaModel> GetSheBei()
        {
            string lujing = GetLuJing();
          
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(lujing);
            List<SheBeiVisaModel> LisSheBesi = JosnOrSModesl.GetLisTModel<SheBeiVisaModel>();
            if (LisSheBesi == null)
            {
                LisSheBesi = new List<SheBeiVisaModel>();
            }
            return LisSheBesi;
        }

        /// <summary>
        /// 保存设备(用于配置)
        /// </summary>
        /// <param name="shebei"></param>
        public void BaoCun(List<SheBeiVisaModel> shebei)
        {
            string lujing = GetLuJing();
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(lujing);
            JosnOrSModesl.XieTModel(shebei);
        }

        public string XieJiDianQi(JiCunQiModel model)
        {
            if (XieIOEvent != null)
            {
                return XieIOEvent(model);
            }
            return "没有注册";
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
