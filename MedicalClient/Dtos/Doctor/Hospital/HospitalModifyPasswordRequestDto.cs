using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 
    /// </summary>
    public class HospitalModifyPasswordRequestDto
    {
        /// <summary>
        /// 医院原密码
        /// </summary>
        [Required(ErrorMessage = "原密码必填")]
        public string Password { get; set; }

        /// <summary>
        /// 医院新密码
        /// </summary>
        [Required(ErrorMessage = "新密码必填")]
        public string NewPassword { get; set; }
    }
}
