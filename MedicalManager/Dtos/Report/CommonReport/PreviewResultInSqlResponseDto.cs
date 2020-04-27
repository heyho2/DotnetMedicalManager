using GD.Common.Base;
using System.Collections.Generic;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 运营审核数据报表
    /// </summary>
    public class PreviewResultInSqlResponseDto : BaseDto
    {
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<dynamic> CurrentPage { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int? Total { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportConditionItemDto : BaseDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段编码
        /// </summary>
        public string FieldCode { get; set; }
        /// <summary>
        /// 必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string ValueType { get; set; }
    }
}
