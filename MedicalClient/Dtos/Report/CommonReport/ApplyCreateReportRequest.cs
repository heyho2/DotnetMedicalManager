using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 申请创建报表
    /// </summary>
    public class ApplyCreateReportRequest
    {

        /// <summary>
        /// 报表名称
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "主题名称")]
        public string Name { get; set; }

        /// <summary>
        /// 需求
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = " 需求")]
        public string Demand { get; set; }


        /// <summary>
        /// 筛选条件需求
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = " 筛选条件需求")]
        public string ConditionDemand { get; set; }

        /// <summary>
        /// 审批Guid()
        /// </summary>
        [Display(Name = "审批Guid (重新申请时-必填)")]
        public string ApproveGuid { get; set; }

        ///<summary>
        ///平台类型
        ///</summary>
        [Display(Name = "平台类型")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString();
    }
}
