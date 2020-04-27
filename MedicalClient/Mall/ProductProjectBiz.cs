using GD.DataAccess;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

namespace GD.Mall
{
    public class ProductProjectBiz
    {
        /// <summary>
        /// 创建商品包含的服务项目关系
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> CreateProductProjectRelation(List<ProductModel> productModels, List<ProductProjectModel> models)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.DeleteListAsync<ProductProjectModel>("where product_guid in @productGuids", new { productGuids = productModels.Select(a => a.ProductGuid).ToList() });

                foreach (var item in productModels)
                {
                    await conn.UpdateAsync(item);
                }

                foreach (var item in models)
                {
                    await conn.InsertAsync<string, ProductProjectModel>(item);
                }
                return true;
            });

        }

        /// <summary>
        /// 通过商品guid获取Models
        /// </summary>
        /// <param name="productGuid">商品guid</param>
        /// <returns></returns>
        public async Task<List<ProductProjectModel>> GetModelsByProductGuidAsync(string productGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<ProductProjectModel>("where product_guid=@productGuid and `enable`=1", new { productGuid }))?.ToList();
            }
        }


    }
}
