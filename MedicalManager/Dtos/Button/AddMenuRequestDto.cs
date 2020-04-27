using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Button
{
    /// <summary>
    /// 添加菜单 请求
    /// </summary>
    public class AddButtonRequestDto : BaseDto
    {
        

        ///<summary>
        ///名称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string ButtonName { get; set; }
        

        ///<summary>
        ///编码
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "编码")]
        public string ButtonCode { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 菜单id
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "菜单id")]
        public string MenuGuid { get; set; }
    }
}
