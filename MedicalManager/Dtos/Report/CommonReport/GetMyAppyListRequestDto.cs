using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 获取登陆人申请列表
    /// </summary>
    public class GetReportListAsyncRequestDto : BasePageRequestDto
    {
        ///<summary>
        ///申请人姓名
        ///</summary>
        [Display(Name = "申请人姓名")]
        public string ApplyUserName { get; set; }

        ///<summary>
        ///需求或名称关键字
        ///</summary>
        [Display(Name = "需求或名称关键字")]
        public string KeyWord { get; set; }

        ///<summary>
        ///开始时间
        ///</summary>
        [Display(Name = "开始时间")]
        public DateTime? StartTime { get; set; }

        ///<summary>
        ///结束时间
        ///</summary>
        [Display(Name = "结束时间")]
        public DateTime? EndTime { get; set; }

        ///<summary>
        ///报表状态（0保存，1发布，2下架）查全部为-1
        ///</summary>
        [Display(Name = "报表状态（0保存，1发布，2下架）")]
        public int RecordStatus { get; set; } = -1;

    }

    /// <summary>
    /// 相应Dto
    /// </summary>
    public class GetReportListAsyncResponseDto : BasePageResponseDto<GetReportListAsyncItemDto>
    {


    }
    /// <summary>
    /// 子项
    /// </summary>
    public class GetReportListAsyncItemDto : BaseDto
    {
        /// <summary>
        ///  报表主题Guid
        /// </summary>
        public string ThemeGuid { get; set; }
        ///<summary>
        ///申请人姓名
        ///</summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        ///  报表名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  需求
        /// </summary>
        public string Demand { get; set; }
        /// <summary>
        ///  sql语句
        /// </summary>
        public string SQLStr { get; set; }
        /// <summary>
        ///  报表状态（0默认，1暂存，2发布，3下架）
        /// </summary>
        public int RecordStatus { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        ///  申请时间
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        ///  申请时间
        /// </summary>
        public DateTime? CreationDate { get; set; }
        /// <summary>
        ///  申请时间
        /// </summary>
        public string LastUpdatedBy { get; set; }
        /// <summary>
        ///  申请时间
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }

    }
}
