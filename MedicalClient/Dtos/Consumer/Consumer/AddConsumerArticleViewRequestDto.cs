using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 添加文章浏览记录
    /// </summary>
    public class AddConsumerArticleViewRequestDto
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "文章ID")]
        public string TargetGuid { get; set; }


    }
}
