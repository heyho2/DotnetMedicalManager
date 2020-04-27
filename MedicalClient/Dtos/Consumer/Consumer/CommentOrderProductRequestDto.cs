using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 评价订单商品请求dto
    /// </summary>
    public class CommentOrderProductRequestDto
    {
        /// <summary>
        /// 订单商品评价记录guid
        /// </summary>
        [Required(ErrorMessage = "订单商品评价记录guid必填")]
        public string ProductCommentGuid { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
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
    }
}
