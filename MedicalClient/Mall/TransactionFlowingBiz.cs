using GD.DataAccess;
using GD.Models.Mall;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using GD.Models.Merchant;
using GD.Models.FAQs;

namespace GD.Mall
{
    /// <summary>
    /// 交易流水业务处理类
    /// </summary>
    public class TransactionFlowingBiz
    {
        /// <summary>
        /// 根据id获取列表
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        public async Task<TransactionFlowingModel> GetModelsById(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<TransactionFlowingModel>("select * from t_mall_transaction_flowing where transaction_flowing_guid = @id", new { id });
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        public async Task<List<TransactionFlowingModel>> GetModels()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<TransactionFlowingModel>(" where  pay_account!='' and transaction_status='Success'", new { })).ToList();
            }
        }
        /// <summary>
        /// 通过订单guid集合获取交易流水记录
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="transactionStatus"></param>
        /// <returns></returns>
        public async Task<List<TransactionFlowingModel>> GetModelsByOrderGuidsAsync(string[] ids, string transactionStatus = "Success")
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            distinct b.* 
                            FROM
	                            t_mall_order a
	                            INNER JOIN t_mall_transaction_flowing b ON a.transaction_flowing_guid = b.transaction_flowing_guid 
                            WHERE
	                            a.order_guid IN @OrderGuids
	                            AND b.transaction_status = @transactionStatus";
                var result = await conn.QueryAsync<TransactionFlowingModel>(sql, new { OrderGuids = ids, transactionStatus });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 根据id获取列表
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        public async Task<List<TransactionFlowingModel>> GetModelsByIds(string[] ids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<TransactionFlowingModel>("where transaction_flowing_guid in @ids", new { ids }))?.ToList();
            }
        }

        /// <summary>
        /// 根据商品订单号查询流水信息
        /// </summary>
        /// <param name="outTradeNo">商品订单号</param>
        /// <returns></returns>
        public async Task<TransactionFlowingModel> GetModelByOutTradeNo(string outTradeNo)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryFirstOrDefaultAsync<TransactionFlowingModel>("select * from t_mall_transaction_flowing where out_trade_no=@out_trade_no", new { out_trade_no = outTradeNo }));
            }
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(List<TransactionFlowingModel> models)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                foreach (var item in models)
                {
                    await conn.UpdateAsync(item);
                }
                return true;
            });
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<string> AddModelAsync(TransactionFlowingModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.InsertAsync<string, TransactionFlowingModel>(model);
            };
        }
        /// <summary>
        /// 更新流水与问答
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> UpdateFlowingAndQuestionAsync(TransactionFlowingModel model, FaqsQuestionModel fAQsQuestionModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, TransactionFlowingModel>(model))) { return false; }
                fAQsQuestionModel.TransferFlowingGuid = model.TransactionFlowingGuid;
                if (await conn.UpdateAsync(fAQsQuestionModel) < 1) { return false; }
                return true;
            });
        }
        /// <summary>
        /// 新增流水数据
        /// </summary>
        /// <param name="model">交易流水model</param>
        /// <param name="mfmodel">商户流水model</param>
        /// <param name="orders">订单model</param>
        /// <returns></returns>
        public async Task<bool> SaveTransactionFlowing(TransactionFlowingModel model, List<MerchantFlowingModel> mfmodel, List<OrderModel> orders)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (model != null)
                {
                    model.LastUpdatedDate = DateTime.Now;
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, TransactionFlowingModel>(model))) { return false; }

                }
                if (mfmodel != null)
                {
                    foreach (var item in mfmodel)
                    {
                        item.LastUpdatedDate = DateTime.Now;
                        if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, MerchantFlowingModel>(item))) { return false; }
                    }
                }
                if (orders != null)
                {
                    foreach (var item in orders)
                    {
                        item.LastUpdatedDate = DateTime.Now;
                        ;
                        if (await conn.UpdateAsync(item) < 1) { return false; }
                    }
                }
                return true;
            });
        }
        /// <summary>
        /// 更新流水数据
        /// </summary>
        /// <param name="model">交易流水model</param>
        /// <param name="mfmodel">商户流水model</param>
        /// <param name="orders">订单model</param>
        /// <returns></returns>
        public async Task<bool> UpdateTransactionFlowing(TransactionFlowingModel model, List<MerchantFlowingModel> mfmodel, List<OrderModel> orders)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (model != null)
                {
                    model.LastUpdatedDate = DateTime.Now;
                    if (await conn.UpdateAsync(model) < 1) { return false; }

                }
                if (mfmodel != null)
                {
                    foreach (var item in mfmodel)
                    {
                        item.LastUpdatedDate = DateTime.Now;
                        if (await conn.UpdateAsync(item) < 1) { return false; }
                    }
                }
                if (orders != null)
                {
                    foreach (var item in orders)
                    {
                        item.LastUpdatedDate = DateTime.Now;
                        ;
                        if (await conn.UpdateAsync(item) < 1) { return false; }
                    }
                }
                return true;
            });
        }
    }
}
