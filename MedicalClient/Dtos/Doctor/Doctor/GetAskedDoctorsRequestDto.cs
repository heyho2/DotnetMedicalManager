using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取问医服务的医生列表请求Dto
    /// </summary>
    public class GetAskedDoctorsRequestDto: BasePageRequestDto
    {
        /// <summary>
        /// 医院Id
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 擅长疾病名称
        /// </summary>
        public string AdeptName { get; set; }
    }
}
