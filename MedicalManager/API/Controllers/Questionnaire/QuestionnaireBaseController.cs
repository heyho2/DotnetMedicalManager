using GD.Common.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.API.Controllers.Questionnaire
{
    /// <summary>
    /// 问卷控制器
    /// </summary>
    [Route("questionnairebase/[controller]/[action]")]
    public abstract class QuestionnaireBaseController : BaseController
    {
    }
}
