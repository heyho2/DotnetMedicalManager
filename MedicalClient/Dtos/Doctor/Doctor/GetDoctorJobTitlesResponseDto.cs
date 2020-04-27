using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取医生职称列表响应Dto
    /// </summary>
    public class GetDoctorJobTitlesResponseDto : BaseDto
    {
        /// <summary>
        /// 字典项Guid
        /// </summary>
        public string DicGuid { get; set; }
        /// <summary>
        /// 配置项code
        /// </summary>
        public string ConfigCode { get; set; }
        /// <summary>
        /// 配置项名称
        /// </summary>
        public string ConfigName { get; set; }
    }
}
