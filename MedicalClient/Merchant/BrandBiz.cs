using GD.DataAccess;
using GD.Models.Merchant;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

namespace GD.Merchant
{
    public class BrandBiz
    {
        /// <summary>
        /// 通过商铺Id获取品牌数据
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<List<BrandModel>> GetModelByMerchantIdAsync(string merchantId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<BrandModel>("where merchant_guid=@merchantId and `enable`=1", new { merchantId });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 异步获取唯一实例
        /// </summary>
        /// <param name="brandGuid">品牌guid</param>
        /// <returns></returns>
        public async Task<BrandModel> GetAsync(string brandGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<BrandModel>("select * from t_merchant_brand where brand_guid =@brandGuid and `enable`=1", new { brandGuid });
                return result;
            }
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(BrandModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, BrandModel>(model);
                return !string.IsNullOrEmpty(result);
            }
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(BrandModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }

        /// <summary>
        /// 移除品牌数据
        /// </summary>
        /// <param name="merchantGuid">商户guid</param>
        /// <param name="brandGuids">平台guid集合</param>
        /// <param name="isPhysicDelete">是否物理删除</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string merchantGuid, List<string> brandGuids, bool isPhysicDelete = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = true;
                if (isPhysicDelete)
                {
                    result = (await conn.DeleteListAsync<BrandModel>("where merchant_guid = @merchantGuid and brand_guid in @brandGuids", new { merchantGuid, brandGuids = brandGuids.ToArray() })) > 0;
                }
                else
                {
                    result = (await conn.ExecuteAsync("update t_merchant_brand set `enable`=0 where merchant_guid = @merchantGuid and brand_guid in @brandGuids", new { merchantGuid, brandGuids = brandGuids.ToArray() })) > 0;
                }
                return result;
            }
        }
    }
}
