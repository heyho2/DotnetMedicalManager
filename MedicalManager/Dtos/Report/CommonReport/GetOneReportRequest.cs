using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 查看或编辑报表
    /// </summary>
    public class GetOneReportRequest
    {
        ///<summary>
        ///报表主键
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "报表主键")]
        public string ThemeGuid { get; set; }

    }
}
