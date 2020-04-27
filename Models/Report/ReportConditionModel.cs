using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Report
{
    ///<summary>
    /// 问题模块-回答表实体
    ///</summary>
    [Table("t_report_condition")]
    public class ReportConditionModel : BaseModel
    {
        ///<summary>
        ///主键
        ///</summary>
        [Column("condition_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "条件主键")]
        public string ConditionGuid { get; set; }
        ///<summary>
        ///需求主键
        ///</summary>
        [Column("theme_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "需求主键")]
        public string ThemeGuid { get; set; }

        ///<summary>
        ///条件名称
        ///</summary>
        [Column("name"), Required(ErrorMessage = "{0}必填"), Display(Name = "条件名称")]
        public string Name { get; set; }

        ///<summary>
        ///字段名
        ///</summary>
        [Column("field_code"), Required(ErrorMessage = "{0}必填"), Display(Name = "字段名")]
        public string FieldCode { get; set; }

        ///<summary>
        ///条件sql语句
        ///</summary>
        [Column("field_value_sql"), Display(Name = "条件sql语句")]
        public string FieldValueSql { get; set; }

        ///<summary>
        ///该条件sql是否可执行
        ///</summary>
        [Column("is_right_sql"), Required(ErrorMessage = "{0}必填"), Display(Name = "sql是否可执行")]
        public bool IsRightSql { get; set; }

        ///<summary>
        ///取值类型，默认为字符类型
        ///</summary>
        [Column("value_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "取值类型，默认为字符类型")]
        public string ValueType { get; set; }

        ///<summary>
        ///取值范围
        ///</summary>
        [Column("value_range"), Display(Name = "取值范围")]
        public string ValueRange { get; set; }

        ///<summary>
        ///扩展字段
        ///</summary>
        [Column("extension_field"), Display(Name = "扩展字段")]
        public string ExtensionField { get; set; }

        ///<summary>
        ///是否必填
        ///</summary>
        [Column("required"), Display(Name = "是否必填")]
        public bool Required { get; set; }
        
        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Display(Name = "排序")]
        public int Sort { get; set; }

        ///<summary>
        ///记录类型（Condition:条件，Column:列名）
        ///</summary>
        [Column("record_type"), Display(Name = "排序")]
        public string RecordType { get; set; }

        /// <summary>
        /// 记录枚举
        /// </summary>
        public enum RecordTypeEnum
        {
            /// <summary>
            /// 条件
            /// </summary>
            [Display(Name = "条件")]
            Condition,
            /// <summary>
            /// 列名
            /// </summary>
            [Display(Name = "列名")]
            Column

        }
    }
}
