using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Collection
{
    /// <summary>
    /// 获取收藏目标的用户列表响应Dto
    /// </summary>
    public class GetTheUserListOfCollectionTargetResponseDto:BaseDto
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
        /// 用户头像url
        /// </summary>
        public string PortraitUrl { get; set; }
    }
}
