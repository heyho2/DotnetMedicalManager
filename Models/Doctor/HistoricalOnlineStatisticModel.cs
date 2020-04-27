using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    ///<summary>
    ///每天医生在线统计
    ///</summary>
    [Table("t_doctor_historical_online_statistic")]
    public class HistoricalOnlineStatisticModel : BaseModel
    {

        ///<summary>
        ///
        ///</summary>
        [Column("online_statistic_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string OnlineStatisticGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("hospital_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///每天有效医生上线人数
        ///</summary>
        [Column("online_number"), Required(ErrorMessage = "{0}必填"), Display(Name = "每天有效医生上线人数")]
        public int OnlineNumber { get; set; }

        ///<summary>
        ///每天有效医生总人数
        ///</summary>
        [Column("total_number")]
        public int TotalNumber { get; set; }

    }
}



