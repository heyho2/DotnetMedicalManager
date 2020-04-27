using GD.Common.Base;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerConsumerListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 健康管理师Guid
        /// </summary>
        [Required(ErrorMessage = "健康管理师参数未提供")]
        public string ManagerGuid { get; set; }
        /// <summary>
        /// 姓名/手机号码
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime? BindTime { get; set; }
        /// <summary>
        /// 后台用
        /// </summary>
        [JsonIgnore]
        public DateTime? EndTime { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerConsumerListResponseDto : BasePageResponseDto<GetHealthManagerConsumerItem>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerConsumerItem : BaseDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string ConsumerGuid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 性别（M/F），默认为M
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime BindDate { get; set; }
    }
}
