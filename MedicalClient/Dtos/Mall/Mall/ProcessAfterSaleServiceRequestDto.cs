using GD.Common.Base;
using GD.Dtos.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Mall;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 处理服务单请求
    /// </summary>
    public class ProcessAfterSaleServiceRequestDto : BaseDto
    {
        /// <summary>
        /// 服务单Guid
        /// </summary>
        [Required(ErrorMessage = "服务单标识需提供")]
        public string ServiceGuid { get; set; }
        /// <summary>
        /// 售后类型
        /// </summary>
        public AfterSaleServiceTypeEnum AfterSaleType { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { get; set; }
        /// <summary>
        /// 未通过原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            if (!IsPass)
            {
                return !string.IsNullOrEmpty(Reason);
            }
            return true;
        }
    }

    /// <summary>
    /// 处理退款上下文
    /// </summary>
    public class ProcessAfterSaleServiceContext
    {
        /// <summary>
        /// .
        /// </summary>
        /// <param name="request"></param>
        public ProcessAfterSaleServiceContext(ProcessAfterSaleServiceRequestDto request)
        {
            Request = request;
        }
        /// <summary>
        /// 相同订单已退款成功服务单
        /// </summary>
        public int SameOrderServiceCompletedCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ProcessAfterSaleServiceRequestDto Request { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public OrderModel Order { get; set; }
        /// <summary>
        /// 订单详情列表
        /// </summary>
        public List<OrderDetailModel> OrderDetails { get; set; }
        /// <summary>
        /// 商品卡项
        /// </summary>
        public List<GoodsModel> Goods { get; set; }
        /// <summary>
        /// 评论列表
        /// </summary>
        public List<OrderProductCommentModel> Comments { get; set; }
        /// <summary>
        /// 交易流水
        /// </summary>
        public TransactionFlowingModel TransactionFlowing { get; set; }
        /// <summary>
        /// 服务单
        /// </summary>
        public AfterSaleServiceModel AfterSaleService { get; set; }
        /// <summary>
        /// 服务单详情
        /// </summary>
        public List<AfterSaleDetailModel> AfterSaleDetails { get; set; }
        /// <summary>
        /// 协商历史
        /// </summary>
        public List<AfterSaleConsultationModel> AfterSaleConsultations { get; set; } = new List<AfterSaleConsultationModel>();
        /// <summary>
        /// 退款流水
        /// </summary>
        public AfterSaleRefundModel AfterSaleRefund { get; set; }
    }
}
