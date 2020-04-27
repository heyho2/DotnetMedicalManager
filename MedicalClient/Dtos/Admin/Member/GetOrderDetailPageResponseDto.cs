using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Member
{
    /// <summary>
    /// 订单详细列表
    /// </summary>
    public class GetOrderDetailPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public string OrderGuid { get; set; }
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
    /// 订单详细列表
    /// </summary>
    public class GetOrderDetailPageResponseDto : BasePageResponseDto<GetOrderDetailPageItemDto>
    {
    }
    /// <summary>
    /// 订单详细列表
    /// </summary>
    public class GetOrderDetailPageItemDto : BaseDto
    {
        ///<summary>
        ///订单详情GUID（对应订单GUID,此仅主键）
        ///</summary>
        public string DetailGuid { get; set; }

        ///<summary>
        ///订单GUID
        ///</summary>
        public string OrderGuid { get; set; }

        ///<summary>
        ///产品GUID
        ///</summary>
        public string ProductGuid { get; set; }

        ///<summary>
        ///产品名称
        ///</summary>
        public string ProductName { get; set; }

        ///<summary>
        ///价格
        ///</summary>
        public decimal ProductPrice { get; set; }

        ///<summary>
        ///数量
        ///</summary>
        public int ProductCount { get; set; }
        ///<summary>
        ///优惠Guid
        ///</summary>
        public string CampaignGuid { get; set; }

        ///<summary>
        ///商品评价guid
        ///</summary>
        public string CommentGuid { get; set; }
    }
}
