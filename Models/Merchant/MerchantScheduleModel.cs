
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
    /// 店铺排班model
    ///</summary>
    [Table("t_merchant_schedule")]
    public class MerchantScheduleModel : BaseModel
    {

        ///<summary>
        ///排班GUID
        ///</summary>
        [Column("schedule_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "排班GUID")]
        public string ScheduleGuid { get; set; }

        ///<summary>
        ///排班日期
        ///</summary>
        [Column("schedule_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "排班日期")]
        public DateTime ScheduleDate { get; set; }

        ///<summary>
        ///店铺guid
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "排班日期")]
        public string MerchantGuid { get; set; } 

        ///<summary>
        ///被排班人：美疗师GUID/其他
        ///</summary>
        [Column("target_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "被排班人：美疗师GUID/其他")]
        public string TargetGuid { get; set; }

        ///<summary>
        ///被排班人类型：美疗师/其他
        ///</summary>
        [Column("target_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "被排班人类型：美疗师/其他")]
        public string TargetType { get; set; }

        ///<summary>
        ///周期排班GUID
        ///</summary>
        [Column("schedule_template_guid"), Display(Name = "周期排班GUID")]
        public string ScheduleTemplateGuid { get; set; }

        ///<summary>
        ///班次GUID
        ///</summary>
        [Column("work_shift_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "班次GUID")]
        public string WorkShiftGuid { get; set; }

        ///<summary>
        ///是否约满
        ///</summary>
        [Column("full_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否约满")]
        public bool FullStatus { get; set; } = false;

        ///<summary>
        ///是否确认
        ///</summary>
        [Column("confirm_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "美疗师是否确认")]
        public bool ConfirmStatus { get; set; } = true;

        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString();

    }
}



