using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 美疗师解锁排班锁定时间
    /// </summary>
    public class UnLockScheduleDetailTimesRequestDto : BaseDto
    {
        /// <summary>
        /// 排班guid
        /// </summary>
        [Required(ErrorMessage ="排班guid必填")]
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 待解锁的排班明细id集合
        /// </summary>
        [Required(ErrorMessage = "待解锁的排班明细id集合必填")]
        public List<string> ScheduleDetailGuids { get; set; }
    }
}
