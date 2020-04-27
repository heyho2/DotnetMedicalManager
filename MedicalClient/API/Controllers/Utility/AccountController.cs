using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GD.AppSettings;
using GD.Common;
using GD.Common.Base;
using GD.Common.EnumDefine;
using GD.Common.Helper;
using GD.Communication.SM;
using GD.Dtos.Utility.Account;
using GD.Models.Consumer;
using GD.Models.CrossTable;
using GD.Models.Utility;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using GD.Communication.XMPP;
using GD.Consumer;
using GD.Dtos.Doctor.Doctor;
using GD.Module.WeChat;
using GD.Doctor;
using GD.Manager;
using Newtonsoft.Json;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 账号控制器，提供登录，用机验证码等功能
    /// </summary>
    public class AccountController : BaseController
    {
        #region 变量定义区
        /// <summary>
        /// xmpp Client
        /// </summary>
        private static readonly Client xmppClient;

        /// <summary>
        /// 手机验证码有效期分钟数
        /// 默认为1分钟
        /// </summary>
        private static readonly int VerificationExpires = 1;

        /// <summary>
        /// 是否启用XMPP注册
        /// </summary>
        private static readonly bool enableXmpp = false;

        /// <summary>
        /// 提供给前端连接OpenFire服务器的连接
        /// </summary>
        private static readonly string httpBind;

        /// <summary>
        /// 提供给前端连接OpenFire服务器的域名
        /// </summary>
        private static readonly string domain;

        /// <summary>
        /// 验证码短信模板ID
        /// </summary>
        private static readonly int VerificationTemplate;

        /// <summary>
        /// 用户端微信公众号ID
        /// </summary>
        private static readonly string weChatClientAppId;

        /// <summary>
        /// 用户端微信公众号密钥
        /// </summary>
        private static readonly string weChatClientSecret;

        /// <summary>
        /// RabbitMQ连接串，JS使用
        /// </summary>
        private static readonly string rabbitMQws;

        #endregion

        static AccountController()
        {
            var settings = Factory.GetSettings("host.json");

            if (int.TryParse(settings["VerificationExpires"], out int verification) && verification >= 1)
            {
                VerificationExpires = verification;
            }

            if (!bool.TryParse(settings["XMPP:enable"], out enableXmpp))
            {
                enableXmpp = false;
            }

            if (!int.TryParse(settings["TencentSMS:VerificationTemplate"], out VerificationTemplate) || VerificationTemplate <= 0)
            {
                Logger.Error("illegal verification template");
                Environment.Exit(1);
            }

            weChatClientAppId = settings["WeChat:Client:AppId"];
            if (string.IsNullOrEmpty(weChatClientAppId))
            {
                Logger.Warn("未配置用户端微信公众号ID");
            }

            weChatClientSecret = settings["WeChat:Client:AppSecret"];
            if (string.IsNullOrEmpty(weChatClientSecret))
            {
                Logger.Warn("未配置用户端微信公众号密钥");
            }

            rabbitMQws = settings["ConnectionString:RabbitMQws"];
            if (string.IsNullOrEmpty(rabbitMQws))
            {
                Logger.Warn("未配置RabbitMQ的WebSocket连接串");
            }

            // 不启用XMPP，则XMPP相关初始化就不需要
            if (!enableXmpp)
            {
                return;
            }

            httpBind = settings["XMPP:httpBind"];

            if (string.IsNullOrEmpty(httpBind))
            {
                Logger.Error("illegal xmpp httpBind");
                Environment.Exit(1);
            }

            domain = settings["XMPP:domain"];

            if (string.IsNullOrEmpty(domain))
            {
                Logger.Error("illegal xmpp domain");
                Environment.Exit(1);
            }
            var xmppAccount = settings["XMPP:operationAccount"];
            var xmppPassword = settings["XMPP:operationPassword"];
            xmppClient = new Client(xmppAccount, xmppPassword, $"{nameof(AccountController)}#@@@#{Guid.NewGuid().ToString("N")}");
            xmppClient.ConnectAsync();
        }

        /// <summary>
        /// 判断电话号码是否已经注册
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public IActionResult AccoutExists(string phone)
        {
            var accountBiz = new AccountBiz();
            var list = accountBiz.GetUserByPhone(phone);

            return Success(list.Any());
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(ResponseDto))]
        public IActionResult Register([FromBody]PhonePasswordCodeRequestDto request)
        {
            var accountBiz = new AccountBiz();
            if (!accountBiz.VerifyCode(request.Phone, request.Code))
            {
                return Failed(ErrorCode.VerificationCode, "手机验证码错误");
            }

            var userID = Guid.NewGuid().ToString("N");
            var saltPassword = CryptoHelper.AddSalt(userID, request.Password);
            if (string.IsNullOrEmpty(saltPassword))
            {
                return Failed(ErrorCode.SystemException, "密码加盐失败");
            }

            var biz = new AccountBiz();
            var list = biz.GetUserByPhone(request.Phone);
            if (list.Any())
            {
                return Failed(ErrorCode.DuplicatePhone, "该手机号已经注册");
            }
            #region 获取用户是否有推荐关注公众号记录，若有，则将推荐人设为平台账户推荐人
            var recommendUser = TryGetSubscriptionRecommendUser(request.OpenId);
            if (!string.IsNullOrWhiteSpace(recommendUser))
            {
                request.Referrer = recommendUser;
            }
            #endregion
            var userModel = new UserModel
            {
                UserGuid = userID,
                WechatOpenid = request.OpenId,
                NickName = userID.Substring(0, 6),
                UserName = userID.Substring(0, 6),
                Phone = request.Phone,
                Password = saltPassword,
                Birthday = new DateTime(2000, 1, 1),
                RecommendGuid = request.Referrer,
                CreatedBy = userID,
                LastUpdatedBy = userID,
                OrgGuid = "guodan"
            };

            var consumerModel = new ConsumerModel
            {
                ConsumerGuid = userID,
                CreatedBy = userID,
                LastUpdatedBy = userID
            };

            var registerModel = new RegisterModel
            {
                PlatformType = request.PlatformType,
                Parameters = request.Parameters
            };

            var result = biz.Register(userModel, consumerModel, registerModel);

            if (result == null)
            {
                return Failed(ErrorCode.DuplicatePhone);
            }

            if (result.Value)
            {
                var message = string.Empty;
                if (enableXmpp && !RegisterIM(userModel)) // 启用XMPP的情况下，才执行注册
                {
                    message = $"register im account failed. user id: {userID}, user phone: {request.Phone}";
                    Logger.Error(message);
                }

                var scoreBiz = new ScoreRulesBiz();
                scoreBiz.AddScoreByRules(userID, ActionEnum.Registered, UserType.Consumer);

                if (!string.IsNullOrEmpty(request.Referrer))
                {
                    scoreBiz.AddScoreByRules(request.Referrer, ActionEnum.RecommendRegistered, UserType.Doctor);
                    scoreBiz.AddScoreByRules(request.Referrer, ActionEnum.RecommendRegistered, UserType.Consumer);
                }

                return Success(userID, message);
            }
            else
            {
                return Failed(ErrorCode.DataBaseError);
            }
        }

        /// <summary>
        /// 尝试获取用户关注公众号的推荐人
        /// </summary>
        /// <returns></returns>
        private string TryGetSubscriptionRecommendUser(string openId)
        {
            if (string.IsNullOrWhiteSpace(openId))
            {
                return string.Empty;
            }
            var model = new WechatSubscriptionBiz().GetLatestRecommendRecordAsync(openId).Result;
            return model?.RecommendUserGuid;
        }

        /// <summary>
        /// 生成手机验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(ResponseDto))]
        public IActionResult CreateVerificationCode(string phone)
        {
            var attr = new PhoneAttribute();
            if (!attr.IsValid(phone))
            {
                return Failed(ErrorCode.FormatError, "invalid phone number");
            }

            var biz = new AccountBiz();
            var code = biz.CreateVerificationCode(phone, VerificationExpires);

            var parameters = new[] { code.ToString(), VerificationExpires.ToString() };
            var success = SMHelper.Send(phone, parameters, VerificationTemplate); // 此处的模板ID和参数需要与腾讯上的短信模板匹配

            if (!success)
            {
                return Failed(ErrorCode.Unknown, "验证码发送失败");
            }

            var dto = new CreateVerificationCodeResponseDto
            {
                // Code = code,
                Minutes = VerificationExpires
            };

            return Success(dto);
        }

        /// <summary>
        /// 校验手机验证码有效性
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(ResponseDto))]
        public IActionResult VerifyCode([FromBody]VerifyCodeRequestDto request)
        {
            var biz = new AccountBiz();
            if (biz.VerifyCode(request.Phone, request.Code))
            {
                return Success();
            }
            else
            {
                return Failed(ErrorCode.VerificationCode);
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(ResponseDto))]
        public IActionResult Login([FromBody]LoginRequestDto request)
        {
            var biz = new AccountBiz();
            var query = biz.GetUserByPhone(request.Phone);

            var model = query.FirstOrDefault(m => string.Equals(m.Password, CryptoHelper.AddSalt(m.UserGuid, request.Password), StringComparison.OrdinalIgnoreCase));
            if (model == null)
            {
                return Failed(ErrorCode.InvalidIdPassword);
            }

            // 启用XMPP的情况下，就检查用户IM账号是否存在
            if (enableXmpp)
            {
                var status = Client.QueryStatusAsync(model.UserGuid);
                status.Wait();

                // 如果不存在，则注册该用户的IM账号
                if (status.Result == IMStatus.NotExist)
                {
                    RegisterIM(model);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.OpenId) && request.OpenId != model.WechatOpenid)
            {
                model.WechatOpenid = request.OpenId;
                model.LastUpdatedDate = DateTime.Now;
                var upRes = biz.UpdateUser(model);
                Logger.Debug($"用户登录时，更新用户({model.UserGuid}) openid 结果:请求参数{JsonConvert.SerializeObject(request)}  更新结果-{upRes.ToString()}");
            }

            var scoreBiz = new ScoreRulesBiz();
            scoreBiz.AddScoreByRules(model.UserGuid, ActionEnum.Login, request.UserType);

            var response = new LoginResponseDto
            {
                UserId = model.UserGuid,
                NickName = model.NickName,
                Token = CreateToken(model.UserGuid, request.UserType, request.Days > 0 ? request.Days : 999),
                Xmpp = httpBind,
                Domain = domain,
                RabbitMQ = rabbitMQws
            };

            return Success(response);
        }
        /// <summary>
        /// 医生端企业微信授权登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(DoctorLoginResponseDto))]
        public async Task<IActionResult> EnterpriseWeChatDoctorLogin([FromBody]DoctorLoginRequestDto request)
        {
            string typeSecret = string.Empty;
            if (request.Type == 0)
            {
                typeSecret = PlatformSettings.EnterpriseWeChatMobileSecret;
            }
            else
            {
                typeSecret = PlatformSettings.EnterpriseWeChatPCAgentid;
            }
            DoctorLoginResponseDto response = null;
            var result = await GetEnterpriseWeChatUserInfo(request.Code, typeSecret);
            if (result == null)
            {
                return Failed(ErrorCode.Empty, "未找到对应用户信息");
            }
            if (result.extattr == null || result.extattr.attrs == null || result.extattr.attrs.Length == 0)
            {
                return Failed(ErrorCode.Empty, "未找到对应用户工号信息");
            }
            var jobNumber = result.extattr.attrs.FirstOrDefault(a => a.name == "工号");
            if (jobNumber == null || string.IsNullOrEmpty(jobNumber.value))
            {
                return Failed(ErrorCode.Empty, "用户工号信息为空");
            }
            //根据工号找到对应的医生
            var doctor = await new DoctorBiz().GetByJobNumberDoctor(jobNumber.value);
            if (doctor == null)
            {
                return Failed(ErrorCode.Empty, "未找到对应工号的医生");
            }
            var biz = new AccountBiz();
            var model = biz.GetUserById(doctor.DoctorGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "未找到对应的医生");
            }
            //更新医生表企业微信用户Id
            if (doctor.WechatOpenid != result.userid)
            {
                doctor.WechatOpenid = result.userid;
                doctor.LastUpdatedBy = UserID;
                doctor.LastUpdatedDate = DateTime.Now;
                var resultUpdate = await new DoctorBiz().UpdateAsync(doctor);
            }
            // 启用XMPP的情况下，就检查用户IM账号是否存在
            if (enableXmpp)
            {
                var status = Client.QueryStatusAsync(model.UserGuid);
                status.Wait();

                // 如果不存在，则注册该用户的IM账号
                if (status.Result == IMStatus.NotExist)
                {
                    RegisterIM(model);
                }
            }

            var scoreBiz = new ScoreRulesBiz();
            await scoreBiz.AddScoreByRules(model.UserGuid, ActionEnum.Login, request.UserType);
            var resultDoctor = await new ReviewRecordBiz().GetLatestReviewRecordByTargetGuidAsync(doctor.DoctorGuid, Models.Manager.ReviewRecordModel.TypeEnum.Doctors.ToString());
            response = new DoctorLoginResponseDto
            {
                UserId = model.UserGuid,
                NickName = model.NickName,
                Token = CreateToken(model.UserGuid, request.UserType, request.Days > 0 ? request.Days : 999),
                Xmpp = httpBind,
                Domain = domain,
                RabbitMQ = rabbitMQws,
                RegisterState = doctor?.Status,
                WhetherRegister = true,
                ApprovalMessage = resultDoctor?.RejectReason
            };
            return Success(response);
        }
        private async Task<Dtos.Doctor.Hospital.UserDetail> GetEnterpriseWeChatUserInfo(string code, string secret)
        {
            Dtos.Doctor.Hospital.UserDetail enterpriseWeChatUserInfo = null;
            //1.获取（access_token）缓存两小时
            var aToken = await EnterpriseWeChatApi.GetEnterpriseAccessToken(PlatformSettings.EnterpriseWeChatAppid, secret);
            if (aToken == null)
            {
                return enterpriseWeChatUserInfo;
            }
            //2.根据code查找用户信息数据
            EnterpriseWeChatApi bill = new EnterpriseWeChatApi();
            string getUserurl = $"https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={aToken.AccessToken}&code={code}";
            var userModel = await bill.Send<Dtos.Doctor.Hospital.UserModel>(getUserurl);
            if (userModel == null || string.IsNullOrEmpty(userModel.UserId))
            {
                return enterpriseWeChatUserInfo;
            }
            //3.根据用户Id查询用户信息
            string getUserDetailurl = $"https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={aToken.AccessToken}&userid={userModel.UserId}";
            var userDetail = await bill.Send<Dtos.Doctor.Hospital.UserDetail>(getUserDetailurl);
            if (userModel == null)
            {
                return enterpriseWeChatUserInfo;
            }
            enterpriseWeChatUserInfo = userDetail;
            return enterpriseWeChatUserInfo;
        }
        /// <summary>
        /// 根据微信授权码更新用户微信登录的 OpenId
        /// </summary>
        /// <param name="wxCode">微信授权码</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateOpenId(string wxCode)
        {
            if (string.IsNullOrEmpty(wxCode))
            {
                return Failed(ErrorCode.FormatError, "微信授权码为空");
            }

            var biz = new AccountBiz();
            var model = biz.GetUserById(UserID);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "用户不存在");
            }

            model.WechatOpenid = GetOpenId(wxCode);
            if (string.IsNullOrEmpty(model.WechatOpenid))
            {
                return Failed(ErrorCode.Empty, "获取用户OpenId为空");
            }

            return Success(biz.UpdateUser(model));
        }

        /// <summary>
        /// 登录管理后台
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(ResponseDto))]
        public IActionResult LoginAdmin([FromBody]LoginAdminRequestDto request)
        {
            var biz = new AccountBiz();
            var query = biz.GetAdministrator(request.Account);

            var model = query.FirstOrDefault(m => string.Equals(m.Password, CryptoHelper.AddSalt(m.UserGuid, request.Password), StringComparison.OrdinalIgnoreCase));
            if (model == null)
            {
                return Failed(ErrorCode.InvalidIdPassword);
            }

            var response = new LoginResponseDto
            {
                UserId = model.UserGuid,
                NickName = model.NickName,
                Token = CreateToken(model.UserGuid, UserType.Admin, request.Days > 0 ? request.Days : 999)
            };

            return Success(response);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(ResponseDto))]
        public IActionResult ResetPassword([FromBody]PhonePasswordCodeRequestDto dto)
        {
            var biz = new AccountBiz();
            if (!biz.VerifyCode(dto.Phone, dto.Code))
            {
                return Failed(ErrorCode.VerificationCode, "手机验证码错误！");
            }

            var model = biz.GetUserByPhone(dto.Phone).FirstOrDefault();
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "该手机号未注册");
            }

            model.LastUpdatedBy = model.UserGuid;
            model.Password = CryptoHelper.AddSalt(model.UserGuid, dto.Password);
            if (string.IsNullOrEmpty(model.Password))
            {
                return Failed(ErrorCode.SystemException, "密码加盐失败");
            }

            return biz.UpdateUser(model) ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败！");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult UpdatePassword(string password)
        {
            // 前端传输的密码为MD5加密后的结果
            if (string.IsNullOrEmpty(password) || password.Length != 32)
            {
                return Failed(ErrorCode.FormatError, "密码为空或者无效");
            }

            var biz = new AccountBiz();
            var userModel = biz.GetUserById(UserID);
            if (userModel == null)
            {
                return Failed(ErrorCode.Empty, "用户不存在或者已经注销");
            }

            userModel.Password = CryptoHelper.AddSalt(UserID, password);
            if (string.IsNullOrEmpty(userModel.Password))
            {
                return Failed(ErrorCode.SystemException, "密码加盐失败");
            }

            return biz.UpdateUser(userModel) ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败");
        }

        /// <summary>
        /// 修改电话号码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult UpdatePhone([FromBody]VerifyCodeRequestDto request)
        {
            var accountBiz = new AccountBiz();
            if (!accountBiz.VerifyCode(request.Phone, request.Code))
            {
                return Failed(ErrorCode.VerificationCode, "手机验证码错误");
            }

            var list = accountBiz.GetUserByPhone(request.Phone);
            if (list.Any())
            {
                return Failed(ErrorCode.DuplicatePhone, "手机号已经注册");
            }

            list = accountBiz.GetUserByPhone(request.Phone, false);
            if (list.Any())
            {
                return Failed(ErrorCode.DuplicatePhone, "手机号已经注册");
            }

            var biz = new AccountBiz();
            var userModel = biz.GetUserById(UserID);
            if (userModel == null)
            {
                return Failed(ErrorCode.Empty, "用户不存在或者已经注销");
            }

            userModel.Phone = request.Phone;

            return biz.UpdateUser(userModel) ? Success() : Failed(ErrorCode.DataBaseError, "修改电话号码失败");
        }

        /// <summary>
        /// 单点登录，验证Token有效性，供外部系统调用验证
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult SSO()
        {
            var biz = new UserBiz();
            if (biz.GetUser(UserID, true) == null)
            {
                return Failed(ErrorCode.Unauthorized, "用户不存在或者已经注销");
            }
            else
            {
                return Success<string>(UserID);
            }
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            // todo:

            return Success($"{UserID} logout success");
        }

        /// <summary>
        /// 查询用户IM在线状态
        /// </summary>
        /// <param name="userId">用户IM账号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IMStatusAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Failed(ErrorCode.FormatError, "用户IM不允许为空");
            }

            var status = await Client.QueryStatusAsync(userId);
            return Success(status, status.GetDescription());
        }

        /// <summary>
        /// 根据微信授权码，获取用户微信OpenId
        /// </summary>
        /// <param name="wxCode"></param>
        /// <returns></returns>
        private string GetOpenId(string wxCode)
        {
            try
            {
                var accessToken = OAuthApi.GetAccessToken(weChatClientAppId, weChatClientSecret, wxCode);
                if (accessToken.errcode == ReturnCode.请求成功)
                {
                    return accessToken.openid;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return string.Empty;
        }

        /// <summary>
        /// 注册IM账号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool RegisterIM(UserModel model)
        {
            var response = xmppClient.CreateUserAsync(model.UserGuid, model.UserGuid.Md5(), model.NickName);
            response.Wait();

            return !response.IsFaulted;
        }
    }
}