using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Office
{
    /// <summary>
    /// 获取医院科室Dto列表（带有图片）
    /// </summary>
    public class GetHospitalOfficeDtoListRequestDto : BaseDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        [Required(ErrorMessage ="医院guid必填")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 上级科室guid
        /// </summary>
        public string ParentOfficeGuid { get; set; }
    }
}
