using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取问医经典问答详情
    /// </summary>
    public class GetAskedDoctorClassicQaDetailsResponseDto:BaseDto
    {
        /// <summary>
        /// 问答Guid
        /// </summary>
        public string QaGuid { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 医生Guid
        /// </summary>
        public string AuthorGuid { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 岗位职称
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 医生医院Guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 医生医院名称
        /// </summary>
        public string HospitalName { get; set; }


        /// <summary>
        /// 医生头像
        /// </summary>
        public string DoctorPortrait { get; set; }

        /// <summary>
        /// 历史提问数量
        /// </summary>
        public int HistoricalQuestionsCount { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int LikeNumber { get; set; }

        /// <summary>
        /// 是否已点赞
        /// </summary>
        public bool Liked { get; set; }
    }
}
