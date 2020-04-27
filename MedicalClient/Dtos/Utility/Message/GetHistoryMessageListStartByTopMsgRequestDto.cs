using GD.Common.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Utility.Message
{
    /// <summary>
    /// 通过顶部消息Id获取历史消息记录请求参数
    /// </summary>
    public class GetHistoryMessageListStartByTopMsgRequestDto : BaseDto
    {
        /// <summary>
        /// 用户一guid
        /// </summary>
        [Required(ErrorMessage = "用户一guid必填")]
        public string UserId1 { get; set; }

        /// <summary>
        /// 用户二guid
        /// </summary>
        [Required(ErrorMessage = "用户二guid必填")]
        public string UserId2 { get; set; }

        /// <summary>
        /// 查询的单页条数
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 顶部消息Id
        /// </summary>
        public string TopMsgId { get; set; }

        /// <summary>
        /// 中间参数，无需填写
        /// </summary>
        public DateTime? TopMsgDate { get; set; }
    }

    /// <summary>
    /// 通过顶部消息Id获取历史消息记录响应Dto
    /// </summary>
    public class GetHistoryMessageListStartByTopMsgResponseDto : BaseDto
    {
        /// <summary>
        /// 消息id
        /// </summary>
        [JsonProperty("id")]
        public string msg_guid { get; set; }

        /// <summary>
        /// 发送者guid
        /// </summary>
        [JsonProperty("from")]
        public string FromGuid { get; set; }

        /// <summary>
        /// 接收者guid
        /// </summary>
        [JsonProperty("to")]
        public string ToGuid { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonProperty("content")]
        public string Context { get; set; }

        /// <summary>
        /// 消息日期
        /// </summary>
        [JsonProperty("datetime")]
        public DateTime CreationDate { get; set; }

        ///<summary>
        ///是否是html消息
        ///</summary>
        [JsonProperty("html")]
        public bool IsHtml { get; set; }
    }
}
