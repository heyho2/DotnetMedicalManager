using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 修改商品基础信息(智慧云医) 请求Dto
    /// </summary>
    public class ModifyProductBasicInfoOfDoctorCloudRequestDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        [Required(ErrorMessage = "商品guid必填")]
        public string ProductGuid { get; set; }

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
        [Required(ErrorMessage = "品牌guid必填")]
        public string BrandGuid { get; set; }

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
        /// 保质期（单位为月）
        /// </summary>
        public string RetentionPeriod { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [Required(ErrorMessage = "商品编码必填")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacture { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Required(ErrorMessage = "规格必填")]
        public string Standerd { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Required(ErrorMessage = "商品图片guid必填")]
        public string PictureGuid { get; set; }
    }
}
