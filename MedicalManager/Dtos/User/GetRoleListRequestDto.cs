using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.User
{
    /// <summary>
    /// 角色列表 请求
    /// </summary>
    public class GetRoleListRequestDto : BaseDto, IBaseOrderBy
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
    /// 角色列表 响应
    /// </summary>
    public class GetRoleListResponseDto : BaseDto
    {
        ///<summary>
        ///角色GUID
        ///</summary>
        public string RoleGuid { get; set; }

        ///<summary>
        ///角色名称
        ///</summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
		/// 创建人
		/// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间，默认为系统当前时间
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// 最后修改日期，默认为系统当前时间
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
    }
}
