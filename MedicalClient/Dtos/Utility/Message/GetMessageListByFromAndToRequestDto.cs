using GD.Common.Base;
using GD.Models.CommonEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Message
{
    /// <summary>
    /// 获取消息记录列表请求Dto
    /// </summary>
    public class GetMessageListByFromAndToRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 用户一guid
        /// </summary>
        public string UserOneId { get; set; }

        /// <summary>
        /// 用户二guid
        /// </summary>
        public string UserTwoId { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 主题相关类型
        /// </summary>
        public TopicAboutTypeEnum TopicAbountType { get; set; }
    }
}
