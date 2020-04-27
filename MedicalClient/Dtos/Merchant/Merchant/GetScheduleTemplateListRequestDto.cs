using GD.Common.Base;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取排班周期数据
    /// </summary>
    public class GetScheduleTemplateListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 店铺guid
        /// </summary>
        public string MerchantGuid { get; set; }

    }
}
