using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 根据分类查产品列表
    /// </summary>
    public class GetProductListByCategoryAndSortRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 产品分类Guid
        /// </summary>
        [Display(Name = "产品分类Guid")]
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [Display(Name = "分页类")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [Display(Name = "关键字")]
        public string Keyword { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [Display(Name = "排序字段")]
        public string OrderBy { get; set; } = "creation_date";

        /// <summary>
        /// 升序或降序
        /// </summary>
        [Display(Name = "升序或降序")]
        public string DescOrAsc { get; set; } = "DESC";


    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetProductListByCategoryAndSortResponseDto : BasePageResponseDto<GetProductListByCategoryAndSortItemDto>
    {
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetProductListByCategoryAndSortItemDto : BaseDto
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
        /// 商品副标题（卖点）
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// 产品规格
        /// </summary>
        public string Standerd { get; set; }
        /// <summary>
        /// 产品标签
        /// </summary>
        public string ProductLabel { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        public string ProPictureUrl { get; set; }
        /// <summary>
        /// 产品销量
        /// </summary>
        public int SoldTotal { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }

    }
}
