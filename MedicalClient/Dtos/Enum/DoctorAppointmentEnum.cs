using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Enum.DoctorAppointment
{
    /// <summary>
    /// 就诊人关系
    /// </summary>
    public enum InquiryRelationshipEnum
    {
        /// <summary>
        /// 自己
        /// </summary>
        [Description("自己")]
        Own = 1,
        /// <summary>
        /// 亲属
        /// </summary>
        [Description("亲属")]
        Relatives = 2,
        /// <summary>
        /// 朋友
        /// </summary>
        [Description("朋友")]
        Friend = 3,
        /// <summary>
        /// 其它
        /// </summary>
        [Description("其它")]
        Other
    }
    /// <summary>
    /// 预约状态
    /// </summary>
    public enum AppointmentStatusEnum
    {
        /// <summary>
        /// 待就诊
        /// </summary>
        [Description("待就诊")]
        Waiting = 1,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancel,
        /// <summary>
        /// 已就诊
        /// </summary>
        [Description("已就诊")]
        Treated,
        /// <summary>
        /// 爽约
        /// </summary>
        [Description("爽约")]
        Miss
    }
}
