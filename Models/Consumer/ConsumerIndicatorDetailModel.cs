using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 用户日常指标明细表
    /// </summary>
    [Table("t_consumer_indicator_detail")]
    public class ConsumerIndicatorDetailModel : BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("record_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string RecordDetailGuid { get; set; }

        /// <summary>
        /// 用户健康指标guid
        /// </summary>
        [Column("indicator_record_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户健康指标guid")]
        public string IndicatorRecordGuid { get; set; }

        /// <summary>
        /// 用户健康指标选项guid
        /// </summary>
        [Column("indicator_option_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户健康指标选项guid")]
        public string IndicatorOptionGuid { get; set; }

        /// <summary>
        /// 指标值
        /// </summary>
        [Column("indicator_value"), Display(Name = "指标值")]
        public decimal? IndicatorValue { get; set; }
    }
}



