using GD.Common.Base;
using GD.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Menu
{
    /// <summary>
    /// 菜单列表 请求
    /// </summary>
    public class GetMenuListRequestDto : BaseDto, IBaseOrderBy
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
    /// 菜单列表 项
    /// </summary>
    public class GetMenuTreeDto : BaseTreeDto<GetMenuTreeDto>
    {
        ///<summary>
        ///ID
        ///</summary>
        public string MenuGuid { get; set; }

        ///<summary>
        ///父菜单GUID
        ///</summary>
        public string ParentGuid { get; set; }

        ///<summary>
        ///图标名称
        ///</summary>
        public string MenuClass { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string MenuName { get; set; }

        ///<summary>
        ///菜单URL（VUE路由地址）
        ///</summary>
        public string MenuUrl { get; set; }

        ///<summary>
        ///编码
        ///</summary>
        public string MenuCode { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }
}
