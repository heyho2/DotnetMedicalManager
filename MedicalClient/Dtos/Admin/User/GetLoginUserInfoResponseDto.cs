using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.User
{
    /// <summary>
    /// 获取用户信息 响应
    /// </summary>
    public class GetLoginUserInfoResponseDto : BaseDto
    {
        ///<summary>
        ///GUID
        ///</summary>
        public string UserGuid { get; set; }
        ///<summary>
        ///用户昵称
        ///</summary>
        public string NickName { get; set; }
        ///<summary>
        ///手机号码
        ///</summary>
        public string Phone { get; set; }
        ///<summary>
        ///头像
        ///</summary>
        public string PortraitUrl { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
    }
}
