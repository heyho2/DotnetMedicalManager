using GD.Common.Base;

namespace GD.Dtos.Doctor
{
    /// <summary>
    /// 重置密码 请求
    /// </summary>
    public class ResetPasswordResponseDto : BaseDto
    {
        /// <summary>
        /// 医生gudi
        /// </summary>
        public string DoctorGuid { get; set; }
    }
}
