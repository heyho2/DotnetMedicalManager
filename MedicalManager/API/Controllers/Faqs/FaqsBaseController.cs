using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Faqs
{
    /// <summary>
    /// 医生模块控制器基类
    /// </summary>
    [Route("faqs/[controller]/[action]")]
    public abstract class FaqsBaseController : BaseController
    {
    }
}