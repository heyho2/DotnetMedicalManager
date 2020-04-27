using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.API.Controllers.Report
{
    /// <summary>
    /// 问答模块控制器基类
    /// </summary>
    [Route("report/[controller]/[action]")]
    public abstract class ReportBaseController:BaseController
    {
    }
}
