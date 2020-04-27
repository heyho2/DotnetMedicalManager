using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Doctor
{
    /// <summary>
    /// 医生模块控制器基类
    /// </summary>
    [Route("doctor/[controller]/[action]")]
    public abstract class DoctorBaseController : BaseController
    {
    }
}