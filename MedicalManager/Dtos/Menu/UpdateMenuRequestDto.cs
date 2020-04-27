using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Menu
{
    /// <summary>
    /// 修改菜单 请求
    /// </summary>
    public class UpdateMenuRequestDto : BaseDto
    {
        ///<summary>
        ///ID
        ///</summary>
        [Key, Required(ErrorMessage = "{0}必填"), Display(Name = "ID")]
        public string MenuGuid { get; set; }

        ///<summary>
        ///父菜单GUID
        ///</summary>
        public string ParentGuid { get; set; }

        ///<summary>
        ///图标名称
        ///</summary>
        public string MenuClass { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string MenuName { get; set; }

        ///<summary>
        ///菜单URL（VUE路由地址）
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "菜单URL（VUE路由地址）")]
        public string MenuUrl { get; set; }

        ///<summary>
        ///编码
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "编码")]
        public string MenuCode { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }
}
