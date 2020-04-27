using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Models.Utility
{
    ///<summary>
    ///会话主题表模型
    ///</summary>
    [Table("t_utility_topic")]
    public class TopicModel : BaseModel
    {
        ///<summary>
        ///主题GUID
        ///</summary>
        [Column("topic_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主题GUID")]
        public string TopicGuid
        {
            get;
            set;
        }

        ///<summary>
        ///发起者GUID
        ///</summary>
        [Column("sponsor_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "发起者GUID")]
        public string SponsorGuid
        {
            get;
            set;
        }

        ///<summary>
        ///接收者GUID
        ///</summary>
        [Column("receiver_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "接收者GUID")]
        public string ReceiverGuid
        {
            get;
            set;
        }

        ///<summary>
        ///话题关于GUID(如来自商品或直接问的医生）
        ///</summary>
        [Column("about_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "话题关于GUID(如来自商品或直接问的医生）")]
        public string AboutGuid
        {
            get;
            set;
        }

        ///<summary>
        ///aboutGuid对应枚举类型
        ///</summary>
        [Column("about_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "aboutGuid对应枚举类型")]
        public string AboutType { get; set; } = TopicAboutTypeEnum.Doctor.ToString();

        ///<summary>
        ///开始时间
        ///</summary>
        [Column("begin_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "开始时间")]
        public DateTime BeginDate
        {
            get;
            set;
        }

        ///<summary>
        ///结束时间
        ///</summary>
        [Column("end_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "结束时间")]
        public DateTime EndDate
        {
            get;
            set;
        }

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]

        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();
    }
}