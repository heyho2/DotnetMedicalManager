namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 
    /// </summary>
    public class MerchantLoginResponseDto
    {
        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商户姓名
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 美疗师g商户Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// XMPP服务器
        /// </summary>
        public string Xmpp { get; set; }

        /// <summary>
        /// XMPP服务器域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 门店图片
        /// </summary>
        public string MerchantPicture { get; set; }
    }
}
