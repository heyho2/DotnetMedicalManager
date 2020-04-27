using System.Collections.Generic;

namespace GD.Dtos.Merchant.Category
{
    /// <summary>
    /// 获取类别详细信息
    /// </summary>
    public class GetMerchantCategoryDetailInfoResponseDto
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassifyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 大类名称
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
        /// 封面图
        /// </summary>
        public string CoverUrl { get; set; }
        /// <summary>
        /// 封面图Guid
        /// </summary>
        public string CoverGuid { get; set; }
        /// <summary>
        /// 经纬度
        /// </summary>
        public string LongLatitude { get; set; }
        /// <summary>
        /// 类型下项目取消预约需要提前的时间
        /// </summary>
        public int LimitTime { get; set; }
        /// <summary>
        /// 大类介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 主图集合
        /// </summary>
        public List<MerchantCategoryBanner> Banners { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MerchantCategoryBanner
    {
        /// <summary>
        /// 图片附件Guid
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
