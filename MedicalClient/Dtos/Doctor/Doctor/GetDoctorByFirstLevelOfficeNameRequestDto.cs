using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 根据一级科室名称获取医生
    /// </summary>
    public class GetDoctorByFirstLevelOfficeNameRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 一级科室名称
        /// </summary>
        [Required(ErrorMessage ="科室名称必填")]
        public string OfficeName { get; set; }

        /// <summary>
        /// 不需要传递此参数
        /// </summary>
        public List<string> OfficeIds { get; set; }
    }
}
