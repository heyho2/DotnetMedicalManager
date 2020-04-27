using GD.Common.Base;
using GD.Dtos.Manager.Banner;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 发布商品-智慧云医
    /// </summary>
    public class CreateProductOfDoctorCloudRequestDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商铺guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商品类别
        /// </summary>
        [Required(ErrorMessage = "商品类别必填")]
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 是否是实体商品
        /// </summary>
        [Required(ErrorMessage = "是否是实体商品必填")]
        public bool IsPhysical { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int OperationTime { get; set; }

        /// <summary>
        /// 品牌guid
        /// </summary>
        public string BrandGuid { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Required(ErrorMessage = "规格必填")]
        public string Standerd { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required(ErrorMessage = "商品名称必填")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品搜索关键词,最多六个关键词，以 "、"隔开
        /// </summary>
        public string SearchKey { get; set; }

        /// <summary>
        /// 产品标题
        /// </summary>
        public string ProductTitle { get; set; }

        /// <summary>
        /// 批准文号
        /// </summary>
        [Required(ErrorMessage = "批准文号必填")]
        public string ApprovalNumber { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [Required(ErrorMessage = "产品编码必填")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 是否上架在售（销售中、已下架）
        /// </summary>
        [Required(ErrorMessage = "是否上架在售")]
        public bool OnSale { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal? Freight { get; set; } = 0M;

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal? CostPrice { get; set; }

        /// <summary>
        /// 售卖价格
        /// </summary>
        [Required(ErrorMessage ="商品售卖价格必填")]
        public decimal Price { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [Required(ErrorMessage = "商品库存必填")]
        public int Inventory { get; set; } = 0;

        /// <summary>
        /// 警戒库存
        /// </summary>
        public int WarningInventory { get; set; } = 0;

        /// <summary>
        /// 商品图片guid
        /// </summary>
        [Required(ErrorMessage = "商品图片Guid必填")]
        public string PictureGuid { get; set; }


        /// <summary>
        /// 商品介绍富文本
        /// </summary>
        [Required(ErrorMessage = "商品介绍富文本必填")]
        public string Introduce { get; set; }

        /// <summary>
        /// 商品详情富文本
        /// </summary>
        [Required(ErrorMessage = "商品详情富文本必填")]
        public string ProductDetail { get; set; }

        /// <summary>
        /// 保质期（单位为月）
        /// </summary>
        public string RetentionPeriod { get; set; }

        /// <summary>
        /// 商品Banner
        /// </summary>
        public List<BannerBaseDto> Banners { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacture { get; set; }

    }
}
