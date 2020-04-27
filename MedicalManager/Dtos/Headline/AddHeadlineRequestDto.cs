using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Headline
{
    /// <summary>
    /// 组织架构列表 请求
    /// </summary>
    public class AddHeadlineRequestDto : BaseDto
    {
        
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
