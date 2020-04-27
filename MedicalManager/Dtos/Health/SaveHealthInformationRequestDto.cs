using GD.Common.Base;
using GD.Dtos.Enum;
using GD.Models.Health;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 保存健康档案-基础信息数据
    /// </summary>
    public class SaveHealthInformationRequestDto : BaseDto
    {
        /// <summary>
        /// 基础信息列表
        /// </summary>
        public List<HealthInfo> Infos { get; set; }

        /// <summary>
        /// 基础信息
        /// </summary>
        public class HealthInfo
        {
            /// <summary>
            /// 信息guid
            /// </summary>
            public string InformationGuid { get; set; }

            /// <summary>
            /// 信息类型
            /// </summary>
            [Required(ErrorMessage = "信息类型必填")]
            public HealthInformationTypeEnum InformationType { get; set; }

            /// <summary>
            /// 信息名称
            /// </summary>
            [Required(ErrorMessage = "信息名称必填")]
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
        }

        /// <summary>
        /// 基础信息详情
        /// </summary>
        public class HealthInfoOption : BaseDto
        {
            /// <summary>
            /// 选项guid：编辑时需传入此值
            /// </summary>
            public string OptionGuid { get; set; }

            /// <summary>
            /// 选项序号
            /// </summary>
            [Required(ErrorMessage = "选项序号必填")]
            public int Sort { get; set; }

            /// <summary>
            /// 选项值
            /// </summary>
            [Required(ErrorMessage = "选项值必填")]
            public string OptionLabel { get; set; }

            /// <summary>
            /// 是否默认值：仅问答题有
            /// </summary>
            public bool IsDefault { get; set; } = false;
        }
    }

    /// <summary>
    /// 保存健康基础信息上下文
    /// </summary>
    public class SaveHealthInformationContext
    {
        /// <summary>
        /// 请求dto
        /// </summary>
        public SaveHealthInformationRequestDto RequestDto { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="requestDto"></param>
        public SaveHealthInformationContext(SaveHealthInformationRequestDto requestDto)
        {
            RequestDto = requestDto;
        }
        /// <summary>
        /// 全部的健康基础信息
        /// </summary>
        public List<HealthInformationModel> AllInfos { get; set; } = new List<HealthInformationModel>();

        /// <summary>
        /// 全部的健康基础信息选项
        /// </summary>
        public List<HealthInformationOptionModel> AllOptions { get; set; } = new List<HealthInformationOptionModel>();


        /// <summary>
        /// 待更新的健康信息
        /// </summary>
        public List<HealthInformationModel> UpdateInfos { get; set; } = new List<HealthInformationModel>();

        /// <summary>
        /// 待新增的健康信息
        /// </summary>
        public List<HealthInformationModel> AddInfos { get; set; } = new List<HealthInformationModel>();

        /// <summary>
        /// 待删除的健康信息
        /// </summary>
        public List<HealthInformationModel> DeleteInfos { get; set; } = new List<HealthInformationModel>();

        /// <summary>
        /// 待更新的信息选项
        /// </summary>
        public List<HealthInformationOptionModel> UpdateOptions { get; set; } = new List<HealthInformationOptionModel>();

        /// <summary>
        /// 待新增的信息选项
        /// </summary>
        public List<HealthInformationOptionModel> AddOptions { get; set; } = new List<HealthInformationOptionModel>();

        /// <summary>
        /// 待删除的信息选项
        /// </summary>
        public List<HealthInformationOptionModel> DeleteOptions { get; set; } = new List<HealthInformationOptionModel>();


    }
}
