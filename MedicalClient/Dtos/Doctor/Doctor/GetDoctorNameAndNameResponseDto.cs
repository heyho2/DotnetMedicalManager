using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取医生头像和姓名响应Dto
    /// </summary>
    public class GetDoctorNameAndNameResponseDto : BaseDto
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
        /// 医生头像url
        /// </summary>
        public string DoctorPortraitUrl { get; set; }
    }
}
