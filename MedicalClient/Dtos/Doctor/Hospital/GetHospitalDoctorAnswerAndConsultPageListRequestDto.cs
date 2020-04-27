using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 
    /// </summary>
    public class HospitalDocorDbBasicData : DoctorAnswerConsultItemDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string DoctorGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RightTimes { get; set; }
    }

    /// <summary>
    /// 医院医生解答、咨询列表请求
    /// </summary>
    public class GetHospitalDoctorAnswerAndConsultPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院Guid(不需要传)
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 医生名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 医生手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 统计开始时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "开始日期")]
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 统计结束日期
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "结束日期")]
        public DateTime EndDate { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class GetHospitalDoctorAnswerAndConsultPageListResponseDto
    {
        /// <summary>
        /// 当前页数据
        /// </summary>
        public IEnumerable<DoctorAnswerConsultItemDto> CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total
        {
            get;
            set;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class DoctorAnswerConsultItemDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string PresenceIcon { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 医生手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 解答问题数
        /// </summary>
        public int AnswerQuestionNumber { get; set; }
        /// <summary>
        /// 被采纳率
        /// </summary>
        public decimal AdopedRate { get; set; }
        /// <summary>
        /// 咨询量
        /// </summary>
        public int ConsultNumber { get; set; }
        /// <summary>
        /// 在线时长
        /// </summary>
        public decimal Duration { get; set; }
        /// <summary>
        /// 扣分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
    }
}
