using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取医生文章详情响应Dto
    /// </summary>
    public class GetDoctorArticleDetailsResponseDto : BaseDto
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
        /// 最后一次修改时间
        /// </summary>
        public string LastUpdatedDate { get; set; }

        /// <summary>
        /// 富文本内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 医生医院Guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 医生医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医生科室Guid
        /// </summary>
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 医生科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 医生头像
        /// </summary>
        public string DoctorPortrait { get; set; }

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
