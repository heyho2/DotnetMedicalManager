using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 名医推荐Dto
    /// </summary>
    public class GetFamousRecommendDoctorListRequestDto : BasePageRequestDto
    {
    }
    /// <summary>
    /// 名医推荐响应Dto
    /// </summary>
    public class GetFamousRecommendDoctorListResponseDto : BasePageResponseDto<GetFamousRecommendDoctorListItemDto>
    {

    }
    /// <summary>
    /// 推荐医生列表响应Dto
    /// </summary>
    public class GetFamousRecommendDoctorListItemDto : BaseDto
    {
        /// <summary>
        /// 医生guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 医生头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 医院guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 科室guid
        /// </summary>
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 擅长标签
        /// </summary>
        public string AdeptTags { get; set; }

        /// <summary>
        /// 医生职称
        /// </summary>
        public string Title { get; set; }
    }
}
