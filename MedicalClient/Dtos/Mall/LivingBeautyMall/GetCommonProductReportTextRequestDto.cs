﻿using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.LivingBeautyMall
{
    /// <inheritdoc />
    /// <summary>
    ///  生美-商品体验报告(富文本)
    /// </summary>
    public class GetCommonProductReportTextRequestDto : BaseDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
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
    /// 响应Dto
    /// </summary>
    public class GetCommonProductReportTextResponseDto : BaseDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 富文本Guid
        /// </summary>
        public string TextGuid { get; set; }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationDate { get; set; }
    }
}
