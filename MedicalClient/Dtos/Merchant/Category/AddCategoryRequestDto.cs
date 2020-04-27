using GD.Common.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Category
{
    /// <summary>
    /// 创建类别请求
    /// </summary>
    public class AddMerchantCategoryRequestDto : BaseDto
    {
        ///<summary>
        ///主键
        ///</summary>
        public string CategoryGuid { get; set; }

        ///<summary>
        ///服务性类别GUID
        ///</summary>
        [Required(ErrorMessage = "平台分类需选择")]
        [MaxLength(40, ErrorMessage = "超过平台分类最大长度限制")]
        public string ClassifyGuid { get; set; }

        /// <summary>
        /// 服务性类别名称
        /// </summary>
        [Required(ErrorMessage = "平台分类需选择")]
        [MaxLength(30, ErrorMessage = "超过平台分类名称最大长度限制")]
        public string ClassifyName { get; set; }

        ///<summary>
        ///商户GUID
        ///</summary>
        public string MerchantGuid { get; set; }

        ///<summary>
        ///类别名称
        ///</summary>
        [Required(ErrorMessage = "大类名称必填")]
        [MaxLength(30, ErrorMessage = "超过大类名称最大长度限制")]
        public string CategoryName { get; set; }

        ///<summary>
        ///地址
        ///</summary>
        public string[] Address { get; set; }

        ///<summary>
        ///详细地址
        ///</summary>
        [Required(ErrorMessage = "详细地址必填")]
        [MaxLength(500, ErrorMessage = "超过地址最大长度限制")]
        public string DetailAddress { get; set; }

        ///<summary>
        ///封面图片GUID
        ///</summary>
        [Required(ErrorMessage = "封面图片需提供")]
        [MaxLength(40, ErrorMessage = "超过封面图片最大长度限制")]
        public string CoverGuid { get; set; }

        ///<summary>
        ///联系电话
        ///</summary>
        [Required(ErrorMessage = "联系电话必填")]
        [StringLength(maximumLength: 11, MinimumLength = 11, ErrorMessage = "联系电话格式不正确")]
        public string Telephone { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        public List<CategoryBanner> Banners { get; set; } = new List<CategoryBanner>();

        /// <summary>
        /// 大类介绍
        /// </summary>
        [Required(ErrorMessage = "大类介绍必填")]
        [StringLength(maximumLength: 500, ErrorMessage = "大类介绍超过最大长度限制")]
        public string Introduction { get; set; }

        ///<summary>
        ///经纬度数组
        ///</summary>
        public string[] LongLatitude { get; set; }

        /// <summary>
        /// 提前多少分钟取消预约限制（分钟）
        /// </summary>
        public int LimitTime { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
    }

    /// <summary>
    /// 类别Banner
    /// </summary>
    public class CategoryBanner
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
    }
}
