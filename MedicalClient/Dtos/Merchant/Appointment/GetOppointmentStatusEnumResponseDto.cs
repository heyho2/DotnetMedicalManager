using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 获得预约状态枚举-响应Dto
    /// </summary>
    public class GetOppointmentStatusEnumResponseDto
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
