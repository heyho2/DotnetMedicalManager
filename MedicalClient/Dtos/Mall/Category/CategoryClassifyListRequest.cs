using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Category
{
    /// <summary>
    /// 服务类目
    /// </summary>
    public class CategoryClassifyListRequest : BasePageRequestDto
    {
        /// <summary>
        /// 二级分类Guid (不填查全部)
        /// </summary>
        [Display(Name = "二级分类Guid")]
        public string DicGuid { get; set; }

    }

    /// <summary>
    /// response
    /// </summary>
    public class CategoryClassifyListResponse :BasePageResponseDto<CategoryClassifyListItem>
    {
      
    }
    /// <summary>
    /// 服务类目
    /// </summary>
    public class CategoryClassifyListItem : BaseDto
    {
        /// <summary>
        /// 类目Guid
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        ///  二级分类Guid
        /// </summary>
        public string ClassifyGuid { get; set; }

        /// <summary>
        /// 类目名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }
        /// <summary>
        /// 主图URL
        /// </summary>
        public string CoverURL { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
    }



}
