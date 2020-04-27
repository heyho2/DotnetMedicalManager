using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Utility.Utility
{
    /// <summary>
    /// 新增别名请求dto
    /// </summary>
    public class AddAliasRequestDto : BaseDto
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 别名针对的目标guid
        /// </summary>
        [Required(ErrorMessage = "别名针对的目标guid必填")]
        public string TargetGuid { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        [Required(ErrorMessage = "别名必填")]
        public string AliasName { get; set; }
    }
}
