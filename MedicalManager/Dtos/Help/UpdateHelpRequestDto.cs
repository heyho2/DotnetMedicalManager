using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Help
{
    /// <summary>
    /// 修改Question
    /// </summary>
    public class UpdateHelpRequestDto : BaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string HelpGuid { get; set; }
        /// <summary>
        /// 问题
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public string PlatformType { get; set; }
    }
}
