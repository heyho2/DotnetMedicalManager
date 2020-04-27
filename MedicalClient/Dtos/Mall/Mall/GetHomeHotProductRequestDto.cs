using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 获取首页热门商品 请求
    /// </summary>
    public class GetHomeHotProductRequestDto : BaseDto
    {
        /// <summary>
        /// 取多少条记录
        /// </summary>
        [Range(1, 100)]
        public int Take { get; set; } = 7;
    }
    /// <summary>
    /// 获取首页热门商品 项
    /// </summary>
    public class GetHomeHotProductItemDto : BaseDto
    {
        ///<summary>
        ///产品GUID
        ///</summary>
        public string ProductGuid { get; set; }

        ///<summary>
        ///商户GUID
        ///</summary>
        public string MerchantGuid { get; set; }

        ///<summary>
        ///所属分类GUID
        ///</summary>
        public string CategoryGuid { get; set; }

        ///<summary>
        ///所属分类名称
        ///</summary>
        public string CategoryName { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        public string PictureUrl { get; set; }

        ///<summary>
        ///产品名称
        ///</summary>
        public string ProductName { get; set; }

        ///<summary>
        ///产品标签
        ///</summary>
        public string ProductLabel { get; set; }
        ///<summary>
        ///品牌
        ///</summary>
        public string Brand { get; set; }

        ///<summary>
        ///规格
        ///</summary>
        public string Standerd { get; set; }

        ///<summary>
        ///保质期
        ///</summary>
        public string RetentionPeriod { get; set; }

        ///<summary>
        ///生产厂家
        ///</summary>
        public string Manufacture { get; set; }

        ///<summary>
        ///批准文号
        ///</summary>
        public string ApprovalNumber { get; set; }
        ///<summary>
        ///价格
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public decimal Inventory { get; set; }

        ///<summary>
        ///是否热门
        ///</summary>
        public bool Recommend { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }

        ///<summary>
        ///平台类型
        ///</summary>
        public string PlatformType { get; set; }
    }
}
