using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Comment
{
    /// <summary>
    /// 获取商铺下商品的所有评论响应Dto
    /// </summary>
    public class GetProductCommentsOfMerchantResponseDto : BasePageResponseDto<GetProductCommentsOfMerchantItemDto>
    {
    }

    /// <summary>
    /// 获取商铺下商品的所有评论响应ItemDto
    /// </summary>
    public class GetProductCommentsOfMerchantItemDto : BaseDto
    {
        /// <summary>
        /// 评论guid
        /// </summary>
        public string CommentGuid { get; set; }

        /// <summary>
        /// 产品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 评论人guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string UserNickName { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 评论日期
        /// </summary>
        public DateTime CommentDate { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }



    }
}
