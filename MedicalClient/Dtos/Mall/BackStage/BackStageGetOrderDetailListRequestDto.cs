using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.BackStage
{
    /// <summary>
    /// 订单详情 请求Dto
    /// </summary>
    public class BackStageGetOrderDetailListRequestDto
    {
        /// <summary>
        /// 订单Guid 
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单Guid")]
        public string OrderGuid { get; set; }

        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class BackStageGetOrderDetailListResponseDto
    {
        /// <summary>
        /// 收货人
        /// </summary>
        public string OrderReceiver { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string OrderPhone { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string OrderAddress { get; set; }
        /// <summary>
        /// 订单备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 优惠活动Guid
        /// </summary>
        public string CampaignGuid { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal SpecialPrices { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 总金额(实付金额)
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 订单单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 物流公司
        /// </summary>
        public string LogisticsCompany { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNo { get; set; }
        /// <summary>
        /// 评论Guid
        /// </summary>
        public string CommentGuid { get; set; }
        /// <summary>
        /// 评价内容
        /// </summary>
        public string Content { get; set; }

    }

}
