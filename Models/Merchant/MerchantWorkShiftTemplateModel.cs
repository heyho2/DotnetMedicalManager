using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Merchant
{
    ///<summary>
    ///班次模板model
    ///</summary>
    [Table("t_merchant_work_shift_template")]
    public class MerchantWorkShiftTemplateModel : BaseModel
    {

        ///<summary>
        ///班次模板GUID
        ///</summary>
        [Column("template_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "班次模板GUID")]
        public string TemplateGuid { get; set; }

        ///<summary>
        ///班次名称
        ///</summary>
        [Column("template_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "班次名称")]
        public string TemplateName { get; set; }

        ///<summary>
        ///商户GUID
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }
    }
}



