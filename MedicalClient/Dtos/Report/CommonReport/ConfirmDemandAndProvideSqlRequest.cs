using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfirmDemandAndProvideSqlRequest
    {
        /// <summary>
        /// 审批记录Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "审批记录Guid")]
        public string ApproveGuid { get; set; }
    }

    /// <summary>
    ///响应Dto
    /// </summary>
    public class ConfirmDemandAndProvideSqlResponse
    {
        /// <summary>
        ///  主题Guid
        /// </summary>
        public string ThemeGuid { get; set; }
        /// <summary>
        ///  主题Guid
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  需求
        /// </summary>
        public string Demand { get; set; }

        /// <summary>
        ///  条件需求
        /// </summary>
        public string ConditionDemand { get; set; }
        /// <summary>
        ///  SQL语句
        /// </summary>
        public string SqlStr { get; set; }
        /// <summary>
        ///  审批记录Guid
        /// </summary>
        public string ApproveGuid { get; set; }
        /// <summary>
        ///  申请人Guid
        /// </summary>
        public string ApplyUserGuid { get; set; }
        /// <summary>
        ///  申请人名称
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        ///  申请时间
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// 条件列表
        /// </summary>
        public List<ApproveConditionInfo> ApproveConditionInfoList { get; set; }


    }

    /// <summary>
    /// 条件信息。
    /// </summary>
    public class ApproveConditionInfo
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
