using GD.Common.Base;

namespace GD.Dtos.Recommend
{
    /// <summary>
    /// 删除推荐详细 请求
    /// </summary>
    public class DeleteRecommendDetailRequestDto : BaseDto
    {
        /// <summary>
        /// 推荐归属id
        /// </summary>
        public string[] DetailGuids { get; set; }
    }
}
