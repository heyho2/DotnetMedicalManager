using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Score
{
    /// <summary>
    ///医生积分列表 请求
    /// </summary>
    public class ExportIntegralInfoRequestDto : BaseDto
    {
        /// <summary>
        /// user id
        /// </summary>
        [Required]
        public string UserGuid { get; set; }

    }
    /// <summary>
    ///医生积分列表 响应
    /// </summary>
    public class ExportIntegralInfoResponseDto 
    {
        /// <summary>
        /// 结果集
        /// </summary>
        public IEnumerable<GetIntegralInfoPageItemDto> Items { get; set; }
    }
}
