using GD.Common.Base;
using GD.Dtos.CommonEnum;
using System;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAfterSaleServiceListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商户Guid（不用传）
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 服务单号
        /// </summary>
        public string ServiceNo { get; set; }
        /// <summary>
        /// 退款状态
        /// </summary>
        public AfterSaleServiceStatusEnum? AfterSaleStatus { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 申请开始时间
        /// </summary>
        public DateTime? ApplicationBeginTime { get; set; }
        /// <summary>
        /// 申请结束时间
        /// </summary>
        public DateTime? ApplicationEndTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAfterSaleServiceListResponseDto : BasePageResponseDto<AfterSaleServiceItem>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class AfterSaleServiceItem : BaseDto
    {
        /// <summary>
        /// 服务单Id
        /// </summary>
        public string ServiceGuid { get; set; }
        /// <summary>
        /// 服务单号
        /// </summary>
        public string ServiceNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 售后类型（1：退款不退货、2：退款退货:3：换货）
        /// </summary>
        public AfterSaleServiceTypeEnum Type { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplicationTime { get; set; }
        /// <summary>
        /// 退款状态（1：申请中、2：已拒绝、3：已完成）
        /// </summary>
        public AfterSaleServiceStatusEnum AfterSaleStatus { get; set; }
    }
}
