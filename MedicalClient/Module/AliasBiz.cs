using Dapper;
using GD.DataAccess;
using GD.Dtos.Utility.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Module
{
    /// <summary>
    /// 别名业务类
    /// </summary>
    public class AliasBiz
    {
        /// <summary>
        /// 插入别名记录 ，若存在则删除再插入
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> InsertOrUpdateAsync(AddAliasRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"INSERT INTO t_utility_alias ( alias_guid, user_guid, target_guid, alias_name, created_by, last_updated_by )
                                VALUES
	                                ( REPLACE(UUID(),'-',''), @UserGuid, @TargetGuid,@AliasName,@UserGuid,@UserGuid ) 
	                                ON DUPLICATE KEY UPDATE alias_name = @AliasName,last_updated_date=NOW();";
                var res = await conn.ExecuteAsync(sql, requestDto);
                return res > 0;
            }
        }
    }
}
