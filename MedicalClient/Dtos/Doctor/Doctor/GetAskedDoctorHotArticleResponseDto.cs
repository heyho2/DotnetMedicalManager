using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取问医热点文章响应Dto
    /// </summary>
    public class GetAskedDoctorHotArticleResponseDto : BaseDto
    {
        /// <summary>
        /// 文章Guid
        /// </summary>
        public string ArticleGuid { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int PageView { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        public int LikeTotal { get; set; }

        /// <summary>
        /// 文章图片
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public string ArticleType { get; set; }
    }
}
