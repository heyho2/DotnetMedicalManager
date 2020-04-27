using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Merchant
{
    /// <summary>
    /// 商家模块控制器基类
    /// </summary>
    [Route("merchantbase/[controller]/[action]")]
    public abstract class MerchantBaseController : BaseController
    {
    }
}
