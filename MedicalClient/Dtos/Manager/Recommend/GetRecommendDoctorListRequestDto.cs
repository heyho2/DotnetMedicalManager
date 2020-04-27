using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Manager.Recommend
{
    /// <summary>
    /// 获取医生推荐列表 请求
    /// </summary>
    public class GetRecommendDoctorListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 推荐id
        /// </summary>
        public string RecommendGuid { get; set; }
        /// <summary>
        /// 医院Guid
        /// </summary>
        public string HospitalGuid { get; set; }

    }
    /// <summary>
    /// 获取医生推荐列表 响应
    /// </summary>
    public class GetRecommendDoctorListResponseDto : BasePageResponseDto<GetDoctorRecommendListItemDto>
    {
    }
    /// <summary>
    /// 获取医生推荐列表 项
    /// </summary>
    public class GetDoctorRecommendListItemDto : BaseDto
    {
        /// <summary>
        /// 推荐id
        /// </summary>
        public string RecommendGuid { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 医生id
        /// </summary>
        public string DoctorGuid { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string PortraitUrl { get; set; }
        /// <summary>
        /// 科室id
        /// </summary>
        public string OfficeGuid { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 工作城市
        /// </summary>
        public string WorkCity { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 医院logo
        /// </summary>
        public string DoctorLogo { get; set; }

        /// <summary>
        /// 擅长标签
        /// </summary>
        public string AdeptTags { get; set; }
        /// <summary>
        /// 荣誉
        /// </summary>
        public string Honors { get; set; }
        /// <summary>
        /// 职称Guid
        /// </summary>
        public string DocTitleGuid { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string DocTitle { get; set; }
    }
}
