using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 商户登录请求
    /// </summary>
    public class MerchantLoginRequestDto
    {
        /// <summary>
        /// 商户账号
        /// </summary>
        [Required(ErrorMessage = "账号必填")]
        public string Account { get; set; }

        /// <summary>
        /// 商户密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        public string Password { get; set; }
    }
}
