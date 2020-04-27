using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Dtos.Doctor.Doctor
{
    /// <inheritdoc />
    /// <summary>
    /// 生美-获取明星医生推荐列表 请求Dto
    /// </summary>
    public class GetStartDoctorListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 分类枚举
        /// </summary>
        public string ClassifyName { get; set; } = ClassifyEnum.StartDoctor.ToString();

        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString();

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }

    /// <inheritdoc />
    /// <summary>
    /// 生美-获取明星医生推荐列表 响应Dto
    /// </summary>
    public class GetStartDoctorListResponseDto:BasePageResponseDto<GetStartDDoctorListItemDto>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetStartDDoctorListItemDto : BaseDto
    {
        /// <summary>
        /// 分类Guid
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommend { get; set; }
        /// <summary>
        /// 医生Guid
        /// </summary>
        public string TargetGuid { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// 医生头衔
        /// </summary>
        public string TargetTitle { get; set; }
        /// <summary>
        /// 医生图片URL
        /// </summary>
        public string TargetUrl { get; set; }
    }
}
