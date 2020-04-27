using System.ComponentModel;

namespace GD.Dtos.CommonEnum
{
    /// <summary>
    /// 售后服务单状态枚举
    /// </summary>
    public enum AfterSaleServiceStatusEnum
    {
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        Applying = 1,
        /// <summary>
        /// 已拒绝
        /// </summary>
        [Description("已拒绝")]
        Reject,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed
    }

    /// <summary>
    /// 退款服务单类型
    /// </summary>
    public enum AfterSaleServiceTypeEnum
    {
        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        RefundWhitoutReturn = 1,
        /// <summary>
        /// 退款退货
        /// </summary>
        [Description("退款退货")]
        RefundWhithReturn,
        /// <summary>
        /// 换货
        /// </summary>
        [Description("换货")]
        Exchange
    }

    /// <summary>
    /// 协商历史角色
    /// </summary>
    public enum AfterSaleConsultationRoleEnum
    {
        /// <summary>
        /// 买家
        /// </summary>
        [Description("买家")]
        Buyer = 1,
        /// <summary>
        /// 卖家
        /// </summary>
        [Description("卖家")]
        Seller,
        /// <summary>
        /// 平台
        /// </summary>
        [Description("平台")]
        Platform
    }

    /// <summary>
    /// 售后退款记录状态
    /// </summary>
    public enum AfterSaleRefundStatusEnum
    {
        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        Refunding,
        /// <summary>
        /// 退款成功
        /// </summary>
        [Description("退款成功")]
        Success,
        /// <summary>
        /// 退款失败
        /// </summary>
        [Description("退款失败")]
        Failed

    }
}
