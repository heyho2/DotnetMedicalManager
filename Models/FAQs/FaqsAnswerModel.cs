using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.FAQs
{
    ///<summary>
    /// 问题模块-回答表实体
    ///</summary>
    [Table("t_faqs_answer")]
    public class FaqsAnswerModel : BaseModel
    {
        ///<summary>
        ///主键
        ///</summary>
        [Column("answer_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string AnswerGuid { get; set; }

        ///<summary>
        ///问题主键
        ///</summary>
        [Column("question_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "问题主键")]
        public string QuestionGuid { get; set; }

        ///<summary>
        ///用户
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户")]
        public string UserGuid { get; set; }

        ///<summary>
        ///是否主解答
        ///</summary>
        [Column("main_answer"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否主解答")]
        public bool MainAnswer { get; set; }

        ///<summary>
        ///已获数额
        ///</summary>
        [Column("reward_intergral"), Required(ErrorMessage = "{0}必填"), Display(Name = "已获悬赏数额")]
        public int RewardIntergral { get; set; } = 0;
        ///<summary>
        ///悬赏类型
        ///</summary>
        [Column("receive_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "悬赏类型")]
        public string ReceiveType { get; set; } = AnswerReceiveTypeEnum.Intergral.ToString();
        /// <summary>
        /// 内容
        /// </summary>
        [Column("content"), Required(ErrorMessage = "{0}必填"), Display(Name = "内容")]
        public string Content { get; set; }

        /// <summary>
        /// 悬赏类型枚举
        /// </summary>
        public enum AnswerReceiveTypeEnum
        {
            /// <summary>
            /// 积分
            /// </summary>
            [Description("积分")]
            Intergral = 1,
            /// <summary>
            /// 人民币
            /// </summary>
            [Description("人民币")]
            Money
        }


    }
}



