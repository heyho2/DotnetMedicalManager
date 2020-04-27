using Dapper;
using GD.DataAccess;
using GD.Dtos.Merchant.Merchant;
using GD.Models.Merchant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Merchant
{
    /// <summary>
    /// 商户经营范围业务类
    /// </summary>
    public class ScopeBiz
    {
        #region 查询
        /// <summary>
        /// 通过商户经营范围Guid获取唯一经营范围实例
        /// </summary>
        /// <param name="scopeGuid">商户经营范围Guid</param>
        /// <returns>唯一经营范围实例</returns>
        public ScopeModel GetScopeModel(string scopeGuid, bool enable = true)
        {
            var sql = "select * from t_merchant_scope where scope_guid=@scopeGuid and enable=@enable";
            var scopeModel = MySqlHelper.SelectFirst<ScopeModel>(sql, new { scopeGuid, enable });
            return scopeModel;
        }
        /// <summary>
        /// 获取商户经营范围详情
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        public async Task<List<GetMerchantScopeDetailResponseDto>> GetMerchantScopeDetailAsync(string merchantGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.scope_dic_guid AS DicGuid,
	                            b.config_name AS ConfigName,
	                            a.picture_guid AS PictureGuid,
	                            CONCAT( c.base_path, c.relative_path ) AS PictureUrl 
                            FROM
	                            t_merchant_scope a
	                            LEFT JOIN t_manager_dictionary b ON a.scope_dic_guid = b.dic_guid 
	                            AND b.`enable` = 1
	                            LEFT JOIN t_utility_accessory c ON a.picture_guid = c.accessory_guid 
	                            AND c.`enable` = 1 
                            WHERE
	                            a.merchant_guid = @merchantGuid 
	                            AND a.`enable` = 1";
                var result = await conn.QueryAsync<GetMerchantScopeDetailResponseDto>(sql, new { merchantGuid });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取商户经营范围
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="enalbe"></param>
        /// <returns></returns>
        public async Task<List<ScopeModel>> GetScopeModelsByMerchantGuidAsync(string merchantGuid,bool enalbe=true)
        {
            using (var conn=MySqlHelper.GetConnection())
            {
                var models= await conn.GetListAsync<ScopeModel>("where merchant_guid = @merchantGuid and `enable`=@enalbe ", new { merchantGuid, enalbe });
                return models?.ToList();
            }
        }

        #endregion

        #region 修改
        /// <summary>
        /// 批量插入商户经营范围记录（事物执行）
        /// </summary>
        /// <param name="scopes">商户经营范围List集合</param>
        /// <returns>执行是否成功</returns>
        public bool InsertScope(List<ScopeModel> scopes)
        {
            if (!scopes.Any())
            {
                return true;
            }
            return MySqlHelper.Transaction((conn, tran) =>
            {
                foreach (var scope in scopes)
                {
                    if (string.IsNullOrEmpty(scope.Insert(conn)))//有记录未执行成功则回滚退出
                    {
                        return false;
                    }
                }
                return true;
            });
        }
        #endregion
    }
}
