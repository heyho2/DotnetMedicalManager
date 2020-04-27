
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Merchant
{
    ///<summary>
    /// 店铺排班明细Model
    ///</summary>
    [Table("t_merchant_schedule_detail")]
    public class MerchantScheduleDetailModel : BaseModel
    {

        ///<summary>
        ///排班明细GUID
        ///</summary>
        [Column("schedule_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "排班明细GUID")]
        public string ScheduleDetailGuid { get; set; }

        ///<summary>
        ///排班GUID
        ///</summary>
        [Column("schedule_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "排班GUID")]
        public string ScheduleGuid { get; set; }

        ///<summary>
        ///开始时间
        ///</summary>
        [Column("start_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "开始时间")]
        public string StartTime { get; set; }

        ///<summary>
        ///结束时间
        ///</summary>
        [Column("end_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "结束时间")]
        public string EndTime { get; set; }

        ///<summary>
        ///消费GUID
        ///</summary>
        [Column("consumption_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "消费GUID")]
        public string ConsumptionGuid { get; set; }

        ///<summary>
        ///项目间隔锁定排班明细guid
        ///</summary>
        [Column("lock_detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "项目间隔锁定排班明细guid")]
        public string LockDetailGuid { get; set; }

    }
}



