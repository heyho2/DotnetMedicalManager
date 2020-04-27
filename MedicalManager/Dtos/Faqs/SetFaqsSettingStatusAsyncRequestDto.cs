using GD.Common.Base;
using System.ComponentModel;

namespace GD.Dtos.Faqs
{
    /// <summary>
    /// 设置问题状态
    /// </summary>
    public class SetFaqsSettingStatusAsyncRequestDto : BaseDto
    {
        ///<summary>
        ///提问主键
        ///</summary>
        public string QuestionGuid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>

        public QuestionStatusEnum Status { get; set; }

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
    }
}
