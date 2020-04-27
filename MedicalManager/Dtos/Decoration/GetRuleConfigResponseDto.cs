using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 获取类别下的规则配置数据
    /// </summary>
    public class GetRuleConfigResponseDto : BaseDto
    {
        /// <summary>
        /// 规则选择方式
        /// </summary>
        public RuleModeEnum RuleMode { get; set; }

        /// <summary>
        /// 规则集合
        /// </summary>
        public List<DecorationRule> DecorationRules { get; set; }
    }
}
