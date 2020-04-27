using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor
{
    /// <summary>
    /// 获取消息对话列表
    /// </summary>
    public class GetDoctorTopicRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医生guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医生Guid")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 注册时间 至
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
    /// <summary>
    /// 获取消息对话列表
    /// </summary>
    public class GetDoctorTopicResponseDto : BasePageResponseDto<GetDoctorTopicItemDto>
    {
    }
    /// <summary>
    /// 获取消息对话列表
    /// </summary>
    public class GetDoctorTopicItemDto : BaseDto
    {
        ///<summary>
        ///主题GUID
        ///</summary>
        public string TopicGuid { get; set; }

        ///<summary>
        ///发起者
        ///</summary>
        public string SponsorGuid { get; set; }
        ///<summary>
        ///发起者
        ///</summary>
        public string SponsorName { get; set; }

        ///<summary>
        ///接收者
        ///</summary>
        public string ReceiverName { get; set; }
        ///<summary>
        ///接收者
        ///</summary>
        public string ReceiverGuid { get; set; }


        ///<summary>
        ///话题关于GUID(如来自商品或直接问的医生）
        ///</summary>
        public string AboutGuid { get; set; }

        ///<summary>
        ///aboutGuid对应枚举类型
        ///</summary>
        public string AboutType { get; set; }

        ///<summary>
        ///开始时间
        ///</summary>
        public DateTime BeginDate { get; set; }

        ///<summary>
        ///结束时间
        ///</summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 咨询者
        /// </summary>
        public string Consultants { get; set; }
        /// <summary>
        /// 最后一条消息
        /// </summary>
        public string LastMessage { get; set; }
    }
}
