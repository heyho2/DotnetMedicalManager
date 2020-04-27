using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Merchant
{
    ///<summary>
    /// 服务人员Model
    ///</summary>
    [Table("t_merchant_therapist")]
    public class TherapistModel : BaseModel
    {
        ///<summary>
        ///服务人员GUID
        ///</summary>
        [Column("therapist_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "美疗师GUID")]
        public string TherapistGuid { get; set; }

        ///<summary>
        ///服务人员姓名
        ///</summary>
        [Column("therapist_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务人员姓名")]
        public string TherapistName { get; set; }

        ///<summary>
        ///所属店铺guid
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属店铺guid")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///职称
        ///</summary>
        [Column("job_title"), Required(ErrorMessage = "{0}必填"), Display(Name = "职称")]
        public string JobTitle { get; set; }

        ///<summary>
        ///手机账号
        ///</summary>
        [Column("therapist_phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "手机账号")]
        public string TherapistPhone { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [Column("therapist_password"), Required(ErrorMessage = "{0}必填"), Display(Name = "密码")]
        public string TherapistPassword { get; set; }

        /// <summary>
        /// 微信公众号openid
        /// </summary>
        [Column("wechat_openid"), Display(Name = "微信公众号openid")]
        public string WeChatOpenId { get; set; }

        ///<summary>
        ///医生一寸照附件Guid
        ///</summary>
        [Column("portrait_guid"), Display(Name = "医生一寸照附件Guid")]
        public string PortraitGuid { get; set; }


        ///<summary>
        ///擅长的标签
        ///</summary>
        [Column("tag"), Required(ErrorMessage = "{0}必填"), Display(Name = "擅长的标签")]
        public string Tag { get; set; }

        ///<summary>
        ///个人介绍
        ///</summary>
        [Column("introduction"), Required(ErrorMessage = "{0}必填"), Display(Name = "个人介绍")]
        public string Introduction { get; set; }

        ///<summary>
        ///是否推荐
        ///</summary>
        [Column("recommend"), Display(Name = "是否明星医生")]
        public string Recommend { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Display(Name = "排序")]
        public int Sort { get; set; }

    }
}



