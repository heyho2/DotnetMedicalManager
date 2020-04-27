using GD.Dtos.Consumer.Consumer;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 添加预约-请求Dto
    /// </summary>
    public class AddAppointmentRequestDto
    {
        /// <summary>
        /// 需要预约的用户手机号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "需要预约的用户手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 项来源guid(个人商品项guid/礼物guid/卡券guid)
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "服务项目")]
        public string FromItemGuid { get; set; }

        /// <summary>
        /// 排班guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "排班guid")]
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 预约时间（结束时间）【不用传】
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 预约时间（开始时间）
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "预约时间（开始时间）")]
        public string StartTime { get; set; }

        /// <summary>
        /// 服务对象guid（有就传，没有不传）
        /// </summary>
        public string ServiceMemberGuid { get; set; }
        /// <summary>
        /// 服务项目Id
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 添加预约-响应Dto
    /// </summary>
    public class AddAppointmentResponseDto
    {
        /// <summary>
        /// 排班guid
        /// </summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 预约消费来源项guid (个人商品项guid/礼物guid)
        /// </summary>
        public string FromItemGuid { get; set; }

        /// <summary>
        /// 消费来源项类型
        /// </summary>
        public ConsumptionFromItemType FromItemType { get; set; }

        /// <summary>
        /// 预约时间（开始时间）
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 预约时间（结束时间）
        /// </summary>
        public string EndTime { get; set; }

    }

}
