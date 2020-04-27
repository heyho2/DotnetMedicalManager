using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 
    /// </summary>
    public class HosipitalLoginRequestDto
    {
        /// <summary>
        /// 医院账号
        /// </summary>
        [Required(ErrorMessage = "账号必填")]
        public string Account { get; set; }

        /// <summary>
        /// 医院密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        public string Password { get; set; }
    }
}
