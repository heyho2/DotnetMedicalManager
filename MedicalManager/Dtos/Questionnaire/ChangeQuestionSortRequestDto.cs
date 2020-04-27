using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 改变问题序号请求dto
    /// </summary>
    public class ChangeQuestionSortRequestDto : BaseDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        [Required(ErrorMessage = "问题guid必填")]
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 变化后的序号
        /// </summary>
        [Required(ErrorMessage = "变化后的序号必填")]
        [Range(1, int.MaxValue)]
        public int Sort { get; set; }
    }
}
