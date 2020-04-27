using GD.Common;
using GD.Consumer;
using GD.Doctor;
using GD.Dtos.Utility.Utility;
using GD.Manager;
using GD.Models.Consumer;
using GD.Models.Manager;
using GD.Models.Utility;
using GD.Module;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <inheritdoc />
    /// <summary>
    /// 我的页面-相关
    /// </summary>
    public class UtilityController : UtilityBaseController
    {
        /// <summary>
        /// 进入首页,调用该接口(头像/昵称)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetUserPortraitResponseDto>))]
        public IActionResult LoadHomePage(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = UserID;
            }
            var uBiz = new UserBiz();
            var uModel = uBiz.GetUser(userId);
            if (uModel == null || !uModel.Enable) return Failed(ErrorCode.DataBaseError, "用户状态不可用。");
            var accBiz = new AccessoryBiz();
            var accModel = accBiz.GetAccessoryModelByGuid(uModel.PortraitGuid);
            var respData = new GetUserPortraitResponseDto()
            {
                Portrait = $"{accModel?.BasePath}{accModel?.RelativePath}",
                NickeName = uModel.NickName
            };
            return Success(respData);
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetUserInfoResponseDto>))]
        public IActionResult GetUserInfo(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = UserID;
            }
            var uBiz = new UserBiz();
            var uModel = uBiz.GetUser(userId);
            if (uModel == null || !uModel.Enable) return Failed(ErrorCode.DataBaseError, "用户状态不可用。");
            var accBiz = new AccessoryBiz();
            var accModel = accBiz.GetAccessoryModelByGuid(uModel.PortraitGuid);
            var outDto = new GetUserInfoResponseDto
            {
                Portrait = $"{accModel?.BasePath}{accModel?.RelativePath}", // +"/" 格式确认,
                NickName = uModel.NickName,
                Gender = uModel.Gender,
                Birthday = uModel.Birthday,
                UserName = uModel.UserName,
                IdentityNumber = uModel.IdentityNumber,
                Phone = uModel.Phone
            };
            return Success(outDto);
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="userPortraitDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult EditPortrait([FromBody]EditPortraitRequestDto userPortraitDto)
        {
            var userBiz = new UserBiz();
            var crrUserModel = userBiz.GetUser(UserID);
            crrUserModel.PortraitGuid = userPortraitDto.PortraitGuid;
            var isSuccess = userBiz.UpdateUser(crrUserModel);
            return isSuccess ? Success() : Failed(ErrorCode.DataBaseError, "更新失败！");
        }
        /// <summary>
        ///根据ID获取用户生活习惯列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetUserHabbitInfoResponseDto>>))]
        public IActionResult GetUserInfoList(string userId = "")
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = UserID;
            }
            var chaBiz = new CharacterBiz();
            var dicBiz = new DictionaryBiz();
            //var modelx = dicBiz.GetModelById("0ed7181b01d211e9b2da00e04c01c721");
            //modelx.ValueRange = "{\"1\":\"内向\",\"2\":\"外向\",\"3\":\"富有创造性\"}";
            //var ssss = dicBiz.Update(modelx);
            var chaModelList = chaBiz.GetCharacterModels(userId);//用户所有的列值
            chaModelList = chaModelList ?? new List<CharacterModel>();
            var dicModel = dicBiz.GetModelById(DictionaryType.UserPersonalInfo);//约定的GUID指定某类型的数据
            if (dicModel == null) return Failed(ErrorCode.FormatError, "查询不到约定ID,请联系管理员！");
            var dicModelList = dicBiz.GetListByParentGuid(dicModel.DicGuid);//个人资料所有的值
            var chaDtoList = from dml in dicModelList
                             join cml in chaModelList
                                 on dml.DicGuid equals cml.ConfGuid into temp
                             from tt in temp.DefaultIfEmpty()
                             select new GetUserHabbitInfoResponseDto
                             {
                                 ConfGuid = dml?.DicGuid,
                                 CharacterGuid = dml?.DicGuid,
                                 ValueType = dml?.ValueType,
                                 ConfName = dml?.ConfigName,
                                 Sort = dml?.Sort,
                                 ValueRange = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(dml?.ValueRange)),
                                 ConfValue = tt?.ConfValue,
                             };
            return Success(chaDtoList.OrderByDescending(a => a.Sort));
        }

        /// <summary>
        /// 生活习惯添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddUserInfoList([FromBody]AddUserInfoRequestDto userInfoDtoList)
        {
            var charsList = new List<CharacterModel>();
            var charsBiz = new CharacterBiz();
            foreach (var kv in userInfoDtoList.UserInfoDtoList)
            {
                var isExist = charsBiz.IsExistConfig(kv.Guid, UserID);
                if (isExist)
                {
                    var charMode = charsBiz.GetCharacterModelByConfigGuid(kv.Guid, UserID);
                    charMode.ConfValue = kv.Value;
                    var isUpdateSuccess = charsBiz.UpdateCharacterModel(charMode);//有记录的则更新
                    if (!isUpdateSuccess)
                    {
                        return Failed(ErrorCode.DataBaseError, "数据更新失败！");
                    }
                }
                else
                {
                    var newCharsGuid = Guid.NewGuid().ToString("N");
                    charsList.Add(new CharacterModel
                    {
                        CharacterGuid = newCharsGuid,
                        UserGuid = UserID,
                        ConfGuid = kv.Guid,
                        ConfValue = kv.Value,
                        CreatedBy = UserID,
                        OrgGuid = "",
                        LastUpdatedBy = UserID,
                        Enable = true
                    });
                }
            }
            if (charsList.Count < 1)
            {
                return Success();
            }
            var isSuccess = charsBiz.InsertCharacterModelList(charsList);//新增所有新纪录
            return isSuccess ? Success() : Failed(ErrorCode.DataBaseError, "数据提交失败！");
        }



        /// <summary>
        /// 更新别名
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async System.Threading.Tasks.Task<IActionResult> AddAliasAsync([FromBody]AddAliasRequestDto requestDto)
        {
            var biz = new AliasBiz();
            var res = await biz.InsertOrUpdateAsync(requestDto);
            return res ? Success() : Failed(ErrorCode.DataBaseError, "网络错误，更新别名失败");
        }

        /// <summary>
        /// 写入系统访问记录
        /// </summary>
        /// <param name="userType">用户类型：Consumer-消费者；Doctor-医生；</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> WriteSystemVisitLogAsync(Common.EnumDefine.UserType userType)
        {
            if (string.IsNullOrWhiteSpace(UserID))
            {
                return Success();
            }
            var model = new VisitModel
            {
                VisitGuid = Guid.NewGuid().ToString("N"),
                UserGuid = UserID,
                UserType = userType.ToString(),
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty
            };
            var result = await new VisitBiz().InsertAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "写入系统访问记录失败");

        }

        /// <summary>
        /// 获取在线客服信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOnlineServiceInfo(string doctorGuid)
        {
            if (string.IsNullOrWhiteSpace(doctorGuid))
            {
                return Success("", null);
            }
            var doctorModel = await new DoctorBiz().GetAsync(doctorGuid);
            var userModel = await new UserBiz().GetModelAsync(doctorGuid);
            var hospitalModel = await new HospitalBiz().GetAsync(doctorModel?.HospitalGuid);
            if (string.IsNullOrWhiteSpace(hospitalModel?.GuidanceUrl))
            {
                return Success(PlatformSettings.CDClientCustomerServiceLink, null);
            }
            var url = hospitalModel?.GuidanceUrl + System.Web.HttpUtility.UrlEncode($"({doctorModel?.OfficeName}{userModel?.UserName}医生)", Encoding.UTF8);
            return Success(url, null);
        }

        /// <summary>
        /// 获取平台客服Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPlatformServiceId()
        {
            return Success<string>(PlatformSettings.PlatformCustomerService);
        }

        /// <summary>
        /// 问医在线客服Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAskedDoctorOnlineCustomerServiceId()
        {
            return Success<string>(PlatformSettings.AskedDoctorOnlineCustomerService);
        }

        /// <summary>
        /// 获取平台客服电话
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetPlatformServiceTelAsync()
        {
            var model = await new DictionaryBiz().GetAsync(DictionaryType.PlatformServiceTel);
            return Success<string>(model?.ExtensionField);
        }
    }
}