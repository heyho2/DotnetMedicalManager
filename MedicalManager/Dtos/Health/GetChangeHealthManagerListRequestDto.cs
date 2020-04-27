using GD.Common.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetChangeHealthManagerListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 会员Guid
        /// </summary>
        [Required(ErrorMessage = "会员参数未提供")]
        public string ConsumerGuid { get; set; }
        /// <summary>
        /// 姓名/手机号码
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegistrationTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    ///账户明细响应
    /// </summary>
    public class GetMealAccountListResponseDto<T> where T : class
    {
        /// <summary>
		/// 当前页数据
		/// </summary>
		public IEnumerable<T> CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetChangeHealthManagerItem
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string ManagerGuid { get; set; }
        /// <summary>
        /// 是否为指定会员绑定的健康管理师
        /// </summary>
        public bool Default { get; set; } = false;
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
        /// 注册时间
        /// </summary>
        public DateTime RegistrationTime { get; set; }
    }
}
