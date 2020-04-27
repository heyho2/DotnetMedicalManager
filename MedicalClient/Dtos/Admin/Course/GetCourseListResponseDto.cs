using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Course
{
    /// <summary>
    /// 获取文章列表 请求
    /// </summary>
    public class GetCourseListRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }

    }
    /// <summary>
    /// 获取文章列表 响应
    /// </summary>
    public class GetCourseListResponseDto : BasePageResponseDto<GetCourseListItemDto>
    {

    }
    /// <summary>
    /// 获取文章列表 项
    /// </summary>
    public class GetCourseListItemDto : BaseDto
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        public string CourseGuid { get; set; }

        ///<summary>
        ///作者GUID
        ///</summary>
        public string AuthorGuid { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string Summary { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public string Enable { get; set; }

        ///<summary>
        ///是否显示
        ///</summary>
        public bool Visible { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        /// <summary>
        /// 点赞
        /// </summary>
        public int LikeCount { get; set; }
        /// <summary>
        /// 访问
        /// </summary>
        public int VisitCount { get; set; }
        /// <summary>
        /// 收藏
        /// </summary>
        public int Collection { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }

    }
}
