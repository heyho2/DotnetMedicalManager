using GD.Common.Base;

namespace GD.Dtos.Mall.WeiXinPay
{
    /// <summary>
    /// 微信Token请求Dto
    /// </summary>
    public class WeiXinTokenRequestDto : BaseDto
    {
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
    }
}
