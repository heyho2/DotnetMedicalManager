using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 健康报告详情
    /// </summary>
    [Table("t_consumer_health_report_detail")]
    public class ConsumerHealthReportDetailModel : BaseModel
    {
        /// <summary>
        /// 健康报告详情主键
        /// </summary>
        [Column("report_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "健康报告详情主键")]
        public string ReportDetailGuid { get; set; }

        /// <summary>
        /// 所属健康报告guid
        /// </summary>
        [Column("report_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属健康报告guid")]
        public string ReportGuid { get; set; }

        /// <summary>
        /// 附件guid
        /// </summary>
        [Column("accessory_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "附件guid")]
        public string AccessoryGuid { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        [Column("accessory_name")]
        public string AccessoryName { get; set; }
    }
}



