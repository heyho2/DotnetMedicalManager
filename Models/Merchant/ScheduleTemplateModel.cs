
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
    ///周期排班模板关系表
    ///</summary>
    [Table("t_merchant_schedule_template")]
    public class ScheduleTemplateModel : BaseModel
    {
        ///<summary>
        ///周期排班GUID
        ///</summary>
        [Column("schedule_template_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "周期排班GUID")]
        public string ScheduleTemplateGuid { get; set; }

        ///<summary>
        ///商户GUID
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///起始日期
        ///</summary>
        [Column("start_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "起始日期")]
        public DateTime StartDate { get; set; }

        ///<summary>
        ///结束日期
        ///</summary>
        [Column("end_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "结束日期")]
        public DateTime EndDate { get; set; }

        ///<summary>
        ///班次模板GUID
        ///</summary>
        [Column("template_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "班次模板GUID")]
        public string TemplateGuid { get; set; }

    }
}



