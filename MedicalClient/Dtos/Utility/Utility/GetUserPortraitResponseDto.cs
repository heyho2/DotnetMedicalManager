using GD.Common.Base;

namespace GD.Dtos.Utility.Utility
{
    /// <summary>
    /// 获取头像
    /// </summary>
    public class GetUserPortraitResponseDto : BaseDto
    {
        /// <summary>
        /// 用户头像URL
        /// </summary>
        public string Portrait { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string NickeName { get; set; }
       
    }
}