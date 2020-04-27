using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Score
{
    /// <summary>
    /// 行为特性反参DTO
    /// </summary>
    public class GetActionRulesResponseDto
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 用户行为GUID
        /// </summary>
        public string RulesGuid { get; set; }

        /// <summary>
        /// 行为特性CODE
        /// </summary>
        public string ActionCharacteristicsCode { get; set; }

        /// <summary>
        /// 行为特性类型
        /// </summary>
        public string ActionCharacteristicsType { get; set; }

        /// <summary>
        /// 行为特性名称
        /// </summary>
        public string ActionCharacteristicsName { get; set; }
    }
}
