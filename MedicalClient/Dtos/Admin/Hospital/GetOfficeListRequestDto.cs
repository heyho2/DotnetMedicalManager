using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Hospital
{
    /// <summary>
    /// 获取科室
    /// </summary>
    public class GetOfficeListRequestDto : BaseDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
