using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class ModifyPhysicalProductBasicInfoRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "产品分类必填")]
        public string CategoryGuid { get; set; }
        /// <summary>
        /// 商品guid
        /// </summary>
        [Required(ErrorMessage = "商品guid必填")]
        public string ProductGuid { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Required(ErrorMessage = "商品名称必填")]
        public string ProductName { get; set; }
        /// <summary>
        /// 产品标题
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [Required(ErrorMessage = "商品编码必填")]
        public string ProductCode { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [Required(ErrorMessage = "商品图片guid必填")]
        public string PictureGuid { get; set; }
        /// <summary>
        /// 是否在售
        /// </summary>
        public bool OnSale { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 有效时间
        /// </summary>
        public int? EffectiveDays { get; set; } = 0;
        /// <summary>
        /// 是否推荐首页
        /// </summary>
        public bool Recommend { get; set; }
    }
}
