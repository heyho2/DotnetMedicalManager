using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Help
{
    /// <summary>
    /// 常见问题列表
    /// </summary>
    /// 
    public class GetHelpPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 问题
        /// </summary>
        public string Question { get; set; }
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
    /// 常见问题列表
    /// </summary>
    public class GetHelpPageResponseDto : BasePageResponseDto<GetHelpPageItemDto>
    {

    }
    /// <summary>
    /// 常见问题列表
    /// </summary>
    public class GetHelpPageItemDto : BaseDto
    {

        ///<summary>
        ///问题GUID
        ///</summary>
        public string HelpGuid { get; set; }

        ///<summary>
        ///问题
        ///</summary>
        public string Question { get; set; }

        ///<summary>
        ///答案
        ///</summary>
        public string Answer { get; set; }

        /// <summary>
        /// 创建时间，默认为系统当前时间   
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 使能标志，默认为 true
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
