using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 推荐产品列表获取Dto
    /// </summary>
    public class GetRecommendProductListRequestDto : BasePageRequestDto
    {
    }
    /// <summary>
    /// 产品列表获取Dto
    /// </summary>
    public class GetByCategoryProductListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 一级产品类型Id
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "产品类型Id")]
        public string CategoryGuid { get; set; }
    }
    /// <summary>
    /// 产品列表响应Dto
    /// </summary>
    public class GetProductListResponseDto : BasePageResponseDto<GetProductListItemDto>
    {

    }
    /// <summary>
    /// 列表明细Dto
    /// </summary>
    public class GetProductListItemDto : BaseDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品图片路径
        /// </summary>
        public string PicturePath { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  商品类型
        /// </summary>
        public string ProductForm { get; set; }
    }
}
