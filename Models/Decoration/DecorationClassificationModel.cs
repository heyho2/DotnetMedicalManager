using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Decoration
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_decoration_classification")]
    public class DecorationClassificationModel : BaseModel
    {

        /// <summary>
        /// 分类GUID
        /// </summary>
        [Column("classification_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "分类GUID")]
        public string ClassificationGuid { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Column("classification_name")]
        public string ClassificationName { get; set; }

        /// <summary>
        /// 规则选择模式:多选或单选
        /// </summary>
        [Column("rule_mode")]
        public string RuleMode { get; set; }
    }
}



