using GD.API.Code;
using GD.Common;
using GD.Common.EnumDefine;
using GD.Dtos.Utility.User;
using GD.Dtos.WeChat;
using GD.Manager;
using GD.Models.Manager;
using GD.Models.Payment;
using GD.Models.Utility;
using GD.Module;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GD.Dtos.WeChat.CreateQRCodeRequestDto;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController : UtilityBaseController
    {
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet, Obsolete("请使用: /Account/UpdatePassword")]
        public IActionResult UpdateUserPassword(string password)
        {
            //前端传输的密码为MD5加密后的结果，存入数据库的密码应该为 用户Guid+MD5密码 再进行一次MD5加密后的密码
            if (string.IsNullOrEmpty(password))
            {
                return Failed(ErrorCode.FormatError, "密码不可为空！");
            }

            UserBiz userBiz = new UserBiz();
            var userModel = userBiz.GetUser(UserID);
            userModel.Password = Common.Helper.CryptoHelper.AddSalt(UserID, password);
            return userBiz.UpdateUser(userModel) ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败！");
        }
        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="phone">用户手机号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Obsolete("请使用: /Account/UpdatePassword")]
        public IActionResult SetUserPassword(string phone, string password)
        {
            //前端传输的密码为MD5加密后的结果，存入数据库的密码应该为 用户Guid+MD5密码 再进行一次MD5加密后的密码
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                return Failed(ErrorCode.FormatError, "密码或手机号不可为空！");
            }
            UserBiz userBiz = new UserBiz();
            var userModel = userBiz.GetUserByPhone(phone);
            if (userModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到数据！");
            }
            userModel.Password = Common.Helper.CryptoHelper.AddSalt(userModel.UserGuid, password);
            return userBiz.UpdateUser(userModel) ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败！");
        }

        /// <summary>
        /// 用户更换手机号
        /// </summary>
        /// <param name="changeUserPhoneDto">用户手机号修改Dto</param>
        /// <returns></returns>
        [HttpPost, Obsolete("请使用: /Account/UpdatePhone")]
        public IActionResult ChangeUserPhone([FromBody]ChangeUserPhoneRequestDto changeUserPhoneDto)
        {
            AccountBiz accountBiz = new AccountBiz();
            if (!accountBiz.VerifyCode(changeUserPhoneDto.Phone, changeUserPhoneDto.VerifyCode))
            {
                return Failed(ErrorCode.VerificationCode, "手机验证码错误！");
            }
            UserBiz userBiz = new UserBiz();
            var userModel = userBiz.GetUser(UserID);
            userModel.Phone = changeUserPhoneDto.Phone;
            return userBiz.UpdateUser(userModel) ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败！");
        }
        /// <summary>
        /// 获取分页积分列表
        /// </summary>
        /// <param name="userScoreDto">含分页的积分传入Dto</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetUserScoreResponseDto>>))]
        public IActionResult GetUserScore([FromBody]GetUserScoreRequestDto userScoreDto)
        {
            var condition = "where user_guid=@user_guid and platform_type=@platformType and enable=@enable and user_type_guid=@user_type_guid " + (userScoreDto.ScoreType == "+" ? "and variation > 0" : (userScoreDto.ScoreType == "-" ? "and variation < 0" : ""));
            ScoreExBiz userBiz = new ScoreExBiz();
            var scores = userBiz.GetScores(userScoreDto.PageNumber, userScoreDto.PageSize, condition, "creation_date desc",
                new { user_guid = UserID, enable = true, user_type_guid = UserType.Consumer.ToString() /*userScoreDto.UserTypeGuid*/, platformType = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString() });
            var response = scores.Select(a => a.ToDto<GetUserScoreResponseDto>()).ToList();
            return Success(response);
        }
        /// <summary>
        /// 获取我的总积分（智慧云医）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetMyTotalScoreAsync()
        {
            var total = await new ScoreExBiz().GetTotalScore(UserID);
            return Success(total);
        }

        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetUserTypeResponseDto>>)), AllowAnonymous]
        public IActionResult GetUserType()
        {
            var dictionaryBiz = new DictionaryBiz();
            var dicModel = dictionaryBiz.GetModelById(DictionaryType.UserTypeConfig);
            if (dicModel == null)
            {
                return Failed(ErrorCode.Empty, "约定ID无效，请联系管理员！");
            }

            var userTypes = dictionaryBiz.GetListByParentGuid(dicModel.DicGuid).OrderBy(a => a.Sort).Select(item => item.ToDto<GetUserTypeResponseDto>());
            return Success(userTypes);
        }

        /// <summary>
        /// 修改用户基础信息
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult UpdateUserInfo([FromBody]UpdateUserInfoRequestDto userDto)
        {
            UserBiz userBiz = new UserBiz();
            var model = userBiz.GetUser(UserID);
            model.NickName = userDto.NickName;
            model.UserName = userDto.UserName;
            model.IdentityNumber = userDto.IdentityNumber;
            model.Birthday = userDto.Birthday;
            model.Gender = userDto.Gender;
            return userBiz.UpdateUser(model) ? Success() : Failed(ErrorCode.DataBaseError, "修改用户信息失败");
        }

        /// <summary>
        /// 判断是否签到
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<bool>))]
        public IActionResult CheckSignIn(UserType userType = UserType.Consumer)
        {
            var scoreBiz = new ScoreExBiz();
            var result = scoreBiz.CheckSignInScores(UserID, userType.ToString());
            return Success(result);
        }
        /// <summary>
        /// 签到积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult GetScoreOfSignIn(UserType userType = UserType.Consumer)
        {
            var scoreBiz = new ScoreExBiz();
            if (scoreBiz.CheckSignInScores(UserID, userType.ToString()))
            {
                return Failed(ErrorCode.SystemException, "今日已签到");
            }

            var dictionaryBiz = new DictionaryBiz();

            var model = new ScoreModel
            {
                ScoreGuid = Guid.NewGuid().ToString("N"),
                Variation = 10,
                Reason = "签到积分",
                UserGuid = UserID,
                UserTypeGuid = userType.ToString(),
                OrgGuid = "",
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };
            return scoreBiz.InsertScore(model) ? Success() : Failed(ErrorCode.DataBaseError, "签到积分插入错误");
        }
        /// <summary>
        /// 用户搜索历史
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<IEnumerable<string>>))]
        public IActionResult GetSearchHistory()
        {
            var response = new CommonBiz().GetUserSearchHistory(UserID);
            return Success(response);
        }
        /// <summary>
        /// 清除搜索历史
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RemoveSearchHistoryAsync()
        {
            await new CommonBiz().RemoveSearchHistoryAsync(UserID);
            return Success();
        }
        /// <summary>
        /// 获取热搜
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<IEnumerable<string>>)), AllowAnonymous]
        public IActionResult GetHotSearchHistory()
        {
            var response = new CommonBiz().GetHotSearchHistory();
            return Success(response);
        }

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeNickName(string nickName)
        {
            var userBiz = new UserBiz();
            var userModel = await userBiz.GetModelAsync(UserID);
            userModel.NickName = nickName;
            userModel.LastUpdatedBy = UserID;
            userModel.LastUpdatedDate = DateTime.Now;
            var result = await userBiz.UpdateAsync(userModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改用户昵称错误");
        }

        /// <summary>
        /// 修改真实姓名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeUserName(string userName)
        {
            var userBiz = new UserBiz();
            var userModel = await userBiz.GetModelAsync(UserID);
            userModel.UserName = userName;
            userModel.LastUpdatedBy = UserID;
            userModel.LastUpdatedDate = DateTime.Now;
            var result = await userBiz.UpdateAsync(userModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改用户姓名错误");
        }

        /// <summary>
        /// 修改性别
        /// </summary>
        /// <param name="gender">性别 F/M</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeUserGender(string gender)
        {
            var userBiz = new UserBiz();
            var userModel = await userBiz.GetModelAsync(UserID);
            userModel.Gender = gender;
            userModel.LastUpdatedBy = UserID;
            userModel.LastUpdatedDate = DateTime.Now;
            var result = await userBiz.UpdateAsync(userModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改用户性别错误");
        }

        /// <summary>
        /// 修改生日
        /// </summary>
        /// <param name="birthday">生日</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeUserBirthday(DateTime birthday)
        {
            var userBiz = new UserBiz();
            var userModel = await userBiz.GetModelAsync(UserID);
            userModel.Birthday = birthday;
            userModel.LastUpdatedBy = UserID;
            userModel.LastUpdatedDate = DateTime.Now;
            var result = await userBiz.UpdateAsync(userModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改用生日别错误");
        }

        /// <summary>
        /// 修改身份证号
        /// </summary>
        /// <param name="identityNo">用户身份证号</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeUserIdentityNumber(string identityNo)
        {
            var userBiz = new UserBiz();
            var userModel = await userBiz.GetModelAsync(UserID);
            userModel.IdentityNumber = identityNo;
            var result = await userBiz.UpdateAsync(userModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改用户身份证号错误");
        }

        /// <summary>
        /// 验证身份（真实姓名+身份证号）
        /// </summary>
        /// <param name="realName"></param>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto)), AllowAnonymous]
        public async Task<IActionResult> CheckYourself(string realName, string identityNumber)
        {
            var userModel = await new UserBiz().GetModelAsync(UserID);
            return Success(userModel.UserName == realName && userModel.IdentityNumber == identityNumber);
        }

        /// <summary>
        /// 检测手机号是否已注册用户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<bool>)), AllowAnonymous]
        public IActionResult CheckExistUser(string phone)
        {
            var model = new UserBiz().GetUserByPhone(phone);
            return Success(model != null, null);
        }

        /// <summary>
        /// 获取云医用户端公众号二维码（有效期29天，到期后需要重新刷新二维码）
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<CreateQRCodeResponseDto>))]
        public async Task<IActionResult> GetUserWeChatQRCodeAsync(string userType)
        {
            var tokenRes = await WeChartApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret);
            if (tokenRes.Errcode != 0)
            {
                Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{nameof(UserController)}.{nameof(GetUserWeChatQRCodeAsync)}({UserID},{userType})-- 获取云医用户端token失败  {Environment.NewLine} {tokenRes.Errmsg}");
                return Failed(ErrorCode.SystemException, "获取云医用户端token失败");
            }

            var sceneBiz = new WechatSceneBiz();

            var scene = new WechatSceneModel()
            {
                SceneId = Guid.NewGuid().ToString("N"),
                Action = WeChatSceneActionEnum.share.ToString(),
                SceneName = "分享关注",
                Extension = JsonConvert.SerializeObject(new
                {
                    value = UserID,
                    entrance = UserType
                }),
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };

            var result = await sceneBiz.InsertAsync(scene);

            if (!result) {return Failed(ErrorCode.Empty, "生成二维码场景参数失败，请稍后重试"); }

            var param = new CreateTemporaryQRCodeRequestDto
            {
                ExpireSeconds = 2592000,//30天
                ActionName = ActionNameEnum.QR_STR_SCENE.ToString(),//临时的字符串参数值
                ActionInfo = new QRCodeActionInfo
                {
                    Scene = new QRCodeActionInfoStringScene
                    {
                        SceneStr = scene.SceneId
                    }
                }
            };
            var qrcodeRes = await WeChartApi.CreateTemporaryQRCodeAsync(param, tokenRes.AccessToken);
            qrcodeRes.Deadline = DateTime.Now.AddSeconds(2505600);//29天
            return Success(qrcodeRes);
        }

        /// <summary>
        /// 通过用户id集合获取用户信息列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetUsersInfoResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetUsersInfoAsync([FromBody]GetUsersInfoRequestDto requestDto)
        {
            var result = await new UserBiz().GetUsersInfoAsync(requestDto);
            return Success(result);
        }
    }
}
