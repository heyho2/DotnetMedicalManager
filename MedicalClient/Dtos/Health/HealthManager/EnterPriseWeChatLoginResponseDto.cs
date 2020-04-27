using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    /// <summary>
    /// 美疗师企业微信授权登录响应Dto
    /// </summary>
    public class EnterPriseWeChatLoginResponseDto : BaseDto
    {
        /// <summary>
        /// 健康管理师Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 健康管理师姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录token
        /// </summary>
        public string Token { get; set; }
    }
}
