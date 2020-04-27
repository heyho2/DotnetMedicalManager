using GD.Common.Base;
using GD.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.User
{
    /// <summary>
    /// 组织架构列表 请求
    /// </summary>
    public class GetOrganizationTreeRequestDto : BaseDto, IBaseOrderBy
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
    /// 组织架构列表 响应
    /// </summary>
    public class GetOrganizationTreeDto : BaseTreeDto<GetOrganizationTreeDto>
    {
        ///<summary>
        ///组织id
        ///</summary>
        public string OrgGuid { get; set; }

        ///<summary>
        ///组织名称
        ///</summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 上级
        /// </summary>
        public string ParentGuid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }
    }
}
