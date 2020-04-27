using Dapper;
using GD.DataAccess;
using GD.Models.Manager;
using GD.Models.Merchant;
using GD.Models.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Fushion.CompositeBiz
{
    /// <summary>
    /// 商户组合业务类
    /// </summary>
    public class MerchantCompositeBiz
    {
        /// <summary>
        /// 注册商户
        /// </summary>
        /// <param name="merchantModel">商户model实例</param>
        /// <param name="scopes">经营范围model实例集合</param>
        /// <param name="certificates">证书项实例集合</param>
        /// <param name="accessories">附件实例集合</param>
        /// <returns>执行结果</returns>
        public async Task<bool> RegisterMerchant(MerchantModel merchantModel, List<ScopeModel> scopes, List<CertificateModel> certificates, List<AccessoryModel> accessories, UserModel userModel, bool isAdd = true)
        {
            if (merchantModel == null)
                return false;
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {

                if (isAdd)
                {
                    //商户信息
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, MerchantModel>(merchantModel)))
                    {
                        return false;
                    }
                }
                else
                {
                    //商户信息
                    if ((await conn.UpdateAsync(merchantModel)) != 1)
                    {
                        return false;
                    }
                    //删除原有的经营范围数据
                    await conn.ExecuteAsync("delete from t_merchant_scope where merchant_guid=@merchantGuid", new { merchantGuid = merchantModel.MerchantGuid });
                    //删除原有的证书项资料
                    var sql = @"DELETE a 
                                    FROM
	                                    t_utility_certificate a
	                                    INNER JOIN t_manager_dictionary b ON a.dic_guid = b.dic_guid 
                                    WHERE
	                                    a.owner_guid = @merchantGuid 
	                                    AND b.parent_guid = @merchantDicConfig
                                        AND a.`enable`=1 AND b.`enable`=1 ";
                    await conn.ExecuteAsync(sql, new { merchantGuid = merchantModel.MerchantGuid, merchantDicConfig = DictionaryType.MerchantDicConfig });
                }
                //用户信息
                if ((await conn.UpdateAsync(userModel)) != 1)
                {
                    return false;
                }

                //经营范围
                if (scopes.Any())
                    foreach (var scope in scopes)
                    {
                        if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ScopeModel>(scope)))
                        {
                            return false;
                        }
                    }
                //商户配置项证书信息 & 配置项证书附件信息
                if (certificates.Any() && accessories.Any())
                {
                    //附件
                    foreach (var accessory in accessories)
                    {
                        if ((await conn.UpdateAsync(accessory)) != 1)
                        {
                            return false;
                        }
                    }
                    //证书
                    foreach (var certificate in certificates)
                    {
                        if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, CertificateModel>(certificate)))
                        {
                            return false;
                        }
                    }
                }
                return true;
            });
        }
    }
}
