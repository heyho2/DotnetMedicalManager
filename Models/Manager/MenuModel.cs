using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Manager
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    [Table("t_manager_menu")]

    public class MenuModel : BaseModel
    {
        ///<summary>
        ///ID
        ///</summary>
        [Column("menu_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "ID")]
        public string MenuGuid { get; set; }

        ///<summary>
        ///父菜单GUID
        ///</summary>
        [Column("parent_guid")]
        public string ParentGuid { get; set; }

        ///<summary>
        ///图标名称
        ///</summary>
        [Column("menu_class")]
        public string MenuClass { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [Column("menu_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string MenuName { get; set; }

        ///<summary>
        ///菜单URL（VUE路由地址）
        ///</summary>
        [Column("menu_url"), Required(ErrorMessage = "{0}必填"), Display(Name = "菜单URL（VUE路由地址）")]
        public string MenuUrl { get; set; }

        ///<summary>
        ///编码
        ///</summary>
        [Column("menu_code"), Required(ErrorMessage = "{0}必填"), Display(Name = "编码")]
        public string MenuCode { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort")]
        public int Sort { get; set; }
    }
}
