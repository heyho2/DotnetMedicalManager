using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 申请创建报表
    /// </summary>
    public class CreateReportRequest
    {
        ///<summary>
        ///申请人姓名（给需求的人）
        ///</summary>
        [Display(Name = "申请人姓名")]
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 报表名称
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "主题名称")]
        public string ReportName { get; set; }

        /// <summary>
        /// 报表需求（包括：筛选条件，显示字段，需求）
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = " 报表需求（包括：筛选条件，显示字段，需求）")]
        public string Demand { get; set; }

        /// <summary>
        /// 条件信息列表
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "条件信息列表")]
        public List<CreateConditionInfo> CreateConditionInfoList { set; get; }

        /// <summary>
        /// 显示列信息列表
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "显示列信息列表")]
        public List<CreateColumnInfo> CreateColumnInfoList { set; get; }
        /// <summary>
        ///  根据需求写SQL语句， 格式：select @ColumnStr from table
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "SQL语句  格式：select @ColumnStr from table")]
        public string Sqlstr { get; set; }

        ///<summary>
        ///报表状态（0保存，1发布，2下架）
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "报表状态（0保存，1发布，2下架）")]
        public int RecordStatus { get; set; } = 0;

        ///<summary>
        ///平台类型
        ///</summary>
        [Display(Name = "平台类型")]
        public string PlatformType { get; set; } = "CloudDoctor";
        ///<summary>
        ///排序
        ///</summary>
        [Display(Name = "排序")]
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 条件信息
        /// </summary>
        public class CreateConditionInfo
        {
            /// <summary>
            /// 条件名
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "条件名")]
            public string ConditionName { get; set; }

            /// <summary>
            /// 字段
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "字段")]
            public string FieldCode { get; set; }

            /// <summary>
            /// 值类型（默认字符串类型）
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "值类型（默认字符串类型）")]
            public string ValueType { get; set; } = "String";

            /// <summary>
            /// 取值范围（暂定）
            /// </summary>
            [Display(Name = "取值范围")]
            public string ValueRange { get; set; } = "{}";
            ///<summary>
            ///是否必填
            ///</summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "是否必填")]
            public bool Required { get; set; } = false;
            /// <summary>
            /// 排序
            /// </summary>
            [Display(Name = "排序")]
            public int Sort { get; set; }
        }

        /// <summary>
        /// 显示列信息列表
        /// </summary>
        public class CreateColumnInfo
        {
            /// <summary>
            /// 中文列名
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "中文列名")]
            public string ColumnName { get; set; }

            /// <summary>
            /// 字段
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "字段")]
            public string FieldCode { get; set; }

            /// <summary>
            /// 值类型（默认字符串类型）
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "值类型（默认字符串类型）")]
            public string ValueType { get; set; } = "String";

            /// <summary>
            /// 取值范围（暂定）
            /// </summary>
            [Display(Name = "取值范围")]
            public string ValueRange { get; set; } = "{}";
            ///<summary>
            ///是否必填
            ///</summary>
            [Display(Name = "是否必填")]
            public bool Required { get; set; } = false;
            /// <summary>
            /// 排序
            /// </summary>
            [Display(Name = "排序")]
            public int Sort { get; set; }
        }


    }
}
