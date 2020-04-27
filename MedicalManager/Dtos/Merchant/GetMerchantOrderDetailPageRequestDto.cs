using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant
{
    /// <summary>
    /// 审核商家列表
    /// </summary>
    public class GetMerchantOrderDetailPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 注册时间 至
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }
        /// <summary>
        /// 商户id
        /// </summary>
        public string MerchantGuid { get; set; }
    }
    /// <summary>
    /// 审核商家列表
    /// </summary>
    public class GetMerchantOrderDetailPageResponseDto : BasePageResponseDto<GetMerchantOrderDetailPageItemDto>
    {


    }
    /// <summary>
    /// 审核商家列表
    /// </summary>
    public class GetMerchantOrderDetailPageItemDto : BaseDto
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
        ///商户guid
        ///</summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

    }
}
