using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 返回列表 （暂时不分页）
    /// </summary>
    public class GetReportListRequest : BasePageRequestDto
    {
        /// <summary>
        /// SQL语句
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "SQL语句")]
        public string SqlStr { get; set; }
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetReportListResponse : BasePageResponseDto<GetPhoneListItemDto>
    {

    }

    /// <summary>
    /// 子项
    /// </summary>
    public class GetPhoneListItemDto : BaseDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "序号")]
        public string No { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        public string Phone { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [Display(Name = "年龄")]
        public string Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public string Sex { get; set; }
    }

   
}
