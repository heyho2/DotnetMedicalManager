using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.ActuatingStation
{
    /// <summary>
    /// 获取美疗师资料
    /// </summary>
    public class GetTherapistInfoResponseDto
    {
        /// <summary>
        /// 美疗师名称
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 美疗师手机号
        /// </summary>
        public string TherapistPhone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 店铺guid
        /// </summary>
        public string MerchantGuid { get; set; }
    }
}
