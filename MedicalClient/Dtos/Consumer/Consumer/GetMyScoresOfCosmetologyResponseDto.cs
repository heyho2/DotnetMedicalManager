using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取双美消费者积分响应Dto
    /// </summary>
    public class GetMyScoresOfCosmetologyResponseDto : BaseDto
    {
        ///<summary>
        ///积分项GUID
        ///</summary>
        public string ScoreGuid
        {
            get;
            set;
        }

        ///<summary>
        ///积分变化，+/-
        ///</summary>
        public int Variation
        {
            get;
            set;
        }

        ///<summary>
        ///积分变化原因
        ///</summary>
        public string Reason
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
