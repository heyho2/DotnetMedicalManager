using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    ///<summary>
    ///意见表模型
    ///</summary>
    [Table("t_consumer_advise")]
    public class AdviseModel : BaseModel
    {
        ///<summary>
        ///建议GUID
        ///</summary>
        [Column("advise_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "建议GUID")]
        public string AdviseGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid
        {
            get;
            set;
        }

        ///<summary>
        ///建议人姓名
        ///</summary>
        [Column("adviser"), Required(ErrorMessage = "{0}必填"), Display(Name = "建议人姓名")]
        public string Adviser
        {
            get;
            set;
        }

        ///<summary>
        ///建议人手机号
        ///</summary>
        [Column("adviser_phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "建议人手机号")]
        public string AdviserPhone
        {
            get;
            set;
        }

        ///<summary>
        ///建议人EMAIL
        ///</summary>
        [Column("adviser_email"), Required(ErrorMessage = "{0}必填"), Display(Name = "建议人EMAIL"), EmailAddress]
        public string AdviserEmail
        {
            get;
            set;
        }

        ///<summary>
        ///建议内容
        ///</summary>
        [Column("advise_content"), Required(ErrorMessage = "{0}必填"), Display(Name = "建议内容")]
        public string AdviseContent
        {
            get;
            set;
        }

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType
        {
            get;
            set;
        } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();
    }
}