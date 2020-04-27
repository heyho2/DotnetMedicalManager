using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Merchant
{
    /// <summary>
    /// 服务人员所属分类
    /// </summary>
    [Table("t_merchant_therapist_classify")]
    public class MerchantTherapistClassifyModel : BaseModel
    {
        ///<summary>
        ///服务人员所属分类guid
        ///</summary>
        [Column("therapist_classify_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "服务人员所属分类guid")]
        public string TherapistClassifyGuid { get; set; }

        ///<summary>
        ///服务人员guid
        ///</summary>
        [Column("therapist_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "服务人员guid")]
        public string TherapistGuid { get; set; }

        ///<summary>
        ///所属分类guid
        ///</summary>
        [Column("classify_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属分类guid")]
        public string ClassifyGuid { get; set; }
    }
}
