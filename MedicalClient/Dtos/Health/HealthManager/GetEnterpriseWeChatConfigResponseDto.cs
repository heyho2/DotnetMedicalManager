using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    public class GetEnterpriseWeChatConfigResponseDto:BaseDto
    {
        /// <summary>
        /// 企业微信AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 企业微信应用id
        /// </summary>
        public string AgentId { get; set; }
    }
}
