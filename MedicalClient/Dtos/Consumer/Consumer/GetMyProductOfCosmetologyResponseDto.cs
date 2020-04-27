using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 双美-我收藏的商品响应Dto
    /// </summary>
    public class GetMyProductOfCosmetologyResponseDto : BasePageResponseDto<GetMyProductOfCosmetologyItemDto>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetMyProductOfCosmetologyItemDto : BaseDto
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal ProductPrice { get; set; }


        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MarketPrice { get; set; }


        /// <summary>
        /// 收藏日期
        /// </summary>
        public DateTime CollectionDate { get; set; }
    }
}
