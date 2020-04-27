using GD.Common.Base;
using GD.Dtos.Manager.Banner;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 创建服务类产品
    /// </summary>
    public class CreateServiceProductRequestDto : BaseDto
    {
        /// <summary>
        /// 商品类别
        /// </summary>
        [Required(ErrorMessage = "商品类别必填")]
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 商品分类名称
        /// </summary>
        [Required(ErrorMessage = "商品分类名称")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required(ErrorMessage = "商品名称必填")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品标题
        /// </summary>
        public string ProductTitle { get; set; }

        /// <summary>
        /// 是否上架在售（销售中、已下架）
        /// </summary>
        public bool OnSale { get; set; } = true;

        /// <summary>
        /// 售卖价格
        /// </summary>
        [Required(ErrorMessage = "商品售卖价格必填")]
        public decimal Price { get; set; }

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
        /// 商品Banner
        /// </summary>
        public List<BannerBaseDto> Banners { get; set; }

        /// <summary>
        /// 自购买日期多少天有效（0表示永久有效）
        /// </summary>
        public int EffectiveDays { get; set; } = 0;

        /// <summary>
        /// 项目总次数使用阈值
        /// </summary>
        public int ProjectThreshold { get; set; } = 0;

        /// <summary>
        /// 是否推荐到热门
        /// </summary>
        public bool Recommend { get; set; } = false;

        /// <summary>
        /// 商品包含的服务项目集合
        /// </summary>
        public List<ProductProjectRelationDto> ProjectGuids { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override IEnumerable<ValidationResult> Verify(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Price <= 0)
            {
                result.Add(new ValidationResult("价格必须大于0"));
            }
            return result;
        }
    }

    /// <summary>
    /// 商品服务项目Dto
    /// </summary>
    public class ProductProjectRelationDto : BaseDto
    {
        /// <summary>
        /// 服务项目guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目次数
        /// </summary>
        public int ProjectTimes { get; set; } = 1;

        /// <summary>
        /// 表示没有次数限制
        /// </summary>
        public bool Infinite { get; set; } = false;
    }
}
