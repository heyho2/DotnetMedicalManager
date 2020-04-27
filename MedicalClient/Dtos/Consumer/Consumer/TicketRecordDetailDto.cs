using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 卡券明细Dto
    /// </summary>
    public class TicketRecordDetailDto : BaseDto
    {
        ///<summary>
        ///个人卡券明细GUID
        ///</summary>
        public string TicketRecordDetailGuid { get; set; }

        ///<summary>
        ///个人卡券GUID
        ///</summary>
        public string TicketRecordGuid { get; set; }

        ///<summary>
        ///项目GUID
        ///</summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目名称
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

        ///<summary>
        ///项目次数
        ///</summary>
        public int Count { get; set; }

        /// <summary>
        /// 可适用门店
        /// </summary>
        public string MerchantNames { get;set;}
    }
}
