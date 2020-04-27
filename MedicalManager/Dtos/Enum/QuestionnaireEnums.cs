using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Enum.Questionnaire
{
    /// <summary>
    /// 问卷问题类型枚举
    /// </summary>
    public enum QuestionnaireQuestionTypeEnum
    {
        /// <summary>
        /// 单选题
        /// </summary>
        [Description("单选题")]
        Enum = 1,

        /// <summary>
        /// 判断题
        /// </summary>
        [Description("判断题")]
        Bool,

        /// <summary>
        /// 数值填空
        /// </summary>
        [Description("数值题")]
        Decimal,

        /// <summary>
        /// 问答
        /// </summary>
        [Description("问答题")]
        String,

        /// <summary>
        /// 多选题
        /// </summary>
        [Description("多选题")]
        Array
    }
}
