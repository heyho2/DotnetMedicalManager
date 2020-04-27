using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;
using GD.Dtos.Doctor.Doctor;

namespace GD.Dtos.Mall.Project
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取明星医生王牌项目请求Dto
    /// </summary>
    public class GetStarDoctorGoodProjectRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <inheritdoc />
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetStarDoctorGoodProjectResponseDto : BasePageResponseDto<GetStarDoctorGoodProjectItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetStarDoctorGoodProjectItem : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }

    }
}
