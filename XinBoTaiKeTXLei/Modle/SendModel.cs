using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.Model;

namespace BoTaiKeTXLei.Modle
{
    public class SendModel
    {
        /// <summary>
        /// 发送指令-16进制
        /// </summary>
        public string CMD { get; set; } = "";
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; } = "";
        /// <summary>
        /// 参数
        /// </summary>
        public string Param { get; set; } = "";
        
        /// <summary>
        /// 0-是进行中 1是完成 2是失败 3是超时失败
        /// </summary>
        public int IsZhengZaiCe { get; set; } = 0;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; } = "";

  

        public CunType IsABO { get; set; } = CunType.XieBoTaiKeFanHui8;

   

        public JiCunQiModel JiCunQiModel { get; set; } = new JiCunQiModel();



        public SendModel FuZhi()
        {
            SendModel model = new SendModel();
            model.CMD = CMD;
            model.Status = Status;
            model.Param = Param;         
            model.IP = IP;
            model.IsABO = IsABO;
            model.IsZhengZaiCe = IsZhengZaiCe;
            model.Name = Name;
            model.JiCunQiModel = JiCunQiModel.FuZhi();
            return model;
        }

        public void SetCanShu(string canshu)
        {
            string[] fenge = canshu.Split('#');
            if (fenge.Length>=4)
            {
                Name= fenge[0];
                CMD = fenge[1];
                Status ="00 00 00 00";
                Param= fenge[2];
                IP= fenge[3];
              
            }
        }
    }

    public enum CunType
    {
        [Description("写指令返回前面8个数据，参数:name#cmd#Param#ip")]
        XieBoTaiKeFanHui8,
        [Description("获取总命令下的子参数，参数:name#(FG,QC,ZYJQ,WU,LCFG)#(canshu1*canshu2*canshu3)")]
        XieGetZiShuJu,
        [Description("写指令不返回，参数:name#cmd#Param#ip")]
        XieBoTaiKeWuFanHui,
        [Description("连接车机，不需要参数")]
        XieOpenBoTaiKe,
        [Description("关闭车机，不需要参数")]
        XieCloseBoTaiKe,
    }

    public enum QuZhiType
    {
        FG,
        QC,
        ZYJQ,
        LCFG,
        Wu,
    }

    
    
}
