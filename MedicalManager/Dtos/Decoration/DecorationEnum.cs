using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 规则选择模式
    /// </summary>
    public enum RuleModeEnum
    {
        /// <summary>
        /// 单选
        /// </summary>
        [Description("单选")]
        Single = 1,
        /// <summary>
        /// 多选
        /// </summary>
        [Description("多选")]
        Multiple
    }

    /// <summary>
    /// 行图片样式
    /// </summary>
    public enum DecorationStyleEnum
    {
        /// <summary>
        /// 轮播图
        /// </summary>
        [Description("轮播图")]
        Slideshow = 1,

        /// <summary>
        /// 平铺图
        /// </summary>
        [Description("平铺图")]
        Tile
    }

    /// <summary>
    /// 行列数取值范围
    /// </summary>
    public enum RangeRuleEnum
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Equal = 1,
        /// <summary>
        /// 小于或等于
        /// </summary>
        [Description("小于或等于")]
        LessThan,
        /// <summary>
        /// 大于或等于
        /// </summary>
        [Description("大于或等于")]
        GreaterThan,
    }
}
