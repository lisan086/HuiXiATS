using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSheBei.Model;
using SundyChengZhong.Model;
using Common.DataChuLi;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.LianJieQi;
using System.Threading;
using SSheBei.CRCJiaoYan;

namespace SundyChengZhong.Frm
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
        public void IniData(int shebeid,string name)
        {           
            string zhenlujing = GetLuJing();
            MoXing=new DataMoXing();
            MoXing.SheBeiID = shebeid;
            MoXing.SheBeiName = name;
            MoXing.IniData(zhenlujing);
        }

        /// <summary>
        /// 配置用的
        /// </summary>
        /// <returns></returns>
        public List<SheBeiModel> GetSheBei()
        {

            string zhenlujing = GetLuJing();
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(zhenlujing);
            List<SheBeiModel> LisSheBesi = JosnOrSModesl.GetLisTModel<SheBeiModel>();
            if (LisSheBesi == null)
            {
                LisSheBesi = new List<SheBeiModel>();
            }
            return LisSheBesi;
        }

        /// <summary>
        /// 配置用的
        /// </summary>
        /// <param name="shebei"></param>
        public void BaoCun(List<SheBeiModel> shebei)
        {

            string zhenlujing = GetLuJing();
            JosnOrSModel JosnOrSModesl = new JosnOrSModel(zhenlujing);
            JosnOrSModesl.XieTModel(shebei);
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


        public void ChuFaShiJian(JiCunQiModel model)
        {
            if (XieIOEvent != null)
            {
                XieIOEvent(model);
            }
        }

    }
}
