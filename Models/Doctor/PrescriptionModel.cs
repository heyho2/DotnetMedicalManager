
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 处方主表
    /// </summary>
    [Table("t_doctor_prescription")]
    public class PrescriptionModel : BaseModel
    {
        /// <summary>
        /// 处方guid
        /// </summary>
        [Column("prescription_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "处方guid")]
        public string PrescriptionGuid { get; set; }

        /// <summary>
        /// 处方名
        /// </summary>
        [Column("prescription_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "处方名")]
        public string PrescriptionName { get; set; }

        /// <summary>
        /// 处方编号
        /// </summary>
        [Column("prescription_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "处方编号")]
        public string PrescriptionNo { get; set; }

        /// <summary>
        /// 处方前记guid
        /// </summary>
        [Column("information_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "处方前记guid")]
        public string InformationGuid { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        [Column("total_cost"), Required(ErrorMessage = "{0}必填"), Display(Name = "总费用")]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 处方状态：待付款、已付款、作废
        /// </summary>
        [Column("status")]
        public string Status { get; set; }

        /// <summary>
        /// 作废原因
        /// </summary>
        [Column("reason")]
        public string Reason { get; set; }
    }
}



