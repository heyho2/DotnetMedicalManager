using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    ///<summary>
    ///用户积分表模型
    ///</summary>
    [Table("t_utility_score")]
    public class ScoreModel : BaseModel
    {
        ///<summary>
        ///积分项GUID
        ///</summary>
        [Column("score_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "积分项GUID")]
        public string ScoreGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 规则GUID
        /// </summary>
        [Column("rules_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "规则GUID")]
        public string RulesGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户类型Guid
        ///</summary>
        [Column("user_type_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户类型Guid")]
        public string UserTypeGuid
        {
            get;
            set;
        }

        ///<summary>
        ///积分变化，+/-
        ///</summary>
        [Column("variation"), Required(ErrorMessage = "{0}必填"), Display(Name = "积分变化，+/-")]
        public int Variation
        {
            get;
            set;
        }

        ///<summary>
        ///积分变化原因
        ///</summary>
        [Column("reason"), Required(ErrorMessage = "{0}必填"), Display(Name = "积分变化原因")]
        public string Reason
        {
            get;
            set;
        }
        /// <summary>
        /// 平台类型
        /// </summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        /// <summary>
        /// 是否锁定
        /// </summary>
        [Column("score_lock"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否锁定")]
        public bool ScoreLock { get; set; } = false;

        /// <summary>
        /// 积分记录guid
        /// </summary>
        [Column("score_record_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "积分记录guid")]
        public string ScoreRecordGuid { get; set; }
    }
}