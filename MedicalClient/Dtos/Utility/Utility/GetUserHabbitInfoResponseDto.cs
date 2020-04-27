using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Utility.Utility
{
    /// <inheritdoc />
    /// <summary>
    /// 配置
    /// </summary>
    public class GetUserHabbitInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 配置ID
        /// </summary>
        public string ConfGuid { get; set; }
        /// <summary>
        /// 特征ID
        /// </summary>
        public string CharacterGuid { get; set; }
        /// <summary>
        /// 值的数据类型
        /// </summary>
        public string ValueType { get; set; }
        /// <summary>
        /// 配置名
        /// </summary>
        public string ConfName { get; set; }

        /// <summary>
        /// 取值范围（针对枚举）
        /// </summary>
        public object ValueRange { get; set; }

        /// <summary>
        /// 特征值
        /// </summary>
        public string ConfValue { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; } = 0;
    }
}
