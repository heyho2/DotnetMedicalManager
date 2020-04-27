using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 分页获取从下级分销消费得到的积分提成列表响应Dto
    /// </summary>
    public class GetScoresOfDistributionResponseDto : BaseDto
    {
        /// <summary>
        /// 消费来源记录Guid
        /// </summary>
        public string ScoreRecordGuid { get; set; }

        /// <summary>
		/// 创建时间
		/// </summary>
        public DateTime CreationDate { get; set; }

        ///<summary>
        ///消费金额
        ///</summary>
        public decimal Ammount { get; set; }

        ///<summary>
        ///积分比例(例如消费积分比例/提成积分比例)
        ///</summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 可得积分
        /// </summary>
        public int Score { get; set; }
    }
}
