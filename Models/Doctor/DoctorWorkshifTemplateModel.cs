using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 医生班次模板表
    /// </summary>
    [Table("t_doctor_workshift_template")]
    public class DoctorWorkshifTemplateModel : BaseModel
    {
        /// <summary>
        /// 医生排班模板主键
        /// </summary>
        [Column("template_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "医生排班模板主键")]
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        [Column("template_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "模板名称")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 医院guid
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院guid")]
        public string HospitalGuid { get; set; }
    }
}