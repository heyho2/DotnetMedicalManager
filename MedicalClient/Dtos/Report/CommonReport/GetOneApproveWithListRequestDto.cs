using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    ///  运营-获取单条申请以及数据列表
    /// </summary>
    public class GetOneApproveWithListRequestDto
    {
        /// <summary>
        /// 审批记录Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "审批记录Guid")]
        public string ApproveGuid { get; set; }
    }


    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetOneApproveWithListResponseDto
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
        ///  需求
        /// </summary>
        public string ConditionDemand { get; set; }
        /// <summary>
        ///  SQL语句
        /// </summary>
        public string SQLStr { get; set; }
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
        /// 审批人Guid
        /// </summary>
        public string SQLWriterGuid { get; set; }
        /// <summary>
        ///  审批人名称
        /// </summary>
        public string SQLWriterName { get; set; }
        /// <summary>
        ///  申请时间
        /// </summary>
        public string CreationDate { get; set; }


    }
}
