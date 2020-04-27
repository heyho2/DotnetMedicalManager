using Dapper;
using GD.DataAccess;
using GD.Dtos.Consumer.Consumer;
using GD.Models.Consumer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 积分来源记录业务类
    /// </summary>
    public class ScoreRecordBiz
    {

        /// <summary>
        /// 分页获取用户的积分来源记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<ScoreRecordModel>> GetModelsByTargetUserId(string userId,string targetId, int pageIndex, int pageSize,string sourceType= "Distribution")
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                string sql = $"select * from t_consumer_score_record where user_guid=@userId and target_user_guid=@targetId and `enable`=1 and score_source_type=@sourceType order by creation_date desc limit @pageIndex ,@pageSize";
                var models = await conn.QueryAsync<ScoreRecordModel>(sql, new { userId, targetId, pageIndex = (pageIndex - 1)* pageSize, pageSize , sourceType });
                return models?.ToList();
            }
        }
        /// <summary>
        /// 获取积分来源详情
        /// </summary>
        /// <param name="recordId">积分来源记录Id</param>
        /// <returns></returns>
        public async Task<GetDistributionConsumptionDetailsResponseDto> GetDistributionConsumptionDetails(string recordId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                string sql = $@"SELECT
	                                o.order_no AS OrderNo,
	                                o.payment_date AS PaymentDate,
	                                o.product_count AS ProductCount,
	                                o.paid_amount AS PaidAmount,
	                                record.rate AS Rate,
	                                record.score AS Score 
                                FROM
	                                t_consumer_score_record record
	                                INNER JOIN t_mall_order o ON record.order_guid = o.order_guid 
                                WHERE
	                                record.score_record_guid =@recordId
	                                AND record.`enable` = 1 
	                                AND o.`enable` =1";
                return await conn.QueryFirstOrDefaultAsync<GetDistributionConsumptionDetailsResponseDto>(sql, new { recordId });
            }
        }

        
    }
}
