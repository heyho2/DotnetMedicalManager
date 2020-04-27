using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 改变信息
    /// </summary>
    public class ChangeInfomationSortRequestDto:BaseDto
    {
        /// <summary>
        /// 健康信息guid
        /// </summary>
        [Required(ErrorMessage = "健康信息guid必填")]
        public string InformationGuid { get; set; }

        /// <summary>
        /// 变化后的序号
        /// </summary>
        [Required(ErrorMessage = "健康信息guid必填")]
        public int Sort { get; set; }
    }
}
