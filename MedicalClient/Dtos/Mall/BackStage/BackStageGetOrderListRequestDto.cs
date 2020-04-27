using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Dtos.Mall.BackStage
{
    /// <inheritdoc />
    /// <summary>
    ///  订单列表 请求Dto
    /// </summary>
    public class BackStageGetOrderListRequestDto : BasePageRequestDto
    {

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OderNo { get; set; }
        /// <summary>
        /// 会员名
        /// </summary>
        public string UserName { get; set; } 
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; } 
        /// <summary>
        /// 所参加的活动
        /// </summary>
        public string CampaignGuid { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string OrderReceiver { get; set; }
        ///// <summary>
        ///// 收货地址
        ///// </summary>
        //public string CampaignName { get; set; }

        /// <summary>
        /// 订单状态（默认待付款）
        /// </summary>
        public OrderStatusEnum OrderStatus { get; set; }

        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }

    /// <inheritdoc />
    /// <summary>
    ///  订单列表 请求Dto
    /// </summary>
    public class BackStageGetOrderListResponseDto : BasePageResponseDto<BackStageGetOrderListItem>
    {


    }

    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class BackStageGetOrderListItem : BaseDto
    {
        /// <summary>
        /// 订单Guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 用户Guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public string PaidAmount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string CreationDate { get; set; }
    }
}
