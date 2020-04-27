using GD.Common.Base;

namespace GD.Dtos.Account
{
    /// <summary>
    /// 登录结果DTO
    /// </summary>
    public class LoginResponseDto : BaseDto
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// XMPP服务器
        /// </summary>
        public string Xmpp { get; set; }

        /// <summary>
        /// XMPP服务器域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// RabbitMQ JS 连接 URL
        /// </summary>
        public string RabbitMQ { get; set; }
    }
}
