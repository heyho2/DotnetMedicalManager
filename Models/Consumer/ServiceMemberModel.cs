using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 服务对象表实体
    /// </summary>
    [Table("t_consumer_service_member")]
    public class ServiceMemberModel : BaseModel
    {
        /// <summary>
        /// 服务对象主键
        /// </summary>
        [Column("service_member_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "服务对象主键")]
        public string ServiceMemberGuid { get; set; }

        /// <summary>
        /// 所属用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属用户guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("name"), Required(ErrorMessage = "{0}必填"), Display(Name = "姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Column("sex"), Required(ErrorMessage = "{0}必填"), Display(Name = "性别")]
        public string Sex { get; set; }

        /// <summary>
        /// 年龄——岁
        /// </summary>
        [Column("age_year"), Required(ErrorMessage = "{0}必填"), Display(Name = "年龄——岁")]
        public int AgeYear { get; set; }

        /// <summary>
        /// 年龄——月
        /// </summary>
        [Column("age_month"), Required(ErrorMessage = "{0}必填"), Display(Name = "年龄——月")]
        public int AgeMonth { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column("phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "手机号")]
        public string Phone { get; set; }
    }
}



