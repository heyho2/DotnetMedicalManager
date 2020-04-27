using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 新增商户班次数据请求Dto
    /// </summary>
    public class AddWorkShiftRequestDto : BaseDto
    {
        /// <summary>
        /// 班次名称
        /// </summary>
        [Required(ErrorMessage = "班次名称必填")]
        public string WorkShiftName { get; set; }

        /// <summary>
        /// 班次模板guid
        /// </summary>
        [Required(ErrorMessage = "班次模板guid必填")]
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 班次明细
        /// </summary>
        public List<WorkShiftDetailDto> WorkShiftDetails { get; set; }

    }

    /// <summary>
    /// 班次明细
    /// </summary>
    public class WorkShiftDetailDto : TimeDto
    {
    }
}
