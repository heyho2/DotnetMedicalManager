using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    public class GetHealthManagerConsumerListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 参数不需要上传
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 查询
        /// </summary>
        public string KeyWord { get; set; }
    }
    public class GetHealthManagerConsumerListResponseDto : BasePageResponseDto<GetHealthManagerConsumerItem>
    {
    }
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
        /// 更新时间
        /// </summary>
        public DateTime? MaxDate { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime? ManagerBindDate { get; set; }
        /// <summary>
        /// 附件更新时间
        /// </summary>
        public DateTime? ReportMaxDate { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string PortraitImg { get; set; }
    }
}
