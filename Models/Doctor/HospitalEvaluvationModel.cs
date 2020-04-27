using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_doctor_hospital_evaluation")]
    public class HospitalEvaluationModel : BaseModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Column("evaluation_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string EvaluationGuid { get; set; }

        /// <summary>
        /// 用户GUID
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 医院GUID
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 科室GUID
        /// </summary>
        [Column("office_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "科室GUID")]
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 评价标签
        /// </summary>
        [Column("evaluation_tag"), Required(ErrorMessage = "{0}必填"), Display(Name = "评价标签")]
        public string EvaluationTag { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        [Column("score"), Required(ErrorMessage = "{0}必填"), Display(Name = "评分")]
        public decimal Score { get; set; }

        /// <summary>
        /// 病情详情
        /// </summary>
        [Column("condition_detail"), Required(ErrorMessage = "{0}必填"), Display(Name = "病情详情")]
        public string ConditionDetail { get; set; }

        /// <summary>
        /// 是否匿名评论
        /// </summary>
        [Column("anonymous")]
        public bool Anonymous { get; set; }
    }
}



