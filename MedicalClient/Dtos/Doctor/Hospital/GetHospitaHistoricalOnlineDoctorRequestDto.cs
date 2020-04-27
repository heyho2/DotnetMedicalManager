using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 
    /// </summary>
    public class HospitalOnline
    {
        /// <summary>
        /// 
        /// </summary>
        public int OnlineNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalNumber { get; set; }
    }
    /// <summary>
    /// 历史统计请求
    /// </summary>
    public class GetHospitaHistoricalDoctorRequestDto
    {
        /// <summary>
        /// 医院guid（不用传）
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 0:昨日，1：最近七日，2：自定义
        /// </summary>
        [Required]
        public int Type { get; set; }
        /// <summary>
        /// 自定义开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 自定义结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetHospitaHistoricalOnlineDoctorResponseDto
    {
        /// <summary>
        /// 医生日均上线人数
        /// </summary>
        public int AvgOnline { get; set; }
        /// <summary>
        /// 医生上线线总人数
        /// </summary>
        public int OnlineTotal { get; set; }
        /// <summary>
        /// 用户日均咨询人数
        /// </summary>
        public int AvgConsult { get; set; }
        /// <summary>
        /// 用户总咨询人数
        /// </summary>
        public int ConsultTotal { get; set; }
        /// <summary>
        /// 医生上线比例
        /// </summary>
        public decimal OnlineRatio { get; set; }
    }
}
