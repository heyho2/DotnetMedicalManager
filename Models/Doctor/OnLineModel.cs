
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    ///<summary>
    ///医生上下线统计
    ///</summary>
    [Table("t_doctor_online")]
    public class OnLineModel : BaseModel
    {

        ///<summary>
        ///
        ///</summary>
        [Column("online_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string OnlineGuid { get; set; }

        ///<summary>
        ///医生GUID
        ///</summary>
        [Column("doctor_guid")]
        public string DoctorGuid { get; set; }

        ///<summary>
        ///上线
        ///</summary>
        [Column("login_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "0：下线，1：上线")]
        public DateTime LoginTime { get; set; }

        ///<summary>
        ///下线
        ///</summary>
        [Column("logout_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "0：下线，1：上线")]
        public DateTime LogoutTime { get; set; }

        /// <summary>
        /// 在线时长
        /// </summary>
        [Column("duration"), Required(ErrorMessage = "{0}必填"), Display(Name = "在线时长")]
        public decimal Duration { get; set; }
    }
}



