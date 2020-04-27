using GD.Common.Base;

namespace GD.Dtos.Doctor.Score
{
    /// <summary>
    /// 获取医生积分所有信息
    /// </summary>
    public class GetDoctorScoreAllInfoResponseDto : BasePageRequestDto
    {
        /// <summary>
        /// 所有积分
        /// </summary>
        public int AllVariation { get; set; }

        /// <summary>
        /// 积分收入
        /// </summary>
        public int IncomeALLVariation { get; set; }

        /// <summary>
        /// 积分支出
        /// </summary>
        public int OutlayALLVariation { get; set; }
    }
}
