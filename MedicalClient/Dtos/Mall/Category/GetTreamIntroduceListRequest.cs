using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Category
{
    /// <summary>
    /// 团队介绍
    /// </summary>
    public class GetTreamIntroduceListRequest : BasePageRequestDto
    {
        /// <summary>
        ///二级分类Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "二级分类Guid")]
        public string DicGuid { get; set; }

        /// <summary>
        ///商户Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "商户Guid")]
        public string MerchantGuid { get; set; }
    }
    /// <summary>
    /// 团队介绍列表响应Dto
    /// </summary>
    public class GetTreamIntroduceResponseDto : BasePageResponseDto<GetTreamIntroduceItemResponse>
    {

    }
    /// <summary>
    /// response
    /// </summary>
    public class GetTreamIntroduceItemResponse : BaseDto
    {
        /// <summary>
        ///调理师Guid
        /// </summary>
        public string TherapistGuid { get; set; }
        /// <summary>
        ///名字
        /// </summary>
        public string TherapistName { get; set; }
        /// <summary>
        ///职称
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        ///手机
        /// </summary>
        public string TherapistPhone { get; set; }
        /// <summary>
        ///照片
        /// </summary>
        public string PortraitURL { get; set; }
        /// <summary>
        ///擅长的标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        ///介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        ///排序
        /// </summary>
        public int Sort { get; set; }

    }


}
