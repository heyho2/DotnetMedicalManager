using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 获取问答动态
    /// </summary>
    public class GetFAQsTrendsResponseDto : BaseDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 提问用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 解答时间
        /// </summary>
        public DateTime CreationDate { get; set; }


    }
}
