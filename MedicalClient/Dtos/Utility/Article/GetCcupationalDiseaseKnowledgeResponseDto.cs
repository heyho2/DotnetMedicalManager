using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Utility.Article
{
    /// <summary>
    /// 职业病常识文章响应Dto
    /// </summary>
    public class GetCcupationalDiseaseKnowledgeResponseDto : BaseDto
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "文章GUID")]
        public string ArticleGuid
        {
            get;
            set;
        }

        ///<summary>
        ///标题
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "标题")]
        public string Title
        {
            get;
            set;
        }

        ///<summary>
        ///简介
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "简介")]
        public string Abstract
        {
            get;
            set;
        }

        ///<summary>
        ///图片
        ///关联附件表
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "图片")]
        public string Picture
        {
            get;
            set;
        }
    }
}
