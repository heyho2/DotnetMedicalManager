using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Merchant
{
    ///<summary>
    ///商户表模型
    ///</summary>
    [Table("t_merchant")]
    public class MerchantModel : BaseModel
    {
        ///<summary>
        ///商户GUID
        ///</summary>
        [Column("merchant_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///商户名
        ///</summary>
        [Column("merchant_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户名")]
        public string MerchantName { get; set; }

        ///<summary>
        ///门店账号
        ///</summary>
        [Column("account"), Required(ErrorMessage = "{0}必填"), Display(Name = "门店账号")]
        public string Account { get; set; }

        ///<summary>
        ///门店密码
        ///</summary>
        [Column("password"), Required(ErrorMessage = "{0}必填"), Display(Name = "门店密码")]
        public string Password { get; set; }

        /// <summary>
        /// 账号申请状态
        /// 'reject','approved','submit','draft'
        /// </summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "账号申请状态")]
        public string Status { get; set; } = StatusEnum.Draft.ToString();

        /// <summary>
        /// 纬度
        /// </summary>
        [Column("latitude"), Required(ErrorMessage = "{0}必填"), Display(Name = "纬度")]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [Column("longitude"), Required(ErrorMessage = "{0}必填"), Display(Name = "经度")]
        public decimal? Longitude { get; set; }

        ///<summary>
        ///签名附件guid
        ///</summary>
        [Column("signature_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "签名附件guid")]
        public string SignatureGuid { get; set; }

        /// <summary>
        /// 商家电话
        /// </summary>
        [Column("telephone")]
        public string Telephone { get; set; }

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        ///<summary>
        ///商户地址
        ///</summary>
        [Column("merchant_address"), Display(Name = "商户地址")]
        public string MerchantAddress { get; set; }

        ///<summary>
        ///商户图片
        ///</summary>
        [Column("merchant_picture"), Display(Name = "商户图片")]
        public string MerchantPicture { get; set; }
        ///<summary>
        ///医院GUID
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///市
        ///</summary>
        [Column("city"), Display(Name = "市")]
        public string City { get; set; }
        ///<summary>
        ///省
        ///</summary>
        [Column("province"), Display(Name = "省")]
        public string Province { get; set; }
        ///<summary>
        ///区
        ///</summary>
        [Column("area"), Display(Name = "区")]
        public string Area { get; set; }
        ///<summary>
        ///街道
        ///</summary>
        [Column("street"), Display(Name = "街道")]
        public string Street { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public enum StatusEnum
        {
            /// <summary>
            /// 驳回
            /// </summary>
            [Description("驳回")]
            Reject,

            /// <summary>
            /// 同意
            /// </summary>
            [Description("同意")]
            Approved,

            /// <summary>
            /// 提交
            /// </summary>
            [Description("提交")]
            Submit,

            /// <summary>
            /// 草稿
            /// </summary>
            [Description("草稿")]
            Draft
        }
    }
}