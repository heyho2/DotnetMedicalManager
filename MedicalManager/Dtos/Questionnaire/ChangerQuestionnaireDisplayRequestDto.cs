using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 切换问卷显示状态
    /// </summary>
    public class ChangerQuestionnaireDisplayRequestDto
    {
        /// <summary>
        /// 问卷guid
        /// </summary>
        public string QuestionnaireGuid { get; set; }


        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }
    }
}
