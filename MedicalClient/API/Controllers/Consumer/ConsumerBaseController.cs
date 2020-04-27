using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Consumer
{
    /// <summary>
    /// 消费者模块控制器基类
    /// </summary>
    [Route("consumer/[controller]/[action]")]
    public abstract class ConsumerBaseController : BaseController
    {
    }
}
