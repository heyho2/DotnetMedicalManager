using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 产品列表返回类
    /// </summary>
    public class GetProductPageListResponseDto : BaseDto
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
        /// 产品销量
        /// </summary>
        public int SoldTotal { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        public string ProPictureUrl{ get; set; }
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

    }
}
