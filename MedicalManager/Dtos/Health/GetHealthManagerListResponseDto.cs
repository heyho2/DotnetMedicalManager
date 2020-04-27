using GD.Common.Base;
using Newtonsoft.Json;
using System;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 姓名/手机号码
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegistrationTime { get; set; }
        /// <summary>
        /// 后台用
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerListResponseDto : BasePageResponseDto<GetHealthManagerItem>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerItem : BaseDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string ManagerGuid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 性别（M/F），默认为M
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string PortraitImg { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 已绑定用户数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegistrationTime { get; set; }
        /// <summary>
        /// 1：启用，0：禁用
        /// </summary>
        public bool Enable { get; set; }
    }
}
