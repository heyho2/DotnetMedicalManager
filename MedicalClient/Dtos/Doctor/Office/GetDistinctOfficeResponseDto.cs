using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Office
{
    /// <summary>
    /// 去重科室响应Dto
    /// </summary>
    public class GetDistinctOfficeResponseDto: BaseDto
    {
        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 科室图片
        /// </summary>
        public string OfficePicture { get; set; }
    }
}
