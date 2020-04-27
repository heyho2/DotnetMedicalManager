using GD.Common.EnumDefine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsIntergral
{
    /// <summary>
    /// 查询用户该月改到数据
    /// </summary>
    public class GetUserSignInDataCurrentMonthRequest
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        [Display(Name = "用户类型")]
        public UserType UserType { get; set; } = UserType.Consumer;
        /// <summary>
        /// 用户ID 
        /// </summary>
        [Display(Name = "用户ID")]
        public string UserID { get; set; } 
    }

    /// <summary>
    /// 响应
    /// </summary>
    public class GetUserSignInDataCurrentMonthResponse
    {
       
        /// <summary>
        /// 已连续签到天数
        /// </summary>
        public int ContinuousCheckInDays { get; set; }

        /// <summary>
        /// 下次签到可获取积分
        /// </summary>
        public int NextTimeCheckInIntergral { get; set; }

        /// <summary>
        /// 签到信息列表
        /// </summary>
        public List<CheckInInfo> CheckInInfoList { get; set; } 

        /// <summary>
        /// 签到信息
        /// </summary>
        public class CheckInInfo
        {
            /// <summary>
            /// 签到日期
            /// </summary>
            public DateTime CheckInDate { get; set; }

            /// <summary>
            /// 是否签到
            /// </summary>
            public bool IsCheckIn { get; set; }

            /// <summary>
            /// 赠送积分
            /// </summary>
            public int Intergral { get; set; }

        }
    }

   
}
