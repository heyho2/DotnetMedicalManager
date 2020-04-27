using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取问医经典问答列表响应Dto
    /// </summary>
    public class GetAskedDoctorClassicQaResponseDto : BaseDto
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
        /// 医生头像
        /// </summary>
        public string DoctorPortrait { get; set; }

        /// <summary>
        /// 历史提问数量
        /// </summary>
        public int HistoricalQuestionsCount { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        public int LikeNumber { get; set; }



    }
}
