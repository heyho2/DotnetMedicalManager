using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Recommend
{
    /// <summary>
    /// 删除推荐
    /// </summary>
    public class DeleteRecommendRequestDto : BaseDto
    {
        /// <summary>
        /// 推荐guid
        /// </summary>
        public string RecommendGuid { get; set; }
    }
}
