using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.ReviewRecord
{
    /// <summary>
    /// 审核记录
    /// </summary>
    /// 
    public class GetReviewRecordPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 归属ig
        /// </summary>
        public string OwnerGuid { get; set; }
        /// <summary>
        /// 类型（Merchant,Doctors）
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }
    }
    /// <summary>
    /// 审核记录
    /// </summary>
    public class GetReviewRecordPageResponseDto : BasePageResponseDto<GetReviewRecordPageItemDto>
    {

    }
    /// <summary>
    /// 审核记录
    /// </summary>
    public class GetReviewRecordPageItemDto : BaseDto
    {

        ///<summary>
        ///GUID
        ///</summary>
        public string ReviewGuid { get; set; }

        ///<summary>
        ///推荐归属GUID
        ///</summary>
        public string OwnerGuid { get; set; }

        ///<summary>
        ///类型
        ///</summary>
        public string Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        ///<summary>
        ///拒绝原因
        ///</summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// 创建时间，默认为系统当前时间   
        /// </summary>
        public DateTime CreationDate { get; set; }

    }
}
