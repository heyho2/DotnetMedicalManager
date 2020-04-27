using Dapper;
using GD.Common.EnumDefine;
using GD.DataAccess;
using GD.Dtos.Help;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Manager.Utility
{
    /// <summary>
    /// 平台帮助业务类
    /// </summary>
    public class HelpBiz : BaseBiz<HelpModel>
    {
        /// <summary>
        /// 常见问题列表
        /// </summary>
        /// <returns></returns>
        public async Task<GetHelpPageResponseDto> GetHelpPageAsync(GetHelpPageRequestDto request)
        {
            var sqlWhere = $@"1 = 1";
            if (!string.IsNullOrWhiteSpace(request.Question))
            {
                sqlWhere = $"{sqlWhere} and question like @Question";
            }
            var sqlOrderBy = "sort desc";
            var sql = $@"
SELECT * FROM
    t_utility_help
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            request.Question = $"%{request.Question}%";
            return await MySqlHelper.QueryByPageAsync<GetHelpPageRequestDto, GetHelpPageResponseDto, GetHelpPageItemDto>(sql, request);
        }
    }
}
