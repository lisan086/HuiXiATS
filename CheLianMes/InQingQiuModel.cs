using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheLianMes
{
    public class InQingQiuModel
    {

    }

    public class InQianGongXuJianCeModel: InQingQiuModel
    {
        /// <summary>
        /// 工作站编码
        /// </summary>
        public string workStation { get; set; } = "";
        /// <summary>
        /// 员工编码
        /// </summary>
        public string empCode { get; set; } = "";
        /// <summary>
        /// MES系统生成的产品SN
        /// </summary>
        public string endProductSn { get; set; } = "";
        /// <summary>
        /// RFID编码，为空时直接使用顶盖SN
        /// </summary>
        public string rfid { get; set; } = "";

    }

    public class InGuoZhanDataModel : InQingQiuModel
    {
        /// <summary>
        /// 工作站编码
        /// </summary>
        public string workStation { get; set; } = "";
        /// <summary>
        /// 员工编码
        /// </summary>
        public string empCode { get; set; } = "";
        /// <summary>
        /// MES系统生成的产品SN
        /// </summary>
        public string endProductSn { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string errorCode { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string beginTime { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string endTime { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string BrandType { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string ModelName { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string MachineName { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string ReportResult { get; set; } = "P";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string ConfirmedResult { get; set; } = "P";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string TotalComponent { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string reportFailComponent { get; set; } = "";
        /// <summary>
        /// 就是错误号
        /// </summary>
        public string confirmedFailComponent { get; set; } = "";

        public List<GuoZhanDatasModel> details { get; set; } = new List<GuoZhanDatasModel>();

    }

    public class GuoZhanDatasModel
    {
        /// <summary>
        /// 工作站编码
        /// </summary>
        public string ComponentName { get; set; } = "";
        /// <summary>
        /// 工作站编码
        /// </summary>
        public string ReportResult { get; set; } = "";
        /// <summary>
        /// 工作站编码
        /// </summary>
        public string ReportResultCode { get; set; } = "";
        /// <summary>
        /// 工作站编码
        /// </summary>
        public string ConfirmResult { get; set; } = "";
        /// <summary>
        /// 工作站编码
        /// </summary>
        public string ConfirmResultCode { get; set; } = "";

    }
}
