using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor
{
    /// <summary>
    ///医生积分列表 请求
    /// </summary>
    public class ExportDoctorIntegralRequestDto : BaseDto
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 注册时间 至
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
    /// <summary>
    ///医生积分列表 响应
    /// </summary>
    public class ExportDoctorIntegralResponseDto
    {
        /// <summary>
        /// 结果集
        /// </summary>
        public IEnumerable<GetDoctorIntegralPageItemDto> Items { get; set; }
    }
}
