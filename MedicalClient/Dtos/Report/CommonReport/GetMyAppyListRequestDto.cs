using GD.Common.Base;
using GD.Dtos.Consumer.Consumer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 获取登陆人申请列表
    /// </summary>
    public class GetMyAppyListRequestDto : PageRequestDto
    {
        /// <summary>
        ///  申请人Guid
        /// </summary>
        [Display(Name = "申请人Guid")]
        public string UserID { get; set; }
    }

    /// <summary>
    /// 相应Dto
    /// </summary>
    public class GetMyAppyListResponseDto : BasePageResponseDto<GetMyAppyListItemDto>
    {


    }
    /// <summary>
    /// 子项
    /// </summary>
    public class GetMyAppyListItemDto : BaseDto
    {
        /// <summary>
        ///  报表主题Guid
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
        /// SQL撰写人
        /// </summary>
        public string SQLWriterGuid { get; set; }
        /// <summary>
        ///  SQL撰写人名称
        /// </summary>
        public string SQLWriterName { get; set; }

        /// <summary>
        /// SQL审批人Guid
        /// </summary>
        public string SQLApproverGuid { get; set; }
        /// <summary>
        /// SQL审批人名称
        /// </summary>
        public string SQLApproverName { get; set; }

        /// <summary>
        /// 列表审核人Guid
        /// </summary>
        public string ListApproverGuid { get; set; }
        /// <summary>
        /// 列表审核人名称
        /// </summary>
        public string ListApproverName { get; set; }

        /// <summary>
        ///  审批原因
        /// </summary>
        public string ApprovedReason { get; set; }

        /// <summary>
        ///  审批时间
        /// </summary>
        public DateTime? ApprovedDatetime { get; set; }
        /// <summary>
        ///  审批进度
        /// </summary>
        [Display(Name = "审批进度")]
        public string ApproveScheduleEnum { get; set; }

        /// <summary>
        ///  审批状态
        /// </summary>
        [Display(Name = "审批状态")]
        public string ApproveStatus { get; set; }

        /// <summary>
        ///  申请时间
        /// </summary>
        public string CreationDate { get; set; }
    }
}
