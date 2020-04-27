using GD.Common.Base;

namespace GD.Dtos.Merchant
{
    /// <summary>
    /// 获取商户信息（用于编辑）
    /// </summary>
    public class GetMerchantInfoResponseDto : BaseDto
    {

        /// <summary>
        /// 商户id
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商户电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 签名附件guid
        /// </summary>
        public string SignatureGuid { get; set; }
        /// <summary>
        /// 商户图片
        /// </summary>
        public string MerchantPicture { get; set; }
        /// <summary>
        /// 商户图片
        /// </summary>
        public string MerchantPictureUrl { get; set; }

        /// <summary>
        /// 商户地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        ///<summary>
        ///市
        ///</summary>
        public string City { get; set; }
        ///<summary>
        ///省
        ///</summary>
        public string Province { get; set; }
        ///<summary>
        ///区
        ///</summary>
        public string Area { get; set; }
        ///<summary>
        ///街道
        ///</summary>
        public string Street { get; set; }
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }
    }
    /// <summary>
    /// 获取商户信息（用于编辑）
    /// </summary>
    public class GetMerchantInfoRequestDto : BaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string MerchantGuid { get; set; }
    }
}
