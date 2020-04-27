using GD.DataAccess;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace GD.Utility
{
    public class VisitBiz
    {
        /// <summary>
        /// 新增model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<bool> InsertAsync(VisitModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, VisitModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }

        }
    }
}
