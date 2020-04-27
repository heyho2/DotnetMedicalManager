using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GD.Common.Base;

namespace GD.Models.CrossTable
{
    /// <inheritdoc />
    /// <summary>
    /// 购物车信息model
    /// </summary>
    public class GetShoppingCarInfoModel : BaseModel
    {
        /// <summary>
        /// 购物车记录主键id
        /// </summary>
        [Column("item_guid"), Key, Display(Name = "购物车记录主键id")]
        public string ItemGuid { get; set; }

        /// <summary>
        /// 用户Guid
        /// </summary>
        [Column("user_guid"), Key, Display(Name = "用户Guid")]
        public string UserGuid { get; set; }
        /// <summary>
        /// 商户Guid
        /// </summary>
        [Column("merchant_guid"), Key, Display(Name = "商户Guid")]
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        [Column("merchant_name"), Key, Display(Name = "商户名称")]
        public string MerchantName { get; set; }
        /// <summary>
        /// 产品Guid
        /// </summary>
        [Column("product_guid"), Key, Display(Name = "产品Guid")]
        public string ProductGuid { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Column("product_name"), Key, Display(Name = "产品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("ProductPicUrl"), Key, Display(Name = "产品图片URL")]
        public string ProductPicUrl { get; set; }
        /// <summary>
        /// 产品简介Guid
        /// </summary>
        [Column("standerd"), Key, Display(Name = "产品规格")]
        public string Standerd { get; set; }
        /// <summary>
        /// 产品简介内容
        /// </summary>
        [Column("content"), Key, Display(Name = "产品简介内容")]
        public string Content { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        [Column("price"), Key, Display(Name = "产品价格")]
        public decimal Price { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        [Column("count"), Display(Name = "购买数量")]
        public string Count { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [Column("freight"), Key, Display(Name = "运费")]
        public decimal Freight { get; set; }

        /// <summary>
        /// 库存有效性
        /// </summary>
        [Column("is_valid"), Key, Display(Name = "库存有效性")]
        public bool IsValid { get; set; }

    }
}
