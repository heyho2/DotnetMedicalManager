using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 创建问卷初始化
    /// </summary>
    public class InitQuestionnaireResponseDto : BaseDto
    {
        /// <summary>
        /// 问卷guid
        /// </summary>
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 问卷名称
        /// </summary>
        public string QuestionnaireName { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subhead { get; set; }
    }
}
