using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Comment
{
    /// <summary>
    /// 从订单明细评价商品 请求Dto
    /// </summary>
    public class CommentProductFromOrderDetailRequestDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        [Required(ErrorMessage ="商品guid必填")]
        public string ProductGuid { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Required(ErrorMessage = "评论内容必填")]
        public string Content { get; set; }

        /// <summary>
        /// 评论分数
        /// </summary>
        [Required(ErrorMessage = "评论分数必填")]
        public int Score { get; set; } = 0;

        /// <summary>
        /// 是否匿名
        /// </summary>
        /// <returns></returns>
        public bool Anonymous { get; set; } = true;

        /// <summary>
        /// 订单明细guid
        /// </summary>
        [Required(ErrorMessage = "订单明细guid必填")]
        public string OrderDetailGuid { get; set; }
    }
}
