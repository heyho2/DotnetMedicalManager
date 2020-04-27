using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.API.Controllers.Meal
{
    /// <summary>
    /// 点餐相关接口控制器基类
    /// </summary>
    [Route("meal/[controller]/[action]")]
    public abstract class MealBaseController : BaseController
    {
    }
}
