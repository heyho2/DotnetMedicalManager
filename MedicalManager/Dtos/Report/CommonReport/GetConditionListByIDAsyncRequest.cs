using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 获取条件/列名list 
    /// </summary>
    public class GetConditionOrColumnListRequest
    {
        ///<summary>
        ///报表主键
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "报表主键")]
        public string ThemeGuid { get; set; }
        ///<summary>
        ///查询类型 条件:Condition  列名:Column
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "查询类型")]
        public string RecordType { get; set; } = "Condition";
    }

    /// <summary>
    /// response
    /// </summary>
    public class GetConditionOrColumnListResponseDto
    {
        /// <summary>
        /// 条件Guid 或 列名Guid
        /// </summary>
        public string ConditionOrColumnGuid { get; set; }
        ///<summary>
        ///报表主键
        ///</summary>
        public string ThemeGuid { get; set; }
        /// <summary>
        /// 条件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        public string FieldCode { get; set; }

        /// <summary>
        /// 值类型（默认字符串类型）
        /// </summary>
        public string ValueType { get; set; } 

        /// <summary>
        /// 取值范围（暂定）
        /// </summary>
        public string ValueRange { get; set; }
        /// <summary>
        /// 记录类型（Condition:条件，Column:列名）
        /// </summary>
        public string RecordType { get; set; }
        ///<summary>
        ///是否必填
        ///</summary>
        public bool Required { get; set; } 
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

    }


}
