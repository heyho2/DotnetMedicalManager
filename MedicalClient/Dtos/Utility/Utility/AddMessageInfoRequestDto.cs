using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;
using Newtonsoft.Json;

namespace GD.Dtos.Utility.Utility
{
    /// <inheritdoc />
    /// <summary>
    /// 聊天记录请求Dto
    /// </summary>
    public class AddMessageInfoRequestDto:BaseDto
    {
        /// <summary>
        /// 消息guid
        /// </summary>
        [JsonProperty("id")]
        public string MessageGuid { get; set; }

        /// <summary>
        /// 主题Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "主题Guid")]
        [JsonProperty("topic")]
        public string TopicGuid { get; set; }

        /// <summary>
        /// 发送者Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "发送者Guid")]
        [JsonProperty("from")]
        public string FromGuid { get; set; }
        /// <summary>
        /// 接收者Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "接收者Guid")]
        [JsonProperty("to")]
        public string ToGuid { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "内容")]
        [JsonProperty("content")]
        public string Context { get; set; }
        /// <summary>
        /// 聊天时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "聊天时间")]
        [JsonProperty("datetime")]
        public long CreationDate { get; set; }

        ///<summary>
        ///是否是html消息
        ///</summary>
        [JsonProperty("html")]
        public bool IsHtml
        {
            get;
            set;
        } = false;

    }
}
