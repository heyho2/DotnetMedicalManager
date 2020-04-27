using System;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 预约成功通知Dto
    /// </summary>
    public class AppointmentNotificationDto
    {
        /// <summary>
        /// 预约店铺
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 预约消费时间
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// 美疗师姓名
        /// </summary>
        public string ThrapistName { get; set; }

        /// <summary>
        /// 消费者名称
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// 消费者手机号
        /// </summary>
        public string ConsumerPhone { get; set; }

        /// <summary>
        /// 预约项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
