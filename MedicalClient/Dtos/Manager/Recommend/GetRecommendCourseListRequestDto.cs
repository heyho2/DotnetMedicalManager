using GD.Common.Base;

namespace GD.Dtos.Manager.Recommend
{
    /// <summary>
    /// 获取课程推荐 请求
    /// </summary>
    public class GetRecommendCourseListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 推荐Guid
        /// </summary>
        public string RecommendGuid { get; set; }
    }
    /// <summary>
    /// 获取课程推荐 响应
    /// </summary>
    public class GetRecommendCourseListResponseDto : BasePageResponseDto<GetRecommendCourseListItemDto>
    {

    }
    /// <summary>
    /// 获取课程推荐 项
    /// </summary>
    public class GetRecommendCourseListItemDto : BaseDto
    {

        /// <summary>
        /// 推荐id
        /// </summary>
        public string RecommendGuid { get; set; }

        ///<summary>
        ///文章GUID
        ///</summary>
        public string CourseGuid { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// loginGuid
        /// </summary>
        public string LogoGuid { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string LogoUrl { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string Summary { get; set; }
    }
}
