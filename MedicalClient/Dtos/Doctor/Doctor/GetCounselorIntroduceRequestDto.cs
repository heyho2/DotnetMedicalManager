using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取咨询师介绍 请求Dto
    /// </summary>
    public class GetCounselorIntroduceRequestDto : BasePageRequestDto
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
    public class GetCounselorIntroduceResponseDto : BasePageResponseDto<GetCounselorIntroduceItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetCounselorIntroduceItem : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
    }
}