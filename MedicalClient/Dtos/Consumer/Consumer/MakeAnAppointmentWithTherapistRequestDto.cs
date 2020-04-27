using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 预约美疗师请求Dto
    /// </summary>
    public class MakeAnAppointmentWithTherapistRequestDto : BaseDto
    {
        /// <summary>
        /// 排班guid
        /// </summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 预约消费来源项guid (个人商品项guid)
        /// </summary>
        public string FromItemGuid { get; set; }

        /// <summary>
        /// 服务对象成员guid
        /// </summary>
        public string ServiceMemberGuid { get; set; }

        /// <summary>
        /// 预约时间（开始时间）
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 预约时间（结束时间）
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 预约备注
        /// </summary>
        [StringLength(255)]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 消费来源项类型
    /// </summary>
    public enum ConsumptionFromItemType
    {
        /// <summary>
        /// 个人商品项
        /// </summary>
        [Description("个人商品项")]
        GoodsItem = 1,
        /// <summary>
        /// 礼物
        /// </summary>
        [Description("礼物")]
        Gift
    }
}
