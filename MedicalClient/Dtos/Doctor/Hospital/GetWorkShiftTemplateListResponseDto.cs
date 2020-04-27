using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取医院班次模板列表响应
    /// </summary>
    public class GetWorkShiftTemplateListResponseDto : BaseDto
    {
        /// <summary>
        /// 模板guid
        /// </summary>
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }
    }
}
