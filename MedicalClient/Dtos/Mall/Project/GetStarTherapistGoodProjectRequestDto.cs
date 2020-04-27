using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;
using GD.Dtos.Doctor.Doctor;

namespace GD.Dtos.Mall.Project
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取明星美疗师王牌项目请求Dto
    /// </summary>
    public class GetStarTherapistGoodProjectRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 美疗师Guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <inheritdoc />
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetStarTherapistGoodProjectResponseDto : BasePageResponseDto<GetStarTherapistGoodProjectItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetStarTherapistGoodProjectItem : BaseDto
    {
        /// <summary>
        /// 美疗师guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 美疗师名称
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 项目Guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public string SoldCount { get; set; }

    }
}
