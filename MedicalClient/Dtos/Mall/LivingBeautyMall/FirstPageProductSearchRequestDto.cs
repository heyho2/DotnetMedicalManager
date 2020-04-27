using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.LivingBeautyMall
{
    /// <summary>
    /// 首页搜索-产品
    /// </summary>
    public class FirstPageProductSearchRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 搜索词
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "搜索词")]
        public string KeyWord { set; get; }



    }
}
