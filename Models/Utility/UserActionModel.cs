using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    /// <summary>
    /// 用户行为Model
    /// </summary>
    [Table("t_utility_user_action")]
    public class UserActionModel : BaseModel
    {
        /// <summary>
        /// 用户行为GUID
        /// </summary>
        [Column("user_action_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "用户行为GUID")]
        public string UserActionGuid { get; set; }

        /// <summary>
        /// 用户类型GUID
        /// </summary>
        [Column("user_type_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户类型GUID")]
        public string UserTypeGuid { get; set; }

        /// <summary>
        /// 操作GUID
        /// </summary>
        [Column("action_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "操作名称")]
        public string ActionGuid { get; set; }
    }
}