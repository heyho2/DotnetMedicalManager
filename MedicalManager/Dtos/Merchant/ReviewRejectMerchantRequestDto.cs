using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant
{
    /// <summary>
    /// 审核驳回
    /// </summary>
    public class ReviewRejectMerchantRequestDto : BaseDto
    {
        /// <summary>
        /// 归属GUID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "归属GUID")]
        public string OwnerGuid { get; set; }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "拒绝原因")]
        public string RejectReason { get; set; }
    }
}
