using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Manager
{
    /// <summary>
    /// 平台管理模块控制器基类
    /// </summary>
    [Route("manager/[controller]/[action]")]
    public abstract class ManagerBaseController : BaseController
    {
    }
}
