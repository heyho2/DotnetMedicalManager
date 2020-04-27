using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 适用门店Dto
    /// </summary>
    public class ProjectMerchantDto:BaseDto
    {
        /// <summary>
        /// 门店guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 门店地址
        /// </summary>
        public string MerchantAddress { get; set; }
    }
}
