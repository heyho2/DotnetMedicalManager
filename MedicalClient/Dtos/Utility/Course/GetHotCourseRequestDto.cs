using GD.Common.Base;
using System;

namespace GD.Dtos.Utility.Course
{
    /// <summary>
    /// 获取首页热门课程 请求
    /// </summary>
    public class GetHotCourseRequestDto
    {
        /// <summary>
        /// 多久以前时间（利用这个就可以查出几天前的热门数据）
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 取多少条数据
        /// </summary>
        public int Take { get; set; } = 6;
    }
    /// <summary>
    /// 获取首页热门课程 响应
    /// </summary>
    public class GetHotCourseItemDto : BaseDto
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

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        public int LikeCount {get;set;}

        /// <summary>
        /// 浏览量
        /// </summary>
        public int VisitCount { get; set;}
    }
}
