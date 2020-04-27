using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Admin.User
{
    /// <summary>
    /// 添加角色 请求
    /// </summary>
    public class AddRoleRequestDto : BaseDto
    {

        ///<summary>
        ///角色名称
        ///</summary>
        [Required]
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(100)]
        public string Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public string Sort { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }
}
