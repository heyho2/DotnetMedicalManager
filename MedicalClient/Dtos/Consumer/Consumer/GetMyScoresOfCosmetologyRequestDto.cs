using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 双美-消费者积分分页列表
    /// </summary>
    public class GetMyScoresOfCosmetologyRequestDto:PageRequestDto
    {
        /// <summary>
        /// 积分类型：收入积分、支出积分、全部
        /// </summary>
        public string ScoreType { get; set; }
    }
}
