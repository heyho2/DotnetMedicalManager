namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 
    /// </summary>
    public class GetHospitaCurrentlOnlineDoctorResponseDto
    {
        /// <summary>
        /// 医生在线人数
        /// </summary>
        public int Online { get; set; }
        /// <summary>
        /// 用户咨询人数
        /// </summary>
        public int Consult { get; set; }
        /// <summary>
        /// 在线比例
        /// </summary>
        public decimal OnlineRatio { get; set; }
    }
}
