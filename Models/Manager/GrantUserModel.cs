using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    ///<summary>
    ///用户授权表模型
    ///</summary>
    [Table("t_manager_grant_user")]
    public class GrantUserModel : BaseModel
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
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid
        {
            get;
            set;
        }

        ///<summary>
        ///角色GUID
        ///</summary>
        [Column("role_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "角色GUID")]
        public string RoleGuid
        {
            get;
            set;
        }
    }
}