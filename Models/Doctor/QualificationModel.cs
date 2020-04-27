using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 医院资质表
    /// </summary>
    [Table("t_doctor_hsopital_qualification")]
    public class QualificationModel : BaseModel
    {
        ///<summary>
        ///特征GUID
        ///</summary>
        [Column("qualification_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "资质GUID")]
        public string CharacterGuid
        {
            get;
            set;
        }

        ///<summary>
        ///医院GUID
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid
        {
            get;
            set;
        }

        ///<summary>
        ///配置guid
        ///</summary>
        [Column("conf_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "配置GUID")]
        public string ConfGuid
        {
            get;
            set;
        }

        ///<summary>
        ///配置对应的值
        ///</summary>
        [Column("conf_value"), Required(ErrorMessage = "{0}必填"), Display(Name = "配置对应的值")]
        public string ConfValue
        {
            get;
            set;
        }
    }
}
