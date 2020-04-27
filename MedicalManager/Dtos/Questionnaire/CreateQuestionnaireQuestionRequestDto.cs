using GD.Common.Base;
using GD.Dtos.Enum.Questionnaire;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 创建问卷问题草稿
    /// </summary>
    public class CreateQuestionnaireQuestionRequestDto : BaseDto
    {
        /// <summary>
        /// 问卷guid
        /// </summary>
        [Required(ErrorMessage = "问卷guid必填")]
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 问题guid
        /// </summary>
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 问题名称
        /// </summary>
        [Required(ErrorMessage = "问题名称必填")]
        [StringLength(30, ErrorMessage = "问题名称不能超过30个字符")]
        public string QuestionName { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        [Required(ErrorMessage = "问卷类型必填")]
        public QuestionnaireQuestionTypeEnum QuestionType { get; set; }

        /// <summary>
        /// 单位（只有数值型有）
        /// </summary>
        [StringLength(7, ErrorMessage = "单位不能超过7个字符")]
        public string Unit { get; set; }

        /// <summary>
        /// 提示文字（只有问答题有）
        /// </summary>
        [StringLength(80, ErrorMessage = "提示文字不能超过80个字符")]
        public string PromptText { get; set; }

        /// <summary>
        /// 是否依赖
        /// </summary>
        public bool IsDepend { get; set; }

        /// <summary>
        /// 依赖的问题答案guid
        /// </summary>
        public string DependAnswer { get; set; }

        /// <summary>
        /// 问题序号
        /// </summary>
        [Required(ErrorMessage = "问卷序号必填")]
        public int Sort { get; set; }

        /// <summary>
        /// 问卷答案选项集合
        /// </summary>
        public IList<Answer> Answers { get; set; }

        /// <summary>
        /// 问卷答案选项
        /// </summary>
        public class Answer
        {
            /// <summary>
            /// 若此回答为编辑后提交，需带出原答案guid
            /// </summary>
            public string AnswerGuid { get; set; }

            /// <summary>
            /// 答案选项值
            /// </summary>
            public string AnswerLabel { get; set; }

            /// <summary>
            /// 是否是默认选项
            /// </summary>
            public bool IsDefault { get; set; } = false;
        }
    }


    /// <summary>
    /// 创建问卷问题草稿
    /// </summary>
    public class CreateQuestionnaireQuestionResponseDto : BaseDto
    {
        /// <summary>
        /// 问卷guid
        /// </summary>
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 问题guid
        /// </summary>
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        public string QuestionName { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        public QuestionnaireQuestionTypeEnum QuestionType { get; set; }

        /// <summary>
        /// 是否依赖
        /// </summary>
        public bool IsDepend { get; set; }

        /// <summary>
        /// 依赖的问题答案guid
        /// </summary>
        public string DependAnswer { get; set; }


        /// <summary>
        /// 依赖描述
        /// </summary>
        public string DependDescription { get; set; }

        /// <summary>
        /// 问题序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 问卷答案选项集合
        /// </summary>
        public IList<CreateQuestionnaireQuestionRequestDto.Answer> Answers { get; set; }
    }
}
