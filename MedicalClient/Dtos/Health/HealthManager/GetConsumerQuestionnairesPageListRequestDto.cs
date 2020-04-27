using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerQuestionnairesPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "指定会员标识未提供")]
        public string UserGuid { get; set; }
        /// <summary>
        /// 问卷名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序标准（JSON格式：【[{"Field":"fill_status","SortOrder":1},{"Field":"commented","SortOrder":2}]】）
        /// 上述JSON格式：【fill_status：填写状态，commented：评论状态，SortOrder：1（正序），2（倒序）】
        /// </summary>
        public List<SortCriteria> SortCriterias { get; set; }
    }

    /// <summary>
    /// 排序字段 
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// 正序
        /// </summary>
        [Description("正序")]
        ASC = 1,
        /// <summary>
        /// 倒序
        /// </summary>
        [Description("倒序")]
        DESC
    }

    /// <summary>
    /// 
    /// </summary>
    public class SortCriteria
    {
        /// <summary>
        /// 
        /// </summary>
        static List<string> Fields => new List<string>() { "fill_status", "commented" };
        /// <summary>
        /// 
        /// </summary>
        static List<string> Sorts => new List<string>() { "ASC", "DESC" };

        /// <summary>
        /// 排序字段（【fill_status：填写状态】，【commented：评论状态】）
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 排序方式，1：正序，2：倒序
        /// </summary>
        public SortOrder SortOrder { get; set; }
        /// <summary>
        /// 校验是否满足约定格式
        /// </summary>
        /// <returns></returns>
        public bool Exist()
        {
            return Fields.Contains(Field) && Sorts.Contains(SortOrder.ToString());
        }
        /// <summary>
        /// 是否已填写问卷
        /// </summary>
        /// <returns></returns>
        public bool IsFill()
        {
            return Field.Equals(Fields[0]);
        }
        /// <summary>
        /// 是否已评论
        /// </summary>
        /// <returns></returns>
        public bool IsCommented()
        {
            return Field.Equals(Fields[1]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerQuestionnairesPageListResponseDto : BasePageResponseDto<GetQuestionnaireItem>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetQuestionnaireItem : BaseDto
    {
        /// <summary>
        /// 问卷结果Guid
        /// </summary>
        public string ResultGuid { get; set; }
        /// <summary>
        /// 问卷Id
        /// </summary>
        public string QuestionnaireGuid { get; set; }
        /// <summary>
        /// 问卷名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 填写状态
        /// </summary>
        public bool FillStatus { get; set; }
        /// <summary>
        /// 评论状态
        /// </summary>
        public bool CommentedStatus { get; set; }
        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime? IssuingDate { get; set; }
    }
}
