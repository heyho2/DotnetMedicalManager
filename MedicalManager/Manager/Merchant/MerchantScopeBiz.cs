using Dapper;
using GD.DataAccess;
using GD.Models.Merchant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Merchant
{
    /// <summary>
    /// 商户经营范围业务类
    /// </summary>
    public class MerchantScopeBiz : BaseBiz<ScopeModel>
    {
        /// <summary>
        /// 判断商户是否使用经营范围
        /// </summary>
        /// <param name="scopeDicGuid"></param>
        /// <returns></returns>
        public async Task<bool> AnyMerchantScopeAsync(string scopeDicGuid)
        {
            var sql = @"select count(1) from t_merchant_scope a
left join t_merchant b on a.merchant_guid=b.merchant_guid
where a.scope_dic_guid=@scopeDicGuid and a.enable=1 and b.enable=1";
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryFirstOrDefaultAsync<int?>(sql, new { scopeDicGuid }) ?? 0) > 0;
            }
        }

        /// <summary>
        /// 判断商户是否使用经营范围
        /// </summary>
        /// <param name="scopeDicGuid"></param>
        /// <returns></returns>
        public async Task<bool> AnyMerchantScopeProductAsync(string merchantGuid, IEnumerable<string> dicGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<int?>(@"select count(1) from t_mall_product WHERE merchant_guid = @merchantGuid and category_guid in @dicGuids", new
                {
                    dicGuids,
                    merchantGuid
                });
                return (result ?? 0) > 0;

            }
        }
    }
}

