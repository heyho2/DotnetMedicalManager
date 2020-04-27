using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 预览结果
    /// </summary>
    public class PreviewResultInSqlStrRequest : BasePageRequestDto
    {
        ///<summary>
        ///报表主键
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "报表主键")]
        public string ThemeGuid { get; set; }
        /// <summary>
        /// 条件list
        /// </summary>
        public List<PreviewConditionInfo> PreviewConditionInfoList { get; set; }

        /// <summary>
        /// 条件info
        /// </summary>
        public class PreviewConditionInfo
        {
            ///<summary>
            ///字段
            ///</summary>
            [Display(Name = "字段")]
            public string FieldCode { get; set; }
            /// <summary>
            /// 值类型
            /// </summary>
            public string ValueType { get; set; }
            ///<summary>
            ///值
            ///</summary>
            [Display(Name = "值")]
            public string FieldValue { get; set; }
            ///<summary>
            ///是否必填
            ///</summary>
            [Required(ErrorMessage = "{0}不能为空"), Display(Name = "是否必填")]
            public bool Required { get; set; } = false;
            ///<summary>
            ///@columnStr
            ///</summary>
            [Display(Name = "字段=值")]
            public string FieldCodeValueString { get; set; }
        }
    }
}
