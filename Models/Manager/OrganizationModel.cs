using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    ///<summary>
    ///组织表模型
    ///</summary>
    [Table("t_manager_organization")]
    public class OrganizationModel : BaseModel
    {
        /// <summary>
        /// guid
        /// </summary>

        [Column("org_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "组织ID")]
        public new string OrgGuid { get; set; }
        ///<summary>
        ///父GUID
        ///</summary>
        [Column("parent_guid")]
        public string ParentGuid { get; set; }

        ///<summary>
        ///组织名称
        ///</summary>
        [Column("org_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "组织名称")]
        public string OrgName { get; set; }
        ///<summary>
        ///排序
        ///</summary>
        [Column("Sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public string Sort { get; set; }
    }
}