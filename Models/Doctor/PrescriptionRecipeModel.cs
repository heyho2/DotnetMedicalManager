
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
    /// 处方取药表
    /// </summary>
    [Table("t_doctor_prescription_recipe")]
    public class PrescriptionRecipeModel : BaseModel
    {
        /// <summary>
        /// 处方取药guid
        /// </summary>
        [Column("recipe_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "处方取药guid")]
        public string RecipeGuid { get; set; }

        /// <summary>
        /// 处方guid
        /// </summary>
        [Column("prescription_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "处方guid")]
        public string PrescriptionGuid { get; set; }

        /// <summary>
        /// 收费项目类型:Drug-药品收费,Treatment-治疗收费
        /// </summary>
        [Column("item_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "收费项目类型:Drug-药品收费,Treatment-治疗收费")]
        public string ItemType { get; set; }

        /// <summary>
        /// 收费项目名称
        /// </summary>
        [Column("item_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "收费项目名称")]
        public string ItemName { get; set; }

        /// <summary>
        /// 收费项目规格
        /// </summary>
        [Column("item_specification"), Required(ErrorMessage = "{0}必填"), Display(Name = "收费项目规格")]
        public string ItemSpecification { get; set; }

        /// <summary>
        /// 收费项目单价
        /// </summary>
        [Column("item_price"), Required(ErrorMessage = "{0}必填"), Display(Name = "收费项目单价")]
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// 收费项目数量
        /// </summary>
        [Column("item_quantity"), Required(ErrorMessage = "{0}必填"), Display(Name = "收费项目数量")]
        public int ItemQuantity { get; set; }

        /// <summary>
        /// 药品用法
        /// </summary>
        [Column("drug_usage")]
        public string DrugUsage { get; set; }

        /// <summary>
        /// 药品用量
        /// </summary>
        [Column("drug_dosage")]
        public string DrugDosage { get; set; }

        /// <summary>
        /// 用药频度数量：例如 “每6小时1次”中的“6”
        /// </summary>
        [Column("drug_frequency_quantity")]
        public decimal? DrugFrequencyQuantity { get; set; }

        /// <summary>
        /// 用药频度单位：例如 “每6小时1次”中的“小时”
        /// </summary>
        [Column("drug_frequency_unit")]
        public string DrugFrequencyUnit { get; set; }

        /// <summary>
        /// 用药频度次数：例如 “每6小时1次”中的“1”
        /// </summary>
        [Column("drug_frequency_times")]
        public decimal? DrugFrequencyTimes { get; set; }
    }
}



