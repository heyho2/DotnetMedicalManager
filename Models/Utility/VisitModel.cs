
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Utility
{
    /// <summary>
    /// 系统访问日志
    /// </summary>
    [Table("t_utility_visit")]
    public class VisitModel : BaseModel
    {

        /// <summary>
        /// 访问GUID
        /// </summary>
        [Column("visit_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "访问GUID")]
        public string VisitGuid { get; set; }

        /// <summary>
        /// 用户GUID
        /// </summary>
        [Column("user_guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// CONSUMER,DOCTOR,MERCHANT,ADMIN
        /// </summary>
        [Column("user_type")]
        public string UserType { get; set; }



    }
}



