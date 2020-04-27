using GD.Common.Base;
using System;

namespace GD.Dtos.User
{
    /// <summary>
    /// 获取用户信息 响应
    /// </summary>
    public class GetAccountInfoResponseDto : BaseDto
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
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuper { get; set; }
        ///<summary>
        ///头像
        ///</summary>
        public string PortraitUrl { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        ///<summary>
        ///部门ID
        ///</summary>
        public string OrganizationGuid { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string OrganizationName { get; set; }

        ///<summary>
        ///EMAIL
        ///</summary>
        public string Email { get; set; }

        ///<summary>
        ///微信号
        ///</summary>
        public string WechatOpenid { get; set; }

        ///<summary>
        ///真实姓名
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        public string Gender { get; set; }

        ///<summary>
        ///生日
        ///</summary>
        public DateTime? Birthday { get; set; }

        ///<summary>
        ///头像
        ///</summary>
        public string PortraitGuid { get; set; }
        /// <summary>
        /// 是否能登陆
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string[] Roles { get; set; }
    }
}
