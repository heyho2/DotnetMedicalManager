using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Manager
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Table("t_manager_account_role")]
    public class AccountRoleModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("arguid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string Arguid { get; set; }

        ///<summary>
        ///账号ID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "账号ID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///角色ID
        ///</summary>
        [Column("role_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "角色ID")]
        public string RoleGuid { get; set; }

    }
}
