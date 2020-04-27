using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Course
{
    /// <summary>
    /// 获取文章
    /// </summary>
    public class GetCourseInfoRequestDto : BaseDto
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "文章GUID")]
        public string CourseGuid { get; set; }


    }
    /// <summary>
    /// 获取文章 响应
    /// </summary>
    public class GetCourseInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 文章GUID
        /// </summary>
        public string CourseGuid { get; set; }

        ///<summary>
        ///文章富文本内容
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string Abstract { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        public string PictureGuid { get; set; }

        ///<summary>
        ///文章类型CODE
        ///</summary>
        public string ArticleTypeDic { get; set; }

        ///<summary>
        ///是否显示
        ///</summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 图片Url
        /// </summary>
        public string PictureUrl { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Enable { get; set; }
    }
}
