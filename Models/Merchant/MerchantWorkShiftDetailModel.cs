
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
    /// 店铺班次明细Model
    ///</summary>
    [Table("t_merchant_work_shift_detail")]
    public class MerchantWorkShiftDetailModel : BaseModel
    {

        ///<summary>
        ///工作班次明细GUID
        ///</summary>
        [Column("work_shift_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "工作班次明细GUID")]
        public string WorkShiftDetailGuid { get; set; }

        ///<summary>
        ///工作班次GUID
        ///</summary>
        [Column("work_shift_guid")]
        public string WorkShiftGuid { get; set; }

        ///<summary>
        ///开始时间
        ///</summary>
        [Column("start_time")]
        public string StartTime { get; set; }

        ///<summary>
        ///结束时间
        ///</summary>
        [Column("end_time")]
        public string EndTime { get; set; }



    }
}



