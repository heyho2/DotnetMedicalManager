namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取医院医生用户咨询排行榜
    /// </summary>
    public class GetHospitalDoctorConsultRankResponseDto
    {
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户咨询次数
        /// </summary>
        public int Times { get; set; }
    }
}
