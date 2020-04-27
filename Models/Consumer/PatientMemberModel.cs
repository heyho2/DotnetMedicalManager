
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 就诊(问诊)档案表
    /// </summary>
    [Table("t_consumer_patient_member")]
    public class PatientMemberModel : BaseModel
    {

        /// <summary>
        /// 就诊人guid
        /// </summary>
        [Column("patient_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "就诊人guid")]
        public string PatientGuid { get; set; }

        /// <summary>
        /// 用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 问诊人与当前用户关系
        /// Own:自己，Relatives:亲属，Friend:朋友,Other:其它
        /// </summary>
        [Column("relationship")]
        public string Relationship { get; set; }

        /// <summary>
        /// 就诊人姓名
        /// </summary>
        [Column("name"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊人姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 就诊人电话
        /// </summary>
        [Column("phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊人电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 性别（M/F），默认为M
        /// </summary>
        [Column("gender")]
        public string Gender { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        [Column("cardno")]
        public string CardNo { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 是否默认就诊人
        /// </summary>
        [Column("is_default")]
        public bool IsDefault { get; set; }



    }
}



