using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 未评价
    /// </summary>
    public class NoConmentProductOrProjectListAsyncRequest
    {
        /// <summary>
        /// UserGuid
        /// </summary>
        [Display(Name = "用户Guid")]
        public string UserGuid { get; set; }
    }
}
