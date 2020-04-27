using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Manager
{
    /// <summary>
    /// 系统按钮
    /// </summary>
    [Table("t_manager_button")]

    public class ButtonModel : BaseModel
    {
        ///<summary>
        ///GUID
        ///</summary>
        [Column("button_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string ButtonGuid { get; set; }

        ///<summary>
        ///菜单GUID
        ///</summary>
        [Column("menu_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "菜单GUID")]
        public string MenuGuid { get; set; }

        ///<summary>
        ///菜单编码（CONTROLLER/ACTION）
        ///</summary>
        [Column("button_code")]
        public string ButtonCode { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [Column("button_name")]
        public string ButtonName { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort")]
        public int Sort { get; set; }
    }
}
