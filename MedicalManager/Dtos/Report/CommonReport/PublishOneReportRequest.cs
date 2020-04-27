using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 发布
    /// </summary>
    public class PublishOneReportRequest
    {
        ///<summary>
        ///报表主键
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "报表主键")]
        public string ThemeGuid { get; set; }

        /////<summary>
        /////变更报表状态（0保存，1发布，2下架）
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "报表状态（0保存，1发布，2下架）")]
        //public int RecordStatus { get; set; } = 0;
    }
}
