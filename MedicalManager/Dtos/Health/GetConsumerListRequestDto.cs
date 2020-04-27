using GD.Common.Base;
using Newtonsoft.Json;
using System;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 姓名/手机号
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 注册开始时间
        /// </summary>
        public DateTime? RegistrationTime { get; set; }
        /// <summary>
        /// 注册结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerListResponseDto : BasePageResponseDto<GetConsumerItem>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerItem : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 健康管理师Guid
        /// </summary>
        public string ManagerGuid { get; set; }
        /// <summary>
        /// 健康管理师名称
        /// </summary>
        public string ManagerName { get; set; }
        /// <summary>
        /// 健康管理师手机号
        /// </summary>
        public string ManagerPhone { get; set; }
        /// <summary>
        /// 会员注册时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedDate { get; set; }
    }
}
