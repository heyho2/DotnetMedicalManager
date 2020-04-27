using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.API.Controllers.Health
{
    /// <summary>
    ///健康模块控制器基类
    /// </summary>
    [Route("health/[controller]/[action]")]
    public abstract class HealthBaseController : BaseController
    {

    }
}
