using GD.Common.Base;

namespace GD.Dtos.Mall.Groupbuy
{
    /// <summary>
    /// 首页团购 请求
    /// </summary>
    public class GetHomeGroupbuyRequestDto : BaseDto
    {
        /// <summary>
        /// 取多少条记录
        /// </summary>
        public int Take { get; set; } = 7;
    }
    /// <summary>
    /// 首页团购 请求
    /// </summary>
    public class GetHomeGroupbuyItemDto : BaseDto
    {

    }
}
