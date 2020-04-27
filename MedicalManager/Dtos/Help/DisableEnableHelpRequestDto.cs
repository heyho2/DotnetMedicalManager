using GD.Common.Base;

namespace GD.Dtos.Question
{
    /// <summary>
    /// 禁用Question
    /// </summary>
    public class DisableEnableHelpRequestDto : BaseDto
    {
        /// <summary>
        /// 禁用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string Guid { get; set; }
    }
}
