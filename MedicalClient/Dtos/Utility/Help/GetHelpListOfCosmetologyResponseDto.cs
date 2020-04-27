using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Help
{
    /// <summary>
    /// 获取双美平台帮助列表响应Dto
    /// </summary>
    public class GetHelpListOfCosmetologyResponseDto : BaseDto
    {
        /// <summary>
        /// 主键guid
        /// </summary>
        public string HelpGuid { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 解答
        /// </summary>
        public string Answer { get; set; }
    }
}
