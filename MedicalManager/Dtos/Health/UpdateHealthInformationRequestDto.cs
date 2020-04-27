﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateHealthInformationRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "会员标识未提供")]
        public string UserGuid { get; set; }
        /// <summary>
        ///健康基础信息项
        /// </summary>
        public List<HealthInformationItem> Items { get; set; } = new List<HealthInformationItem>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class HealthInformationItem
    {
        /// <summary>
        /// 健康信息Id
        /// </summary>
        public string InformationGuid { get; set; }
        /// <summary>
        /// 健康信息选项Id
        /// </summary>
        public List<string> OptionGuids { get; set; } = new List<string>();
        /// <summary>
        /// 用户填空值
        /// </summary>
        public string ResultValue { get; set; }
    }
}
