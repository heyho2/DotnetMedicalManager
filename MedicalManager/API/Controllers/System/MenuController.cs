using GD.API.Code;
using GD.Common;
using GD.Dtos.Common;
using GD.Dtos.Menu;
using GD.Manager.Manager;
using GD.Models.Manager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuController : SystemBaseController
    {
        /// <summary>
        /// 菜单树
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<IEnumerable<GetMenuTreeDto>>))]
        public async Task<IActionResult> GetMenuTreeAsync([FromBody]GetMenuListRequestDto request)
        {
            var menuBiz = new MenuBiz();
            var entityList = await menuBiz.GetListAsync();
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                entityList = entityList.Where(a => a.MenuName.Contains(request.Name));
            }
            entityList = entityList.OrderByDescending(a => a.Sort).ThenBy(a => a.CreatedBy);
            var response = entityList.GetTree(null, a => a.ParentGuid, a => a.MenuGuid, a => new GetMenuTreeDto
            {
                Enable = a.Enable,
                MenuGuid = a.MenuGuid,
                MenuName = a.MenuName,
                Sort = a.Sort,
                MenuClass = a.MenuClass,
                MenuCode = a.MenuCode,
                MenuUrl = a.MenuUrl,
                ParentGuid = a.ParentGuid,
            });
            return Success(response);
        }
        /// <summary>
        /// 菜单数，包括按钮
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<IEnumerable<GetMenuTreeDto>>))]
        public async Task<IActionResult> GetMenuTree2Async([FromBody]GetMenuListRequestDto request)
        {
            var menuBiz = new MenuBiz();
            var entityList = await menuBiz.GetListAsync();
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                entityList = entityList.Where(a => a.MenuName.Contains(request.Name));
            }
            entityList = entityList.OrderByDescending(a => a.Sort).ThenBy(a => a.CreatedBy);
            var buttonBiz = new ButtonBiz();
            var buttonList = await buttonBiz.GetListAsync(enable: true);
            entityList = entityList.Union(buttonList.Select(a => new MenuModel
            {
                Enable = true,
                MenuGuid = a.ButtonGuid,
                MenuName = a.ButtonName,
                Sort = a.Sort,
                MenuClass = null,
                MenuCode = a.ButtonCode,
                MenuUrl = null,
                ParentGuid = a.MenuGuid,
            }));

            var response = entityList.GetTree(null, a => a.ParentGuid, a => a.MenuGuid, a => new GetMenuTreeDto
            {
                Enable = a.Enable,
                MenuGuid = a.MenuGuid,
                MenuName = a.MenuName,
                Sort = a.Sort,
                MenuClass = a.MenuClass,
                MenuCode = a.MenuCode,
                MenuUrl = a.MenuUrl,
                ParentGuid = a.ParentGuid,
            });
            return Success(response);
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddMenuAsync([FromBody]AddMenuRequestDto request)
        {
            var menuBiz = new MenuBiz();
            var entity = await menuBiz.GetByCodeAsync(request.MenuCode);
            if (entity != null)
            {
                return Failed(ErrorCode.UserData, "code重复");
            }
            var result = await menuBiz.InsertAsync(new MenuModel
            {
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = request.Enable,
                MenuGuid = Guid.NewGuid().ToString("N"),
                MenuName = request.MenuName,
                OrgGuid = string.Empty,
                Sort = request.Sort,
                MenuClass = request.MenuClass,
                MenuCode = request.MenuCode,
                MenuUrl = request.MenuUrl,
                ParentGuid = request.ParentGuid,
            });
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateMenuAsync([FromBody]UpdateMenuRequestDto request)
        {
            var menuBiz = new MenuBiz();
            var entity = await menuBiz.GetAsync(request.MenuGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.MenuName = request.MenuName;
            entity.Sort = request.Sort;
            entity.Enable = request.Enable;
            entity.MenuClass = request.MenuClass;
            entity.MenuCode = request.MenuCode;
            entity.MenuUrl = request.MenuUrl;
            entity.ParentGuid = request.ParentGuid;
            var result = await menuBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用启用菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableMenuAsync([FromBody]DisableEnableRequestDto request)
        {
            var menuBiz = new MenuBiz();
            var entity = await menuBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await menuBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteMenuAsync([FromBody]DeleteRequestDto request)
        {
            var menuBiz = new MenuBiz();
            var result = await menuBiz.DeleteAsync(request.Guid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }
        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SaveRoleMenuAsync([FromBody]AddRoleMenuRequestDto request)
        {
            GrantRoleBiz grantRoleBiz = new GrantRoleBiz();
            var roleRights = await grantRoleBiz.GetRoleRightAsync(request.RoleGuid);
            //提交中数据中 数据库存在的
            var dbRoleRights = roleRights.Where(a => request.RightGuids.Contains(a));

            var addRoleRights = request.RightGuids.Except(dbRoleRights).ToArray();
            var deleteRoleRights = roleRights.Except(dbRoleRights).ToArray();

            var result = await grantRoleBiz.SaveRoleMenuAsync(addRoleRights.Select(a => new GrantRoleModel
            {
                RoleGuid = request.RoleGuid,
                RightGuid = a,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty,
                GrantGuid = Guid.NewGuid().ToString("N")
            }), request.RoleGuid, deleteRoleRights);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<string[]>))]
        public async Task<IActionResult> GetRoleMenuAsync([FromBody]GetRoleMenuRequestDto request)
        {
            GrantRoleBiz grantRoleBiz = new GrantRoleBiz();
            var roleRights = await grantRoleBiz.GetRoleRightAsync(request.RoleGuid);
            return Success(roleRights);
        }
    }
}
