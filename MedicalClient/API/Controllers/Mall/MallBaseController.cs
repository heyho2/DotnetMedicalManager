using GD.Common.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Mall
{
    /// <summary>
    /// 商城模块控制器基类
    /// </summary>
    [Route("mall/[controller]/[action]"), AllowAnonymous]
    public abstract class MallBaseController : BaseController
    {
    }
}