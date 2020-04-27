using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Article
{
    /// <summary>
    /// 获取客户端综合文章(普通文章+健康管理文章)详情响应Dto
    /// </summary>
    public class GetClientArticleDetailResponseDto : BaseDto
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
        /// 作者Guid
        /// </summary>
        public string AuthorGuid { get; set; }

        /// <summary>
        /// 作者姓名
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// 作者头像(平台文章无此数据)
        /// </summary>
        public string AuthorPortrait { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        /// <summary>
        /// 富文本内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 医生医院名称(平台文章无此数据)
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医生科室名称(平台文章无此数据)
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int LikeNumber { get; set; }

        /// <summary>
        /// 是否点赞
        /// </summary>
        public bool Liked { get; set; }
    }
}
