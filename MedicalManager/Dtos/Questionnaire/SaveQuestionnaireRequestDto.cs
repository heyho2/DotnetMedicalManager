using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 保存问卷请求dto
    /// </summary>
    public class SaveQuestionnaireRequestDto : BaseDto
    {
        /// <summary>
        /// 问卷guid
        /// </summary>
        [Required(ErrorMessage = "问卷guid必填")]
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 问卷名称
        /// </summary>
        [Required(ErrorMessage = "问卷名称必填")]
        [StringLength(30,ErrorMessage ="问卷名称长度不能超过30")]
        public string QuestionnaireName { get; set; }

        /// <summary>
        /// 问卷副标题
        /// </summary>
        [Required(ErrorMessage = "问卷副标题必填")]
        [StringLength(200, ErrorMessage = "问卷名称长度不能超过200")]
        public string Subhead { get; set; }
    }
}
