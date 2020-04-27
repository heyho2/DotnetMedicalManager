using GD.Common.Base;
using GD.Dtos.Admin;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 获取美疗师列表
    /// </summary>
    public class GetTherapistListRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 店铺guid
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        ///分类
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; } = true;
    }
}
