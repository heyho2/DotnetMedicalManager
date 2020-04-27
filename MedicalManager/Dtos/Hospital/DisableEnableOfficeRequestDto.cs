using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Hospital
{
    /// <summary>
    /// 启动禁用科室
    /// </summary>
    public class DisableEnableOfficeRequestDto : BaseDto
    {
        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
