using GD.Common.Base;
using GD.Models.CommonEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Message
{
    /// <summary>
    /// 通过接收者用户guid获取发送者用户列表请求Dto
    /// </summary>
    public class GetFromUserListByToUserGuidRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 接收者用户guid
        /// </summary>
        public string toUserGuid { get; set; }

        /// <summary>
        /// 主题相关类型
        /// </summary>
        public TopicAboutTypeEnum TopicAbountType { get; set; }
    }
}
