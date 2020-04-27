using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GD.DataAccess;
using Dapper;
using GD.Models.Decoration;

namespace GD.Decoration
{
    public class DecorationBiz : BizBase.BaseBiz<DecorationModel>
    {
        /// <summary>
        /// 获取装修记录json内容
        /// </summary>
        /// <param name="decorationGuid"></param>
        /// <returns></returns>
        public async Task<string> GetDecorationContentAsync(string decorationGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<string>("select JSON_EXTRACT(content,'$') as content from t_decoration where decoration_guid=@decorationGuid", new { decorationGuid });
                return result;
            }
        }
    }
}
