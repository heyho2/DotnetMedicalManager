using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.MerchantFlowing
{
    /// <summary>
    /// 项目销售排名请求Dto
    /// </summary>
    public class ProjectSoldReportRankingAsyncRequestDto
    {
        /// <summary>
        /// 商户GUID
        /// </summary>
        [Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "开始时间")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "结束时间")]
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class ProjectSoldReportRankingAsyncResponseDto
    {
        /// <summary>
        /// 商户Guid
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 销售量
        /// </summary>
        public long SoldCount { get; set; }

    }


}
