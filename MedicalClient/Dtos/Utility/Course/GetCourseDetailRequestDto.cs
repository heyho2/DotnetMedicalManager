using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Course
{
    /// <summary>
    /// 获取课程详细 请求
    /// </summary>
    public class GetCourseDetailRequestDto : BaseDto
    {
        ///<summary>
        ///主键
        ///</summary>
        public string CourseGuid { get; set; }
    }
    /// <summary>
    /// 获取课程详细 响应 
    /// </summary>
    public class GetCourseDetailResponseDto : BaseDto
    {
        ///<summary>
        ///主键
        ///</summary>
        public string CourseGuid { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string AuthorName { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string Summary { get; set; }

        ///<summary>
        ///否显示
        ///</summary>
        public bool Visible { get; set; }

        ///<summary>
        ///文章富文本内容GUID
        ///</summary>
        public string ContentGuid { get; set; }
        /// <summary>
        /// 医生
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生头像
        /// </summary>
        public string DoctorPortrait { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }


        /// <summary>
        /// 文章富文本内容
        /// </summary>
        public string Content { get; set; }

        ///<summary>
        ///图片url
        ///</summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// 图片guid
        /// </summary>
        public string LogoGuid { get; set; }

        ///<summary>
        ///课堂视频资源
        ///</summary>
        public string VideoUrl { get; set; }
        /// <summary>
        /// 课堂视频资源guid
        /// </summary>
        public string VideoGuid { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        public int LikeTotal { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int VisitTotal { get; set; }

        /// <summary>
		/// 创建时间
		/// </summary>
        public DateTime CreationDate
        {
            get;
            set;
        }

        /// <summary>
		/// 最后修改日期
		/// </summary>
        public DateTime LastUpdatedDate
        {
            get;
            set;
        }
    }
}
