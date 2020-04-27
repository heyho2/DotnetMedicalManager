using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Expert
{
    /// <summary>
    /// 医疗专家模块控制器基类
    /// </summary>
    [Route("expert/[controller]/[action]")]
    public abstract class ExpertBaseController : BaseController
    {
    }
}