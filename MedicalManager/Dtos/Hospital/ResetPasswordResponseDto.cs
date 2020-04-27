using GD.Common.Base;

namespace GD.Dtos.Hospital
{
    /// <summary>
    /// 重置密码 请求
    /// </summary>
    public class ResetPasswordResponseDto : BaseDto
    {
        /// <summary>
        /// gudi
        /// </summary>
        public string Guid { get; set; }
    }
}
