using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    ///<summary>
    ///权限定义表模型
    ///</summary>
    [Table("t_manager_right")]
    public class RightModel : BaseModel
    {
        ///<summary>
        ///权限GUID
        ///</summary>
        [Column("right_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "权限GUID")]
        public string RightGuid { get; set; }

        ///<summary>
        ///权限名
        ///</summary>
        [Column("right_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "权限名")]
        public string RightName { get; set; }
        ///<summary>
        ///类型
        ///</summary>
        [Column("right_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "类型")]
        public string RightType { get; set; } = RightTypeEnum.Button.ToString();

        /// <summary>
        /// 类型
        /// </summary>
        public enum RightTypeEnum
        {
            /// <summary>
            /// 菜单
            /// </summary>
            [Description("菜单")]
            Menu,
            /// <summary>
            /// 按钮
            /// </summary>
            [Description("按钮")]
            Button
        }
    }
}