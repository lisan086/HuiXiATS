using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.Model;

namespace ViSAKu.Model
{

    /// <summary>
    /// 设备model
    /// </summary>
    public class SheBeiVisaModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int SheBeiID { get; set; } = 0;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 连接名
        /// </summary>
        public string LianJieName { get; set; } = "";

        /// <summary>
        /// true表示通信成功，false 通信失败
        /// </summary>
        public bool IsConnect { get; set; } = false;

        public int XieYanShi { get; set; } = 50;

        /// <summary>
        /// 数据
        /// </summary>
        public List<DataLieModel> LisData { get; set; } = new List<DataLieModel>();
    }
    /// <summary>
    /// 详细数据
    /// </summary>
    public class DataLieModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 命令
        /// </summary>
        public string CMD { get; set; } = "";
        public ShuJuType ShuJuType { get; set; } = ShuJuType.Str;

        public CunType IsDu { get; set; } = CunType.DuShuJu;
        public string MiaoSu { get; set; } = "";
        public int SheBeiID { get; set; }

        /// <summary>
        /// 1表示写完 2表示写失败 0表示正在写
        /// </summary>
        public int IsXieWan { get; set; } = 0;

        public int XieYanShi { get; set; } = 50;

        public JiCunQiModel JiCunQiModel { get; set; } = new JiCunQiModel();

        public void GetValue(string cangshu)
        {
            switch (ShuJuType)
            {
                case ShuJuType.Int:
                    {
                        if (JiCunQiModel != null)
                        {
                            JiCunQiModel.Value = ChangYong.TryDouble(cangshu.Split('\n')[0], -1).ToString("0");
                        }
                    }
                    break;
                case ShuJuType.Str:
                    {
                        if (JiCunQiModel != null)
                        {
                            JiCunQiModel.Value = ChangYong.TryStr(cangshu.Split('\n')[0], "");
                        }
                    }
                    break;
                case ShuJuType.Doule:
                    {
                        if (JiCunQiModel != null)
                        {
                            JiCunQiModel.Value = ChangYong.TryDouble(cangshu.Split('\n')[0], -1);
                        }
                    }
                    break;
               
                default:
                    break;
            }
        }

        public DataLieModel FuZhi()
        {
            DataLieModel model = new DataLieModel();
            model.CMD = CMD;
            model.IsDu = IsDu;
            model.IsXieWan = IsXieWan; 
            model.JiCunQiModel = JiCunQiModel;
            model.MiaoSu = MiaoSu;
            model.Name = Name;
            model.SheBeiID = SheBeiID; 
            model.ShuJuType = ShuJuType;
            model.JiCunQiModel = JiCunQiModel.FuZhi();
            model.XieYanShi = XieYanShi;
            return model;
        }
    }

    public enum ShuJuType
    {
        Int,
        Str,
        Doule,      
    }

    public enum CunType
    {
        [Description("读数据,无需参数")]
        DuShuJu,
        [Description("写数据有返回,需要填写参数,为单个参数")]
        XieYouCanShuFanHui,
        [Description("写数据无返回,需要填写参数,为单个参数")]
        XieYouCanShuWuFanHui,
        [Description("写数据有返回,不需要填写参数")]
        XieWuCanShuFanHui,
        [Description("写数据无返回,不需要填写参数")]
        XieWuCanShuWuFanHui,
    }
}
