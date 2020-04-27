using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    ///<summary>
    ///角色授权表模型
    ///</summary>
    [Table("t_manager_grant_role")]
    public class GrantRoleModel : BaseModel
    {
        ///<summary>
        ///授权GUID
        ///</summary>
        [Column("grant_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "授权GUID")]
        public string GrantGuid
        {
            get;
            set;
        }

        ///<summary>
        ///角色ID
        ///</summary>
        [Column("role_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "角色ID")]
        public string RoleGuid
        {
            get;
            set;
        }

        ///<summary>
        ///权限ID
        ///</summary>
        [Column("right_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "权限ID")]
        public string RightGuid
        {
            get;
            set;
        }
    }
}