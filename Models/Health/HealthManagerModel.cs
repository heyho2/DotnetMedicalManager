using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Health
{
    /// <summary>
    /// 健康管理师表
    /// </summary>
    [Table("t_health_manager")]
    public class HealthManagerModel : BaseModel
    {

        /// <summary>
        /// 主键id
        /// </summary>
        [Column("manager_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string ManagerGuid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("user_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Column("job_number"), Required(ErrorMessage = "{0}必填"), Display(Name = "工号")]
        public string JobNumber { get; set; }

        /// <summary>
        /// 性别（M/F），默认为M
        /// </summary>
        [Column("gender"), Required(ErrorMessage = "{0}必填"), Display(Name = "性别（M/F），默认为M")]
        public string Gender { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Column("phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "手机号码")]
        public string Phone { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Column("identity_number"), Required(ErrorMessage = "{0}必填"), Display(Name = "身份证号")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 所在省份
        /// </summary>
        [Column("province"), Required(ErrorMessage = "{0}必填"), Display(Name = "所在省份")]
        public string Province { get; set; }

        /// <summary>
        /// 工作城市
        /// </summary>
        [Column("city"), Required(ErrorMessage = "{0}必填"), Display(Name = "工作城市")]
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [Column("district"), Required(ErrorMessage = "{0}必填"), Display(Name = "区")]
        public string District { get; set; }

        /// <summary>
        /// 1:一级健康管理师（FirstLevel），2：二级健康管理师(SecondLevel)，3：三级健康管理师(ThirdLevel)
        /// </summary>
        [Column("occupation_grade"), Required(ErrorMessage = "{0}必填"), Display(Name = "1:一级健康管理师（FirstLevel），2：二级健康管理师(SecondLevel)，3：三级健康管理师(ThirdLevel)")]
        public string OccupationGrade { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Column("portrait_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "头像")]
        public string PortraitGuid { get; set; }

        /// <summary>
        /// 职业资格证书
        /// </summary>
        [Column("qualification_certificate_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "职业资格证书")]
        public string QualificationCertificateGuid { get; set; }

        /// <summary>
        /// 企业微信用户id
        /// </summary>
        [Column("enterprise_user_id"),  Display(Name = "企业微信用户id")]
        public string EnterpriseUserId { get; set; }
    }
}



