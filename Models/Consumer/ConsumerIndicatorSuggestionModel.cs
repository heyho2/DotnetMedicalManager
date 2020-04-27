using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 用户指标建议主表
    /// </summary>
    [Table("t_consumer_indicator_suggestion")]
    public class ConsumerIndicatorSuggestionModel : BaseModel
    {
        /// <summary>
        /// guid
        /// </summary>
        [Column("indicator_suggestion_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "guid")]
        public string IndicatorSuggestionGHuid { get; set; }

        /// <summary>
        /// 用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 医生建议
        /// </summary>
        [Column("suggestion")]
        public string Suggestion { get; set; }
    }
}



