using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Message
{
    /// <summary>
    /// 获取消息记录列表请求Dto
    /// </summary>
    public class GetMessageListByFromAndToResponseDto:BasePageResponseDto<GetMessageListByFromAndToItemDto>
    {
        
    }

    /// <summary>
    /// 获取消息记录列表请求Dto
    /// </summary>
    public class GetMessageListByFromAndToItemDto : BaseDto
    {
        /// <summary>
        /// 发送者guid
        /// </summary>
        public string FromGuid { get; set; }

        /// <summary>
        /// 接收者guid
        /// </summary>
        public string ToGuid { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 消息日期
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
