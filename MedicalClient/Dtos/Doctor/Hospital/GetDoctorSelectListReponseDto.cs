using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    public class GetDoctorSelectListReponseDto : BaseDto
    {
        /// <summary>
        /// 医生guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
    }
}
