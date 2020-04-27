using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Decoration
{
    /// <summary>
    /// 装修管理控制器基类
    /// </summary>
    [Route("decoration/[controller]/[action]")]
    public abstract class DecorationBaseController : BaseController
    {
    }
}
