using GD.Common.Base;
using System;

namespace GD.Dtos.Doctor.Score
{
    /// <summary>
    /// 医生积分返回DTO
    /// </summary>
    public class GetDoctorScoreResponseDto : BaseDto
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 积分收入
        /// </summary>
        public int IncomeVariation { get; set; }

        /// <summary>
        /// 积分支出
        /// </summary>
        public int OutlayVariation { get; set; }

        /// <summary>
        /// 积分变化
        /// </summary>
        public int Variation { get; set; }

        /// <summary>
        /// 积分变化原因
        /// </summary>
        public string Reason { get; set; }
    }

    /// <summary>
    /// 积分页面DTO
    /// </summary>
    public class ScorePageDto<T> : BasePageResponseDto<T> where T : BaseDto
    {

    }
}