using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Course
{
    /// <summary>
    /// 搜索文章课程 请求
    /// </summary>
    public class SearchCourseRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
    /// <summary>
    /// 搜索文章课程 响应
    /// </summary>
    public class SearchCourseResponseDto : BasePageResponseDto<SearchCourseItemDto>
    {

    }
    /// <summary>
    /// 搜索文章课程 项
    /// </summary>
    public class SearchCourseItemDto : BaseDto
    {
        ///<summary>
        ///主键
        ///</summary>
        public string CourseGuid { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string Summary { get; set; }

        /// <summary>
        /// 图片url
        /// </summary>
        public string LogoUrl { get; set; }
    }
}
