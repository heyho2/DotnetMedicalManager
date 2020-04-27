using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取医院下推荐医生列表请求Dto
    /// </summary>
    public class GetHospitalRecommendDoctorListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        [Required(ErrorMessage = "医院guid必填")]
        public string HospitalGuid { get; set; }
    }

    /// <summary>
    /// 获取医院下推荐医生列表响应Dto
    /// </summary>
    public class GetHospitalRecommendDoctorListResponseDto : BasePageResponseDto<GetHospitalRecommendDoctorListItemDto>
    {

    }

    /// <summary>
    /// 获取医院下推荐医生列表响应Dto
    /// </summary>
    public class GetHospitalRecommendDoctorListItemDto : BaseDto
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
        /// 医院logo
        /// </summary>
        public string HospitalLogo { get; set; }

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
