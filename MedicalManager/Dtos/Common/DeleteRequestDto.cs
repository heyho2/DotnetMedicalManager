using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 删除
    /// </summary>
    public class DeleteRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        [Required]
        public string Guid { get; set; }
    }
}
