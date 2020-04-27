using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取美疗师最近排班 请求Dto
    /// </summary>
    public class GetBeautyDoctorSchedulingRequestDto : BasePageRequestDto
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
    public class GetBeautyDoctorSchedulingResponseDto : BasePageResponseDto<GetBeautyDoctorSchedulingItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetBeautyDoctorSchedulingItem : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
    }
}
