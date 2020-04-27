﻿using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 评价消费
    /// </summary>
    public class CommentConsumptionRequestDto : BaseDto
    {
        /// <summary>
        /// 订单明细guid
        /// </summary>
        [Required(ErrorMessage = "消费记录guid")]
        public string ConsumptionGuid { get; set; }

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
    }
}
