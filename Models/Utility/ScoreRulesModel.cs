using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;
using GD.Common.EnumDefine;

namespace GD.Models.Utility
{
    /// <summary>
    /// 积分规则model
    /// </summary>
    [Table("t_utility_score_rules")]
    public class ScoreRulesModel : BaseModel
    {
        /// <summary>
        /// 规则GUID
        /// </summary>
        [Column("rules_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "规则GUID")]
        public string RulesGuid { get; set; }

        /// <summary>
        /// 用户行为GUID
        /// </summary>
        [Column("user_action_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户操作GUID")]
        public string UserActionGuid { get; set; }

        /// <summary>
        /// 规则类型:Unlimited(无限制); Year(每年); Month(每月); Week(每周); Day(每天);
        /// </summary>
        [Column("rules_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "规则类型")]
        public ScoreRulesCycle RulesType { get; set; }

        /// <summary>
        /// 使用次数
        /// </summary>
        [Column("usage_count"), Required(ErrorMessage = "{0}必填"), Display(Name = "使用次数")]
        public int UsageCount { get; set; }

        /// <summary>
        /// 用户类型GUID，关联字典表
        /// </summary>
        [Column("user_type_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户类型GUID")]
        public string UserTypeGuid { get; set; }

        /// <summary>
        /// 积分变化，+/-
        /// </summary>
        [Column("variation"), Required(ErrorMessage = "{0}必填"), Display(Name = "积分变化")]
        public int Variation { get; set; }
    }
}