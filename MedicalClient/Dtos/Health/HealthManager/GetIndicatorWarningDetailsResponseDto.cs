using GD.Common.Base;
using GD.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    /// <summary>
    /// 获取预警记录详情响应
    /// </summary>
    public class GetIndicatorWarningDetailsResponseDto : BaseDto
    {
        /// <summary>
        /// 预警记录Id
        /// </summary>
        public string WarningGuid { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 预警日期
        /// </summary>
        public DateTime WarningDate { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 预警描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 预警状态：1:待处理（Pending），2：已失效(Expired)，3：已关闭(Closed)
        /// </summary>
        public IndicatorWarningStatusEnum Status { get; set; }
    }
}
