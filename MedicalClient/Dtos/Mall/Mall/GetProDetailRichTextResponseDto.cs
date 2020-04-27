using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 商品说明书(富文本) 请求
    /// </summary>
    public class GetProDetailRichTextResponseDto : BaseDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 富文本Guid
        /// </summary>
        public string TextGuid { get; set; }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationDate { get; set; }

    }
}
