using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Utility.Account
{
    /// <summary>
    /// 验证验证码Dto
    /// </summary>
    public class VerifyCodeRequestDto
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "电话号码"), Phone(ErrorMessage = "请输入正确的电话号码")]
        public string Phone
        {
            get;
            set;
        }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "验证码"), Range(111111, 999999, ErrorMessage = "{0}无效")]
        public int Code
        {
            get;
            set;
        }
    }
}
