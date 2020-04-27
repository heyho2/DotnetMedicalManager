using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.User
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    public class GetAccountInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 用户id（不填写则是获取当前用户）
        /// </summary>
        public string UserID { get; set; }
    }
}
