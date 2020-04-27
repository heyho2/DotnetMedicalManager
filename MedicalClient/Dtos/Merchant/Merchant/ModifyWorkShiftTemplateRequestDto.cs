using GD.Common.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 修改班次模板数据请求Dto
    /// </summary>
    public class ModifyWorkShiftTemplateRequestDto : BaseDto
    {
        /// <summary>
        /// 班次模板guid
        /// </summary>
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 班次方案名称
        /// </summary>
        [Required(ErrorMessage = "模板名称必填")]
        [MaxLength(50, ErrorMessage = "超过模板最大长度限制")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 班次数据
        /// </summary>
        public List<TheWorkShiftDto> WorkShifts { get; set; }
    }
}
