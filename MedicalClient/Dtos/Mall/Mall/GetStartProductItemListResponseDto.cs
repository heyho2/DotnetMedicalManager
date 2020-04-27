using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 生美-明星项目列表 请求Dto
    /// </summary>
    public class GetStartProductItemListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 分类枚举
        /// </summary>
        public string ClassifyName { get; set; } = ClassifyEnum.StartProduct.ToString();

        ///// <summary>
        ///// 是否推荐的（即首页推广展示位）
        ///// </summary>
        //public bool Recommend { get; set; } = true;
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
    /// 生美-明星项目列表 响应Dto
    /// </summary>
    public class GetStartProductItemListResponseDto : BasePageResponseDto<GetStartProductItemItemDto>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetStartProductItemItemDto :BaseDto
    {
        /// <summary>
        /// 分类Guid
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string TargetGuid { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommend { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal TargetPrice { get; set; }
        /// <summary>
        /// 医生Guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 医生名称
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SoldTotal { get; set; } = 0;
    }
}
