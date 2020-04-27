using System;
using GD.AppSettings;
using GD.Common.Base;
using GD.Communication.XMPP;
using GD.Manager.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 测试服务
    /// </summary>
    public class TestController : BaseController
    {
        /// <summary>
        /// xmpp Client
        /// </summary>
        private static readonly Client xmppClient;

        static TestController()
        {
            var settings = Factory.GetSettings("host.json");
            var xmppAccount = settings["XMPP:operationAccount"];
            var xmppPassword = settings["XMPP:operationPassword"];
            xmppClient = new Client(xmppAccount, xmppPassword, nameof(AccountController));
            xmppClient.ConnectAsync();
        }


        /// <summary>
        /// 测试数据库服务
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, NoTiming]
        public ActionResult Db()
        {
            if (TestBiz.Crud())
            {
                return Ok("db is running");
            }
            else
            {
                return Ok("db is error");
            }
        }

        /// <summary>
        /// XMPP服务测试
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, NoTiming, ApiExplorerSettings(IgnoreApi = false)]
        public ActionResult Xmpp()
        {
            var useid = Guid.NewGuid().ToString("N");
            var response = xmppClient.CreateUserAsync(useid, "123456", "test");
            response.Wait();

            if (response.IsFaulted)
            {
                return Ok("create xmpp user account failed");
            }
            var testXmppClient = new Client(useid, "123456", "test");
            testXmppClient.ConnectAsync().Wait();

            response = testXmppClient.DeleteUserAsync();
            response.Wait();

            if (!response.Result)
            {
                return Ok("delete xmpp user account failed");
            }

            return Ok("xmpp server running");
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        [HttpPost, NoTiming]
        public ActionResult Upload(IFormFile fileData)
        {
            return Ok($"recieved file {fileData.FileName}");
        }
    }
}
