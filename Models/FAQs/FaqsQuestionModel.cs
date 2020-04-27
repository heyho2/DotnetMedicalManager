using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.FAQs
{
    ///<summary>
    ///问答模块-问题
    ///</summary>
    [Table("t_faqs_question")]
    public class FaqsQuestionModel : BaseModel
    {
        ///<summary>
        ///提问主键
        ///</summary>
        [Column("question_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "提问主键")]
        public string QuestionGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///悬赏数额(单位分)
        ///</summary>
        [Column("reward_intergral"), Required(ErrorMessage = "{0}必填"), Display(Name = "悬赏数额")]
        public int RewardIntergral { get; set; }

        ///<summary>
        ///悬赏类型
        ///</summary>
        [Column("reward_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "悬赏积分")]
        public string RewardType { get; set; } = RewardTypeEnum.Intergral.ToString();

        ///<summary>
        ///状态
        ///</summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "状态")]
        public string Status { get; set; } = QuestionStatusEnum.Solving.ToString();

        ///<summary>
        ///已抢答数
        ///</summary>
        [Column("answer_num"), Required(ErrorMessage = "{0}必填"), Display(Name = "已抢答数")]
        public int AnswerNum { get; set; }

        ///<summary>
        ///内容
        ///</summary>
        [Column("content"), Required(ErrorMessage = "{0}必填"), Display(Name = "内容")]
        public string Content { get; set; }

        ///<summary>
        ///附件GUID，数据库为json格式：["Guid1","Guid2",……]
        ///</summary>
        [Column("attachment_guid_list"), Required(ErrorMessage = "{0}必填"), Display(Name = "附件GUID")]
        public string AttachmentGuidList { get; set; }
        ///<summary>
        ///流水Guid
        ///</summary>
        [Column("transfer_flowing_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "流水Guid")]
        public string TransferFlowingGuid { get; set; } = string.Empty;
        /// <summary>
        /// 状态枚举
        /// </summary>
        public enum QuestionStatusEnum
        {
            /// <summary>
            /// 解决中
            /// </summary>
            [Description("解决中")]
            Solving = 1,
            /// <summary>
            /// 已解决
            /// </summary>
            [Description("已解决")]
            Solved,
            /// <summary>
            /// 已结束
            /// </summary>
            [Description("已结束")]
            End,
            /// <summary>
            /// 待审核
            /// </summary>
            [Description("待审核")]
            Pending,
            /// <summary>
            /// 审核不通过
            /// </summary>
            [Description("审核不通过")]
            Reject,
            /// <summary>
            /// 已结束
            /// </summary>
            [Description("审核通过")]
            Adopt,
            /// <summary>
            /// 已取消
            /// </summary>
            [Description("已取消")]
            Cancel
        }

        /// <summary>
        /// 悬赏类型枚举
        /// </summary>
        public enum RewardTypeEnum
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



