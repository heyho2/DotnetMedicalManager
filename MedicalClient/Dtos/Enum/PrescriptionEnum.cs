using System.ComponentModel;

namespace GD.Dtos.Enum
{
    /// <summary>
    /// 处方相关枚举
    /// </summary>
    public enum PrescriptionEnum
    {
        /// <summary>
        /// 用法
        /// </summary>
        [Description("用法")]
        Usage = 1,
        /// <summary>
        /// 用量
        /// </summary>
        [Description("用量")]
        Dosage,
        /// <summary>
        /// 频度
        /// </summary>
        [Description("频度")]
        Frequency
    }

    /// <summary>
    /// 就诊类型
    /// </summary>
    public enum ReceptionTypeEnum
    {
        /// <summary>
        /// 初诊
        /// </summary>
        [Description("初诊")]
        First = 1,

        /// <summary>
        /// 复诊
        /// </summary>
        [Description("复诊")]
        Repeat
    }

    public enum ReceptionRecipeTypeEnum
    {
        /// <summary>
        /// 药品收费
        /// </summary>
        Drug = 1,
        /// <summary>
        /// 治疗收费
        /// </summary>
        Treatment = 2
    }
    /// <summary>
    /// 处方状态枚举
    /// </summary>
    public enum PrescriptionStatusEnum
    {
        /// <summary>
        /// 待付款
        /// </summary>
        [Description("待付款")]
        Obligation = 1,
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        Paied=2,
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Cancellation=3
    }

    /// <summary>
    /// 接诊记录收款状态
    /// </summary>
    public enum PrescriptionInformationPaidStatus
    {
        /// <summary>
        /// 未收款
        /// </summary>
        [Description("未收款")]
        NotPaid = 1,
       
        /// <summary>
        /// 部分未收款
        /// </summary>
        [Description("部分未收款")]
        PartiallyUnpaid,
        /// <summary>
        /// 已收款
        /// </summary>
        [Description("已收款")]
        Paided,
        /// <summary>
        /// 无有效处方
        /// </summary>
        [Description("无有效处方")]
        None
    }
}
