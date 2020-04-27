using GD.Common;
using GD.Dtos.Common;
using GD.Manager.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    ///<summary>
    ///执行sql
    ///</summary>
    public class ExecuteSqlController : UtilityBaseController
    {
        ///// <summary>
        ///// 执行查询sql
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost, Produces(typeof(ResponseDto<SqlPageResponseDto>))]
        //public async Task<IActionResult> QueryAsync([FromBody]SqlPageRequestDto request)
        //{
        //    ExecuteSqlBiz executeSqlBiz = new ExecuteSqlBiz();
        //    try
        //    {
        //        string regex = "DROP";
        //        if (Regex.IsMatch(request.Sql, regex, RegexOptions.IgnoreCase))
        //        {
        //            return Failed(message: "禁止DROP");
        //        }
        //        var data = await executeSqlBiz.QueryAsync(request);
        //        List<string> vs = new List<string>();
        //        if (data.CurrentPage.Count() > 0)
        //        {
        //            foreach (var property in (IDictionary<string, object>)data.CurrentPage.FirstOrDefault())
        //            {
        //                vs.Add(property.Key);
        //            }
        //        }
        //        data.Columns = vs.ToArray();

        //        return Success(data);
        //    }
        //    catch (global::System.Exception ex)
        //    {
        //        return Failed(message: ex.Message);
        //    }
        //}
    }
}