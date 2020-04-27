using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Headline
{
    /// <summary>
    /// 头条列表 请求
    /// </summary>
    public class GetHeadlinePageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public bool? Enable { get; set; }
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
    /// 头条列表 响应
    /// </summary>
    public class GetHeadlinePageResponseDto : BasePageResponseDto<GetHeadlinePageItemDto>
    {

    }
    /// <summary>
    /// 头条列表 项
    /// </summary>
    public class GetHeadlinePageItemDto : BaseDto
    {
        ///<summary>
        ///头条GUID
        ///</summary>
        public string HeadlineGuid { get; set; }

        ///<summary>
        ///头条名称
        ///</summary>
        public string HeadlineName { get; set; }

        ///<summary>
        ///头条简介
        ///</summary>
        public string HeadlineAbstract { get; set; }

        ///<summary>
        ///点击响应目标
        ///</summary>
        public string Target { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        ///<summary>
        ///是否启用
        ///</summary>
        public bool Enable { get; set; }
        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime CreationDate { get; set; }
    }
}
