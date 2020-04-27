using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Collection
{
    /// <summary>
    /// 获取收藏目标的用户列表请求Dto
    /// </summary>
    public class GetTheUserListOfCollectionTargetRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 目标guid
        /// </summary>
        public string TargetGuid { get; set; }

        /// <summary>
        /// 用户名称搜索条件
        /// </summary>
        public string Keyword { get; set; }
    }
}
