using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    ///<summary>
    ///角色定义表模型
    ///</summary>
    [Table("t_manager_role")]
    public class RoleModel : BaseModel
    {
        ///<summary>
        ///角色GUID
        ///</summary>
        [Column("role_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "角色GUID")]
        public string RoleGuid { get; set; }

        ///<summary>
        ///角色名称
        ///</summary>
        [Column("role_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "角色名称")]
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column("Description")]
        public string Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column("Sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public string Sort { get; set; }
    }
}