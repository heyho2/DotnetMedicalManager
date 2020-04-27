using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 用户健康信息表
    /// </summary>
    [Table("t_consumer_health_info")]
    public class ConsumerHealthInfoModel : BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("info_record_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string InfoRecordGuid { get; set; }

        /// <summary>
        /// 健康信息问题guid
        /// </summary>
        [Column("information_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "健康信息问题guid")]
        public string InformationGuid { get; set; }

        /// <summary>
        /// 用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 健康信息选项guid数组
        /// </summary>
        [Column("option_guids")]
        public string OptionGuids { get; set; }

        /// <summary>
        /// 填空值
        /// </summary>
        [Column("result_value")]
        public string ResultValue { get; set; }
        /// <summary>
        /// 数值指标:numericalvalue,单选指标: singleelection,多选指标:multipleselection,问答指标:questionsandanswers
        /// </summary>
        [Column("information_type"), Display(Name = "题目类型：单选、判断、数值、文本、多选")]
        public string InformationType { get; set; }
    }
}



