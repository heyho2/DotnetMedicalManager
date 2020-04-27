using GD.Common.Base;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 下拉框
    /// </summary>
    public class SelectItemDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
    }
}
