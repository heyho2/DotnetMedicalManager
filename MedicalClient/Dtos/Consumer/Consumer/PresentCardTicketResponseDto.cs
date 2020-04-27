using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 双美--转赠卡券响应Dto
    /// </summary>
    public class PresentCardTicketResponseDto : BaseDto
    {
        /// <summary>
        /// 卡券Guid
        /// </summary>
        public string TicketRecordGuid { get; set; }

        ///<summary>
        ///有效起始日期
        ///</summary>
        public DateTime? EffectiveStartDate { get; set; }

        ///<summary>
        ///有效终止日期
        ///</summary>
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary>
        /// 转赠卡券明细
        /// </summary>
        public List<PresentCardTicketDetailDto> PresentCardTicketDetails { get; set; }

    }
    /// <summary>
    /// 双美--转赠卡券明细Dto
    /// </summary>
    public class PresentCardTicketDetailDto : BaseDto
    {
        /// <summary>
        /// 商品项目名称
        /// </summary>
        public string ProjectName { get; set; }


        /// <summary>
        /// 项目时长
        /// </summary>
        public int OperationTime { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 剩余张数
        /// </summary>
        public int Remain { get; set; }

        /// <summary>
        /// 适用门店名称
        /// </summary>
        public List<string> ApplicableStores { get; set; }

    }
}
