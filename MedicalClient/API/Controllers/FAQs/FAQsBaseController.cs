using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.FAQs
{
    /// <summary>
    /// 问答模块控制器基类
    /// </summary>
    [Route("faqs/[controller]/[action]")]
    public abstract class FAQsBaseController : BaseController
    {
    }
}
