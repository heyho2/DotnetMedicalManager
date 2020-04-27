using GD.Common.Base;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
	/// 获取班次模板分页列表请求Dto
	/// </summary>
	public class GetWorkShiftTemplatePageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid
        {
            get;
            set;
        }
    }
}
