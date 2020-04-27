using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取下级分销人员列表请求Dto
    /// </summary>
    public class GetMyDistributionUsersRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 用户Guid
        /// </summary>
        public string UserGuid { get; set; }
    }
}
