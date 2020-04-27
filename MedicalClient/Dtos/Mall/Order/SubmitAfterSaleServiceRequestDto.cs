using GD.Common.Base;
using GD.Dtos.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Order
{
    /// <summary>
    /// 提交售后服务单
    /// </summary>
    public class SubmitAfterSaleServiceRequestDto : BaseDto
    {
        /// <summary>
        /// 售后商品详细
        /// </summary>
        public List<AfterSaleServiceDetailRequestDto> Detials { get; set; }

        /// <summary>
        /// 售后类型 服务类订单只支持退款
        /// RefundWhitoutReturn-1:退款
        /// RefundWhithReturn-2:退款退货
        /// Exchange-3:换货
        /// </summary>
        public AfterSaleServiceTypeEnum? AfterSaleServiceType { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        [StringLength(500)]
        public string Reason { get; set; }

        /// <summary>
        /// 售后商品
        /// </summary>
        public class AfterSaleServiceDetailRequestDto
        {
            /// <summary>
            /// 订单详情guid
            /// </summary>
            public string OrderDetailGuid { get; set; }

            /// <summary>
            /// 售后数量
            /// </summary>
            public int? ProductCount { get; set; }
        }
    }

    /// <summary>
    /// 提交售后服务单上下文
    /// </summary>
    public class SubmitAfterSaleServiceContext
    {

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="requestDto"></param>
        public SubmitAfterSaleServiceContext(SubmitAfterSaleServiceRequestDto requestDto)
        {
            RequestDto = requestDto;
        }

        /// <summary>
        /// 请求dto
        /// </summary>
        public SubmitAfterSaleServiceRequestDto RequestDto { get; set; }

        /// <summary>
        /// 需要进行售后操作的订单明细集合
        /// </summary>
        public List<OrderDetailModel> OrderDetails { get; set; }

        /// <summary>
        /// 售后操作所属订单
        /// </summary>
        public OrderModel OrderModel { get; set; }

        /// <summary>
        /// 售后服务单model
        /// </summary>
        public AfterSaleServiceModel ServiceModel { get; set; }

        /// <summary>
        /// 售后服务单详情
        /// </summary>
        public List<AfterSaleDetailModel> ServiceDetailModels { get; set; }

        /// <summary>
        /// 售后协商记录
        /// </summary>
        public List<AfterSaleConsultationModel> ConsultationModels { get; set; }

        /// <summary>
        /// 服务类商品在申请退款时，需先将对应的商品卡设置为不可用
        /// </summary>
        public List<GoodsModel> GoodsModels { get; set; } = new List<GoodsModel>();

        /// <summary>
        /// 订单在申请退款时，需将对应的商品待评记录设置为不可用
        /// </summary>
        public List<OrderProductCommentModel> CommentModels { get; set; } = new List<OrderProductCommentModel>();
    }
}
