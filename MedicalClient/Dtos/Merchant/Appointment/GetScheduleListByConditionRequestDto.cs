using GD.Common.Base;
using GD.Dtos.Consumer.Consumer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 按条件查询排班列表 -请求Dto
    /// </summary>
    public class GetScheduleListByConditionRequestDto:PageRequestDto
    {
        /// <summary>
        /// 商户的UserID
        /// </summary>
        [Display(Name = "商户的UserID")]
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 客户手机号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "客户手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 项目Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "项目Guid")]
        public string ProjectGuid { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        public DateTime? Date { get; set; }
        /// <summary>
        /// 美疗师Guid
        /// </summary>
        [Display(Name = "美疗师Guid")]
        public string TherapistGuid { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class TotalNumDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int TotalNum { get; set; }
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetScheduleListByConditionResponseDto:BaseDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// 美疗师Guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 美疗师名
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 是否已购买
        /// </summary>
        public string GoodsItemGuid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public  class GetScheduleListByConditionPageResponseDto : BaseDto
    {
        /// <summary>
        /// 当前页数据
        /// </summary>
        public IEnumerable<GetScheduleListByConditionResponseDto> CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 子项
    /// </summary>
    public class GetScheduleListByConditionItemDto:BaseDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// 美疗师Guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 美疗师名
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 是否已购买
        /// </summary>
        public string IsBuy { get; set; }
        
    }
}
