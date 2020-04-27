using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.API.Controllers.Payment
{
    /// <summary>
    /// 商家模块控制器基类
    /// </summary>
    [Route("payment/[controller]/[action]")]
    public class PaymentBaseController : BaseController
    {
    }
}
