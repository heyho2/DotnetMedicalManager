namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取医院医生在线时长排行榜
    /// </summary>
    public class GetHospitalDoctorOnlineRankResponseDto
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
        /// 在线时长
        /// </summary>
        public decimal Duration { get; set; }
    }
}
