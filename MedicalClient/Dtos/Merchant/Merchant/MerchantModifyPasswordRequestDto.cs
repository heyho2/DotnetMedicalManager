using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class MerchantModifyPasswordRequestDto
    {
        /// <summary>
        /// 商户原密码
        /// </summary>
        [Required(ErrorMessage = "原密码必填")]
        public string Password { get; set; }

        /// <summary>
        /// 商户新密码
        /// </summary>
        [Required(ErrorMessage = "新密码必填")]
        public string NewPassword { get; set; }
    }
}
