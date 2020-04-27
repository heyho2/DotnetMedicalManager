using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 明星产品请求Dto
    /// </summary>
    public class GetStartProductListResponseDto
    {
        /// <summary>
        /// 分类Guid
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommend { get; set; }
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string TargetGuid { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// 产品URL
        /// </summary>
        public string TargetUrl { get; set; }


    }
}
