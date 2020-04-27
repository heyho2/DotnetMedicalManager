using GD.Models.CommonEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Collection
{
    /// <summary>
    /// 收藏回参Model
    /// </summary>
    public class TargetCollectResponseDto
    {
        /// <summary>
        /// 是否收藏成功
        /// </summary>
        public bool result { get; set; }

        /// <summary>
        /// 收藏状态
        /// </summary>
        public CollectionStateEnum collectionState { get; set; }

        /// <summary>
        /// 是否是第一次收藏
        /// </summary>
        public bool first { get; set; }
    }
}
