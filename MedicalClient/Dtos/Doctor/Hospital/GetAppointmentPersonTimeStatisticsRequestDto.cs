using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 诊所挂号趋势统计请求Dto
    /// </summary>
    public class GetAppointmentPersonTimeStatisticsRequestDto : BaseDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        [Required(ErrorMessage = "起始日期必填")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Required(ErrorMessage = "结束日期必填")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override IEnumerable<ValidationResult> Verify(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (StartDate > EndDate)
            {
                result.Add(new ValidationResult("起始日期必须小于结束日期"));
            }
            else
            {
                var interval = (EndDate - StartDate).TotalDays;
                if (interval > 366)
                {
                    result.Add(new ValidationResult("查询区间不可大于366天"));
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAppointmentPersonTimeStatisticsResponseDto : BaseDto
    {

        /// <summary>
        /// 查询区间的连续日期
        /// </summary>
        public List<string> AppointmentDates { get; set; }

        /// <summary>
        /// 每日预约量统计
        /// </summary>
        public List<GetAppointmentPersonTimeStatisticsDto> StatisticsDatas { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetAppointmentPersonTimeStatisticsDto : BaseDto
    {
        /// <summary>
        /// 预约日期
        /// </summary>
        public string AppointmentDate { get; set; }

        /// <summary>
        /// 预约数量
        /// </summary>
        public int? AppointmentQuantity { get; set; }
    }
}
