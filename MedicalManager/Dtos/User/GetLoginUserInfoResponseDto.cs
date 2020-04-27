using GD.Common.Base;
using GD.Dtos.Common;
using System.Collections.Generic;

namespace GD.Dtos.User
{
    /// <summary>
    /// 获取用户信息 响应
    /// </summary>
    public class GetLoginUserInfoResponseDto : BaseDto
    {
        ///<summary>
        ///GUID
        ///</summary>
        public string UserGuid { get; set; }
        ///<summary>
        ///用户昵称
        ///</summary>
        public string NickName { get; set; }
        ///<summary>
        ///手机号码
        ///</summary>
        public string Phone { get; set; }
        ///<summary>
        ///头像
        ///</summary>
        public string PortraitUrl { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuper { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public string[] Roles { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public IList<MenuItemDto> Menus { get; set; }

        /// <summary>
        /// 按钮
        /// </summary>
        public string[] Buttons { get; set; }
    }
    /// <summary>
    ///菜单信息
    /// </summary>
    public class MenuItemDto : BaseTreeDto<MenuItemDto>
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
        /// 按钮
        /// </summary>
        public string[] Buttons { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
    ///// <summary>
    ///// 按钮权限
    ///// </summary>
    //public class MenuButtonItemDto
    //{
    //    /// <summary>
    //    /// 编号
    //    /// </summary>
    //    public string Code { get; set; }
    //    public string Name { get; set; }

    //}
}
