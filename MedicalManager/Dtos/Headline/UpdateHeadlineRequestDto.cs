using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Headline
{
    /// <summary>
    /// 修改头条 请求
    /// </summary>
    public class UpdateHeadlineRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string HeadlineGuid { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string HeadlineName { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string HeadlineAbstract { get; set; }
        /// <summary>
        /// 响应目标
        /// </summary>
        public string Target { get; set; }
    }
}
