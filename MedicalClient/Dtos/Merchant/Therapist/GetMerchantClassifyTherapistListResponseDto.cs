using Newtonsoft.Json;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 获取指定商户分类下服务人员列表
    /// </summary>
    public class GetMerchantClassifyTherapistListResponseDto
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MerchantClassifyTherapistItem> Items { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>

    public class MerchantClassifyTherapistItem
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        [JsonIgnore]
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 服务人员Guid
        /// </summary>
        public string TherapistGuid { get; set; }
        /// <summary>
        /// 服务人员姓名
        /// </summary>
        public string TherapistName { get; set; }
    }
}
