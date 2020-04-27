using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 系统模块控制器基类
    /// </summary>
    [Route("System/[controller]/[action]")]
    public abstract class SystemBaseController : BaseController
    {

    }
}
