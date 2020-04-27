using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Banner
{
    /// <summary>
    /// 添加banner
    /// </summary>
    public class AddBannerRequestDto : BaseDto
    {


        ///<summary>
        ///属于谁的GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "属于谁的GUID")]
        public string OwnerGuid { get; set; }

        ///<summary>
        ///BANNER名称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "BANNER名称")]
        public string BannerName { get; set; }

        ///<summary>
        ///BANNER图片GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "BANNER图片GUID")]
        public string PictureGuid { get; set; }

        ///<summary>
        ///目标地址
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "目标地址")]
        public string TargetUrl { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort { get; set; }
        /// <summary>
        /// 启用？
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(100), Display(Name = "描述")]
        public string Description { get; set; }
    }
}
