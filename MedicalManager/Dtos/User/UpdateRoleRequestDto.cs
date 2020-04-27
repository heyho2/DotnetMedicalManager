using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.User
{
    /// <summary>
    /// 修改角色 请求
    /// </summary>
    public class UpdateRoleRequestDto : BaseDto
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        public string RoleGuid { get; set; }
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
