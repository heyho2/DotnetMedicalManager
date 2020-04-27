using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 用户指标主表
    /// </summary>
    [Table("t_consumer_indicator")]
    public class ConsumerIndicatorModel : BaseModel
    {
        /// <summary>
        /// guid
        /// </summary>
        [Column("indicator_record_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "guid")]
        public string IndicatorRecordGuid { get; set; }

        /// <summary>
        /// 健康指标guid
        /// </summary>
        [Column("indicator_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "健康指标guid")]
        public string IndicatorGuid { get; set; }

        /// <summary>
        /// 用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 排序默认值0
        /// </summary>
        [Column("sort"), Display(Name = "排序默认值0")]
        public int Sort { get; set; }
    }
}



