using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Report
{
    ///<summary>
    /// 报表需求
    ///</summary>
    [Table("t_report_theme")]
    public class ReportThemeModel : BaseModel
    {
        ///<summary>
        ///报表需求
        ///</summary>
        [Column("theme_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "报表需求")]
        public string ThemeGuid { get; set; }

        ///<summary>
        ///申请人姓名
        ///</summary>
        [Column("apply_user_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "申请人姓名")]
        public string ApplyUserName { get; set; }
        ///<summary>
        ///报表名称
        ///</summary>
        [Column("name"), Required(ErrorMessage = "{0}必填"), Display(Name = "报表名称")]
        public string Name { get; set; }

        ///<summary>
        ///报表需求
        ///</summary>
        [Column("demand"), Required(ErrorMessage = "{0}必填"), Display(Name = "报表需求")]
        public string Demand { get; set; }

        ///<summary>
        ///sql语句
        ///</summary>
        [Column("sqlstr"), Display(Name = "sql语句")]
        public string SQLStr { get; set; }

        ///<summary>
        ///报表状态（0默认，1暂存，2发布，3下架）
        ///</summary>
        [Column("record_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "报表状态（0默认，1暂存，2发布，3下架）")]
        public int RecordStatus { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Display(Name = "排序")]
        public int Sort { get; set; } = 0;

        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type"), Display(Name = "平台类型")]
        public string PlatformType { get; set; }
    }
}
