using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Utility.Article
{
    /// <summary>
    /// 职业病常识文章请求Dto
    /// </summary>
    public class GetCcupationalDiseaseKnowledgeRequestDto : BaseDto
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "页码")]
        public int PageNumber { get; set; }

        /// <summary>
        /// 单页条数
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "单页条数")]
        public int PageSize { get; set; }
    }
}
