using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Consultant
{
    /// <summary>
    /// 咨询师模块控制器基类
    /// </summary>
    [Route("consultant/[controller]/[action]")]
    public abstract class ConsultantBaseController : BaseController
    {
    }
}
