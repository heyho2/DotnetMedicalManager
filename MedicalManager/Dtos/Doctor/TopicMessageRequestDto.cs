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
    public class TopicMessageRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医生guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "会话Guid")]
        public string TopicGuid { get; set; }
    }
    /// <summary>
    /// 获取消息对话列表
    /// </summary>
    public class TopicMessageResponseDto : BasePageResponseDto<TopicMessageItemDto>
    {
    }
    /// <summary>
    /// 获取消息对话列表
    /// </summary>
    public class TopicMessageItemDto : BaseDto
    {
        ///<summary>
        ///消息GUID
        ///</summary>
        public string MsgGuid { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string TopicGuid { get; set; }

        /// <summary>
        /// 发送者guid
        /// </summary>
        public string FromGuid { get; set; }

        ///<summary>
        ///发送者
        ///</summary>
        public string FromName { get; set; }

        /// <summary>
        /// 接收者guid
        /// </summary>
        public string ToGuid { get; set; }

        ///<summary>
        ///接收者
        ///</summary>
        public string ToName { get; set; }

        ///<summary>
        ///消息内容
        ///</summary>
        public string Context { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
