using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.User
{
    /// <summary>
    /// 获取用户信息列表 请求
    /// </summary>
    public class GetGetAccountListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
    /// <summary>
    /// 获取用户信息列表 响应
    /// </summary>
    public class GetAccountListResponseDto : BasePageResponseDto<GetAccountListItemDto>
    {
    }
    /// <summary>
    /// 获取用户信息列表 项
    /// </summary>
    public class GetAccountListItemDto : BaseDto
    {
        ///<summary>
        ///GUID
        ///</summary>
        public string UserGuid { get; set; }
        ///<summary>
        ///用户昵称
        ///</summary>
        public string UserName { get; set; }
        ///<summary>
        ///手机号码
        ///</summary>
        public string Phone { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        ///<summary>
        ///部门ID
        ///</summary>
        public string OrganizationGuid { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrganizationName { get; set; }

        ///<summary>
        ///超级管理员
        ///</summary>
        public sbyte IsSuper { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }
        
    }
}
