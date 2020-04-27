using GD.Common.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 新增班次方案请求Dto
    /// </summary>
    public class AddWorkShiftTemplateRequestDto : BaseDto
    {
        /// <summary>
        /// 班次方案名称
        /// </summary>
        [Required(ErrorMessage = "模板名称必填")]
        [MaxLength(50, ErrorMessage = "超过模板最大长度限制")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 班次数据
        /// </summary>
        public List<TheWorkShiftDto> WorkShifts { get; set; }
    }

    /// <summary>
    /// 商户班次数据请求Dto
    /// </summary>
    public class TheWorkShiftDto : BaseDto
    {
        /// <summary>
        /// 班次名称
        /// </summary>
        [Required(ErrorMessage = "班次名称必填")]
        [MaxLength(20, ErrorMessage = "超过班次名称最大长度限制")]
        public string WorkShiftName { get; set; }

        /// <summary>
        /// 班次明细（班次时间段）
        /// </summary>
        public List<TimeDto> WorkShiftDetailTimes { get; set; }
    }
}
