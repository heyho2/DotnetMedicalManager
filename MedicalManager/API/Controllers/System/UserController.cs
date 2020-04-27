using GD.API.Code;
using GD.Common;
using GD.Common.Helper;
using GD.Dtos.Common;
using GD.Dtos.User;
using GD.Dtos.WeChat;
using GD.Manager.Doctor;
using GD.Manager.Manager;
using GD.Manager.Payment;
using GD.Manager.Utility;
using GD.Manager.WeChat;
using GD.Models.Manager;
using GD.Models.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static GD.Dtos.WeChat.CreateQRCodeRequestDto;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : SystemBaseController
    {
        #region 账号
        /// <summary>
        /// 获取当前登陆人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetLoginUserInfoResponseDto>))]
        public async Task<IActionResult> GetLoginUserInfoAsync()
        {
            var account = await new ManagerAccountBiz().GetAsync(UserID);
            if (account == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            var accessory = await new AccessoryBiz().GetAsync(account?.PortraitGuid);
            var response = account.ToDto<GetLoginUserInfoResponseDto>();
            response.PortraitUrl = $"{accessory?.BasePath}{accessory?.RelativePath}";

            response.Roles = (await new AccountRoleBiz().GetListAsync(UserID)).ToArray();
            var roleRights = await new GrantRoleBiz().GetRoleRightAsync(response.Roles);
            //response.Menus = .Select(a => a.ToDto<MenuItemDto>()).ToArray();
            var menuBiz = new MenuBiz();
            IEnumerable<MenuModel> menus;
            if (account.IsSuper)
            {
                menus = await menuBiz.GetListAsync();
            }
            else
            {
                menus = await menuBiz.GetByGuidsAsync(roleRights.ToArray());
            }
            var buttonBiz = new ButtonBiz();
            IEnumerable<ButtonModel> buttons;
            if (account.IsSuper)
            {
                buttons = await buttonBiz.GetListAsync();
            }
            else
            {
                buttons = await buttonBiz.GetByGuidsAsync(roleRights.ToArray());
            }
            menus = menus.OrderByDescending(a => a.Sort).ThenBy(a => a.CreatedBy);
            response.Menus = menus.GetTree(null, a => a.ParentGuid, a => a.MenuGuid, a => new MenuItemDto
            {
                MenuGuid = a.MenuGuid,
                MenuName = a.MenuName,
                Sort = a.Sort,
                MenuClass = a.MenuClass,
                MenuCode = a.MenuCode,
                MenuUrl = a.MenuUrl,
                ParentGuid = a.ParentGuid,
                Enable = a.Enable,
                Buttons = buttons.Where(s => s.MenuGuid == a.MenuGuid).Select(s => s.ButtonCode).ToArray()
            });

            return Success(response);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetAccountInfoResponseDto>))]
        public async Task<IActionResult> GetAccountInfoAsync([FromBody]GetAccountInfoRequestDto request)
        {
            var account = await new ManagerAccountBiz().GetAsync(request.UserID);
            if (account == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            var accessory = await new AccessoryBiz().GetAsync(account?.PortraitGuid);

            var response = account.ToDto<GetAccountInfoResponseDto>();
            var accountRoleBiz = new AccountRoleBiz();
            //var roles = await accountRoleBiz.GetListAsync(request.UserID);
            var organization = await new OrganizationBiz().GetAsync(account.OrganizationGuid);
            response.Roles = (await accountRoleBiz.GetListAsync(request.UserID)).ToArray();
            response.PortraitUrl = $"{accessory?.BasePath}{accessory?.RelativePath}";
            response.OrganizationName = organization?.OrgName;

            return Success(response);
        }
        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetAccountListResponseDto>))]
        public async Task<IActionResult> GetAccountListAsync([FromBody]GetGetAccountListRequestDto request)
        {
            var response = await new ManagerAccountBiz().GetAccountListAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 添加账号
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddAccountAsync([FromBody]AddAccountRequestDto request)
        {
            var managerAccountBiz = new ManagerAccountBiz();
            var accounts = await managerAccountBiz.GetModelsAsync(request.Account, request.Phone);
            if (accounts.Count() > 0)
            {
                return Failed(ErrorCode.UserData, "账号存在或者手机号已存在");
            }
            string userGuid = Guid.NewGuid().ToString("N");
            var accountModel = new AccountModel
            {
                Account = request.Account,
                UserGuid = userGuid,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = request.Enable,
                IsSuper = request.IsSuper,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrganizationGuid = request.OrganizationGuid,
                OrgGuid = string.Empty,
                Password = CryptoHelper.AddSalt(userGuid, request.Password),
                Birthday = request.Birthday,
                Email = request.Email,
                Gender = request.Gender,
                NickName = request.UserName,
                Phone = request.Phone,
                PortraitGuid = request.PortraitGuid,
                UserName = request.UserName,
                WechatOpenid = request.WechatOpenid
            };
            List<AccountRoleModel> accountRoleModels = new List<AccountRoleModel>();
            foreach (var item in request.Roles)
            {
                accountRoleModels.Add(new AccountRoleModel
                {
                    Arguid = Guid.NewGuid().ToString("N"),
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    Enable = request.Enable,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty,
                    LastUpdatedDate = DateTime.Now,
                    RoleGuid = item,
                    UserGuid = userGuid,
                });
            }
            if (!await new ManagerAccountBiz().AddAsync(accountModel, accountRoleModels))
            {
                return Failed(ErrorCode.UserData, "添加失败");
            }
            return Success();

        }
        /// <summary>
        /// 修改账号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateAccountAsync([FromBody]UpdateAccountRequestDto request)
        {
            var managerAccountBiz = new ManagerAccountBiz();
            var accountModel = await managerAccountBiz.GetAsync(request.UserGuid);
            if (accountModel == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            var accounts = await managerAccountBiz.GetModelsAsync(request.UserGuid, request.Account, request.Phone);
            if (accounts.Count() > 0)
            {
                return Failed(ErrorCode.UserData, "账号存在或者手机号已经存在");
            }

            accountModel.Account = request.Account;
            accountModel.Enable = request.Enable;
            accountModel.IsSuper = request.IsSuper;
            accountModel.LastUpdatedBy = UserID;
            accountModel.LastUpdatedDate = DateTime.Now;
            accountModel.OrganizationGuid = request.OrganizationGuid;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                accountModel.Password = CryptoHelper.AddSalt(request.UserGuid, request.Password);
            }
            accountModel.Birthday = request.Birthday;
            accountModel.Email = request.Email;
            accountModel.Gender = request.Gender;
            accountModel.NickName = request.UserName;
            accountModel.Phone = request.Phone;
            accountModel.PortraitGuid = request.PortraitGuid;
            accountModel.UserName = request.UserName;
            accountModel.WechatOpenid = request.WechatOpenid;

            var accountRoleModels = new List<AccountRoleModel>();
            foreach (var item in request.Roles)
            {
                accountRoleModels.Add(new AccountRoleModel
                {
                    Arguid = Guid.NewGuid().ToString("N"),
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    Enable = request.Enable,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty,
                    LastUpdatedDate = DateTime.Now,
                    RoleGuid = item,
                    UserGuid = request.UserGuid,
                });
            }
            if (!await new ManagerAccountBiz().UpdateAsync(accountModel, accountRoleModels))
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();

        }
        /// <summary>
        /// 禁用账号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableAccountAsync([FromBody]DisableEnableAccountRequestDto request)
        {
            var managerAccountBiz = new ManagerAccountBiz();
            var entity = await managerAccountBiz.GetAsync(request.UserGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await managerAccountBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateAccountPasswordAsync([FromBody]UpdateAccountPasswordRequestDto request)
        {
            var managerAccountBiz = new ManagerAccountBiz();
            var entity = await managerAccountBiz.GetAsync(UserID);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            if (entity.Password != CryptoHelper.AddSalt(entity.UserGuid, request.OldPassword))
            {
                return Failed(ErrorCode.UserData, "旧密码输入错误");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Password = CryptoHelper.AddSalt(entity.UserGuid, request.Password);
            var result = await managerAccountBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        #endregion

        #region 组织架构

        /// <summary>
        /// 组织架构列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetOrganizationTreeDto>>))]
        public async Task<IActionResult> GetOrganizationTreeAsync([FromBody]GetOrganizationTreeRequestDto request)
        {
            var organizationBiz = new OrganizationBiz();
            var modelList = await organizationBiz.GetAllAsync(request);
            var response = GetOrganizationTree(null, modelList);
            return Success(response);
        }
        /// <summary>
        /// 获取组织架构树
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        private List<GetOrganizationTreeDto> GetOrganizationTree(string pid, IEnumerable<OrganizationModel> models)
        {
            IEnumerable<OrganizationModel> organizations;
            if (pid == null)
            {
                organizations = models.Where(a => a.ParentGuid == pid || !models.Any(b => b.OrgGuid == a.ParentGuid));
            }
            else
            {
                organizations = models.Where(a => a.ParentGuid == pid);
            }
            var tree = organizations.Select(a => new GetOrganizationTreeDto
            {
                Children = GetOrganizationTree(a.OrgGuid, models).ToList(),
                OrgName = a.OrgName,
                OrgGuid = a.OrgGuid,
                Sort = a.Sort,
                Enable = a.Enable,
                ParentGuid = a.ParentGuid,
                CreationDate = a.CreationDate
            }).ToList();
            return tree;
        }

        /// <summary>
        /// 添加组织架构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddOrganizationAsync([FromBody]AddOrganizationRequestDto request)
        {
            var organizationBiz = new OrganizationBiz();
            var result = await organizationBiz.InsertAsync(new OrganizationModel
            {
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = request.Enable,
                OrgGuid = Guid.NewGuid().ToString("N"),
                OrgName = request.OrgName,
                ParentGuid = string.IsNullOrWhiteSpace(request.ParentGuid) ? null : request.ParentGuid,
                Sort = request.Sort,
            });
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改组织架构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateOrganizationAsync([FromBody] UpdateOrganizationRequestDto request)
        {
            var organizationBiz = new OrganizationBiz();
            var entity = await organizationBiz.GetAsync(request.OrgGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.Enable = request.Enable;
            entity.LastUpdatedDate = DateTime.Now;
            entity.OrgName = request.OrgName;
            entity.ParentGuid = string.IsNullOrWhiteSpace(request.ParentGuid) ? null : request.ParentGuid;
            entity.Sort = request.Sort;
            var result = await organizationBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用组织架构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableOrganizationAsync([FromBody]DisableEnableOrganizationRequestDto request)
        {
            var organizationBiz = new OrganizationBiz();
            var entity = await organizationBiz.GetAsync(request.OrgGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            var oall = await organizationBiz.GetListAsync();
            var entitys = GetOrganizationSubset(oall, request.OrgGuid);
            entitys.Add(entity);
            foreach (var item in entitys)
            {
                item.LastUpdatedBy = UserID;
                item.LastUpdatedDate = DateTime.Now;
                item.Enable = request.Enable;
            }
            var result = await organizationBiz.UpdateAsync(entitys);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 获取子集
        /// </summary>
        /// <returns></returns>
        private List<OrganizationModel> GetOrganizationSubset(IEnumerable<OrganizationModel> organizations, string pid)
        {
            List<OrganizationModel> torganizations = new List<OrganizationModel>();
            var os = organizations.Where(a => a.ParentGuid == pid);
            torganizations.AddRange(os);
            foreach (var item in os)
            {
                torganizations.AddRange(GetOrganizationSubset(organizations, item.OrgGuid));
            }
            return torganizations;
        }
        /// <summary>
        /// 删除组织架构
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteOrganizationAsync([FromBody]DeleteOrganizationRequestDto request)
        {
            var organizationBiz = new OrganizationBiz();
            var result = await organizationBiz.DeleteAsync(request.OrgGuid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }

        #endregion

        #region 角色

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRoleListResponseDto>))]
        public async Task<IActionResult> GetRoleListAsync([FromBody]GetRoleListRequestDto request)
        {
            var roleBiz = new RoleBiz();
            var entityList = await roleBiz.GetAllAsync(request);
            var response = entityList?.Select(a => a.ToDto<GetRoleListResponseDto>());
            return Success(response);
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddRoleAsync([FromBody]AddRoleRequestDto request)
        {
            var roleBiz = new RoleBiz();
            var result = await roleBiz.InsertAsync(new RoleModel
            {
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = request.Enable,
                RoleGuid = Guid.NewGuid().ToString("N"),
                Description = request.Description,
                RoleName = request.RoleName,
                OrgGuid = string.Empty,
                Sort = request.Sort
            });
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateRoleAsync([FromBody]UpdateRoleRequestDto request)
        {
            var roleBiz = new RoleBiz();
            var entity = await roleBiz.GetAsync(request.RoleGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.RoleName = request.RoleName;
            entity.Description = request.Description;
            entity.Sort = request.Sort;
            entity.Enable = request.Enable;
            var result = await roleBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用启用角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableRoleAsync([FromBody]DisableEnableRoleRequestDto request)
        {
            var roleBiz = new RoleBiz();
            var entity = await roleBiz.GetAsync(request.RoleGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await roleBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteRoleAsync([FromBody]DeleteRequestDto request)
        {
            var roleBiz = new RoleBiz();
            var result = await roleBiz.DeleteAsync(request.Guid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }

        /// <summary>
        /// ceshi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto)), AllowAnonymous]
        public IActionResult MAJIA()
        {

            var sql = string.Empty;
            var num = 10000000000;

            var aaa = "abcdefghijklmnopqrstuvwxyz";
            int[] s = new int[5];


            for (int i = 0; i < 20; i++)
            {
                var ra = new Random();

                var name = string.Empty;
                for (int k = 0; k < s.Length; k++)
                {
                    name += aaa.Substring(ra.Next(0, aaa.Length), 1);
                }

                var userGuid = Guid.NewGuid().ToString("N");
                var phone = num++;
                var password = CryptoHelper.AddSalt(userGuid, CryptoHelper.Md5("123456"));

                sql += $"INSERT INTO `t_utility_user` VALUES ('{userGuid}', NULL, NULL, '{name}', '{name}', '{password}', '{phone}', 'M', '2000-01-01 00:00:00', NULL, NULL, '{userGuid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{userGuid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', 'guodan', 1);";

                sql += $"INSERT INTO `t_consumer` VALUES ('{userGuid}', NULL, 0, NULL, NULL, NULL, 1, '{userGuid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{userGuid}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', NULL, 1);";
            }

            return Success<string>(sql);
        }
        #endregion

        /// <summary>
        /// 获取云医医院公众号二维码
        /// </summary>
        /// <param name="hospiltalGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<CreateQRCodeResponseDto>))]
        public async Task<IActionResult> GetUserWeChatQRCodeAsync(string hospiltalGuid)
        {
            if (string.IsNullOrEmpty(hospiltalGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var hosiptalBiz = new HospitalBiz();
            var hospital = await hosiptalBiz.GetAsync(hospiltalGuid);
            if (hospital is null)
            {
                return Failed(ErrorCode.Empty, "对应医院不存在，请检查参数");
            }

            if (!hospital.Enable)
            {
                return Failed(ErrorCode.Empty, $"医院“{hospital.HosName}”已被禁用");
            }

            var tokenResult = await WeChartApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret);

            if (tokenResult.Errcode != 0)
            {
                Logger.Error($"GD.API.Controllers.System.{nameof(UserController)}.{nameof(GetUserWeChatQRCodeAsync)}({hospiltalGuid})-- 获取云医用户端token失败  {Environment.NewLine} {tokenResult.Errmsg}");
                return Failed(ErrorCode.SystemException, "获取云医用户端token失败");
            }

            var qrcodeResult = (CreateQRCodeResponseDto)null;

            var sceneBiz = new WechatSceneBiz();

            var scene = await sceneBiz.GetAsync(hospiltalGuid);

            if (scene is null)
            {
                var param = new CreateTemporaryQRCodeRequestDto
                {
                    ActionName = ActionNameEnum.QR_LIMIT_STR_SCENE.ToString(),
                    ActionInfo = new QRCodeActionInfo
                    {
                        Scene = new QRCodeActionInfoStringScene
                        {
                            SceneStr = hospiltalGuid
                        }
                    }
                };

                qrcodeResult = await WeChartApi.CreateQRCodeAsync(param, tokenResult.AccessToken);

                scene = new WechatSceneModel()
                {
                    SceneId = hospiltalGuid,
                    Action = WeChatSceneActionEnum.pay.ToString(),
                    SceneName = "扫码缴费",
                    Extension = JsonConvert.SerializeObject(new
                    {
                        value = hospiltalGuid,
                        url = qrcodeResult.Url,
                        name = HttpUtility.UrlEncode(hospital.HosName)
                    }),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID
                };

                var result = await sceneBiz.InsertAsync(scene);

                if (!result) { return Failed(ErrorCode.Empty, "生成二维码场景参数失败，请稍后重试"); }
            }
            else
            {
                dynamic extension = JObject.Parse(scene.Extension);
                qrcodeResult = new CreateQRCodeResponseDto()
                {
                    Errcode = 0,
                    Url = (string)extension.url
                };
            }

            return Success(qrcodeResult);
        }
    }
}
