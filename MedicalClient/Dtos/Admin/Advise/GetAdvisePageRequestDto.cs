using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Advise
{
    /// <summary>
    /// 用户反馈
    /// </summary>
    public class GetAdvisePageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 注册时间 至
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 平台
        /// </summary>
        public string PlatformType { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }
    }
    /// <summary>
    /// 用户反馈
    /// </summary>
    public class GetAdvisePageResponseDto : BasePageResponseDto<GetAdvisePageItemDto>
    {

    }
    /// <summary>
    /// 用户反馈
    /// </summary>
    public class GetAdvisePageItemDto : BaseDto
    {
        ///<summary>
        ///建议GUID
        ///</summary>
        public string AdviseGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///建议人姓名
        ///</summary>
        public string Adviser { get; set; }

        ///<summary>
        ///建议人手机号
        ///</summary>
        public string AdviserPhone { get; set; }

        ///<summary>
        ///建议人EMAIL
        ///</summary>
        public string AdviserEmail { get; set; }

        ///<summary>
        ///建议内容
        ///</summary>
        public string AdviseContent { get; set; }

        ///<summary>
        ///平台类型
        ///</summary>
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        /// <summary>
        /// 创建时间，默认为系统当前时间   
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 使能标志，默认为 true
        /// </summary>
        public bool Enable { get; set; }
    }
}
