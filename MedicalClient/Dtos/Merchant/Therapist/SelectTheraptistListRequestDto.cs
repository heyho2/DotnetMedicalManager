using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 查询服务人员列表 -请求Dto
    /// </summary>
    public class SelectTheraptistListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 店铺Guid
        /// </summary>
        [Display(Name = "店铺Guid")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 服务人员姓名/手机号
        /// </summary>
        [Display(Name = "服务人员姓名/手机号")]
        public string SelectStr { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Display(Name = "是否可用")]
        public bool Enable { get; set; } = true;
    }
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class SelectTheraptistListResponseDto : BasePageResponseDto<SelectTheraptistItemDto>
    {

    }
    /// <summary>
    /// 子项
    /// </summary>
    public class SelectTheraptistItemDto : BaseDto
    {
        /// <summary>
        /// Guid
        /// </summary>
        [Display(Name = "服务人员Guid")]
        public string TherapistGuid { get; set; }
        /// <summary>
        /// 服务人员姓名
        /// </summary>
        [Display(Name = "服务人员姓名")]
        public string TherapistName { get; set; }
        /// <summary>
        /// 服务人员电话
        /// </summary>
        [Display(Name = "服务人员电话")]
        public string TherapistPhone { get; set; }
    }
}
