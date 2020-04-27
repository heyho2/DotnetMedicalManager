
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
    /// 店铺工作班次Model
    ///</summary>
    [Table("t_merchant_work_shift")]
    public class MerchantWorkShiftModel : BaseModel
    {

        ///<summary>
        ///工作班次GUID
        ///</summary>
        [Column("work_shift_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "工作班次GUID")]
        public string WorkShiftGuid { get; set; }

        ///<summary>
        ///班次名称
        ///</summary>
        [Column("work_shift_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "班次名称")]
        public string WorkShiftName { get; set; }

        ///<summary>
        ///班次模板GUID
        ///</summary>
        [Column("template_guid"), Display(Name = "班次模板GUID")]
        public string TemplateGuid { get; set; }

        ///<summary>
        ///所属店铺GUID
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属店铺GUID")]
        public string MerchantGuid { get; set; }



    }
}



