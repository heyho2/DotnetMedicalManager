using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Admin.Course
{
    /// <summary>
    /// 禁用文章
    /// </summary>
    public class SetCourseVisibleRequestDto : BaseDto
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "文章GUID")]
        public string CourseGuid { get; set; }
        /// <summary>
        /// 是否可阅读
        /// </summary>
        public bool Visible { get; set; }
    }
}
