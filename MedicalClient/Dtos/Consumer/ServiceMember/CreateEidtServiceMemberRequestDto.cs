using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.ServiceMember
{
    /// <summary>
    /// 创建/修改服务对象成员数据请求dto
    /// </summary>
    public class CreateEidtServiceMemberRequestDto : BaseDto
    {
        /// <summary>
        /// 服务对象成员guid
        /// </summary>
        public string ServiceMemberGuid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 年龄——岁
        /// </summary>
        public int AgeYear { get; set; } = 0;

        /// <summary>
        /// 年龄——月
        /// </summary>
        public int? AgeMonth { get; set; } = 0;

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
}
