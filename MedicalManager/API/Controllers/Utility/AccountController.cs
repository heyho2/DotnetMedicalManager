using GD.AppSettings;
using GD.Common;
using GD.Common.Base;
using GD.Common.EnumDefine;
using GD.Common.Helper;
using GD.Dtos.Account;
using GD.Dtos.WeChat;
using GD.Manager.CommonUtility;
using GD.Manager.Utility;
using GD.Manager.WeChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 账号控制器，提供登录，用机验证码等功能
    /// </summary>
    public class AccountController : BaseController
    {
        #region 变量定义区

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
                Token = CreateToken(model.UserGuid, UserType.Admin, request.Days > 0 ? request.Days : 999),
                Xmpp = httpBind,
                Domain = domain,
                RabbitMQ = rabbitMQws
            };
            return Success(response);
        }
        /// <summary>
        /// 获取企业微信平台端配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Produces(typeof(ResponseDto<GetEnterpriseWeChatResponse>))]
        public IActionResult GetWeChatMobileConfig()
        {
            var response = new GetEnterpriseWeChatResponse
            {
                Appid = PlatformSettings.EnterpriseWeChatAppid,
                Agentid = PlatformSettings.EnterpriseWeChatPlatformAgentid
            };
            return Success(response);
        }
        /// <summary>
        /// 使用企业微信登录管理后台
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> EnterpriseWeChatLoginAdmin([FromBody]EnterpriseWeChatLoginAdminRequestDto request)
        {
            var result = await GetEnterpriseWeChatUserInfo(request.Code, PlatformSettings.EnterpriseWeChatPlatformSecret);
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
            var biz = new AccountBiz();
            var model = biz.GetAdministrator(jobNumber.value).FirstOrDefault();

            if (model == null)
            {
                return Failed(ErrorCode.Empty, "账号不存在");
            }
            var response = new LoginResponseDto
            {
                UserId = model.UserGuid,
                NickName = model.NickName,
                Token = CreateToken(model.UserGuid, UserType.Admin, request.Days > 0 ? request.Days : 999),
                Xmpp = httpBind,
                Domain = domain,
                RabbitMQ = rabbitMQws
            };
            return Success(response);
        }
        private async Task<UserDetail> GetEnterpriseWeChatUserInfo(string code, string secret)
        {
            UserDetail enterpriseWeChatUserInfo = null;
            //1.获取（access_token）缓存两小时
            var aToken = await EnterpriseWeChatApi.GetEnterpriseAccessToken(PlatformSettings.EnterpriseWeChatAppid, secret);
            if (aToken == null)
            {
                return enterpriseWeChatUserInfo;
            }
            //2.根据code查找用户信息数据
            EnterpriseWeChatApi bill = new EnterpriseWeChatApi();
            string getUserurl = $"https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={aToken.AccessToken}&code={code}";
            var userModel = await HttpClientHelper.HttpGetAsync<UserModel>(getUserurl);
            if (userModel == null || string.IsNullOrEmpty(userModel.UserId))
            {
                return enterpriseWeChatUserInfo;
            }
            //3.根据用户Id查询用户信息
            string getUserDetailurl = $"https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={aToken.AccessToken}&userid={userModel.UserId}";
            var userDetail = await HttpClientHelper.HttpGetAsync<UserDetail>(getUserDetailurl);
            if (userModel == null)
            {
                return enterpriseWeChatUserInfo;
            }
            enterpriseWeChatUserInfo = userDetail;
            return enterpriseWeChatUserInfo;
        }
        /// <summary>
        /// 单点登录，验证Token有效性，供外部系统调用验证
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult SSO()
        {
            return Success(UserID);
        }
    }
}