using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 双美--转赠项目 响应Dto
    /// </summary>
    public class PresentGoodsItemOfCosmetologyResponseDto : BaseDto
    {
        /// <summary>
        /// 礼物Guid
        /// </summary>
        public string GiftGuid { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目时长
        /// </summary>
        public int OperationTime { get; set; }

        ///<summary>
        ///有效起始日期
        ///</summary>
        public DateTime? EffectiveStartDate { get; set; }

        ///<summary>
        ///有效终止日期
        ///</summary>
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary>
        /// 适用门店名称
        /// </summary>
        public List<string> ApplicableStores { get; set; }
    }
}
