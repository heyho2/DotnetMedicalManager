using GD.Common.Base;
using System;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取消费者从未使用过的项目列表响应Dto
    /// </summary>
    public class GetMyUnusedGoodsItemListResponseDto : BaseDto
    {
        /// <summary>
        /// 卡项guid
        /// </summary>
        public string GoodsItemGuid { get; set; }

        /// <summary>
        /// 项目guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目时长
        /// </summary>
        public int OperationTime { get; set; }

        /// <summary>
        /// 服务项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 卡项剩余次数
        /// </summary>
        public int Remain { get; set; }

        /// <summary>
        /// 卡有效期（起始）
        /// </summary>
        public DateTime? EffectiveStartDate { get; set; }

        /// <summary>
        /// 卡有效期（截止）
        /// </summary>
        public DateTime? EffectiveEndDate { get; set; }

    }
}
