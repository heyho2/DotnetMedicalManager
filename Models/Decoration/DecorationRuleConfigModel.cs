using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Decoration
{
    /// <summary>
    /// 装修规则配置表
    /// </summary>
    [Table("t_decoration_rule_config")]
    public class DecorationRuleConfigModel : BaseModel
    {
        /// <summary>
        /// 装修和规则关系GUID
        /// </summary>
        [Column("config_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "装修和规则关系GUID")]
        public string ConfigGuid { get; set; }

        /// <summary>
        /// 装修类别GUID
        /// </summary>
        [Column("classification_guid")]
        public string ClassificationGuid { get; set; }

        /// <summary>
        /// 规则GUID
        /// </summary>
        [Column("rule_guid")]
        public string RuleGuid { get; set; }
    }
}



