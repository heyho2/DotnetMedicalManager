using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 装修记录详情
    /// </summary>
    public class DecorationDto : BaseDto
    {
        /// <summary>
        /// 装修记录guid
        /// </summary>
        public string DecorationGuid { get; set; }

        /// <summary>
        /// 装修记录名称
        /// </summary>
        public string DecorationName { get; set; }

        /// <summary>
        /// 规则模式
        /// Single = 1 单选；Multiple=2 多选
        /// </summary>
        public RuleModeEnum RuleMode { get; set; }

        /// <summary>
        /// 分类guid
        /// </summary>
        public string ClassificationGuid { get; set; }

        /// <summary>
        /// 规则配置
        /// </summary>
        public List<DecorationRule> DecorationRules { get; set; }

        /// <summary>
        /// 拼图行集合
        /// </summary>
        public List<DecorationRow> Rows { get; set; }

        /// <summary>
        /// 拼图行
        /// </summary>
        public class DecorationRow
        {
            /// <summary>
            /// 行拼图样式：平铺或轮播
            /// Slideshow = 1 轮播 ; Tile = 2 平铺
            /// </summary>
            public DecorationStyleEnum Style { get; set; }

            /// <summary>
            /// 规则guid
            /// </summary>
            public string RuleGuid { get; set; }

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

    /// <summary>
    /// 规则
    /// </summary>
    public class DecorationRule : BaseDto
    {
        /// <summary>
        /// 装修规则
        /// </summary>
        public string RuleGuid { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowNum { get; set; }

        /// <summary>
        /// 行数规则：Equal-1等于,LessThan-2小于或等于,GreaterThan-3大于或等于
        /// </summary>
        public RangeRuleEnum RowRule { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnNum { get; set; }

        /// <summary>
        /// 列数规则：Equal-1等于,LessThan-2小于或等于,GreaterThan-3大于或等于
        /// </summary>
        public RangeRuleEnum ColumnRule { get; set; }

        /// <summary>
        /// 行图片样式：Slideshow-轮播图，Tile-平铺图
        /// </summary>
        public DecorationStyleEnum Style { get; set; }
    }
}
