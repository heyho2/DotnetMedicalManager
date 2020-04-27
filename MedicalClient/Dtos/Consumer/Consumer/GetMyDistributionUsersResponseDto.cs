using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取下级分销人员列表响应Dto
    /// </summary>
    public class GetMyDistributionUsersResponseDto : BasePageResponseDto<GetMyDistributionUsersItemDto>
    {
    }
    /// <summary>
    /// 获取下级分销人员列表项Dto
    /// </summary>
    public class GetMyDistributionUsersItemDto : BaseDto
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait { get; set; }
    }
}
