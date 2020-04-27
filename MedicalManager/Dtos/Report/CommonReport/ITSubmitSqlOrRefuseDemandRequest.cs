using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 
    /// </summary>
    public class ITSubmitSqlOrRefuseDemandRequest
    {
        /// <summary>
        /// 审批记录Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "审批记录Guid")]
        public string ApproveGuid { get; set; }

        /// <summary>
        ///  根据需求写SQL语句，SQL语句返回固定UserName,Phone等字段
        ///  举例： select username, phone，age，sex from table
        /// </summary>
        [Display(Name = "SQL语句")]
        public string Sqlstr { get; set; }
        /// <summary>
        /// 条件信息列表
        /// </summary>
        public List<ConditionInfo> ConditionInfoList { set; get; }
        /// <summary>
        ///  审批原因
        /// </summary>
        [Display(Name = "原因")]
        public string ApprovedReason { get; set; }
        /// <summary>
        ///  审批状态（驳回/通过）
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "审批状态（驳回/通过）")]
        public string ApproveStatus { get; set; }

        /// <summary>
        /// 条件信息
        /// </summary>
        public class ConditionInfo
        {
            /// <summary>
            /// 条件名
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "条件名")]
            public string Name { get; set; }

            /// <summary>
            /// 字段
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "字段")]
            public string FieldCode { get; set; }

            /// <summary>
            /// 可执行sql，也是下拉列表的值
            /// </summary>
            [Display(Name = "条件SQL语句")]
            public string FieldValueSql { get; set; }
            /// <summary>
            /// 是否可执行的SQL
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "是否可执行的SQL")]
            public bool IsRightSql { get; set; }

            /// <summary>
            /// 值类型（默认字符串类型）
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "是否可执行的SQL")]
            public string ValueType { get; set; } = "String";

            /// <summary>
            /// 取值范围（暂定）
            /// </summary>
            [Display(Name = "取值范围")]
            public string ValueRange { get; set; }

            /// <summary>
            /// 排序
            /// </summary>
            [Display(Name = "排序")]
            public int Sort { get; set; }

        }
    }
}
