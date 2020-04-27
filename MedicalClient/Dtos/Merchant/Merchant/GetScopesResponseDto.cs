using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取经营范围基础数据响应Dto
    /// </summary>
    public class GetScopesResponseDto : BaseDto
    {
        /// <summary>
        /// 配置项Guid
        /// </summary>
        public string DicGuid { get; set; }

        /// <summary>
        /// 配置项Code
        /// </summary>
        public string ConfigCode { get; set; }

        /// <summary>
        /// 配置项名称
        /// </summary>
        public string ConfigName { get; set; }
    }
}
