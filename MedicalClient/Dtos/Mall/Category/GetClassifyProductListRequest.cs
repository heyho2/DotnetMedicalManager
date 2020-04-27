using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Category
{
    /// <summary>
    /// 类目产品
    /// </summary>
    public class GetClassifyProductListRequest:BasePageRequestDto
    {
        /// <summary>
        ///二级分类Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "二级分类Guid")]
        public string DicGuid { get; set; }

        /// <summary>
        ///商户Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "商户Guid")]
        public string MerchantGuid { get; set; }

    }

    /// <summary>
    /// response
    /// </summary>
    public class GetClassifyProductListResponse:BasePageResponseDto<GetClassifyProductListItem>
    {
        
      
    }

    public class GetClassifyProductListItem:BaseDto
    {
        /// <summary>
        ///商品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        ///商品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///商品标题
        /// </summary>
        public string ProductTitle { get; set; }

        /// <summary>
        ///图片URL
        /// </summary>
        public string PictureURL { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
    }
}
