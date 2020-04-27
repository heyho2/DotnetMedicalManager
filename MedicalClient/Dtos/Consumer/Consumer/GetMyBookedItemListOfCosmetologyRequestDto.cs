using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 生美--消费者获取预约列表请求Dto
    /// </summary>
    public class GetMyBookedItemListOfCosmetologyRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 用户guid(选填，默认为登录用户guid)
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 消费预约状态
        /// </summary>
        public ConsumptionStatusEnum? ConsumptionStatus { get; set; }
    }
}
