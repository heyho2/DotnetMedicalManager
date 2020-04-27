using GD.Common.Base;
using GD.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 获取健康基础信息列表响应
    /// </summary>
    public class GetHealthInfoBasicDataResponseDto : BaseDto
    {
        /// <summary>
        /// 信息guid
        /// </summary>
        public string InformationGuid { get; set; }

        /// <summary>
        /// 信息类型
        /// </summary>
        public HealthInformationTypeEnum InformationType { get; set; }

        /// <summary>
        /// 信息名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 单位：仅数值类型有此项
        /// </summary>
        public string SubjectUnit { get; set; }

        /// <summary>
        /// 问答提示语：仅问答类型有此项
        /// </summary>
        public string SubjectPromptText { get; set; }

        /// <summary>
        /// 是否是单行文本：仅问答题类型有此项
        /// </summary>
        public bool IsSingleLine { get; set; } = false;

        /// <summary>
        /// 信息排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 详情列表
        /// </summary>
        public List<HealthInfoOption> Options { get; set; } = new List<HealthInfoOption>();

        /// <summary>
        /// 基础信息详情
        /// </summary>
        public class HealthInfoOption : BaseDto
        {
            /// <summary>
            /// 选项guid
            /// </summary>
            public string OptionGuid { get; set; }

            /// <summary>
            /// 选项序号
            /// </summary>
            public int Sort { get; set; }

            /// <summary>
            /// 选项值
            /// </summary>
            public string OptionLabel { get; set; }

            /// <summary>
            /// 是否默认值：仅问答题有
            /// </summary>
            public bool IsDefault { get; set; } = false;
        }
    }
}
