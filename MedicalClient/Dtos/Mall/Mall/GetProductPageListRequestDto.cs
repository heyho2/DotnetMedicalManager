using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 获取产品列表-请求类
    /// </summary>
    public class GetProductPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 产品分类Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "产品分类Guid")]
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [Display(Name = "分页类")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

    }
}
