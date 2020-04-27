using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 
    /// </summary>
    public class GetUseableOrderListOfCosmetologyRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 不传值，表示获取所有；
        /// </summary>
        public GoodsStateEnum? GoodsState { get; set; }

        /// <summary>
        /// 卡状态枚举
        /// </summary>
        public enum GoodsStateEnum
        {
            /// <summary>
            /// 可用的
            /// </summary>
            [Description("可使用的")]
            Usable = 1
        }
    }

    /// <summary>
    /// 双美-订单列表（可使用的）响应Dto
    /// </summary>
    public class GetUseableOrderListOfCosmetologyResponseDto : BasePageResponseDto<GetUseableOrderListOfCosmetologyItemDto>
    {
        
    }

    /// <summary>
    /// 双美-订单列表（可使用的）响应Dto
    /// </summary>
    public class GetUseableOrderListOfCosmetologyItemDto : BaseDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        ///订单类型（服务类，实体类）
        /// </summary>
        public string OrderCategory { get; set; }
        /// <summary>
        /// 个人商品Guid
        /// </summary>
        public string GoodsGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 有效期开始日期
        /// </summary>
        public DateTime? EffectiveStartDate { get; set; }

        /// <summary>
        /// 有效期结束日期
        /// </summary>
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 个人商品是否可用
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// 商品可用项目阈值
        /// </summary>
        public int? ProjectThreshold { get; set; }

        /// <summary>
        /// 是否已过期
        /// </summary>
        public bool IsEffective { get; set; } = false;

        /// <summary>
        /// 项目明细
        /// </summary>
        public List<GoodsItemResponseDto> GoodsItems { get; set; }
    }

    /// <summary>
    /// 双美-卡项响应Dto
    /// </summary>
    public class GoodsItemResponseDto : BaseDto
    {
        /// <summary>
        /// 个人商品项目guid
        /// </summary>
        public string GoodsItemGuid { get; set; }

        /// <summary>
        /// 商品项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 商品项目guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目预计耗时
        /// </summary>
        public int OperationTime { get; set; }

        /// <summary>
        /// 项目总次数
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// 项目剩余可用次数
        /// </summary>
        public int ItemRemain { get; set; }

        /// <summary>
        /// 项目是否可用
        /// </summary>
        public bool ItemAvailable { get; set; }
    }


    /// <summary>
    /// 双美-卡项明细
    /// </summary>
    public class GoodsItemDetailDto : BaseDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        ///订单类型（服务类，实体类）
        /// </summary>
        public string OrderCategory { get; set; }

        /// <summary>
        /// 个人商品Guid
        /// </summary>
        public string GoodsGuid { get; set; }

        /// <summary>
        /// 个人商品是否可用
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// 商品可用项目阈值
        /// </summary>
        public int? ProjectThreshold { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 有效期开始日期
        /// </summary>
        public DateTime? EffectiveStartDate { get; set; }

        /// <summary>
        /// 有效期结束日期
        /// </summary>
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary>
        /// 个人商品项目guid
        /// </summary>
        public string GoodsItemGuid { get; set; }

        /// <summary>
        /// 商品项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 商品项目guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目预计耗时
        /// </summary>
        public int OperationTime { get; set; }

        /// <summary>
        /// 项目总次数
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// 项目剩余可用次数
        /// </summary>
        public int ItemRemain { get; set; }

        /// <summary>
        /// 项目是否可用
        /// </summary>
        public bool ItemAvailable { get; set; }

        /// <summary>
        /// 卡是否过期（true:已过期，false:没过期）
        /// </summary>
        public bool IsEffective { get; set; }
    }

    public class GetUseableOrderListCount
    {
        public int Count { get; set; }
    }
}
