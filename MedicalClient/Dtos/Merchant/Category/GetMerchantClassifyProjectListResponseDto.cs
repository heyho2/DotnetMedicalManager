using Newtonsoft.Json;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Category
{
    /// <summary>
    /// 获取指定商户平台分类下项目列表
    /// </summary>
    public class GetMerchantClassifyProjectListResponseDto
    {
        /// <summary>
        /// 
        /// 平台分类Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 平台分类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 服务项目列表
        /// </summary>
        public List<MerchantClassifyProjectItem> Items { get; set; }
    }

    /// <summary>
    /// 服务项目
    /// </summary>
    public class MerchantClassifyProjectItem
    {
        /// <summary>
        /// 所属分类guid
        /// </summary>
        [JsonIgnore]
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 服务项目Id
        /// </summary>
        public string PorjectId { get; set; }
        /// <summary>
        /// 服务项目名称
        /// </summary>
        public string PorjectName { get; set; }
    }
}
