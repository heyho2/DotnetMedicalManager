using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 预约美疗师(消费者入口)响应Dto
    /// </summary>
    public class MakeAnAppointmentWithTherapistResponseDto : BaseDto
    {
        /// <summary>
        /// 预约的店铺名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 商户地址
        /// </summary>
        public string MerchantAddress { get; set; }

        /// <summary>
        /// 预约的美疗师名称
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 消费码
        /// </summary>
        public string ConsumptionNo { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// 美疗师头像url
        /// </summary>
        public string ProjectPictureUrl { get; set; }

        /// <summary>
        /// 预约对象资料
        /// </summary>
        public string ServiceMember { get; set; }

        /// <summary>
        /// 门店项目类目
        /// </summary>
        public string CategoryExtension { get; set; }

    }
}
