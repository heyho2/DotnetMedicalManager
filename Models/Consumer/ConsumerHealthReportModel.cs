using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 用户健康报告表
    /// </summary>
    [Table("t_consumer_health_report")]
    public class ConsumerHealthReportModel : BaseModel
    {
        /// <summary>
        /// 报告主键guid
        /// </summary>
        [Column("report_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "报告主键guid")]
        public string ReportGuid { get; set; }

        /// <summary>
        /// 报告名称
        /// </summary>
        [Column("report_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "报告名称")]
        public string ReportName { get; set; }

        /// <summary>
        /// 报告建议
        /// </summary>
        [Column("suggestion")]
        public string Suggestion { get; set; }

        /// <summary>
        /// 用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
        public string UserGuid { get; set; }
    }
}



