namespace GD.Dtos.Account
{
    /// <summary>
    /// 生成手机验证码Dto
    /// </summary>
    public class CreateVerificationCodeResponseDto
    {
        /// <summary>
        /// 验证码有效期分钟数
        /// </summary>
        public int Minutes { get; set; } = 1;

        /// <summary>
        /// 验证码，生产环境返回空
        /// </summary>
        public int? Code { get; set; }
    }
}
