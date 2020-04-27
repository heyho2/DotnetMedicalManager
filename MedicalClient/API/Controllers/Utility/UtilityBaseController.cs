using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 通用功能模块控制器基类
    /// </summary>
    [Route("utility/[controller]/[action]")]
    public abstract class UtilityBaseController : BaseController
    {
    }
}