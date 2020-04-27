using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Utility
{
    /// <summary>
    /// 服务单号Model
    /// </summary>
    [Table("t_serviceNo")]
    public class ServiceNoModel
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Column("id"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "ID")]
        public string Id { get; set; }
    }
}
