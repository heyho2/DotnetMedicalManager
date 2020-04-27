using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 获取装修记录内容响应Dto
    /// </summary>
    public class GetDecorationContentResponseDto : BaseDto
    {
        /// <summary>
        /// 行拼图样式：平铺或轮播
        /// Slideshow = 1 轮播 ; Tile = 2 平铺
        /// </summary>
        public DecorationStyleEnum Style { get; set; }
        /// <summary>
        /// 行集合
        /// </summary>
        public List<DecorationColumn> Columns { get; set; }
    }


    /// <summary>
    /// 拼图列内容
    /// </summary>
    public class DecorationColumn
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }
    }
}
