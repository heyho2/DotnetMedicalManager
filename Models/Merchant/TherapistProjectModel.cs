using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Merchant
{
    ///<summary>
    /// 服务人员可做项目关系表
    ///</summary>
    [Table("t_merchant_therapist_project")]
    public class TherapistProjectModel : BaseModel
    {

        ///<summary>
        ///服务人员项目关系GUID
        ///</summary>
        [Column("therapist_project_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "服务人员项目关系GUID")]
        public string TherapistProjectGuid { get; set; }

        ///<summary>
        ///服务人员GUID
        ///</summary>
        [Column("therapist_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务人员GUID")]
        public string TherapistGuid { get; set; }

        ///<summary>
        ///服务项目GUID
        ///</summary>
        [Column("project_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务项目GUID")]
        public string ProjectGuid { get; set; }
    }
}



