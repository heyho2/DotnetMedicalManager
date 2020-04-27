using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Admin.Course
{
    /// <summary>
    /// 添加文章
    /// </summary>
    public class AddCourseRequestDto : BaseDto
    {

        ///<summary>
        ///文章富文本内容
        ///</summary>
        [Required(ErrorMessage = "文本内容必填")]
        public string Content { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        [Required(ErrorMessage = "标题必填")]
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        [Required(ErrorMessage = "简介必填")]
        public string Abstract { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        public string PictureGuid { get; set; }

        ///<summary>
        ///是否显示
        ///</summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
