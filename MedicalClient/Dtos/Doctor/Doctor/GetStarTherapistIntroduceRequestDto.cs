using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取明星医生介绍 请求Dto
    /// </summary>
    public class GetStarTherapistIntroduceRequestDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetStarTherapistIntroduceResponseDto
    {
        /// <summary>
        /// 美疗师Guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 美疗师名称
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 工龄
        /// </summary>
        public string WorkAge { get; set; }

        /// <summary>
        /// 头像Url
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 所获荣誉
        /// </summary>
        public string Honor { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 美疗师介绍
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 是否明星医生
        /// </summary>
        public string Recommend { get; set; }
    }
   
}