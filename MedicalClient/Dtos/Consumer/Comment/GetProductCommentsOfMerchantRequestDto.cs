using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Comment
{
    /// <summary>
    /// 获取商铺下商品的所有评论
    /// </summary>
    public class GetProductCommentsOfMerchantRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商户guid
        /// </summary>
        [Required(ErrorMessage = "商铺guid必填")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 评论起始日期
        /// </summary>
        [Required(ErrorMessage = "评论起始日期必填")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 评论结束日期
        /// </summary>
        [Required(ErrorMessage = "评论结束日期必填")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 回复状态
        /// </summary>
        public ReceiveStatusEnum ReceiveStatus { get; set; } = ReceiveStatusEnum.All;
    }
    /// <summary>
    /// 回复状态
    /// </summary>
    public enum ReceiveStatusEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All,

        /// <summary>
        /// 已回复
        /// </summary>
        [Description("已回复")]
        Replied,

        /// <summary>
        /// 未回复
        /// </summary>
        [Description("未回复")]
        NotReplied,
    }
}
